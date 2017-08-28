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

public partial class GodownManager : System.Web.UI.Page
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
            BindGVGodown();
        }
    }

    private void BindGVGodown()
    {
        string SelectGodown = "SELECT * FROM View_GodownDetails";
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelectGodown);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            GVGodown.DataSource = DSRecord.Tables[0];
            GVGodown.DataBind();
        }
        else
        {
            GVGodown.EmptyDataText = "No Record Found !";
            GVGodown.DataBind();
        }
        GVStatus();
    }

    private void GVStatus()
    {
        foreach (GridViewRow Row in GVGodown.Rows)
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

    protected void BtnAddNewClick(object sender, EventArgs e)
    {
        MVGodown.ActiveViewIndex = 1;
    }

    protected void BtnSubmitCancel(object sender, EventArgs e)
    {
        if (sender == btnSubmit)
        {
            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@Action", "INSERT");
                arrParam[1] = new SqlParameter("@Name", TBGodownName.Text);
                arrParam[2] = new SqlParameter("@Description", TBDiscription.Text);
                arrParam[3] = new SqlParameter("@IsActive", CBIsActive.Checked);
                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
                if (result > 0)
                    ShowMessage("Added Suceess", MessageType.Success);
                else
                    ShowMessage("Some Technical Error Occur!!!", MessageType.Error);
                ClearForm();
                MVGodown.ActiveViewIndex = 0;
                BindGVGodown();
            }
            else if (btnSubmit.Text == "Update")
            {
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@Action", "UPDATE");
                arrParam[1] = new SqlParameter("@Name", TBGodownName.Text);
                arrParam[2] = new SqlParameter("@Description", TBDiscription.Text);
                arrParam[3] = new SqlParameter("@ID", Session["RecordId"]);
                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
                if (result > 0)
                    ShowMessage("Updated Successfully", MessageType.Success);
                else
                    ShowMessage("Error", MessageType.Error);
                ClearForm();
                MVGodown.ActiveViewIndex = 0;
                BindGVGodown();
                btnSubmit.Text = "Submit";
            }
        }

        else if (sender == btnCancel)
        {
            ClearForm();
            MVGodown.ActiveViewIndex = 0;
            BindGVGodown();
        }
    }

    private void ClearForm()
    {
        TBGodownName.Text = string.Empty;
        TBDiscription.Text = string.Empty;
    }

    protected void GVGodown_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int recordId = Convert.ToInt32(e.CommandArgument);
            Session["RecordId"] = recordId;

            #region For Edit Record
            if (e.CommandName == "EditRecord")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_All_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
                DataTable DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    MVGodown.ActiveViewIndex = 1;
                    TBGodownName.Text = DTRecords.Rows[0]["Name"].ToString();
                    TBDiscription.Text = DTRecords.Rows[0]["Description"].ToString();
                    CBIsActive.Checked = Convert.ToBoolean(DTRecords.Rows[0]["IsActive"]);
                    if (DTRecords.Rows[0]["BusinessLocationID"].ToString() != "")
                    {
                        CBIsDefault.Checked = Convert.ToBoolean(DTRecords.Rows[0]["IsDefaultGodown"]);
                        CBIsTransit.Checked = Convert.ToBoolean(DTRecords.Rows[0]["IsTransitGodown"]);
                        CBForStockCorrection.Checked = Convert.ToBoolean(DTRecords.Rows[0]["IsUsedForStockCorrection"]);
                    }
                    else
                    {
                        CBIsDefault.Enabled = false;
                        CBIsTransit.Enabled = false;
                        CBForStockCorrection.Enabled = false;
                    }
                    btnSubmit.Text = "Update";
                }
            }
            #endregion

            #region For Delete Record
            if (e.CommandName == "DeleteRecord")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "DELETE");
                arrParam[1] = new SqlParameter("@ID", recordId);
                int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
            }
            #endregion

            #region For Change Status
            if (e.CommandName == "ChangeStatus")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
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
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam2);
                    }
                    #endregion

                    #region Change Status False TO True
                    if (DTRecords.Rows[0]["Status"].ToString() == "False")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", true);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam2);
                    }
                    #endregion
                }
            }
            #endregion

            BindGVGodown();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void GVGodown_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVGodown.PageIndex = e.NewPageIndex;
        BindGVGodown();
        GVGodown.DataBind();
    }

    protected void SearchEvent(object sender, EventArgs e)
    {
        string SearchText = "SELECT * FROM View_GodownDetails ";
        SearchText += " WHERE View_GodownDetails.BusinessLocationName LIKE '" + TBSearch.Text + "%'";
        SearchText += " OR View_GodownDetails.Name LIKE '" + TBSearch.Text + "%'";
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SearchText);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GVGodown.DataSource = ds.Tables[0];
            GVGodown.DataBind();
        }
        else
        {
            GVGodown.EmptyDataText = "No Record Found !!";
            GVGodown.DataBind();
        }
        GVStatus();
    }
}