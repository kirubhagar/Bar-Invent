using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

public partial class ItemManager : System.Web.UI.Page
{
    #region Dynamic DataTable Section

    // Basic Data
    DataTable TempDT = new DataTable();
    string[] ItemArray = new string[10];

    // Create / Load DataTable
    private void CreateDTTable()
    {
        TempDT = new DataTable();
        if (TempDT.Columns.Count == 0)
        {
            TempDT.Columns.Add("ID", typeof(int));
            TempDT.Columns.Add("Image", typeof(string));
            TempDT.Columns.Add("Name", typeof(string));
            TempDT.Columns.Add("PCode", typeof(string));
            TempDT.Columns.Add("HSNCode", typeof(string));
            TempDT.Columns.Add("BCode", typeof(string));
            TempDT.Columns.Add("OS", typeof(string));
            TempDT.Columns.Add("OSDate", typeof(string));
            TempDT.Columns.Add("RQty", typeof(string));
            TempDT.Columns.Add("Bin", typeof(string));
        }
        ViewState["DTTable"] = TempDT;
    }

    // For Insert Into DataTable
    private void InsertDTTable(string[] ItemArray)
    {
        TempDT = ViewState["DTTable"] as DataTable;
        TempDT.Rows.Add(ItemArray[0], ItemArray[1], ItemArray[2], ItemArray[3], ItemArray[4], ItemArray[5], ItemArray[6], ItemArray[7], ItemArray[8], ItemArray[9]);
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
                ItemArray[0] = dr["ID"].ToString();
                ItemArray[1] = dr["Image"].ToString();
                ItemArray[2] = dr["Name"].ToString();
                ItemArray[3] = dr["PCode"].ToString();
                ItemArray[4] = dr["HSNCode"].ToString();
                ItemArray[5] = dr["BCode"].ToString();
                ItemArray[6] = dr["OS"].ToString();
                ItemArray[7] = dr["OSDate"].ToString();
                ItemArray[8] = dr["RQty"].ToString();
                ItemArray[9] = dr["Bin"].ToString();
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
            if ((Int32)dr[0] == Convert.ToInt32(RecordID))
            {
                dr["ID"] = ItemArray[0].ToString();
                dr["Image"] = ItemArray[1].ToString();
                dr["Name"] = ItemArray[2].ToString();
                dr["PCode"] = ItemArray[3].ToString();
                dr["HSNCode"] = ItemArray[4].ToString();
                dr["BCode"] = ItemArray[5].ToString();
                dr["OS"] = ItemArray[6].ToString();
                dr["OSDate"] = ItemArray[7].ToString();
                dr["RQty"] = ItemArray[8].ToString();
                dr["Bin"] = ItemArray[9].ToString();
            }
        }
        ViewState["DTTable"] = TempDT;

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
            BindCom();   
        }
    }

    private void BindCom()
    {
        try
        {
            SqlParameter[] arrparm = new SqlParameter[1];
            arrparm[0] = new SqlParameter("@Action", "SELECT");
            DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrparm);
            if (DSRecord.Tables[0].Rows.Count > 0)
            {
                DDLCompany.DataSource = DSRecord.Tables[0];
                DDLCompany.DataTextField = "CompanyName";
                DDLCompany.DataValueField = "ID";
                DDLCompany.DataBind();
                DDLCompany.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }

    }
   
    protected void DDLCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDDLNGO(DDLCompany.SelectedValue);
        BindDDLGodown(DDLCompany.SelectedValue);
    }

    private void BindDDLNGO(string ComID)
    {
        try
        {
            SqlParameter[] arrparm = new SqlParameter[2];
            arrparm[0] = new SqlParameter("@Action", "SELECT_ByComID");
            arrparm[1] = new SqlParameter("@CompanyID", ComID);
            DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrparm);
            if (DSRecord.Tables[0].Rows.Count > 0)
            {
                DDLNGO.DataSource = DSRecord.Tables[0];
                DDLNGO.DataTextField = "BusinessLocationName";
                DDLNGO.DataValueField = "ID";
                DDLNGO.DataBind();
                DDLNGO.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void BindDDLGodown(string ComID)
    {
        try
        {
            SqlParameter[] arrparm = new SqlParameter[2];
            arrparm[0] = new SqlParameter("@Action", "SELECT_ByComID");
            arrparm[1] = new SqlParameter("@ComID", ComID);
            DataSet DSRecord = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrparm);
            if (DSRecord.Tables[0].Rows.Count > 0)
            {
                DDLGodown.DataSource = DSRecord.Tables[0];
                DDLGodown.DataTextField = "Name";
                DDLGodown.DataValueField = "ID";
                DDLGodown.DataBind();
                DDLGodown.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    DataTable DTStockItem = new DataTable();

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        CreateDTTable();
        DataSet DSItemStock = new DataSet();

        switch (DDLItemType.SelectedItem.Text)
        {
            case "Raw Material":
                SqlParameter[] ArrParm = new SqlParameter[3];
                ArrParm[0] = new SqlParameter("@Action", "SELECT_ItemStock");
                ArrParm[1] = new SqlParameter("@GodownID", DDLGodown.SelectedValue);
                ArrParm[2] = new SqlParameter("@ItemType", "Raw Material");
                DSItemStock = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemStock_CRUD", ArrParm);
                break;
            case "Kit Material":
                SqlParameter[] ArrParm1 = new SqlParameter[3];
                ArrParm1[0] = new SqlParameter("@Action", "SELECT_ItemStock");
                ArrParm1[1] = new SqlParameter("@GodownID", DDLGodown.SelectedValue);
                ArrParm1[2] = new SqlParameter("@ItemType", "Kit Material");
                DSItemStock = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemStock_CRUD", ArrParm1);
                break;
            case "Finish Material":
                SqlParameter[] ArrParm2 = new SqlParameter[3];
                ArrParm2[0] = new SqlParameter("@Action", "SELECT_ItemStock");
                ArrParm2[1] = new SqlParameter("@GodownID", DDLGodown.SelectedValue);
                ArrParm2[2] = new SqlParameter("@ItemType", "Finish Material");
                DSItemStock = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemStock_CRUD", ArrParm2);
                break;
            default:
                break;
        }

        
        DataTable DTItemStock = DSItemStock.Tables[0];

        if (DTItemStock.Rows.Count > 0)
        {
            for (int i = 0; i < DTItemStock.Rows.Count; i++)
            {
                TempDT.Rows.Add(DTItemStock.Rows[i]["ID"],
                                     DTItemStock.Rows[i]["Image"],
                                     DTItemStock.Rows[i]["Name"],
                                     DTItemStock.Rows[i]["ProductCode"],
                                     DTItemStock.Rows[i]["HSNCode"],
                                     DTItemStock.Rows[i]["Barcode"],
                                     DTItemStock.Rows[i]["OpeningStock"],
                                     DTItemStock.Rows[i]["OSDate"],
                                     DTItemStock.Rows[i]["ReorderQty"],
                                     DTItemStock.Rows[i]["BinLocation"]);
            }
            ViewState["DTTable"] = TempDT;
            GVItem.DataSource = TempDT;
            GVItem.DataBind();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow GVR in GVItem.Rows)
        {
            HiddenField hf = (HiddenField)GVR.Cells[0].FindControl("HFID");
            TextBox OpeningStock = (TextBox)GVR.Cells[6].FindControl("TBOpeningStock");
            TextBox OSDate = (TextBox)GVR.Cells[7].FindControl("TBOSDate");
            TextBox ReorderQty = (TextBox)GVR.Cells[8].FindControl("TBReorderQty");
            TextBox BinLocation = (TextBox)GVR.Cells[9].FindControl("TBBinLocation");
            CheckBox CBChoice = (CheckBox)GVR.Cells[0].FindControl("chkRow");

            if (CBChoice.Checked)
            {

                // Select Existing Data
                SqlParameter[] arrparm = new SqlParameter[4];
                arrparm[0] = new SqlParameter("@Action", "SELECT_ByItem");
                arrparm[1] = new SqlParameter("@ItemType", DDLItemType.SelectedItem.Text);
                arrparm[2] = new SqlParameter("@ItemID", hf.Value);
                arrparm[3] = new SqlParameter("@GodownID", DDLGodown.SelectedValue);
                DataSet DSItem = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemStock_CRUD", arrparm);
                if (DSItem.Tables[0].Rows.Count > 0)
                {
                    SqlParameter[] arrparm1 = new SqlParameter[8];
                    arrparm1[0] = new SqlParameter("@Action", "UPDATE_Items");
                    arrparm1[1] = new SqlParameter("@OpeningStock", Convert.ToDecimal(OpeningStock.Text));
                    arrparm1[2] = new SqlParameter("@OSDate", OSDate.Text);
                    arrparm1[3] = new SqlParameter("@ReorderQty", Convert.ToDecimal(ReorderQty.Text));
                    arrparm1[4] = new SqlParameter("@BinLocation", BinLocation.Text);
                    arrparm1[5] = new SqlParameter("@ItemType", DDLItemType.SelectedItem.Text);
                    arrparm1[6] = new SqlParameter("@ItemID", hf.Value);
                    arrparm1[7] = new SqlParameter("@GodownID", DDLGodown.SelectedValue);
                    int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemStock_CRUD", arrparm1);
                }
                else
                {
                    SqlParameter[] arrparm1 = new SqlParameter[8];
                    arrparm1[0] = new SqlParameter("@Action", "INSERT");
                    arrparm1[1] = new SqlParameter("@GodownID", DDLGodown.SelectedValue);
                    arrparm1[2] = new SqlParameter("@ItemType", DDLItemType.SelectedItem.Text);
                    arrparm1[3] = new SqlParameter("@ItemID", hf.Value);
                    arrparm1[4] = new SqlParameter("@OpeningStock", Convert.ToDecimal(OpeningStock.Text));
                    arrparm1[5] = new SqlParameter("@OSDate", OSDate.Text);
                    arrparm1[6] = new SqlParameter("@ReorderQty", Convert.ToDecimal(ReorderQty.Text));
                    arrparm1[7] = new SqlParameter("@BinLocation", BinLocation.Text);
                    int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemStock_CRUD", arrparm1);
                }
            }
        }
    }

    protected void chkHeader_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkHeader = (CheckBox)GVItem.HeaderRow.FindControl("chkHeader");
        if (chkHeader.Checked)
        {
            foreach (GridViewRow gvrow in GVItem.Rows)
            {
                CheckBox chkRow = (CheckBox)gvrow.FindControl("chkRow");
                chkRow.Checked = true;
            }
        }
        else
        {
            foreach (GridViewRow gvrow in GVItem.Rows)
            {
                CheckBox chkRow = (CheckBox)gvrow.FindControl("chkRow");
                chkRow.Checked = false;
            }
        }

    }

    protected void chkRow_CheckedChanged(object sender, EventArgs e)
    {
        int count = 0;
        int totalRowCountGrid = GVItem.Rows.Count;

        CheckBox chkHeader = (CheckBox)GVItem.HeaderRow.FindControl("chkHeader");

        foreach (GridViewRow gvrow in GVItem.Rows)
        {
            CheckBox chkRow = (CheckBox)gvrow.FindControl("chkRow");
            if (chkRow.Checked)
            {
                count++;
            }
        }

        if (count == totalRowCountGrid)
        {
            chkHeader.Checked = true;
        }
        else
        {
            chkHeader.Checked = false;
        }
    }


    #region Bin Location Section

    DataSet DSRecords = new DataSet();
    DataTable DTRecords = new DataTable();

    private void BindDDLWarehouse()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByDefault");
        DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
        DTRecords = DSRecords.Tables[0];
        DDLWarehouse.DataSource = DTRecords;
        DDLWarehouse.DataTextField = "Name";
        DDLWarehouse.DataValueField = "ID";
        DDLWarehouse.DataBind();
        DDLWarehouse.Items.Insert(0, new ListItem("SELECT", "0"));
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

        TBModelBinLocation.Text = "#" + Warehouse + "." + Zone + "." + Area + "." + Row + "." + Shelf;
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

    #endregion

    protected void ModelSubmitBtnClickEvent(object sender, EventArgs e)
    {
        if (sender == ModelBinSubmit)
        {
            string str = "SELECT * FROM RawMaterial WHERE BinLocation='" + TBModelBinLocation.Text + "'";
            DataSet dsBin = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, str);
            if (dsBin.Tables[0].Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Bin Is not Empty Sorry!')", true);
            }
            else
            {
                foreach (GridViewRow gvrow in GVItem.Rows)
                {
                    HiddenField hf = (HiddenField)gvrow.FindControl("HFID");
                    if (hf.Value == Session["Index"].ToString())
                    {
                        string[] StrArray = new string[11];
                        StrArray = SelectDTTable(Convert.ToInt32(Session["Index"]));
                        StrArray[9] = TBModelBinLocation.Text;

                        UpdateDTTable(Convert.ToInt32(Session["Index"]), StrArray);
                    }
                }
                BindInnerGV();
            }
        }
    }

    public void BindInnerGV()
    {
        GVItem.DataSource = ViewState["DTTable"] as DataTable;
        GVItem.DataBind();
    }

    protected void GVItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Session["Index"] = "";
        int RowIndex = Convert.ToInt32(e.CommandArgument);
        Session["Index"] = RowIndex;
        MPE2.Show();
        BindDDLWarehouse();
    }
}