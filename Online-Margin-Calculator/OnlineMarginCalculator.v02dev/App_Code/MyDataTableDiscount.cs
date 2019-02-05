using System;
using System.Web.UI.WebControls;

public class MyDataTableDiscount : MyEvent {
    private GridViewRow GridViewRow { get; set; }

    public void PopulateGridview() {
        base.UpdateGridView(base.GVDiscount, base.DtDiscount);
        for (int i = 0; i <= 8; i++) {
            this.GridViewRow = base.GVDiscount.Rows[i];
            switch (i) {
                case 0: FirstRow(); break;
                case 1: SecondRow(); break;
                case 2: ThirdRow(); break;
                case 3: ForthRow(); break;
                case 4: FifthRow(); break;
                case 5: SixthRow(); break;
                case 6: SeventhRow(); break;
                case 7: EigthRow(); break;
                case 8: NinthRow(); break;
                default: break;
            }
        }
    }

    private void FirstRow() {
        (GridViewRow.FindControl("ltlDescription") as Literal).Text = "Discount %";
        (GridViewRow.FindControl("ltlQuotePercentage") as Literal).Text = Math.Round(base.LstMyDiscount[0].DiscountPercent * 100, 2).ToString() + "%";
        (GridViewRow.FindControl("ltlNoDiscount") as Literal).Text = "0%";
        (GridViewRow.FindControl("ltlFirstDiscount") as Literal).Text = Math.Round(Double.Parse(System.Configuration.ConfigurationManager.AppSettings["FirstDiscount"]) * 100, 2).ToString() + "%";
        (GridViewRow.FindControl("ltlSecondDiscount") as Literal).Text = Math.Round(Double.Parse(System.Configuration.ConfigurationManager.AppSettings["SecondDiscount"]) * 100, 2).ToString() + "%";
        (GridViewRow.FindControl("ltlThirdDiscount") as Literal).Text = Math.Round(Double.Parse(System.Configuration.ConfigurationManager.AppSettings["ThirdDiscount"]) * 100, 2).ToString() + "%";
    }

    public void SecondRow() {
        (GridViewRow.FindControl("ltlDescription") as Literal).Text = "Undiscounted - Each";
        (GridViewRow.FindControl("ltlQuotePercentage") as Literal).Text = base.LstMyDiscount[0].ListPrice.ToString("C");
        (GridViewRow.FindControl("ltlNoDiscount") as Literal).Text = base.LstMyDiscount[1].ListPrice.ToString("C");
        (GridViewRow.FindControl("ltlFirstDiscount") as Literal).Text = base.LstMyDiscount[2].ListPrice.ToString("C");
        (GridViewRow.FindControl("ltlSecondDiscount") as Literal).Text = base.LstMyDiscount[3].ListPrice.ToString("C");
        (GridViewRow.FindControl("ltlThirdDiscount") as Literal).Text = base.LstMyDiscount[4].ListPrice.ToString("C");
    }

    public void ThirdRow() {
        (GridViewRow.FindControl("ltlDescription") as Literal).Text = "Discounted - Each";
        (GridViewRow.FindControl("ltlQuotePercentage") as Literal).Text = base.LstMyDiscount[0].DiscountedPrice.ToString("C");
        (GridViewRow.FindControl("ltlNoDiscount") as Literal).Text = base.LstMyDiscount[1].DiscountedPrice.ToString("C");
        (GridViewRow.FindControl("ltlFirstDiscount") as Literal).Text = base.LstMyDiscount[2].DiscountedPrice.ToString("C");
        (GridViewRow.FindControl("ltlSecondDiscount") as Literal).Text = base.LstMyDiscount[3].DiscountedPrice.ToString("C");
        (GridViewRow.FindControl("ltlThirdDiscount") as Literal).Text = base.LstMyDiscount[4].DiscountedPrice.ToString("C");
    }

    public void ForthRow() {
        (GridViewRow.FindControl("ltlDescription") as Literal).Text = "Cost - Each";
        (GridViewRow.FindControl("ltlQuotePercentage") as Literal).Text = base.LstMyDiscount[0].Cost.ToString("C");
        (GridViewRow.FindControl("ltlNoDiscount") as Literal).Text = base.LstMyDiscount[1].Cost.ToString("C");
        (GridViewRow.FindControl("ltlFirstDiscount") as Literal).Text = base.LstMyDiscount[2].Cost.ToString("C");
        (GridViewRow.FindControl("ltlSecondDiscount") as Literal).Text = base.LstMyDiscount[3].Cost.ToString("C");
        (GridViewRow.FindControl("ltlThirdDiscount") as Literal).Text = base.LstMyDiscount[4].Cost.ToString("C");
    }

    public void FifthRow() {
        (GridViewRow.FindControl("ltlDescription") as Literal).Text = "Quantity";
        (GridViewRow.FindControl("ltlQuotePercentage") as Literal).Text = base.LstMyDiscount[0].Quantity.ToString();
        (GridViewRow.FindControl("ltlNoDiscount") as Literal).Text = base.LstMyDiscount[1].Quantity.ToString();
        (GridViewRow.FindControl("ltlFirstDiscount") as Literal).Text = base.LstMyDiscount[2].Quantity.ToString();
        (GridViewRow.FindControl("ltlSecondDiscount") as Literal).Text = base.LstMyDiscount[3].Quantity.ToString();
        (GridViewRow.FindControl("ltlThirdDiscount") as Literal).Text = base.LstMyDiscount[4].Quantity.ToString();
    }

    public void SixthRow() {
        (GridViewRow.FindControl("ltlDescription") as Literal).Text = "Revenues - Extended";
        (GridViewRow.FindControl("ltlQuotePercentage") as Literal).Text = base.LstMyDiscount[0].RevenuesExtended.ToString("C");
        (GridViewRow.FindControl("ltlNoDiscount") as Literal).Text = base.LstMyDiscount[1].RevenuesExtended.ToString("C");
        (GridViewRow.FindControl("ltlFirstDiscount") as Literal).Text = base.LstMyDiscount[2].RevenuesExtended.ToString("C");
        (GridViewRow.FindControl("ltlSecondDiscount") as Literal).Text = base.LstMyDiscount[3].RevenuesExtended.ToString("C");
        (GridViewRow.FindControl("ltlThirdDiscount") as Literal).Text = base.LstMyDiscount[4].RevenuesExtended.ToString("C");
    }

    public void SeventhRow() {
        (GridViewRow.FindControl("ltlDescription") as Literal).Text = "Cost - Extended";
        (GridViewRow.FindControl("ltlQuotePercentage") as Literal).Text = base.LstMyDiscount[0].CostExtended.ToString("C");
        (GridViewRow.FindControl("ltlNoDiscount") as Literal).Text = base.LstMyDiscount[1].CostExtended.ToString("C");
        (GridViewRow.FindControl("ltlFirstDiscount") as Literal).Text = base.LstMyDiscount[2].CostExtended.ToString("C");
        (GridViewRow.FindControl("ltlSecondDiscount") as Literal).Text = base.LstMyDiscount[3].CostExtended.ToString("C");
        (GridViewRow.FindControl("ltlThirdDiscount") as Literal).Text = base.LstMyDiscount[4].CostExtended.ToString("C");
    }

    public void EigthRow() {
        (GridViewRow.FindControl("ltlDescription") as Literal).Text = "Margin - Extended";
        (GridViewRow.FindControl("ltlQuotePercentage") as Literal).Text = base.LstMyDiscount[0].MarginExtended.ToString("C");
        (GridViewRow.FindControl("ltlNoDiscount") as Literal).Text = base.LstMyDiscount[1].MarginExtended.ToString("C");
        (GridViewRow.FindControl("ltlFirstDiscount") as Literal).Text = base.LstMyDiscount[2].MarginExtended.ToString("C");
        (GridViewRow.FindControl("ltlSecondDiscount") as Literal).Text = base.LstMyDiscount[3].MarginExtended.ToString("C");
        (GridViewRow.FindControl("ltlThirdDiscount") as Literal).Text = base.LstMyDiscount[4].MarginExtended.ToString("C");
    }

    public void NinthRow() {
        (GridViewRow.FindControl("ltlDescription") as Literal).Text = "Margin/Revenues %";
        (GridViewRow.FindControl("ltlQuotePercentage") as Literal).Text = Math.Round(base.LstMyDiscount[0].MarginRevenuesPercentage * 100, 2).ToString() + "%";
        (GridViewRow.FindControl("ltlNoDiscount") as Literal).Text = Math.Round(base.LstMyDiscount[1].MarginRevenuesPercentage * 100, 2).ToString() + "%";
        (GridViewRow.FindControl("ltlFirstDiscount") as Literal).Text = Math.Round(base.LstMyDiscount[2].MarginRevenuesPercentage * 100, 2).ToString() + "%";
        (GridViewRow.FindControl("ltlSecondDiscount") as Literal).Text = Math.Round(base.LstMyDiscount[3].MarginRevenuesPercentage * 100, 2).ToString() + "%";
        (GridViewRow.FindControl("ltlThirdDiscount") as Literal).Text = Math.Round(base.LstMyDiscount[4].MarginRevenuesPercentage * 100, 2).ToString() + "%";
    }
}