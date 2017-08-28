<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="BinLocationMaster.aspx.cs" Inherits="BinLocationMaster" %>

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
                    Bin Location <small>Create & manage Bin Location For Items</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Masters</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="BinLocationMaster.aspx">Bin Location</a><span class="divider-last">&nbsp;</span></li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <asp:LinkButton ID="LBBinLocation" runat="server" Text="Bin Location Manager" OnClick="LBSelection"
                    CssClass="btn btn-info"></asp:LinkButton>
                <asp:LinkButton ID="LBWarehouse" runat="server" Text="Godown Manager" OnClick="LBSelection"
                    CssClass="btn btn-info"></asp:LinkButton>
                <asp:LinkButton ID="LBZone" runat="server" Text="Zone Manager" OnClick="LBSelection"
                    CssClass="btn btn-info"></asp:LinkButton>
                <asp:LinkButton ID="LBArea" runat="server" Text="Area Manager" OnClick="LBSelection"
                    CssClass="btn btn-info"></asp:LinkButton>
                <div class="space15">
                </div>
                <asp:MultiView ID="MVWarehouse" runat="server" ActiveViewIndex="0">
                    <!-- View For Bin Location Manager-->
                    <asp:View ID="View0" runat="server">
                        <!-- BEGIN Bin Location Manager-->
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
                                            <asp:TextBox ID="TBBinLocation" runat="server" Enabled="false" placeholder="#_._._._._"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- END Bin Location Manager-->
                    </asp:View>
                    <!-- View For Warehouse Manager-->
                    <asp:View ID="View1" runat="server">
                        <!-- BEGIN Warahouse Manager-->
                        <div class="widget">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-globe"></i>Godown Master</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                            </div>
                            <div class="widget-body form-horizontal">
                                <div class="row-fluid">
                                    <asp:GridView ID="GVGodown" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                        Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                        PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" OnPageIndexChanging="GVGodown_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Godown Title" DataField="Name" />
                                            <asp:BoundField HeaderText="Short Name" DataField="ShortForm" />
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status").ToString() == "True" ? "Active" : "InActive" %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <!-- END Warahouse Manager-->
                    </asp:View>
                    <!-- View For Zone Manager-->
                    <asp:View ID="View2" runat="server">
                        <!-- BEGIN Zone Manager-->
                        <div class="widget">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-globe"></i>Zone Manager</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                            </div>
                            <div class="widget-body form-horizontal">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Select Godown</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLGodownZone" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Zone Title</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBZoneTitle" runat="server" placeholder="Zone Title"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Short Form</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBZoneShortForm" runat="server" placeholder="Short Form"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label"></span>
                                                <div class="controls">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid text-right">
                                    <asp:Button ID="btnSubmitZone" runat="server" Text="Submit" CssClass="btn btn-success"
                                        OnClick="SubmitMethodZone" />
                                    <asp:Button ID="btnCancelZone" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                        OnClick="SubmitMethodZone" />
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <asp:GridView ID="GVZone" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                        Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                        PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" OnPageIndexChanging="GVZone_PageIndexChanging"
                                        OnRowCommand="GVZone_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Godown Name" DataField="Name" />
                                            <asp:BoundField HeaderText="Zone Title" DataField="ZoneTitle" />
                                            <asp:BoundField HeaderText="Short Name" DataField="ShortName" />
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status").ToString() == "True" ? "Active" : "InActive" %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="________Controls________">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBView" Visible="false" runat="server" CssClass="btn tooltips"
                                                        data-placement="top" data-original-title="View Record" CommandName="ViewRecord"
                                                        CommandArgument='<%# Eval("ID") %>'>
                                                        <i class="icon-eye-open"></i>
                                                    </asp:LinkButton>&nbsp;
                                                    <asp:LinkButton ID="LBEdit" runat="server" CssClass="btn btn-primary tooltips" data-placement="top"
                                                        data-original-title="Edit Record" CommandName="EditRecord" CommandArgument='<%# Eval("ID") %>'>
                                                        <i class="icon-pencil icon-white"></i>
                                                    </asp:LinkButton>&nbsp;
                                                    <asp:LinkButton ID="LBDelete" runat="server" CssClass="btn btn-danger tooltips" data-placement="top"
                                                        data-original-title="Delete Record" CommandName="DeleteRecord" CommandArgument='<%# Eval("ID") %>'>
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
                        </div>
                        <!-- END Zone Manager-->
                    </asp:View>
                    <!-- View For Area Manager-->
                    <asp:View ID="View3" runat="server">
                        <!-- BEGIN Area Manager-->
                        <div class="widget">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-globe"></i>Area Manager</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Select Godown</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLGodownArea" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLWarehouseArea_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Select Zone</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLZoneArea" runat="server">
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
                                                <span class="control-label">Area Title</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBAreaTitle" runat="server" placeholder="Area Title"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Short Form</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBAreaShortForm" runat="server" placeholder="Area Short Form"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">No Of Rows</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBNoOfRows" runat="server" placeholder="No Of Rows In Area"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">No Of Shelf (Per Row)</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBNoOfShelfs" runat="server" placeholder="No Of Shelfs Per Row"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid text-right">
                                    <asp:Button ID="btnSubmitArea" runat="server" Text="Submit" CssClass="btn btn-success"
                                        OnClick="SubmitMethodArea" />
                                    <asp:Button ID="btnCancelArea" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                        OnClick="SubmitMethodArea" />
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <asp:GridView ID="GVArea" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" OnPageIndexChanging="GVArea_PageIndexChanging"
                                            OnRowCommand="GVArea_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Godown Name" DataField="Name" />
                                                <asp:BoundField HeaderText="Zone Name" DataField="ZoneTitle" />
                                                <asp:BoundField HeaderText="Area Name" DataField="AreaTitle" />
                                                <asp:BoundField HeaderText="Short Name" DataField="ShortForm" />
                                                <asp:BoundField HeaderText="No Of Rows" DataField="NoOfRow" />
                                                <asp:BoundField HeaderText="No Of Shelf" DataField="NoOfShelf" />
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status").ToString() == "True" ? "Active" : "InActive" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="________Controls________">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LBView" Visible="false" runat="server" CssClass="btn tooltips"
                                                            data-placement="top" data-original-title="View Record" CommandName="ViewRecord"
                                                            CommandArgument='<%# Eval("ID") %>'>
                                                        <i class="icon-eye-open"></i>
                                                        </asp:LinkButton>&nbsp;
                                                        <asp:LinkButton ID="LBEdit" runat="server" CssClass="btn btn-primary tooltips" data-placement="top"
                                                            data-original-title="Edit Record" CommandName="EditRecord" CommandArgument='<%# Eval("ID") %>'>
                                                        <i class="icon-pencil icon-white"></i>
                                                        </asp:LinkButton>&nbsp;
                                                        <asp:LinkButton ID="LBDelete" runat="server" CssClass="btn btn-danger tooltips" data-placement="top"
                                                            data-original-title="Delete Record" CommandName="DeleteRecord" CommandArgument='<%# Eval("ID") %>'>
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
                            </div>
                        </div>
                        <!-- END Area Manager-->
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END PAGE CONTAINER-->
</asp:Content>
