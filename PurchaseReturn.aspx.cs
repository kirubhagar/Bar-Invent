using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Text;
//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;

public partial class PurchaseReturn : System.Web.UI.Page
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

    #region Dyanimic DataTable Section

    // Basi Data
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
            TempDT.Columns.Add("Image", typeof(string));
            TempDT.Columns.Add("Barcode", typeof(string));
            TempDT.Columns.Add("ItemName", typeof(string));
            TempDT.Columns.Add("ReturnQty", typeof(string));
            TempDT.Columns.Add("AvStock", typeof(string));
            TempDT.Columns.Add("MeasuringUnit", typeof(string));
            TempDT.Columns.Add("UnitPrice", typeof(string));
            TempDT.Columns.Add("SubTotal", typeof(string));
            TempDT.Columns.Add("CGSTPer", typeof(string));
            TempDT.Columns.Add("CGSTValue", typeof(string));
            TempDT.Columns.Add("SGSTPer", typeof(string));
            TempDT.Columns.Add("SGSTValue", typeof(string));
            TempDT.Columns.Add("UnitPriceATax", typeof(string));
            TempDT.Columns.Add("SubTotalATax", typeof(string));
        }
        ViewState["DTTable"] = TempDT;
    }

    // For Insert Into DataTable
    private void InsertDTTable(string[] ItemArray)
    {
        TempDT = ViewState["DTTable"] as DataTable;

        TempDT.Rows.Add(null, ItemArray[1], ItemArray[2], ItemArray[3], ItemArray[4],
                        ItemArray[5], ItemArray[6], ItemArray[7], ItemArray[8], ItemArray[9],
                        ItemArray[10], ItemArray[11], ItemArray[12], ItemArray[13], ItemArray[14]);

        ViewState["DTTable"] = TempDT;
    }

    // For Select From DataTable Via ID
    private string[] SelectDTTable(int ID)
    {
        TempDT = ViewState["DTTable"] as DataTable;
        for (int i = TempDT.Rows.Count - 1; i > 0; i++)
        {
            DataRow dr = TempDT.Rows[i];
            if ((Int32)dr[0] == ID)
            {
                ItemArray[1] = dr["Image"].ToString();
                ItemArray[2] = dr["Barcode"].ToString();
                ItemArray[3] = dr["ItemName"].ToString();
                ItemArray[4] = dr["ReturnQty"].ToString();
                ItemArray[5] = dr["AvStock"].ToString();
                ItemArray[6] = dr["MeasuringUnit"].ToString();
                ItemArray[7] = dr["UnitPrice"].ToString();
                ItemArray[8] = dr["SubTotal"].ToString();
                ItemArray[9] = dr["CGSTPer"].ToString();
                ItemArray[10] = dr["CGSTValue"].ToString();
                ItemArray[11] = dr["SGSTPer"].ToString();
                ItemArray[12] = dr["SGSTValue"].ToString();
                ItemArray[13] = dr["UnitPriceATax"].ToString();
                ItemArray[14] = dr["SubTotalATax"].ToString();
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
                dr["Image"] = ItemArray[1].ToString();
                dr["Barcode"] = ItemArray[2].ToString();
                dr["ItemName"] = ItemArray[3].ToString();
                dr["ReturnQty"] = ItemArray[4].ToString();
                dr["AvStock"] = ItemArray[5].ToString();
                dr["MeasuringUnit"] = ItemArray[6].ToString();
                dr["UnitPrice"] = ItemArray[7].ToString();
                dr["SubTotal"] = ItemArray[8].ToString();
                dr["CGSTPer"] = ItemArray[9].ToString();
                dr["CGSTValue"] = ItemArray[10].ToString();
                dr["SGSTPer"] = ItemArray[11].ToString();
                dr["SGSTValue"] = ItemArray[12].ToString();
                dr["UnitPriceATax"] = ItemArray[13].ToString();
                dr["SubTotalATax"] = ItemArray[14].ToString();
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

    // For Get Total Details
    private string[] TotalDetails()
    {
        string[] DataArray = new string[3];
        double Qty = 0.0;
        double value = 0.0;
        double taxvalue = 0.0;
        TempDT = ViewState["DTTable"] as DataTable;
        for (int i = TempDT.Rows.Count - 1; i >= 0; i--)
        {
            Qty += Convert.ToDouble(TempDT.Rows[i][4]);
            value += Convert.ToDouble(TempDT.Rows[i][13]);
            taxvalue += Convert.ToDouble(TempDT.Rows[i][12]);
        }
        DataArray[0] = Qty.ToString();
        DataArray[1] = value.ToString();
        DataArray[2] = taxvalue.ToString();
        return DataArray;
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CreateDTTable();
            BindGVPurReturn();
        }
    }

    private void BindGVPurReturn()
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[1];
            arrParam[0] = new SqlParameter("@Action", "SELECT");
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_PurReturnMaster_CRUD", arrParam);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                GVPurchaseReturn.DataSource = DSRecords;
                GVPurchaseReturn.DataBind();
            }
            else
            {
                GVPurchaseReturn.DataBind();
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void LBAddNew_Click(object sender, EventArgs e)
    {
        MVPurchaseReturn.ActiveViewIndex = 1;
        BindBL();
        BindSupplier();
        LBLDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        DDLGodown.Enabled = false;
        DDLSupplier.Enabled = false;
        TBBarcode.Enabled = false;
        DDLItemName.Enabled = false;
        TBSearchCode.Enabled = false;
        LBAddAllItem.Enabled = false;
        CBAd.Checked = true;
        BindGVPurReturnInner();
        BindDDLRIE();
        BindCode();
    }

    private void BindGVPurReturnInner()
    {
        try
        {
            GVPurchaseReturnInner.DataSource = ViewState["DTTable"] as DataTable;
            GVPurchaseReturnInner.DataBind();
            InnerGVData();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void InnerGVData()
    {
        try
        {
            if (GVPurchaseReturnInner.Rows.Count > 0)
            {
                if (CBShowImage.Checked)
                    GVPurchaseReturnInner.Columns[1].Visible = true;
                else
                    GVPurchaseReturnInner.Columns[1].Visible = false;

                string[] array = new string[3];
                array = TotalDetails();
                LBLNoOfPackages.Text = array[0];
                TBSubTotal.Text = array[1];
                TBTotalTaxAmount.Text = array[2];
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void BindBL()
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[1];
            arrParam[0] = new SqlParameter("@Action", "SELECT");
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrParam);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                DDLBusinessLocation.DataSource = DSRecords.Tables[0];
                DDLBusinessLocation.DataTextField = "BusinessLocationName";
                DDLBusinessLocation.DataValueField = "ID";
                DDLBusinessLocation.DataBind();
                DDLBusinessLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select-", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void DDLBusinessLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LBLPRNumber.Text = BindCode(DDLBusinessLocation.SelectedValue);
        bindGodown();
        //BindDDLRIE();
    }

    private void BindDDLRIE()
    {
        try
        {
            DDLRecItemEntry.Items.Clear();
            string SelByBL = "SELECT * FROM Receive_Master";
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelByBL);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                DDLRecItemEntry.DataSource = DSRecords.Tables[0];
                DDLRecItemEntry.DataTextField = "RecNo";
                DDLRecItemEntry.DataValueField = "ID";
                DDLRecItemEntry.DataBind();
                DDLRecItemEntry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select-", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }
    
    private void BindCode()
    {
        try
        {
            string DocumentCode = "PR"; string PrvID = ""; int NextID = 1;
            string GetMaxId = "SELECT PRNo From PRMaster Order By ID DESC ";
            DataTable dtBill = clsConnectionSql.filldt(GetMaxId);
            if (dtBill.Rows.Count > 0)
            {
                PrvID = dtBill.Rows[0]["PRNo"].ToString();
                string[] PrvCon = PrvID.Split('R');
                NextID = Convert.ToInt32(PrvCon[1]) + 1;
                string Zero = new String('0', (5 - NextID.ToString().Length));
                DocumentCode = DocumentCode + Zero + NextID.ToString();
                LBLPRNumber.Text = DocumentCode.ToString();
            }
            else
                LBLPRNumber.Text = "PR00001";
        }
        catch (Exception ex)
        {
        }
    }

    protected void CBAllowRIENumber_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CBAllowRIENumber.Checked)
            {
                DDLRecItemEntry.ClearSelection();
                DDLRecItemEntry.Dispose();
                DDLRecItemEntry.Enabled = false;
                DDLGodown.ClearSelection();
                DDLGodown.Enabled = true;
                DDLSupplier.ClearSelection();
                DDLSupplier.Enabled = true;
                LBAddAllItem.Enabled = false;
                TBBarcode.Enabled = false;
                DDLItemName.Items.Clear();
                DDLItemName.Enabled = false;
                TBSearchCode.Enabled = false;
            }
            else
            {
                DDLRecItemEntry.Enabled = true;
                DDLGodown.ClearSelection();
                DDLGodown.Enabled = false;
                DDLSupplier.ClearSelection();
                DDLSupplier.Enabled = false;
                LBAddAllItem.Enabled = false;
                TBBarcode.Enabled = false;
                DDLItemName.Items.Clear();
                DDLItemName.Enabled = false;
                TBSearchCode.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void bindGodown()
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[1];
            arrParam[0] = new SqlParameter("@Action", "SELECT_ByDefault");
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                DDLGodown.DataSource = DSRecords.Tables[0];
                DDLGodown.DataTextField = "Name";
                DDLGodown.DataValueField = "ID";
                DDLGodown.DataBind();
                DDLGodown.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select-", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void BindSupplier()
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[1];
            arrParam[0] = new SqlParameter("@Action", "SELECT");
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Supplier_CRUD", arrParam);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                DDLSupplier.DataSource = DSRecords.Tables[0];
                DDLSupplier.DataTextField = "Name";
                DDLSupplier.DataValueField = "ID";
                DDLSupplier.DataBind();
                DDLSupplier.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select-", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void DDLRecItemEntry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            TBBarcode.Enabled = true;
            DDLItemName.Enabled = true;
            TBSearchCode.Enabled = true;
            LBAddAllItem.Enabled = true;

            string ss = "SELECT Supplier FROM Receive_Master WHERE ID='" + DDLRecItemEntry.SelectedValue + "'";
            DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, ss);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLSupplier.ClearSelection();
                DDLSupplier.Items.FindByValue(ds.Tables[0].Rows[0][0].ToString()).Selected = true;
            }

            string ss2 = "SELECT ID FROM Godown WHERE NGOID='" + DDLBusinessLocation.SelectedValue + "' AND ComID='" + Session["ComID"] + "'";
            DataSet ds2 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, ss2);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                DDLGodown.ClearSelection();
                DDLGodown.Items.FindByValue(ds2.Tables[0].Rows[0][0].ToString()).Selected = true;
            }

            string ss3 = "SELECT dbo.RawMaterial.ID,dbo.RawMaterial.Name FROM dbo.Receive_details INNER JOIN dbo.RawMaterial ON dbo.Receive_details.ItemID = dbo.RawMaterial.ID WHERE (dbo.Receive_details.rec_Id = '" + DDLRecItemEntry.SelectedValue + "')";
            DataSet ds3 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, ss3);
            if (ds3.Tables[0].Rows.Count > 0)
            {
                DDLItemName.Items.Clear();
                DDLItemName.DataSource = ds3.Tables[0];
                DDLItemName.DataTextField = "Name";
                DDLItemName.DataValueField = "ID";
                DDLItemName.DataBind();
                DDLItemName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select-", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void DDLGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            TBBarcode.Enabled = true;
            DDLItemName.Enabled = true;
            TBSearchCode.Enabled = true;

            string ss3 = "SELECT ID,Name FROM RawMaterial";
            DataSet ds3 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, ss3);
            if (ds3.Tables[0].Rows.Count > 0)
            {
                DDLItemName.Items.Clear();
                DDLItemName.DataSource = ds3.Tables[0];
                DDLItemName.DataTextField = "Name";
                DDLItemName.DataValueField = "ID";
                DDLItemName.DataBind();
                DDLItemName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select-", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void SelectItem(object sender, EventArgs e)
    {
        if (sender == DDLItemName && DDLItemName.SelectedIndex > 0)
        {
            string[] TempArray = new string[15];

            string SelQurey = "SELECT * FROM View_ItemDetails";
            if (sender == DDLItemName && DDLItemName.SelectedIndex > 0)
                SelQurey += " WHERE View_ItemDetails.ID='" + DDLItemName.SelectedValue + "'";
            else if (sender == TBBarcode && TBBarcode.Text != string.Empty)
                SelQurey += " WHERE View_ItemDetails.Barcode='" + TBBarcode.Text + "'";
            else if (sender == TBSearchCode && TBSearchCode.Text != string.Empty)
                SelQurey += " WHERE View_ItemDetails.SearchCode='" + TBSearchCode.Text + "'";

            DataSet DSDetails = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQurey);
            if (DSDetails.Tables[0].Rows.Count > 0)
            {
                string[] DataArray = new string[15];
                DataArray[1] = DSDetails.Tables[0].Rows[0]["Image"].ToString();
                DataArray[2] = DSDetails.Tables[0].Rows[0]["Barcode"].ToString();
                DataArray[3] = DSDetails.Tables[0].Rows[0]["Name"].ToString();
                DataArray[4] = "00.00";
                DataArray[5] = "00.00";
                DataArray[6] = "00.00";
                DataArray[7] = DSDetails.Tables[0].Rows[0]["UOM"].ToString();
                DataArray[8] = DSDetails.Tables[0].Rows[0]["PurPrice"].ToString();
                DataArray[9] = DSDetails.Tables[0].Rows[0]["TaxPer"].ToString();
                DataArray[10] = "00.00";
                DataArray[11] = DSDetails.Tables[0].Rows[0]["TaxPer"].ToString();
                DataArray[12] = "00.00";
                DataArray[13] = "00.00";
                DataArray[14] = "00.00";
                InsertDTTable(DataArray);
            }
            BindGVPurReturnInner();
        }
    }
}