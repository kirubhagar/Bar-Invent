<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="PurchaseReturn.aspx.cs" Inherits="PurchaseReturn" %>

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
                    Purchase Return Manager <small>Create & manager your purchase return records....</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Purchase</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="PurchaseReturn.aspx">Purchase Return Manager</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>Purchase Return Manager</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <asp:MultiView ID="MVPurchaseReturn" runat="server" ActiveViewIndex="0">
                            <asp:View ID="View0" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <asp:LinkButton ID="LBAddNew" runat="server" CssClass="btn btn-info" OnClick="LBAddNew_Click">
                                            <i class="icon-plus"></i> Add New
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span10">
                                            <div class="span6">
                                                <div class="control-group">
                                                    <span class="control-label">From Date : </span>
                                                    <div class="controls">
                                                        <asp:TextBox ID="TBFromDate" runat="server" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CEFromDate" runat="server" Format="dd/MM/yyyy" TargetControlID="TBFromDate"
                                                            PopupButtonID="TBFromDate">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <div class="control-group">
                                                    <span class="control-label">To Date : </span>
                                                    <div class="controls">
                                                        <asp:TextBox ID="TBToDate" runat="server" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CEToDate" runat="server" Format="dd/MM/yyyy" TargetControlID="TBToDate"
                                                            PopupButtonID="TBToDate">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span2">
                                            <label class="checkbox">
                                                <asp:CheckBox ID="CBShowInDateRange" runat="server" />
                                                <span>Show In Date Range</span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <label class="checkbox span2">
                                            <asp:CheckBox ID="CBShowOnlyPending" runat="server" />
                                            <span>Show Only Draft</span>
                                        </label>
                                        <div class="span10 text-right">
                                            <asp:TextBox ID="TBSearch" runat="server" placeholder="Search Record"></asp:TextBox>
                                            <asp:LinkButton ID="LBSearch" runat="server" CssClass="btn btn-info">
                                                <i class="icon-search" style="line-height:18px;"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVPurchaseReturn" runat="server" Width="100%" ShowHeaderWhenEmpty="true"
                                            AutoGenerateColumns="false" HeaderStyle-Wrap="false" RowStyle-Wrap="false" CssClass="table table-hover table-advance table-condensed">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Doc Code" DataField="PRNumber" />
                                                <asp:BoundField HeaderText="Business Location" DataField="BusinessLocation" />
                                                <asp:BoundField HeaderText="Doc Date" DataField="DocDate" />
                                                <asp:BoundField HeaderText="RIE Enable" DataField="AllowRIE" />
                                                <asp:BoundField HeaderText="RIE Code" DataField="RIENumber" />
                                                <asp:BoundField HeaderText="Godown" DataField="Godown" />
                                                <asp:BoundField HeaderText="Supplier" DataField="Supplier" />
                                                <asp:BoundField HeaderText="Total Tax" DataField="TotalTax" />
                                                <asp:BoundField HeaderText="No Of Packages" DataField="NoOfPackages" />
                                                <asp:BoundField HeaderText="Sub Total" DataField="SubTotal" />
                                                <asp:BoundField HeaderText="Adjest Amount" DataField="AdjAmount" />
                                                <asp:BoundField HeaderText="Total Amount" DataField="TotalAmount" />
                                                <asp:BoundField HeaderText="Document Status" DataField="RecordStatus" />
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("POStatus").ToString() == "0" ? "Active" : Eval("POStatus").ToString() == "1" ? "Pending" : Eval("POStatus").ToString() == "2" ? "Completed" : ""%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Select Business Location : </span>
                                            <div class="controls">
                                                <asp:DropDownList ID="DDLBusinessLocation" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLBusinessLocation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <label class="checkbox">
                                            <asp:CheckBox ID="CBAllowRIENumber" runat="server" AutoPostBack="True" OnCheckedChanged="CBAllowRIENumber_CheckedChanged" />
                                            <span>Allow to return without RIE Number</span>
                                        </label>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Purchase Return Number : </span>
                                            <div class="controls MyLable">
                                                <asp:Label ID="LBLPRNumber" runat="server" Text="..."></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Date :</span>
                                            <div class="controls MyLable">
                                                <asp:Label ID="LBLDate" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Select Received Item Entry : </span>
                                            <div class="controls">
                                                <asp:DropDownList ID="DDLRecItemEntry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLRecItemEntry_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Barcode : </span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBBarcode" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Godown : </span>
                                            <div class="controls">
                                                <asp:DropDownList ID="DDLGodown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLGodown_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Item Name : </span>
                                            <div class="controls">
                                                <asp:DropDownList ID="DDLItemName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SelectItem">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Supplier : </span>
                                            <div class="controls">
                                                <asp:DropDownList ID="DDLSupplier" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Search Code : </span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBSearchCode" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span2">
                                            <label class="checkbox">
                                                <asp:CheckBox ID="CBShowImage" runat="server" />
                                                <span>Show Images</span>
                                            </label>
                                        </div>
                                        <div class="span3">
                                            <div class="control-group">
                                                <span class="control-label">Outstanding Balance : </span>
                                                <div class="controls MyLable">
                                                    <asp:Label ID="LBLOutBalance" runat="server" Text="0.00"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span2">
                                            <asp:LinkButton ID="LBAddAllItem" runat="server" CssClass="btn btn-info">
                                                Add All Items
                                            </asp:LinkButton>
                                        </div>
                                        <div class="span3">
                                            <label class="checkbox">
                                                <asp:CheckBox ID="CBAd" runat="server" />
                                                <span>Advance/Apply Towards Pending</span>
                                            </label>
                                        </div>
                                        <div class="span2">
                                            <label class="checkbox">
                                                <asp:CheckBox ID="CBSCreNote" runat="server" />
                                                <span>Supplier Credit Note </span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVPurchaseReturnInner" runat="server" CssClass="table table-hover table-advance"
                                            Width="100%" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" HeaderStyle-Wrap="false"
                                            RowStyle-Wrap="false">
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
                                                <asp:TemplateField HeaderText="Return Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TBReturnQuantity" runat="server" CssClass="input-mini" Text='<%# Eval("ReturnQty") %>'
                                                            onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Available Stock" DataField="AvStock" />
                                                <asp:BoundField HeaderText="Measuring Unit" DataField="MeasuringUnit" />
                                                <asp:BoundField HeaderText="Unit Price" DataField="UnitPrice" />
                                                <asp:BoundField HeaderText="SubTotalBTax" DataField="SubTotal" />
                                                <asp:BoundField HeaderText="CGST Per" DataField="CGSTPer" />
                                                <asp:BoundField HeaderText="CGST Value" DataField="CGSTValue" />
                                                <asp:BoundField HeaderText="SGST Per" DataField="SGSTPer" />
                                                <asp:BoundField HeaderText="SGST Value" DataField="SGSTValue" />
                                                <asp:BoundField HeaderText="Unit Price After Tax" DataField="UnitPriceATax" />
                                                <asp:BoundField HeaderText="Sub Total After Tax" DataField="SubTotalATax" />
                                                <asp:TemplateField HeaderText="Controls">
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
                                    <div class="span4">
                                        <asp:TextBox ID="TBNotes" runat="server" TextMode="MultiLine" placeholder="Enter Notes (If Any)"
                                            Style="width: 100%; height: 125px; resize: none;"></asp:TextBox>
                                    </div>
                                    <div class="span4">
                                        <div class="control-group">
                                            <span class="control-label">Total Tax Amount : </span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBTotalTaxAmount" runat="server" CssClass="input-medium text-right"
                                                    Enabled="false">0.00</asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">No Of Packages : </span>
                                            <div class="controls MyLable">
                                                <asp:Label ID="LBLNoOfPackages" runat="server" Text="255.00"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span4">
                                        <div class="control-group">
                                            <span class="control-label">Sub Total : </span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBSubTotal" runat="server" CssClass="input-medium text-right" Enabled="false">0.00</asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Adjustment Amount : </span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBAdjustmentAmount" runat="server" CssClass="input-medium text-right"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Total Amount : </span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBTotAmount" runat="server" CssClass="input-medium text-right" Enabled="false">0.00</asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid text-right">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-info" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-info" />
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
