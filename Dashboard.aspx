<%@ Page Title="" Language="C#" MasterPageFile="~/WMSMain.master" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="//www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load('visualization', '1', { packages: ['corechart'] });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Dashboard.aspx/GetData',
                data: '{}',
                success:
                    function (response) {
                        drawVisualization(response.d);
                    }
            });
        })

        function drawVisualization(dataValues) {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Column Name');
            data.addColumn('number', 'Column Value');
            for (var i = 0; i < dataValues.length; i++) {
                data.addRow([dataValues[i].ColumnName, dataValues[i].Value]);
            }
            new google.visualization.PieChart(document.getElementById('visualization')).
                draw(data, { title: "Kit Material Vs Raw Materials" });
        } 
         
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN PAGE CONTAINER-->
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                    Dashboard <small>General Information </small>
                </h3>
                <ul class="breadcrumb">
                    <li><a href="#"><i class="icon-home"></i></a><span class="divider">&nbsp;</span>
                    </li>
                    <li><a href="#">RMS</a> <span class="divider">&nbsp;</span> </li>
                    <li><a href="Dashboard.aspx">Dashboard</a><span class="divider-last">&nbsp;</span></li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div id="page" class="dashboard">
            <!-- BEGIN OVERVIEW STATISTIC BLOCKS-->
            <div class="row-fluid circle-state-overview">
                <div class="span2 responsive" data-tablet="span3" data-desktop="span2">
                    <div class="circle-stat block">
                        <div class="visual">
                            <div class="circle-state-icon">
                                <i class="icon-user turquoise-color"></i>
                            </div>
                            <input class="knob" data-width="100" data-height="100" data-displayprevious="true"
                                data-thickness=".2" data-fgcolor="#4CC5CD" data-bgcolor="#ddd" id="knobcustomer"
                                runat="server" />
                        </div>
                        <div class="details">
                            <div class="number" id="numberCustomer" runat="server">
                            </div>
                            <div class="title">
                                Purchase Order</div>
                        </div>
                    </div>
                </div>
                <div class="span2 responsive" data-tablet="span3" data-desktop="span2">
                    <div class="circle-stat block">
                        <div class="visual">
                            <div class="circle-state-icon">
                                <i class="icon-tags red-color"></i>
                            </div>
                            <input class="knob" data-width="100" data-height="100" data-displayprevious="true"
                                data-thickness=".2" value="65" data-fgcolor="#e17f90" data-bgcolor="#ddd" />
                        </div>
                        <div class="details">
                            <div class="number" id="numberSales" runat="server">
                            </div>
                            <div class="title">
                                Purchase Receive</div>
                        </div>
                    </div>
                </div>
                <div class="span2 responsive" data-tablet="span3" data-desktop="span2">
                    <div class="circle-stat block">
                        <div class="visual">
                            <div class="circle-state-icon">
                                <i class="icon-shopping-cart green-color"></i>
                            </div>
                            <input class="knob" data-width="100" data-height="100" data-displayprevious="true"
                                data-thickness=".2" value="30" data-fgcolor="#a8c77b" data-bgcolor="#ddd" />
                        </div>
                        <div class="details">
                            <div class="number">
                                +320</div>
                            <div class="title">
                                Issue Request</div>
                        </div>
                    </div>
                </div>
                <div class="span2 responsive" data-tablet="span3" data-desktop="span2">
                    <div class="circle-stat block">
                        <div class="visual">
                            <div class="circle-state-icon">
                                <i class="icon-comments-alt gray-color"></i>
                            </div>
                            <input class="knob" data-width="100" data-height="100" data-displayprevious="true"
                                data-thickness=".2" value="15" data-fgcolor="#b9baba" data-bgcolor="#ddd" />
                        </div>
                        <div class="details">
                            <div class="number">
                                387</div>
                            <div class="title">
                                Comments</div>
                        </div>
                    </div>
                </div>
                <div class="span2 responsive" data-tablet="span3" data-desktop="span2">
                    <div class="circle-stat block">
                        <div class="visual">
                            <div class="circle-state-icon">
                                <i class="icon-eye-open purple-color"></i>
                            </div>
                            <input class="knob" data-width="100" data-height="100" data-displayprevious="true"
                                data-thickness=".2" value="45" data-fgcolor="#c8abdb" data-bgcolor="#ddd" />
                        </div>
                        <div class="details">
                            <div class="number">
                                +987</div>
                            <div class="title">
                                Unique Visitor</div>
                        </div>
                    </div>
                </div>
                <div class="span2 responsive" data-tablet="span3" data-desktop="span2">
                    <div class="circle-stat block">
                        <div class="visual">
                            <div class="circle-state-icon">
                                <i class="icon-bar-chart blue-color"></i>
                            </div>
                            <input class="knob" data-width="100" data-height="100" data-displayprevious="true"
                                data-thickness=".2" value="25" data-fgcolor="#93c4e4" data-bgcolor="#ddd" />
                        </div>
                        <div class="details">
                            <div class="number">
                                478</div>
                            <div class="title">
                                Updates</div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- END OVERVIEW STATISTIC BLOCKS-->
            <!-- BEGIN SQUARE STATISTIC BLOCKS-->
            <div class="square-state">
                <div class="row-fluid">
                    <a class="icon-btn span2" href="BusinessLocation.aspx"><i class="icon-sitemap"></i>
                        <div>
                            Training Center / NGO</div>
                    </a><a class="icon-btn span2" href="GodownManager.aspx"><i class="icon-truck"></i>
                        <div>
                            Godowns</div>
                    </a><a class="icon-btn span2" href="SupplierManager.aspx"><i class="icon-group"></i>
                        <div>
                            Suppliers</div>
                        <span class="badge badge-important" id="SpanSupp" runat="server"></span></a>
                    <a class="icon-btn span2" href="UserMaster.aspx"><i class="icon-group"></i>
                        <div>
                            Users</div>
                        <span class="badge badge-info" id="SpanCust" runat="server"></span></a><a class="icon-btn span2"
                            href="EmployeeManager.aspx"><i class="icon-group"></i>
                            <div>
                                Employees</div>
                            <span class="badge badge-inverse" id="SpanEmp" runat="server"></span></a>
                </div>
                <div class="row-fluid">
                </div>
            </div>
            <!-- END SQUARE STATISTIC BLOCKS-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN RECENT ORDERS PORTLET-->
                    <div class="widget">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-tags"></i>Recent Purchase Order List</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                        </div>
                        <div class="widget-body">
                            <asp:GridView ID="GVPOMaster" runat="server" Width="100%" ShowHeaderWhenEmpty="true"
                                AutoGenerateColumns="false" HeaderStyle-Wrap="false" RowStyle-Wrap="false" CssClass="table table-hover table-advance table-condensed">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="PO No" DataField="PONo" />
                                    <asp:BoundField HeaderText="PO Date" DataField="PODate" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" />
                                    <%-- <asp:BoundField HeaderText="Business Location" DataField="BusinessLocationName" Visible="false" />--%>
                                    <asp:BoundField HeaderText="Delivary Date" DataField="DeliveryDate" HtmlEncode="false"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Delivar To" DataField="DeliverTo" Visible="false" />
                                    <asp:BoundField HeaderText="Supplier" DataField="Name" />
                                    <asp:BoundField HeaderText="Supplier Refrence" DataField="SupplierRef" Visible="false" />
                                    <asp:BoundField HeaderText="Terms & Condition" DataField="TermsCondition" Visible="false" />
                                    <asp:BoundField HeaderText="Payment Terms" DataField="PaymentTerms" Visible="false" />
                                    <asp:BoundField HeaderText="Total Quantity" DataField="TotalQty" />
                                    <asp:BoundField HeaderText="Total Values" DataField="TotalValue" />
                                    <asp:BoundField HeaderText="Remark" DataField="Remark" Visible="false" />
                                    <asp:TemplateField HeaderText="POStatus">
                                        <ItemTemplate>
                                            <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("POStatus").ToString() == "0" ? "Active" : Eval("POStatus").ToString() == "1" ? "Pending" : Eval("POStatus").ToString() == "2" ? "Completed" : ""%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="space7">
                            </div>
                            <div class="clearfix">
                                <a href="POManager.aspx" class="btn btn-mini pull-right">All Orders</a>
                            </div>
                        </div>
                    </div>
                    <!-- END RECENT ORDERS PORTLET-->
                    <%--<div class="span6">
                        <!-- BEGIN Item Kit Charts-->
                        <div class="widget">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-bar-chart"></i>&nbsp; Kit Raw Materials
                                </h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                            </div>
                            <div class="widget-body">
                                <asp:DropDownList ID="DDLKit" runat="server" AutoPostBack="true" OnSelectedIndexChanged="BindRawDataChart"></asp:DropDownList>
                                <div id="visualization" style="width: 600px; height: 350px;">
                                </div>
                            </div>
                        </div>
                        <!-- BEGIN Item Kit Charts-->
                    </div>--%>
                </div>
            </div>
            <%-- <div class="row-fluid">
                <div class="span7">
                    <!-- BEGIN CHAT PORTLET-->
                    <div class="widget" id="chats">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-comments-alt"></i>Chats</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="timeline-messages">
                                <!-- Comment -->
                                <div class="msg-time-chat">
                                    <a class="message-img" href="#">
                                        <img alt="" src="img/avatar1.jpg" class="avatar"></a>
                                    <div class="message-body msg-in">
                                        <div class="text">
                                            <p class="attribution">
                                                <a href="#">Mosaddek Hossain</a> at 1:55pm, 13th April 2013</p>
                                            <p>
                                                Hello, How are you brother?</p>
                                        </div>
                                    </div>
                                </div>
                                <!-- /comment -->
                                <!-- Comment -->
                                <div class="msg-time-chat">
                                    <a class="message-img" href="#">
                                        <img alt="" src="img/avatar2.jpg" class="avatar"></a>
                                    <div class="message-body msg-out">
                                        <div class="text">
                                            <p class="attribution">
                                                <a href="#">Dulal Khan</a> at 2:01pm, 13th April 2013</p>
                                            <p>
                                                I'm Fine, Thank you. What about you? How is going on?</p>
                                        </div>
                                    </div>
                                </div>
                                <!-- /comment -->
                                <!-- Comment -->
                                <div class="msg-time-chat">
                                    <a class="message-img" href="#">
                                        <img alt="" src="img/avatar1.jpg" class="avatar"></a>
                                    <div class="message-body msg-in">
                                        <div class="text">
                                            <p class="attribution">
                                                <a href="#">Mosaddek Hossain</a> at 2:03pm, 13th April 2013</p>
                                            <p>
                                                Yeah I'm fine too. Everything is going fine here.</p>
                                        </div>
                                    </div>
                                </div>
                                <!-- /comment -->
                                <!-- Comment -->
                                <div class="msg-time-chat">
                                    <a class="message-img" href="#">
                                        <img alt="" src="img/avatar2.jpg" class="avatar"></a>
                                    <div class="message-body msg-out">
                                        <div class="text">
                                            <p class="attribution">
                                                <a href="#">Dulal Khan</a> at 2:05pm, 13th April 2013</p>
                                            <p>
                                                well good to know that. anyway how much time you need to done your task?</p>
                                        </div>
                                    </div>
                                </div>
                                <!-- /comment -->
                            </div>
                            <div class="chat-form">
                                <div class="input-cont">
                                    <input type="text" placeholder="Type a message here..." />
                                </div>
                                <div class="btn-cont">
                                    <a href="javascript:;" class="btn btn-primary">Send</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- END CHAT PORTLET-->
                </div>
                <div class="span5">
                    <!-- BEGIN NOTIFICATIONS PORTLET-->
                    <div class="widget">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-bell"></i>Notifications</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                        </div>
                        <div class="widget-body">
                            <ul class="item-list scroller padding" data-height="365" data-always-visible="1">
                                <li><span class="label label-success"><i class="icon-bell"></i></span><span>New user
                                    registered.</span> <span class="small italic">Just now</span> </li>
                                <li><span class="label label-success"><i class="icon-bell"></i></span><span>New order
                                    received.</span> <span class="small italic">15 mins ago</span> </li>
                                <li><span class="label label-warning"><i class="icon-bullhorn"></i></span><span>Alerting
                                    a user account balance.</span> <span class="small italic">2 hrs ago</span> </li>
                                <li><span class="label label-important"><i class="icon-bolt"></i></span><span>Alerting
                                    administrators staff.</span> <span class="small italic">11 hrs ago</span> </li>
                                <li><span class="label label-important"><i class="icon-bolt"></i></span><span>Messages
                                    are not sent to users.</span> <span class="small italic">14 hrs ago</span> </li>
                                <li><span class="label label-warning"><i class="icon-bullhorn"></i></span><span>Web
                                    server #12 failed to relosd.</span> <span class="small italic">2 days ago</span>
                                </li>
                                <li><span class="label label-success"><i class="icon-bell"></i></span><span>New order
                                    received.</span> <span class="small italic">15 mins ago</span> </li>
                                <li><span class="label label-warning"><i class="icon-bullhorn"></i></span><span>Alerting
                                    a user account balance.</span> <span class="small italic">2 hrs ago</span> </li>
                                <li><span class="label label-important"><i class="icon-bolt"></i></span><span>Alerting
                                    administrators staff.</span> <span class="small italic">11 hrs ago</span> </li>
                                <li><span class="label label-important"><i class="icon-bolt"></i></span><span>Messages
                                    are not sent to users.</span> <span class="small italic">14 hrs ago</span> </li>
                                <li><span class="label label-warning"><i class="icon-bullhorn"></i></span><span>Web
                                    server #12 failed to relosd.</span> <span class="small italic">2 days ago</span>
                                </li>
                                <li><span class="label label-success"><i class="icon-bell"></i></span><span>New order
                                    received.</span> <span class="small italic">15 mins ago</span> </li>
                                <li><span class="label label-warning"><i class="icon-bullhorn"></i></span><span>Alerting
                                    a user account balance.</span> <span class="small italic">2 hrs ago</span> </li>
                                <li><span class="label label-important"><i class="icon-bolt"></i></span><span>Alerting
                                    administrators support staff.</span> <span class="small italic">11 hrs ago</span>
                                </li>
                                <li><span class="label label-important"><i class="icon-bolt"></i></span><span>Messages
                                    are not sent to users.</span> <span class="small italic">14 hrs ago</span> </li>
                                <li><span class="label label-warning"><i class="icon-bullhorn"></i></span><span>Web
                                    server #12 failed to relosd.</span> <span class="small italic">2 days ago</span>
                                </li>
                            </ul>
                            <div class="space5">
                            </div>
                            <a href="#" class="pull-right">View all notifications</a>
                            <div class="clearfix no-top-space no-bottom-space">
                            </div>
                        </div>
                    </div>
                    <!-- END NOTIFICATIONS PORTLET-->
                </div>
            </div>
            <div class="row-fluid">
                <div class="span8 responsive" data-tablet="span8 fix-margin" data-desktop="span8">
                    <!-- BEGIN CALENDAR PORTLET-->
                    <div class="widget">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-calendar"></i>Calendar</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                        </div>
                        <div class="widget-body">
                            <div id="calendar" class="has-toolbar">
                            </div>
                        </div>
                    </div>
                    <!-- END CALENDAR PORTLET-->
                </div>
                <div class="span4 responsive" data-tablet="span4 fix-margin" data-desktop="span4">
                    <!-- BEGIN TODO_LIST PORTLET-->
                    <div class="widget">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-check"></i>To Do List</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <ul class="todo-list">
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Weekly Meeting.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-success"><i class="icon-bell"></i>Today</span> <span class="actions">
                                            <a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon"><i class="icon-remove">
                                            </i></a></span>
                                    </div>
                                </li>
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Monthly Status Update.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-default"><i class="icon-bell"></i>12.00PM</span> <span class="actions">
                                            <a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon"><i class="icon-remove">
                                            </i></a></span>
                                    </div>
                                </li>
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Upgrage server OS.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-success"><i class="icon-bell"></i>4 March</span> <span class="actions">
                                            <a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon"><i class="icon-remove">
                                            </i></a></span>
                                    </div>
                                </li>
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Weekly technical support report.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-success"><i class="icon-bell"></i>2 Jan</span> <span class="actions">
                                            <a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon"><i class="icon-remove">
                                            </i></a></span>
                                    </div>
                                </li>
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Project materials.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-warning"><i class="icon-bell"></i>09 Feb</span> <span class="actions">
                                            <a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon"><i class="icon-remove">
                                            </i></a></span>
                                    </div>
                                </li>
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Project Status Update.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-important"><i class="icon-bell"></i>4.30PM</span> <span
                                            class="actions"><a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon">
                                                <i class="icon-remove"></i></a></span>
                                    </div>
                                </li>
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Anual Project Meeting.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-important"><i class="icon-bell"></i>Today</span> <span class="actions">
                                            <a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon"><i class="icon-remove">
                                            </i></a></span>
                                    </div>
                                </li>
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Prepare project materials.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-warning"><i class="icon-bell"></i>3 May</span> <span class="actions">
                                            <a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon"><i class="icon-remove">
                                            </i></a></span>
                                    </div>
                                </li>
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Update salary status.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-reverse"><i class="icon-bell"></i>1 June</span> <span class="actions">
                                            <a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon"><i class="icon-remove">
                                            </i></a></span>
                                    </div>
                                </li>
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Update Task Status.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-reverse"><i class="icon-bell"></i>3 April</span> <span class="actions">
                                            <a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon"><i class="icon-remove">
                                            </i></a></span>
                                    </div>
                                </li>
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Project Status Report.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-important"><i class="icon-bell"></i>10.00PM</span> <span
                                            class="actions"><a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon">
                                                <i class="icon-remove"></i></a></span>
                                    </div>
                                </li>
                                <li>
                                    <div class="col1">
                                        <div class="cont">
                                            <a href="#">Update project rates.</a>
                                        </div>
                                    </div>
                                    <div class="col2">
                                        <span class="label label-reverse"><i class="icon-bell"></i>28 April</span> <span
                                            class="actions"><a href="#" class="icon"><i class="icon-ok"></i></a><a href="#" class="icon">
                                                <i class="icon-remove"></i></a></span>
                                    </div>
                                </li>
                            </ul>
                            <a href="#" class="pull-right">View all todo list</a>
                            <div class="clearfix">
                            </div>
                        </div>
                    </div>
                    <!-- END TODO_LIST PORTLET-->
                </div>
            </div>--%>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END PAGE CONTAINER-->
</asp:Content>
