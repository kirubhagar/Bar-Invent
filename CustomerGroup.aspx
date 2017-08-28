<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true" CodeFile="CustomerGroup.aspx.cs" Inherits="CustomerGroup" %>

<%@ Register src="MyControls/CustomerGroup.ascx" tagname="CustomerGroup" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- BEGIN PAGE CONTAINER-->
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                    Customer Group  <small>Create & manage Customers Groups...</small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="Dashboard.aspx"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">Customer</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="CustomerGroup.aspx">Customer Group</a><span class="divider-last">&nbsp;</span></li>
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
                            <i class="icon-globe"></i>Customer Group</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
                    </div>
                    <div class="widget-body">
                        
                        <uc1:CustomerGroup ID="CustomerGroup1" runat="server" />
                        
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END PAGE CONTAINER-->
</asp:Content>

