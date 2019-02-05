using System;

public class MyDiscount
{
    public double DiscountPercent { get; set; }
    public double ListPrice { get; set; }
    public double DiscountedPrice { get; set; }
    public double Cost { get; set; }
    public int Quantity { get; set; }
    public double RevenuesExtended { get; set; }
    public double CostExtended { get; set; }
    public double MarginExtended { get; set; }
    public double MarginRevenuesPercentage { get; set; }
    public double Warranty { get; set; }

    public static double FirstDiscount = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["FirstDiscount"]);
    public static double SecondDiscount = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["SecondDiscount"]);
    public static double ThirdDiscount = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["ThirdDiscount"]);

    internal void FinishSettingMyDiscount(int i)
    {
        if (i == 0)
        {
            this.DiscountPercent = DiscountPercent / 100.00;
        }
        else if (i == 1)
        {
            this.DiscountPercent = 0;
        }
        else if (i == 2)
        {
            this.DiscountPercent = FirstDiscount;
        }
        else if (i == 3)
        {
            this.DiscountPercent = SecondDiscount;
        }
        else if (i == 4)
        {
            this.DiscountPercent = ThirdDiscount;
        }
        this.CalculateMyDiscount();
    }

    private void CalculateMyDiscount()
    {
        this.ListPrice = this.ListPrice;// - ((this.Undiscounted - this.Warranty) * (this.DiscountPercent));
        this.DiscountedPrice = (this.ListPrice - this.Warranty) * (1 - this.DiscountPercent) + this.Warranty;
        this.RevenuesExtended = this.DiscountedPrice * this.Quantity;
        this.CostExtended = this.Cost * this.Quantity;
        this.MarginExtended = this.RevenuesExtended - this.CostExtended;
        this.MarginRevenuesPercentage = this.MarginExtended / this.RevenuesExtended;
    }
}