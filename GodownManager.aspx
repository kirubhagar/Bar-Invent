<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true" CodeFile="GodownManager.aspx.cs" Inherits="GodownManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN PAGE CONTAINER-->
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                    Godown Manager <small>Create & Manager Godown details...</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Organization</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="GodownManager.aspx">Godown Details</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>Godown Manager</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <asp:MultiView ID="MVGodown" runat="server" ActiveViewIndex="0">
                            <asp:View ID="View0" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span4 text-center">
                                            <asp:LinkButton ID="LBAddNew" runat="server" CssClass="btn btn-info" OnClick="BtnAddNewClick">
                                                <i class="icon-plus"></i> Add New
                                            </asp:LinkButton>
                                        </div>
                                        <div class="span4 text-center">
                                            <div class="input-append">
                                                <asp:TextBox ID="TBSearch" runat="server" placeholder="BL Name / Godown Name"></asp:TextBox>
                                                <span class="add-on">
                                                    <asp:LinkButton ID="LBSearch" runat="server" OnClick="SearchEvent">
                                                    <i class="icon-search" style="text-decoration:none;"></i>
                                                    </asp:LinkButton>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVGodown" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" OnPageIndexChanging="GVGodown_PageIndexChanging"
                                            OnRowCommand="GVGodown_RowCommand"  HeaderStyle-Font-Bold="true"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" CellPadding="10"
                                            RowStyle-VerticalAlign="Middle">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Company" DataField="CompanyName" />
                                                <asp:BoundField HeaderText="NGO" DataField="BusinessLocationName" />
                                                <asp:BoundField HeaderText="Godown" DataField="Name" />
                                                <asp:BoundField HeaderText="Short Form" DataField="ShortForm" />
                                                <asp:TemplateField HeaderText="Is Default Godown">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDefault" runat="server" Text='<%# Eval("IsDefaultGodown").ToString() == "True" ? "Yes" : "No" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Is Used For Stock Correction">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCorrection" runat="server" Text='<%# Eval("IsUsedForStockCorrection").ToString() == "True" ? "Yes" : "No" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Is Active">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActive" runat="server" Text='<%# Eval("IsActive").ToString() == "True" ? "Yes" : "No" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Is Transit Godown">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTransit" runat="server" Text='<%# Eval("IsTransitGodown").ToString() == "True" ? "Yes" : "No" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                            </asp:View>
                            <asp:View ID="View1" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span8 offset3">
                                            <div class="control-group">
                                                <span class="control-label">Godown Name :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBGodownName" runat="server" placeholder="Godown Name"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Discriptions :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBDiscription" runat="server" TextMode="MultiLine" placeholder="Discription....."></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:CheckBox ID="CBIsActive" runat="server" />
                                                    <span>Is Active</span>
                                                </div>
                                                <div class="control-group">
                                                    <asp:CheckBox ID="CBIsDefault" runat="server" />
                                                    <span>Is Default Godown</span>
                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:CheckBox ID="CBIsTransit" runat="server" />
                                                    <span>Is Transit Godown</span>
                                                </div>
                                                <div class="control-group">
                                                    <asp:CheckBox ID="CBForStockCorrection" runat="server" />
                                                    <span>Is Used For Stock Correction</span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12 text-right">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                            OnClick="BtnSubmitCancel" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                            OnClick="BtnSubmitCancel" />
                                    </div>
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
