using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page {
    #region Properties
    public string QuoteIDTxtBoxString {
        get { return Session["strQuoteIDTxtBox"] as string; }
        set {
            Session["strQuoteIDTxtBox"] = value;
            txtQuoteID.Text = value;
        }
    }
    public string QuantityTxtBoxString {
        get { return Session["strQuantityTxtBox"] as string; }
        set {
            Session["strQuantityTxtBox"] = value;
            txtQuantity.Text = value;
        }
    }
    public string DiscountpercentTxtBoxString {
        get { return Session["strDiscountpercentTxtBox"] as string; }
        set {
            Session["strDiscountpercentTxtBox"] = value;
            txtDiscount.Text = value;
        }
    }
    public string URLString {
        get { return Session["strURL"] as string; }
        set { Session["strURL"] = value; }
    }
    public string QueryString {
        get { return Session["strQuerySting"] as string; }
        set { Session["strQuerySting"] = value; }
    }
    public bool? BoolFromLink {
        get { return Session["boolFromLink"] as bool?; }
        set { Session["boolFromLink"] = value; }
    }
    public bool? BoolSubmitted {
        get { return Session["boolSubmitted"] as bool?; }
        set { Session["boolSubmitted"] = value; }
    }
    public List<MyComponent> LstMyComponent {
        get { return Session["LstMyComponent"] as List<MyComponent>; }
        set { Session["LstMyComponent"] = value; }
    }
    public List<MyComponent> OriginalLstMyComponent {
        get { return Session["OriginalLstMyComponent"] as List<MyComponent>; }
        set { Session["OriginalLstMyComponent"] = value; }
    }
    public bool? OriginalDiscountWarranty {
        get { return Session["OriginalDiscountWarranty"] as bool?; }
        set { Session["OriginalDiscountWarranty"] = value; }
    }
    public string QuoteIDReset {
        get { return Session["QuoteIDReset"] as string; }
        set { Session["QuoteIDReset"] = value; }
    }
    public int? QuantityReset {
        get { return Session["QuantityReset"] as int?; }
        set { Session["QuantityReset"] = value; }
    }
    public double? DiscountReset {
        get { return Session["DiscountReset"] as double?; }
        set { Session["DiscountReset"] = value; }
    }
    public bool? DiscountWarrantyChecked {
        get { return Session["DiscountWarrantyChecked"] as bool?; }
        set { Session["DiscountWarrantyChecked"] = value; }
    }
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e) {
        // Set this.URLString to current string
        // Get URLQueryString Collection and Set this.QueryString
        this.URLString = HttpContext.Current.Request.Url.AbsoluteUri;

        // Page Refresh - Update textboxes and ListPice Label
        // Initial page load
        // Perform steps neccessary to set up page
        if (IsPostBack) { this.RefreshPage(); } else { this.InitialPageLoad(); }
        this.SetUpPage();
        txtQuoteID.Attributes.Add("onkeypress", "txtOnkeypress(event)");
        txtQuantity.Attributes.Add("onkeypress", "txtOnkeypress(event)");
        txtDiscount.Attributes.Add("onkeypress", "txtOnkeypress(event)");
    }

    private void SetUpPage() {
        // Place cursor in quoteID textbox and reset the gridviews
        // Submit info if page was submitted by using a url
        // Set Label to display the stored Session value for List Price
        txtQuoteID.Focus();
        if (this.BoolFromLink == true) { SubmitQuote(); }
        if (this.BoolSubmitted == true) { this.BoolSubmitted = null; }
    }

    private void InitialPageLoad() {
        // Page queryString does NOT exist
        // Page queryString EXISTS
        if (HttpContext.Current.Request.QueryString.Count != 0) { this.PageQueryExists(); } else { this.BoolFromLink = false; }
    }

    private void RefreshPage() {
        // Set Textbox and ListPice Label values before submitting
        this.QuoteIDTxtBoxString = txtQuoteID.Text;
        if (txtQuantity.Text == "") { txtQuantity.Text = "1"; }
        if (txtDiscount.Text == "") { txtDiscount.Text = "0"; }
        this.QuantityTxtBoxString = txtQuantity.Text;
        this.DiscountpercentTxtBoxString = txtDiscount.Text;
    }

    private void PageQueryExists() {
        NameValueCollection qs = this.Request.QueryString;
        this.QueryString = string.Join("", qs);
        string qsQuoteID;
        string qsQuantity;
        string qsDiscountpercent;
        // Incase not all queries exist in the URL
        try { qsQuoteID = qs["quoteID"].ToString(); } catch { qsQuoteID = "0"; }
        try { qsQuantity = qs["quantity"].ToString(); } catch { qsQuantity = "1"; }
        try { qsDiscountpercent = qs["discountpercent"].ToString(); } catch { qsDiscountpercent = "0"; }
        try { chbDiscountWarranty.Checked = (bool)this.DiscountWarrantyChecked; } catch { }
        // Determine if querey string is valid, populate textboxes or refresh with blank page
        DeterminePageDisplay(qsQuoteID, qsQuantity, qsDiscountpercent);
    }

    private void DeterminePageDisplay(string qsQuoteID, string qsQuantity, string qsDiscountpercent) {
        int i = new int();
        double d = new double();
        // Check if queryString QuoteID has an integer value if not, refresh page with blank page
        if (int.TryParse(qsQuoteID, out i) && qsQuoteID != "0") {
            // Set quote ID to query string value and default for quantity and discountPercent
            this.QuoteIDTxtBoxString = qsQuoteID;
            this.QuantityTxtBoxString = "1";
            this.DiscountpercentTxtBoxString = "0";
            this.BoolFromLink = true;
            // Check if queryString Quantity has value
            if (int.TryParse(qsQuantity, out i)) {
                this.QuantityTxtBoxString = qsQuantity;
            }
            // Check if queryString Discountpercent has value
            if (double.TryParse(qsDiscountpercent, out d)) {
                this.DiscountpercentTxtBoxString = qsDiscountpercent;
            }
        }
        // QuoteID no good, refresh with blank page and empty queryString
        else {
            string popUp = "Quote ID not valid";
            this.Response.Write("<script>alert('" + popUp + "');</script>");
            this.URLString = this.URLString.Substring(0, this.URLString.IndexOf("?"));
            this.Response.Redirect(this.URLString);
        }
    }
    #endregion

    #region Actions
    private void Submit() {
        // Create a TBV Object
        // If all textboxes pass validation, then submit quote number, update URL, and refresh page with new information
        MyTextBoxValidation tb = new MyTextBoxValidation() {
            StrQuoteIDTxtBox = this.QuoteIDTxtBoxString,
            StrQuantityTxtBox = this.QuantityTxtBoxString,
            StrDiscountpercentTxtBox = this.DiscountpercentTxtBoxString,
            Response = this.Response
        };
        if (tb.Validated()) {
            this.SubmitQuote();
            this.UpdateURLText();
            this.Response.Redirect(URLString);
        }
    }

    private void ResetCalculator() {
        // Clear all textboxes and refreshes to blank page
        this.QuoteIDTxtBoxString = "";
        this.QuantityTxtBoxString = "";
        this.DiscountpercentTxtBoxString = "";
        this.UpdateURLText();
        this.Response.Redirect(URLString);
    }

    private void Recalculate() {
        // Checks if quote ID has changed, if so, resubmit the quote
        // Create a recalculate object
        // Use all information in the GridView to recalculate the Component Table
        // Sets ListPrice Label text
        if (txtQuoteID.Text == this.QuoteIDReset) {
            MyRecalculate recalculate = new MyRecalculate {
                LstMyComponent = this.LstMyComponent,
                GVComponent = this.gvComponent,
                GVDiscount = this.gvDiscount,
                GVTargetMargin = this.gvTargetMargin,
                Quantity = Convert.ToInt32(txtQuantity.Text),
                Discount = Convert.ToDouble(txtDiscount.Text),
                DiscountWarrantyChecked = chbDiscountWarranty.Checked
            };
            recalculate.Recalculate();
        } else { Submit(); }
    }

    private void ReturnGridViewOriginalValues() {
        // Create a recalculate object
        // Returns Component Table back to the original quote values
        // Sets ListPrice Label text
        // Returns the textboxes to the original submitted quote
        MyRecalculate recalculate = new MyRecalculate {
            LstMyComponent = this.OriginalLstMyComponent,
            GVComponent = this.gvComponent,
            GVDiscount = this.gvDiscount,
            GVTargetMargin = this.gvTargetMargin,
            Quantity = Convert.ToInt32(this.QuantityReset),
            Discount = Convert.ToDouble(this.DiscountReset)
        };
        recalculate.ResetQuoteValues();
        txtQuoteID.Text = this.QuoteIDReset;
        txtQuantity.Text = this.QuantityReset.ToString();
        txtDiscount.Text = this.DiscountReset.ToString();
        chbDiscountWarranty.Checked = (bool)this.OriginalDiscountWarranty;
    }

    private void CreatePDF() {
        // Recalculate
        // Create a MyPdf object
        // Using the displayed GridViews on the page, creates a pdf document to save or display
        this.Recalculate();
        MyPDF myPDF = new MyPDF {
            GVComponent = this.gvComponent,
            GVDiscount = this.gvDiscount,
            GVTargetMargin = this.gvTargetMargin,
            Server = this.Server,
            Response = this.Response,
            CompanyName = lblCompanyName.Text,
            QuoteNumber = txtQuoteID.Text
        };
        myPDF.Create();
    }

    private void SubmitQuote() {
        // Creates a MyQuote object
        // If valid quote, proceed with submit, otherwise display a pop up
        MyQuote myQuote = new MyQuote { QuoteId = txtQuoteID.Text };
        if (myQuote.GetData()) { this.ValidQuote(myQuote); } else { this.InValidQuote(); }
    }

    private void ValidQuote(MyQuote myQuote) {
        // Populates all costs and prices using the item ID
        // Creates a MySubmit object
        // Populates all 3 GridViews
        // Sets LstMyComponent and OriginalLstMyComponent from MySubmit.LstMyComponent
        // Sets Original quote values Session Variables
        // Sets ListPrice Label text
        // Change visibilities and Page Booleans
        myQuote.UpdateLines();
        MySubmit submit = new MySubmit {
            Quote = myQuote,
            GVComponent = this.gvComponent,
            GVDiscount = this.gvDiscount,
            GVTargetMargin = this.gvTargetMargin,
            Quantity = int.Parse(txtQuantity.Text),
            Discount = Double.Parse(txtDiscount.Text),
            DiscountWarrantyChecked = chbDiscountWarranty.Checked
        };
        submit.Sumbit();
        lblCompanyName.Text = myQuote.CompanyName;
        this.LstMyComponent = submit.LstMyComponent;
        this.DiscountWarrantyChecked = chbDiscountWarranty.Checked;
        this.OriginalLstMyComponent = submit.LstMyComponent.DeepCopy();
        this.OriginalDiscountWarranty = this.DiscountWarrantyChecked;
        this.QuoteIDReset = txtQuoteID.Text;
        this.QuantityReset = Convert.ToInt32(txtQuantity.Text);
        this.DiscountReset = Convert.ToDouble(txtDiscount.Text);
        this.SetPageBools();
    }

    private void InValidQuote() {
        // Dispaly a popup and highlight the quoteID textbox
        string popUp = "Quote ID not valid";
        this.Response.Write("<script>alert('" + popUp + "');</script>");
        txtQuoteID.Focus();
        txtQuoteID.Attributes.Add("onfocus", "this.select();");
    }

    private void SetPageBools() {
        // Change visibilites and Page Booleans
        lblCompanyName.Visible = true;
        lblUpdate.Visible = false;
        btnCopyURL.Visible = true;
        btnPDF.Visible = true;
        btnRecalculate.Visible = true;
        btnOriginalValues.Visible = true;
        btnResetCalculator.Visible = true;
        chbDiscountWarranty.Visible = true;
        this.BoolFromLink = false;
        this.BoolSubmitted = true;
    }

    private void UpdateURLText() {
        // Create a blank queryString collection
        // As long as ...
        // Set queryString collection
        // Append QueryString to URLString
        NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
        if (this.QueryString != "" && this.QueryString != null) {
            try {
                this.URLString = this.URLString.Substring(0, this.URLString.IndexOf("?"));
            } catch { }
        }
        queryString["quoteid"] = this.QuoteIDTxtBoxString;
        queryString["quantity"] = this.QuantityTxtBoxString;
        queryString["discountpercent"] = this.DiscountpercentTxtBoxString;
        this.URLString += "?" + queryString.ToString();
    }

    private void DiscountWarranty()
    {

    }
    #endregion

    #region Buttons
    protected void BtnSubmit_Click(object sender, EventArgs e) { this.Submit(); }

    protected void BtnResetCalculator_Click(object sender, EventArgs e) { this.ResetCalculator(); }

    protected void BtnRecalculate_Click(object sender, EventArgs e) { this.Recalculate(); }

    protected void BtnPDF_Click(object sender, EventArgs e) { this.CreatePDF(); }

    protected void BtnOriginalValues_Click(object sender, EventArgs e) { this.ReturnGridViewOriginalValues(); }
    #endregion

    #region Used by ASPX
    public void ComponentRowDataBound(object sender, GridViewRowEventArgs e) {
        if (e.Row.RowType == DataControlRowType.DataRow) {
            if ((e.Row.FindControl("ltlPartName") as Literal).Text == "Totals") {
                (e.Row.FindControl("ltlDollarSignAC") as Literal).Visible = false;
                (e.Row.FindControl("txtAccountingCost") as TextBox).Visible = false;
                (e.Row.FindControl("ltlAccountingCost") as Literal).Visible = true;
                (e.Row.FindControl("ltlDollarSignCP") as Literal).Visible = false;
                (e.Row.FindControl("txtComponentPrice") as TextBox).Visible = false;
                (e.Row.FindControl("ltlComponentPrice") as Literal).Visible = true;
            }
        }
    }

    public string ConvertCurrency(double d) { return d.ToString("C"); }

    public string Convert2DecimalPlaces(double d) { return string.Format("{0:#.00}", d); }
    #endregion

}