using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;

public class MyQuote
{
    public MyQuote()
    {
        this.QuoteLines = new List<MyQuoteLine>();
        this.BV2 = new BOXX_V2Entities();
        this.WebQuote = new VW_OMCWebQuote();
    }

    public string QuoteId { get; set; }
    public List<MyQuoteLine> QuoteLines { get; set; }
    public double AccountingCostTotal { get; set; }
    public double StandardCostTotal { get; set; }
    public double PriceTotal { get; set; }
    public BOXX_V2Entities BV2 { get; set; }
    public VW_OMCWebQuote WebQuote { get; set; }
    public string CompanyName { get; set; }
    public string QuoteType { get; set; }

    public Boolean GetData()
    {
        // Remove the Q from quote ID if exists
        // Retrieve row from WebQuote table using the quoteID
        // Return true if valid quote
        if (QuoteId.Substring(0, 1) == "Q") { QuoteId = QuoteId.Substring(1, QuoteId.Length - 1); }
        int i = new int();
        if (int.TryParse(this.QuoteId, out i)) { WebQuote = BV2.VW_OMCWebQuote.Where(q => q.QuoteID == i).FirstOrDefault(); }
        return DetermineQuoteType();
    }

    private bool DetermineQuoteType()
    {
        // If WebQuote null, quote = QuoteExplorer/Quickship
        // Else, Get Company name and Check CudXML
        // If null, quote = quickship
        // Else, quote = webQuote
        // Return true if valid quote
        try
        {
            if (WebQuote != null)
            {
                CompanyName = WebQuote.OrderCustomerCompany;
                if (WebQuote.CudXML != null)
                {
                    SetQuoteLineFromCudXML(WebQuote.CudXML);
                }
                else
                {
                    SetQuoteLineForQuickShip();
                }
                return true;
            }
            else
            {
                if (SetQuoteLineForQuoteExplorer()) { return true; }
            }
            return false;
        }
        catch /*(Exception e)*/ { return false; }
    }

    public void UpdateLines()
    {
        // For each quote line in a quote
        // Set ComponentType, PartName, StandardCost, and AccountingCost
        for (int i = 0; i < this.QuoteLines.Count; i++)
        {
            this.QuoteLines[i].PopulateLineValues();
        }
    }

    private void SetQuoteLineFromCudXML(string cudXml)
    {
        // Create an XMLDocument object from the cudXML string
        // Get all nodes with <list-entity/values/value>
        // For each <value> node,
        //      Create a MyQuoteLine object
        //      Set MyQuoteLine.PartNumber to the <list-entity/values/value/key-value> inner node
        //      Get all <list-entity/values/value/characteristics> nodes
        //      Set MyQuoteLine Characteristics and add to QuoteLines
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(cudXml);
        XmlNodeList itemVals = doc.DocumentElement.SelectNodes("/*[name() = 'user-selections']/*[name()='list-entity']/*[name() = 'values']/*[name() = 'value']");

        foreach (XmlNode itemVal in itemVals)
        {
            MyQuoteLine ql = new MyQuoteLine
            {
                PartNumber = itemVal.SelectSingleNode("*[name() = 'key-value']").InnerText.ToString()
            };
            XmlNodeList charNodes = itemVal.SelectNodes("*[name() = 'characteristics']/*[name()='characteristic']");
            SetQuoteLineCharacteristics(charNodes, ql);
            this.QuoteLines.Add(ql);
        }
    }

    private void SetQuoteLineCharacteristics(XmlNodeList charNodes, MyQuoteLine ql)
    {
        // For each <characteristics> node
        //      If node has a PartNumber node, set MyQuote.Partnumber
        //      If noes has a SalesPriceDom node, set MyQuote.ComponentPrice
        foreach (XmlNode node in charNodes)
        {
            if (node.Attributes != null)
            {
                if (node.Attributes["name"].Value.Equals("PartNumber"))
                {
                    ql.PartNumber = node.InnerText.ToString();
                }
                if (node.Attributes["name"].Value.Equals("SalesPriceDom"))
                {
                    double outDouble = new double();
                    if (double.TryParse(node.InnerText.ToString(), out outDouble))
                    {
                        ql.ComponentPrice = outDouble;
                    }
                    else
                    {
                        ql.ComponentPrice = 0;
                    }
                }
            }
        }
    }

    private void SetQuoteLineForQuickShip()
    {
        // Create a List of VW_OMCQuickShip Items
        // Add all rows from VW_OMCQuickShip that have an OrderLineID or OrderLineParentLineID equal to the given quoteID
        // For each VW_OMCQuickShip item in the list
        //      Create a MyQuoteLine object
        //      Set MyQuoteLine, and and MyQuoteLine to QuoteLines
        List<VW_OMCQuickShip> quickShipPrices = BV2.VW_OMCQuickShip.Where(z => z.OrderLineID == WebQuote.cudID
                || z.OrderLineParentLineID == WebQuote.cudID).ToList();
        foreach (var line in quickShipPrices)
        {
            MyQuoteLine quoteLine = new MyQuoteLine
            {
                PartNumber = line.OrderLineProductID,
                PartName = line.OrderLineProductName,
                ComponentPrice = Double.Parse(line.OrderLineUnitPriceWithVAT.ToString())
            };
            this.QuoteLines.Add(quoteLine);
        }
    }

    private bool SetQuoteLineForQuoteExplorer()
    {
        // Convert QuoteID to an integer
        // Create a List of IntQuoteTran Items
        // Add all rows from IntQuoteTable that have ID equal to the given quoteid AND ts_QuoteDescription is not "None" AND ts_Price is not 0
        // If List contains no values, return false (invalid quote)
        // For each item in List,
        //      Create a MyQuoteLine Item
        //      Set MyQuoteLine, get IMA_ItemID from row and set MyQuoteLine PartNumber
        // Add MyQuoteLine to QuoteLines

        int quoteId = Convert.ToInt32(this.QuoteId);
        List<IntQuoteTran> quoteTrans = BV2.IntQuoteTrans.Where(z => z.QuoteID == quoteId && z.ts_QuoteDescription != "None" && z.ts_Price != 0).ToList();
        if (quoteTrans.Count == 0) { return false; }
        foreach (IntQuoteTran line in quoteTrans)
        {
            MyQuoteLine quoteLine = new MyQuoteLine
            {
                PartName = line.ts_QuoteDescription,
                ComponentPrice = Convert.ToDouble(line.ts_Price)
            };
            tItem itemId = BV2.tItems.Where(z => z.ItemID == line.ItemID.ToString()).FirstOrDefault();
            quoteLine.PartNumber = itemId.IMA_ItemID;
            QuoteLines.Add(quoteLine);
        }
        return true;
    }

    public void CalculateTotals()
    {
        // Sum columns for AccountingCost, StandardCost, and ComponentPrice
        for (int i = 0; i < this.QuoteLines.Count; i++)
        {
            this.AccountingCostTotal += this.QuoteLines[i].AccountingCost;
            this.StandardCostTotal += this.QuoteLines[i].StandardCost;
            this.PriceTotal += this.QuoteLines[i].ComponentPrice;
        }
    }
}