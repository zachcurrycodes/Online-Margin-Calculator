using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

public class MyEvent
{
    public MyEvent()
    {
        this.LstMyComponent = new List<MyComponent>();
        this.LstMyDiscount = new List<MyDiscount>();
        this.LstMyTargetMargin = new List<MyTargetMargin>();
    }

    public GridView GVComponent { protected get; set; }
    public GridView GVDiscount { protected get; set; }
    public GridView GVTargetMargin { protected get; set; }
    public List<MyComponent> LstMyComponent { get; set; }
    protected List<MyDiscount> LstMyDiscount { get; set; }
    protected List<MyTargetMargin> LstMyTargetMargin { get; set; }
    protected DataTable DtDiscount { get; set; }
    private DataTable DtTargetMargin { get; set; }
    public double ListPrice2 { get; protected set; }
    public double Warranty { get; protected set; }
    public double ListPrice { get; protected set; }
    public int Quantity { private get; set; }
    public double Discount { private get; set; }
    protected double AccountingCost { get; set; }
    public bool DiscountWarrantyChecked { private get; set; }

    protected void DiscountGrid()
    {
        this.PopulateLstMyDiscount();
        this.PopulateGVDiscount();
    }

    protected void TargetMarginGrid()
    {
        this.PopulateLstMyTargetMargin();
        this.PopulateGVTargetMargin();
    }

    protected void UpdateGridView(GridView gv, object ds)
    {
        gv.DataSource = ds;
        gv.DataBind();
    }

    private DataTable CreateEmptyRowsForDataTable(DataTable dt, int numberOfRows)
    {
        // Set DataTable to update
        // Create empty rows in DataTable
        // Return the DataTable
        dt = new DataTable();
        for (int i = 0; i < numberOfRows; i++)
        {
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
        }
        return dt;
    }

    #region Discount Functions
    private void PopulateLstMyDiscount()
    {
        // For a total of 5 times (5 columns), Create a MyDiscount object
        // Set Discount Percent and Calculate MyDiscount cost extended, margin extended, and margin/revenues percentage
        // Add MyDiscount to LstMyDiscount
        for (int i = 0; i < 5; i++)
        {
            MyDiscount md = new MyDiscount
            {
                ListPrice = this.ListPrice,
                Cost = this.AccountingCost,
                Quantity = this.Quantity,
                DiscountPercent = this.Discount,
                Warranty = this.Warranty
            };
            md.FinishSettingMyDiscount(i);
            this.LstMyDiscount.Add(md);
        }
    }

    private void PopulateGVDiscount()
    {
        // Create 9 empty rows in the DtDiscount DataTable
        // Update the Discount GridView with the 9 blank rows
        // Populate each row using the MyDiscount objects in LstMyDiscount
        // Update Discount Gridview
        this.DtDiscount = this.CreateEmptyRowsForDataTable(DtDiscount, 9);
        this.UpdateGridView(this.GVDiscount, this.DtDiscount);
        MyDataTableDiscount ddt = new MyDataTableDiscount
        {
            LstMyDiscount = this.LstMyDiscount,
            GVDiscount = this.GVDiscount,
            DtDiscount = this.DtDiscount
        };
        ddt.PopulateGridview();
    }

    protected void GetWarrantyFromGrid()
    {
        // if checkbox checked get warranty
        //      if warranty is included, set warranty
        //      else warranty is zero
        if (!this.DiscountWarrantyChecked)
        {
            if (this.LstMyComponent[this.LstMyComponent.Count - 2].ComponentType.Contains("Warranty"))
            {
                this.Warranty = this.LstMyComponent[this.LstMyComponent.Count - 2].ComponentPrice;
            }
            else
            {
                this.Warranty = 0;
            }
        }
        else { this.Warranty = 0; }
    }
    #endregion

    #region TargetMargin Functions
    private void PopulateLstMyTargetMargin()
    {
        // For a total of 3 times (3 columns), Create a MyTargetMargin object
        // Set TargetMargin and calculate SellPrice and Discount
        // Add MyTargetMargin to LstMyTargetMargin
        for (int i = 0; i < 3; i++)
        {
            MyTargetMargin mtm = new MyTargetMargin
            {
                ListPrice = this.ListPrice,
                Cost = this.AccountingCost
            };
            mtm.FinishSettingTargetMargin(i);
            this.LstMyTargetMargin.Add(mtm);
        }
    }

    private void PopulateGVTargetMargin()
    {
        // Create 2 empty rows in the dtTargetMargin DataTable
        // Update the Target Margin GridView with the 2 blank rows
        // Populate each row using the MyTargetMargin objects in LstMyTargetMargin
        // Update Target Margin Gridview
        this.DtTargetMargin = this.CreateEmptyRowsForDataTable(DtTargetMargin, 2);
        this.UpdateGridView(this.GVTargetMargin, this.DtTargetMargin);
        MyDataTableTargetMargin tmdt = new MyDataTableTargetMargin
        {
            GVTargetMargin = this.GVTargetMargin,
            LstMyTargetMargin = this.LstMyTargetMargin,
            DtTargetMargin = this.DtTargetMargin
        };
        tmdt.PopulateGridview();
    }
    #endregion 
}