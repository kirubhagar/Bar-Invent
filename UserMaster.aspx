<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="UserMaster.aspx.cs" Inherits="UserMaster" %>

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
                    User Manager <small>Create & Manage User Management...</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="#"><i class="icon-home "></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">User Manager</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="UserMaster.aspx">User Master</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>User Manager</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <asp:MultiView ID="MVUser" runat="server" ActiveViewIndex="0">
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
                                        <asp:GridView ID="GVUsers" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" HeaderStyle-Wrap="false"
                                            HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                            CellPadding="10" RowStyle-VerticalAlign="Middle" ShowHeaderWhenEmpty="true" RowStyle-Wrap="false"
                                            PageSize="10" OnPageIndexChanging="GVUsers_PageIndexChanging" OnRowCommand="GVUsers_RowCommand">
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
                                                <asp:BoundField HeaderText="First Name" DataField="FirstName" />
                                                <asp:BoundField HeaderText="Last Name" DataField="LastName" />
                                                <asp:BoundField HeaderText="User Name" DataField="UserName" />
                                                <asp:BoundField HeaderText="Password" DataField="PassWord" />
                                                <asp:BoundField HeaderText="EmailID" DataField="Email" />
                                                <asp:BoundField HeaderText="Phone No" DataField="Phone" />
                                                <asp:BoundField HeaderText="Mobile No" DataField="Mobile" />
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
                                    <h4>
                                        User Registrations...</h4>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">First Name :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBFirstName" runat="server" placeholder="First Name"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFVFirstName" CssClass="help-inline" runat="server"
                                                        ErrorMessage="Required!" ForeColor="#dc5d3a" ControlToValidate="TBFirstName"
                                                        ValidationGroup="Submit" SetFocusOnError="true">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Last Name :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBLastName" runat="server" placeholder="Last  Name"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <asp:FileUpload ID="FULogo" runat="server" onchange="showpreview(this);" Style="top: 0;
                                                display: block;" />
                                            <img id="imgpreview" alt="logo" src="" style="border-width: 0px; visibility: hidden;
                                                height: 100px;" />
                                            <asp:Image ID="ImgLogo" runat="server" Width="100px" />
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Email ID :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBEmail" runat="server" placeholder="Ex: example@domain.com"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">User Name :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBUserName" runat="server" placeholder="User  Name"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFVUserName" CssClass="help-inline" runat="server"
                                                        ErrorMessage="Required!" ForeColor="#dc5d3a" ControlToValidate="TBUserName"
                                                        ValidationGroup="Submit" SetFocusOnError="true">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Password :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBPassword" runat="server" placeholder="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFVPassword" CssClass="help-inline" runat="server"
                                                        ErrorMessage="Required!" ForeColor="#dc5d3a" ControlToValidate="TBPassword"
                                                        ValidationGroup="Submit" SetFocusOnError="true">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Confirm Password :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBCPassword" runat="server" placeholder="Confirm Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="help-inline" runat="server"
                                                        ErrorMessage="Required!" ForeColor="#dc5d3a" ControlToValidate="TBCPassword"
                                                        ValidationGroup="Submit" SetFocusOnError="true">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="CVPass" runat="server" ControlToValidate="TBCPassword" ControlToCompare="TBPassword"
                                                        ErrorMessage="Mismatch!" ForeColor="#dc5d3a" Operator="Equal" SetFocusOnError="true" ValidationGroup="Submit">
                                                    </asp:CompareValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Mobile No :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBMobile" runat="server" placeholder="Mobile No"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Phone No :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBPhone" runat="server" placeholder="Phone No"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid text-right">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success" ValidationGroup="Submit"
                                        OnClick="btnSubmit_Click" />
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
