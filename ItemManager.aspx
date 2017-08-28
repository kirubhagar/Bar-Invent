<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="ItemManager.aspx.cs" Inherits="ItemManager" %>

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
                <!-- Alert Message Section-->
                <div id="alert_container">
                </div>
                <!-- END Alert Message Section-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                    Item Manager <small>Manage Item Stock Position</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Material Manager</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="#">Item Manager</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>Item Manager (Opening Stock, Reorder Quantity & Bin Locations)</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="row-fluid">
                            <div class="span12">
                                <div class="span6">
                                    <div class="control-group">
                                        <span class="control-label">Company : </span>
                                        <div class="controls">
                                            <asp:DropDownList ID="DDLCompany" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLCompany_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <span class="control-label">NGO : </span>
                                        <div class="controls">
                                            <asp:DropDownList ID="DDLNGO" runat="server">
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
                                        <span class="control-label">Godown :</span>
                                        <div class="controls">
                                            <asp:DropDownList ID="DDLGodown" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <span class="control-label">Item Type : </span>
                                        <div class="controls">
                                            <asp:DropDownList ID="DDLItemType" runat="server">
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Raw Material" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Kit Material" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Finish Material" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="row-fluid text-right">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-success"
                                OnClick="btnSearch_Click" />
                        </div>
                        <hr />
                        <div class="row-fluid">
                            <div class="span12" style="overflow-x: auto;">
                                <asp:UpdatePanel ID="UP1" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="GVItem" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" HeaderStyle-Wrap="false"
                                            HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                            CellPadding="10" RowStyle-VerticalAlign="Middle" ShowHeaderWhenEmpty="true" OnRowCommand="GVItem_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkHeader" runat="server" AutoPostBack="true" OnCheckedChanged="chkHeader_CheckedChanged">
                                                        </asp:CheckBox>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRow" runat="server" AutoPostBack="true" OnCheckedChanged="chkRow_CheckedChanged">
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                        <asp:HiddenField ID="HFID" runat="server" Value='<%# Eval("ID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Image">
                                                    <ItemTemplate>
                                                        <asp:Image ID="ImgLogo" runat="server" Style="max-width: 50px;" ImageUrl='<%# Eval("Image") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Name" DataField="Name" />
                                                <asp:BoundField HeaderText="Product Code" DataField="PCode" />
                                                <asp:BoundField HeaderText="HSN Code" DataField="HSNCode" />
                                                <asp:BoundField HeaderText="Barcode" DataField="BCode" />
                                                <asp:TemplateField HeaderText="Opening Stock">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TBOpeningStock" runat="server" CssClass="input-mini" Text='<%# Eval("OS") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Opening Stock Date">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TBOSDate" runat="server" CssClass="input-small" Text='<%# Eval("OSDate") %>'></asp:TextBox>
                                                        <asp:CalendarExtender ID="CEOSDate" runat="server" Format="dd/MM/yyyy" TargetControlID="TBOSDate"
                                                            PopupButtonID="TBOSDate">
                                                        </asp:CalendarExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reorder Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TBReorderQty" runat="server" CssClass="input-mini" Text='<%# Eval("RQty") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="BinLocation">
                                                    <ItemTemplate>
                                                        <div class="input-append">
                                                            <asp:TextBox ID="TBBinLocation" runat="server" CssClass="input-medium" Text='<%# Eval("Bin") %>'></asp:TextBox>
                                                            <span class="add-on">
                                                                <asp:LinkButton ID="LBGetBin" runat="server" Font-Underline="false" CommandArgument='<%# Eval("ID")%>' CommandName="GetBin" >
                                                                    <i class="icon-map-marker" style="line-height:20px;"></i>
                                                                </asp:LinkButton>
                                                            </span>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <hr />
                        <div class="row-fluid">
                            <div class="span12 text-right">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                    OnClick="btnSubmit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
        <!-- Bin Selector-->
        <asp:LinkButton ID="LB1" runat="server"></asp:LinkButton>
        <asp:ModalPopupExtender ID="MPE2" BehaviorID="mpe2" runat="server" PopupControlID="PnlBinMaster"
            TargetControlID="LB1" BackgroundCssClass="modalBackground" CancelControlID="ModelBinCancel">
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
                                                        <asp:DropDownList ID="DDLWarehouse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLWarehouse_SelectedIndexChanged">
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
        <!-- END Bin Selector-->
    </div>
    <!-- END PAGE CONTAINER-->
</asp:Content>
