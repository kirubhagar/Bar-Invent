<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="IssueMaterial.aspx.cs" Inherits="IssueMaterial" %>

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
                    Issued Material<small>See Availability and Issue Material</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Sample Pages</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="IssueMaterial.aspx">Issue Material</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>Issue material</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <asp:MultiView ID="MVIssueMaterial" runat="server" ActiveViewIndex="0">
                            <asp:View ID="view0" runat="server">
                                <%--<div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <asp:LinkButton ID="LBAddNew" runat="server" CssClass="btn btn-info">
                                                <i class="icon-plus"></i>&nbsp; Add New
                                            </asp:LinkButton>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <div class="input-append">
                                                    <asp:TextBox ID="TBSearchText" runat="server" CssClass="input-large" placeholder="Search Via IssueNo OR IssueDate"></asp:TextBox>
                                                    <span class="add-on">
                                                        <asp:LinkButton ID="LBSearch" runat="server" Font-Underline="false">
                                                            <i class="icon-search" style="line-height:20px;"></i>
                                                        </asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                    </div>
                                </div>--%>
                                <h5>Issue Request Details</h5>
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
                                                <asp:TemplateField HeaderText="_Controls_">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LBEdit" runat="server" CssClass="btn btn-info" CommandName="ViewRecord"
                                                            CommandArgument='<%# Eval("ID") %>'>
                                                            <i class=" icon-eye-open icon-white"></i>
                                                        </asp:LinkButton>&nbsp;
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                
                                <div class="space20"></div>
                                
                                <h5>Issued Details</h5>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PageSize="5" PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom"
                                            HeaderStyle-Wrap="false" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-VerticalAlign="Middle" CellPadding="10" RowStyle-VerticalAlign="Middle"
                                            ShowHeaderWhenEmpty="true" ClientIDMode="Predictable"
                                            OnPageIndexChanging="GridView1_PageIndexChanging" >
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="RequestNo" DataField="IssueNo" />
                                                <asp:BoundField HeaderText="RequestDate" DataField="IssueDate" />
                                                <asp:BoundField HeaderText="DeliveryDate" DataField="DeliveryDate" />
                                                <asp:BoundField HeaderText="Deliver To" DataField="Name" />
                                                <asp:TemplateField HeaderText="Issue For">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("ItemType").ToString() == "1" ? "Kit Material" : Eval("ItemType").ToString() == "2" ? "Finish Material": ""%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="TotalQty" DataField="TotalQty" />
                                                <asp:BoundField HeaderText="TotalValue" DataField="TotalValue" />
                                                <asp:TemplateField HeaderText="IssueStatus">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("IssueStatus").ToString() == "True" ? "Active" : "InActive"%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </asp:View>
                            <!--VIEW SECOND AREA-->
                            <asp:View ID="view1" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">IssueNo.</span>
                                                <div class="controls">
                                                    <asp:Label ID="LBLReqNo" runat="server" Text="..."></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">IssueDate</span>
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
                                                    <asp:DropDownList ID="DDLComLoc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLComLoc_SelectedIndexChanged"
                                                        Enabled="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Ngo</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLNgoLoc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLNgoLoc_SelectedIndexChanged"
                                                        Enabled="false">
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
                                                    <asp:DropDownList ID="DDLDelTo" runat="server" AutoPostBack="true" Enabled="false">
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
                                                <span class="control-label"></span>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtBarCode" runat="server" OnTextChanged="DDLItemList_SelectedIndexChanged"
                                                        Enabled="false" Visible="false" placeholder="Enter BarCode" AutoPostBack="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label"></span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLItemType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLItemType_SelectedIndexChanged"
                                                        Enabled="false" Visible="false">
                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Kit" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Finished Good" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="Both" Value="3"></asp:ListItem>
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
                                            </div>
                                            <div class="controls">
                                                <asp:DropDownList ID="DDLItemList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLItemList_SelectedIndexChanged"
                                                    Enabled="false" Visible="false">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <!--FORM RAGION -->
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
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LBLNAMe" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="ProductCode" DataField="ProductCode" />
                                                <asp:BoundField HeaderText="HSNCode" DataField="HSNCode" />
                                                <asp:TemplateField HeaderText="Barcode">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBarcode" runat="server" Text='<%# Eval("Barcode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="UOM" DataField="UOM" />
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LB" runat="server" Text='<%# Eval("Quantity") %>' OnTextChanged="BindInnerData2"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Issued">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="LBIssued" runat="server" Text=></asp:Label>--%>
                                                        <asp:TextBox ID="TBIssud" runat="server" CssClass="input-mini" Text='<%# Eval("IssueQty") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Unit Price" DataField="UnitPrice" />
                                                <asp:BoundField HeaderText="Total Price" DataField="TotalPrice" />
                                                <asp:TemplateField HeaderText="Availability">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="CheckAvailability" runat="server" CssClass="btn btn-danger" CommandArgument='<%# Eval("ID") %>'
                                                            CommandName='<%# Container.DataItemIndex%>'>
                                                                        <i class=" icon-check icon-white"></i>
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
                                                    <asp:Label ID="LBLTOtQut" runat="server" Text="00.00" CssClass="MyLable"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">TotalValues</span>
                                                <div class="controls">
                                                    <asp:Label ID="LBLTotVal" runat="server" Text="00.00" CssClass="MyLable"></asp:Label>
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
                        <!-- Model Section-->
                        <asp:LinkButton ID="LB1" runat="server"></asp:LinkButton>
                        <asp:ModalPopupExtender ID="MPE2" BehaviorID="mpe2" runat="server" PopupControlID="PnlBinMaster"
                            TargetControlID="LB1" BackgroundCssClass="modalBackground" CancelControlID="ModelBinCancel">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="PnlBinMaster" runat="server" CssClass="pnlBackGround" Style="display: none">
                            <div class="modal span10 offset1" style="left: 0;">
                                <div class="modal-header">
                                    <h3 id="H1">
                                        Request Detail</h3>
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <div class="row-fluid">
                                                <div class="span12">
                                                    <div class="span4">
                                                        <div class="control-group">
                                                            <span class="control-label">Item Name</span>
                                                            <div class="controls">
                                                                <asp:Label ID="LBItemName" runat="server" CssClass="MyLable"></asp:Label>
                                                                <asp:HiddenField ID="HFIndex" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="span4">
                                                        <div class="control-group">
                                                            <span class="control-label">BarCode</span>
                                                            <div class="controls">
                                                                <asp:Label ID="LBBarcode" runat="server" CssClass="MyLable"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="span4">
                                                        <div class="control-group">
                                                            <span class="control-label">Item Quantity</span>
                                                            <div class="controls">
                                                                <asp:Label ID="LBItemQty" runat="server" CssClass="MyLable"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <hr />
                                            <h5 class="text-left">
                                                Row Material Availability</h5>
                                            <hr />
                                            <asp:GridView ID="GVPopUpData" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                                Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" HeaderStyle-Wrap="false"
                                                HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                                CellPadding="10" RowStyle-VerticalAlign="Middle" ShowHeaderWhenEmpty="true" ClientIDMode="Predictable">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="HeaderCheck" runat="server" AutoPostBack="true" OnCheckedChanged="chkHeader_CheckedChanged" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChkSelection" runat="server" AutoPostBack="true" OnCheckedChanged="chkRow_CheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="S.NO.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex+1 %>
                                                            <asp:HiddenField ID="HDFD" runat="server" Value='<%# Eval("ID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RowMaterialName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LB_Name" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Request Quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LB_Quantity" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Availavility">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LB_Availavility" runat="server" Text='<%# Eval("TotalStock") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LBBalance" runat="server" Text='<%# Eval("Balance") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Order">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="LBOrder" runat="server" CssClass="btn btn-success" CommandArgument='<%# Eval("ID") %>'>
                                                                                <i class="icon-arrow-right"></i> &nbsp; Place Order
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                </Columns>
                                            </asp:GridView>
                                            <div class="space15">
                                            </div>
                                            <asp:Button ID="btnForOrder" runat="server" OnClick="Generate_Order" CssClass="btn btn-primary"
                                                Text="Place Order" />
                                            <div class="space15">
                                            </div>
                                            <hr />
                                            <div class="row-fluid">
                                                <div class="span12">
                                                    <div class="span6">
                                                        <div class="control-group">
                                                            <span class="control-label">Allot</span>
                                                            <asp:TextBox ID="txtAllot" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="span6">
                                                        <!--<asp:LinkButton ID="btnOk" runat="server" OnClick="btnOk_Click" Text="OK" CssClass="btn btn-info"></asp:LinkButton>-->
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="ModelBinSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                        OnClick="ModelSubmitButton" />
                                    <asp:Button ID="ModelBinCancel" runat="server" Text="Close" CssClass="btn btn-danger" />
                                </div>
                            </div>
                        </asp:Panel>
                        <!-- End Model Section-->
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END PAGE CONTAINER-->
</asp:Content>
