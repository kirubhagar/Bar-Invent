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

public partial class EmployeeManager : System.Web.UI.Page
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
            BindGVEmployees();
        }
    }

    private void BindGVEmployees()
    {
        string SelectQuery = "SELECT * FROM View_EmployeeDetails";
        BindGVEmployee(SelectQuery);
    }

    #region Event Set For View0 

    private void BindGVEmployee(string QueryText)
    {
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, QueryText);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            GVEmployee.DataSource = DSRecord.Tables[0];
            GVEmployee.DataBind();
        }
        else
        {
            GVEmployee.EmptyDataText = "No Record Found !";
            GVEmployee.DataBind();
        }
        foreach (GridViewRow Row in GVEmployee.Rows)
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
    
    protected void TopEventSet(object sender, EventArgs e)
    {
        if (sender == LBAddNew)
        {
            MVEmployee.ActiveViewIndex = 1;
            ViewOneLoad();
        }
    }

    protected void RadioEventSet(object sender, EventArgs e)
    {
        if (sender == RBShowActiveOnly)
        {
            string SelQuery = "SELECT * FROM View_EmployeeDetails WHERE View_EmployeeDetails.IsActive ='" + true + "'";
            BindGVEmployee(SelQuery);
        }

        else if (sender == RBShowInactiveOnly)
        {
            string SelQuery = "SELECT * FROM View_EmployeeDetails WHERE View_EmployeeDetails.IsActive ='" + false + "'";
            BindGVEmployee(SelQuery);
        }

        else if (sender == RBShowAll)
        {
            string SelectQuery = "SELECT * FROM View_EmployeeDetails";
            BindGVEmployee(SelectQuery);
        }
    }

    protected void ShowHideImageEvent(object sender, EventArgs e)
    {
        if (CBShowImage.Checked)
            GVEmployee.Columns[1].Visible = true;
        else
            GVEmployee.Columns[1].Visible = false;
    }

    protected void SearchEvent(object sender, EventArgs e)
    {
        string SearchQuery = "SELECT * FROM View_EmployeeDetails WHERE View_EmployeeDetails.EmployeeName LIKE '" + TBSearch.Text.Trim() + "%'";
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SearchQuery);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            GVEmployee.DataSource = DSRecord.Tables[0];
            GVEmployee.DataBind();
        }
        else
        {
            GVEmployee.EmptyDataText = "No Record Found !";
            GVEmployee.DataBind();
        }
        foreach (GridViewRow Row in GVEmployee.Rows)
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

    #endregion

    #region Event Set For View1

    private void ViewOneLoad()
    {
        LoadEmployeeCode();
        BindDDLBusinessLocation();
        BindDDLDepartment();
        BindDDLDesignation();
        BindDDLWorkShift();
    }

    #endregion

    #region Additional Methods

    private void LoadEmployeeCode()
    {
        string PrvID = ""; int NextID = 1;
        string GetMaxId = "SELECT TOP(1) ID FROM Employee ORDER BY ID DESC ";
        DataSet DSRecordID = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, GetMaxId);
        if (DSRecordID.Tables[0].Rows.Count > 0)
        {
            PrvID = DSRecordID.Tables[0].Rows[0]["ID"].ToString();
            NextID = Convert.ToInt32(PrvID.ToString()) + 1;
        }

        string SelQuery = "SELECT * FROM View_CodeDetails WHERE View_CodeDetails.Type='" + 14 + "'";
        DataSet DSCode = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
        if (DSCode.Tables[0].Rows.Count > 0)
        {
            string Zero = new String('0', (Convert.ToInt32(DSCode.Tables[0].Rows[0]["MinCodeLength"].ToString()) - NextID.ToString().Length));

            if (Convert.ToBoolean(DSCode.Tables[0].Rows[0]["IsBLApplicable"].ToString()) == true)
                TBEmpCode.Text = DSCode.Tables[0].Rows[0]["DocumentPrefix"].ToString() +
                              DSCode.Tables[0].Rows[0]["Prefix"].ToString() +
                              Zero +
                              NextID.ToString();
            else
                TBEmpCode.Text = DSCode.Tables[0].Rows[0]["Prefix"].ToString() +
                              Zero +
                              NextID.ToString();
            TBEmpCode.Enabled = false;
        }
    }
    
    protected void DefineMyOwn(object sender, EventArgs e)
    {
        if (sender == btnEmployeeOption)
        {
            if (btnEmployeeOption.Text == "Define My Own")
            {
                TBEmpCode.Text = "";
                TBEmpCode.Enabled = true;
                TBEmpCode.Focus();
                btnEmployeeOption.Text = "Auto Generate";
            }

            else if (btnEmployeeOption.Text == "Auto Generate")
            {
                LoadEmployeeCode();
                TBEmpCode.Enabled = false;
                btnEmployeeOption.Text = "Define My Own";
            }
        }
    }
    
    private void BindDDLBusinessLocation()
    {
        SqlParameter[] arrparm = new SqlParameter[2];
        arrparm[0] = new SqlParameter("@Action", "SELECT_ByStatus");
        arrparm[1] = new SqlParameter("@Status", true);
        DataSet DSRecordBusinessLocation = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrparm);
        if (DSRecordBusinessLocation.Tables[0].Rows.Count > 0)
        {
            DDLBusinessLocation.DataSource = DSRecordBusinessLocation.Tables[0];
            DDLBusinessLocation.DataTextField = "BusinessLocationName";
            DDLBusinessLocation.DataValueField = "ID";
            DDLBusinessLocation.DataBind();
            DDLBusinessLocation.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void BindDDLDepartment()
    {
        SqlParameter[] arrparm = new SqlParameter[2];
        arrparm[0] = new SqlParameter("@Action", "SELECT_ByStatus");
        arrparm[1] = new SqlParameter("@Status", true);
        DataSet DSRecordDepartment = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_EmployeeDepartment_CRUD", arrparm);
        if (DSRecordDepartment.Tables[0].Rows.Count > 0)
        {
            DDLDepartment.DataSource = DSRecordDepartment.Tables[0];
            DDLDepartment.DataTextField = "Name";
            DDLDepartment.DataValueField = "ID";
            DDLDepartment.DataBind();
            DDLDepartment.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void BindDDLDesignation()
    {
        SqlParameter[] arrparm = new SqlParameter[2];
        arrparm[0] = new SqlParameter("@Action", "SELECT_ByStatus");
        arrparm[1] = new SqlParameter("@Status", true);
        DataSet DSRecordDesignation = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_EmployeeDesignation_CRUD", arrparm);
        if (DSRecordDesignation.Tables[0].Rows.Count > 0)
        {
            DDLEmployeeDesignation.DataSource = DSRecordDesignation.Tables[0];
            DDLEmployeeDesignation.DataTextField = "Name";
            DDLEmployeeDesignation.DataValueField = "ID";
            DDLEmployeeDesignation.DataBind();
            DDLEmployeeDesignation.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void BindDDLWorkShift()
    {
        SqlParameter[] arrparm = new SqlParameter[2];
        arrparm[0] = new SqlParameter("@Action", "SELECT_ByStatus");
        arrparm[1] = new SqlParameter("@Status", true);
        DataSet DSRecordWorkShift = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_EmployeeWorkShift_CRUD", arrparm);
        if (DSRecordWorkShift.Tables[0].Rows.Count > 0)
        {
            DDLWorkingShift.DataSource = DSRecordWorkShift.Tables[0];
            DDLWorkingShift.DataTextField = "Name";
            DDLWorkingShift.DataValueField = "ID";
            DDLWorkingShift.DataBind();
            DDLWorkingShift.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    #endregion

    protected void ModelsBtnClickEvents(object sender, EventArgs e)
    {
        if (sender == ModelBtnSubmitDep)
            BindDDLDepartment();
        else if (sender == ModelBtnSubmitDesi)
            BindDDLDesignation();
        else if (sender == ModelBtnSubmitWorkShift)
            BindDDLWorkShift();
    }

    protected void EmpSubmitCancel(object sender, EventArgs e)
    {
        if (sender == btnSubmit)
        {
            string ImagePath = GetImagePath(FULogo);

            int ContactID = 0, AddressID = 0, EmpID = 0, UserID = 0;

            if (btnSubmit.Text == "Submit")
            {
                ContactID = InsertContact();
                if (ContactID > 0)
                {
                    AddressID = InsertAddress(ContactID);
                    UserID = InsertUserDetail(ImagePath);
                    if (AddressID > 0 && UserID > 0)
                    {
                        EmpID = InsertEmployee(AddressID, ImagePath, UserID);

                        if (EmpID > 0)
                        {
                            ShowMessage("Employee Details SuccessFully Inserted. ", MessageType.Success);
                        }
                        else
                            ShowMessage("Employee Details Not Inserted!", MessageType.Error);
                    }
                    else
                        ShowMessage("Address Details Not Inserted !", MessageType.Error);
                }
                else
                    ShowMessage("Contact Details Not Inserted !!", MessageType.Error);

                ClearForm();
                MVEmployee.ActiveViewIndex = 0;
                BindGVEmployees();
            }
            else if (btnSubmit.Text == "Update")
            {
                EmpID = Convert.ToInt32(Session["RecordID"].ToString());
                int[] AddUser = new int[2];
                AddUser = UpdateEmployee(EmpID, ImagePath);
                ContactID = UpdateAddress(AddUser[0]);
                UpdateContact(ContactID);
                UpdateUserDetail(ImagePath, AddUser[1]);
                
                ClearForm();
                btnSubmit.Text = "Submit";
                MVEmployee.ActiveViewIndex = 0;
                BindGVEmployees();
            }
        }

        else if (sender == btnCancel)
        {
            ClearForm();
            MVEmployee.ActiveViewIndex = 0;
            BindGVEmployees();
        }
    }

    private int[] UpdateEmployee(int EmployeeID, string ImagePath)
    {
        int[] RArray = new int[2] { 0, 0 };
        SqlParameter[] arrParam = new SqlParameter[17];
        arrParam[0] = new SqlParameter("@Action", "UPDATE");
        arrParam[1] = new SqlParameter("@Image", ImagePath);
        arrParam[2] = new SqlParameter("@EmployeeCode", TBEmpCode.Text);
        arrParam[3] = new SqlParameter("@FirstName", TBFirstName.Text);
        arrParam[4] = new SqlParameter("@MiddleName", TBMiddleName.Text);
        arrParam[5] = new SqlParameter("@LastName", TBLastName.Text);
        arrParam[6] = new SqlParameter("@SearchCode", TBSearchCode.Text);
        arrParam[7] = new SqlParameter("@DateOfBirth", Convert.ToDateTime(TBDOB.Text));
        arrParam[8] = new SqlParameter("@MartialStatus", DDLMaritalStatus.SelectedItem.Text);
        arrParam[9] = new SqlParameter("@Gender", DDLGender.SelectedItem.Text);
        arrParam[10] = new SqlParameter("@DateOfJoining", Convert.ToDateTime(TBDOJ.Text));
        arrParam[11] = new SqlParameter("@BusinessLocation", DDLBusinessLocation.SelectedValue);
        arrParam[12] = new SqlParameter("@Department", DDLDepartment.SelectedValue);
        arrParam[13] = new SqlParameter("@Designation", DDLEmployeeDesignation.SelectedValue);
        arrParam[14] = new SqlParameter("@NotAnEmployee", Convert.ToBoolean(CBNotAnEmployee.Checked));
        arrParam[15] = new SqlParameter("@WorkingShift", DDLWorkingShift.SelectedValue);
        arrParam[16] = new SqlParameter("@ID", EmployeeID);
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Employee_CRUD", arrParam);

        string SelQuery = "SELECT Address FROM Employee WHERE ID='" + EmployeeID + "'";
        DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
        if (DSRecord.Tables[0].Rows.Count > 0)
        {
            RArray[0] = Convert.ToInt32(DSRecord.Tables[0].Rows[0]["Address"].ToString());
            RArray[1] = Convert.ToInt32(DSRecord.Tables[0].Rows[0]["UserID"].ToString());
            return RArray;
        }
        else
            return RArray;
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

    private void UpdateUserDetail(string ImagePath,int UserID)
    {
        int ComID = Convert.ToInt32(Session["ComID"]);
        int NGOID = Convert.ToInt32(Session["NGOID"]);
        //Insert Default Godown Details
        SqlParameter[] arrParam = new SqlParameter[12];
        arrParam[0] = new SqlParameter("@Action", "UPDATE");
        arrParam[1] = new SqlParameter("@Image", ImagePath);
        arrParam[2] = new SqlParameter("@ComID", ComID);
        arrParam[3] = new SqlParameter("@NGOID", NGOID);
        arrParam[4] = new SqlParameter("@FirstName", TBFirstName.Text);
        arrParam[6] = new SqlParameter("@UserName", TBUserName.Text);
        arrParam[7] = new SqlParameter("@PassWord", TBRPassword.Text);
        arrParam[8] = new SqlParameter("@Email", TBEmail.Text);
        arrParam[9] = new SqlParameter("@Phone", TBPhoneNumber.Text);
        arrParam[10] = new SqlParameter("@Mobile", TBMobileNumber.Text);
        arrParam[11] = new SqlParameter("@ID", UserID);
        int resultDefaultGodown = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);

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

    private int InsertEmployee(int AddressID, string ImagePath,int UserID)
    {
        SqlParameter[] arrParam = new SqlParameter[18];
        arrParam[0] = new SqlParameter("@Action", "INSERT");
        arrParam[1] = new SqlParameter("@Image", ImagePath);
        arrParam[2] = new SqlParameter("@EmployeeCode", TBEmpCode.Text);
        arrParam[3] = new SqlParameter("@FirstName", TBFirstName.Text);
        arrParam[4] = new SqlParameter("@MiddleName", TBMiddleName.Text);
        arrParam[5] = new SqlParameter("@LastName", TBLastName.Text);
        arrParam[6] = new SqlParameter("@SearchCode", TBSearchCode.Text);
        arrParam[7] = new SqlParameter("@DateOfBirth", Convert.ToDateTime(TBDOB.Text));
        arrParam[8] = new SqlParameter("@MartialStatus", DDLMaritalStatus.SelectedItem.Text);
        arrParam[9] = new SqlParameter("@Gender", DDLGender.SelectedItem.Text);
        arrParam[10] = new SqlParameter("@DateOfJoining", Convert.ToDateTime(TBDOJ.Text));
        arrParam[11] = new SqlParameter("@BusinessLocation", DDLBusinessLocation.SelectedValue);
        arrParam[12] = new SqlParameter("@Department", DDLDepartment.SelectedValue);
        arrParam[13] = new SqlParameter("@Designation", DDLEmployeeDesignation.SelectedValue);
        arrParam[14] = new SqlParameter("@NotAnEmployee", Convert.ToBoolean(CBNotAnEmployee.Checked));
        arrParam[15] = new SqlParameter("@Address", AddressID);
        arrParam[16] = new SqlParameter("@UserID", UserID);
        arrParam[17] = new SqlParameter("@WorkingShift", DDLWorkingShift.SelectedValue);
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Employee_CRUD", arrParam);

        string MaxQuery = "SELECT Top(1) ID FROM Employee ORDER BY ID DESC";
        DataSet DSMax = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, MaxQuery);
        if (DSMax.Tables[0].Rows.Count > 0)
            return Convert.ToInt32(DSMax.Tables[0].Rows[0]["ID"]);
        return 0;
    }

    private int InsertUserDetail(string ImagePath)
    {
        int ComID = Convert.ToInt32(Session["ComID"]);
        int NGOID = Convert.ToInt32(Session["NGOID"]);
        //Insert Default Godown Details
        SqlParameter[] arrParam = new SqlParameter[11];
        arrParam[0] = new SqlParameter("@Action", "INSERT");
        arrParam[1] = new SqlParameter("@Image", ImagePath);
        arrParam[2] = new SqlParameter("@ComID", ComID);
        arrParam[3] = new SqlParameter("@NGOID", NGOID);
        arrParam[4] = new SqlParameter("@FirstName", TBFirstName.Text);
        arrParam[6] = new SqlParameter("@UserName", TBUserName.Text);
        arrParam[7] = new SqlParameter("@PassWord", TBRPassword.Text);
        arrParam[8] = new SqlParameter("@Email", TBEmail.Text);
        arrParam[9] = new SqlParameter("@Phone", TBPhoneNumber.Text);
        arrParam[10] = new SqlParameter("@Mobile", TBMobileNumber.Text);
        int resultDefaultGodown = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Users_CRUD", arrParam);

        string MaxQuery = "SELECT Top(1) ID FROM Users ORDER BY ID DESC";
        DataSet DSMax = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, MaxQuery);
        if (DSMax.Tables[0].Rows.Count > 0)
            return Convert.ToInt32(DSMax.Tables[0].Rows[0]["ID"]);
        return 0;
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
        TBEmpCode.Text = string.Empty;
        TBFirstName.Text = string.Empty;
        TBMiddleName.Text = string.Empty;
        TBLastName.Text = string.Empty;
        TBSearchCode.Text = string.Empty;
        TBDOB.Text = string.Empty;
        DDLMaritalStatus.SelectedIndex = 0;
        DDLGender.SelectedIndex = 0;
        TBDOJ.Text = string.Empty;
        DDLBusinessLocation.ClearSelection();
        DDLDepartment.ClearSelection();
        DDLEmployeeDesignation.ClearSelection();
        CBNotAnEmployee.Checked = false;
        DDLWorkingShift.ClearSelection();
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
        btnSubmit.Text = "Submit";
    }
    
    protected void GVEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVEmployee.PageIndex = e.NewPageIndex;
        BindGVEmployees();
        GVEmployee.DataBind();
    }
    
    protected void GVEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int recordId = Convert.ToInt32(e.CommandArgument);
            Session["RecordId"] = recordId;

            #region For Edit Record
            if (e.CommandName == "EditRecord")
            {
                ShowEmployeeDetails(recordId);
            }
            #endregion

            #region For Delete Record
            if (e.CommandName == "DeleteRecord")
            {
                // Check Existing Record
                DeleteEmployee(recordId);
                BindGVEmployees();
            }
            #endregion

            #region For Change Status
            if (e.CommandName == "ChangeStatus")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Employee_CRUD", arrParam);
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
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Employee_CRUD", arrParam2);
                    }
                    #endregion

                    #region Change Status False TO True
                    if (DTRecords.Rows[0]["Status"].ToString() == "False")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", true);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Employee_CRUD", arrParam2);
                    }
                    #endregion
                }
            }
            #endregion

            BindGVEmployees();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void ShowEmployeeDetails(int recordId)
    {
        string Query = "SELECT * FROM View_EmployeeDetails WHERE ID='" + recordId + "'";
        DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, Query);
        DataTable DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
        {
            MVEmployee.ActiveViewIndex = 1;
            ImgLogo.ImageUrl = DTRecords.Rows[0]["Image"].ToString();
            TBEmpCode.Text = DTRecords.Rows[0]["EmployeeCode"].ToString();
            TBFirstName.Text = DTRecords.Rows[0]["FirstName"].ToString();
            TBMiddleName.Text = DTRecords.Rows[0]["MiddleName"].ToString();
            TBLastName.Text = DTRecords.Rows[0]["LastName"].ToString();
            TBSearchCode.Text = DTRecords.Rows[0]["SearchCode"].ToString();
            TBDOB.Text = DTRecords.Rows[0]["DateOfBirth"].ToString();
            DDLMaritalStatus.ClearSelection();
            DDLMaritalStatus.Items.FindByText(DTRecords.Rows[0]["MartialStatus"].ToString()).Selected = true;
            DDLGender.ClearSelection();
            DDLGender.Items.FindByText(DTRecords.Rows[0]["Gender"].ToString()).Selected = true;
            TBDOJ.Text = DTRecords.Rows[0]["DateOfJoining"].ToString();
            BindDDLBusinessLocation();
            DDLBusinessLocation.ClearSelection();
            DDLBusinessLocation.Items.FindByValue(DTRecords.Rows[0]["BusinessLocation"].ToString()).Selected = true;
            BindDDLDepartment();
            DDLDepartment.ClearSelection();
            DDLDepartment.Items.FindByValue(DTRecords.Rows[0]["Department"].ToString()).Selected = true;
            BindDDLDesignation();
            DDLEmployeeDesignation.ClearSelection();
            DDLEmployeeDesignation.Items.FindByValue(DTRecords.Rows[0]["Designation"].ToString()).Selected = true;
            //CBCanRequestReportViaSMS.Checked = Convert.ToBoolean(DTRecords.Rows[0]["EmployeeCode"].ToString());
            CBNotAnEmployee.Checked = Convert.ToBoolean(DTRecords.Rows[0]["NotAnEmployee"].ToString());
            BindDDLWorkShift();
            DDLWorkingShift.ClearSelection();
            DDLWorkingShift.Items.FindByValue(DTRecords.Rows[0]["WorkingShift"].ToString()).Selected = true;
            TBAddressLine1.Text = DTRecords.Rows[0]["Address1"].ToString();
            TBAddressLine2.Text = DTRecords.Rows[0]["Address2"].ToString();
            TBCity.Text = DTRecords.Rows[0]["City"].ToString();
            TBState.Text = DTRecords.Rows[0]["State"].ToString();
            TBCountry.Text = DTRecords.Rows[0]["Country"].ToString();
            TBPin.Text = DTRecords.Rows[0]["ZipCode"].ToString();
            TBPhoneNumber.Text = DTRecords.Rows[0]["TelephoneNumber"].ToString();
            TBMobileNumber.Text = DTRecords.Rows[0]["MobileNumber"].ToString();
            TBFaxNumber.Text = DTRecords.Rows[0]["FaxNumber"].ToString();
            TBEmail.Text = DTRecords.Rows[0]["Email"].ToString();
            TBWebsite.Text = DTRecords.Rows[0]["Website"].ToString();
            btnSubmit.Text = "Update";
        }
    }

    private void DeleteEmployee(int recordId)
    {
        string SelQuery = "SELECT Address FROM Employee WHERE ID='" + recordId + "'";
        DataSet DSAddress = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
        if (DSAddress.Tables[0].Rows.Count > 0)
        {
            string SelContactQuery = "SELECT ContactID FROM Address WHERE ID='" + Convert.ToInt32(DSAddress.Tables[0].Rows[0]["Address"]) + "'";
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
        int DelCompany = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Employee_CRUD", arrParam2);
    }

    protected void LBExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string fileName = "Employee_Report" + System.DateTime.Now + ".xls";
            string Extension = ".xls";
            if (Extension == ".xls")
            {
                for (int i = 0; i < GVEmployee.Columns.Count; i++)
                {
                    GVEmployee.Columns[i].Visible = true;
                }

                BindGVEmployees();

                clsConnectionSql.PrepareControlForExport(GVEmployee);
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
                            table.GridLines = GVEmployee.GridLines;
                            //add the header row to the table
                            if (GVEmployee.HeaderRow != null)
                            {
                                clsConnectionSql.PrepareControlForExport(GVEmployee.HeaderRow);
                                table.Rows.Add(GVEmployee.HeaderRow);
                            }
                            //add each of the data rows to the table
                            foreach (GridViewRow row in GVEmployee.Rows)
                            {
                                clsConnectionSql.PrepareControlForExport(row);
                                table.Rows.Add(row);
                            }

                            //add the footer row to the table
                            if (GVEmployee.FooterRow != null)
                            {
                                clsConnectionSql.PrepareControlForExport(GVEmployee.FooterRow);
                                table.Rows.Add(GVEmployee.FooterRow);
                            }
                            //render the table into the htmlwriter
                            GVEmployee.GridLines = GridLines.Both;
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
            MVEmployee.ActiveViewIndex = 0;
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
                        FillEmployee(dt_sheet);
                        break;
                }
                i++;
            }
        }
    }

    private void FillEmployee(DataTable dt_sheet)
    {
        int ContactID = 0, AddressID = 0;
        if (dt_sheet.Rows.Count > 0)
        {
            for (int i = 0; i < dt_sheet.Rows.Count; i++)
            {
                // Insert Supplier Contact Details
                string InsertSuppContact = "INSERT INTO Contact (MobileNumber,TelephoneNumber,FaxNumber,UniquePhoneNumber,Email,Website,Status) VALUES ('" + dt_sheet.Rows[i]["MobileNumber"] + "','" + dt_sheet.Rows[i]["TelephoneNumber"] + "','" + dt_sheet.Rows[i]["FaxNumber"] + "','" + dt_sheet.Rows[i]["UniquePhoneNumber"] + "','" + dt_sheet.Rows[i]["Email"] + "','" + dt_sheet.Rows[i]["Website"] + "','" + true + "')";
                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.Text, InsertSuppContact);
                if (result > 0)
                {
                    // Get Last Insert Contact Details ID
                    string getLastContactID = "SELECT TOP(1) ID FROM Contact ORDER BY ID DESC";
                    DataTable DTLastID = clsConnectionSql.filldt(getLastContactID);
                    ContactID = Convert.ToInt32(DTLastID.Rows[0][0]);
                }
                else
                {
                    ShowMessage("Error in Contact Uploads !", MessageType.Error);
                    break;
                }

                // Insert Supplier AddressDetails
                string InsertSuppAddress = "INSERT INTO Address(Address1,Address2,City,State,Country,ZipCode,ContactID,Status) VALUES ('" + dt_sheet.Rows[i]["Address1"] + "','" + dt_sheet.Rows[i]["Address2"] + "','" + dt_sheet.Rows[i]["City"] + "','" + dt_sheet.Rows[i]["State"] + "','" + dt_sheet.Rows[i]["Country"] + "','" + dt_sheet.Rows[i]["ZipCode"] + "','" + ContactID + "','" + true + "')";
                int reslult = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.Text, InsertSuppAddress);

                if (reslult > 0)
                {
                    // Get Last Insert Address Details ID
                    string getLastAddressID = "SELECT TOP(1) ID FROM Address ORDER BY ID DESC";
                    DataTable DTLastID = clsConnectionSql.filldt(getLastAddressID);
                    AddressID = Convert.ToInt32(DTLastID.Rows[0][0]);
                }
                else
                {
                    ShowMessage("Address Not Upload Successfully !", MessageType.Error);
                    break;
                }

                // Insert Supplier Details

                int BL = 0, Dept = 0, Desi = 0, WorkShift = 0;

                // Get Business Location ID
                
                string Query = "SELECT * FROM BusinessLocation WHERE BusinessLocationName='" + dt_sheet.Rows[i]["BusinessLocation"] + "' ";
                DataTable DTBL = clsConnectionSql.filldt(Query);
                if (DTBL.Rows.Count > 0)
                {
                    BL = Convert.ToInt32(DTBL.Rows[0][0]);
                }

                // Get Department ID

                string Query2 = "SELECT * FROM EmployeeDepartment WHERE Name='" + dt_sheet.Rows[i]["Department"] + "' ";
                DataTable DTDept = clsConnectionSql.filldt(Query2);
                if (DTDept.Rows.Count > 0)
                {
                    Dept = Convert.ToInt32(DTDept.Rows[0][0]);
                }

                // Get Designation ID

                string Query3 = "SELECT * FROM EmployeeDesignation WHERE Name='" + dt_sheet.Rows[i]["Designation"] + "' ";
                DataTable DTDesi = clsConnectionSql.filldt(Query2);
                if (DTDesi.Rows.Count > 0)
                {
                    Desi = Convert.ToInt32(DTDesi.Rows[0][0]);
                }

                // Get Work Shift ID

                string Query4 = "SELECT * FROM EmployeeWorkShift WHERE Name='" + dt_sheet.Rows[i]["WorkingShift"] + "' ";
                DataTable DTWShift = clsConnectionSql.filldt(Query2);
                if (DTWShift.Rows.Count > 0)
                {
                    WorkShift = Convert.ToInt32(DTWShift.Rows[0][0]);
                }

                string InsertSupplier = "INSERT INTO Employee (Image,EmployeeCode,FirstName,MiddleName,LastName,SearchCode,DateOfBirth,MartialStatus,Gender,DateOfJoining,BusinessLocation,Department,Designation,IsActive,NotAnEmployee,CanHaveAppointment,Address,WorkingShift,SalesCommissionPercent,CommissionQuickPosition,IsAllowSpotDiscountPercentLimit,SpotDiscountAmount,Status) VALUES ('" + dt_sheet.Rows[i]["Image"] + "','" + dt_sheet.Rows[i]["EmployeeCode"] + "','" + dt_sheet.Rows[i]["FirstName"] + "','" + dt_sheet.Rows[i]["MiddleName"] + "','" + dt_sheet.Rows[i]["LastName"] + "','" + dt_sheet.Rows[i]["SearchCode"] + "','" + Convert.ToDateTime(dt_sheet.Rows[i]["DateOfBirth"]) + "','" + dt_sheet.Rows[i]["MartialStatus"] + "','" + dt_sheet.Rows[i]["Gender"] + "','" + Convert.ToDateTime(dt_sheet.Rows[i]["DateOfJoining"]) + "','" + BL + "','" + Dept + "','" + Desi + "','" + dt_sheet.Rows[i]["IsActive"] + "','" + dt_sheet.Rows[i]["NotAnEmployee"] + "','" + dt_sheet.Rows[i]["CanHaveAppointment"] + "','" + AddressID + "','" + WorkShift + "','" + dt_sheet.Rows[i]["SalesCommissionPercent"] + "','" + dt_sheet.Rows[i]["CommissionQuickPosition"] + "','" + dt_sheet.Rows[i]["IsAllowSpotDiscountPercentLimit"] + "','" + dt_sheet.Rows[i]["SpotDiscountAmount"] + "','" + true + "')";
                int InsResult = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.Text, InsertSupplier);
                if (InsResult > 0)
                {
                    ShowMessage("Supplier Uploding Successfully Completed. ", MessageType.Success);
                }
                else
                {
                    ShowMessage("Error In Supplier Uploading... ", MessageType.Error);
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
        MVEmployee.ActiveViewIndex = 2;
    }

    protected void LBFile_Click(object sender, EventArgs e)
    {
        string filename = "~/Uploader/UploadEmployee.xlsx";
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