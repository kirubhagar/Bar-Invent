<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="RTInventory.aspx.cs" Inherits="RTInventory" %>

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
                    Stock Summary <small>View Stock Summary...</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Material Manager</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="RTInventory.aspx">Stock Summary</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>Stock Summary
                        </h4>
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
                        <h4>
                            Material Details</h4>
                        <hr />
                        <div class="row-fluid">
                            <div class="span12">
                                <div class="span6">
                                    <label class="radio inline">
                                        <asp:RadioButton ID="RBKit" runat="server" GroupName="Material" AutoPostBack="true" OnCheckedChanged="RBCheckedChanged" />
                                        <span>Kit Material</span>
                                    </label>
                                    <label class="radio inline">
                                        <asp:RadioButton ID="RBFinish" runat="server" GroupName="Material" AutoPostBack="true" OnCheckedChanged="RBCheckedChanged" />
                                        <span>Finish Material</span>
                                    </label>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <span class="control-label">Select <asp:Label ID="LBLMaterialName" runat="server"></asp:Label> : </span>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="DDLMaterial" runat="server">
                                                </asp:DropDownList>
                                                <span class="add-on">
                                                    <asp:LinkButton ID="LBMaterial" runat="server" Font-Underline="false" Text="Submit" OnClick="LBMaterial_Click"></asp:LinkButton>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="row-fluid">
                            <div class="span12" style="overflow-x: auto;">
                                <asp:UpdatePanel ID="UP1" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="GVItem" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" HeaderStyle-Wrap="false"
                                            HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                            CellPadding="10" RowStyle-VerticalAlign="Middle" ShowHeaderWhenEmpty="true">
                                            <Columns>
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
                                                <asp:TemplateField HeaderText="Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LBLPrice" runat="server" CssClass="MyLable" Text='<%# Eval("Price") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Stock">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LBLTotalStock" runat="server" CssClass="MyLable" Text='<%# Eval("TotalStock") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Available Kits" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LBLKitQty" runat="server" CssClass="MyLable" Text='<%# Eval("KitQty") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Available Finish" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LBLFinishQty" runat="server" CssClass="MyLable" Text='<%# Eval("FinishQty") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
