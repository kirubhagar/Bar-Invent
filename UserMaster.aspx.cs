using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class UserMaster : System.Web.UI.Page
{
    #region ALERT Methods For Alert Messages Section
    
    public enum MessageType { Success, Error, Info, Warning };
    
    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
    }
    
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindUserDetails();
        }
    }

    private void BindUserDetails()
    {
        try
        {
            DataSet DSUser=new DataSet();
            switch (AbhiMethod.GetUserRole())
            { 
                case "Super":
                    SqlParameter[] arrParam = new SqlParameter[1];
                    arrParam[0] = new SqlParameter("@Action", "SELECT");
                    DSUser = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);
                    break;
                case "ComLevel":
                    SqlParameter[] arrParam1 = new SqlParameter[2];
                    arrParam1[0] = new SqlParameter("@Action", "SELECT_ByComID");
                    arrParam1[1] = new SqlParameter("@ComID", Session["ComID"]);
                    DSUser = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam1);
                    break;
                case "NGOLevel":
                    SqlParameter[] arrParam2 = new SqlParameter[3];
                    arrParam2[0] = new SqlParameter("@Action", "SELECT_ByComIDNGOID");
                    arrParam2[1] = new SqlParameter("@ComID", Session["ComID"]);
                    arrParam2[2] = new SqlParameter("@NGOID", Session["NGOID"]);
                    DSUser = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam2);
                    break;
                default:
                    break;
            }
            
            if (DSUser.Tables[0].Rows.Count > 0)
            {
                GVUsers.DataSource = DSUser.Tables[0];
                GVUsers.DataBind();
            }
            else
            {
                GVUsers.EmptyDataText = "No Record Found !";
                GVUsers.DataBind();
            }
            foreach (GridViewRow Row in GVUsers.Rows)
            {
                Label lblStatus = (Label)Row.FindControl("lblStatus");
                LinkButton LBEdit = (LinkButton)Row.FindControl("LBEdit");
                LinkButton LBDelete = (LinkButton)Row.FindControl("LBDelete");
                if (lblStatus.Text == "Active")
                {
                    lblStatus.CssClass = "label label-success";
                    LBEdit.Enabled = true;
                    LBDelete.Enabled = true;
                }
                else
                {
                    lblStatus.CssClass = "label label-danger";
                    LBEdit.Enabled = false;
                    LBDelete.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void LBAddNew_Click(object sender, EventArgs e)
    {
        MVUser.ActiveViewIndex = 1;
    }

    protected void CBShowImage_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (CBShowImage.Checked)
                GVUsers.Columns[1].Visible = true;
            else
                GVUsers.Columns[1].Visible = false;
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void LBSearch_Click(object sender, EventArgs e)
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "SEARCH");
            arrParam[1] = new SqlParameter("@SearchText", TBSearch.Text);
            DataSet DSUser = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);

            if (DSUser.Tables[0].Rows.Count > 0)
            {
                GVUsers.DataSource = DSUser.Tables[0];
                GVUsers.DataBind();
            }
            else
            {
                GVUsers.EmptyDataText = "No Record Found !";
                GVUsers.DataBind();
            }
            foreach (GridViewRow Row in GVUsers.Rows)
            {
                Label lblStatus = (Label)Row.FindControl("lblStatus");
                LinkButton LBEdit = (LinkButton)Row.FindControl("LBEdit");
                LinkButton LBDelete = (LinkButton)Row.FindControl("LBDelete");
                if (lblStatus.Text == "Active")
                {
                    lblStatus.CssClass = "label label-success";
                    LBEdit.Enabled = true;
                    LBDelete.Enabled = true;
                }
                else
                {
                    lblStatus.CssClass = "label label-danger";
                    LBEdit.Enabled = false;
                    LBDelete.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (sender == btnSubmit)
            {
                string ImagePath = GetImagePath(FULogo);

                if (btnSubmit.Text == "Submit")
                {
                    SqlParameter[] arrParam = new SqlParameter[11];
                    arrParam[0] = new SqlParameter("@Action", "INSERT");
                    arrParam[1] = new SqlParameter("@Image", ImagePath);
                    arrParam[2] = new SqlParameter("@ComID", Session["ComID"]);
                    arrParam[3] = new SqlParameter("@NGOID", Session["NGOID"]);
                    arrParam[4] = new SqlParameter("@FirstName", TBFirstName.Text);
                    arrParam[5] = new SqlParameter("@LastName", TBLastName.Text);
                    arrParam[6] = new SqlParameter("@UserName", TBUserName.Text);
                    arrParam[7] = new SqlParameter("@Password", TBPassword.Text);
                    arrParam[8] = new SqlParameter("@Email", TBEmail.Text);
                    arrParam[9] = new SqlParameter("@Phone", TBPhone.Text);
                    arrParam[10] = new SqlParameter("@Mobile", TBMobile.Text);

                    int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);
                    if (result > 0)
                        ShowMessage("Added Suceess", MessageType.Success);
                    else
                        ShowMessage("Some Technical Error Occur!!!", MessageType.Error);
                    ClearUserForm();
                    MVUser.ActiveViewIndex = 0;
                    BindUserDetails();
                }
                else if (btnSubmit.Text == "Update")
                {
                    SqlParameter[] arrParam = new SqlParameter[12];
                    arrParam[0] = new SqlParameter("@Action", "UPDATE");
                    arrParam[1] = new SqlParameter("@Image", ImagePath);
                    arrParam[2] = new SqlParameter("@ComID", Session["ComID"]);
                    arrParam[3] = new SqlParameter("@NGOID", Session["NGOID"]);
                    arrParam[4] = new SqlParameter("@FirstName", TBFirstName.Text);
                    arrParam[5] = new SqlParameter("@LastName", TBLastName.Text);
                    arrParam[6] = new SqlParameter("@UserName", TBUserName.Text);
                    arrParam[7] = new SqlParameter("@Password", TBPassword.Text);
                    arrParam[8] = new SqlParameter("@Email", TBEmail.Text);
                    arrParam[9] = new SqlParameter("@Phone", TBPhone.Text);
                    arrParam[10] = new SqlParameter("@Mobile", TBMobile.Text);
                    arrParam[11] = new SqlParameter("@ID", Session["RecordId"]);

                    int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);

                    if (result > 0)
                        ShowMessage("Updated Successfully", MessageType.Success);
                    else
                        ShowMessage("Error", MessageType.Error);

                    ClearUserForm();
                    MVUser.ActiveViewIndex = 0;
                    BindUserDetails();
                    btnSubmit.Text = "Submit";
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private string GetImagePath(FileUpload FULogo)
    {
        string ImagePath = "";
        if (FULogo.HasFile)
        {
            string NameImage = DateTime.Now.Millisecond + FULogo.FileName;
            FULogo.SaveAs(Server.MapPath("~/AdminImages/Users/" + NameImage));
            ImagePath = "~/AdminImages/Users/" + NameImage;
            return ImagePath;
        }
        if (ImgLogo.ImageUrl != "")
        {
            return ImgLogo.ImageUrl;
        }
        else
        {
            return ImagePath = "~/AdminImages/noimg.png";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearUserForm();
        MVUser.ActiveViewIndex = 0;
        BindUserDetails();
    }

    private void ClearUserForm()
    {
        TBFirstName.Text = string.Empty;
        TBLastName.Text = string.Empty;
        TBUserName.Text = string.Empty;
        TBPassword.Text = string.Empty;
        TBCPassword.Text = string.Empty;
        TBEmail.Text = string.Empty;
        TBPhone.Text = string.Empty;
        TBMobile.Text = string.Empty;
    }

    protected void GVUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int recordId = Convert.ToInt32(e.CommandArgument);
            Session["RecordId"] = recordId;

            #region For Edit Record
            if (e.CommandName == "EditRecord")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);
                DataTable DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    MVUser.ActiveViewIndex = 1;
                    TBFirstName.Text=DTRecords.Rows[0]["FirstName"].ToString();
                    TBLastName.Text = DTRecords.Rows[0]["LastName"].ToString();
                    TBUserName.Text = DTRecords.Rows[0]["UserName"].ToString();
                    TBEmail.Text = DTRecords.Rows[0]["Email"].ToString();
                    TBPhone.Text = DTRecords.Rows[0]["Phone"].ToString();
                    TBMobile.Text = DTRecords.Rows[0]["Mobile"].ToString();
                    ImgLogo.ImageUrl = DTRecords.Rows[0]["Image"].ToString();
                    btnSubmit.Text = "Update";
                }
            }
            #endregion

            #region For Delete Record
            if (e.CommandName == "DeleteRecord")
            {
                // Delete Image From Folder
                SqlParameter[] arrParamDel = new SqlParameter[2];
                arrParamDel[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParamDel[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParamDel);
                if (DSRecords.Tables[0].Rows[0]["Image"].ToString() != "" && DSRecords.Tables[0].Rows[0]["Image"].ToString() != "~/AdminImages/noimg.png")
                {
                    File.Delete(Server.MapPath(DSRecords.Tables[0].Rows[0]["Image"].ToString()));
                }

                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "DELETE");
                arrParam[1] = new SqlParameter("@ID", recordId);
                int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);
            }
            #endregion

            #region For Change Status
            if (e.CommandName == "ChangeStatus")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);
                DataTable DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    #region Change Status True To False
                    if (DTRecords.Rows[0]["Status"].ToString() == "True")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", false);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam2);
                    }
                    #endregion

                    #region Change Status False TO True
                    if (DTRecords.Rows[0]["Status"].ToString() == "False")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", true);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam2);
                    }
                    #endregion
                }
            }
            #endregion

            BindUserDetails();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void GVUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVUsers.PageIndex = e.NewPageIndex;
        BindUserDetails();
        GVUsers.DataBind();
    }
    
}