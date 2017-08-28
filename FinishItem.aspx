<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="FinishItem.aspx.cs" Inherits="FinishItem" %>

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
                    Finish Materials <small>Create & Manage Finish Item Details... </small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="#"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Material Manager</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="FinishItem.aspx">Finish Materials</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>Finish Materials</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <asp:MultiView ID="MVFinishMatrial" runat="server" ActiveViewIndex="0">
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
                                                <asp:TextBox ID="TBSearch" runat="server" placeholder="Type Users Name To Search"></asp:TextBox>
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
                                        <asp:GridView ID="GVFinishMaterial" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" HeaderStyle-Font-Bold="true"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" CellPadding="10"
                                            RowStyle-VerticalAlign="Middle" ShowHeaderWhenEmpty="true" PageSize="10" OnPageIndexChanging="GVFinishMaterial_PageIndexChanging"
                                            OnRowCommand="GVFinishMaterial_RowCommand">
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
                                                <asp:BoundField HeaderText="Product Code" DataField="ProductCode" />
                                                <asp:BoundField HeaderText="HSN Code" DataField="HSNCode" />
                                                <asp:BoundField HeaderText="Measuring Unit" DataField="UOM" />
                                                <asp:BoundField HeaderText="Category" DataField="Category" />
                                                <asp:BoundField HeaderText="Total Qty" DataField="TotalQty" />
                                                <asp:BoundField HeaderText="Total Value" DataField="TotalValue" />
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
                                                <span class="control-label">HSN Code : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBHSN" runat="server" placeholder="HSN Code"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Measuring Unit : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBUOM" runat="server" placeholder="Measuring Unit"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Category : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBCategory" runat="server" placeholder="Category"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Raw Material : </span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLItemList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ItemList_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Barcode : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBSearchBarcode" runat="server" placeholder="Search Via Barcode"
                                                        AutoPostBack="true" OnTextChanged="ItemList_SelectedIndexChanged"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVInner" runat="server" CssClass="table table-hover table-advance"
                                            Width="100%" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" HeaderStyle-Wrap="false"
                                            OnRowCommand="GVInner_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                        <asp:HiddenField ID="HFID" runat="server" Value='<%# Eval("ID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                <asp:BoundField HeaderText="Barcode" DataField="Barcode" />
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
                                                <span class="control-label">Total Quantity : </span>
                                                <div class="controls">
                                                    <asp:Label ID="lblTotalQty" runat="server" CssClass="MyLable" Text="0.00"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Total Value : </span>
                                                <div class="controls">
                                                    <asp:Label ID="lblTotalValue" runat="server" CssClass="MyLable" Text="0.00"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Image : </span>
                                                <div class="controls">
                                                    <asp:FileUpload ID="FULogo" runat="server" onchange="showpreview(this);" Style="top: 0;
                                                        display: block;" />
                                                    <img id="imgpreview" alt="logo" src="" style="border-width: 0px; visibility: hidden;
                                                        height: 100px;" />
                                                    <asp:Image ID="ImgLogo" runat="server" Width="100px" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6 text-center">
                                            <div class="control-group">
                                                <asp:TextBox ID="TBDescription" runat="server" placeholder="Material Description..."
                                                    TextMode="MultiLine" Style="resize: none; height: 100px; width: 74%;"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid text-right">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                        ValidationGroup="Submit" OnClick="btnSubmit_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                        OnClick="btnCancel_Click" />
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
