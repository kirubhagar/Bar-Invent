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

public partial class BusinessLocation : System.Web.UI.Page
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
            BindGVBusinessLocation();
        }
    }

    private void BindGVBusinessLocation()
    {
        string SelectQuery = "SELECT * FROM View_BusinessLocationDetails";
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelectQuery);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            GVBusinessLocation.DataSource = DSRecord.Tables[0];
            GVBusinessLocation.DataBind();
        }
        else
        {
            GVBusinessLocation.EmptyDataText = "No Record Found !";
            GVBusinessLocation.DataBind();
        }
        foreach (GridViewRow Row in GVBusinessLocation.Rows)
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
        MVBusinessLocation.ActiveViewIndex = 1;
        BindCompanyDetails();
        // Disable Company TIN Number
        TBCompanyTINNumber.Enabled = false;
    }

    private void BindCompanyDetails()
    {
        SqlParameter[] arrparm = new SqlParameter[2];
        arrparm[0] = new SqlParameter("@Action", "SELECT_ByStatus");
        arrparm[1] = new SqlParameter("@Status", true);
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrparm);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            DDLCompanyName.DataSource = DSRecord.Tables[0];
            DDLCompanyName.DataTextField = "CompanyName";
            DDLCompanyName.DataValueField = "ID";
            DDLCompanyName.DataBind();
            DDLCompanyName.Items.Insert(0, new ListItem("Select Company", "0"));
        }
    }

    protected void DDLCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLCompanyName.SelectedIndex > 0)
        {
            BindCompanyTINNumber(DDLCompanyName.SelectedValue);
        }
    }

    private void BindCompanyTINNumber(string p)
    {
        SqlParameter[] arrparm = new SqlParameter[2];
        arrparm[0] = new SqlParameter("@Action", "SELECT_ByID");
        arrparm[1] = new SqlParameter("@ID", DDLCompanyName.SelectedValue);
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrparm);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            TBCompanyTINNumber.Text = DSRecord.Tables[0].Rows[0]["TINNo"].ToString();
        }
    }

    protected void BtnSubmitCancel(object sender, EventArgs e)
    {
        if (sender == btnSubmit)
        {
            string ImagePath = GetImagePath(FULogo);
            string UserImagePath = GetImagePath(FUUser);
            int ContactID = 0, 
                AddressID = 0,
                CompanyID = Convert.ToInt32(DDLCompanyName.SelectedValue), 
                NGOID = 0;
            
            #region BtnSubmit NGO Section
            
            if (btnSubmit.Text == "Submit")
            {
                ContactID = InsertContact();
                if (ContactID > 0)
                {
                    AddressID = InsertAddress(ContactID);
                    if (AddressID > 0)
                    {
                        NGOID = InsertBusinessLocation(AddressID, ImagePath);
                        if (NGOID > 0)
                        {
                            InsertGodown(CompanyID,NGOID);
                            InsertNGOUser(CompanyID, NGOID,UserImagePath);
                        }
                        else
                            ShowMessage("NGO Details Not Inserted !", MessageType.Error);
                    }
                    else
                        ShowMessage("Address Details Not Inserted !", MessageType.Error);
                }
                else
                    ShowMessage("Contact Details Not Inserted !", MessageType.Error);
                ClearForm();
                MVBusinessLocation.ActiveViewIndex = 0;
                BindGVBusinessLocation();
            }
            #endregion

            #region BtnUpdate NGO Section

            else if (btnSubmit.Text == "Update")
            {
                NGOID = Convert.ToInt32(Session["RecordID"].ToString());

                AddressID = UpdateBusinessLocation(NGOID, ImagePath);
                ContactID = UpdateAddress(AddressID);
                UpdateContact(ContactID);

                ClearForm();
                btnSubmit.Text = "Submit";
                MVBusinessLocation.ActiveViewIndex = 0;
                BindGVBusinessLocation();
            }
            #endregion
        }

        else if (sender == btnCancel)
        {
            ClearForm();
            MVBusinessLocation.ActiveViewIndex = 0;
            BindGVBusinessLocation();
        }
    }

    private int UpdateBusinessLocation(int BLocationID, string ImagePath)
    {
        SqlParameter[] arrParam = new SqlParameter[14];
        arrParam[0] = new SqlParameter("@Action", "UPDATE");
        arrParam[1] = new SqlParameter("@CompanyID", Convert.ToInt32(DDLCompanyName.SelectedValue));
        arrParam[2] = new SqlParameter("@Image", ImagePath);
        arrParam[3] = new SqlParameter("@BusinessLocationName", TBBusinessLocationName.Text);
        arrParam[4] = new SqlParameter("@DocumentPrefix", TBDocumentPrefix.Text);
        arrParam[5] = new SqlParameter("@DocumentStartName", TBDocumentStartNumber.Text);
        arrParam[6] = new SqlParameter("@TINNo", TBTINNumber.Text);
        arrParam[7] = new SqlParameter("@ESICNumber", TBESICNumber.Text);
        arrParam[8] = new SqlParameter("@LBTNumber", TBLBTNo.Text);
        arrParam[9] = new SqlParameter("@CSTNumber", TBCSTNo.Text);
        arrParam[10] = new SqlParameter("@PTRegistrationNumber", TBPTRegistrationNumber.Text);
        arrParam[11] = new SqlParameter("@ServiceTaxNumber", TBServiceTaxNumber.Text);
        arrParam[12] = new SqlParameter("@AdditionalReceiptID", 1);
        arrParam[13] = new SqlParameter("@ID", BLocationID);
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrParam);

        string SelQuery = "SELECT RegisteredAddress FROM BusinessLocation WHERE ID='" + BLocationID + "'";
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

        string MaxQuery = "SELECT Top(1) ID FROM Contact ORDER BY ID DESC";
        DataSet DSMax = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, MaxQuery);
        if (DSMax.Tables[0].Rows.Count > 0)
            return Convert.ToInt32(DSMax.Tables[0].Rows[0]["ID"]);
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

        string MaxQuery = "SELECT Top(1) ID FROM Address ORDER BY ID DESC";
        DataSet DSMax = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, MaxQuery);
        if (DSMax.Tables[0].Rows.Count > 0)
            return Convert.ToInt32(DSMax.Tables[0].Rows[0]["ID"]);
        return 0;
    }

    private int InsertBusinessLocation(int AddressID, string ImagePath)
    {
        SqlParameter[] arrParam = new SqlParameter[14];
        arrParam[0] = new SqlParameter("@Action", "INSERT");
        arrParam[1] = new SqlParameter("@CompanyID", Convert.ToInt32(DDLCompanyName.SelectedValue));
        arrParam[2] = new SqlParameter("@Image", ImagePath);
        arrParam[3] = new SqlParameter("@BusinessLocationName", TBBusinessLocationName.Text);
        arrParam[4] = new SqlParameter("@DocumentPrefix", TBDocumentPrefix.Text);
        arrParam[5] = new SqlParameter("@DocumentStartName", TBDocumentStartNumber.Text);
        arrParam[6] = new SqlParameter("@RegisteredAddress", AddressID);
        arrParam[7] = new SqlParameter("@TINNo", TBTINNumber.Text);
        arrParam[8] = new SqlParameter("@ESICNumber", TBESICNumber.Text);
        arrParam[9] = new SqlParameter("@LBTNumber", TBLBTNo.Text);
        arrParam[10] = new SqlParameter("@CSTNumber", TBCSTNo.Text);
        arrParam[11] = new SqlParameter("@PTRegistrationNumber", TBPTRegistrationNumber.Text);
        arrParam[12] = new SqlParameter("@ServiceTaxNumber", TBServiceTaxNumber.Text);
        arrParam[13] = new SqlParameter("@AdditionalReceiptID", 1);
        
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrParam);

        if (result > 0)
        {
            string SelectBLID = "SELECT TOP(1) ID,BusinessLocationName FROM BusinessLocation ORDER BY ID DESC";
            DataSet DSBLID = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelectBLID);
            if (DSBLID.Tables[0].Rows.Count > 0)
                return Convert.ToInt32(DSBLID.Tables[0].Rows[0]["ID"]);
            else
                return 0;
        }
        else
            return 0;
    }

    private void InsertGodown(int CompanyID,int NGOID)
    {

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

    private void InsertNGOUser(int CompanyID,int NGOID, string UserImgPath)
    {
        //Insert Default Godown Details
        SqlParameter[] arrParam = new SqlParameter[11];
        arrParam[0] = new SqlParameter("@Action", "INSERT");
        arrParam[1] = new SqlParameter("@Image", UserImgPath);
        arrParam[2] = new SqlParameter("@ComID", CompanyID);
        arrParam[3] = new SqlParameter("@NGOID", NGOID);
        arrParam[4] = new SqlParameter("@FirstName", TBBusinessLocationName.Text);
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
        if (ImgLogo.ImageUrl != "")
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
        DDLCompanyName.ClearSelection();
        TBBusinessLocationName.Text = string.Empty;
        TBDocumentPrefix.Text = string.Empty;
        TBDocumentStartNumber.Text = string.Empty;
        TBCompanyTINNumber.Text = string.Empty;
        TBTINNumber.Text = string.Empty;
        TBESICNumber.Text = string.Empty;
        TBLBTNo.Text = string.Empty;
        TBCSTNo.Text = string.Empty;
        TBPTRegistrationNumber.Text = string.Empty;
        TBServiceTaxNumber.Text = string.Empty;
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
        TBGodownName.Text = string.Empty;
        TBGodownShortForm.Text = string.Empty;
        ImgLogo.ImageUrl = "";
    }

    protected void GVBusinessLocation_RowCommand(object sender, GridViewCommandEventArgs e)
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
                DeleteBusinessLocation(recordId);
                BindGVBusinessLocation();
            }
            #endregion

            #region For Change Status
            if (e.CommandName == "ChangeStatus")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrParam);
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
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrParam2);
                    }
                    #endregion

                    #region Change Status False TO True
                    if (DTRecords.Rows[0]["Status"].ToString() == "False")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", true);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrParam2);
                    }
                    #endregion
                }
            }
            #endregion

            BindGVBusinessLocation();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void DeleteBusinessLocation(int recordId)
    {
        string SelQuery = "SELECT RegisteredAddress FROM BusinessLocation WHERE ID='" + recordId + "'";
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
        int DelCompany = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrParam2);
    }

    private void ShowCompanyDetails(int recordId)
    {
        SqlParameter[] arrParam = new SqlParameter[2];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
        arrParam[1] = new SqlParameter("@ID", recordId);
        DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrParam);
        DataTable DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
        {
            MVBusinessLocation.ActiveViewIndex = 1;
            ImgLogo.ImageUrl = DTRecords.Rows[0]["Image"].ToString();
            BindCompanyDetails();
            DDLCompanyName.ClearSelection();
            DDLCompanyName.Items.FindByValue(DTRecords.Rows[0]["CompanyID"].ToString()).Selected = true;
            BindCompanyTINNumber(DDLCompanyName.SelectedValue);
            TBBusinessLocationName.Text = DTRecords.Rows[0]["BusinessLocationName"].ToString();
            TBDocumentPrefix.Text = DTRecords.Rows[0]["DocumentPrefix"].ToString();
            TBDocumentStartNumber.Text = DTRecords.Rows[0]["DocumentStartNumber"].ToString();
            ShowAddressDetails(Convert.ToInt32(DTRecords.Rows[0]["RegisteredAddress"].ToString()));
            TBTINNumber.Text = DTRecords.Rows[0]["TINNo"].ToString();
            TBESICNumber.Text = DTRecords.Rows[0]["ESICNumber"].ToString();
            TBLBTNo.Text = DTRecords.Rows[0]["LBTNumber"].ToString();
            TBCSTNo.Text = DTRecords.Rows[0]["CSTNumber"].ToString();
            TBPTRegistrationNumber.Text = DTRecords.Rows[0]["PTRegistrationNumber"].ToString();
            TBServiceTaxNumber.Text = DTRecords.Rows[0]["ServiceTaxNumber"].ToString();
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

    private void ShowUserDetail(int NGOID)
    {
        SqlParameter[] arrParam = new SqlParameter[2];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByNGOID");
        arrParam[1] = new SqlParameter("@NGOID", NGOID);
        DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);
        DataTable DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
        {
            TBUserName.Text = DTRecords.Rows[0]["UserName"].ToString();
            TBPassWord.Text = DTRecords.Rows[0]["PassWord"].ToString();
            ImgUser.ImageUrl = DTRecords.Rows[0]["Image"].ToString();
        }
    }

    private void ShowGodownDetail(int NGOID)
    {
        SqlParameter[] arrParam = new SqlParameter[2];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByNGOID");
        arrParam[1] = new SqlParameter("@NGOID", NGOID);
        DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
        DataTable DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
        {
            TBGodownName.Text = DTRecords.Rows[0]["Name"].ToString();
            TBGodownShortForm.Text = DTRecords.Rows[0]["ShortForm"].ToString();
        }
    }

    protected void GVBusinessLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVBusinessLocation.PageIndex = e.NewPageIndex;
        BindGVBusinessLocation();
        GVBusinessLocation.DataBind();
    }

    protected void SearchEvent(object sender, EventArgs e)
    {
        string SearchQuery = "SELECT * FROM View_BusinessLocationDetails WHERE View_BusinessLocationDetails.BusinessLocationName LIKE '" + TBSearch.Text.Trim() + "%'";
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SearchQuery);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            GVBusinessLocation.DataSource = DSRecord.Tables[0];
            GVBusinessLocation.DataBind();
        }
        else
        {
            GVBusinessLocation.EmptyDataText = "No Record Found !";
            GVBusinessLocation.DataBind();
        }
        foreach (GridViewRow Row in GVBusinessLocation.Rows)
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
            GVBusinessLocation.Columns[1].Visible = true;
        else
            GVBusinessLocation.Columns[1].Visible = false;
    }

    protected void TBUserName_TextChanged(object sender, EventArgs e)
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