using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            User.Focus();
        }
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        string strlogin = "Select * From Users where UserName='" + User.Value + "' AND PassWord='" + Pwd.Value + "' AND Status='" + true + "' ";
        DataTable UserData = clsConnectionSql.filldt(strlogin);
        if (UserData.Rows.Count > 0)
        {
            Session.Add("Image", UserData.Rows[0]["Image"]);
            Session.Add("ComID", UserData.Rows[0]["ComID"]);
            Session.Add("NGOID", UserData.Rows[0]["NGOID"]);
            Session.Add("FullName", UserData.Rows[0]["FirstName"] + " " + UserData.Rows[0]["LastName"]);
            Session.Add("UserName", UserData.Rows[0]["UserName"]);
            Response.Redirect("Dashboard.aspx");
        }
        else
        {   
            Response.Write(@"<script language='javascript'>alert('Please Enter valid User Name and Password')</script>");
            User.Value = "";
            Pwd.Value = "";
        }
    }
}