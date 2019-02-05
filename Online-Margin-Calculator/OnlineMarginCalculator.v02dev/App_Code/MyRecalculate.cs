using System;
using System.Web.UI.WebControls;

public class MyRecalculate : MyEvent { 
    public void Recalculate() {
        // Use all information in the GridView to recalculate the all GridViews
        this.ComponentGrid();
        base.DiscountGrid();
        base.TargetMarginGrid();
    }

    public void ResetQuoteValues() {
        // Returns Component Table back to the original quote values
        base.UpdateGridView(base.GVComponent, base.LstMyComponent);
        this.Recalculate();
    }

    public void ComponentGrid() {
        this.PopulateLstMyComponent();
        base.UpdateGridView(base.GVComponent, base.LstMyComponent);
    }

    public void SetTotalPriceAccountingCostWarranty() {
        base.ListPrice = base.LstMyComponent[base.LstMyComponent.Count - 1].ComponentPrice;
        base.AccountingCost = base.LstMyComponent[base.LstMyComponent.Count - 1].AccountingCost;
        base.GetWarrantyFromGrid();
    }

    private void PopulateLstMyComponent() {
        // Set Total of Cost, Price, and Margin to Zero before adding totals
        // For a each MyComponent in LstMyComponent, get Cost and price from current GridView, then add to totals
        // Calculate Component Margin for each row
        // Calculate Component for totals row
        // Get List Price values
        base.LstMyComponent[base.LstMyComponent.Count - 1].AccountingCost = 0;
        base.LstMyComponent[base.LstMyComponent.Count - 1].ComponentPrice = 0;
        for (int i = 0; i < base.LstMyComponent.Count - 1; i++) {
            base.LstMyComponent[LstMyComponent.Count - 1].AccountingCost += GetAccountingCost(i);
            base.LstMyComponent[LstMyComponent.Count - 1].ComponentPrice += GetComponentPrice(i);
            base.LstMyComponent[i].CalculateComponentMargin();
        }
        base.LstMyComponent[LstMyComponent.Count - 1].CalculateComponentMargin();
        this.SetTotalPriceAccountingCostWarranty();
    }

    private double GetAccountingCost(int i) {
        // Get cost written in textbox
        // If empty, set to 0, Else check if double
        // Round to 2 decimal places, set and return AccountingCost for this row
        double dblAccountingCost = 0;
        string txtAccountingCost = (base.GVComponent.Rows[i].FindControl("txtAccountingCost") as TextBox).Text;
        if (txtAccountingCost == "") { txtAccountingCost = "0"; }
        if (Double.TryParse(txtAccountingCost, out dblAccountingCost)) {
            if (dblAccountingCost != Math.Round(base.LstMyComponent[i].AccountingCost, 2)) {
                base.LstMyComponent[i].AccountingCost = dblAccountingCost;
            }
        }
        return dblAccountingCost;
    }
    private double GetComponentPrice(int i) {
        // Get price written in textbox
        // If empty, set to 0, Else check if double
        // Round to 2 decimal places, set and return ComponentPrice for this row
        double dblComponentPrice = 0;
        string txtComponentPrice = (base.GVComponent.Rows[i].FindControl("txtComponentPrice") as TextBox).Text;
        if (txtComponentPrice == "") { txtComponentPrice = "0"; }
        if (Double.TryParse(txtComponentPrice, out dblComponentPrice)) {
            if (dblComponentPrice != Math.Round(base.LstMyComponent[i].ComponentPrice, 2)) {
                base.LstMyComponent[i].ComponentPrice = dblComponentPrice;
            }
        }
        return dblComponentPrice;
    }
}