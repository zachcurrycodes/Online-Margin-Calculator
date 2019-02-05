using System;
using System.Web.UI.WebControls;

public class MyDataTableTargetMargin : MyEvent {
    private GridViewRow GridViewRow { get; set; }

    private static double TargetMarginPercent1 = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["TargetMarginPercent1"]);
    private static double TargetMarginPercent2 = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["TargetMarginPercent2"]);
    private static double TargetMarginPercent3 = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["TargetMarginPercent3"]);

    public void PopulateGridview() {
        HeaderRow();
        for (int i = 0; i <= 1; i++) {
            this.GridViewRow = base.GVTargetMargin.Rows[i]; switch (i) {
                case 0: FirstRow(); break;
                case 1: SecondRow(); break;
                default: break;
            }
        }
    }

    private void HeaderRow() {
        (base.GVTargetMargin.HeaderRow.Cells[1]).Text = Math.Round(TargetMarginPercent1 * 100, 2).ToString() + "%";
        (base.GVTargetMargin.HeaderRow.Cells[2]).Text = Math.Round(TargetMarginPercent2 * 100, 2).ToString() + "%";
        (base.GVTargetMargin.HeaderRow.Cells[3]).Text = Math.Round(TargetMarginPercent3 * 100, 2).ToString() + "%";
    }

    private void FirstRow() {
        (GridViewRow.FindControl("ltlDescription") as Literal).Text = "Sell Price";
        (GridViewRow.FindControl("ltlTargetMarginPercent1") as Literal).Text = base.LstMyTargetMargin[0].SellPrice.ToString("C");
        (GridViewRow.FindControl("ltlTargetMarginPercent2") as Literal).Text = base.LstMyTargetMargin[1].SellPrice.ToString("C");
        (GridViewRow.FindControl("ltlTargetMarginPercent3") as Literal).Text = base.LstMyTargetMargin[2].SellPrice.ToString("C");
    }

    private void SecondRow() {
        (GridViewRow.FindControl("ltlDescription") as Literal).Text = "Discount";
        (GridViewRow.FindControl("ltlTargetMarginPercent1") as Literal).Text = Math.Round(base.LstMyTargetMargin[0].Discount * 100, 2).ToString() + "%";
        (GridViewRow.FindControl("ltlTargetMarginPercent2") as Literal).Text = Math.Round(base.LstMyTargetMargin[1].Discount * 100, 2).ToString() + "%";
        (GridViewRow.FindControl("ltlTargetMarginPercent3") as Literal).Text = Math.Round(base.LstMyTargetMargin[2].Discount * 100, 2).ToString() + "%";
    }
}