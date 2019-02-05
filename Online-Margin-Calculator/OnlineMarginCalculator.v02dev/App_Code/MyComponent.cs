using System;

[Serializable]
public class MyComponent {
    public string ComponentType { get; set; }
    public string PartNumber { get; set; }
    public string PartName { get; set; }
    public double AccountingCost { get; set; }
    public double StandardCost { get; set; }
    public double ComponentPrice { get; set; }
    public double ComponentMargin { get; set; }

    internal void CalculateComponentMargin() {
        this.ComponentMargin = this.ComponentPrice -this.AccountingCost;
    }
}