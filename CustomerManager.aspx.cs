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
using System.Data.OleDb;

public partial class CustomerManager : System.Web.UI.Page
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
            BindGVCustomer();
        }
    }

    private void BindGVCustomer()
    {
        string SelQuery = "SELECT * FROM View_CustomerDetails";
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            GVCustomer.DataSource = DSRecord.Tables[0];
            GVCustomer.DataBind();
        }
        else
        {
            GVCustomer.EmptyDataText = "No Record Found !";
            GVCustomer.DataBind();
        }
        foreach (GridViewRow Row in GVCustomer.Rows)
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

    protected void TopBarControls(object sender, EventArgs e)
    {
        if (sender == LBAddNew)
        {
            MVCustomers.ActiveViewIndex = 1;
            BindCustomerCode();
            BindBusinessLocation();
            CreateDTTable();
        }

        else if (sender == LBImportRecord)
        { }

        else if (sender == LBExprotRecord)
        { }
    }

    private void BindCustomerCode()
    {
        string PrvID = ""; int NextID = 1;
        string GetMaxId = "SELECT TOP(1) ID FROM Customer ORDER BY ID DESC ";
        DataSet DSRecordID = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, GetMaxId);
        if (DSRecordID.Tables[0].Rows.Count > 0)
        {
            PrvID = DSRecordID.Tables[0].Rows[0]["ID"].ToString();
            NextID = Convert.ToInt32(PrvID.ToString()) + 1;
        }

        string SelQuery = "SELECT * FROM View_CodeDetails WHERE View_CodeDetails.Type='" + 10 + "'";
        DataSet DSCode = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
        if (DSCode.Tables[0].Rows.Count > 0)
        {
            string Zero = new String('0', (Convert.ToInt32(DSCode.Tables[0].Rows[0]["MinCodeLength"].ToString()) - NextID.ToString().Length));

            if (Convert.ToBoolean(DSCode.Tables[0].Rows[0]["IsBLApplicable"].ToString()) == true)
                TBCustomerCode.Text = DSCode.Tables[0].Rows[0]["DocumentPrefix"].ToString() +
                              DSCode.Tables[0].Rows[0]["Prefix"].ToString() +
                              Zero +
                              NextID.ToString();
            else
                TBCustomerCode.Text = DSCode.Tables[0].Rows[0]["Prefix"].ToString() +
                              Zero +
                              NextID.ToString();
            TBCustomerCode.Enabled = false;
        }
    }

    protected void ChRadioControls(object sender, EventArgs e)
    {
        if (sender == CBShowImage)
        {
            if (CBShowImage.Checked)
                GVCustomer.Columns[1].Visible = true;
            else
                GVCustomer.Columns[1].Visible = false;
        }

        else if (sender == RBShowActive)
        {
            string SelQuery = "SELECT * FROM View_CustomerDetails WHERE View_CustomerDetails.IsActive ='" + true + "'";
            BindGVCustomer(SelQuery);
        }

        else if (sender == RBShowInactive)
        {
            string SelQuery = "SELECT * FROM View_CustomerDetails WHERE View_CustomerDetails.IsActive ='" + false + "'";
            BindGVCustomer(SelQuery);
        }

        else if (sender == RBShowAll)
        {
            string SelectQuery = "SELECT * FROM View_CustomerDetails";
            BindGVCustomer(SelectQuery);
        }
    }

    private void BindGVCustomer(string QueryText)
    {
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, QueryText);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            GVCustomer.DataSource = DSRecord.Tables[0];
            GVCustomer.DataBind();
        }
        else
        {
            GVCustomer.EmptyDataText = "No Record Found !";
            GVCustomer.DataBind();
        }
        foreach (GridViewRow Row in GVCustomer.Rows)
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

    protected void SearchEvent(object sender, EventArgs e)
    {
        string SearchQuery = "SELECT * FROM View_CustomerDetails WHERE View_CustomerDetails.CustomerName LIKE '" + TBSearchText.Text.Trim() + "%'";
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SearchQuery);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            GVCustomer.DataSource = DSRecord.Tables[0];
            GVCustomer.DataBind();
        }
        else
        {
            GVCustomer.EmptyDataText = "No Record Found !";
            GVCustomer.DataBind();
        }
        foreach (GridViewRow Row in GVCustomer.Rows)
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

    protected void DefineMyOwn(object sender, EventArgs e)
    {
        if (sender == LBCustomerCode)
        {
            if (LBCustomerCode.Text == "Define My Own")
            {
                TBCustomerCode.Text = "";
                TBCustomerCode.Enabled = true;
                TBCustomerCode.Focus();
                LBCustomerCode.Text = "Auto Generate";
            }

            else if (LBCustomerCode.Text == "Auto Generate")
            {
                BindCustomerCode();
                TBCustomerCode.Enabled = false;
                LBCustomerCode.Text = "Define My Own";
            }
        }
    }

    private void BindBusinessLocation()
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
            DDLBusinessLocation.Items.Insert(0, new ListItem("Select", "0"));
        }
        DSRecord.Clear();
    }

    private string GetImagePath(FileUpload FULogo)
    {
        string ImagePath = "";
        if (FULogo.HasFile)
        {
            string NameImage = DateTime.Now.Millisecond + FULogo.FileName;
            FULogo.SaveAs(Server.MapPath("~/AdminImages/Customer/" + NameImage));
            ImagePath = "~/AdminImages/Customer/" + NameImage;
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
        TBCustomerCode.Text = string.Empty;
        RBCompany.Checked = false;
        RBIndividual.Checked = false;
        DDLBusinessLocation.ClearSelection();
        TBSearchCode.Text = string.Empty;
        TBFirstName.Text = string.Empty;
        TBLastName.Text = string.Empty;
        TBCompanyName.Text = string.Empty;
        TBDOB.Text = string.Empty;
        TBAnniversaryDate.Text = string.Empty;
        TBVATNumber.Text = string.Empty;
        TBCSTNumber.Text = string.Empty;
        TBTINNumber.Text = string.Empty;
        TBPANNumber.Text = string.Empty;
        CBIsActive.Checked = false;
        TBAddressLine1.Text = string.Empty;
        TBAddressLine2.Text = string.Empty;
        TBCity.Text = string.Empty;
        TBState.Text = string.Empty;
        TBCountry.Text = string.Empty;
        TBPinCode.Text = string.Empty;
        TBPhoneNumber.Text = string.Empty;
        TBMobileNumber.Text = string.Empty;
        TBFaxNumber.Text = string.Empty;
        TBEmailID.Text = string.Empty;
        TBWebsite.Text = string.Empty;
        btnSubmit.Text = "Submit";
    }

    #region Dynamic DataTable Section

    // Basic Data
    DataTable TempDT = new DataTable();
    string[] ItemArray = new string[15];

    // Create / Load DataTable
    private void CreateDTTable()
    {
        TempDT = new DataTable();
        if (TempDT.Columns.Count == 0)
        {
            DataColumn dc = TempDT.Columns.Add("ID", typeof(int));
            dc.AutoIncrement = true;
            dc.AutoIncrementSeed = 1;
            dc.AutoIncrementStep = 1;
            TempDT.Columns.Add("Title", typeof(string));
            TempDT.Columns.Add("ContactName", typeof(string));
            TempDT.Columns.Add("Position", typeof(string));
            TempDT.Columns.Add("EmailID", typeof(string));
            TempDT.Columns.Add("PhoneNumber", typeof(string));
            TempDT.Columns.Add("MobileNumber", typeof(string));
            TempDT.Columns.Add("Note", typeof(string));
            TempDT.Columns.Add("AddressType", typeof(string));
            TempDT.Columns.Add("AddressLine1", typeof(string));
            TempDT.Columns.Add("AddressLine2", typeof(string));
            TempDT.Columns.Add("City", typeof(string));
            TempDT.Columns.Add("State", typeof(string));
            TempDT.Columns.Add("Country", typeof(string));
            TempDT.Columns.Add("PinCode", typeof(string));
        }
        ViewState["DTTable"] = TempDT;
    }

    // For Insert Into DataTable
    private void InsertDTTable(string[] ItemArray)
    {
        TempDT = ViewState["DTTable"] as DataTable;
        TempDT.Rows.Add(null, ItemArray[1], ItemArray[2], ItemArray[3], ItemArray[4], ItemArray[5], ItemArray[6], ItemArray[7], ItemArray[8], ItemArray[9], ItemArray[10], ItemArray[11], ItemArray[12], ItemArray[13], ItemArray[14]);
        ViewState["DTTable"] = TempDT;
    }

    // For Select Frome DataTable
    private string[] SelectDTTable(int ID)
    {
        TempDT = ViewState["DTTable"] as DataTable;
        for (int i = TempDT.Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = TempDT.Rows[i];
            if ((Int32)dr[0] == ID)
            {
                ItemArray[1] = dr["Title"].ToString();
                ItemArray[2] = dr["ContactName"].ToString();
                ItemArray[3] = dr["Position"].ToString();
                ItemArray[4] = dr["EmailID"].ToString();
                ItemArray[5] = dr["PhoneNumber"].ToString();
                ItemArray[6] = dr["MobileNumber"].ToString();
                ItemArray[7] = dr["Note"].ToString();
                ItemArray[8] = dr["AddressType"].ToString();
                ItemArray[9] = dr["AddressLine1"].ToString();
                ItemArray[10] = dr["AddressLine2"].ToString();
                ItemArray[11] = dr["City"].ToString();
                ItemArray[12] = dr["State"].ToString();
                ItemArray[13] = dr["Country"].ToString();
                ItemArray[14] = dr["PinCode"].ToString();
            }
        }
        return ItemArray;
    }

    // For Update DataTable Record
    private void UpdateDTTable(object RecordID, string[] ItemArray)
    {
        TempDT = ViewState["DTTable"] as DataTable;
        for (int i = TempDT.Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = TempDT.Rows[i];
            if ((Int32)dr[0] == (Int32)RecordID)
            {
                dr["Title"] = ItemArray[1].ToString();
                dr["ContactName"] = ItemArray[2].ToString();
                dr["Position"] = ItemArray[3].ToString();
                dr["EmailID"] = ItemArray[4].ToString();
                dr["PhoneNumber"] = ItemArray[5].ToString();
                dr["MobileNumber"] = ItemArray[6].ToString();
                dr["Note"] = ItemArray[7].ToString(); ;
                dr["AddressType"] = ItemArray[8].ToString();
                dr["AddressLine1"] = ItemArray[9].ToString();
                dr["AddressLine2"] = ItemArray[10].ToString();
                dr["City"] = ItemArray[11].ToString();
                dr["State"] = ItemArray[12].ToString();
                dr["Country"] = ItemArray[13].ToString();
                dr["PinCode"] = ItemArray[14].ToString();
            }
        }
    }

    // For Delete DataTable Record
    private void DeletDTTable(int ID)
    {
        TempDT = ViewState["DTTable"] as DataTable;
        for (int i = TempDT.Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = TempDT.Rows[i];
            if ((Int32)dr[0] == ID)
                dr.Delete();
        }
        TempDT.AcceptChanges();
        ViewState["DTTable"] = TempDT;
    }

    #endregion
    
    protected void BtnSubmitCancel(object sender, EventArgs e)
    {
        if (sender == btnSubmit)
        {
            string ImagePath = GetImagePath(FULogo);

            string CustomerType = "";
            if (RBCompany.Checked == true)
                CustomerType = "Company";
            else if (RBIndividual.Checked == true)
                CustomerType = "Individual";

            #region For Submit button
            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] arrParam = new SqlParameter[28];
                arrParam[0] = new SqlParameter("@Action", "INSERT");
                arrParam[1] = new SqlParameter("@Image", ImagePath);
                arrParam[2] = new SqlParameter("@CustomerCode", TBCustomerCode.Text);
                arrParam[3] = new SqlParameter("@CustomerType", CustomerType);
                arrParam[4] = new SqlParameter("@FirstName", TBFirstName.Text);
                arrParam[5] = new SqlParameter("@LastName", TBLastName.Text);
                arrParam[6] = new SqlParameter("@CompanyName", TBCompanyName.Text);
                arrParam[7] = new SqlParameter("@AddressLine1", TBAddressLine1.Text);
                arrParam[8] = new SqlParameter("@AddressLine2", TBAddressLine2.Text);
                arrParam[9] = new SqlParameter("@City", TBCity.Text);
                arrParam[10] = new SqlParameter("@State", TBState.Text);
                arrParam[11] = new SqlParameter("@Country", TBCountry.Text);
                arrParam[12] = new SqlParameter("@PinCode", TBPinCode.Text);
                arrParam[13] = new SqlParameter("@PhoneNumber", TBPhoneNumber.Text);
                arrParam[14] = new SqlParameter("@MobileNumber", TBMobileNumber.Text);
                arrParam[15] = new SqlParameter("@FaxNumber", TBFaxNumber.Text);
                arrParam[16] = new SqlParameter("@EmailID", TBEmailID.Text);
                arrParam[17] = new SqlParameter("@Website", TBWebsite.Text);
                arrParam[18] = new SqlParameter("@VATNumber", TBVATNumber.Text);
                arrParam[19] = new SqlParameter("@CSTNumber", TBCSTNumber.Text);
                arrParam[20] = new SqlParameter("@TINNumber", TBTINNumber.Text);
                arrParam[21] = new SqlParameter("@PANNumber", TBPANNumber.Text);
                arrParam[22] = new SqlParameter("@SearchCode", TBSearchCode.Text);
                arrParam[23] = new SqlParameter("@BusinessLocation", DDLBusinessLocation.SelectedValue);
                arrParam[24] = new SqlParameter("@BirthDate", TBDOB.Text);
                arrParam[25] = new SqlParameter("@AnniversaryDate", TBAnniversaryDate.Text);
                arrParam[26] = new SqlParameter("@IsActive", CBIsActive.Checked);
                arrParam[27] = new SqlParameter("@RegistrationDate", DateTime.Now);

                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Customer_CRUD", arrParam);
                if (result > 0)
                    ShowMessage("Added Suceess", MessageType.Success);
                else
                    ShowMessage("Some Technical Error Occur!!!", MessageType.Error);

                // Get Item ID

                string SelCustomer = "SELECT TOP(1) ID FROM Customer ORDER BY ID DESC";
                DataSet DSCustomerID = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelCustomer);

                int CustomerID = Convert.ToInt32(DSCustomerID.Tables[0].Rows[0][0]);

                #region Insert Customer Contacts Details
                TempDT = ViewState["DTTable"] as DataTable;
                for (int i = 0; i < TempDT.Rows.Count; i++)
                {
                    DataRow dr = TempDT.Rows[i];
                    SqlParameter[] arrParamDetails = new SqlParameter[16];
                    arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                    arrParamDetails[1] = new SqlParameter("@CustomerID", CustomerID);
                    arrParamDetails[2] = new SqlParameter("@Title", dr[1]);
                    arrParamDetails[3] = new SqlParameter("@ContactName", dr[2]);
                    arrParamDetails[4] = new SqlParameter("@Position", dr[3]);
                    arrParamDetails[5] = new SqlParameter("@EmailID", dr[4]);
                    arrParamDetails[6] = new SqlParameter("@PhoneNumber", dr[5]);
                    arrParamDetails[7] = new SqlParameter("@MobileNumber", dr[6]);
                    arrParamDetails[8] = new SqlParameter("@Note", dr[7]);
                    arrParamDetails[9] = new SqlParameter("@AddressType", dr[8]);
                    arrParamDetails[10] = new SqlParameter("@AddressLine1", dr[9]);
                    arrParamDetails[11] = new SqlParameter("@AddressLine2", dr[10]);
                    arrParamDetails[12] = new SqlParameter("@City", dr[11]);
                    arrParamDetails[13] = new SqlParameter("@State", dr[12]);
                    arrParamDetails[14] = new SqlParameter("@Country", dr[13]);
                    arrParamDetails[15] = new SqlParameter("@PinCode", dr[14]);
                    int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_CustomerContacts_CRUD", arrParamDetails);
                }
                #endregion
                ClearForm();
                MVCustomers.ActiveViewIndex = 0;
                BindGVCustomer();
            }

            #endregion

            #region For Update Button

            else if (btnSubmit.Text == "Update")
            {
                SqlParameter[] arrParam = new SqlParameter[29];
                arrParam[0] = new SqlParameter("@Action", "UPDATE");
                arrParam[1] = new SqlParameter("@Image", ImagePath);
                arrParam[2] = new SqlParameter("@CustomerCode", TBCustomerCode.Text);
                arrParam[3] = new SqlParameter("@CustomerType", CustomerType);
                arrParam[4] = new SqlParameter("@FirstName", TBFirstName.Text);
                arrParam[5] = new SqlParameter("@LastName", TBLastName.Text);
                arrParam[6] = new SqlParameter("@CompanyName", TBCompanyName.Text);
                arrParam[7] = new SqlParameter("@AddressLine1", TBAddressLine1.Text);
                arrParam[8] = new SqlParameter("@AddressLine2", TBAddressLine2.Text);
                arrParam[9] = new SqlParameter("@City", TBCity.Text);
                arrParam[10] = new SqlParameter("@State", TBState.Text);
                arrParam[11] = new SqlParameter("@Country", TBCountry.Text);
                arrParam[12] = new SqlParameter("@PinCode", TBPinCode.Text);
                arrParam[13] = new SqlParameter("@PhoneNumber", TBPhoneNumber.Text);
                arrParam[14] = new SqlParameter("@MobileNumber", TBMobileNumber.Text);
                arrParam[15] = new SqlParameter("@FaxNumber", TBFaxNumber.Text);
                arrParam[16] = new SqlParameter("@EmailID", TBEmailID.Text);
                arrParam[17] = new SqlParameter("@Website", TBWebsite.Text);
                arrParam[18] = new SqlParameter("@VATNumber", TBVATNumber.Text);
                arrParam[19] = new SqlParameter("@CSTNumber", TBCSTNumber.Text);
                arrParam[20] = new SqlParameter("@TINNumber", TBTINNumber.Text);
                arrParam[21] = new SqlParameter("@PANNumber", TBPANNumber.Text);
                arrParam[22] = new SqlParameter("@SearchCode", TBSearchCode.Text);
                arrParam[23] = new SqlParameter("@BusinessLocation", DDLBusinessLocation.SelectedValue);
                arrParam[24] = new SqlParameter("@BirthDate", TBDOB.Text);
                arrParam[25] = new SqlParameter("@AnniversaryDate", TBAnniversaryDate.Text);
                arrParam[26] = new SqlParameter("@IsActive", CBIsActive.Checked);
                arrParam[27] = new SqlParameter("@RegistrationDate", DateTime.Now);
                arrParam[28] = new SqlParameter("@ID", Session["MRecordID"]);

                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Customer_CRUD", arrParam);

                if (result > 0)
                    ShowMessage("Updated Successfully", MessageType.Success);
                else
                    ShowMessage("Error", MessageType.Error);

                // Delete All Records From POProducts Details
                SqlParameter[] arrParamDel = new SqlParameter[2];
                arrParamDel[0] = new SqlParameter("@Action", "DELETE_ByMasterID");
                arrParamDel[1] = new SqlParameter("@CustomerID", Convert.ToInt32(Session["MRecordID"]));
                int Delresult = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_CustomerContacts_CRUD", arrParamDel);

                #region Insert CustomerContacts Details
                TempDT = ViewState["DTTable"] as DataTable;
                for (int i = 0; i < TempDT.Rows.Count; i++)
                {
                    DataRow dr = TempDT.Rows[i];
                    SqlParameter[] arrParamDetails = new SqlParameter[16];
                    arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                    arrParamDetails[1] = new SqlParameter("@CustomerID", Convert.ToInt32(Session["MRecordID"]));
                    arrParamDetails[2] = new SqlParameter("@Title", dr[1]);
                    arrParamDetails[3] = new SqlParameter("@ContactName", dr[2]);
                    arrParamDetails[4] = new SqlParameter("@Position", dr[3]);
                    arrParamDetails[5] = new SqlParameter("@EmailID", dr[4]);
                    arrParamDetails[6] = new SqlParameter("@PhoneNumber", dr[5]);
                    arrParamDetails[7] = new SqlParameter("@MobileNumber", dr[6]);
                    arrParamDetails[8] = new SqlParameter("@Note", dr[7]);
                    arrParamDetails[9] = new SqlParameter("@AddressType", dr[8]);
                    arrParamDetails[10] = new SqlParameter("@AddressLine1", dr[9]);
                    arrParamDetails[11] = new SqlParameter("@AddressLine2", dr[10]);
                    arrParamDetails[12] = new SqlParameter("@City", dr[11]);
                    arrParamDetails[13] = new SqlParameter("@State", dr[12]);
                    arrParamDetails[14] = new SqlParameter("@Country", dr[13]);
                    arrParamDetails[15] = new SqlParameter("@PinCode", dr[14]);
                    int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_CustomerContacts_CRUD", arrParamDetails);
                }
                #endregion
                ClearForm();
                btnSubmit.Text = "Submit";
                MVCustomers.ActiveViewIndex = 0;
                BindGVCustomer();
            }

            #endregion
        }

        else if (sender == btnCancel)
        {
            ClearForm();
            MVCustomers.ActiveViewIndex = 0;
            BindGVCustomer();
        }
    }

    // Method For Add New Product Submit/Cancel 
    protected void CustomerContactSubmitCancel(object sender, EventArgs e)
    {
        if (sender == ModelContactSubmit)
        {
            if (ModelContactSubmit.Text == "Submit")
            {
                string[] DataArray = new string[15];
                DataArray[1] = DDLTitle.SelectedItem.Text;
                DataArray[2] = TBContactName.Text;
                DataArray[3] = TBJobPosition.Text;
                DataArray[4] = TBContactEmail.Text;
                DataArray[5] = TBContactPhone.Text;
                DataArray[6] = TBContactMobile.Text;
                DataArray[7] = TBContactNotes.Text;
                DataArray[8] = DDLAddressType.SelectedItem.Text;
                DataArray[9] = TBContactAddLine1.Text;
                DataArray[10] = TBContactAddLine2.Text;
                DataArray[11] = TBContactCity.Text;
                DataArray[12] = TBContactState.Text;
                DataArray[13] = TBContactCountry.Text;
                DataArray[14] = TBContactPinCode.Text;
                InsertDTTable(DataArray);
                BindInnerGV();
            }

            if (ModelContactSubmit.Text == "Update")
            {
                string[] DataArray = new string[15];
                DataArray[1] = DDLTitle.SelectedItem.Text;
                DataArray[2] = TBContactName.Text;
                DataArray[3] = TBJobPosition.Text;
                DataArray[4] = TBContactEmail.Text;
                DataArray[5] = TBContactPhone.Text;
                DataArray[6] = TBContactMobile.Text;
                DataArray[7] = TBContactNotes.Text;
                DataArray[8] = DDLAddressType.SelectedItem.Text;
                DataArray[9] = TBContactAddLine1.Text;
                DataArray[10] = TBContactAddLine2.Text;
                DataArray[11] = TBContactCity.Text;
                DataArray[12] = TBContactState.Text;
                DataArray[13] = TBContactCountry.Text;
                DataArray[14] = TBContactPinCode.Text;

                UpdateDTTable(Session["IRecordID"], DataArray);
                ModelContactSubmit.Text = "Submit";
            }
            ClearContactForm();
            
        }

        if (sender == ModelContactCancel)
        {
            ClearContactForm();
        }
    }

    // Method For Bind Inner GridView
    private void BindInnerGV()
    {
        RepeaterAddressDetails.DataSource = ViewState["DTTable"] as DataTable;
        RepeaterAddressDetails.DataBind();
    }

    // Method For Clear Model Data
    private void ClearContactForm()
    {
        DDLTitle.ClearSelection();
        TBContactName.Text = string.Empty;
        TBJobPosition.Text = string.Empty;
        TBContactEmail.Text = string.Empty;
        TBContactPhone.Text = string.Empty;
        TBContactMobile.Text = string.Empty;
        TBContactNotes.Text = string.Empty;
        DDLAddressType.ClearSelection();
        TBContactAddLine1.Text = string.Empty;
        TBContactAddLine2.Text = string.Empty;
        TBContactCity.Text = string.Empty;
        TBContactState.Text = string.Empty;
        TBContactCountry.Text = string.Empty;
        TBContactPinCode.Text = string.Empty;
        ModelContactSubmit.Text = "Submit";
    }

    protected void GVCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int MRecordID = Convert.ToInt32(e.CommandArgument);
        Session["MRecordID"] = MRecordID;

        #region For Command EditRecord
        
        if (e.CommandName == "EditRecord")
        {
            MVCustomers.ActiveViewIndex = 1;
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
            arrParam[1] = new SqlParameter("@ID", MRecordID);
            DataSet DSRecords1 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Customer_CRUD", arrParam);
            if (DSRecords1.Tables[0].Rows.Count > 0)
            {
                ImgLogo.ImageUrl = DSRecords1.Tables[0].Rows[0]["Image"].ToString();
                TBCustomerCode.Text = DSRecords1.Tables[0].Rows[0]["CustomerCode"].ToString();
                TBCustomerCode.Enabled = false;

                if (DSRecords1.Tables[0].Rows[0]["CustomerType"].ToString() == "Individual") RBIndividual.Checked = true;
                if (DSRecords1.Tables[0].Rows[0]["CustomerType"].ToString() == "Company") RBCompany.Checked = true;

                TBFirstName.Text = DSRecords1.Tables[0].Rows[0]["FirstName"].ToString();
                TBLastName.Text = DSRecords1.Tables[0].Rows[0]["LastName"].ToString();
                TBCompanyName.Text = DSRecords1.Tables[0].Rows[0]["CompanyName"].ToString();
                TBAddressLine1.Text = DSRecords1.Tables[0].Rows[0]["AddressLine1"].ToString();
                TBAddressLine2.Text = DSRecords1.Tables[0].Rows[0]["AddressLine2"].ToString();
                TBCity.Text = DSRecords1.Tables[0].Rows[0]["City"].ToString();
                TBState.Text = DSRecords1.Tables[0].Rows[0]["State"].ToString();
                TBCountry.Text = DSRecords1.Tables[0].Rows[0]["Country"].ToString();
                TBPinCode.Text = DSRecords1.Tables[0].Rows[0]["PinCode"].ToString();
                TBPhoneNumber.Text = DSRecords1.Tables[0].Rows[0]["PhoneNumber"].ToString();
                TBMobileNumber.Text = DSRecords1.Tables[0].Rows[0]["MobileNumber"].ToString();
                TBFaxNumber.Text = DSRecords1.Tables[0].Rows[0]["FaxNumber"].ToString();
                TBEmailID.Text = DSRecords1.Tables[0].Rows[0]["EmailID"].ToString();
                TBWebsite.Text = DSRecords1.Tables[0].Rows[0]["Website"].ToString();
                TBVATNumber.Text = DSRecords1.Tables[0].Rows[0]["VATNumber"].ToString();
                TBCSTNumber.Text = DSRecords1.Tables[0].Rows[0]["CSTNumber"].ToString();
                TBTINNumber.Text = DSRecords1.Tables[0].Rows[0]["TINNumber"].ToString();
                TBPANNumber.Text = DSRecords1.Tables[0].Rows[0]["PANNumber"].ToString();
                TBSearchCode.Text = DSRecords1.Tables[0].Rows[0]["SearchCode"].ToString();
                
                BindBusinessLocation();
                DDLBusinessLocation.ClearSelection();
                DDLBusinessLocation.Items.FindByValue(DSRecords1.Tables[0].Rows[0]["BusinessLocation"].ToString()).Selected = true;

                TBDOB.Text = DSRecords1.Tables[0].Rows[0]["BirthDate"].ToString();
                TBAnniversaryDate.Text = DSRecords1.Tables[0].Rows[0]["AnniversaryDate"].ToString();
                CBIsActive.Checked = Convert.ToBoolean(DSRecords1.Tables[0].Rows[0]["IsActive"].ToString());
            }

            SqlParameter[] arrParam2 = new SqlParameter[2];
            arrParam2[0] = new SqlParameter("@Action", "SELECT_ByMasterID");
            arrParam2[1] = new SqlParameter("@CustomerID", MRecordID);
            DataSet DSRecords2 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_CustomerContacts_CRUD", arrParam2);
            // Bind Table
            CreateDTTable();
            TempDT = ViewState["DTTable"] as DataTable;
            for (int i = 0; i < DSRecords2.Tables[0].Rows.Count; i++)
            {
                TempDT.Rows.Add(null,
                DSRecords2.Tables[0].Rows[i]["Title"].ToString(),
                DSRecords2.Tables[0].Rows[i]["ContactName"].ToString(),
                DSRecords2.Tables[0].Rows[i]["Position"].ToString(),
                DSRecords2.Tables[0].Rows[i]["EmailID"].ToString(),
                DSRecords2.Tables[0].Rows[i]["PhoneNumber"].ToString(),
                DSRecords2.Tables[0].Rows[i]["MobileNumber"].ToString(),
                DSRecords2.Tables[0].Rows[i]["Note"].ToString(),
                DSRecords2.Tables[0].Rows[i]["AddressType"].ToString(),
                DSRecords2.Tables[0].Rows[i]["AddressLine1"].ToString(),
                DSRecords2.Tables[0].Rows[i]["AddressLine2"].ToString(),
                DSRecords2.Tables[0].Rows[i]["City"].ToString(),
                DSRecords2.Tables[0].Rows[i]["State"].ToString(),
                DSRecords2.Tables[0].Rows[i]["Country"].ToString(),
                DSRecords2.Tables[0].Rows[i]["PinCode"].ToString());
            }
            RepeaterAddressDetails.DataSource = TempDT;
            RepeaterAddressDetails.DataBind();
            ViewState["DTTable"] = TempDT;
            btnSubmit.Text = "Update";
        }

        #endregion

        #region For Command DeleteRecord

        if (e.CommandName == "DeleteRecord")
        {
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "DELETE_ByMasterID");
            arrParam[1] = new SqlParameter("@POID", MRecordID);
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_CustomerContacts_CRUD", arrParam);

            SqlParameter[] arrParam1 = new SqlParameter[2];
            arrParam1[0] = new SqlParameter("@Action", "DELETE");
            arrParam1[1] = new SqlParameter("@ID", MRecordID);
            int result1 = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Customer_CRUD", arrParam1);

            BindGVCustomer();
        }

        #endregion

        #region For Command ChangeStatus

        if (e.CommandName == "ChangeStatus")
        {
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
            arrParam[1] = new SqlParameter("@ID", MRecordID);
            DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Customer_CRUD", arrParam);
            DataTable DTRecords = DSRecords.Tables[0];
            if (DTRecords.Rows.Count > 0)
            {
                #region Change Status True To False
                if (DTRecords.Rows[0]["Status"].ToString() == "True")
                {
                    SqlParameter[] arrParam2 = new SqlParameter[3];
                    arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                    arrParam2[1] = new SqlParameter("@Status", false);
                    arrParam2[2] = new SqlParameter("@ID", MRecordID);
                    int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Customer_CRUD", arrParam2);
                }
                #endregion

                #region Change Status False TO True
                if (DTRecords.Rows[0]["Status"].ToString() == "False")
                {
                    SqlParameter[] arrParam2 = new SqlParameter[3];
                    arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                    arrParam2[1] = new SqlParameter("@Status", true);
                    arrParam2[2] = new SqlParameter("@ID", MRecordID);
                    int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Customer_CRUD", arrParam2);
                }
                #endregion
            }

            BindGVCustomer();
        }

        #endregion

    }

    protected void GVCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVCustomer.PageIndex = e.NewPageIndex;
        BindGVCustomer();
        GVCustomer.DataBind();
    }
    
    protected void RepeaterAddressDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "DelContact")
        {
            DataTable TempDT = ViewState["DTTable"] as DataTable;
            int ContactID=Convert.ToInt32(e.CommandArgument);
            DeletDTTable(ContactID);
            RepeaterAddressDetails.DataSource = ViewState["DTTable"] as DataTable;
            RepeaterAddressDetails.DataBind();
        }
    }

    protected void LBExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string fileName = "Customer_Report" + System.DateTime.Now + ".xls";
            string Extension = ".xls";
            if (Extension == ".xls")
            {
                for (int i = 0; i < GVCustomer.Columns.Count; i++)
                {
                    GVCustomer.Columns[i].Visible = true;
                }

                BindGVCustomer();

                clsConnectionSql.PrepareControlForExport(GVCustomer);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                try
                {
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                        {
                            //Create a form to contain the grid
                            System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table();
                            table.GridLines = GVCustomer.GridLines;
                            //add the header row to the table
                            if (GVCustomer.HeaderRow != null)
                            {
                                clsConnectionSql.PrepareControlForExport(GVCustomer.HeaderRow);
                                table.Rows.Add(GVCustomer.HeaderRow);
                            }
                            //add each of the data rows to the table
                            foreach (GridViewRow row in GVCustomer.Rows)
                            {
                                clsConnectionSql.PrepareControlForExport(row);
                                table.Rows.Add(row);
                            }

                            //add the footer row to the table
                            if (GVCustomer.FooterRow != null)
                            {
                                clsConnectionSql.PrepareControlForExport(GVCustomer.FooterRow);
                                table.Rows.Add(GVCustomer.FooterRow);
                            }
                            //render the table into the htmlwriter
                            GVCustomer.GridLines = GridLines.Both;
                            table.RenderControl(htw);
                            HttpContext.Current.Response.Write(sw.ToString());
                            HttpContext.Current.Response.End();
                        }
                    }
                }
                catch (HttpException ex)
                {
                    ShowMessage(ex.Message, MessageType.Error);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }


    }

    #region Upload Section

    protected void ModelSubmitCancel(object sender, EventArgs e)
    {
        if (sender == BtnUploadPOSubmit)
        {
            //if File is not selected then return  
            if (FUPO.HasFile)
            {
                //Get the file extension  
                string fileExtension = Path.GetExtension(FUPO.PostedFile.FileName);

                //If file is not in excel format then return  
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    return;
                }

                //Get the File name and create new path to save it on server  
                string fileLocation = Server.MapPath("~/Uploader/") + FUPO.PostedFile.FileName;

                //if the File is exist on serevr then delete it  
                if (File.Exists(fileLocation))
                {
                    File.Delete(fileLocation);
                }

                //save the file on the server before loading  
                FUPO.SaveAs(fileLocation);

                //Create the QueryString for differnt version of fexcel file  
                string strConn = "";
                switch (fileExtension)
                {
                    case ".xls":
                        //Excel 1997-2003  
                        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                        break;
                    case ".xlsx":
                        //Excel 2007-2010  
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0 xml;HDR=Yes;IMEX=1\"";
                        break;
                }
                //Get the sheets data and bind that data to the grids  
                BindData(strConn);

                //Delete the excel file from the server  
                File.Delete(fileLocation);
            }
            else
            {
                ShowMessage("Please Select File First !!!", MessageType.Error);
            }

            ShowMessage("File successfully Uploaded........................", MessageType.Success);
        }
        if (sender == BtnUploadPOCancel)
        {
            MVCustomers.ActiveViewIndex = 0;
        }
    }

    private void BindData(string strConn)
    {
        OleDbConnection objConn = new OleDbConnection(strConn);
        objConn.Open();

        // Get the data table containg the schema guid.  
        DataTable dt = null;
        dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        objConn.Close();

        if (dt.Rows.Count > 0)
        {
            int i = 0;

            // Bind the sheets to the Grids  
            foreach (DataRow row in dt.Rows)
            {
                DataTable dt_sheet = null;
                dt_sheet = getSheetData(strConn, row["TABLE_NAME"].ToString());
                switch (i)
                {
                    case 0:
                        FillCustomer(dt_sheet);
                        break;
                }
                i++;
            }
        }
    }

    private void FillCustomer(DataTable dt_sheet)
    {
       
        if (dt_sheet.Rows.Count > 0)
        {
            for (int i = 0; i < dt_sheet.Rows.Count; i++)
            {
                // Get Business Location ID
                int BL = 0;
                string Query = "SELECT * FROM BusinessLocation WHERE BusinessLocationName='" + dt_sheet.Rows[i]["BusinessLocation"] + "' ";
                DataTable DTBL = clsConnectionSql.filldt(Query);
                if (DTBL.Rows.Count > 0)
                {
                    BL = Convert.ToInt32(DTBL.Rows[0][0]);
                }
                    
                // Insert Customer Details
                string InsertCustomer = "INSERT INTO Customer (Image,CustomerCode,CustomerType,FirstName,LastName,CompanyName,AddressLine1,AddressLine2,City,State,Country,PinCode,PhoneNumber,MobileNumber,FaxNumber,EmailID,Website,VATNumber,CSTNumber,TINNumber,PANNumber,SearchCode,BusinessLocation,BirthDate,AnniversaryDate,IsActive,Status) VALUES ('" + dt_sheet.Rows[i]["Image"] + "','" + dt_sheet.Rows[i]["CustomerCode"] + "','" + dt_sheet.Rows[i]["CustomerType"] + "','" + dt_sheet.Rows[i]["FirstName"] + "','" + dt_sheet.Rows[i]["LastName"] + "','" + dt_sheet.Rows[i]["CompanyName"] + "','" + dt_sheet.Rows[i]["AddressLine1"] + "','" + dt_sheet.Rows[i]["AddressLine2"] + "','" + dt_sheet.Rows[i]["City"] + "','" + dt_sheet.Rows[i]["State"] + "','" + dt_sheet.Rows[i]["Country"] + "','" + dt_sheet.Rows[i]["PinCode"] + "','" + dt_sheet.Rows[i]["PhoneNumber"] + "','" + dt_sheet.Rows[i]["MobileNumber"] + "','" + dt_sheet.Rows[i]["FaxNumber"] + "','" + dt_sheet.Rows[i]["EmailID"] + "','" + dt_sheet.Rows[i]["Website"] + "','" + dt_sheet.Rows[i]["VATNumber"] + "','" + dt_sheet.Rows[i]["CSTNumber"] + "','" + dt_sheet.Rows[i]["TINNumber"] + "','" + dt_sheet.Rows[i]["PANNumber"] + "','" + dt_sheet.Rows[i]["SearchCode"] + "','" + BL + "','" + dt_sheet.Rows[i]["BirthDate"] + "','" + dt_sheet.Rows[i]["AnniversaryDate"] + "','" + dt_sheet.Rows[i]["IsActive"] + "','" + true + "')";
                int InsResult = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.Text, InsertCustomer);
                if (InsResult > 0)
                {
                    ShowMessage("Customer Uploding Successfully Completed. ", MessageType.Success);
                }
                else
                {
                    ShowMessage("Error In Customer Uploading... ", MessageType.Error);
                }
            }
        }
    }


    private DataTable getSheetData(string strConn, string sheet)
    {
        string query = "select * from [" + sheet + "]";
        OleDbConnection objConn;
        OleDbDataAdapter oleDA;
        DataTable dt = new DataTable();
        objConn = new OleDbConnection(strConn);
        objConn.Open();
        oleDA = new OleDbDataAdapter(query, objConn);
        oleDA.Fill(dt);
        objConn.Close();
        oleDA.Dispose();
        objConn.Dispose();
        return dt;
    }

    #endregion

    protected void LBUploadPO_Click(object sender, EventArgs e)
    {
        MVCustomers.ActiveViewIndex = 2;
    }

    protected void LBFile_Click(object sender, EventArgs e)
    {
        string filename = "~/Uploader/UploadCustomer.xlsx";
        if (filename != "")
        {
            string path = Server.MapPath(filename);
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.End();
            }
            else
            {
                Response.Write("This file does not exist.");
            }
        }
    }
}