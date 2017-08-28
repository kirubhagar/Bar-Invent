<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="Setting.aspx.cs" Inherits="Setting" %>

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
                    Settings <small>Manage All system settings from here....</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Setting</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="Setting.aspx">System Settings</a><span class="divider-last">&nbsp;</span></li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <asp:MultiView ID="MVSettings" runat="server" ActiveViewIndex="0">
                    <!-- View For All Settings-->
                    <asp:View ID="View0" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                    <div class="row-fluid">
                                        <asp:LinkButton ID="LBInvoice" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-file"></i><div>INVOICES</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBPrinter" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-print"></i><div>PRINTERS</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBCurrency" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-money"></i><div>CURRENCY</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBTechnicalSetting" runat="server" CssClass="icon-btn span2"
                                            OnClick="SettingClickEvents">
                                                <i class="icon-wrench"></i><div>TECHNICAL SETTINGS</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBGeneral" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-cog"></i><div>GENERAL</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBDateFormat" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-calendar"></i><div>DATE FORMAT</div> 
                                        </asp:LinkButton>
                                    </div>
                                    <div class="row-fluid">
                                        <asp:LinkButton ID="LBBackUp" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-refresh"></i><div>BACKUP</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBEmailSetting" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-envelope"></i><div>EMAIL SETTINGS</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBWeighingScale" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-sign-blank"></i><div>WEIGHING SCALE</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBCodeFormat" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-certificate"></i><div>CODE FORMAT</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBFAFields" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-group"></i><div>FA FIELDS</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBItemFields" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-group"></i><div>ITEM FIELDS</div> 
                                        </asp:LinkButton>
                                    </div>
                                    <div class="row-fluid">
                                        <asp:LinkButton ID="LBCustomerFields" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-group"></i><div>CUSTOMER FIELDS</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBSupplierFields" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-truck"></i><div>SUPPLIER FIELDS</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBCIFields" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-group"></i><div>C.I. FIELDS</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBRearrange" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-group"></i><div>REARRANGE</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBCloudSetting" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-cloud"></i><div>CLOUD SETTING</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBCustomerDisplay" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-sign-blank"></i><div>CUSTOMER DISPLAY</div> 
                                        </asp:LinkButton>
                                    </div>
                                    <div class="row-fluid">
                                        <asp:LinkButton ID="LBUserInterface" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-th-large"></i><div>USER INTERFACE</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBOnlineShop" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-globe"></i><div>ONLINE SHOP</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBFASetting" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-group"></i><div>FA SETTINGS</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBCategoryTree" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-sitemap"></i><div>CATEGORY TREE</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBSMSSetting" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-envelope"></i><div>SMS SETTING</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBAppointmtFields" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-calendar"></i><div>APPOINTMT FIELDS</div> 
                                        </asp:LinkButton>
                                    </div>
                                    <div class="row-fluid">
                                        <asp:LinkButton ID="LBAdditional" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-key"></i><div>ADDITIONAL</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBEmployeeFields" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-user"></i><div>EMPOYEE FIELDS</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBTimeTokenFields" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-time"></i><div>TIME TOKEN FIELDS</div> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LBDeveloperGuide" runat="server" CssClass="icon-btn span2" OnClick="SettingClickEvents">
                                                <i class="icon-book"></i><div>DEVELOPER GUIDE</div> 
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Invoice-->
                    <asp:View ID="View1" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Invoice Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Printer-->
                    <asp:View ID="View2" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Printer Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Currency-->
                    <asp:View ID="View3" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Currency Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Technical Setting-->
                    <asp:View ID="View4" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Technical Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For General-->
                    <asp:View ID="View5" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>General Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                    <!--BEGIN TABS-->
                                    <div class="tabbable tabbable-custom">
                                        <ul class="nav nav-tabs">
                                            <li class="active"><a href="#tab_1_1" data-toggle="tab">Login User Setting</a></li>
                                            <li><a href="#tab_1_2" data-toggle="tab">Section 2</a></li>
                                            <li><a href="#tab_1_3" data-toggle="tab">Section 3</a></li>
                                        </ul>
                                        <div class="tab-content">
                                            <div class="tab-pane active" id="tab_1_1">
                                                <p>
                                                    Login Users </p>

                                                <div class="row-fluid">
                                                    <div class="span12">
                                                        <div class="span6">
                                                            <div class="control-group">
                                                                <span class="control-label">Show Login User : </span>
                                                                <div class="controls">
                                                                    <asp:DropDownList ID="DDLLoginUser" runat="server">
                                                                        <asp:ListItem Text="NGo Level" Value="0"></asp:ListItem>
                                                                        <asp:ListItem Text="No Level" Value="1"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <hr />
                                                <div class="row-fluid text-right">
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-info" />
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="tab_1_2">
                                                <p>
                                                    Section 2.</p>
                                                <p>
                                                    Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis
                                                    nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit
                                                    in vulputate velit esse molestie consequat. Ut wisi enim ad minim veniam, quis nostrud
                                                    exerci tation.
                                                </p>
                                            </div>
                                            <div class="tab-pane" id="tab_1_3">
                                                <p>
                                                    Section 3.</p>
                                                <p>
                                                    Duis autem vel eum iriure dolor in hendrerit in vulputate. Ut wisi enim ad minim
                                                    veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip
                                                    ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate
                                                    velit esse molestie consequat
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                    <!--END TABS-->
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Date Format-->
                    <asp:View ID="View6" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Date Format Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For BackUp-->
                    <asp:View ID="View7" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>BackUp Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Email Setting-->
                    <asp:View ID="View8" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Email Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Weighing Scale-->
                    <asp:View ID="View9" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Weighing Scale Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Code Format-->
                    <asp:View ID="View10" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Code Format Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                    <div class="row-fluid">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Select Type : </span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLDocumentType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLDocumentType_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Current Code Format : </span>
                                                <div class="controls MyLable">
                                                    <asp:Label ID="LBLCurrentCodeFormat" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Select Business Location : </span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLBusinessLocation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CodeControlChange">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Business Location Prefix : </span>
                                                <div class="controls MyLable">
                                                    <asp:Label ID="LBLBusinessLocationPrefix" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Document Prefix : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBDocumentPrefix" runat="server" AutoPostBack="true" OnTextChanged="CodeControlChange"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Minimum Code Length : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBMinimumCodeLength" runat="server" AutoPostBack="true" OnTextChanged="CodeControlChange"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Code Start Number : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBCodeStartNumber" runat="server" AutoPostBack="true" OnTextChanged="CodeControlChange"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">Sample Code : </span>
                                                <div class="controls MyLable">
                                                    <asp:Label ID="LBLSampleCode" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row-fluid text-right">
                                        <asp:Button ID="btnApplyForCode" runat="server" Text="Apply" CssClass="btn btn-success"
                                            OnClick="CodeSubmitCancelEvent" />
                                        <asp:Button ID="btnCancelForCode" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                            OnClick="CodeSubmitCancelEvent" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For FA Fields-->
                    <asp:View ID="View11" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>FA Fields Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Item Fields-->
                    <asp:View ID="View12" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Item Fields Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Customer Fields-->
                    <asp:View ID="View13" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Customer Fields Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Supplier Fields-->
                    <asp:View ID="View14" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Supplier Fields Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For CI Fields-->
                    <asp:View ID="View15" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>CI Fields Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Rearrange-->
                    <asp:View ID="View16" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Rearrange Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Cloud Setting-->
                    <asp:View ID="View17" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Cloud Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Customer Fields-->
                    <asp:View ID="View18" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Customer Fields Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For User Interface-->
                    <asp:View ID="View19" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>User Interface Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Online Shop-->
                    <asp:View ID="View20" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Online Shop Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For FA Setting-->
                    <asp:View ID="View21" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>FA Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Category Tree-->
                    <asp:View ID="View22" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Category Tree Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For SMS Setting-->
                    <asp:View ID="View23" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>SMS Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Appointmt Fields-->
                    <asp:View ID="View24" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Appointmt Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Additional-->
                    <asp:View ID="View25" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Additional Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Employee Fields-->
                    <asp:View ID="View26" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Employee Fields Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Time Token Fields-->
                    <asp:View ID="View27" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Time Token Settings</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <!-- View For Developer Option-->
                    <asp:View ID="View28" runat="server">
                        <div class="row-fluid">
                            <div class="widget">
                                <div class="widget-title">
                                    <h4>
                                        <i class="icon-globe"></i>Develoer Option</h4>
                                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                                </div>
                                <div class="widget-body">
                                    <div class="row-fluid">
                                        <div class="span6 offset3">
                                            <div class="control-group">
                                                <span class="control-label">Document Type</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBDocumentType" runat="server" placeholder="Document Type"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <asp:CheckBox ID="CBApplicableBL" runat="server" />
                                                <span>Is Business Location Applicable For Type</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid text-right">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                            OnClick="BtnSubmitCancelDT" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                            OnClick="BtnSubmitCancelDT" />
                                    </div>
                                    <hr />
                                    <div class="row-fluid">
                                        <asp:GridView ID="GVDocumentType" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" OnPageIndexChanging="GVDocumentType_PageIndexChanging"
                                            OnRowCommand="GVDocumentType_RowCommand" HeaderStyle-Wrap="false" HeaderStyle-Font-Bold="true"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" CellPadding="10"
                                            RowStyle-VerticalAlign="Middle">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Name" DataField="Name" />
                                                <asp:BoundField HeaderText="Business Location Applicable" DataField="IsBLApplicable" />
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
                </asp:MultiView>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END PAGE CONTAINER-->
</asp:Content>
