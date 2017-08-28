<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<!--[if IE 8]> <html lang="en" class="ie8"> <![endif]-->
<!--[if IE 9]> <html lang="en" class="ie9"> <![endif]-->
<!--[if !IE]><!-->
<html lang="en">
<!--<![endif]-->
<!-- BEGIN HEAD -->
<!-- Mirrored from thevectorlab.net/adminlab/login.html by HTTrack Website Copier/3.x [XR&CO'2014], Fri, 01 Apr 2016 06:51:57 GMT -->
<head>
    <meta charset="utf-8" />
    <title>Login page</title>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta content="" name="description" />
    <meta content="" name="author" />
    <link href="assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style_responsive.css" rel="stylesheet" />
    <link href="css/style_default.css" rel="stylesheet" id="style_color" />
</head>
<!-- END HEAD -->
<!-- BEGIN BODY -->
<body id="login-body">
    <form id="form1" runat="server">
    <!-- BEGIN LOGIN -->
    <div id="login">
        <!-- BEGIN LOGIN FORM -->
        <form id="loginform" class="form-vertical no-padding no-margin" action="">
        <div class="lock">
            <i class="icon-lock"></i>
        </div>
        <div class="control-wrap">
            <h4>
                User Login</h4>
            <div class="control-group">
                <div class="controls">
                    <div class="input-prepend">
                        <span class="add-on"><i class="icon-user"></i></span>
                        <input id="User" runat="server" type="text" placeholder="Username" />
                    </div>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <div class="input-prepend">
                        <span class="add-on"><i class="icon-key"></i></span>
                        <input id="Pwd" runat="server" type="password" placeholder="Password" />
                    </div>
                    <div class="mtop10" id="forgetPass" runat="server" visible="false">
                        <div class="block-hint pull-left small">
                            <input type="checkbox" id="">
                            Remember Me
                        </div>
                        <div class="block-hint pull-right">
                            <a href="javascript:;" class="" id="forget-password">Forgot Password?</a>
                        </div>
                    </div>
                    <div class="clearfix space5">
                    </div>
                </div>
            </div>
        </div>
            <asp:Button ID="btn" Text="submit" runat="server" 
            CssClass="btn btn-block login-btn" onclick="btn_Click" />
        </form>
        <!-- END LOGIN FORM -->
        <!-- BEGIN FORGOT PASSWORD FORM -->
        <form id="forgotform" class="form-vertical no-padding no-margin hide" action="http://thevectorlab.net/adminlab/index.html">
        <p class="center">
            Enter your e-mail address below to reset your password.</p>
        <div class="control-group">
            <div class="controls">
                <div class="input-prepend">
                    <span class="add-on"><i class="icon-envelope"></i></span>
                    <input id="input-email" type="text" placeholder="Email" />
                </div>
            </div>
            <div class="space20">
            </div>
        </div>
        <input type="button" id="forget-btn" class="btn btn-block login-btn" value="Submit" />
        </form>
        <!-- END FORGOT PASSWORD FORM -->
    </div>
    <!-- END LOGIN -->
    <!-- BEGIN COPYRIGHT -->
    <div id="login-copyright">
        2017 &copy; BeechTree System & Solution Pvt. Ltd.
    </div>
    <!-- END COPYRIGHT -->
    <!-- BEGIN JAVASCRIPTS -->
    <script src="js/jquery-1.8.3.min.js"></script>
    <script src="assets/bootstrap/js/bootstrap.min.js"></script>
    <script src="js/jquery.blockui.js"></script>
    <script src="js/scripts.js"></script>
    <script>
        jQuery(document).ready(function () {
            App.initLogin();
        });
    </script>
    <!-- END JAVASCRIPTS -->
    </form>
</body>
<!-- END BODY -->
</html>
