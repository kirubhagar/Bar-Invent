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

public partial class CompanyManager : System.Web.UI.Page
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
            BindGVCompany();
        }
    }

    private void BindGVCompany()
    {
        string SelectQuery = "SELECT * FROM View_Companydetails";
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelectQuery);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            GVCompany.DataSource = DSRecord.Tables[0];
            GVCompany.DataBind();
        }
        else
        {
            GVCompany.EmptyDataText = "No Record Found !";
            GVCompany.DataBind();
        }
        foreach (GridViewRow Row in GVCompany.Rows)
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
        MVCompany.ActiveViewIndex = 1;
    }

    protected void BtnSubmitCancel(object sender, EventArgs e)
    {
        if (sender == btnSubmit)
        {
            string ImagePath = GetImagePath(FULogo);
            string UserImagePath = GetImagePath(FUUser);
            int ContactID = 0, AddressID = 0, CompanyID = 0;

            #region BtnSubmit Company Section
            if (btnSubmit.Text == "Submit")
            {
                ContactID = InsertContact();
                if (ContactID > 0)
                {
                    AddressID = InsertAddress(ContactID);
                    if (AddressID > 0)
                    {
                        CompanyID = InsertCompany(AddressID, ImagePath);
                        if (CompanyID > 0)
                        {
                            InsertGodown(CompanyID);
                            InsertCompanyUser(CompanyID,UserImagePath);
                        }
                        else
                            ShowMessage("Company Details Not Inserted !", MessageType.Error);
                    }
                    else
                        ShowMessage("Address Details Not Inserted !", MessageType.Error);
                }
                else
                    ShowMessage("Contact Details not Inserted !", MessageType.Error);

                ClearForm();
                MVCompany.ActiveViewIndex = 0;
                BindGVCompany();
            }
            #endregion

            #region BtnUpdate Company Section
            else if (btnSubmit.Text == "Update")
            {
                CompanyID = Convert.ToInt32(Session["RecordID"].ToString());

                AddressID = UpdateCompany(CompanyID, ImagePath);
                ContactID = UpdateAddress(AddressID);
                UpdateContact(ContactID);
                UpdateComUser(CompanyID,UserImagePath);
                UpdateComGodown(CompanyID);

                ClearForm();
                btnSubmit.Text = "Submit";
                MVCompany.ActiveViewIndex = 0;
                BindGVCompany();
            }
            #endregion

        }

        else if (sender == btnCancel)
        {
            ClearForm();
            MVCompany.ActiveViewIndex = 0;
            BindGVCompany();
        }
    }

    private int UpdateCompany(int CompanyID, string ImagePath)
    {
        SqlParameter[] arrParam = new SqlParameter[10];
        arrParam[0] = new SqlParameter("@Action", "UPDATE");
        arrParam[1] = new SqlParameter("@Image", ImagePath);
        arrParam[2] = new SqlParameter("@CompanyName", TBCompanyName.Text);
        arrParam[3] = new SqlParameter("@TinNo", TBTINNumber.Text);
        arrParam[4] = new SqlParameter("@AlternateCompanyName", TBAlternateCompanyName.Text);
        arrParam[5] = new SqlParameter("@TANNumber", TBTANNumber.Text);
        arrParam[6] = new SqlParameter("@PANNumber", TBPANNumber.Text);
        arrParam[7] = new SqlParameter("@ISOCertificateNumber", TBISOCertificateNumber.Text);
        arrParam[8] = new SqlParameter("@ExciseDutyNumber", TBExciseDutyNumber.Text);
        arrParam[9] = new SqlParameter("@ID", CompanyID);
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrParam);

        string SelQuery = "SELECT RegisteredAddress FROM Company WHERE ID='" + CompanyID + "'";
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
        if (DSRecord.Tables[0].Rows.Count > 0)
            return Convert.ToInt32(DSRecord.Tables[0].Rows[0]["RegisteredAddress"].ToString());
        else
            return 0;
    }

    private int UpdateAddress(int AddressID)
    {
        SqlParameter[] arrParam = new SqlParameter[8];
        arrParam[0] = new SqlParameter("@Action", "UPDATE");
        arrParam[1] = new SqlParameter("@Address1", TBAddressLine1.Text);
        arrParam[2] = new SqlParameter("@Address2", TBAddressLine2.Text);
        arrParam[3] = new SqlParameter("@City", TBCity.Text);
        arrParam[4] = new SqlParameter("@State", TBState.Text);
        arrParam[5] = new SqlParameter("@Country", TBCountry.Text);
        arrParam[6] = new SqlParameter("@ZipCode", TBPin.Text);
        arrParam[7] = new SqlParameter("@ID", AddressID);
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Address_CRUD", arrParam);

        string SelQuery = "SELECT ContactID FROM Address WHERE ID='" + AddressID + "'";
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
        if (DSRecord.Tables[0].Rows.Count > 0)
            return Convert.ToInt32(DSRecord.Tables[0].Rows[0]["ContactID"].ToString());
        else
            return 0;
    }

    private void UpdateContact(int ContactID)
    {
        SqlParameter[] arrParam = new SqlParameter[7];
        arrParam[0] = new SqlParameter("@Action", "UPDATE");
        arrParam[1] = new SqlParameter("@MobileNumber", TBMobileNumber.Text);
        arrParam[2] = new SqlParameter("@TelephoneNumber", TBPhoneNumber.Text);
        arrParam[3] = new SqlParameter("@FaxNumber", TBFaxNumber.Text);
        arrParam[4] = new SqlParameter("@Email", TBEmail.Text);
        arrParam[5] = new SqlParameter("@Website", TBWebsite.Text);
        arrParam[6] = new SqlParameter("@ID", ContactID);
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Contact_CRUD", arrParam);
    }

    private void UpdateComUser(int CompanyID, string UserImgPath)
    {
        SqlParameter[] arrParam = new SqlParameter[9];
        arrParam[0] = new SqlParameter("@Action", "UPDATE");
        arrParam[1] = new SqlParameter("@Image", UserImgPath);
        arrParam[2] = new SqlParameter("@FirstName", TBCompanyName.Text);
        arrParam[3] = new SqlParameter("@UserName", TBUserName.Text);
        arrParam[4] = new SqlParameter("@PassWord", TBPassWord.Text);
        arrParam[5] = new SqlParameter("@Email", TBEmail.Text);
        arrParam[6] = new SqlParameter("@Phone", TBPhoneNumber.Text);
        arrParam[7] = new SqlParameter("@Mobile", TBMobileNumber.Text);
        arrParam[8] = new SqlParameter("@ComID", CompanyID);
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);
    }

    private void UpdateComGodown(int CompanyID)
    {
        int NGOID = 0;

        //Insert Default Godown Details
        SqlParameter[] arrParamDefaultGodown = new SqlParameter[10];
        arrParamDefaultGodown[0] = new SqlParameter("@Action", "UPDATE");
        arrParamDefaultGodown[1] = new SqlParameter("@ComID", CompanyID);
        arrParamDefaultGodown[2] = new SqlParameter("@NGOID", NGOID);
        arrParamDefaultGodown[3] = new SqlParameter("@Name", TBGodownName.Text);
        arrParamDefaultGodown[4] = new SqlParameter("@ShortForm", TBGodownShortForm.Text);
        arrParamDefaultGodown[5] = new SqlParameter("@Description", "Default Godown");
        arrParamDefaultGodown[6] = new SqlParameter("@IsActive", true);
        arrParamDefaultGodown[7] = new SqlParameter("@IsDefaultGodown", true);
        arrParamDefaultGodown[8] = new SqlParameter("@IsTransitGodown", false);
        arrParamDefaultGodown[9] = new SqlParameter("@IsUsedForStockCorrection", false);
        int resultDefaultGodown = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParamDefaultGodown);
    }

    private int InsertContact()
    {
        SqlParameter[] arrParam = new SqlParameter[6];
        arrParam[0] = new SqlParameter("@Action", "INSERT");
        arrParam[1] = new SqlParameter("@MobileNumber", TBMobileNumber.Text);
        arrParam[2] = new SqlParameter("@TelephoneNumber", TBPhoneNumber.Text);
        arrParam[3] = new SqlParameter("@FaxNumber", TBFaxNumber.Text);
        arrParam[4] = new SqlParameter("@Email", TBEmail.Text);
        arrParam[5] = new SqlParameter("@Website", TBWebsite.Text);
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Contact_CRUD", arrParam);
        if (result > 0)
        {
            string MaxQuery = "SELECT Top(1) ID FROM Contact ORDER BY ID DESC";
            DataSet DSMax = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, MaxQuery);
            if (DSMax.Tables[0].Rows.Count > 0)
                return Convert.ToInt32(DSMax.Tables[0].Rows[0]["ID"]);
            else
                return 0;
        }
        else
            return 0;
    }

    private int InsertAddress(int ContactID)
    {
        SqlParameter[] arrParam = new SqlParameter[8];
        arrParam[0] = new SqlParameter("@Action", "INSERT");
        arrParam[1] = new SqlParameter("@Address1", TBAddressLine1.Text);
        arrParam[2] = new SqlParameter("@Address2", TBAddressLine2.Text);
        arrParam[3] = new SqlParameter("@City", TBCity.Text);
        arrParam[4] = new SqlParameter("@State", TBState.Text);
        arrParam[5] = new SqlParameter("@Country", TBCountry.Text);
        arrParam[6] = new SqlParameter("@ZipCode", TBPin.Text);
        arrParam[7] = new SqlParameter("@ContactID", ContactID);
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Address_CRUD", arrParam);
        if (result > 0)
        {
            string MaxQuery = "SELECT Top(1) ID FROM Address ORDER BY ID DESC";
            DataSet DSMax = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, MaxQuery);
            if (DSMax.Tables[0].Rows.Count > 0)
                return Convert.ToInt32(DSMax.Tables[0].Rows[0]["ID"]);
            else
                return 0;
        }
        else
            return 0;
    }

    private int InsertCompany(int AddressID, string ImagePath)
    {
        SqlParameter[] arrParam = new SqlParameter[10];
        arrParam[0] = new SqlParameter("@Action", "INSERT");
        arrParam[1] = new SqlParameter("@Image", ImagePath);
        arrParam[2] = new SqlParameter("@CompanyName", TBCompanyName.Text);
        arrParam[3] = new SqlParameter("@TinNo", TBTINNumber.Text);
        arrParam[4] = new SqlParameter("@AlternateCompanyName", TBAlternateCompanyName.Text);
        arrParam[5] = new SqlParameter("@TANNumber", TBTANNumber.Text);
        arrParam[6] = new SqlParameter("@PANNumber", TBPANNumber.Text);
        arrParam[7] = new SqlParameter("@ISOCertificateNumber", TBISOCertificateNumber.Text);
        arrParam[8] = new SqlParameter("@ExciseDutyNumber", TBExciseDutyNumber.Text);
        arrParam[9] = new SqlParameter("@RegisteredAddress", AddressID);
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrParam);

        if (result > 0)
        {
            string SelectComID = "SELECT TOP(1) ID,CompanyName FROM Company ORDER BY ID DESC";
            DataSet DSComID = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelectComID);
            if (DSComID.Tables[0].Rows.Count > 0)
                return Convert.ToInt32(DSComID.Tables[0].Rows[0]["ID"]);
            else
                return 0;
        }
        else
            return 0;

    }

    private void InsertGodown(int CompanyID)
    {
        int NGOID = 0;
        
        //Insert Default Godown Details
        SqlParameter[] arrParamDefaultGodown = new SqlParameter[10];
        arrParamDefaultGodown[0] = new SqlParameter("@Action", "INSERT");
        arrParamDefaultGodown[1] = new SqlParameter("@ComID", CompanyID);
        arrParamDefaultGodown[2] = new SqlParameter("@NGOID", NGOID);
        arrParamDefaultGodown[3] = new SqlParameter("@Name", TBGodownName.Text);
        arrParamDefaultGodown[4] = new SqlParameter("@ShortForm", TBGodownShortForm.Text);
        arrParamDefaultGodown[5] = new SqlParameter("@Description", "Default Godown");
        arrParamDefaultGodown[6] = new SqlParameter("@IsActive", true);
        arrParamDefaultGodown[7] = new SqlParameter("@IsDefaultGodown", true);
        arrParamDefaultGodown[8] = new SqlParameter("@IsTransitGodown", false);
        arrParamDefaultGodown[9] = new SqlParameter("@IsUsedForStockCorrection", false);
        int resultDefaultGodown = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParamDefaultGodown);

        //Insert Correction Godown Details
        SqlParameter[] arrParamCorrectionGodown = new SqlParameter[10];
        arrParamCorrectionGodown[0] = new SqlParameter("@Action", "INSERT");
        arrParamCorrectionGodown[1] = new SqlParameter("@ComID", CompanyID);
        arrParamCorrectionGodown[2] = new SqlParameter("@NGOID", NGOID);
        arrParamCorrectionGodown[3] = new SqlParameter("@Name", TBGodownName.Text + "-Stock Correction");
        arrParamCorrectionGodown[4] = new SqlParameter("@ShortForm", TBGodownShortForm.Text + "-Corr");
        arrParamCorrectionGodown[5] = new SqlParameter("@Description", "Stock Correction Godown");
        arrParamCorrectionGodown[6] = new SqlParameter("@IsActive", true);
        arrParamCorrectionGodown[7] = new SqlParameter("@IsDefaultGodown", false);
        arrParamCorrectionGodown[8] = new SqlParameter("@IsTransitGodown", false);
        arrParamCorrectionGodown[9] = new SqlParameter("@IsUsedForStockCorrection", true);
        int resultCorrectionGodown = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParamCorrectionGodown);

    }

    private void InsertCompanyUser(int CompanyID,string UserImgPath)
    {
        int NGOID = 0;
        //Insert Default Godown Details
        SqlParameter[] arrParam = new SqlParameter[11];
        arrParam[0] = new SqlParameter("@Action", "INSERT");
        arrParam[1] = new SqlParameter("@Image", UserImgPath);
        arrParam[2] = new SqlParameter("@ComID", CompanyID);
        arrParam[3] = new SqlParameter("@NGOID", NGOID);
        arrParam[4] = new SqlParameter("@FirstName", TBCompanyName.Text);
        arrParam[6] = new SqlParameter("@UserName", TBUserName.Text);
        arrParam[7] = new SqlParameter("@PassWord", TBRPassWord.Text);
        arrParam[8] = new SqlParameter("@Email", TBEmail.Text);
        arrParam[9] = new SqlParameter("@Phone", TBPhoneNumber.Text);
        arrParam[10] = new SqlParameter("@Mobile", TBMobileNumber.Text);
        int resultDefaultGodown = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);

    }

    private string GetImagePath(FileUpload FULogo)
    {
        string ImagePath = "";
        if (FULogo.HasFile)
        {
            string NameImage = DateTime.Now.Millisecond + FULogo.FileName;
            FULogo.SaveAs(Server.MapPath("~/AdminImages/Company/" + NameImage));
            ImagePath = "~/AdminImages/Company/" + NameImage;
            return ImagePath;
        }
        else if (ImgLogo.ImageUrl != "")
        {
            return ImgLogo.ImageUrl;
        }
        else
        {
            return ImagePath = "~/AdminImages/noimg.png";
        }
    }

    private void ClearForm()
    {
        TBCompanyName.Text = string.Empty;
        TBTINNumber.Text = string.Empty;
        TBAlternateCompanyName.Text = string.Empty;
        TBTANNumber.Text = string.Empty;
        TBPANNumber.Text = string.Empty;
        TBISOCertificateNumber.Text = string.Empty;
        TBExciseDutyNumber.Text = string.Empty;
        TBAddressLine1.Text = string.Empty;
        TBAddressLine2.Text = string.Empty;
        TBCity.Text = string.Empty;
        TBState.Text = string.Empty;
        TBCountry.Text = string.Empty;
        TBPin.Text = string.Empty;
        TBPhoneNumber.Text = string.Empty;
        TBMobileNumber.Text = string.Empty;
        TBFaxNumber.Text = string.Empty;
        TBEmail.Text = string.Empty;
        TBWebsite.Text = string.Empty;
        TBUserName.Text = string.Empty;
        TBPassWord.Text = string.Empty;
        TBRPassWord.Text = string.Empty;
        btnSubmit.Text = "Submit";
    }

    protected void GVCompany_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int recordId = Convert.ToInt32(e.CommandArgument);
            Session["RecordId"] = recordId;

            #region For Edit Record
            if (e.CommandName == "EditRecord")
            {
                ShowCompanyDetails(recordId);
            }
            #endregion

            #region For Delete Record
            if (e.CommandName == "DeleteRecord")
            {
                // Check Existing Record
                DeleteCompany(recordId);
                BindGVCompany();
            }
            #endregion

            #region For Change Status
            if (e.CommandName == "ChangeStatus")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrParam);
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
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrParam2);
                    }
                    #endregion

                    #region Change Status False TO True
                    if (DTRecords.Rows[0]["Status"].ToString() == "False")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", true);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrParam2);
                    }
                    #endregion
                }
            }
            #endregion

            BindGVCompany();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void DeleteCompany(int recordId)
    {
        string SelQuery = "SELECT RegisteredAddress FROM Company WHERE ID='" + recordId + "'";
        DataSet DSAddress = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
        if (DSAddress.Tables[0].Rows.Count > 0)
        {
            string SelContactQuery = "SELECT ContactID FROM Address WHERE ID='" + Convert.ToInt32(DSAddress.Tables[0].Rows[0]["RegisteredAddress"]) + "'";
            DataSet DSContact = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelContactQuery);
            if (DSContact.Tables[0].Rows.Count > 0)
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "DELETE");
                arrParam[1] = new SqlParameter("@ID", Convert.ToInt32(DSContact.Tables[0].Rows[0]["ContactID"]));
                int DelContact = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Contact_CRUD", arrParam);
            }

            SqlParameter[] arrParam1 = new SqlParameter[2];
            arrParam1[0] = new SqlParameter("@Action", "DELETE");
            arrParam1[1] = new SqlParameter("@ID", Convert.ToInt32(DSAddress.Tables[0].Rows[0]["RegisteredAddress"]));
            int DelAddress = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Address_CRUD", arrParam1);
        }

        SqlParameter[] arrParam2 = new SqlParameter[2];
        arrParam2[0] = new SqlParameter("@Action", "DELETE");
        arrParam2[1] = new SqlParameter("@ID", recordId);
        int DelCompany = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrParam2);
    }

    private void ShowCompanyDetails(int recordId)
    {
        SqlParameter[] arrParam = new SqlParameter[2];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
        arrParam[1] = new SqlParameter("@ID", recordId);
        DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrParam);
        DataTable DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
        {
            MVCompany.ActiveViewIndex = 1;
            ImgLogo.ImageUrl = DTRecords.Rows[0]["Image"].ToString();
            TBCompanyName.Text = DTRecords.Rows[0]["CompanyName"].ToString();
            TBTINNumber.Text = DTRecords.Rows[0]["TINNo"].ToString();
            TBAlternateCompanyName.Text = DTRecords.Rows[0]["AlternateCompanyName"].ToString();
            TBTANNumber.Text = DTRecords.Rows[0]["TANNumber"].ToString();
            TBPANNumber.Text = DTRecords.Rows[0]["PANNumber"].ToString();
            TBISOCertificateNumber.Text = DTRecords.Rows[0]["ISOCertificateNumber"].ToString();
            TBExciseDutyNumber.Text = DTRecords.Rows[0]["ExciseDutyNumber"].ToString();
            ShowAddressDetails(Convert.ToInt32(DTRecords.Rows[0]["RegisteredAddress"].ToString()));
            ShowUserDetail(Convert.ToInt32(DTRecords.Rows[0]["ID"]));
            ShowGodownDetail(Convert.ToInt32(DTRecords.Rows[0]["ID"]));
            btnSubmit.Text = "Update";
        }
    }

    private void ShowAddressDetails(int AddressID)
    {
        SqlParameter[] arrParam = new SqlParameter[2];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
        arrParam[1] = new SqlParameter("@ID", AddressID);
        DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Address_CRUD", arrParam);
        DataTable DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
        {
            TBAddressLine1.Text = DTRecords.Rows[0]["Address1"].ToString();
            TBAddressLine2.Text = DTRecords.Rows[0]["Address2"].ToString();
            TBCity.Text = DTRecords.Rows[0]["City"].ToString();
            TBState.Text = DTRecords.Rows[0]["State"].ToString();
            TBCountry.Text = DTRecords.Rows[0]["Country"].ToString();
            TBPin.Text = DTRecords.Rows[0]["ZipCode"].ToString();

            ShowContactDetails(Convert.ToInt32(DTRecords.Rows[0]["ContactID"].ToString()));
        }
    }

    private void ShowContactDetails(int ContactID)
    {
        SqlParameter[] arrParam = new SqlParameter[2];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
        arrParam[1] = new SqlParameter("@ID", ContactID);
        DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Contact_CRUD", arrParam);
        DataTable DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
        {
            TBPhoneNumber.Text = DTRecords.Rows[0]["TelephoneNumber"].ToString();
            TBMobileNumber.Text = DTRecords.Rows[0]["MobileNumber"].ToString();
            TBFaxNumber.Text = DTRecords.Rows[0]["FaxNumber"].ToString();
            TBEmail.Text = DTRecords.Rows[0]["Email"].ToString();
            TBWebsite.Text = DTRecords.Rows[0]["Website"].ToString();
        }
    }

    private void ShowUserDetail(int CompanyID)
    {
        SqlParameter[] arrParam = new SqlParameter[2];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByComID");
        arrParam[1] = new SqlParameter("@ComID", CompanyID);
        DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);
        DataTable DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
        {
            TBUserName.Text = DTRecords.Rows[0]["UserName"].ToString();
            TBPassWord.Text = DTRecords.Rows[0]["PassWord"].ToString();
            ImgUser.ImageUrl = DTRecords.Rows[0]["Image"].ToString();
        }
    }

    private void ShowGodownDetail(int CompanyID)
    {
        SqlParameter[] arrParam = new SqlParameter[2];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByComID");
        arrParam[1] = new SqlParameter("@ComID", CompanyID);
        DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
        DataTable DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
        {
            TBGodownName.Text = DTRecords.Rows[0]["Name"].ToString();
            TBGodownShortForm.Text = DTRecords.Rows[0]["ShortForm"].ToString();
        }
    }

    protected void GVCompany_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVCompany.PageIndex = e.NewPageIndex;
        BindGVCompany();
        GVCompany.DataBind();
    }

    protected void SearchEvent(object sender, EventArgs e)
    {
        string SearchQuery = "SELECT * FROM View_Companydetails WHERE View_Companydetails.CompanyName LIKE '" + TBSearch.Text.Trim() + "%'";
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SearchQuery);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            GVCompany.DataSource = DSRecord.Tables[0];
            GVCompany.DataBind();
        }
        else
        {
            GVCompany.EmptyDataText = "No Record Found !";
            GVCompany.DataBind();
        }
        foreach (GridViewRow Row in GVCompany.Rows)
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

    protected void ShowHideImageEvent(object sender, EventArgs e)
    {
        if (CBShowImage.Checked)
            GVCompany.Columns[1].Visible = true;
        else
            GVCompany.Columns[1].Visible = false;
    }

    protected void TBUserName_TextChanged(object sender,EventArgs e)
    {
        if (!string.IsNullOrEmpty(TBUserName.Text))
        {
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "SELECT_ByUser");
            arrParam[1] = new SqlParameter("@UserName", TBUserName.Text);
            DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                UserHelp.InnerText = "UserName Already Exist !";
                UserHelp.Style.Add("color", "Red");
            }
            else
                UserHelp.InnerText = "";
        }
    }
}