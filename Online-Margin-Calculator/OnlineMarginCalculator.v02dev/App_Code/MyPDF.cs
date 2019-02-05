using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class MyPDF {
    public GridView GVComponent { get; set; }
    public GridView GVDiscount { get; set; }
    public GridView GVTargetMargin { get; set; }
    public HttpServerUtility Server { get; set; }
    public HttpResponse Response { get; set; }
    private PdfPTable TableComponent { get; set; }
    private PdfPTable TableDiscount { get; set; }
    private PdfPTable TableTargetMargin { get; set; }
    public string CompanyName { get; set; }
    public string QuoteNumber { get; set; }
    private PdfPCell Cell { get; set; }

    public void Create() {
        // Add Q if quoteID doesn't have one
        if (this.QuoteNumber.Substring(0, 1) != "Q") { this.QuoteNumber = "Q" + this.QuoteNumber; }
        this.CreateCompentTable();
        this.CreateDiscountTable();
        this.CreateTargetMarginTable();
        this.BeginPDF();
    }

    private void BeginPDF() {
        // Create the PDF Document
        // Get Instance for PdfWriter to output the file
        // MakePDF
        // Create fileName with Company name and append timestamp
        // Prompt save or open pdf message box
        Document pdfDoc = new Document(PageSize.A3.Rotate());
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        this.MakePDF(pdfDoc);
        string fileName = string.Join("_", (this.QuoteNumber + "_" + this.CompanyName + " " + DateTime.Now).Split(Path.GetInvalidFileNameChars())) + ".pdf";
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName.Replace(" ", "_") + "\"");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(pdfDoc);
        Response.End();
    }

    private void MakePDF(Document pdfDoc) {
        // Open the stream
        // Set HeaderRow to 1 for each table
        // Set default font Company Name and quoteNumber
        // Create Paragraph objects for Company Name, Quote Number, and date or PDF
        // Add space, Component Table, space, Discount Table, space, TargetMargin Table
        // Give PDF a Title
        // Close the stream
        pdfDoc.Open();
        this.TableComponent.HeaderRows = 1;
        this.TableDiscount.HeaderRows = 1;
        this.TableTargetMargin.HeaderRows = 1;
        iTextSharp.text.Font fdefault = FontFactory.GetFont("Verdana", 18, iTextSharp.text.Font.BOLD, new BaseColor(ColorTranslator.FromHtml("#AD9C65")));
        pdfDoc.Add(new Paragraph(this.CompanyName, fdefault));
        pdfDoc.Add(new Paragraph(this.QuoteNumber, fdefault));
        pdfDoc.Add(new Paragraph(DateTime.Now.ToShortDateString()));
        pdfDoc.Add(Chunk.NEWLINE);
        pdfDoc.Add(this.TableComponent);
        pdfDoc.Add(Chunk.NEWLINE);
        pdfDoc.Add(this.TableDiscount);
        pdfDoc.Add(Chunk.NEWLINE);
        pdfDoc.Add(this.TableTargetMargin);
        //pdfDoc.AddTitle("Quote - Margin Calculator");
        pdfDoc.AddTitle("Quote " + this.QuoteNumber + " " + this.CompanyName + " " + DateTime.Now);
        pdfDoc.Close();
    }

    private void CreateCompentTable() {
        // Create a table with 7 columns and assign to TableComponent
        // Create Header Cells and populate with corresponding cells on Gridview
        this.TableComponent = this.CreateTable(this.GVComponent, 100, new int[] { 50, 15, 100, 25, 25, 25, 25 });
        for (int colIndex = 0; colIndex < GVComponent.Columns.Count; colIndex++) {
            this.Cell = CreateHeaderCell(this.GVComponent.HeaderRow.Cells[colIndex].Text);
            this.TableComponent.AddCell(Cell);
        }
        // Export rows from GridView to table
        for (int rowIndex = 0; rowIndex < GVComponent.Rows.Count; rowIndex++) {
            if (GVComponent.Rows[rowIndex].RowType == DataControlRowType.DataRow) {
                ContinueWithCellText((this.GVComponent.Rows[rowIndex].FindControl("ltlComponentType") as Literal).Text, rowIndex, this.TableComponent, false);
                ContinueWithCellText((this.GVComponent.Rows[rowIndex].FindControl("ltlPartNumber") as Literal).Text, rowIndex, this.TableComponent, false);
                ContinueWithCellText((this.GVComponent.Rows[rowIndex].FindControl("ltlPartName") as Literal).Text, rowIndex, this.TableComponent, false);
                ContinueWithCellText(this.GetAccountingCost(rowIndex), rowIndex, this.TableComponent, true);
                ContinueWithCellText((this.GVComponent.Rows[rowIndex].FindControl("ltlStandardCost") as Literal).Text, rowIndex, this.TableComponent, true);
                ContinueWithCellText((this.GVComponent.Rows[rowIndex].FindControl("ltlComponentPrice") as Literal).Text, rowIndex, this.TableComponent, true);
                ContinueWithCellText((this.GVComponent.Rows[rowIndex].FindControl("ltlComponentMargin") as Literal).Text, rowIndex, this.TableComponent, true);
            }
        }
    }

    private void CreateDiscountTable() {
        // Create a table with 6 columns and assign to TableDiscount
        // Create Header Cells and populate with corresponding cells on Gridview
        // Align cells right align
        this.TableDiscount = this.CreateTable(this.GVDiscount, 60, new int[] { 15, 10, 10, 10, 10, 10 });
        for (int colIndex = 0; colIndex < this.GVDiscount.Columns.Count; colIndex++) {
            this.Cell = this.CreateHeaderCell(this.GVDiscount.HeaderRow.Cells[colIndex].Text);
            this.Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            this.TableDiscount.AddCell(Cell);
        }
        // Export rows from GridView to table
        for (int rowIndex = 0; rowIndex < this.GVDiscount.Rows.Count; rowIndex++) {
            if (this.GVDiscount.Rows[rowIndex].RowType == DataControlRowType.DataRow) {
                AddRow((this.GVDiscount.Rows[rowIndex].FindControl("ltlDescription") as Literal).Text, rowIndex, this.TableDiscount, false);
                AddRow((this.GVDiscount.Rows[rowIndex].FindControl("ltlQuotePercentage") as Literal).Text, rowIndex, this.TableDiscount, true);
                AddRow((this.GVDiscount.Rows[rowIndex].FindControl("ltlNoDiscount") as Literal).Text, rowIndex, this.TableDiscount, true);
                AddRow((this.GVDiscount.Rows[rowIndex].FindControl("ltlFirstDiscount") as Literal).Text, rowIndex, this.TableDiscount, true);
                AddRow((this.GVDiscount.Rows[rowIndex].FindControl("ltlSecondDiscount") as Literal).Text, rowIndex, this.TableDiscount, true);
                AddRow((this.GVDiscount.Rows[rowIndex].FindControl("ltlThirdDiscount") as Literal).Text, rowIndex, this.TableDiscount, true);
            }
        }
    }

    private void CreateTargetMarginTable() {
        // Create a table with 4 columns and assign to TableDiscount
        // Create Header Cells and populate with corresponding cells on Gridview
        // Align cells 2,3,4 right algin
        this.TableTargetMargin = this.CreateTable(this.GVTargetMargin, 45, new int[] { 1, 1, 1, 1 });
        for (int colIndex = 0; colIndex < this.GVTargetMargin.Columns.Count; colIndex++) {
            this.Cell = CreateHeaderCell(this.GVTargetMargin.HeaderRow.Cells[colIndex].Text);
            if (colIndex > 0) {
                Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            } else {
                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            this.TableTargetMargin.AddCell(Cell);
        }
        // Export rows from GridView to table
        for (int rowIndex = 0; rowIndex < this.GVTargetMargin.Rows.Count; rowIndex++) {
            if (this.GVTargetMargin.Rows[rowIndex].RowType == DataControlRowType.DataRow) {
                AddRow((this.GVTargetMargin.Rows[rowIndex].FindControl("ltlDescription") as Literal).Text, rowIndex, this.TableTargetMargin, false);
                AddRow((this.GVTargetMargin.Rows[rowIndex].FindControl("ltlTargetMarginPercent1") as Literal).Text, rowIndex, this.TableTargetMargin, true);
                AddRow((this.GVTargetMargin.Rows[rowIndex].FindControl("ltlTargetMarginPercent2") as Literal).Text, rowIndex, this.TableTargetMargin, true);
                AddRow((this.GVTargetMargin.Rows[rowIndex].FindControl("ltlTargetMarginPercent3") as Literal).Text, rowIndex, this.TableTargetMargin, true);
            }
        }
    }

    private string GetAccountingCost(int rowIndex) {
        if (rowIndex != GVComponent.Rows.Count - 1) {
            return Double.Parse((this.GVComponent.Rows[rowIndex].FindControl("txtAccountingCost") as TextBox).Text).ToString("C");
        } else {
            return (GVComponent.Rows[rowIndex].FindControl("ltlAccountingCost") as Literal).Text;
        }
    }

    private void ContinueWithCellText(string cellText, int rowIndex, PdfPTable table, bool rightAlign) {
        // If cell text contains "<", trim string
        // Continue with Adding row to table
        if (cellText.Contains("<")) {
            int i = cellText.IndexOf("<") + 3;
            cellText = cellText.Substring(i);
            if (cellText.Contains("<")) {
                i = cellText.IndexOf("<");
                cellText = cellText.Substring(0, i);
            }
        }
        AddRow(cellText, rowIndex, table, rightAlign);
    }

    private void AddRow(string cellText, int rowIndex, PdfPTable table, bool rightAlign) {
        // Create a new cell with column value
        // Assign alignment from passed argument
        PdfPCell cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont("PrepareForExport", 14)));
        if (rightAlign) {
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        } else {
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        }
        cell.VerticalAlignment = Element.ALIGN_MIDDLE;

        // Set Color of Alternating row
        if (rowIndex % 2 != 0) {
            cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F4EFE1"));
        }

        table.AddCell(cell);
    }

    private PdfPTable CreateTable(GridView gv, int wp, int[] i) {
        // Create a PdfPTable object
        // Set width of columns from passed argument
        // Return table
        PdfPTable table = new PdfPTable(gv.Columns.Count) {
            HorizontalAlignment = Element.ALIGN_LEFT,
            WidthPercentage = wp
        };
        table.SetWidths(i);
        return table;
    }

    private PdfPCell CreateHeaderCell(string s) {
        // If cell values is "&nbsp;" clear string
        // Return a new PdfPCell object
        if (s == "&nbsp;") { s = ""; }
        iTextSharp.text.Font NewFont = FontFactory.GetFont("Arial", 12, new BaseColor(ColorTranslator.FromHtml("#F4EFE1")));
        return new PdfPCell(new Phrase(s, NewFont)) {
            VerticalAlignment = Element.ALIGN_MIDDLE,
            // Set the background color for the header cell
            BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#450084"))
        };
    }
}