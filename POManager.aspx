<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="POManager.aspx.cs" Inherits="POManager" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function ShowMessage(message, messagetype) {
            var cssclass;
            switch (messagetype) {
                case 'Success':
                    cssclass = 'alert-success'
                    break;
                case 'Error':
                    cssclass = 'alert-danger'
                    break;
                case 'Warning':
                    cssclass = 'alert-warning'
                    break;
                default:
                    cssclass = 'alert-info'
            }
            $('#alert_container').append('<div id="alert_div" style="margin: 0 0.5%; -webkit-box-shadow: 3px 4px 6px #999;" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>' + messagetype + '!</strong> <span>' + message + '</span></div>');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN PAGE CONTAINER-->
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <div id="alert_container">
                </div>
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                    PO Manager <small>Create & Manage PO (Purchase Order)...</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Purchase</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="POManager.aspx">PO Manager</a><span class="divider-last">&nbsp;</span></li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <div class="widget">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-globe"></i>PO Manager</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <asp:MultiView ID="MVPO" runat="server" ActiveViewIndex="0">
                            <asp:View ID="View0" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <asp:LinkButton ID="LBAddNew" runat="server" CssClass="btn btn-info" OnClick="LBAddNew_Click"><i class="icon-plus"></i> Add</asp:LinkButton>
                                            <asp:LinkButton ID="LBExportToExcel" runat="server" CssClass="btn btn-info" OnClick="LBExportToExcel_Click"><i class="icon-download-alt"></i> Export</asp:LinkButton>
                                            <asp:LinkButton ID="LBUploadPO" runat="server" CssClass="btn btn-info" OnClick="LBUploadPO_Click"><i class="icon-upload-alt"></i> Upload</asp:LinkButton>
                                        </div>
                                        <div class="span6">
                                            <div class="input-append">
                                                <asp:TextBox ID="TBSearch" runat="server" placeholder="Search Record"></asp:TextBox>
                                                <span class="add-on">
                                                    <asp:LinkButton ID="LBSearch" runat="server" OnClick="SearchEvent">
                                                        <i class="icon-search" style="line-height:18px; text-decoration:none;"></i>
                                                    </asp:LinkButton>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVPOMaster" runat="server" Width="100%" ShowHeaderWhenEmpty="true"
                                            AutoGenerateColumns="false" HeaderStyle-Wrap="false" RowStyle-Wrap="false" CssClass="table table-hover table-advance table-condensed"
                                            OnRowCommand="GVPOMaster_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="PO No" DataField="PONo" />
                                                <asp:BoundField HeaderText="PO Date" DataField="PODate" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" />
                                               <%-- <asp:BoundField HeaderText="Business Location" DataField="BusinessLocationName" Visible="false" />--%>
                                                <asp:BoundField HeaderText="Delivary Date" DataField="DeliveryDate" HtmlEncode="false"
                                                    DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField HeaderText="Delivar To" DataField="DeliverTo" Visible="false" />
                                                <asp:BoundField HeaderText="Supplier" DataField="Name" />
                                                <asp:BoundField HeaderText="Supplier Refrence" DataField="SupplierRef" Visible="false" />
                                                <asp:BoundField HeaderText="Terms & Condition" DataField="TermsCondition" Visible="false" />
                                                <asp:BoundField HeaderText="Payment Terms" DataField="PaymentTerms" Visible="false" />
                                                <asp:BoundField HeaderText="Total Quantity" DataField="TotalQty" />
                                                <asp:BoundField HeaderText="Total Values" DataField="TotalValue" />
                                                <asp:BoundField HeaderText="Remark" DataField="Remark" Visible="false" />
                                                <asp:TemplateField HeaderText="POStatus">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("POStatus").ToString() == "0" ? "Active" : Eval("POStatus").ToString() == "1" ? "Pending" : Eval("POStatus").ToString() == "2" ? "Completed" : ""%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Controls">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LBEdit" runat="server" CssClass="btn btn-info" CommandName="EditRecord"
                                                            CommandArgument='<%# Eval("ID") %>'>
                                                            <i class="icon-pencil icon-white"></i>
                                                        </asp:LinkButton>&nbsp;
                                                        <asp:LinkButton ID="LBDelete" runat="server" CssClass="btn btn-danger" CommandName="DeleteRecord"
                                                            CommandArgument='<%# Eval("ID") %>'>
                                                            <i class="icon-trash icon-white"></i>
                                                        </asp:LinkButton>&nbsp;
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-Success" CommandName="PrintPO"
                                                            CommandArgument='<%# Eval("ID") %>'>
                                                            <i class="icon-print icon-white"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </asp:View>
                            <asp:View ID="View1" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">PO Number</span>
                                                <div class="controls MyLable">
                                                    <asp:UpdatePanel ID="UPPONo" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="LBLPONo" runat="server" Text="..."></asp:Label>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DDLRetailOutlet" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">PO Date</span>
                                                <div class="controls MyLable">
                                                    <asp:Label ID="LBLPODate" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Select Company : </span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLCompany" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Select NGO : </span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLRetailOutlet" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Delivery Date</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBDeliveryDate" runat="server" placeholder="dd//mm/yyyy"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CEFDeliveryDate" runat="server" Format="dd/MM/yyyy" TargetControlID="TBDeliveryDate"
                                                        PopupButtonID="TBDeliveryDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Deliver To</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLDeliverTo" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Supplier</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLSupplier" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SuppChangeEvent">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Barcode : </span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBBarcode" runat="server" placeholder="Barcode" AutoPostBack="true"
                                                    OnTextChanged="DDLItemList_SelectedIndexChanged"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Item : </span>
                                            <div class="controls">
                                                <asp:DropDownList ID="DDLItemList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLItemList_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVPO" runat="server" CssClass="table table-hover table-advance"
                                            Width="100%" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" HeaderStyle-Wrap="false"
                                            RowStyle-Wrap="false" OnRowCommand="GVPO_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                        <asp:HiddenField ID="HFID" runat="server" Value='<%# Eval("ID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                <asp:BoundField HeaderText="Barcode" DataField="Barcode" />
                                                <asp:BoundField HeaderText="HSNCode" DataField="HSNCode" />
                                                <asp:BoundField HeaderText="UOM" DataField="UOM" />
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TBItemQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="input-mini"
                                                            AutoPostBack="true" OnTextChanged="BindInnerData2"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Unit Price" DataField="UnitPrice" />
                                                <asp:BoundField HeaderText="Total Price (Initial)" DataField="TotalPriceInitial" />
                                                <asp:TemplateField HeaderText="Discount Percentage" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TBDisPer" runat="server" Text='<%# Eval("DiscountPer") %>' CssClass="input-mini"
                                                            AutoPostBack="true" OnTextChanged="BindInnerData2"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Discount Value" DataField="DiscountValue" Visible="false" />
                                                <asp:BoundField HeaderText="Unit Price (With Discount)" DataField="UnitPriceWithDiscount"
                                                    Visible="false" />
                                                <asp:BoundField HeaderText="Total Price (With Discount)" DataField="TotalPriceWithDiscount"
                                                    Visible="false" />
                                                <%--  <asp:BoundField HeaderText="Tax  Name" DataField="TaxName" />--%>
                                                <asp:TemplateField>
                                                    <HeaderStyle CssClass="no-padding" />
                                                    <ItemStyle CssClass="no-padding" />
                                                    <HeaderTemplate>
                                                        <table border="1" width="100%" style="border-collapse:collapse; margin:0px; padding:0px; border:0px; text-align:center;">
                                                            <tr>
                                                                <td colspan="2">CGST Tax</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Per(%)
                                                                </td>
                                                                <td>
                                                                    Value
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table border="1" width="100%" style="border-collapse:collapse; margin:0px; padding:0px; border:0px; text-align:center;">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LBLTaxPe1r" runat="server" Text='<%# Eval("CGSTPer") %>'></asp:Label>
                                                                </td>
                                                                
                                                                <td>
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("CGSTValue") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderStyle CssClass="no-padding" />
                                                    <ItemStyle CssClass="no-padding" />
                                                    <HeaderTemplate>
                                                        <table border="1" width="100%" style="border-collapse:collapse; text-align:center; border:0px;">
                                                            <tr>
                                                                <td colspan="2">SGST Tax</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Per(%)
                                                                </td>
                                                                <td>
                                                                   Value
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table border="1" width="100%" style="border-collapse:collapse; text-align:center; border:0px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LBLTaxPer2" runat="server" Text='<%# Eval("SGSTPer") %>'></asp:Label>
                                                                </td>
                                                                
                                                                <td>
                                                                    <asp:Label ID="Label12" runat="server" Text='<%# Eval("SGSTValue") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                             
                                                <asp:BoundField HeaderText="Unit Price (With Tax)" DataField="UnitPriceWithTaxDiscount" />
                                                <asp:BoundField HeaderText="Total Price (With Tax & Discount)" DataField="TotalPriceWithTaxDiscount"
                                                    Visible="false" />
                                                <asp:BoundField HeaderText="Effective Rate Per Unit" DataField="EffectiveRatePerUnit" />
                                                <asp:BoundField HeaderText="GrandTotal" DataField="GrandTotal" />
                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LBDelete" runat="server" CssClass="btn btn-danger" CommandArgument='<%# Eval("ID") %>'
                                                            CommandName="DeleteRecord">
                                                                        <i class="icon-remove icon-white"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Total Quantity</span>
                                                <div class="controls MyLable">
                                                    <asp:Label ID="LBLTotalQuantity" runat="server" Text="0.0"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Total Values</span>
                                                <div class="controls MyLable">
                                                    <asp:Label ID="LBLTotalValues" runat="server" Text="0.00"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Supplier Refrence</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBSupplierRefrence" runat="server" placeholder="Supplier Refrence"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Payment Terms</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBPaymentTerms" runat="server" placeholder="Payment Terms"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group text-center">
                                                <asp:TextBox ID="TBTermsCondition" runat="server" placeholder="Terms & Conditions"
                                                    TextMode="MultiLine" Style="width: 75%; height: 90px; resize: none;"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group text-center">
                                                <asp:TextBox ID="TBRemarks" runat="server" TextMode="MultiLine" placeholder="Remarks"
                                                    Style="width: 75%; height: 90px; resize: none;"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid text-right">
                                    <asp:LinkButton ID="LBSubmitPrint" runat="server" CssClass="btn btn-info" Visible="false"
                                        OnClick="BottomBtnControls">
                                        <i class="icon-print icon-white"></i> Submit & Print
                                    </asp:LinkButton>
                                    &nbsp;
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info" OnClick="BottomBtnControls" />
                                    &nbsp;
                                    <asp:LinkButton ID="LBCancel" runat="server" CssClass="btn btn-info" OnClick="BottomBtnControls">
                                        <i class="icon-remove icon-white"></i> Cancel
                                    </asp:LinkButton>
                                </div>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group">
                                            <span class="control-label">Download Sample File : </span>
                                            <div class="controls">
                                                <asp:LinkButton ID="LBFile" runat="server" OnClick="LBFile_Click" Style="text-decoration: none;
                                                    line-height: 30px;">
                                                    <i class="icon-file" ></i>  File Formate
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Upload Your File : </span>
                                            <div class="controls">
                                                <asp:FileUpload ID="FUPO" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid text-right">
                                    <asp:Button ID="BtnUploadPOSubmit" runat="server" Text="Upload" CssClass="btn btn-success"
                                        OnClick="ModelSubmitCancel" />
                                    <asp:Button ID="BtnUploadPOCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                        OnClick="ModelSubmitCancel" />
                                </div>
                            </asp:View>
                        </asp:MultiView>
                        <div id="dd" runat="server" visible="false">
                            <div id="PrintPODocument" runat="server">
                                <table width="100%" style="font-size: 10px;">
                                    <tr>
                                        <td>
                                            <h4>
                                                <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                                            </h4>
                                            <br />
                                            <asp:Label ID="lblCompanyAddressLine1" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblCompanyAddressLine2" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblCompanyCity" runat="server"></asp:Label>
                                            <asp:Label ID="lblCompanyState" runat="server"></asp:Label>
                                            <asp:Label ID="lblCompanyPin" runat="server"></asp:Label><br />
                                            Phone No :
                                            <asp:Label ID="lblCompanyPhon" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <h2>
                                                Purchase Order</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            The following number must appear on all related correspondence, shipping papers,
                                            and invoices.<br />
                                            <h4>
                                                P.O. NUMBER : &nbsp;
                                                <asp:Label ID="lblPONumber" runat="server"></asp:Label></h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            To :
                                            <br />
                                            <asp:Label ID="lblToName" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblToCompany" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblToAddress1" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblToAddress2" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblToCity" runat="server"></asp:Label>
                                            <asp:Label ID="lblToState" runat="server"></asp:Label>
                                            <asp:Label ID="lblToZip" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblToPhon" runat="server"></asp:Label><br />
                                        </td>
                                        <td>
                                            Ship To:
                                            <br />
                                            <asp:Label ID="lblShipName" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblShipCompany" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblShipAddress1" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblShipAddress2" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblShipCity" runat="server"></asp:Label>
                                            <asp:Label ID="lblShipState" runat="server"></asp:Label>
                                            <asp:Label ID="lblShipZip" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblShipPhon" runat="server"></asp:Label><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%" border="1">
                                                <tr>
                                                    <td>
                                                        P.O. DATE
                                                    </td>
                                                    <td>
                                                        REQUISITIONER
                                                    </td>
                                                    <td>
                                                        SHIPPED VIA
                                                    </td>
                                                    <td>
                                                        F.O.B. POINT
                                                    </td>
                                                    <td>
                                                        TERMS
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPrintPODate" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRequisitioner" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblShippedVia" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFOBPoint" runat="server"> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTerms" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%" border="1">
                                                <tr>
                                                    <td>
                                                        S.No.
                                                    </td>
                                                    <td>
                                                        QTY
                                                    </td>
                                                    <td>
                                                        Item Description
                                                    </td>
                                                    <td>
                                                        UNIT PRICE
                                                    </td>
                                                    <td>
                                                        TOTAL
                                                    </td>
                                                </tr>
                                                <asp:Repeater ID="GVInvPrint" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <%# Container.ItemIndex + 1 %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("Quantity") %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("ProductID") %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("UnitPriceWithTaxDiscount") %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("TotalPriceWithTaxDiscount") %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            SUBTOTAL : &nbsp; &nbsp; &nbsp;
                                            <asp:Label ID="lblSubtotal" runat="server" Text="0.00"></asp:Label><br />
                                            SALES TAX : &nbsp; &nbsp; &nbsp; &nbsp;
                                            <asp:Label ID="lblSalesTax" runat="server" Text="0.00"></asp:Label><br />
                                            SHIPPING & HANDLING : &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            <asp:Label ID="lblShipping" runat="server" Text="0.00"></asp:Label><br />
                                            OTHER : &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            <asp:Label ID="lblOther" runat="server" Text="0.00"></asp:Label><br />
                                            Total : &nbsp; &nbsp; &nbsp;
                                            <asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p id="DetailID" runat="server">
                                            </p>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="center">
                                            Authorized by Date
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END PAGE CONTAINER-->
</asp:Content>
