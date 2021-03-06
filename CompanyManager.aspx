﻿<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="CompanyManager.aspx.cs" Inherits="CompanyManager" %>

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
                    Company Manager <small>Create & Manage Company details...</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Organization</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="CompanyManager.aspx">Company Manager</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>Company Manager</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <asp:MultiView ID="MVCompany" runat="server" ActiveViewIndex="0">
                            <asp:View ID="View0" runat="server">
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span4 text-center">
                                            <asp:LinkButton ID="LBAddNew" runat="server" CssClass="btn btn-info" OnClick="BtnAddNewClick">
                                                <i class="icon-plus"></i> Add New
                                            </asp:LinkButton>
                                        </div>
                                        <div class="span4 text-center">
                                            <asp:TextBox ID="TBSearch" runat="server" placeholder="Type Company Name To Search"></asp:TextBox>
                                            <asp:LinkButton ID="LBSearch" runat="server" CssClass="btn btn-info" OnClick="SearchEvent">
                                                <i class="icon-search" style="line-height:17px;"></i>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="span4 text-center">
                                            <asp:CheckBox ID="CBShowImage" runat="server" Checked="true" AutoPostBack="true"
                                                OnCheckedChanged="ShowHideImageEvent" />
                                            <span>Show Images</span>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12" style="overflow-x: auto;">
                                        <asp:GridView ID="GVCompany" runat="server" CssClass="table table-advance table-bordered table-condensed table-hover"
                                            Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" GridLines="Both" AllowPaging="true"
                                            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" HeaderStyle-Wrap="false"
                                            HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                            CellPadding="10" RowStyle-VerticalAlign="Middle" ShowHeaderWhenEmpty="true" OnRowCommand="GVCompany_RowCommand"
                                            OnPageIndexChanging="GVCompany_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Image">
                                                    <ItemTemplate>
                                                        <asp:Image ID="ImgLogo" runat="server" Style="max-width: 100px;" ImageUrl='<%# Eval("Image") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Company Name" DataField="CompanyName" />
                                                <asp:BoundField HeaderText="Telephone Number" DataField="TelephoneNumber" />
                                                <asp:BoundField HeaderText="Mobile Number" DataField="MobileNumber" />
                                                <asp:BoundField HeaderText="Email" DataField="Email" />
                                                <asp:BoundField HeaderText="Website" DataField="Website" />
                                                <asp:BoundField HeaderText="TINNo" DataField="TINNo" />
                                                <asp:BoundField HeaderText="Address1" DataField="Address1" />
                                                <asp:BoundField HeaderText="Address2" DataField="Address2" />
                                                <asp:BoundField HeaderText="City" DataField="City" />
                                                <asp:BoundField HeaderText="State" DataField="State" />
                                                <asp:BoundField HeaderText="Country" DataField="Country" />
                                                <asp:BoundField HeaderText="Zip Code" DataField="ZipCode" />
                                                <asp:BoundField HeaderText="Alternate Company Name" DataField="AlternateCompanyName" />
                                                <asp:BoundField HeaderText="TAN Number" DataField="TANNumber" />
                                                <asp:BoundField HeaderText="PAN Number" DataField="PANNumber" />
                                                <asp:BoundField HeaderText="ISO Certificate Number" DataField="ISOCertificateNumber" />
                                                <asp:BoundField HeaderText="Excise Duty Number" DataField="ExciseDutyNumber" />
                                                <asp:BoundField HeaderText="Fax Number" DataField="FaxNumber" />
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
                                        <div class="span6">
                                            <h4>
                                                General Information</h4>
                                            <hr />
                                            <div class="control-group">
                                                <span class="control-label">Company Name :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBCompanyName" runat="server" placeholder="CompanyName"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">GSTIN Number :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBTINNumber" runat="server" placeholder="GSTIN Number"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Alternate Company Name :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBAlternateCompanyName" runat="server" placeholder="Alternate Company Name"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">TAN Number :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBTANNumber" runat="server" placeholder="TAN Number"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">PAN Number :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBPANNumber" runat="server" placeholder="PAN Number"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">ISO Certificate Number :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBISOCertificateNumber" runat="server" placeholder="ISO Certificate Number"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Excise Duty Number :</span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBExciseDutyNumber" runat="server" placeholder="Excise Duty Number"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="span8 offset2" style="height:225px;">
                                                <h4>
                                                    Image Information</h4>
                                                <hr />
                                                <div class="control-group text-center">
                                                    <asp:FileUpload ID="FULogo" runat="server" onchange="showpreview(this);" />
                                                    <img id="imgpreview" height="100" width="100" alt="logo" src="" style="border-width: 0px;
                                                        visibility: hidden;" />
                                                    <asp:Image ID="ImgLogo" runat="server" Width="100px" />
                                                </div>
                                            </div>
                                            <div class="space15">
                                            </div>
                                            <h4>
                                                Godown Information</h4>
                                            <hr />
                                            <div class="control-group">
                                                <span class="control-label">Godown Name : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBGodownName" runat="server" placeholder="Godown Name"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Short Form : </span>
                                                <div class="controls">
                                                    <asp:TextBox ID="TBGodownShortForm" runat="server" placeholder="Short Form"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <h4>
                                                Address Information</h4>
                                            <hr />
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
                                        <div class="span6">
                                            <h4>
                                                Contact Information</h4>
                                            <hr />
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
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="span6">
                                            <h4>
                                                Company Login Details</h4>
                                            <hr />
                                            <div class="control-group">
                                                <span class="control-label">User Name : </span>
                                                <div class="controls">
                                                    <asp:UpdatePanel ID="UPUN" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-prepend">
                                                                <span class="add-on"><i class="icon icon-user" style="line-height: 20px;"></i></span>
                                                                <asp:TextBox ID="TBUserName" runat="server" placeholder="UserName" AutoPostBack="true"
                                                                    OnTextChanged="TBUserName_TextChanged"></asp:TextBox>
                                                            </div>
                                                            <span class="help-block" id="UserHelp" runat="server"></span>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="TBUserName" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Password : </span>
                                                <div class="controls">
                                                    <div class="input-prepend">
                                                        <span class="add-on"><i class="icon icon-key" style="line-height: 20px;"></i></span>
                                                        <asp:TextBox ID="TBPassWord" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <span class="control-label">Retype Password : </span>
                                                <div class="controls">
                                                    <div class="input-prepend">
                                                        <span class="add-on"><i class="icon icon-key" style="line-height: 20px;"></i></span>
                                                        <asp:TextBox ID="TBRPassWord" runat="server" TextMode="Password" placeholder="Retype Password"></asp:TextBox>
                                                    </div>
                                                    <asp:CompareValidator ID="CV" runat="server" CssClass="help-block" ControlToValidate="TBRPassWord"
                                                        ControlToCompare="TBPassWord" Operator="Equal" ErrorMessage="PassWord MisMatch !"
                                                        ForeColor="Red" SetFocusOnError="true" ValidationGroup="SubCom"></asp:CompareValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span4 offset1">
                                            <h4>
                                                User Image Information</h4>
                                            <hr />
                                            <div class="control-group text-center">
                                                <asp:FileUpload ID="FUUser" runat="server" onchange="showpreview1(this);" />
                                                <img id="imgpreview1" height="100" width="100" alt="UserImg" src="" style="border-width: 0px;
                                                    visibility: hidden;" />
                                                <asp:Image ID="ImgUser" runat="server" Width="100px" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row-fluid">
                                    <div class="span12 text-right">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success"
                                            ValidationGroup="SubCom" OnClick="BtnSubmitCancel" />
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
