<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Online Margin Calculator</title>
    <script src="Scripts/DefaultJS04091350.js"></script>
    <script type="text/javascript">
        function getQuoteID() {
            return "<%= txtQuoteID.Text %>";
        }
        function getQuantity() {
            return "<%= txtQuantity.Text %>";
        }
        function getDiscount() {
            return "<%= txtDiscount.Text %>";
        }
        function getURLString() {
            return '<%= Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.AbsolutePath + "?" %>';
        }
        function txtOnkeypress(event) {
            var char = event.which || event.keyCode;
            keyPress(event, char);
        }
        function eee() {
            document.getElementById("btnRecalculate").click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <asp:Label ID="lblTitle" runat="server" Text="Online Margin Calculator" Font-Size="XX-Large" Font-Bold="True" />
            </div>
            <br />
            <div>
                <asp:TextBox ID="txtQuoteID" runat="server" TabIndex="1" />
                <asp:Label ID="lblQuoteID" runat="server" Text=" - Quote ID " />
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="BtnSubmit_Click" TabIndex="5" CausesValidation="False" UseSubmitBehavior="False" />
                <asp:Button ID="btnResetCalculator" runat="server" Text="Reset Calculator" OnClick="BtnResetCalculator_Click" TabIndex="6" CausesValidation="False" UseSubmitBehavior="False" Visible="false" />
            </div>
            <div>
                <asp:TextBox ID="txtQuantity" runat="server" TabIndex="2" />
                <asp:Label ID="lblQuantity" runat="server" Text=" - Quantity" />
            </div>
            <div>
                <asp:TextBox ID="txtDiscount" runat="server" TabIndex="3" />
                <asp:Label ID="lblDiscountPercent" runat="server" Text=" - Discount Percent" />
            </div>
            <br />
            <div>
                <asp:Label ID="lblUpdate" runat="server" Text="The search defaults to quantity of 1 and discount of 0%"></asp:Label>
            </div>
            <div>
                <asp:Label ID="lblCompanyName" runat="server" Text="" Visible="false" Font-Size="Large" Font-Bold="True" />
            </div>
            <div>
                <asp:GridView ID="gvComponent" runat="server" OnRowDataBound="ComponentRowDataBound" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Component Type" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlComponentType" Text='<%# Eval("ComponentType") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part Number" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlPartNumber" Text='<%# Eval("PartNumber") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part Name" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlPartName" Text='<%# Eval("PartName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Accounting Cost" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlDollarSignAC" Text="$" />
                                <asp:TextBox runat="server"
                                    ID="txtAccountingCost"
                                    Text='<%# Convert2DecimalPlaces(Double.Parse(Eval("AccountingCost").ToString())) %>'
                                    Style="text-align: right; width: 6em"
                                    onKeyPress="javascript:txtOnkeypress(event);" />
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator1"
                                    runat="server"
                                    ErrorMessage="Must Be a Dollar Amount"
                                    Text="*"
                                    ControlToValidate="txtAccountingCost"
                                    ValidationExpression="^(?=.)\d*(\.\d{1,2})?$"
                                    ForeColor="Red" />
                                <asp:Literal runat="server" ID="ltlAccountingCost" Text='<%# ConvertCurrency(Double.Parse(Eval("AccountingCost").ToString())) %>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Standard Cost" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlStandardCost" Text='<%#ConvertCurrency(Double.Parse(Eval("StandardCost").ToString()))  %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Component Price" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlDollarSignCP" Text="$" />
                                <asp:TextBox runat="server"
                                    ID="txtComponentPrice"
                                    Text='<%# Convert2DecimalPlaces(Double.Parse(Eval("ComponentPrice").ToString())) %>'
                                    Style="text-align: right; width: 6em"
                                    onKeyPress="javascript:txtOnkeypress(event);" />
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator2"
                                    runat="server"
                                    ErrorMessage="Dollar Amount Needed"
                                    Text="*"
                                    ControlToValidate="txtComponentPrice"
                                    ValidationExpression="^(?=.)\d*(\.\d{1,2})?$"
                                    ForeColor="Red" />
                                <asp:Literal runat="server" ID="ltlComponentPrice" Text='<%# ConvertCurrency(Double.Parse(Eval("ComponentPrice").ToString())) %>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Component Margin" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlComponentMargin" Text='<%# ConvertCurrency(Double.Parse(Eval("ComponentMargin").ToString())) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <div>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" />
            </div>
            <div>
                <asp:Button ID="btnRecalculate" runat="server" Text="Recalculate" OnClick="BtnRecalculate_Click" Visible="false" UseSubmitBehavior="False" />
                <asp:Button ID="btnOriginalValues" runat="server" Text="Original Quote Values" Visible="false" UseSubmitBehavior="False" OnClick="BtnOriginalValues_Click" />
                <asp:CheckBox ID="chbDiscountWarranty" runat="server" Text="Discount Warranty?" Checked="false" Visible="false" UseSubmitBehavior="False" OnClick="javascript:document.getElementById('btnRecalculate').click();" />
            </div>
            <br />
            <div>
                <asp:GridView
                    ID="gvDiscount"
                    runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlDescription" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Per Quote" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlQuotePercentage" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlNoDiscount" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlFirstDiscount" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlSecondDiscount" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlThirdDiscount" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <div>
                <asp:GridView
                    ID="gvTargetMargin"
                    runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Target Margin %" HeaderStyle-Font-Bold="true">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlDescription" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlTargetMarginPercent1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlTargetMarginPercent2" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlTargetMarginPercent3" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <div>
                <asp:Button ID="btnCopyURL" runat="server" Text="Copy URL" OnClientClick="javascript:copyToClipboard()" Visible="False" CausesValidation="False" UseSubmitBehavior="False" />
            </div>
            <br />
            <div>
                <asp:Button ID="btnPDF" runat="server" Text="PDF" OnClick="BtnPDF_Click" UseSubmitBehavior="False" Visible="false" />
            </div>
        </div>
    </form>
</body>
</html>
