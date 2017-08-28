using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WMSMain : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { }

        if (Session["UserName"] == null || Session["UserName"] == "")
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            UserImg.Src = Session["Image"].ToString();
            lblUserName.Text = Session["UserName"].ToString();
        }
    }

    protected void LBLogOut_Click(object sender, EventArgs e)
    {
        Session.Remove("Image");
        Session.Remove("ComID");
        Session.Remove("NGOID");
        Session.Remove("FullName");
        Session.Remove("UserName");

        Response.Redirect("Login.aspx");
    }
}
