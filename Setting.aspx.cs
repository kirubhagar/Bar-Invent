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

public partial class Setting : System.Web.UI.Page
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

        }
    }

    protected void SettingClickEvents(object sender, EventArgs e)
    {
        if (sender == LBInvoice)
            MVSettings.ActiveViewIndex = 1;

        else if (sender == LBPrinter)
            MVSettings.ActiveViewIndex = 2;

        else if (sender == LBCurrency)
            MVSettings.ActiveViewIndex = 3;

        else if (sender == LBTechnicalSetting)
            MVSettings.ActiveViewIndex = 4;

        else if (sender == LBGeneral)
            MVSettings.ActiveViewIndex = 5;

        else if (sender == LBDateFormat)
            MVSettings.ActiveViewIndex = 6;

        else if (sender == LBBackUp)
            MVSettings.ActiveViewIndex = 7;

        else if (sender == LBEmailSetting)
            MVSettings.ActiveViewIndex = 8;

        else if (sender == LBWeighingScale)
            MVSettings.ActiveViewIndex = 9;

        else if (sender == LBCodeFormat)
        {
            MVSettings.ActiveViewIndex = 10;
            BindDDLDocumentType();
            BindDDLBusinessLocation();
        }

        else if (sender == LBFAFields)
            MVSettings.ActiveViewIndex = 11;

        else if (sender == LBItemFields)
            MVSettings.ActiveViewIndex = 12;

        else if (sender == LBCustomerFields)
            MVSettings.ActiveViewIndex = 13;

        else if (sender == LBSupplierFields)
            MVSettings.ActiveViewIndex = 14;

        else if (sender == LBCIFields)
            MVSettings.ActiveViewIndex = 15;

        else if (sender == LBRearrange)
            MVSettings.ActiveViewIndex = 16;

        else if (sender == LBCloudSetting)
            MVSettings.ActiveViewIndex = 17;

        else if (sender == LBCustomerDisplay)
            MVSettings.ActiveViewIndex = 18;

        else if (sender == LBUserInterface)
            MVSettings.ActiveViewIndex = 19;

        else if (sender == LBOnlineShop)
            MVSettings.ActiveViewIndex = 20;

        else if (sender == LBFASetting)
            MVSettings.ActiveViewIndex = 21;

        else if (sender == LBCategoryTree)
            MVSettings.ActiveViewIndex = 22;

        else if (sender == LBSMSSetting)
            MVSettings.ActiveViewIndex = 23;

        else if (sender == LBAppointmtFields)
            MVSettings.ActiveViewIndex = 24;

        else if (sender == LBAdditional)
            MVSettings.ActiveViewIndex = 25;

        else if (sender == LBEmployeeFields)
            MVSettings.ActiveViewIndex = 26;

        else if (sender == LBTimeTokenFields)
            MVSettings.ActiveViewIndex = 27;

        else if (sender == LBDeveloperGuide)
        {
            MVSettings.ActiveViewIndex = 28;
            BindGVDocumentType();
        }
    }

    protected void SettingCancelEvents(object sender, EventArgs e)
    {
        MVSettings.ActiveViewIndex = 0;
    }



    #region 10. Code Setting
    private void BindDDLDocumentType()
    {
        SqlParameter[] arrparm = new SqlParameter[1];
        arrparm[0] = new SqlParameter("@Action", "SELECT");
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_DocumentType_CRUD", arrparm);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            DDLDocumentType.DataSource = DSRecord.Tables[0];
            DDLDocumentType.DataTextField = "Name";
            DDLDocumentType.DataValueField = "ID";
            DDLDocumentType.DataBind();
            DDLDocumentType.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void BindDDLBusinessLocation()
    {
        SqlParameter[] arrparm = new SqlParameter[1];
        arrparm[0] = new SqlParameter("@Action", "SELECT");
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrparm);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            DDLBusinessLocation.DataSource = DSRecord.Tables[0];
            DDLBusinessLocation.DataTextField = "BusinessLocationName";
            DDLBusinessLocation.DataValueField = "ID";
            DDLBusinessLocation.DataBind();
        }

        string qury = "Select DocumentPrefix From BusinessLocation Where ID= '" + DDLBusinessLocation.SelectedValue + "'";
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, qury);
        if (ds.Tables[0].Rows.Count > 0)
            LBLBusinessLocationPrefix.Text = ds.Tables[0].Rows[0]["DocumentPrefix"].ToString();

    }

    protected void DDLDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Set Enable Disable DDLBusinessLocation
        SqlParameter[] arrparm = new SqlParameter[2];
        arrparm[0] = new SqlParameter("@Action", "SELECT_ByID");
        arrparm[1] = new SqlParameter("@ID", Convert.ToInt32(DDLDocumentType.SelectedValue));
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_DocumentType_CRUD", arrparm);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            DDLBusinessLocation.Enabled = Convert.ToBoolean(DSRecord.Tables[0].Rows[0]["IsBLApplicable"]);
        }
        #endregion

        #region Fill form details

        string SelQuery = "SELECT * FROM View_CodeDetails WHERE View_CodeDetails.Type='" + DDLDocumentType.SelectedValue + "'";
        DataSet DSCode = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
        if (DSCode.Tables[0].Rows.Count > 0)
        {

            TBCodeStartNumber.Text = DSCode.Tables[0].Rows[0]["StartNumber"].ToString();
            TBDocumentPrefix.Text = DSCode.Tables[0].Rows[0]["Prefix"].ToString();
            TBMinimumCodeLength.Text = DSCode.Tables[0].Rows[0]["MinCodeLength"].ToString();
            string Zero = new String('0', (Convert.ToInt32(DSCode.Tables[0].Rows[0]["MinCodeLength"].ToString()) - DSCode.Tables[0].Rows[0]["StartNumber"].ToString().Length));
            string CurrentCode = "";
            if (DDLBusinessLocation.Enabled == true)
                CurrentCode = DSCode.Tables[0].Rows[0]["DocumentPrefix"].ToString() +
                              DSCode.Tables[0].Rows[0]["Prefix"].ToString() +
                              Zero +
                              DSCode.Tables[0].Rows[0]["StartNumber"].ToString();
            else
                CurrentCode = DSCode.Tables[0].Rows[0]["Prefix"].ToString() +
                              Zero +
                              DSCode.Tables[0].Rows[0]["StartNumber"].ToString();

            LBLCurrentCodeFormat.Text = CurrentCode.ToString();
        }

        #endregion
    }

    protected void CodeControlChange(object sender, EventArgs e)
    {
        string SampleCode = "", BL = LBLBusinessLocationPrefix.Text, DocPre = TBDocumentPrefix.Text, Start = TBCodeStartNumber.Text, MinLength = TBMinimumCodeLength.Text;

        if (sender == DDLBusinessLocation)
        {
            string qury = "Select DocumentPrefix From BusinessLocation Where ID= '" + DDLBusinessLocation.SelectedValue + "'";
            DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, qury);
            if (ds.Tables[0].Rows.Count > 0)
                LBLBusinessLocationPrefix.Text = ds.Tables[0].Rows[0]["DocumentPrefix"].ToString();
            BL = ds.Tables[0].Rows[0]["DocumentPrefix"].ToString();
        }
        if (sender == TBDocumentPrefix)
        {
            DocPre = TBDocumentPrefix.Text;
        }
        if (sender == TBCodeStartNumber)
        {
            Start = TBCodeStartNumber.Text;
        }
        if (sender == TBMinimumCodeLength)
        {
            MinLength = TBMinimumCodeLength.Text;
        }

        string Zero = new String('0', (Convert.ToInt32(MinLength) - Start.Length));

        if (DDLBusinessLocation.Enabled == true)
            SampleCode = BL + DocPre + Zero + Start;
        else
            SampleCode = DocPre + Zero + Start;

        LBLSampleCode.Text = SampleCode.ToString();
    }

    protected void CodeSubmitCancelEvent(object sender, EventArgs e)
    {
        if (sender == btnApplyForCode)
        {
            SqlParameter[] arrParam = new SqlParameter[6];
            arrParam[0] = new SqlParameter("@Action", "UPDATE");
            arrParam[1] = new SqlParameter("@BusinessLocation", DDLBusinessLocation.SelectedValue);
            arrParam[2] = new SqlParameter("@Prefix", TBDocumentPrefix.Text);
            arrParam[3] = new SqlParameter("@MinCodeLength", TBMinimumCodeLength.Text);
            arrParam[4] = new SqlParameter("@StartNumber", TBCodeStartNumber.Text);
            arrParam[5] = new SqlParameter("@Type", DDLDocumentType.SelectedValue);
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Code_CRUD", arrParam);
        }

        if (sender == btnCancelForCode)
        {
            MVSettings.ActiveViewIndex = 0;
        }
    }



    #endregion

    #region 28. Developer Option View
    private void BindGVDocumentType()
    {
        SqlParameter[] arrparm = new SqlParameter[1];
        arrparm[0] = new SqlParameter("@Action", "SELECT");
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_DocumentType_CRUD", arrparm);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            GVDocumentType.DataSource = DSRecord.Tables[0];
            GVDocumentType.DataBind();
        }
        else
        {
            GVDocumentType.EmptyDataText = "No Record Found !";
            GVDocumentType.DataBind();
        }
        foreach (GridViewRow Row in GVDocumentType.Rows)
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

    protected void BtnSubmitCancelDT(object sender, EventArgs e)
    {
        if (sender == btnSubmit)
        {
            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@Action", "INSERT");
                arrParam[1] = new SqlParameter("@Name", TBDocumentType.Text);
                arrParam[2] = new SqlParameter("@IsBLApplicable", CBApplicableBL.Checked);

                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_DocumentType_CRUD", arrParam);
                if (result > 0)
                    ShowMessage("Added Suceess", MessageType.Success);
                else
                    ShowMessage("Some Technical Error Occur!!!", MessageType.Error);
                ClearForm();
                BindGVDocumentType();
            }
            else if (btnSubmit.Text == "Update")
            {
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@Action", "UPDATE");
                arrParam[1] = new SqlParameter("@Name", TBDocumentType.Text);
                arrParam[2] = new SqlParameter("@IsBLApplicable", CBApplicableBL.Checked);
                arrParam[3] = new SqlParameter("@ID", Session["RecordId"]);
                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_DocumentType_CRUD", arrParam);
                if (result > 0)
                    ShowMessage("Updated Successfully", MessageType.Success);
                else
                    ShowMessage("Error", MessageType.Error);
                ClearForm();
                BindGVDocumentType();
                btnSubmit.Text = "Submit";
            }
        }

        else if (sender == btnCancel)
        {
            ClearForm();
            BindGVDocumentType();
        }
    }

    private void ClearForm()
    {
        TBDocumentType.Text = string.Empty;
        CBApplicableBL.Checked = false;
    }

    protected void GVDocumentType_RowCommand(object sender, GridViewCommandEventArgs e)
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
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_DocumentType_CRUD", arrParam);
                DataTable DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    TBDocumentType.Text = DTRecords.Rows[0]["Name"].ToString();
                    CBApplicableBL.Checked = Convert.ToBoolean(DTRecords.Rows[0]["IsBLApplicable"]);
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
                int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_DocumentType_CRUD", arrParam);
            }
            #endregion

            #region For Change Status
            if (e.CommandName == "ChangeStatus")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_DocumentType_CRUD", arrParam);
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
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_DocumentType_CRUD", arrParam2);
                    }
                    #endregion

                    #region Change Status False TO True
                    if (DTRecords.Rows[0]["Status"].ToString() == "False")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", true);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_DocumentType_CRUD", arrParam2);
                    }
                    #endregion
                }
            }
            #endregion

            BindGVDocumentType();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void GVDocumentType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVDocumentType.PageIndex = e.NewPageIndex;
        BindGVDocumentType();
        GVDocumentType.DataBind();
    }
    #endregion


}