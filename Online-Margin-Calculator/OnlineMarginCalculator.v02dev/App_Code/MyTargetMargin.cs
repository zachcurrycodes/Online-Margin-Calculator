using System;

public class MyTargetMargin {
    public double SellPrice { get; set; }
    public double Discount { get; set; }
    public double ListPrice { get; set; }
    public double Cost { get; set; }
    private double TargetMargin { get; set; }

    public static double TargetMarginPercent1 = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["TargetMarginPercent1"]);
    public static double TargetMarginPercent2 = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["TargetMarginPercent2"]);
    public static double TargetMarginPercent3 = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["TargetMarginPercent3"]);

    internal void FinishSettingTargetMargin(int i) {
        if (i == 0) {
            this.TargetMargin = TargetMarginPercent1;
        } else if (i == 1) {
            this.TargetMargin = TargetMarginPercent2;
        } else if (i == 2) {
            this.TargetMargin = TargetMarginPercent3;
        }
        this.CalculateMyTargetMargin();
    }

    private void CalculateMyTargetMargin() {
        this.SellPrice = this.Cost / (1 - this.TargetMargin);
        this.Discount = 1 - (this.SellPrice / this.ListPrice);
    }
}