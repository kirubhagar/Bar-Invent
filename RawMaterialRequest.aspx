<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="RawMaterialRequest.aspx.cs" Inherits="RawMaterialRequest" %>

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
                    Request Page <small>Request Page</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx">Dashboard<i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">RawMaterial</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="RawMaterialRequest.aspx">RawMaterialRequest</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>RequestPage</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <!-- MULTIVIEW STARTS HERE-->
                        <asp:MultiView ID="MVRequest" runat="server" ActiveViewIndex="0">
                            <asp:View ID="view0" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <asp:LinkButton ID="LBAddNew" runat="server" CssClass="btn btn-info" OnClick="TopEventSet">
                                                 <i class="icon-plus"></i> New
                                            </asp:LinkButton>&nbsp;
                                            <asp:LinkButton ID="LBExport" runat="server" CssClass="btn btn-info" Visible="false">
                                                 <i class="icon-download-alt"></i> Export
                                            </asp:LinkButton>&nbsp;
                                            <asp:LinkButton ID="LBImport" runat="server" CssClass="btn btn-info" Visible="false">
                                                 <i class="icon-upload-alt"></i> import
                                            </asp:LinkButton>&nbsp;
                                        </div>
                                        <div class="span6">
                                            <div class="input-append">
                                                <asp:TextBox ID="TBSearch" runat="server" CssClass="input-xlarge" placeholder="Search Record Via Date OR ReqNo"></asp:TextBox>
                                                <span class="add-on">
                                                    <asp:LinkButton ID="LBSearch" runat="server" OnClick="LBSearch_Click">
                                                         <i class="icon icon-search" style="line-height:20px;"></i>
                                                    </asp:LinkButton>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVItemDetail" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PageSize="4" PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom"
                                            HeaderStyle-Wrap="false" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-VerticalAlign="Middle" CellPadding="10" RowStyle-VerticalAlign="Middle"
                                            ShowHeaderWhenEmpty="true" ClientIDMode="Predictable" OnRowCommand="GVItemDetail_RowCommand"
                                            OnPageIndexChanging="GVItemDetail_PageIndexChanging" PageIndex="5">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="RequestNo" DataField="ReqNo" />
                                                <asp:BoundField HeaderText="RequestDate" DataField="ReqDate" />
                                                <asp:BoundField HeaderText="DeliveryDate" DataField="DeliverDate" />
                                                <asp:BoundField HeaderText="Deliver To" DataField="Name" />
                                                <asp:TemplateField HeaderText="Req For">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("ItemType").ToString() == "1" ? "Kit Material" : Eval("ItemType").ToString() == "2" ? "Finish Material": ""%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="TotalQty" DataField="TotalQty" />
                                                <asp:BoundField HeaderText="TotalValue" DataField="TotalValue" />
                                                <asp:TemplateField HeaderText="ReqStatus">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("ReqStatus").ToString() == "True" ? "Active" : "InActive"%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="____Controls____">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LBEdit" runat="server" CssClass="btn btn-info" CommandName="EditRecord"
                                                            CommandArgument='<%# Eval("ID") %>'>
                                                            <i class="icon-pencil icon-white"></i>
                                                        </asp:LinkButton>&nbsp;
                                                        <asp:LinkButton ID="LBDelete" runat="server" CssClass="btn btn-danger" CommandName="DeleteRecord"
                                                            CommandArgument='<%# Eval("ID") %>'>
                                                            <i class="icon-trash icon-white"></i>
                                                        </asp:LinkButton>&nbsp;
                                                        <asp:LinkButton ID="LBStatus" runat="server" CssClass="btn btn-Success" CommandName="CheckStatus"
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
                            <!--NEXT VIEW STARTS HERE-->
                            <asp:View ID="view1" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">RequestNo.</span>
                                                <div class="controls">
                                                    <asp:Label ID="LBLReqNo" runat="server" CssClass="MyLable" Text="..."></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">RequestDate</span>
                                                <div class="controls">
                                                    <asp:Label ID="LBReqDate" runat="server" Text="..." CssClass="MyLable"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Com</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLComLoc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLComLoc_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Ngo</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLNgoLoc" runat="server">
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
                                                <span class="control-label">Deliver Date</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtDelDate" runat="server" placeholder="Select Deliver Date"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CERequestDeliverDate" runat="server" Format="dd/MM/yyyy"
                                                        TargetControlID="txtDelDate" PopupButtonID="txtDelDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">DeliveredTo</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLDelTo" runat="server">
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
                                                <span class="control-label">BarCode</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtBarCode" runat="server" OnTextChanged="DDLItemList_SelectedIndexChanged"
                                                        placeholder="Enter BarCode" AutoPostBack="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">ItemType</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLItemType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLItemType_SelectedIndexChanged">
                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Kit Material" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Finish Material" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group">
                                            <div class="control-label">
                                                ItemList</div>
                                            <div class="controls">
                                                <asp:DropDownList ID="DDLItemList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLItemList_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <asp:GridView ID="GVInner" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PageSize="4" PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom"
                                            HeaderStyle-Wrap="false" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-VerticalAlign="Middle" CellPadding="10" RowStyle-VerticalAlign="Middle"
                                            ShowHeaderWhenEmpty="true" ClientIDMode="Predictable" OnRowCommand="GVInner_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                        <asp:HiddenField ID="HFID" runat="server" Value='<%# Eval("ID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Image">
                                                    <ItemTemplate>
                                                        <asp:Image ID="ImgLogo" runat="server" Style="max-width: 80px;" ImageUrl='<%# Eval("Image") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Name" DataField="Name" />
                                                <asp:BoundField HeaderText="ProductCode" DataField="ProductCode" />
                                                <asp:BoundField HeaderText="HSNCode" DataField="HSNCode" />
                                                <asp:BoundField HeaderText="Barcode" DataField="Barcode" />
                                                <asp:BoundField HeaderText="UOM" DataField="UOM" />
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TBItemQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="input-mini"
                                                            AutoPostBack="true" OnTextChanged="BindInnerData2"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Unit Price" DataField="UnitPrice" />
                                                <asp:BoundField HeaderText="Total Price" DataField="TotalPrice" />
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
                                                <span class="control-label">TotalQuantity</span>
                                                <div class="controls">
                                                    <asp:Label ID="LBLTOtQut" runat="server" CssClass="MyLable" Text="00.00"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">TotalValues</span>
                                                <div class="controls">
                                                    <asp:Label ID="LBLTotVal" runat="server" CssClass="MyLable" Text="00.00"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Terms & Conditions</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtTermCon" runat="server" placeholder="Terms & Conditions" TextMode="MultiLine"
                                                        Style="resize: none;"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="span6">
                                                <div class="control-group">
                                                    <span class="control-label">Remarks</span>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtRemarks" runat="server" placeholder="Remarks" TextMode="MultiLine"
                                                            Style="resize: none;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid text-right">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                        OnClick="BtnSubmitCancel" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                        OnClick="BtnSubmitCancel" />
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END PAGE CONTENT-->
    <!-- END PAGE CONTAINER-->
</asp:Content>
