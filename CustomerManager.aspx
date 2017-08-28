<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="CustomerManager.aspx.cs" Inherits="CustomerManager" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
                    Customer Manager <small>Create & Manage Customers</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Customer</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="CustomerManager.aspx">Customer Manager</a><span class="divider-last">&nbsp;</span></li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <asp:MultiView ID="MVCustomers" runat="server" ActiveViewIndex="0">
                    <asp:View ID="View0" runat="server">
                        <div class="widget">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-globe"></i>Manage Customers</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <asp:LinkButton ID="LBAddNew" runat="server" CssClass="btn btn-info" OnClick="TopBarControls">
                                                <i class="icon-plus"></i> New
                                        </asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="LBExprotRecord" runat="server" CssClass="btn btn-info" OnClick="LBExportToExcel_Click">
                                                <i class="icon-download-alt"></i> Export
                                        </asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="LBImportRecord" runat="server" CssClass="btn btn-info" OnClick="LBUploadPO_Click">
                                                <i class="icon-upload-alt"></i> Import
                                        </asp:LinkButton>&nbsp;
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span2">
                                            <label class="checkbox">
                                                <asp:CheckBox ID="CBShowImage" runat="server" Checked="true" AutoPostBack="true"
                                                    OnCheckedChanged="ChRadioControls" />
                                                <span>Show Images</span>
                                            </label>
                                        </div>
                                        <div class="span2">
                                            <label class="radio">
                                                <asp:RadioButton ID="RBShowActive" runat="server" GroupName="ShowRecord" AutoPostBack="true"
                                                    OnCheckedChanged="ChRadioControls" />
                                                <span>Show Active</span>
                                            </label>
                                        </div>
                                        <div class="span2">
                                            <label class="radio">
                                                <asp:RadioButton ID="RBShowInactive" runat="server" GroupName="ShowRecord" AutoPostBack="true"
                                                    OnCheckedChanged="ChRadioControls" />
                                                <span>Show Inactive</span>
                                            </label>
                                        </div>
                                        <div class="span2">
                                            <label class="radio">
                                                <asp:RadioButton ID="RBShowAll" runat="server" Checked="true" GroupName="ShowRecord"
                                                    AutoPostBack="true" OnCheckedChanged="ChRadioControls" />
                                                <span>Show All</span>
                                            </label>
                                        </div>
                                        <div class="span4">
                                            <asp:TextBox ID="TBSearchText" runat="server" placeholder="Search Text"></asp:TextBox>
                                            <asp:LinkButton ID="LBSearch" runat="server" CssClass="btn btn-info" OnClick="SearchEvent">
                                                <i class="icon-search" style="line-height:18px;"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVCustomer" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" HeaderStyle-Wrap="false"
                                            HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                            CellPadding="10" RowStyle-VerticalAlign="Middle" ShowHeaderWhenEmpty="true" RowStyle-Wrap="false"
                                            OnPageIndexChanging="GVCustomer_PageIndexChanging" OnRowCommand="GVCustomer_RowCommand">
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
                                                <asp:BoundField HeaderText="Customer Code" DataField="CustomerCode" />
                                                <asp:BoundField HeaderText="Customer Type" DataField="CustomerType" />
                                                <asp:BoundField HeaderText="Name" DataField="CustomerName" />
                                                <asp:BoundField HeaderText="Company" DataField="CompanyName" />
                                                <asp:BoundField HeaderText="City" DataField="City" />
                                                <asp:BoundField HeaderText="Phone No" DataField="PhoneNumber" />
                                                <asp:BoundField HeaderText="Mobile No" DataField="MobileNumber" />
                                                <asp:BoundField HeaderText="Fax Number" DataField="FaxNumber" />
                                                <asp:BoundField HeaderText="EmailID" DataField="EmailID" />
                                                <asp:BoundField HeaderText="BusinessLocation" DataField="BusinessLocation" />
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
                                <hr />
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="View1" runat="server">
                        <div class="widget">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-globe"></i>Add New Customers</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span7">
                                            <div class="control-group">
                                                <span class="control-label">Customer Code :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBCustomerCode" runat="server" placeholder="Customer Code"></asp:TextBox>
                                                    <asp:LinkButton ID="LBCustomerCode" runat="server" Text="Define My Own" CssClass="btn"
                                                        OnClick="DefineMyOwn"></asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Customer Type :</span>
                                                <div class="controls">
                                                    <label class="radio">
                                                        <asp:RadioButton ID="RBIndividual" runat="server" GroupName="CustomerType" />
                                                        <span>Individual</span>
                                                    </label>
                                                    <label class="radio">
                                                        <asp:RadioButton ID="RBCompany" runat="server" GroupName="CustomerType" />
                                                        <span>Company</span>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Business Location :</span>
                                                <div class="controls">
                                                    <asp:DropDownList ID="DDLBusinessLocation" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Search Code :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBSearchCode" runat="server" placeholder="Search Code"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5 text-center">
                                            <asp:FileUpload ID="FULogo" runat="server" onchange="showpreview(this);" Style="top: 0;
                                                display: block;" />
                                            <img id="imgpreview" alt="logo" src="" style="border-width: 0px; visibility: hidden;
                                                height: 200px;" />
                                            <asp:Image ID="ImgLogo" runat="server" Width="200px" />
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">First Name :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBFirstName" runat="server" placeholder="First Name"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Last Name :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBLastName" runat="server" placeholder="Last Name"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Company Name :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBCompanyName" runat="server" placeholder="Company Name"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Birth Date :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBDOB" runat="server" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CEFPOdate" runat="server" Format="dd/MM/yyyy" TargetControlID="TBDOB"
                                                        PopupButtonID="TBDOB">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Anniversary Date :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBAnniversaryDate" runat="server" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="TBAnniversaryDate"
                                                        PopupButtonID="TBAnniversaryDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <span class="control-label">VAT Number :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBVATNumber" runat="server" placeholder="VAT Number"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">CST Number :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBCSTNumber" runat="server" placeholder="CST Number"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">TIN Number :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBTINNumber" runat="server" placeholder="TINNumber"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">PAN Number :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBPANNumber" runat="server" placeholder="PAN Number"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <div class="controls">
                                                    <label class="checkbox">
                                                        <asp:CheckBox ID="CBIsActive" runat="server" Checked="true" />
                                                        <span>IsActive</span>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
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
                                                <span class="control-label">Pincode :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBPinCode" runat="server" placeholder="Pin Code"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
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
                                                <span class="control-label">Email ID :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBEmailID" runat="server" placeholder="example@domain.com"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Website :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBWebsite" runat="server" placeholder="http://www.domain.com"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        Contact Details
                                        <asp:LinkButton ID="LBAddNewContact" runat="server" CssClass="btn" Text="Add New"></asp:LinkButton>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12">
                                        <asp:Repeater ID="RepeaterAddressDetails" runat="server" OnItemCommand="RepeaterAddressDetails_ItemCommand">
                                            <ItemTemplate>
                                                <div class="span4 alert alert-block alert-success fade in">
                                                    <asp:LinkButton ID="LBDelContact" runat="server" class="close" CommandName="DelContact"
                                                        CommandArgument='<%# Eval("ID") %>'>X
                                                    </asp:LinkButton>
                                                    <h4>
                                                        <%# Eval("AddressType")%>
                                                    </h4>
                                                    <hr />
                                                    <div class="row-fluid">
                                                        <div class="span4">
                                                            <img src="Images/Contact.png" style="width: 70px; margin: 30px auto;" />
                                                        </div>
                                                        <div class="span8">
                                                            <%# Eval("Title").ToString() == "1" ? "Mr" : Eval("Title").ToString() == "2" ? "Mrs" : Eval("Title").ToString() == "3" ? "Miss" : Eval("Title").ToString() == "4" ? "Master" : Eval("Title").ToString() == "5" ? "Sir" : Eval("Title").ToString() == "6" ? "Madam" : ""%>
                                                            <%# Eval("ContactName") %>
                                                            <br />
                                                            <%# Eval("Position") %>
                                                            <br />
                                                            <%# Eval("EmailID") %>
                                                            <br />
                                                            <%# Eval("AddressLine1") %>
                                                            <br />
                                                            <%# Eval("AddressLine2") %>
                                                            <br />
                                                            <%# Eval("City") %>,<%# Eval("PinCode") %><br />
                                                            <%# Eval("PhoneNumber") %>,
                                                            <%# Eval("MobileNumber") %><br />
                                                            <%# Eval("Note") %><br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid text-right">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                        OnClick="BtnSubmitCancel" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                        OnClick="BtnSubmitCancel" />
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
        <!-- END PAGE CONTENT-->
        <!-- Model Sections For Add Contact Details-->
        <asp:ModalPopupExtender ID="Model" BehaviorID="mpe1" runat="server" PopupControlID="pnlContact"
            TargetControlID="LBAddNewContact" BackgroundCssClass="modalBackground" CancelControlID="ModelContactCancel">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlContact" runat="server" CssClass="pnlBackGround" Style="display: none">
            <div class="modal span10 offset1" style="left: 0;">
                <div class="modal-header">
                    <h3 id="H5">
                        Contact Details</h3>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <div class="row-fluid">
                                <div class="span12">
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Title</span>
                                            <div class="controls">
                                                <asp:DropDownList ID="DDLTitle" runat="server">
                                                    <asp:ListItem Text="- Select -" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Mr" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Mrs" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Miss" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Master" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Sir" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="Madam" Value="6"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Contact Name</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBContactName" runat="server" placeholder="Contact Name"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Job Position</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBJobPosition" runat="server" placeholder="Job Position"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Email</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBContactEmail" runat="server" placeholder="Ex. example@domainname"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Phone</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBContactPhone" runat="server" placeholder="Phone No"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Mobile</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBContactMobile" runat="server" placeholder="Mobile"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Notes</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBContactNotes" runat="server" placeholder="Notes"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <span class="control-label">Address Type</span>
                                            <div class="controls">
                                                <asp:DropDownList ID="DDLAddressType" runat="server">
                                                    <asp:ListItem Text="Select Address Type" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Invoice Address" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Shipping Address" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Other Address" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Address Line 1</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBContactAddLine1" runat="server" placeholder="Address Line 1"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Address Line 2</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBContactAddLine2" runat="server" placeholder="Address Line 2"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">City</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBContactCity" runat="server" placeholder="City"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">State</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBContactState" runat="server" placeholder="State"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Country</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBContactCountry" runat="server" placeholder="Country"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <span class="control-label">Pincode</span>
                                            <div class="controls">
                                                <asp:TextBox ID="TBContactPinCode" runat="server" placeholder="Pin Code"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="ModelContactSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                        OnClick="CustomerContactSubmitCancel" />
                    <asp:Button ID="ModelContactCancel" runat="server" Text="Close" CssClass="btn btn-danger"
                        OnClick="CustomerContactSubmitCancel" />
                </div>
            </div>
        </asp:Panel>
    </div>
    <!-- END PAGE CONTAINER-->
</asp:Content>
