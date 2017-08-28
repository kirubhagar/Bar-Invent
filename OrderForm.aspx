<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="OrderForm.aspx.cs" Inherits="OrderForm" %>

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
        function ConfirmOnDelete() {
            if (confirm("Are you sure?") == true)
                return true;
            else
                return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN PAGE CONTAINER-->
    <asp:UpdatePanel runat="server" ID="UPOrder">
        <ContentTemplate>
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
                            OrderForm <small>OrderForm</small>
                        </h3>
                        <ul class="breadcrumb">
                            <li><a href="Dashboard.aspx">Dashboard<i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                            </li>
                            <li><a href="#">Sample Pages</a> <span class="divider">&nbsp;</span> </li>
                            <li><a href="OrderForm.aspx">Order Form</a><span class="divider-last">&nbsp;</span></li>
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
                                    <i class="icon-globe"></i>Blank Page</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                    class="icon-remove"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Supplier List</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLSuplList" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <asp:GridView ID="GVOrderMaster" runat="server" CssClass="table table-advance table-bordered table-condensed"
                                    Width="100%" AutoGenerateColumns="False" DataKeyNames="ID" GridLines="Vertical"
                                    AllowPaging="True" PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom"
                                    HeaderStyle-Wrap="false" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-VerticalAlign="Middle" CellPadding="4" RowStyle-VerticalAlign="Middle"
                                    ForeColor="#333333" OnPageIndexChanging="GVOrderMaster_PageIndexChanging" PageSize="10">
                                    <HeaderStyle BackColor="#1fbdc9" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Selection">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="HeaderCheck" runat="server" AutoPostBack="true" data-original-title="Select Header"
                                                    CommandName="SelectHeader" OnCheckedChanged="chkHeader_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkSelection" runat="server" data-original-title="Select Record"
                                                    OnCheckedChanged="chkRow_CheckedChanged" AutoPostBack="true" CommandName="SelectRecord"
                                                    CommandArgument='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>
                                                <asp:HiddenField ID="HDFD" runat="server" Value='<%# Eval("ItemId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ComID" DataField="ComId" Visible="false" />
                                        <asp:BoundField HeaderText="NGOId" DataField="NGOId" Visible="false" />
                                        <asp:BoundField HeaderText="GodId" DataField="GodId" Visible="false" />
                                        <asp:TemplateField HeaderText="Category">
                                            <ItemTemplate>
                                                <asp:Label ID="LB_Cat" runat="server" Text='<%# Eval("CatName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="LBItem" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="HSNCode">
                                            <ItemTemplate>
                                                <asp:Label ID="LB_HSN" runat="server" Text='<%# Eval("HSNCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Barcode">
                                            <ItemTemplate>
                                                <asp:Label ID="LBI_Barcode" runat="server" Text='<%# Eval("Barcode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UOM">
                                            <ItemTemplate>
                                                <asp:Label ID="LBI_UOM" runat="server" Text='<%# Eval("UOM") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price">
                                            <ItemTemplate>
                                                <asp:Label ID="LBI_PRice" runat="server" Text='<%# Eval("PurPrice") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax">
                                            <ItemTemplate>
                                                <asp:Label ID="LB_tax" runat="server" Text='<%# Eval("TaxPer") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="LB_Bal" runat="server" Text='<%# Eval("Balance") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" 
                                        Mode="NumericFirstLast" />
                                    <RowStyle VerticalAlign="Middle" />
                                </asp:GridView>
                                <div class="space15">
                                </div>
                                <div class="row-fluid">
                                    <div class="span12 text-right">
                                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Generate PO"
                                            OnClick="Generate_PO"  OnClientClick="return confirm('Your PO Has Been Accepted');"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- END PAGE CONTENT-->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- END PAGE CONTAINER-->
</asp:Content>
