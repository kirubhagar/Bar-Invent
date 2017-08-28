<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="ReceiveManager.aspx.cs" Inherits="ReceiveManager" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .completionList
        {
            border: solid 1px Gray;
            margin: 0px;
            padding: 3px;
            height: 120px;
            overflow: auto;
            background-color: #FFFFFF;
        }
        .listItem
        {
            color: #191919;
        }
        .itemHighlighted
        {
            background-color: #ADD6FF;
        }
    </style>
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
                    Receive Item Manager <small>Receive Item Using PO Or Manually....</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Purchase</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="ReceiveItem.aspx">Recieve Item Manager</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>Receive Item Manager</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <asp:MultiView ID="MVReceivedItem" runat="server" ActiveViewIndex="0">
                            <asp:View ID="View0" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <asp:LinkButton ID="LBAddNew" runat="server" CssClass="btn btn-info" OnClick="AddNewClick">
                                            <i class="icon-plus"></i> Add New
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group span3">
                                            <span class="control-label span4">From Date : </span>
                                            <asp:TextBox ID="TBFromDate" runat="server" placeholder="DD/MM/YYYY" CssClass="span7 offset1"></asp:TextBox>
                                            <asp:CalendarExtender ID="CEFromDate" runat="server" Format="dd/MM/yyyy" TargetControlID="TBFromDate"
                                                PopupButtonID="TBFromDate">
                                            </asp:CalendarExtender>
                                        </div>
                                        <div class="control-group span3">
                                            <span class="control-label span4">To Date : </span>
                                            <asp:TextBox ID="TBToDate" runat="server" placeholder="DD/MM/YYYY" CssClass="span7 offset1"></asp:TextBox>
                                            <asp:CalendarExtender ID="CEToDate" runat="server" Format="dd/MM/yyyy" TargetControlID="TBToDate"
                                                PopupButtonID="TBToDate">
                                            </asp:CalendarExtender>
                                        </div>
                                        <label class="checkbox span2">
                                            <asp:CheckBox ID="CBShowInDateRange" runat="server" AutoPostBack="True" OnCheckedChanged="CBShowInDateRange_CheckedChanged" />
                                            <span>Show In Date Range</span>
                                        </label>
                                        <div class="span4">
                                            <div class="input-append">
                                                <asp:TextBox ID="TBSearch" runat="server" placeholder="Search Record"></asp:TextBox>
                                                <span class="add-on">
                                                    <asp:LinkButton ID="LBSearch" runat="server" OnClick="LBSearch_Click">
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
                                                <asp:BoundField HeaderText="Bussiness Location" DataField="BusinessLocationName" />
                                                <asp:BoundField HeaderText="Received Doc No" DataField="RecNo" />
                                                <asp:BoundField HeaderText="Doc Date" DataField="RecDate" />
                                                <asp:BoundField HeaderText="PO Number" DataField="PoNo" />
                                                <asp:BoundField HeaderText="Supplier" DataField="Supp" />
                                                <asp:BoundField HeaderText="Godown" DataField="Name" />
                                                <asp:BoundField HeaderText="Receiving Employee" DataField="receiveBy" />
                                                <asp:BoundField HeaderText="No Of Item" DataField="TotalQty" />
                                                <asp:BoundField HeaderText="Total Values" DataField="SubTotal" />
                                                <asp:BoundField HeaderText="TotalCGST" DataField="TotalCGST" />
                                                <asp:BoundField HeaderText="TotalSGST" DataField="totalSGST" />
                                                <asp:BoundField HeaderText="TotalAmount" DataField="Amount" />
                                                <asp:TemplateField HeaderText="_______Controls_______">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LBEdit" runat="server" CssClass="btn btn-info" CommandName="EditRecord"
                                                            CommandArgument='<%# Eval("ID") %>'>
                                                            <i class="icon-pencil icon-white"></i>
                                                        </asp:LinkButton>&nbsp;
                                                        <asp:LinkButton ID="LBDelete" runat="server" CssClass="btn btn-danger" CommandName="DeleteRecord"
                                                            CommandArgument='<%# Eval("ID") %>'>
                                                            <i class="icon-trash icon-white"></i>
                                                        </asp:LinkButton>&nbsp;
                                                        <asp:LinkButton ID="LBStatus" runat="server" CssClass="btn btn-Success" CommandName="ChangeStatus"
                                                            CommandArgument='<%# Eval("ID") %>'>
                                                            <i class="icon-refresh icon-white"></i>
                                                        </asp:LinkButton>&nbsp;
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
                                                <span class="control-label">Recieved Item Entry No : </span>
                                                <div class="controls">
                                                    <asp:UpdatePanel ID="UPRIENO" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="LBLItemEntryNo" runat="server" CssClass="MyLable" Text="..."></asp:Label>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DDLBusinessLocation" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Company : </span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLCompany" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">NGO : </span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLBusinessLocation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLBusinessLocation_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="control-group" id="GroupBarcode" runat="server">
                                                <span class="control-label">Barcode : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBBarcode" runat="server" placeholder="Barcode" AutoPostBack="true"
                                                        OnTextChanged="ProductSelect"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="ACEBarcode" runat="server" TargetControlID="TBBarcode"
                                                        MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" ServiceMethod="AutoCompleteTBBarcode">
                                                    </asp:AutoCompleteExtender>
                                                </div>
                                            </div>
                                            <div class="control-group" id="GroupItemName" runat="server">
                                                <span class="control-label">Item Name : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBItemName" runat="server" placeholder="Item Name" AutoPostBack="true"
                                                        OnTextChanged="ProductSelect"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="ACE1" runat="server" TargetControlID="TBItemName" MinimumPrefixLength="1"
                                                        EnableCaching="true" CompletionSetCount="1" ServiceMethod="AutoCompleteTBItemName"
                                                        CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </asp:AutoCompleteExtender>
                                                </div>
                                            </div>
                                            <div class="control-group" id="GroupSearchCode" runat="server">
                                                <span class="control-label">SearchCode : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBSearchCode" runat="server" placeholder="Search Code" AutoPostBack="true"
                                                        OnTextChanged="ProductSelect"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="ACESearchCode" runat="server" TargetControlID="TBSearchCode"
                                                        MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" ServiceMethod="AutoCompleteTBSearchCode">
                                                    </asp:AutoCompleteExtender>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Supplier Invoice No : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBSupplierInvoiceNo" runat="server" placeholder="Supplier Invoice Number"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Supplier Invoice Date : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBSupplierInvoiceDate" runat="server" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CESupplierInvoiceDate" runat="server" Format="dd/MM/yyyy"
                                                        TargetControlID="TBSupplierInvoiceDate" PopupButtonID="TBSupplierInvoiceDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Date : </span>
                                                <div class="controls">
                                                    <asp:Label ID="LBLDate" runat="server" CssClass="MyLable"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Received On Date : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBReceivedOnDate" runat="server" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CEReceivedOnDate" runat="server" Format="dd/MM/yyyy" TargetControlID="TBReceivedOnDate"
                                                        PopupButtonID="TBReceivedOnDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="checkbox">
                                                    <asp:CheckBox ID="CBReceivedViaPO" runat="server" AutoPostBack="true" OnCheckedChanged="RecViaPOClick" />
                                                    <span>Received Item With Existing PO</span>
                                                </label>
                                            </div>
                                            <div class="control-group" id="GroupPO" runat="server">
                                                <span class="control-label">PO Number : </span>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="DDLPO" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ChangePOData">
                                                        </asp:DropDownList>
                                                        <span class="add-on">
                                                            <asp:LinkButton ID="LBGetPOItem" runat="server" CssClass="tooltips" data-placement="top"
                                                                data-original-title="Add All Item" OnClick="BindPOItem">
                                                                <i class="icon-ok-sign" style="line-height:20px; text-decoration:none;"></i>
                                                            </asp:LinkButton>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" id="GroupSupplier" runat="server">
                                                <span class="control-label">Supplier : </span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLSupplier" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Payment Terms : </span>
                                                <div class="controls">
                                                    <%--<asp:DropDownList ID="DDLPaymentTerms" runat="server">
                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Immediate Payment" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="With In 15 Days" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="With In 30 Days" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="After 30 Days" Value="4"></asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="TBPaymentTerms" runat="server" placeholder="Payment Terms"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Godown : </span>
                                                <div class="controls">
                                                    <asp:UpdatePanel ID="UPDGodown" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="DDLGodown" runat="server">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DDLBusinessLocation" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVPO" runat="server" CssClass="table table-hover table-advance"
                                            Width="100%" AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" HeaderStyle-Wrap="false"
                                            RowStyle-Wrap="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                        <asp:HiddenField ID="HFID" runat="server" Value='<%# Eval("ID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Image">
                                                    <ItemTemplate>
                                                        <asp:Image ID="ImgLogo" runat="server" Width="90px" ImageUrl='<%# Eval("Image") %>' /></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Barcode" DataField="Barcode" />
                                                <asp:BoundField HeaderText="ItemName" DataField="ItemName" />
                                                <asp:TemplateField HeaderText="Receive Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TBReceiveQuantity" runat="server" CssClass="input-mini" Text='<%# Eval("RecQty") %>'
                                                            onkeypress="return isNumberKey(event)" AutoPostBack="true" OnTextChanged="ChangeInnerGV"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Ordered Qty" DataField="OrderedQty" />
                                                <asp:BoundField HeaderText="Pending Qty" DataField="PendingQty" />
                                                <asp:BoundField HeaderText=" UOM" DataField="UOM" />
                                                <asp:BoundField HeaderText="Unit Price" DataField="unitprice" />
                                                <asp:BoundField HeaderText="Total Price" DataField="TotalPrice" />
                                                <asp:TemplateField HeaderText="Item Level Discount Percentage">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TBItemLevelDisPer" runat="server" CssClass="input-mini" Text='<%# Eval("ItemLevelDisPer") %>'
                                                            onkeypress="return isNumberKey(event)" AutoPostBack="true" OnTextChanged="ChangeInnerGV"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Level Discount Value">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TBItemLevelDisValue" runat="server" CssClass="input-mini" Text='<%# Eval("ItemLevelDisValue") %>'
                                                            onkeypress="return isNumberKey(event)" AutoPostBack="true" OnTextChanged="ChangeInnerGV"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="TaxableAmt" DataField="TaxableAmt" />
                                                <asp:TemplateField>
                                                    <HeaderStyle CssClass="no-padding" />
                                                    <ItemStyle CssClass="no-padding" />
                                                    <HeaderTemplate>
                                                        <table border="1" width="100%" style="border-collapse: collapse; margin: 0px; padding: 0px;
                                                            border: 0px; text-align: center;">
                                                            <tr>
                                                                <td colspan="2">
                                                                    CGST Tax
                                                                </td>
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
                                                        <table border="1" width="100%" style="border-collapse: collapse; margin: 0px; padding: 0px;
                                                            border: 0px; text-align: center;">
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
                                                        <table border="1" width="100%" style="border-collapse: collapse; text-align: center;
                                                            border: 0px;">
                                                            <tr>
                                                                <td colspan="2">
                                                                    SGST Tax
                                                                </td>
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
                                                        <table border="1" width="100%" style="border-collapse: collapse; text-align: center;
                                                            border: 0px;">
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
                                                <asp:BoundField HeaderText="Effective Rate Per Unit" DataField="UnitPriceATax" />
                                                <asp:TemplateField HeaderText="Controls">
                                                    <ItemTemplate>
                                                        <%--<asp:LinkButton ID="LBEdit" runat="server" CssClass="btn btn-info" CommandArgument='<%# Eval("ID") %>'
                                                            CommandName="EditRecord">
                                                                        <i class="icon-pencil icon-white"></i>
                                                        </asp:LinkButton>&nbsp;--%>
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
                                        <div class="span4">
                                            <div class="control-group">
                                                <span class="control-label span6">Number Of items : </span>
                                                <asp:Label ID="LBLNoOfPackages" runat="server" Text="0.00" CssClass="span6 MyLable"></asp:Label>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label span6">Receiving Employee : </span>
                                                <asp:DropDownList ID="DDLRecEmp" runat="server" CssClass="span6">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TBNote" runat="server" TextMode="MultiLine" placeholder="Enter Note For Receiving Details..."
                                                    Style="width: 94%; height: 140px; resize: none;"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="span4">
                                        </div>
                                        <%--<div class="span4">
                                            <div class="control-group">
                                                <label class="checkbox">
                                                    <span>Discount On Total Before Tax</span>
                                                    <asp:CheckBox ID="CBDisOnTotalBTax" runat="server" />
                                                </label>
                                            </div>
                                            <div class="control-group">
                                                <div class="span9">
                                                    <div class="row-fluid">
                                                        <div class="span7">
                                                            <label class="radio">
                                                                <asp:RadioButton ID="RBDisFlat" runat="server" GroupName="GroupDiscount" AutoPostBack="true"
                                                                    OnCheckedChanged="RBDiscount" />
                                                                <span>Discount Flat</span>
                                                            </label>
                                                        </div>
                                                        <div class="span5">
                                                            <asp:TextBox ID="TBDisFlat" runat="server" CssClass="input-small" placeholder="0.00"
                                                                onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row-fluid">
                                                        <div class="span7">
                                                            <label class="radio">
                                                                <asp:RadioButton ID="RBDisPer" runat="server" GroupName="GroupDiscount" AutoPostBack="true"
                                                                    OnCheckedChanged="RBDiscount" />
                                                                <span>Discount Percent</span>
                                                            </label>
                                                        </div>
                                                        <div class="span5">
                                                            <asp:TextBox ID="TBDisPer" runat="server" CssClass="input-small" placeholder="0.00"
                                                                onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="span3">
                                                    <asp:LinkButton ID="LBApplyDiscount" runat="server" CssClass="btn" Text="Apply Discount"></asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label span6">Invoice Discount Amount : </span>
                                                <asp:Label ID="LBLInvDisAmount" runat="server" Text="0.00" CssClass="span6" Style="line-height: 30px;"></asp:Label>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label span6">Curr. Adv. with Supplier : </span>
                                                <asp:Label ID="LBLCurrAdvWithSupplier" runat="server" Text="0.00" CssClass="span6"
                                                    Style="line-height: 30px;"></asp:Label>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label span6">Additional Charges : </span>
                                                <asp:TextBox ID="TBAdiCharges" runat="server" CssClass="span6 input-mini" placeholder="0.00"
                                                    onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </div>
                                            <div class="control-group">
                                                <label class="checkbox">
                                                    <asp:CheckBox ID="CBApplyForEffPrice" runat="server" />
                                                    <span>Apply For Effective Price</span>
                                                </label>
                                            </div>
                                        </div>--%>
                                        <div class="span4">
                                            <div class="control-group">
                                                <span class="control-label span8">Sub Total Before Tax : </span>
                                                <asp:TextBox ID="TBSubTotalBTax" runat="server" CssClass="span4 text-right" placeholder="0.00"
                                                    ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label span8">Total CGST Amount : </span>
                                                <asp:TextBox ID="TBTotalTaxAmount" runat="server" CssClass="span4 text-right" placeholder="0.00"
                                                    ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label span8">Total SGST Amount : </span>
                                                <asp:TextBox ID="TBSGST" runat="server" CssClass="span4 text-right" placeholder="0.00"
                                                    ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label span8">Total : </span>
                                                <asp:TextBox ID="TBSubTotal" runat="server" CssClass="span4 text-right" placeholder="0.00"
                                                    ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <%--<div class="control-group">
                                                <span class="control-label span8">Round Off Adjustment Amount : </span>
                                                <asp:TextBox ID="TBRoundOff" runat="server" CssClass="span4 text-right" placeholder="0.00"
                                                    onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </div>--%>
                                            <div class="control-group">
                                                <span class="control-label span8">Total Amount : </span>
                                                <asp:TextBox ID="TBTotalAmount" runat="server" CssClass="span4 text-right" placeholder="0.00"
                                                    ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <%--<div class="control-group">
                                                <span class="control-label span8">Use Supplier Advance Amount : </span>
                                                <asp:TextBox ID="TBSuppAdvAmount" runat="server" CssClass="span4 text-right" placeholder="0.00"
                                                    ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label span8">Grand Total : </span>
                                                <asp:TextBox ID="TBGrandTotal" runat="server" CssClass="span4 text-right" placeholder="0.00"
                                                    ReadOnly="true"></asp:TextBox>
                                            </div>--%>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid text-right">
                                    <asp:LinkButton ID="LBHold" runat="server" CssClass="btn btn-info" Visible="false"
                                        OnClick="BottomButtonEvent">
                                        <i class="icon-print icon-white"></i> Hold
                                    </asp:LinkButton>
                                    &nbsp;
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info" OnClick="BottomButtonEvent" />
                                    &nbsp;
                                    <asp:LinkButton ID="LBCancel" runat="server" CssClass="btn btn-info" OnClick="BottomButtonEvent">
                                        <i class="icon-remove icon-white"></i> Cancel
                                    </asp:LinkButton>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END PAGE CONTAINER-->
</asp:Content>
