<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="EmployeeManager.aspx.cs" Inherits="EmployeeManager" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="MyControls/EmployeeDepartment.ascx" TagName="EmployeeDepartment"
    TagPrefix="uc1" %>
<%@ Register Src="MyControls/EmployeeDesignation.ascx" TagName="EmployeeDesignation"
    TagPrefix="uc2" %>
<%@ Register Src="MyControls/EmployeeWorkShift.ascx" TagName="EmployeeWorkShift"
    TagPrefix="uc3" %>
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
                <div id="alert_container">
                </div>
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                    Employee Manager <small>Create & Manager Employee Details.......</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Employee</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="EmployeeManager.aspx">Employee Manager</a><span class="divider-last">&nbsp;</span></li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <asp:MultiView ID="MVEmployee" runat="server" ActiveViewIndex="0">
                    <asp:View ID="View0" runat="server">
                        <div class="widget">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-globe"></i>Employee Manager</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <asp:LinkButton ID="LBAddNew" runat="server" CssClass="btn btn-info" OnClick="TopEventSet">
                                                <i class="icon-plus"></i> Add
                                        </asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="LBExportEmployee" runat="server" CssClass="btn btn-info" OnClick="LBExportToExcel_Click">
                                                <i class="icon-download-alt"></i>  Export
                                        </asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="LBUploadEmployee" runat="server" CssClass="btn btn-info" OnClick="LBUploadPO_Click">
                                                <i class="icon-upload-alt"></i>  Import
                                        </asp:LinkButton>&nbsp;
                                        <%--<asp:LinkButton ID="LBSendSMS" runat="server" CssClass="btn btn-danger">
                                                <i class="icon-envelope"></i> Send SMS
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="LBSendEmail" runat="server" CssClass="btn btn-danger">
                                                <i class="icon-envelope"></i> Send Email
                                            </asp:LinkButton>--%>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span8">
                                        <label class="radio">
                                            <asp:RadioButton ID="RBShowActiveOnly" runat="server" GroupName="ShowRecord" AutoPostBack="true"
                                                OnCheckedChanged="RadioEventSet" />
                                            Show Active Employee Only
                                        </label>
                                        <label class="radio">
                                            <asp:RadioButton ID="RBShowInactiveOnly" runat="server" GroupName="ShowRecord" AutoPostBack="true"
                                                OnCheckedChanged="RadioEventSet" />
                                            Show Inactive Employee Only
                                        </label>
                                        <label class="radio">
                                            <asp:RadioButton ID="RBShowAll" runat="server" GroupName="ShowRecord" AutoPostBack="true"
                                                OnCheckedChanged="RadioEventSet" />
                                            Show All
                                        </label>
                                        <label class="checkbox">
                                            <asp:CheckBox ID="CBShowImage" runat="server" Checked="true" AutoPostBack="true"
                                                OnCheckedChanged="ShowHideImageEvent" />
                                            Show Images
                                        </label>
                                    </div>
                                    <div class="span4 text-center">
                                        <div class="input-append">
                                            <asp:TextBox ID="TBSearch" runat="server" placeholder="Search Record"></asp:TextBox>
                                            <span class="add-on">
                                                <asp:LinkButton ID="LBSearch" runat="server" OnClick="SearchEvent">
                                                    <i class="icon-search" style="line-height:18px; text-decoration:none;"></i>
                                                </asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVEmployee" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PageSize="10" PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom"
                                            HeaderStyle-Wrap="false" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-VerticalAlign="Middle" CellPadding="10" RowStyle-VerticalAlign="Middle"
                                            OnPageIndexChanging="GVEmployee_PageIndexChanging" OnRowCommand="GVEmployee_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemStyle VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Image">
                                                    <ItemTemplate>
                                                        <asp:Image ID="ImgLogo" runat="server" Width="90px" ImageUrl='<%# Eval("Image") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Employee Code" DataField="EmployeeCode" />
                                                <asp:BoundField HeaderText="Name" DataField="EmployeeName" />
                                                <asp:BoundField HeaderText="Search Code" DataField="SearchCode" />
                                                <asp:BoundField HeaderText="Joining Date" DataField="DateOfJoining" />
                                                <asp:BoundField HeaderText="Department" DataField="Name" />
                                                <asp:BoundField HeaderText="Designation" DataField="Expr1" />
                                                <asp:BoundField HeaderText="Is Active" DataField="IsActive" />
                                                <asp:BoundField HeaderText="Can Have Appointment" DataField="CanHaveAppointment" />
                                                <asp:BoundField HeaderText="Work Shift Name" DataField="Expr2" />
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status").ToString() == "True" ? "Active" : "InActive" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="________Controls________">
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
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="View1" runat="server">
                        <div class="widget">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-globe"></i>New Employee Record</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <!--BEGIN TABS-->
                                        <div class="tabbable tabbable-custom">
                                            <ul class="nav nav-tabs">
                                                <li class="active"><a href="#tab_1_1" data-toggle="tab">General Details</a></li>
                                                <%--<li><a href="#tab_1_2" data-toggle="tab">User Fields</a></li>
                                            <li><a href="#tab_1_3" data-toggle="tab">Qualification</a></li>
                                            <li><a href="#tab_1_4" data-toggle="tab">Attachments</a></li>--%>
                                            </ul>
                                            <div class="tab-content">
                                                <div class="tab-pane active" id="tab_1_1">
                                                    <div class="row-fluid">
                                                        <div class="span12">
                                                            <span>Employee Code :</span>
                                                            <asp:TextBox ID="TBEmpCode" runat="server" placeholder="EmpCode"></asp:TextBox>
                                                            <asp:Button ID="btnEmployeeOption" runat="server" Text="Define My Own" OnClick="DefineMyOwn" />
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row-fluid">
                                                        <div class="span12">
                                                            <div class="span6">
                                                                <div class="widget">
                                                                    <div class="widget-title">
                                                                        <h4>
                                                                            Employee Detail</h4>
                                                                    </div>
                                                                    <div class="widget-body">
                                                                        <div class="control-group">
                                                                            <span class="control-label">First Name</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBFirstName" runat="server" placeholder="First Name"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Middle Name</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBMiddleName" runat="server" placeholder="Middle Name"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Last Name</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBLastName" runat="server" placeholder="Last Name"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Search Code</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBSearchCode" runat="server" placeholder="Search Code"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Date Of Birth</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBDOB" runat="server" placeholder="Date Of Birth"></asp:TextBox>
                                                                                <asp:CalendarExtender ID="CEFPOdate" runat="server" Format="dd/MM/yyyy" TargetControlID="TBDOB"
                                                                                    PopupButtonID="TBDOB">
                                                                                </asp:CalendarExtender>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Marital Status</span>
                                                                            <div class="controls">
                                                                                <asp:DropDownList ID="DDLMaritalStatus" runat="server">
                                                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                                    <asp:ListItem Text="Single" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="Married" Value="2"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Gender</span>
                                                                            <div class="controls">
                                                                                <asp:DropDownList ID="DDLGender" runat="server">
                                                                                    <asp:ListItem Text="Male" Value="0"></asp:ListItem>
                                                                                    <asp:ListItem Text="Female" Value="1"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Date Of Joining</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBDOJ" runat="server" placeholder="Date Of Joining"></asp:TextBox>
                                                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="TBDOJ"
                                                                                    PopupButtonID="TBDOJ">
                                                                                </asp:CalendarExtender>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Business Location</span>
                                                                            <div class="controls">
                                                                                <asp:DropDownList ID="DDLBusinessLocation" runat="server">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Department</span>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="DDLDepartment" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <span class="add-on">
                                                                                        <asp:LinkButton ID="LBNewDepartment" runat="server" CssClass="tooltips" Font-Underline="false"
                                                                                            data-placement="top" data-original-title="Add New Department">
                                                                                                <i class="icon-plus" style="line-height:20px;"></i>
                                                                                        </asp:LinkButton>
                                                                                    </span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Employee Designation</span>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="DDLEmployeeDesignation" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <span class="add-on">
                                                                                        <asp:LinkButton ID="LBNewDesignation" runat="server" CssClass="tooltips" Font-Underline="false"
                                                                                            data-placement="top" data-original-title="Add New Designation">
                                                                                                <i class="icon-plus" style="line-height:20px;"></i>
                                                                                        </asp:LinkButton>
                                                                                    </span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <label class="checkbox">
                                                                                <asp:CheckBox ID="CBNotAnEmployee" runat="server" />
                                                                                Not an Employee
                                                                            </label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="span6">
                                                                <div class="widget">
                                                                    <div class="widget-title">
                                                                        <h4>
                                                                            Photo</h4>
                                                                    </div>
                                                                    <div class="widget-body" style="height: 150px;">
                                                                        <div class="row-fluid">
                                                                            <div class="span4">
                                                                                <div class="control-group">
                                                                                    <asp:FileUpload ID="FULogo" runat="server" onchange="showpreview(this);" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="span4">
                                                                                <div class="control-group">
                                                                                    <img id="imgpreview" width="100%" alt="logo" src="" style="border-width: 0px; visibility: hidden;" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="span4">
                                                                                <div class="control-group">
                                                                                    <asp:Image ID="ImgLogo" runat="server" Width="100%" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="widget">
                                                                    <div class="widget-title">
                                                                        <h4>
                                                                            Login Details</h4>
                                                                    </div>
                                                                    <div class="widget-body">
                                                                        <div class="row-fluid">
                                                                            <div class="control-group">
                                                                                <span class="control-label">User Name</span>
                                                                                <div class="controls">
                                                                                    <div class="input-prepend">
                                                                                        <span class="add-on"><i class="icon icon-user" style="line-height: 20px;"></i></span>
                                                                                        <asp:TextBox ID="TBUserName" runat="server" placeholder="User Name"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="control-group">
                                                                                <span class="control-label">Password</span>
                                                                                <div class="controls">
                                                                                    <div class="input-prepend">
                                                                                        <span class="add-on"><i class="icon icon-key" style="line-height: 20px;"></i></span>
                                                                                        <asp:TextBox ID="TBPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="control-group">
                                                                                <span class="control-label">Retype Password</span>
                                                                                <div class="controls">
                                                                                    <div class="input-prepend">
                                                                                        <span class="add-on"><i class="icon icon-key" style="line-height: 20px;"></i></span>
                                                                                        <asp:TextBox ID="TBRPassword" runat="server" TextMode="Password" placeholder="Retype Password"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="control-group">
                                                                                <label class="checkbox">
                                                                                    <asp:CheckBox ID="CBAdminPermission" runat="server" />
                                                                                    Can not Login without Administrator Permission (Need Active Session)
                                                                                </label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="widget">
                                                                    <div class="widget-title">
                                                                        <h4>
                                                                            Additional Details</h4>
                                                                    </div>
                                                                    <div class="widget-body" style="height: 75px; overflow-y: auto;">
                                                                        <div class="control-group">
                                                                            <span class="control-label">Working Shift</span>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="DDLWorkingShift" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <span class="add-on">
                                                                                        <asp:LinkButton ID="LBNewWorkingShift" runat="server" CssClass="tooltips" Font-Underline="false"
                                                                                            data-placement="top" data-original-title="Add New Working Shift">
                                                                                                <i class="icon-plus" style="line-height:20px;"></i>
                                                                                        </asp:LinkButton>
                                                                                    </span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row-fluid">
                                                        <div class="span12">
                                                            <div class="span6">
                                                                <div class="widget">
                                                                    <div class="widget-title">
                                                                        <h4>
                                                                            Address</h4>
                                                                    </div>
                                                                    <div class="widget-body">
                                                                        <div class="control-group">
                                                                            <span class="control-label">Address Line 1 :</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBAddressLine1" runat="server" placeholder="Address Line 1"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Address Line 2 :</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBAddressLine2" runat="server" placeholder="Address Line 2"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">City :</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBCity" runat="server" placeholder="City"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">State :</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBState" runat="server" placeholder="State"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Country :</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBCountry" runat="server" placeholder="Country"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Pin :</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBPin" runat="server" placeholder="PIN Code"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="span6">
                                                                <div class="widget">
                                                                    <div class="widget-title">
                                                                        <h4>
                                                                            Contact</h4>
                                                                    </div>
                                                                    <div class="widget-body">
                                                                        <div class="control-group">
                                                                            <span class="control-label">Phone Number :</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBPhoneNumber" runat="server" placeholder="Phone Number"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Mobile Number :</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBMobileNumber" runat="server" placeholder="Mobile Number"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Fax Number :</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBFaxNumber" runat="server" placeholder="Fax Number"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">E Mail :</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBEmail" runat="server" placeholder="Email ID"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <span class="control-label">Website :</span>
                                                                            <div class="controls">
                                                                                <asp:TextBox ID="TBWebsite" runat="server" placeholder="Website"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%--<div class="tab-pane" id="tab_1_2">
                                                Section 2.
                                            </div>
                                            <div class="tab-pane" id="tab_1_3">
                                                Section 3.
                                            </div>
                                            <div class="tab-pane" id="tab_1_4">
                                                Section 4.
                                            </div>--%>
                                            </div>
                                        </div>
                                        <!--END TABS-->
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid text-right">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                        OnClick="EmpSubmitCancel" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                        OnClick="EmpSubmitCancel" />
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <div class="row-fluid">
                            <div class="span12">
                                <div class="control-group">
                                    <span class="control-label">Download Sample File : </span>
                                    <div class="controls">
                                        <asp:LinkButton ID="LBFile" runat="server" OnClick="LBFile_Click" Style="text-decoration: none;
                                            line-height: 30px;">
                                            <i class="icon-file" ></i>  File Formate
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <span class="control-label">Upload Your File : </span>
                                    <div class="controls">
                                        <asp:FileUpload ID="FUPO" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="row-fluid text-right">
                            <asp:Button ID="BtnUploadPOSubmit" runat="server" Text="Upload" CssClass="btn btn-success"
                                OnClick="ModelSubmitCancel" />
                            <asp:Button ID="BtnUploadPOCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                OnClick="ModelSubmitCancel" />
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
        <div>
            <!-- Various Models-->
            <!-- For Employee Department-->
            <asp:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mpe" runat="server"
                PopupControlID="pnlPopupDep" TargetControlID="LBNewDepartment" BackgroundCssClass="modalBackground"
                CancelControlID="ModelBtnCancelDep">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlPopupDep" runat="server" CssClass="pnlBackGround" Style="display: none">
                <div class="modal span10 offset1" style="left: 0;">
                    <div class="modal-header">
                        <h3 id="myModalLabel1">
                            Item Categories</h3>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UP1" runat="server">
                            <ContentTemplate>
                                <uc1:EmployeeDepartment ID="EmployeeDepartment1" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="ModelBtnSubmitDep" runat="server" Text="Submit" CssClass="btn btn-success"
                            OnClick="ModelsBtnClickEvents" />
                        <asp:Button ID="ModelBtnCancelDep" runat="server" Text="Close" CssClass="btn btn-danger" />
                    </div>
                </div>
            </asp:Panel>
            <!-- For Employee Designation-->
            <asp:ModalPopupExtender ID="ModalPopupExtender2" BehaviorID="mpe1" runat="server"
                PopupControlID="pnlPopupDesi" TargetControlID="LBNewDesignation" BackgroundCssClass="modalBackground"
                CancelControlID="ModelBtnCancelDesi">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlPopupDesi" runat="server" CssClass="pnlBackGround" Style="display: none">
                <div class="modal span10 offset1" style="left: 0;">
                    <div class="modal-header">
                        <h3 id="H1">
                            Item Categories</h3>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <uc2:EmployeeDesignation ID="EmployeeDesignation1" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="ModelBtnSubmitDesi" runat="server" Text="Submit" CssClass="btn btn-success"
                            OnClick="ModelsBtnClickEvents" />
                        <asp:Button ID="ModelBtnCancelDesi" runat="server" Text="Close" CssClass="btn btn-danger" />
                    </div>
                </div>
            </asp:Panel>
            <!-- For Employee Working Shift-->
            <asp:ModalPopupExtender ID="ModalPopupExtender3" BehaviorID="mpe2" runat="server"
                PopupControlID="pnlPopupWorShift" TargetControlID="LBNewWorkingShift" BackgroundCssClass="modalBackground"
                CancelControlID="ModelBtnCancelWorkShift">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlPopupWorShift" runat="server" CssClass="pnlBackGround" Style="display: none">
                <div class="modal span10 offset1" style="left: 0;">
                    <div class="modal-header">
                        <h3 id="H2">
                            Item Categories</h3>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <uc3:EmployeeWorkShift ID="EmployeeWorkShift1" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="ModelBtnSubmitWorkShift" runat="server" Text="Submit" CssClass="btn btn-success"
                            OnClick="ModelsBtnClickEvents" />
                        <asp:Button ID="ModelBtnCancelWorkShift" runat="server" Text="Close" CssClass="btn btn-danger" />
                    </div>
                </div>
            </asp:Panel>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END PAGE CONTAINER-->
</asp:Content>
