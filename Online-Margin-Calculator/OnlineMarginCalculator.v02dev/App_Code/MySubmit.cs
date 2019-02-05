using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public class MySubmit : MyEvent {
    public MyQuote Quote { private get; set; }

    public void Sumbit() {
        this.ComponentGrid();
        base.DiscountGrid();
        base.TargetMarginGrid();
    }

    private void ComponentGrid() {
        this.PopulateLstMyComponent();
        this.UpdateGridView(GVComponent, base.LstMyComponent);
    }

    private void PopulateLstMyComponent() {
        // Get total of quoteLines
        // For a total of (QuoteLine total) times, Create a MyDiscount object
        // Set ComponentType, PartNumber, PartName, AccouningCost, StandardCost, ComponentPrice
        // Calculate the Componnent Margin
        // Add MyComponent to LstMyComponent
        for (int i = 0; i < this.Quote.QuoteLines.Count; i++) {
            MyComponent mc = new MyComponent {
                ComponentType = this.Quote.QuoteLines[i].ComponentType,
                PartNumber = this.Quote.QuoteLines[i].PartNumber,
                PartName = this.Quote.QuoteLines[i].PartName,
                AccountingCost = this.Quote.QuoteLines[i].AccountingCost,
                StandardCost = this.Quote.QuoteLines[i].StandardCost,
                ComponentPrice = this.Quote.QuoteLines[i].ComponentPrice
            };
            mc.CalculateComponentMargin();
            base.LstMyComponent.Add(mc);
        }
        // Continue Submit Process
        this.SetTotalsWarrantyListPrice();
    }

    private void SetTotalsWarrantyListPrice() {
        // Remove uneeded quotelines and update the totals
        // Set Warranty and Limit
        this.RemoveBlankLines();
        this.UpdateTotals();
        base.GetWarrantyFromGrid();
    }

    private void UpdateTotals() {
        // Add up total costs and price
        // Create a MyComponent object
        // Calculate the Componnent Margin
        // Set TotalPrice and Accounting Cost
        // Add MyComponnent to LstMyComponent
        this.Quote.CalculateTotals();
        MyComponent mc = new MyComponent {
            PartName = "Totals",
            AccountingCost = this.Quote.AccountingCostTotal,
            StandardCost = this.Quote.StandardCostTotal,
            ComponentPrice = this.Quote.PriceTotal
        };
        mc.CalculateComponentMargin();
        this.ListPrice = mc.ComponentPrice;
        this.AccountingCost = mc.AccountingCost;
        base.LstMyComponent.Add(mc);
    }

    private void RemoveBlankLines() {
        // Remove any and all lines that include any of the following
        base.LstMyComponent = base.LstMyComponent.Where(x => x.PartName != null && !(
                                                   x.PartName.ToLower() == "none"
                                                || x.PartName.ToLower() == "no card selected"
                                                || x.PartName.ToLower() == "no add in card selected"
                                                || x.PartName == "&nbsp;")).ToList();
    }
}