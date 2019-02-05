using System.Data;
using System.Linq;

public class MyQuoteLine {
    public double AccountingCost { get; set; }
    public string Component { get; internal set; }
    public double ComponentPrice { get; internal set; }
    public string ComponentType { get; internal set; }
    public string PartName { get; internal set; }
    public string PartNumber { get; internal set; }
    public double StandardCost { get; private set; }
    public double ComponentMargin { get; private set; }

    internal void PopulateLineValues() {
        BOXX_V2Entities bv2 = new BOXX_V2Entities();
        VW_OMCWebItem item = bv2.VW_OMCWebItem.Where(z => z.ItemID == this.PartNumber).FirstOrDefault();
        try {
            this.ComponentType = item.ComponentType;
            this.PartName = item.ItemName;
            this.StandardCost = double.Parse(item.StandardCost.ToString());
            this.AccountingCost = double.Parse(item.AccountingCost.ToString());
        } catch {
            return;
        }
    }
}