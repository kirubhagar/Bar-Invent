using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

public partial class BinLocationMaster : System.Web.UI.Page
{
    #region Common Fields

    DataSet DSRecords = new DataSet();
    DataTable DTRecords = new DataTable();

    #endregion

    #region ALERT Methods For Alert Messages Section

    public enum MessageType { Success, Error, Info, Warning };

    protected void ShowMessage(string Message, MessageType type)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
    }

    #endregion

    #region Page Load Section

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Load Bin Location DDL
            BindDDLWarehouse();

            // Bind All GridView
            BindGVGodown();
            BindGVZone();
            BindGVArea();

            // Bind All DDL
            BindDDLGodownZone();
        }
    }

    private void BindDDLWarehouse()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByDefault");
        DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
        DTRecords = DSRecords.Tables[0];

        DDLGodown.DataSource = DTRecords;
        DDLGodown.DataTextField = "Name";
        DDLGodown.DataValueField = "ID";
        DDLGodown.DataBind();
        DDLGodown.Items.Insert(0, new ListItem("SELECT", "0"));
    }

    protected void DDLWarehouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (DDLGodown.SelectedIndex > 0)
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByGodownID");
                arrParam[1] = new SqlParameter("@GodownID", DDLGodown.SelectedValue);
                DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_ZoneMaster_CRUD", arrParam);
                DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    DDLZone.DataSource = DTRecords;
                    DDLZone.DataTextField = "ZoneTitle";
                    DDLZone.DataValueField = "ID";
                    DDLZone.DataBind();
                    DDLZone.Items.Insert(0, new ListItem("SELECT", "0"));
                }
            }
            else
            {
                ShowMessage("Select Warehouse First!", MessageType.Error);
            }
            GenerateBinLocation();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void DDLZone_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (DDLZone.SelectedIndex > 0)
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByZoneID");
                arrParam[1] = new SqlParameter("@ZoneID", DDLZone.SelectedValue);
                DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam);
                DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    DDLArea.DataSource = DTRecords;
                    DDLArea.DataTextField = "AreaTitle";
                    DDLArea.DataValueField = "ID";
                    DDLArea.DataBind();
                    DDLArea.Items.Insert(0, new ListItem("SELECT", "0"));
                }
            }
            else
            {
                ShowMessage("Select Godown First!", MessageType.Error);
            }
            GenerateBinLocation();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void DDLArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (DDLArea.SelectedIndex > 0)
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", DDLArea.SelectedValue);
                DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam);
                DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    List<Int32> RowList = Enumerable.Range(1, Convert.ToInt32(DTRecords.Rows[0]["NoOfRow"])).ToList();
                    DDLRow.Items.Clear();
                    //DDLRow.Items.Add()
                    DDLRow.DataSource = RowList;
                    DDLRow.DataBind();
                    DDLRow.Items.Insert(0, new ListItem("SELECT", "0"));

                    List<Int32> ShelfList = Enumerable.Range(1, Convert.ToInt32(DTRecords.Rows[0]["NoOfShelf"])).ToList();
                    DDLShelf.Items.Clear();
                    //DDLShelf.Items.Add(li);
                    DDLShelf.DataSource = ShelfList;
                    DDLShelf.DataBind();
                    DDLShelf.Items.Insert(0, new ListItem("SELECT", "0"));
                }
            }
            else
            {
                ShowMessage("Select Godown First!", MessageType.Error);
            }
            GenerateBinLocation();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void BindDDLGodownZone()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByDefault");
        DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
        DTRecords = DSRecords.Tables[0];

        DDLGodownZone.DataSource = DTRecords;
        DDLGodownZone.DataTextField = "Name";
        DDLGodownZone.DataValueField = "ID";
        DDLGodownZone.DataBind();
        DDLGodownZone.Items.Insert(0, new ListItem("SELECT", "0"));


        DDLGodownArea.DataSource = DTRecords;
        DDLGodownArea.DataTextField = "Name";
        DDLGodownArea.DataValueField = "ID";
        DDLGodownArea.DataBind();
        DDLGodownArea.Items.Insert(0, new ListItem("SELECT", "0"));
    }

    protected void DDLWarehouseArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (DDLGodownArea.SelectedIndex > 0)
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByGodownID");
                arrParam[1] = new SqlParameter("@GodownID", DDLGodownArea.SelectedValue);
                DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_ZoneMaster_CRUD", arrParam);
                DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    DDLZoneArea.DataSource = DTRecords;
                    DDLZoneArea.DataTextField = "ZoneTitle";
                    DDLZoneArea.DataValueField = "ID";
                    DDLZoneArea.DataBind();
                    DDLZoneArea.Items.Insert(0, new ListItem("SELECT", "0"));
                }
            }
            else
            {
                ShowMessage("Select Warehouse First!", MessageType.Error);
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void BindGVZone()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "ZONE_SELECT_BY_VIEW");
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_ZoneMaster_CRUD", arrParam);
        GVZone.DataSource = ds.Tables[0];
        GVZone.DataBind();

        foreach (GridViewRow Row in GVZone.Rows)
        {
            Label lblStatus = (Label)Row.FindControl("lblStatus");
            LinkButton LBView = (LinkButton)Row.FindControl("LBView");
            LinkButton LBEdit = (LinkButton)Row.FindControl("LBEdit");
            LinkButton LBDelete = (LinkButton)Row.FindControl("LBDelete");
            if (lblStatus.Text == "Active")
            {
                lblStatus.CssClass = "label label-success";
                LBView.Enabled = true;
                LBEdit.Enabled = true;
                LBDelete.Enabled = true;
            }
            else
            {
                lblStatus.CssClass = "label label-danger";
                LBView.Enabled = false;
                LBEdit.Enabled = false;
                LBDelete.Enabled = false;
            }
        }
    }

    private void BindGVArea()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "AREA_SELECT_BY_VIEW");
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam);
        GVArea.DataSource = ds.Tables[0];
        GVArea.DataBind();

        foreach (GridViewRow Row in GVArea.Rows)
        {
            Label lblStatus = (Label)Row.FindControl("lblStatus");
            LinkButton LBView = (LinkButton)Row.FindControl("LBView");
            LinkButton LBEdit = (LinkButton)Row.FindControl("LBEdit");
            LinkButton LBDelete = (LinkButton)Row.FindControl("LBDelete");
            if (lblStatus.Text == "Active")
            {
                lblStatus.CssClass = "label label-success";
                LBView.Enabled = true;
                LBEdit.Enabled = true;
                LBDelete.Enabled = true;
            }
            else
            {
                lblStatus.CssClass = "label label-danger";
                LBView.Enabled = false;
                LBEdit.Enabled = false;
                LBDelete.Enabled = false;
            }
        }
    }

    #endregion

    #region Warehouse Section Events & Methods

    #region Gridview Events & Methods

    // Method for Bind GridView Warehouse
    private void BindGVGodown()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByDefault");
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
        GVGodown.DataSource = ds.Tables[0];
        GVGodown.DataBind();

        foreach (GridViewRow Row in GVGodown.Rows)
        {
            Label lblStatus = (Label)Row.FindControl("lblStatus");
            if (lblStatus.Text == "Active")
                lblStatus.CssClass = "label label-success";
            else
                lblStatus.CssClass = "label label-danger";
        }
    }

    protected void GVGodown_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVGodown.PageIndex = e.NewPageIndex;
        BindGVGodown();
        GVGodown.DataBind();
    }

    #endregion

    #endregion

    #region Zone Section Events & Methods

    // Method For All Button for Zone Section
    protected void SubmitMethodZone(object sender, EventArgs e)
    {
        if (sender == btnSubmitZone)
        {
            if (btnSubmitZone.Text == "Submit")
            {
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@Action", "INSERT");
                arrParam[1] = new SqlParameter("@GodownID", DDLGodownZone.SelectedValue);
                arrParam[2] = new SqlParameter("@ZoneTitle", TBZoneTitle.Text);
                arrParam[3] = new SqlParameter("@ShortName", TBZoneShortForm.Text);
                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_ZoneMaster_CRUD", arrParam);
                if (result > 0)
                    ShowMessage("Added Suceess", MessageType.Success);
                else
                    ShowMessage("Some Technical Error Occur!!!", MessageType.Error);
            }

            if (btnSubmitZone.Text == "Update")
            {
                SqlParameter[] arrParam = new SqlParameter[5];
                arrParam[0] = new SqlParameter("@Action", "UPDATE");
                arrParam[1] = new SqlParameter("@GodownID", DDLGodownZone.SelectedValue);
                arrParam[2] = new SqlParameter("@ZoneTitle", TBZoneTitle.Text);
                arrParam[3] = new SqlParameter("@ShortName", TBZoneShortForm.Text);
                arrParam[4] = new SqlParameter("@ID", Session["RecordId"]);
                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_ZoneMaster_CRUD", arrParam);
                if (result > 0)
                    ShowMessage("Added Suceess", MessageType.Success);
                else
                    ShowMessage("Some Technical Error Occur!!!", MessageType.Error);
            }
            ClearFormZone();
            BindGVZone();
        }

        if (sender == btnCancelZone)
        {
            ClearFormZone();
        }
    }

    // Method For Clear Warhouse Form Data
    private void ClearFormZone()
    {
        DDLGodownZone.SelectedIndex = 0;
        TBZoneTitle.Text = string.Empty;
        TBZoneShortForm.Text = string.Empty;
        btnSubmitZone.Text = "Submit";
    }

    #endregion

    #region Area Section Events & Methods

    // Method For All Button for Area Section
    protected void SubmitMethodArea(object sender, EventArgs e)
    {
        if (sender == btnSubmitArea)
        {
            if (btnSubmitArea.Text == "Submit")
            {
                SqlParameter[] arrParam = new SqlParameter[6];
                arrParam[0] = new SqlParameter("@Action", "INSERT");
                arrParam[1] = new SqlParameter("@ZoneID", DDLZoneArea.SelectedValue);
                arrParam[2] = new SqlParameter("@AreaTitle", TBAreaTitle.Text);
                arrParam[3] = new SqlParameter("@ShortForm", TBAreaShortForm.Text);
                arrParam[4] = new SqlParameter("@NoOfRow", TBNoOfRows.Text);
                arrParam[5] = new SqlParameter("@NoOfShelf", TBNoOfShelfs.Text);
                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam);
                if (result > 0)
                    ShowMessage("Added Suceess", MessageType.Success);
                else
                    ShowMessage("Some Technical Error Occur!!!", MessageType.Error);
            }

            if (btnSubmitArea.Text == "Update")
            {
                SqlParameter[] arrParam = new SqlParameter[7];
                arrParam[0] = new SqlParameter("@Action", "UPDATE");
                arrParam[1] = new SqlParameter("@ZoneID", DDLZoneArea.SelectedValue);
                arrParam[2] = new SqlParameter("@AreaTitle", TBAreaTitle.Text);
                arrParam[3] = new SqlParameter("@ShortForm", TBAreaShortForm.Text);
                arrParam[4] = new SqlParameter("@NoOfRow", TBNoOfRows.Text);
                arrParam[5] = new SqlParameter("@NoOfShelf", TBNoOfShelfs.Text);
                arrParam[6] = new SqlParameter("@ID", Session["RecordId"]);
                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam);
                if (result > 0)
                    ShowMessage("Added Suceess", MessageType.Success);
                else
                    ShowMessage("Some Technical Error Occur!!!", MessageType.Error);
            }
            ClearFormArea();
            BindGVArea();
        }

        if (sender == btnCancelArea)
        {
            ClearFormArea();
        }
    }

    // Method For Clear Area Form Data
    private void ClearFormArea()
    {
        DDLGodownArea.SelectedIndex = 0;
        DDLZoneArea.SelectedIndex = 0;
        TBAreaTitle.Text = string.Empty;
        TBAreaShortForm.Text = string.Empty;
        TBNoOfRows.Text = string.Empty;
        TBNoOfShelfs.Text = string.Empty;
        btnSubmitArea.Text = "Submit";
    }

    #endregion

    // Method For Link Button Selection
    protected void LBSelection(object sender, EventArgs e)
    {
        if (sender == LBBinLocation)
            MVWarehouse.ActiveViewIndex = 0;
        else if (sender == LBWarehouse)
            MVWarehouse.ActiveViewIndex = 1;
        else if (sender == LBZone)
            MVWarehouse.ActiveViewIndex = 2;
        else if (sender == LBArea)
            MVWarehouse.ActiveViewIndex = 3;
        else
            MVWarehouse.ActiveViewIndex = 0;
    }

    // Method For Generate Bin location 
    private void GenerateBinLocation()
    {
        string Warehouse, Zone, Area, Row, Shelf;

        // Select Warehouse Short Name
        SqlParameter[] arrParam = new SqlParameter[2];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
        arrParam[1] = new SqlParameter("@ID", DDLGodown.SelectedValue);
        DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
        DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
            Warehouse = DTRecords.Rows[0]["ShortForm"].ToString();
        else
            Warehouse = "_";


        // Select Zone Short Name
        SqlParameter[] arrParam2 = new SqlParameter[2];
        arrParam2[0] = new SqlParameter("@Action", "SELECT_ByID");
        arrParam2[1] = new SqlParameter("@ID", DDLZone.SelectedValue);
        DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_ZoneMaster_CRUD", arrParam2);
        DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
            Zone = DTRecords.Rows[0]["ShortName"].ToString();
        else
            Zone = "_";


        // Select Area Short Name
        SqlParameter[] arrParam3 = new SqlParameter[2];
        arrParam3[0] = new SqlParameter("@Action", "SELECT_ByID");
        arrParam3[1] = new SqlParameter("@ID", DDLArea.SelectedValue);
        DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam3);
        DTRecords = DSRecords.Tables[0];
        if (DTRecords.Rows.Count > 0)
            Area = DTRecords.Rows[0]["ShortForm"].ToString();
        else
            Area = "_";


        // Select Row No
        if (DDLRow.SelectedIndex > 0)
            Row = DDLRow.SelectedValue.ToString();
        else
            Row = "_";

        // Select Shelf No
        if (DDLShelf.SelectedIndex > 0)
            Shelf = DDLShelf.SelectedValue.ToString();
        else
            Shelf = "_";

        // Generate Bin Location 

        TBBinLocation.Text = "#" + Warehouse + "." + Zone + "." + Area + "." + Row + "." + Shelf;

    }

    // Method For DDLRow Select Index Changed
    protected void DDLRow_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateBinLocation();
    }

    // Method For DDLShelf Select Index Changed
    protected void DDLShelf_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateBinLocation();
    }

    // Method For GridView Zone Row Command
    protected void GVZone_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int recordId = Convert.ToInt32(e.CommandArgument);
            Session["RecordId"] = recordId;

            #region For View Record
            if (e.CommandName == "ViewRecord")
            {
            }
            #endregion

            #region For Edit Record
            if (e.CommandName == "EditRecord")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_ZoneMaster_CRUD", arrParam);
                DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    TBZoneTitle.Text = DTRecords.Rows[0]["ZoneTitle"].ToString();
                    TBZoneShortForm.Text = DTRecords.Rows[0]["ShortName"].ToString();
                    DDLGodownZone.SelectedValue = DTRecords.Rows[0]["GodownID"].ToString();
                    btnSubmitZone.Text = "Update";
                }
            }
            #endregion

            #region For Delete Record
            if (e.CommandName == "DeleteRecord")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "DELETE");
                arrParam[1] = new SqlParameter("@ID", recordId);
                int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_ZoneMaster_CRUD", arrParam);
            }
            #endregion

            #region For Change Status
            if (e.CommandName == "ChangeStatus")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_ZoneMaster_CRUD", arrParam);
                DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    #region Change Status True To False
                    if (DTRecords.Rows[0]["Status"].ToString() == "True")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", false);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_ZoneMaster_CRUD", arrParam2);
                    }
                    #endregion

                    #region Change Status False TO True
                    if (DTRecords.Rows[0]["Status"].ToString() == "False")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", true);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_ZoneMaster_CRUD", arrParam2);
                    }
                    #endregion
                }
            }
            #endregion

            BindGVZone();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method for Paging in GridView Warehouse
    protected void GVZone_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVZone.PageIndex = e.NewPageIndex;
        BindGVZone();
        GVZone.DataBind();
    }

    // Method For GridView Area Row Command
    protected void GVArea_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int recordId = Convert.ToInt32(e.CommandArgument);
            Session["RecordId"] = recordId;

            #region For View Record
            if (e.CommandName == "ViewRecord")
            {
            }
            #endregion

            #region For Edit Record
            if (e.CommandName == "EditRecord")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords2 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam);
                DataTable DTRecords2 = DSRecords2.Tables[0];
                if (DTRecords2.Rows.Count > 0)
                {
                    BindDDLGodownZone();
                    SqlParameter[] arrParam2 = new SqlParameter[2];
                    arrParam2[0] = new SqlParameter("@Action", "SELECT_ByView");
                    arrParam2[1] = new SqlParameter("@ID", Convert.ToInt32(DTRecords2.Rows[0]["ZoneId"]));
                    DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam2);
                    DDLGodownArea.SelectedValue = DSRecords.Tables[0].Rows[0]["GodownID"].ToString();
                    DDLWarehouseArea_SelectedIndexChanged(sender, e);
                    DDLZoneArea.SelectedValue = DTRecords2.Rows[0]["ZoneId"].ToString();
                    TBAreaTitle.Text = DTRecords2.Rows[0]["AreaTitle"].ToString();
                    TBAreaShortForm.Text = DTRecords2.Rows[0]["ShortForm"].ToString();
                    TBNoOfRows.Text = DTRecords2.Rows[0]["NoOfRow"].ToString();
                    TBNoOfShelfs.Text = DTRecords2.Rows[0]["NoOfShelf"].ToString();
                    btnSubmitArea.Text = "Update";
                }
            }
            #endregion

            #region For Delete Record
            if (e.CommandName == "DeleteRecord")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "DELETE");
                arrParam[1] = new SqlParameter("@ID", recordId);
                int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam);
            }
            #endregion

            #region For Change Status
            if (e.CommandName == "ChangeStatus")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam);
                DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    #region Change Status True To False
                    if (DTRecords.Rows[0]["Status"].ToString() == "True")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", false);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam2);
                    }
                    #endregion

                    #region Change Status False TO True
                    if (DTRecords.Rows[0]["Status"].ToString() == "False")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", true);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_Bin_AreaMaster_CRUD", arrParam2);
                    }
                    #endregion
                }
            }
            #endregion

            BindGVArea();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method for Paging in GridView Warehouse
    protected void GVArea_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVArea.PageIndex = e.NewPageIndex;
        BindGVArea();
        GVArea.DataBind();
    }

}