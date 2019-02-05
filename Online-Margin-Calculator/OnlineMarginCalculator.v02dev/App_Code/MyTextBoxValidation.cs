using System;
using System.Web;

public class MyTextBoxValidation {
    public string StrQuoteIDTxtBox { get; set; }
    public string StrQuantityTxtBox { get; set; }
    public string StrDiscountpercentTxtBox { get; set; }
    public HttpResponse Response { get; set; }

    public bool Validated() {
        if (this.quoteIDValidation()) { return false; }
        if (this.quantityValidation()) { return false; }
        if (this.discountPercentValidation()) { return false; }
        return true;
    }

    public bool quoteIDValidation() {
        int ignoreMe = new int(); //used only for the tryParse
        if (this.StrQuoteIDTxtBox == "") {
            this.Response.Write("<script>alert('Please enter a quote ID');</script>");
            return true;
        }
        if (this.StrQuoteIDTxtBox.Substring(0, 1) == "Q") {
            this.StrQuoteIDTxtBox = this.StrQuoteIDTxtBox.Substring(1, this.StrQuoteIDTxtBox.Length - 1);
        }
        if (!int.TryParse(this.StrQuoteIDTxtBox, out ignoreMe)) {
            this.Response.Write("<script>alert('Please enter a number (integer) for quote ID');</script>");
            return true;
        }
        return false;
    }

    public bool quantityValidation() {
        int ignoreMe = new int(); //used only for the tryParse
        if (!int.TryParse(this.StrQuantityTxtBox, out ignoreMe)) {
            this.Response.Write("<script>alert('Please enter a number (integer) for quantity');</script>");
            return true;
        }
        if (Convert.ToInt32(this.StrQuantityTxtBox) < 1) {
            this.Response.Write("<script>alert('Please enter a number greater than zero (integer) for quantity');</script>");
            return true;
        }
        return false;
    }

    public bool discountPercentValidation() {
        double ignoreMe = new double(); //used only for the tryParse
        if (!double.TryParse(this.StrDiscountpercentTxtBox, out ignoreMe)) {
            this.Response.Write("<script>alert('Please enter a number for Discount Percent');</script>");
            return true;
        }
        if (Convert.ToDouble(this.StrDiscountpercentTxtBox) < 0) {
            this.Response.Write("<script>alert('Please enter a number greater than zero (0) for Discount Percent');</script>");
            return true;
        }
        if (Convert.ToDouble(this.StrDiscountpercentTxtBox) >= 100) {
            this.Response.Write("<script>alert('Please enter a number less than One Hundred (100) for Discount Percent');</script>");
            return true;
        }
        if (decimal.Round(Convert.ToDecimal(this.StrDiscountpercentTxtBox), 2) != Convert.ToDecimal(this.StrDiscountpercentTxtBox)) {
            this.Response.Write("<script>alert('Please enter a number with less than 2 decimal places for Discount Percent');</script>");
            return true;
        }
        return false;
    }
}