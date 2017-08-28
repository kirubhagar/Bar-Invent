<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="RawMaterial.aspx.cs" Inherits="RawMaterial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="MyControls/CategoryUser.ascx" TagName="CategoryUser" TagPrefix="uc1" %>
<%@ Register Src="MyControls/Supplier.ascx" TagName="Supplier" TagPrefix="uc2" %>
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
                <!-- Alert Message Section-->
                <div id="alert_container">
                </div>
                <!-- END Alert Message Section-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                    Raw Material <small>Create & Manage Raw Material Details... </small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="#"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Material Manager</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="RawMaterial.aspx">Raw Material</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>Raw Materials</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <asp:MultiView ID="MVRawMatrial" runat="server" ActiveViewIndex="0">
                            <asp:View ID="View0" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span4">
                                            <asp:LinkButton ID="LBAddNew" runat="server" CssClass="btn btn-info" OnClick="LBAddNew_Click">
                                                <i class="icon-plus"></i>  Add New
                                            </asp:LinkButton>&nbsp;
                                        </div>
                                        <div class="span4">
                                            <div class="input-append">
                                                <asp:TextBox ID="TBSearch" runat="server" placeholder="Type Item Name To Search"></asp:TextBox>
                                                <span class="add-on">
                                                    <asp:LinkButton ID="LBSearch" runat="server" Font-Underline="false" OnClick="LBSearch_Click"
                                                        ValidationGroup="Search">
                                                        <i class="icon-search" style="line-height:20px;"></i>
                                                    </asp:LinkButton>
                                                </span>
                                            </div>
                                            <asp:RequiredFieldValidator ID="RFVSearch" CssClass="help-inline" runat="server"
                                                ErrorMessage="Details Required!" ForeColor="#dc5d3a" ControlToValidate="TBSearch"
                                                ValidationGroup="Search" SetFocusOnError="true">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                        <div class="span4">
                                            <label class="checkbox">
                                                <asp:CheckBox ID="CBShowImage" runat="server" Checked="true" AutoPostBack="true"
                                                    OnCheckedChanged="CBShowImage_CheckedChanged" />
                                                <span>Show Images</span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVRawMaterial" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" HeaderStyle-Font-Bold="true"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" CellPadding="10"
                                            RowStyle-VerticalAlign="Middle" ShowHeaderWhenEmpty="true" PageSize="10" OnPageIndexChanging="GVRawMaterial_PageIndexChanging"
                                            OnRowCommand="GVRawMaterial_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Image">
                                                    <ItemTemplate>
                                                        <asp:Image ID="ImgLogo" runat="server" Style="max-width: 80px;" ImageUrl='<%# Eval("Image") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Name" DataField="Name" />
                                                <asp:BoundField HeaderText="Barcode" DataField="Barcode" />
                                                <asp:BoundField HeaderText="Measuring Unit" DataField="UOM" />
                                                <asp:BoundField HeaderText="Category" DataField="Category" />
                                                <asp:BoundField HeaderText="Purchase Price" DataField="PurPrice" />
                                                <asp:BoundField HeaderText="Sale Price" DataField="SalePrice" />
                                                <asp:BoundField HeaderText="Created On" DataField="CreatedOn" />
                                                <asp:BoundField HeaderText="Created By" DataField="CreatedBy" />
                                                <asp:BoundField HeaderText="Description" DataField="Description" />
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status").ToString() == "True" ? "Active" : "InActive" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="_____Controls_____">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LBEdit" runat="server" CssClass="btn btn-primary tooltips" data-placement="top"
                                                            data-original-title="Edit Record" CommandName="EditRecord" CommandArgument='<%# Eval("ID") %>'>
                                                            <i class="icon-pencil icon-white"></i>
                                                        </asp:LinkButton>&nbsp;
                                                        <asp:LinkButton ID="LBDelete" runat="server" CssClass="btn btn-danger tooltips" data-placement="top"
                                                            data-original-title="Delete Record" CommandName="DeleteRecord" CommandArgument='<%# Eval("ID") %>'
                                                            OnClientClick="return confirm('Are you sure?');">
                                                            <i class="icon-remove icon-white"></i>
                                                        </asp:LinkButton>&nbsp;
                                                        <asp:LinkButton ID="LBChangeStatus" runat="server" CssClass="btn btn-warning tooltips"
                                                            data-placement="top" data-original-title="Change Status" CommandName="ChangeStatus"
                                                            CommandArgument='<%# Eval("ID") %>'>
                                                            <i class="icon-refresh icon-white"></i>
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
                                                <span class="control-label">Category : </span>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="DDLCat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDlCat_SelectedIndexChang">
                                                        </asp:DropDownList>
                                                        <span class="add-on">
                                                            <asp:LinkButton ID="LBAddCategory" runat="server" CssClass="tooltips" data-placement="top"
                                                                data-original-title="Insert New Category Records" Font-Underline="false">
                                                                <i class="icon icon-plus" style="line-height:20px;"></i>
                                                            </asp:LinkButton>
                                                        </span>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="RFVDDlCat" runat="server" Display="Dynamic" ErrorMessage="Required!"
                                                        ControlToValidate="DDLCat" InitialValue="0" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationGroup="Submit">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">HSN Code : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBHSN" runat="server" placeholder="HSNCode" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Name : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBName" runat="server" placeholder="Name"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Barcode : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBBarcode" runat="server" placeholder="Barcode"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Product Code : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBProductCode" runat="server" placeholder="Product Code"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Measuring Unit : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBUOM" runat="server" placeholder="Measuring Unit"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Purchase Price : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBPurPrice" runat="server" placeholder="Purchase Price"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Sale Price : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBSalePrice" runat="server" placeholder="SalePrice"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">MFG Date : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBMFGDate" runat="server" placeholder="MFG Date (If Any)"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="TBMFGDate"
                                                        PopupButtonID="TBMFGDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Expire Date : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBExpireDate" runat="server" placeholder="Expire Date (If Any)"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="TBExpireDate"
                                                        PopupButtonID="TBExpireDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Opening Stock : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBOpeningStock" runat="server" placeholder="Opening Stock"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Opening Stock On : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBOSDate" runat="server" placeholder="Opening Stock Date"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy" TargetControlID="TBOSDate"
                                                        PopupButtonID="TBOSDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Reorder Qty : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBReorderQty" runat="server" placeholder="Reorder Qty"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Bin Location : </span>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="TBBinLocation" runat="server" placeholder="#_._._._._" Enabled="false"></asp:TextBox>
                                                        <span class="add-on">
                                                            <asp:LinkButton ID="LBMapBinLocation" runat="server" CssClass="tooltips" data-placement="top"
                                                                data-original-title="Map Bin Location with this Item" Font-Underline="false">
                                                                <i class="icon icon-map-marker" style="line-height:20px;"></i>
                                                            </asp:LinkButton>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Preferred Supplier : </span>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="DDLSupplier" runat="server">
                                                        </asp:DropDownList>
                                                        <span class="add-on">
                                                            <asp:LinkButton ID="LBAddSupplier" runat="server" CssClass="tooltips" data-placement="top"
                                                                data-original-title="Insert New Supplier Records" Font-Underline="false">
                                                                <i class="icon icon-plus" style="line-height:20px;"></i>
                                                            </asp:LinkButton>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Material Image : </span>
                                                <div class="controls">
                                                    <asp:FileUpload ID="FULogo" runat="server" onchange="showpreview(this);" Style="top: 0;
                                                        display: block;" />
                                                    <img id="imgpreview" alt="logo" src="" style="border-width: 0px; visibility: hidden;
                                                        height: 100px;" />
                                                    <asp:Image ID="ImgLogo" runat="server" Width="100px" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group">
                                            <span class="control-label">Description : </span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBDescription" runat="server" Width="95%" Height="120px" Style="resize: none;"
                                                    placeholder="Material Description..." TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid text-right">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                        ValidationGroup="Submit" OnClick="btnSubmit_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                        OnClick="btnCancel_Click" />
                                </div>
                                <!-- Models For Category-->
                                <asp:ModalPopupExtender ID="MPE1" BehaviorID="mpe1" runat="server" PopupControlID="PnlCatMaster"
                                    TargetControlID="LBAddCategory" BackgroundCssClass="modalBackground" CancelControlID="ModelCatCancel">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="PnlCatMaster" runat="server" CssClass="pnlBackGround" Style="display: none">
                                    <div class="modal span10 offset1" style="left: 0;">
                                        <div class="modal-header">
                                            <h3 id="H4">
                                                Category Details</h3>
                                        </div>
                                        <div class="modal-body">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <uc1:CategoryUser ID="CategoryUser1" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="ModelCatSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                                OnClick="ModelSubmitBtnClickEvent" />
                                            <asp:Button ID="ModelCatCancel" runat="server" Text="Close" CssClass="btn btn-danger" />
                                        </div>
                                    </div>
                                </asp:Panel>
                                <!--Models For Bin Location-->
                                <asp:ModalPopupExtender ID="MPE2" BehaviorID="mpe2" runat="server" PopupControlID="PnlBinMaster"
                                    TargetControlID="LBMapBinLocation" BackgroundCssClass="modalBackground" CancelControlID="ModelBinCancel">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="PnlBinMaster" runat="server" CssClass="pnlBackGround" Style="display: none">
                                    <div class="modal span10 offset1" style="left: 0;">
                                        <div class="modal-header">
                                            <h3 id="H1">
                                                Bin Details</h3>
                                        </div>
                                        <div class="modal-body">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <div class="widget">
                                                        <div class="widget-title">
                                                            <h4>
                                                                <i class="icon-globe"></i>Bin Location Manager</h4>
                                                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                                        </div>
                                                        <div class="widget-body">
                                                            <div class="row-fluid">
                                                                <div class="span12">
                                                                    <div class="span6">
                                                                        <div class="control-group">
                                                                            <span class="control-label">Select Godown</span>
                                                                            <div class="controls">
                                                                                <asp:DropDownList ID="DDLGodown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLWarehouse_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="span6">
                                                                        <div class="control-group">
                                                                            <span class="control-label">Select Zone</span>
                                                                            <div class="controls">
                                                                                <asp:DropDownList ID="DDLZone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLZone_SelectedIndexChanged">
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
                                                                            <span class="control-label">Select Area</span>
                                                                            <div class="controls">
                                                                                <asp:DropDownList ID="DDLArea" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLArea_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="span6">
                                                                        <div class="control-group">
                                                                            <span class="control-label">Select Row</span>
                                                                            <div class="controls">
                                                                                <asp:DropDownList ID="DDLRow" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLRow_SelectedIndexChanged">
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
                                                                            <span class="control-label">Select Shelf</span>
                                                                            <div class="controls">
                                                                                <asp:DropDownList ID="DDLShelf" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLShelf_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="span6">
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <hr />
                                                            <div class="row-fluid text-center">
                                                                <div class="control-group">
                                                                    <span class="control-label">Bin Location</span>
                                                                    <div class="controls">
                                                                        <asp:TextBox ID="TBModelBinLocation" runat="server" Enabled="false" placeholder="#_._._._._"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="ModelBinSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                                OnClick="ModelSubmitBtnClickEvent" />
                                            <asp:Button ID="ModelBinCancel" runat="server" Text="Close" CssClass="btn btn-danger" />
                                        </div>
                                    </div>
                                </asp:Panel>
                                <!-- Models For Supplier-->
                                <asp:ModalPopupExtender ID="MPE3" BehaviorID="mpe3" runat="server" PopupControlID="PnlSupplierMaster"
                                    TargetControlID="LBAddSupplier" BackgroundCssClass="modalBackground" CancelControlID="ModelSupplierCancel">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="PnlSupplierMaster" runat="server" CssClass="pnlBackGround" Style="display: none">
                                    <div class="modal span10 offset1" style="left: 0;">
                                        <div class="modal-header">
                                            <h3 id="H2">
                                                Supplier Details</h3>
                                        </div>
                                        <div class="modal-body">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <uc2:Supplier ID="Supplier1" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="ModelSupplierSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                                OnClick="ModelSubmitBtnClickEvent" />
                                            <asp:Button ID="ModelSupplierCancel" runat="server" Text="Close" CssClass="btn btn-danger" />
                                        </div>
                                    </div>
                                </asp:Panel>
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
