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

public partial class ReceiveManager : System.Web.UI.Page
{
    #region AutoComplete For Item Textbox ,Barcode TextBox Or SearchCode TextBox

    // Auto Complete For Item List
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> AutoCompleteTBItemName(string prefixText)
    {
        DataSet DS = new DataSet();
        string query = "Select DISTINCT Name From Item Where Name Like '" + prefixText + "%'";
        DS = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, query);
        List<string> Data = new List<string>();
        for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
        {
            Data.Add(DS.Tables[0].Rows[i][0].ToString());
        }
        return Data;
    }

    // Auto Complete For Item Barcode
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> AutoCompleteTBBarcode(string prefixTax1)
    {
        DataSet DS = new DataSet();
        string query = "Select DISTINCT Barcode From Item Where Barcode Like '" + prefixTax1 + "%'";
        DS = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, query);
        List<string> Data = new List<string>();
        for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
        {
            Data.Add(DS.Tables[0].Rows[i][0].ToString());
        }
        return Data;
    }

    // Auto Complete For Item SearchCode
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> AutoCompleteTBSearchCode(string prefixTax2)
    {
        DataSet DS = new DataSet();
        string query = "Select DISTINCT SearchCode From Item Where SearchCode Like '" + prefixTax2 + "%'";
        DS = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, query);
        List<string> Data = new List<string>();
        for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
        {
            Data.Add(DS.Tables[0].Rows[i][0].ToString());
        }
        return Data;
    }

    #endregion

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

    #region Dynamic DataTable Section

    // Basic Data
    DataTable TempDT = new DataTable();
    string[] ItemArray = new string[18];

    // Create / Load DataTable
    private void CreateDTTable()
    {
        TempDT = new DataTable();
        if (TempDT.Columns.Count == 0)
        {
            TempDT.Columns.Add("ID", typeof(Int32));
            TempDT.Columns.Add("Image", typeof(string));
            TempDT.Columns.Add("Barcode", typeof(string));
            TempDT.Columns.Add("ItemName", typeof(string));
            TempDT.Columns.Add("UOM", typeof(string));
            TempDT.Columns.Add("RecQty", typeof(string));
            TempDT.Columns.Add("OrderedQty", typeof(string));
            TempDT.Columns.Add("PendingQty", typeof(string));
            TempDT.Columns.Add("unitprice", typeof(string));
            TempDT.Columns.Add("TotalPrice", typeof(string));
            TempDT.Columns.Add("ItemLevelDisPer", typeof(string));
            TempDT.Columns.Add("ItemLevelDisValue", typeof(string));
            TempDT.Columns.Add("TaxableAmt", typeof(string));
            TempDT.Columns.Add("CGSTPer", typeof(string));
            TempDT.Columns.Add("CGSTValue", typeof(string));
            TempDT.Columns.Add("SGSTPer", typeof(string));
            TempDT.Columns.Add("SGSTValue", typeof(string));
            TempDT.Columns.Add("UnitPriceATax", typeof(string));
          
        }
        ViewState["DTTable"] = TempDT;
    }

    // For Insert Into DataTable
    private void InsertDTTable(string[] ItemArray)
    {
        TempDT = ViewState["DTTable"] as DataTable;

        TempDT.Rows.Add(ItemArray[0], ItemArray[1], ItemArray[2], ItemArray[3], ItemArray[4],
                        ItemArray[5], ItemArray[6], ItemArray[7], ItemArray[8],
                        ItemArray[9], ItemArray[10], ItemArray[11], ItemArray[12],
                        ItemArray[13], ItemArray[14], ItemArray[15], ItemArray[16],
                        ItemArray[17]);

        ViewState["DTTable"] = TempDT;
    }

    // For Select From DataTable Via ID
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
                ItemArray[2] = dr["Barcode"].ToString();
                ItemArray[3] = dr["ItemName"].ToString();
                ItemArray[4] = dr["UOM"].ToString();
                ItemArray[5] = dr["RecQty"].ToString();
                ItemArray[6] = dr["OrderedQty"].ToString();
                ItemArray[7] = dr["PendingQty"].ToString();
                ItemArray[8] = dr["unitprice"].ToString();
                ItemArray[9] = dr["TotalPrice"].ToString();
                ItemArray[10] = dr["ItemLevelDisPer"].ToString();
                ItemArray[11] = dr["ItemLevelDisValue"].ToString();
                ItemArray[12] = dr["TaxableAmt"].ToString();
                ItemArray[13] = dr["CGSTPer"].ToString();
                ItemArray[14] = dr["CGSTValue"].ToString();
                ItemArray[15] = dr["SGSTPer"].ToString();
                ItemArray[16] = dr["SGSTValue"].ToString();
                ItemArray[17] = dr["UnitPriceATax"].ToString();
             
            }
        }
        return ItemArray;
    }

    // For Select From DataTable Via Barcode
    private string[] SelectDTTable(string Barcode)
    {
        TempDT = ViewState["DTTable"] as DataTable;
        for (int i = TempDT.Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = TempDT.Rows[i];
            if (dr[2].ToString() == Barcode)
            {
                ItemArray[0] = dr["ID"].ToString();
                ItemArray[1] = dr["Image"].ToString();
                ItemArray[2] = dr["Barcode"].ToString();
                ItemArray[3] = dr["ItemName"].ToString();
                ItemArray[4] = dr["UOM"].ToString();
                ItemArray[5] = dr["RecQty"].ToString();
                ItemArray[6] = dr["OrderedQty"].ToString();
                ItemArray[7] = dr["PendingQty"].ToString();
                ItemArray[8] = dr["unitprice"].ToString();
                ItemArray[9] = dr["TotalPrice"].ToString();
                ItemArray[10] = dr["ItemLevelDisPer"].ToString();
                ItemArray[11] = dr["ItemLevelDisValue"].ToString();

                ItemArray[12] = dr["TaxableAmt"].ToString();
                ItemArray[13] = dr["CGSTPer"].ToString();
                ItemArray[14] = dr["CGSTValue"].ToString();
                ItemArray[15] = dr["SGSTPer"].ToString();
                ItemArray[16] = dr["SGSTValue"].ToString();
                ItemArray[17] = dr["UnitPriceATax"].ToString();
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
                dr["ID"] = ItemArray[0].ToString();
                dr["Image"] = ItemArray[1].ToString();
                dr["Barcode"] = ItemArray[2].ToString();
                dr["ItemName"] = ItemArray[3].ToString();
                dr["UOM"] = ItemArray[4].ToString();
                dr["RecQty"] = ItemArray[5].ToString();
                dr["OrderedQty"] = ItemArray[6].ToString();
                dr["PendingQty"] = ItemArray[7].ToString();
                dr["unitprice"] = ItemArray[8].ToString();
                dr["TotalPrice"] = ItemArray[9].ToString();
                dr["ItemLevelDisPer"] = ItemArray[10].ToString();
                dr["ItemLevelDisValue"] = ItemArray[11].ToString(); ;
                //dr["InvoiceLevelDisPer"] = ItemArray[11].ToString();
                //dr["InvoiceLevelDisValue"] = ItemArray[12].ToString();
                dr["TaxableAmt"] = ItemArray[12].ToString();
                dr["CGSTPer"] = ItemArray[13].ToString();
                dr["CGSTValue"] = ItemArray[14].ToString();
                dr["SGSTPer"] = ItemArray[15].ToString();
                dr["SGSTValue"] = ItemArray[16].ToString();
                dr["UnitPriceATax"] = ItemArray[17].ToString();
               
            }
        }
    }

    // For Update DataTable Record
    private void UpdateDTTable(string Barcode, string[] ItemArray)
    {
        TempDT = ViewState["DTTable"] as DataTable;
        for (int i = TempDT.Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = TempDT.Rows[i];
            if (dr[2].ToString() == Barcode)
            {
                dr["ID"] = ItemArray[0].ToString();
                dr["Image"] = ItemArray[1].ToString();
                dr["Barcode"] = ItemArray[2].ToString();
                dr["ItemName"] = ItemArray[3].ToString();
                dr["UOM"] = ItemArray[4].ToString();
                dr["RecQty"] = ItemArray[5].ToString();
                dr["OrderedQty"] = ItemArray[6].ToString();
                dr["PendingQty"] = ItemArray[7].ToString();
                dr["unitprice"] = ItemArray[8].ToString();
                dr["TotalPrice"] = ItemArray[9].ToString();
                dr["ItemLevelDisPer"] = ItemArray[10].ToString();
                dr["ItemLevelDisValue"] = ItemArray[11].ToString(); ;
                //dr["InvoiceLevelDisPer"] = ItemArray[11].ToString();
                //dr["InvoiceLevelDisValue"] = ItemArray[12].ToString();
                dr["TaxableAmt"] = ItemArray[12].ToString();
                dr["CGSTPer"] = ItemArray[13].ToString();
                dr["CGSTValue"] = ItemArray[14].ToString();
                dr["SGSTPer"] = ItemArray[15].ToString();
                dr["SGSTValue"] = ItemArray[16].ToString();
                dr["UnitPriceATax"] = ItemArray[17].ToString();
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
        string[] DataArray = new string[5];
        double Qty = 0.0;
        double value = 0.0;
        double CGST = 0.0;
        double SGST = 0.0;
        double tototal = 0.0;
        TempDT = ViewState["DTTable"] as DataTable;
        for (int i = TempDT.Rows.Count - 1; i >= 0; i--)
        {
            Qty += Convert.ToDouble(TempDT.Rows[i][5]);
            value += Convert.ToDouble(TempDT.Rows[i][12]);
            CGST += Convert.ToDouble(TempDT.Rows[i][14]);
            SGST += Convert.ToDouble(TempDT.Rows[i][16]);
            tototal += Convert.ToDouble(TempDT.Rows[i][17]);
        }
        DataArray[0] = Qty.ToString();
        DataArray[1] = value.ToString();
        DataArray[2] = CGST.ToString();
        DataArray[3] = SGST.ToString();
        DataArray[4] = tototal.ToString();
        return DataArray;
    }

    #endregion

    // Method For Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CreateDTTable();
            BindGVReceive();
        }
    }

    // Method For Bind GridView Receive
    private void BindGVReceive()
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[1];
            arrParam[0] = new SqlParameter("@Action", "ALL");
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveMasterView",arrParam );
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                ViewState["ReceiveGrid"] = DSRecords.Tables[0];
                GVPOMaster.DataSource = DSRecords;
                GVPOMaster.DataBind();
            }
            else
            {
                GVPOMaster.DataBind();
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For AddNew Button Click
    protected void AddNewClick(object sender, EventArgs e)
    {
        try
        {
            MVReceivedItem.ActiveViewIndex = 1;
            LBLDate.Text = DateTime.Now.ToString();
            BindBusinessLocation();
            BindPO();
            BindSupplier();
            BindEmployee();
            DDLPO.Enabled = false;
            LBGetPOItem.Enabled = false;
            BindInnerGV();
            GetDocumentCode();
            BindCompany();
            ////RBDisFlat.Checked = true;
            ////TBDisPer.Enabled = false;
            CreateDTTable();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void BindCompany()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT");
        DataSet DSCompany = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrParam);
        if (DSCompany.Tables[0].Rows.Count > 0)
        {
            DDLCompany.DataSource = DSCompany.Tables[0];
            DDLCompany.DataTextField = "CompanyName";
            DDLCompany.DataValueField = "ID";
            DDLCompany.DataBind();
            DDLCompany.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    // Method For Bind Business Location Details
    private void BindBusinessLocation()
    {
        try
        {
            SqlParameter[] arrparm = new SqlParameter[1];
            arrparm[0] = new SqlParameter("@Action", "SELECT");
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrparm);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                DDLBusinessLocation.DataSource = DSRecords.Tables[0];
                DDLBusinessLocation.DataTextField = "BusinessLocationName";
                DDLBusinessLocation.DataValueField = "ID";
                DDLBusinessLocation.DataBind();
                DDLBusinessLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Business Location Selected Index Changed
    protected void DDLBusinessLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LBLItemEntryNo.Text = GetDocumentCode(DDLBusinessLocation.SelectedValue);
        BindGodown();
    }

    // Get Document Code
    private void GetDocumentCode()
    {
        try
        {
            string DocumentCode = "RIE"; string PrvID = ""; int NextID = 1;
            string GetMaxId = "SELECT RecNo From Receive_Master Order By ID DESC ";
            DataTable dtBill = clsConnectionSql.filldt(GetMaxId);
            if (dtBill.Rows.Count > 0)
            {
                PrvID = dtBill.Rows[0]["RecNo"].ToString();
                string[] PrvCon = PrvID.Split('E');
                NextID = Convert.ToInt32(PrvCon[1]) + 1;
                string Zero = new String('0', (5 - NextID.ToString().Length));
                DocumentCode = DocumentCode + Zero + NextID.ToString();
                LBLItemEntryNo.Text = DocumentCode.ToString();
            }
            else
                LBLItemEntryNo.Text = "RIE00001";
        }
        catch (Exception ex)
        {
        }
    }

    // Method For Bind Godown Details
    private void BindGodown()
    {
        try
        {
            SqlParameter[] arrparm = new SqlParameter[1];
            arrparm[0] = new SqlParameter("@Action", "SELECT_ByDefault");
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrparm);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                DDLGodown.DataSource = DSRecords.Tables[0];
                DDLGodown.DataTextField = "Name";
                DDLGodown.DataValueField = "ID";
                DDLGodown.DataBind();
                DDLGodown.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Bind PO Details
    private void BindPO()
    {
        try
        {
            string Query = "SELECT * FROM POMaster WHERE POStatus IN (0,1)";
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, Query);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                DDLPO.DataSource = DSRecords.Tables[0];
                DDLPO.DataTextField = "PONo";
                DDLPO.DataValueField = "ID";
                DDLPO.DataBind();
                DDLPO.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Bind Supplier Details
    private void BindSupplier()
    {
        try
        {
            SqlParameter[] arrparm = new SqlParameter[1];
            arrparm[0] = new SqlParameter("@Action", "SELECT");
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Supplier_CRUD", arrparm);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                DDLSupplier.DataSource = DSRecords.Tables[0];
                DDLSupplier.DataTextField = "Name";
                DDLSupplier.DataValueField = "ID";
                DDLSupplier.DataBind();
                DDLSupplier.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Bind Employee Details
    private void BindEmployee()
    {
        try
        {
            string str = "SELECT ID,(FirstName +' '+ MiddleName +' '+ LastName) AS EmpName FROM Employee";
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, str);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                DDLRecEmp.DataSource = DSRecords.Tables[0];
                DDLRecEmp.DataTextField = "EmpName";
                DDLRecEmp.DataValueField = "ID";
                DDLRecEmp.DataBind();
                DDLRecEmp.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Bind Inner GridView
    private void BindInnerGV()
    {
        try
        {
            GVPO.DataSource = ViewState["DTTable"] as DataTable;
            GVPO.DataBind();
            InnerGVData();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }

    }

    // Method For Product Select
    protected void ProductSelect(object sender, EventArgs e)
    {
        try
        {
            string[] TempArrProduct = new string[18];

            string SelQuery = "SELECT * FROM View_ItemDetails";
            if (sender == TBBarcode)
                SelQuery += " WHERE View_ItemDetails.Barcode='" + TBBarcode.Text.Trim() + "'";
            else if (sender == TBItemName)
                SelQuery += " WHERE View_ItemDetails.ItemName='" + TBItemName.Text.Trim() + "'";
            else if (sender == TBSearchCode)
                SelQuery += " WHERE View_ItemDetails.ProductCode='" + TBSearchCode.Text.Trim() + "'";

            DataSet DSItemDetails = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
            if (DSItemDetails.Tables[0].Rows.Count > 0)
            {
                TempArrProduct = SelectDTTable(DSItemDetails.Tables[0].Rows[0]["Barcode"].ToString());
                if (TempArrProduct[2] == DSItemDetails.Tables[0].Rows[0]["Barcode"].ToString())
                {
                    double PurPrice = Convert.ToDouble(DSItemDetails.Tables[0].Rows[0]["Purprice"]);
                    double PurTax = Convert.ToDouble(DSItemDetails.Tables[0].Rows[0]["TaxPer"])/2;
                  
                    double RecQty = Convert.ToDouble(TempArrProduct[5]) + 1.0;
                    double OrdQty = 00.00;
                    double PendingQty = (OrdQty > 0.0) ? (OrdQty - RecQty) : 00.00;
                    double totunitp = PurPrice * RecQty;
                 //   double RecQty = ReceiveQty;
                   // double OrdQty = OrderedQty;
                   // double PendingQty = (OrdQty > 0.0) ? (OrdQty - RecQty) : 00.00;

                    //double LevelDisPer = LevelDiscount;
                    //double LevelDisValue = (LevelDisPer * totunitp) / 100;

                    //double InvoiceDisPer = InvoiceLevelDis;
                    //double InvoiceDisValue = (InvoiceDisPer * totunitp) / 100;

                    //double UnitPriceWithDis = totunitp - (LevelDisValue + InvoiceDisValue);
                    //double SubTotalBTax = UnitPriceWithDis * RecQty;

                    double TaxValue = (PurTax * totunitp) / 100;
                    double TotalTax = TaxValue;

                    double UnitPriceWithTax = totunitp + TaxValue + TaxValue;
                    double SubTotalATax = UnitPriceWithTax;

                    double EffRatePerUnit = UnitPriceWithTax;

                    string[] DataArray = new string[18];
                    DataArray[0] = DSItemDetails.Tables[0].Rows[0]["ID"].ToString();
                    DataArray[1] = DSItemDetails.Tables[0].Rows[0]["Image"].ToString();
                    DataArray[2] = DSItemDetails.Tables[0].Rows[0]["Barcode"].ToString();
                    DataArray[3] = DSItemDetails.Tables[0].Rows[0]["Name"].ToString();
                    DataArray[4] = DSItemDetails.Tables[0].Rows[0]["UOM"].ToString();
                    DataArray[5] = RecQty.ToString("00.00");
                    DataArray[6] = OrdQty.ToString("00.00");
                    DataArray[7] = PendingQty.ToString("00.00");
                    DataArray[8] = PurPrice.ToString("00.00");
                    DataArray[9] = totunitp.ToString("00.00");
                    DataArray[10] = "00.00";
                    DataArray[11] = "00.00";
                    DataArray[12] = totunitp.ToString("00.00");
                    DataArray[13] = PurTax.ToString("00.00");
                    DataArray[14] = TaxValue.ToString("00.00");
                    DataArray[15] = PurTax.ToString("00.00");
                    DataArray[16] = TaxValue.ToString("00.00");
                    DataArray[17] = UnitPriceWithTax.ToString("00.00");
                    //DataArray[19] = SubTotalATax.ToString("00.00");
                    //DataArray[20] = EffRatePerUnit.ToString("00.00");
                   // InsertDTTable(DataArray);
                    UpdateDTTable(DataArray[2].ToString(), DataArray);
                }
                else
                {
                    ProductBindInnerGVDataCalculation(DSItemDetails, 1.0, 0.0, 0.0, 0.0);
                }
            }
            BindInnerGV();

            if (sender == TBBarcode)
            {
                TBBarcode.Text = string.Empty;
                TBBarcode.Focus();
            }
            if (sender == TBItemName)
            {
                TBItemName.Text = string.Empty;
                TBItemName.Focus();
            }
            if (sender == TBSearchCode)
            {
                TBSearchCode.Text = string.Empty;
                TBSearchCode.Focus();
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }
    
    private void ProductBindInnerGVDataCalculation(DataSet DSItemDetails, double ReceiveQty, double OrderedQty, double LevelDiscount, double InvoiceLevelDis)
    {
        try
        {
            double PurPrice = 0;
            //if (OrderedQty > 1)
            //{
            //    PurPrice = Convert.ToDouble(DSItemDetails.Tables[0].Rows[0]["Pur"]) / OrderedQty;
            //}
            //else
            //{
                PurPrice = Convert.ToDouble(DSItemDetails.Tables[0].Rows[0]["PurPrice"]);
           // }
            double PurTax = Convert.ToDouble(DSItemDetails.Tables[0].Rows[0]["TaxPer"]) / 2;
            //double taxval = (100 / (100 + PurTax)) * PurPrice;

            double totunitp = PurPrice * ReceiveQty;
            double RecQty = ReceiveQty;
            double OrdQty = OrderedQty;
            double PendingQty = (OrdQty > 0.0) ? (OrdQty - RecQty) : 00.00;

            double LevelDisPer = LevelDiscount;
            double LevelDisValue = (LevelDisPer * totunitp) / 100;

            double InvoiceDisPer = InvoiceLevelDis;
            double InvoiceDisValue = (InvoiceDisPer * totunitp) / 100;

            double UnitPriceWithDis = totunitp - (LevelDisValue + InvoiceDisValue);
            //double SubTotalBTax = UnitPriceWithDis * RecQty;

            double TaxValue = (PurTax * UnitPriceWithDis) / 100;
            double TotalTax = TaxValue;

            double UnitPriceWithTax = UnitPriceWithDis + TaxValue + TaxValue;
            double SubTotalATax = UnitPriceWithTax;

            double EffRatePerUnit = UnitPriceWithTax;

            string[] DataArray = new string[18];
            DataArray[0] = DSItemDetails.Tables[0].Rows[0]["ID"].ToString();
            DataArray[1] = DSItemDetails.Tables[0].Rows[0]["Image"].ToString();
            DataArray[2] = DSItemDetails.Tables[0].Rows[0]["Barcode"].ToString();
            DataArray[3] = DSItemDetails.Tables[0].Rows[0]["Name"].ToString();
            DataArray[4] = DSItemDetails.Tables[0].Rows[0]["UOM"].ToString();
            DataArray[5] = RecQty.ToString("00.00");
            DataArray[6] = OrdQty.ToString("00.00");
            DataArray[7] = PendingQty.ToString("00.00");
            DataArray[8] = PurPrice.ToString("00.00");
            DataArray[9] = totunitp.ToString("00.00");
            DataArray[10] = LevelDisPer.ToString("00.00");
            DataArray[11] = LevelDisValue.ToString("00.00");
            DataArray[12] = UnitPriceWithDis.ToString("00.00");
            DataArray[13] = PurTax.ToString("00.00");
            DataArray[14] = TaxValue.ToString("00.00");
            DataArray[15] = PurTax.ToString("00.00");
            DataArray[16] = TaxValue.ToString("00.00");
            DataArray[17] = UnitPriceWithTax.ToString("00.00");
            //DataArray[19] = SubTotalATax.ToString("00.00");
            //DataArray[20] = EffRatePerUnit.ToString("00.00");
            InsertDTTable(DataArray);
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }
    
    // Method For Bind InnerGVDataCaluclation
    private void BindInnerGVDataCalculation(DataSet DSItemDetails, double ReceiveQty, double OrderedQty, double LevelDiscount, double InvoiceLevelDis)
    {
        try
        {
            double PurPrice = 0;
            if (OrderedQty > 1)
            {
                 PurPrice = Convert.ToDouble(DSItemDetails.Tables[0].Rows[0]["UnitPrice"])/OrderedQty;
            }
            else
            {
                 PurPrice = Convert.ToDouble(DSItemDetails.Tables[0].Rows[0]["UnitPrice"]);
            }
            double PurTax = Convert.ToDouble(DSItemDetails.Tables[0].Rows[0]["TaxPer"])/2;
            //double taxval = (100 / (100 + PurTax)) * PurPrice;

            double totunitp =PurPrice*ReceiveQty;
            double RecQty = ReceiveQty;
            double OrdQty = OrderedQty;
            double PendingQty = (OrdQty > 0.0) ? (OrdQty - RecQty) : 00.00;

            double LevelDisPer = LevelDiscount;
            double LevelDisValue = (LevelDisPer * totunitp) / 100;

            double InvoiceDisPer = InvoiceLevelDis;
            double InvoiceDisValue = (InvoiceDisPer * totunitp) / 100;

            double UnitPriceWithDis = totunitp - (LevelDisValue + InvoiceDisValue);
            //double SubTotalBTax = UnitPriceWithDis * RecQty;

            double TaxValue = (PurTax * UnitPriceWithDis) / 100;
            double TotalTax = TaxValue ;

            double UnitPriceWithTax = UnitPriceWithDis + TaxValue;
            double SubTotalATax = UnitPriceWithTax ;

            double EffRatePerUnit = UnitPriceWithTax;

            string[] DataArray = new string[18];
            DataArray[0] = DSItemDetails.Tables[0].Rows[0]["ID"].ToString();
            DataArray[1] = DSItemDetails.Tables[0].Rows[0]["Image"].ToString();
            DataArray[2] = DSItemDetails.Tables[0].Rows[0]["Barcode"].ToString();
            DataArray[3] = DSItemDetails.Tables[0].Rows[0]["Name"].ToString();
            DataArray[4] =  DSItemDetails.Tables[0].Rows[0]["UOM"].ToString();
            DataArray[5] = RecQty.ToString("00.00");
            DataArray[6] = OrdQty.ToString("00.00");
            DataArray[7] =PendingQty.ToString("00.00");
            DataArray[8] = PurPrice.ToString("00.00");
            DataArray[9] = totunitp.ToString("00.00");
            DataArray[10] = LevelDisPer.ToString("00.00");
            DataArray[11] = LevelDisValue.ToString("00.00");
            DataArray[12] = UnitPriceWithDis.ToString("00.00");
            DataArray[13] = PurTax.ToString("00.00");
            DataArray[14] = TaxValue.ToString("00.00");
            DataArray[15] = PurTax.ToString("00.00");
            DataArray[16] = TaxValue.ToString("00.00");        
            DataArray[17] = UnitPriceWithTax.ToString("00.00");
            //DataArray[19] = SubTotalATax.ToString("00.00");
            //DataArray[20] = EffRatePerUnit.ToString("00.00");
            InsertDTTable(DataArray);
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Inner GridTB Text Changed
    protected void InnerGridTBTextChanges()
    {
        try
        {
            for (int i = 0; i < GVPO.Rows.Count; i++)
            {
                TextBox RecQty = (TextBox)GVPO.Rows[i].FindControl("TBReceoveQuantity");
                TextBox ItemLevelDis = (TextBox)GVPO.Rows[i].FindControl("TBItemLevelDisPer");
                TextBox InvoiceLeveDis = (TextBox)GVPO.Rows[i].FindControl("TBInvoiceLevelDisPer");
                //UpdateInnerGVDataCalculation(DSItemDetails, 1.0, 0.0, 0.0, 0.0);
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For RecViaPO CheckBox Checked Changed
    protected void RecViaPOClick(object sender, EventArgs e)
    {
        try
        {
            if (CBReceivedViaPO.Checked)
            {
                TBBarcode.Enabled = false;
                TBItemName.Enabled = false;
                TBSearchCode.Enabled = false;
                DDLSupplier.Enabled = false;
                DDLPO.Enabled = true;
                BindPO();
                LBGetPOItem.Enabled = true;
            }
            else
            {
                TBBarcode.Enabled = true;
                TBItemName.Enabled = true;
                TBSearchCode.Enabled = true;
                DDLSupplier.Enabled = true;
                DDLPO.Enabled = false;
                DDLPO.ClearSelection();
                LBGetPOItem.Enabled = false;
            }

            ClearProductGrid();
            BindInnerGV();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Clear Product Grid
    private void ClearProductGrid()
    {
        try
        {
            DataTable TempDT = ViewState["DTTable"] as DataTable;
            TempDT.Clear();
            ViewState["DTTable"] = TempDT;
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Change PO Data
    protected void ChangePOData(object sender, EventArgs e)
    {
        try
        {
            if (DDLPO.SelectedIndex > 0)
            {
                string GetSupplier = "SELECT Supplier,PaymentTerms FROM POMaster WHERE ID='" + DDLPO.SelectedValue + "'";
                DataSet DSSup = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, GetSupplier);
                if (DSSup.Tables[0].Rows.Count > 0)
                {
                    BindSupplier();
                    DDLSupplier.ClearSelection();
                    DDLSupplier.Items.FindByValue(DSSup.Tables[0].Rows[0]["Supplier"].ToString()).Selected = true;
                    //DDLPaymentTerms.ClearSelection();
                    TBPaymentTerms.Text = DSSup.Tables[0].Rows[0]["PaymentTerms"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For InnerGVData
    protected void InnerGVData()
    {
        try
        {
            if (CBReceivedViaPO.Checked == false)
            {
                GVPO.Columns[6].Visible = false;
                GVPO.Columns[7].Visible = false;
            }
            else
            {
                GVPO.Columns[6].Visible = true;
                GVPO.Columns[7].Visible = true;
            }

            string[] array = new string[5];
            array = TotalDetails();
            LBLNoOfPackages.Text = array[0];
            TBSubTotalBTax.Text = array[1];
            TBTotalTaxAmount.Text = array[2];
            TBSGST.Text = array[3];
            ////TBGrandTotal.Text = Use below
            TBTotalAmount.Text =array[4];
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Bottom Button Events
    protected void BottomButtonEvent(object sender, EventArgs e)
    {
        try
        {
            // Method For Hold Record
            if (sender == LBHold)
            {
                InsertMasterHoldRecord();

                //Get Master Data ID
                int MID = 0;
                string SelQuery = "SELECT TOP(1) ID FROM ReceiveMaster ORDER BY ID DESC";
                DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
                if (ds.Tables[0].Rows.Count > 0)
                    MID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                InsertDetailHoldRecord(MID);

                ClearMasterDatas();
                ClearProductGrid();
            }

            // Method for Submit Record
            else if (sender == btnSubmit)
            {
                if (btnSubmit.Text == "Submit")
                {
                    InsertMasterRecord();

                    //Get Master Data ID
                    int MID = 0;
                    string SelQuery = "SELECT TOP(1) ID FROM Receive_Master ORDER BY ID DESC";
                    DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
                    if (ds.Tables[0].Rows.Count > 0)
                        MID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    InsertDetailRecord(MID);
                }
                else if (btnSubmit.Text == "Update")
                {
                    UpdateMasterRecord();

                    //Get Master Data ID
                    int MID = 0;
                    string SelQuery = "SELECT TOP(1) ID FROM Receive_Master ORDER BY ID DESC";
                    DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
                    if (ds.Tables[0].Rows.Count > 0)
                        MID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    UpdateDetailRecord(MID);
                }
                ClearMasterDatas();
                ClearProductGrid();
            }

            // Method for Cancel Record
            else if (sender == LBCancel)
            {
                ClearMasterDatas();
                ClearProductGrid();
            }

            MVReceivedItem.ActiveViewIndex = 0;
            BindGVReceive();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Insertmaster hold Record
    private void InsertMasterHoldRecord()
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[26];
            arrParam[0] = new SqlParameter("@Action", "INSERT");
            arrParam[1] = new SqlParameter("@BusinessLocation", DDLBusinessLocation.SelectedValue);
            arrParam[2] = new SqlParameter("@RecItemDate", Convert.ToDateTime(LBLDate.Text));
            arrParam[3] = new SqlParameter("@RecOnDate", Convert.ToDateTime(TBReceivedOnDate.Text));
            arrParam[4] = new SqlParameter("@RecViaPO", CBReceivedViaPO.Checked);
            arrParam[5] = new SqlParameter("@PONumber", DDLPO.SelectedIndex > 0 ? DDLPO.SelectedItem.Text : "0");
            arrParam[6] = new SqlParameter("@Supplier", DDLSupplier.SelectedValue);
            arrParam[7] = new SqlParameter("@SupplierInvoiceNo", TBSupplierInvoiceNo.Text);
            arrParam[8] = new SqlParameter("@PaymentTerm", TBPaymentTerms.Text);
            arrParam[9] = new SqlParameter("@SupplierInvoiceDate", Convert.ToDateTime(TBSupplierInvoiceDate.Text));
            arrParam[10] = new SqlParameter("@Godown", DDLGodown.SelectedValue);
            arrParam[11] = new SqlParameter("@ReceivingEmp", DDLRecEmp.SelectedValue);
            arrParam[12] = new SqlParameter("@NumberOfPackages", LBLNoOfPackages.Text);
            arrParam[13] = new SqlParameter("@DiscountOnTotalBTax", 0);                 ////CBDisOnTotalBTax.Checked);
            arrParam[14] = new SqlParameter("@IsDisFlat", 0);                           ////RBDisFlat.Checked);
            arrParam[15] = new SqlParameter("@DisFlatValue", 0);                         //// TBDisFlat.Text);
            arrParam[16] = new SqlParameter("@isDisPer", false);                        ////RBDisPer.Checked);
            arrParam[17] = new SqlParameter("@DisPerValue", 0);                          //// RBDisPer.Text);
            arrParam[18] = new SqlParameter("@InvoiceDisAmount", 0);                     //// LBLInvDisAmount.Text);
            arrParam[19] = new SqlParameter("@SubTotalBTax", TBSubTotalBTax.Text);
            arrParam[20] = new SqlParameter("@TaxAmount", TBTotalTaxAmount.Text);
            arrParam[21] = new SqlParameter("@SubTotalATax", TBSubTotal.Text);
            arrParam[22] = new SqlParameter("@AddCharges", 0);                           //// TBAdiCharges.Text);
            arrParam[23] = new SqlParameter("@GrandTotal", 0);                           //// TBGrandTotal.Text);
            arrParam[24] = new SqlParameter("@Notes", TBNote.Text);
            arrParam[25] = new SqlParameter("@DocStatus", "Hold");
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveMaster_CRUD", arrParam);
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Insert Details Hold Record
    private void InsertDetailHoldRecord(int MID)
    {
        try
        {
            TempDT = ViewState["DTTable"] as DataTable;
            for (int i = 0; i < TempDT.Rows.Count; i++)
            {
                DataRow dr = TempDT.Rows[i];
                SqlParameter[] arrParamDetails = new SqlParameter[22];
                arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                arrParamDetails[1] = new SqlParameter("@MID", MID);
                arrParamDetails[2] = new SqlParameter("@Image", dr[1]);
                arrParamDetails[3] = new SqlParameter("@Barcode", dr[2]);
                arrParamDetails[4] = new SqlParameter("@ItemName", dr[3]);
                arrParamDetails[5] = new SqlParameter("@RecQty", dr[4]);
                arrParamDetails[6] = new SqlParameter("@OrderedQty", dr[5]);
                arrParamDetails[7] = new SqlParameter("@PendingQty", dr[6]);
                arrParamDetails[8] = new SqlParameter("@MeasuringUnit", dr[7]);
                arrParamDetails[9] = new SqlParameter("@UnitPriceBTax", dr[8]);
                arrParamDetails[10] = new SqlParameter("@ItemLevelDisPer", dr[9]);
                arrParamDetails[11] = new SqlParameter("@ItemLevelDisValue", dr[10]);
                arrParamDetails[12] = new SqlParameter("@InvoiceLevelDisPer", dr[11]);
                arrParamDetails[13] = new SqlParameter("@InvoicelevelDisValue", dr[12]);
                arrParamDetails[14] = new SqlParameter("@SubTotalBTax", dr[13]);
                arrParamDetails[15] = new SqlParameter("@TaxName", dr[14]);
                arrParamDetails[16] = new SqlParameter("@TaxPercent", dr[15]);
                arrParamDetails[17] = new SqlParameter("@TaxValue", dr[16]);
                arrParamDetails[18] = new SqlParameter("@TotalTaxAmount", dr[17]);
                arrParamDetails[19] = new SqlParameter("@UnitPriceATax", dr[18]);
                arrParamDetails[20] = new SqlParameter("@SubTotalATax", dr[19]);
                arrParamDetails[21] = new SqlParameter("@EffRatePerUnit", dr[20]);
                int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveDetails_CRUD", arrParamDetails);
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Insert Master Record
    private void InsertMasterRecord()
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[16];
            arrParam[0] = new SqlParameter("@Action", "INSERT");
            arrParam[1] = new SqlParameter("@ComID", 1);
            arrParam[2] = new SqlParameter("@NGOID", DDLBusinessLocation.SelectedValue);
            arrParam[3] = new SqlParameter("@RecNo", LBLItemEntryNo.Text);
            arrParam[4] = new SqlParameter("@RecDate", Convert.ToDateTime(TBReceivedOnDate.Text));
            arrParam[5] = new SqlParameter("@PONo", DDLPO.Text);
            arrParam[6] = new SqlParameter("@Godown", DDLGodown.SelectedValue);
            arrParam[7] = new SqlParameter("@Supplier", DDLSupplier.SelectedValue);
            arrParam[8] = new SqlParameter("@PaymentTerms", TBPaymentTerms.Text);
            arrParam[9] = new SqlParameter("@TotalQty", Convert.ToDecimal(LBLNoOfPackages.Text));
            arrParam[10] = new SqlParameter("@TotalValue",  Convert.ToDecimal(TBSubTotalBTax.Text));
            arrParam[11] = new SqlParameter("@TotalCGST",  Convert.ToDecimal(TBTotalTaxAmount.Text));
            arrParam[12] = new SqlParameter("@TotalSGST",  Convert.ToDecimal(TBSGST.Text));
            arrParam[13] = new SqlParameter("@TotalAmount",  Convert.ToDecimal(TBTotalAmount.Text));
            arrParam[14] = new SqlParameter("@Remark",TBNote.Text); ////CBDisOnTotalBTax.Checked);
            arrParam[15] = new SqlParameter("@CreatedBy", DDLRecEmp.SelectedValue);//// RBDisFlat.Checked);
           
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveMaster_CRUD", arrParam);

            if (CBReceivedViaPO.Checked)
            {
                string SelQuery = "Select TotalQty FROM POMaster WHERE ID='" + DDLPO.SelectedValue + "'";
                DataTable DTPO = clsConnectionSql.filldt(SelQuery);
                string UpdatePO = "";
                if (Convert.ToDecimal(DTPO.Rows[0][0]) == Convert.ToDecimal(LBLNoOfPackages.Text))
                    UpdatePO = "Update POMaster Set POStatus='" + 2 + "' WHERE ID='" + DDLPO.SelectedValue + "'";
                else if (Convert.ToDecimal(DTPO.Rows[0][0]) > Convert.ToDecimal(LBLNoOfPackages.Text))
                    UpdatePO = "Update POMaster Set POStatus='" + 1 + "' WHERE ID='" + DDLPO.SelectedValue + "'";

                int result2 = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.Text, UpdatePO);

            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }
  
    // Method For insert Details Records 
    private void InsertDetailRecord(int MID)
    {
        try
        {
            TempDT = ViewState["DTTable"] as DataTable;
            for (int i = 0; i < TempDT.Rows.Count; i++)
            {
                DataRow dr = TempDT.Rows[i];
                SqlParameter[] arrParamDetails = new SqlParameter[16];
                arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                arrParamDetails[1] = new SqlParameter("@RecID", MID);
                arrParamDetails[2] = new SqlParameter("@ItemID",Convert.ToInt32(dr[0]));
                arrParamDetails[3] = new SqlParameter("@Qty", Convert.ToDecimal( dr[5]));
                arrParamDetails[4] = new SqlParameter("@orderQty",  Convert.ToDecimal(dr[6]));
                arrParamDetails[5] = new SqlParameter("@PendingQty", Convert.ToDecimal( dr[7]));
                arrParamDetails[6] = new SqlParameter("@UnitPrice",  Convert.ToDecimal(dr[8]));
                arrParamDetails[7] = new SqlParameter("@TotalPrice", Convert.ToDecimal( dr[9]));
                arrParamDetails[8] = new SqlParameter("@DiscountPer",  Convert.ToDecimal(dr[10]));
                arrParamDetails[9] = new SqlParameter("@Discountval",  Convert.ToDecimal(dr[11]));
                arrParamDetails[10] = new SqlParameter("@TaxableAmt",  Convert.ToDecimal(dr[12]));
                arrParamDetails[11] = new SqlParameter("@CGSTPer",  Convert.ToDecimal(dr[13]));
                arrParamDetails[12] = new SqlParameter("@CGSTValue", Convert.ToDecimal( dr[14]));
                arrParamDetails[13] = new SqlParameter("@SGSTPer",  Convert.ToDecimal(dr[15]));
                arrParamDetails[14] = new SqlParameter("@SGSTValue", Convert.ToDecimal( dr[16]));
                arrParamDetails[15] = new SqlParameter("@TotalAmt", Convert.ToDecimal( dr[17]));

                int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveDetail_CRUD", arrParamDetails);

                //string InsertInner = "INSERT INTO ReceiveProducts (MID,PID,Quantity) VALUES ('" + MID + "','" + dr[0] + "','" + dr[4] + "')";
                //int resultInner2 = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.Text, InsertInner);

                if (resultInner > 0)
                {
                    // Add Quantity TO Stock

                    string UpdateStock = "Update ItemStock Set TotalStock=TotalStock+'" +Convert.ToDecimal( dr[5]) + "' WHERE ItemID='" + dr[0] + "' and GodownId='" + DDLGodown.SelectedValue + "'";
                    int res = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.Text, UpdateStock);

                    if (res == 0)
                    {
                        SqlParameter[] arrParam = new SqlParameter[10];
                        arrParam[0] = new SqlParameter("@Action", "INSERT");
                        arrParam[1] = new SqlParameter("@GodownID", DDLGodown.SelectedValue);
                        arrParam[2] = new SqlParameter("@ItemID", dr[0]);
                        arrParam[3] = new SqlParameter("@ItemType", "Raw Material");
                        arrParam[4] = new SqlParameter("@Date", Convert.ToDateTime(TBReceivedOnDate.Text));
                        arrParam[5] = new SqlParameter("@StockIN", dr[5]);
                        arrParam[6] = new SqlParameter("@StockOut", "0.00");
                        arrParam[7] = new SqlParameter("@Narration", "Receiving Stock");
                        arrParam[8] = new SqlParameter("@DocType", "Receiving");
                        arrParam[9] = new SqlParameter("@DocNo", LBLItemEntryNo.Text);

                        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_StockLedger_CRUD", arrParam);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Update Master Records
    private void UpdateMasterRecord()
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[16];
            arrParam[0] = new SqlParameter("@Action", "Update");
            arrParam[1] = new SqlParameter("@ComID", 1);
            arrParam[2] = new SqlParameter("@NGOID", DDLBusinessLocation.SelectedValue);
            arrParam[3] = new SqlParameter("@RecNo", LBLItemEntryNo.Text);
            arrParam[4] = new SqlParameter("@RecDate", Convert.ToDateTime(TBReceivedOnDate.Text));
            arrParam[5] = new SqlParameter("@PONo", DDLPO.Text);
            arrParam[6] = new SqlParameter("@Godown", DDLGodown.SelectedValue);
            arrParam[7] = new SqlParameter("@Supplier", DDLSupplier.SelectedValue);
            arrParam[8] = new SqlParameter("@PaymentTerms", TBPaymentTerms.Text);
            arrParam[9] = new SqlParameter("@TotalQty", Convert.ToDecimal(LBLNoOfPackages.Text));
            arrParam[10] = new SqlParameter("@TotalValue", Convert.ToDecimal(TBSubTotalBTax.Text));
            arrParam[11] = new SqlParameter("@TotalCGST", Convert.ToDecimal(TBTotalTaxAmount.Text));
            arrParam[12] = new SqlParameter("@TotalSGST", Convert.ToDecimal(TBSGST.Text));
            arrParam[13] = new SqlParameter("@TotalAmount", Convert.ToDecimal(TBTotalAmount.Text));
            arrParam[14] = new SqlParameter("@Remark", TBNote.Text); ////CBDisOnTotalBTax.Checked);
            arrParam[15] = new SqlParameter("@CreatedBy", DDLRecEmp.SelectedValue);//// RBDisFlat.Checked);

            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveMaster_CRUD", arrParam);
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Update Details Records
    private void UpdateDetailRecord(int MID)
    {
        try
        {
            TempDT = ViewState["DTTable"] as DataTable;
            for (int i = 0; i < TempDT.Rows.Count; i++)
            {
                DataRow dr = TempDT.Rows[i];
                SqlParameter[] arrParamDetails = new SqlParameter[22];
                arrParamDetails[0] = new SqlParameter("@Action", "Update");
                arrParamDetails[1] = new SqlParameter("@MID", MID);
                arrParamDetails[2] = new SqlParameter("@Image", dr[1]);
                arrParamDetails[3] = new SqlParameter("@Barcode", dr[2]);
                arrParamDetails[4] = new SqlParameter("@ItemName", dr[3]);
                arrParamDetails[5] = new SqlParameter("@RecQty", dr[4]);
                arrParamDetails[6] = new SqlParameter("@OrderedQty", dr[5]);
                arrParamDetails[7] = new SqlParameter("@PendingQty", dr[6]);
                arrParamDetails[8] = new SqlParameter("@MeasuringUnit", dr[7]);
                arrParamDetails[9] = new SqlParameter("@UnitPriceBTax", dr[8]);
                arrParamDetails[10] = new SqlParameter("@ItemLevelDisPer", dr[9]);
                arrParamDetails[11] = new SqlParameter("@ItemLevelDisValue", dr[10]);
                arrParamDetails[12] = new SqlParameter("@InvoiceLevelDisPer", dr[11]);
                arrParamDetails[13] = new SqlParameter("@InvoicelevelDisValue", dr[12]);
                arrParamDetails[14] = new SqlParameter("@SubTotalBTax", dr[13]);
                arrParamDetails[15] = new SqlParameter("@TaxName", dr[14]);
                arrParamDetails[16] = new SqlParameter("@TaxPercent", dr[15]);
                arrParamDetails[17] = new SqlParameter("@TaxValue", dr[16]);
                arrParamDetails[18] = new SqlParameter("@TotalTaxAmount", dr[17]);
                arrParamDetails[19] = new SqlParameter("@UnitPriceATax", dr[18]);
                arrParamDetails[20] = new SqlParameter("@SubTotalATax", dr[19]);
                arrParamDetails[21] = new SqlParameter("@EffRatePerUnit", dr[20]);
                int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveDetails_CRUD", arrParamDetails);

            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Clear Master Datas
    private void ClearMasterDatas()
    {
        try
        {
            DDLBusinessLocation.ClearSelection();
            TBSupplierInvoiceNo.Text = string.Empty;
            TBSupplierInvoiceDate.Text = string.Empty;
            TBReceivedOnDate.Text = string.Empty;
            DDLSupplier.ClearSelection();
            DDLPO.ClearSelection();
            TBPaymentTerms.Text = string.Empty;
            DDLRecEmp.ClearSelection();
            TBNote.Text = string.Empty;
            ////CBDisOnTotalBTax.Checked = false;
            ////RBDisFlat.Checked = false;
            ////TBDisFlat.Text = string.Empty;
            ////RBDisPer.Checked = false;
            ////TBDisPer.Text = string.Empty;
            ////TBAdiCharges.Text = string.Empty;
            ////CBApplyForEffPrice.Checked = false;
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Radio Button Discount
    protected void RBDiscount(object sender, EventArgs e)
    {
        ////if (RBDisFlat.Checked)
        ////{
        ////    TBDisPer.Enabled = false;
        ////    TBDisFlat.Enabled = true;
        ////}
        ////if (RBDisPer.Checked)
        ////{
        ////    TBDisFlat.Enabled = false;
        ////    TBDisPer.Enabled = true;
        ////}
    }

    // Method For Change Inner GridView
    protected void ChangeInnerGV(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < GVPO.Rows.Count; i++)
            {
                TextBox TBReceive = (TextBox)GVPO.Rows[i].FindControl("TBReceiveQuantity");
                TextBox TBItemLevelDisPer = (TextBox)GVPO.Rows[i].FindControl("TBItemLevelDisPer");
                TextBox TBItemLevelDisValue = (TextBox)GVPO.Rows[i].FindControl("TBItemLevelDisValue");

                HiddenField HFIDQuantity = (HiddenField)GVPO.Rows[i].FindControl("HFID");
                string[] StrArray = new string[18];
                StrArray = SelectDTTable(Convert.ToInt32(HFIDQuantity.Value));

               
                double RecQty = Convert.ToDouble(TBReceive.Text);
                double OrdQty = Convert.ToDouble(StrArray[6]);
                double PendingQty = (OrdQty > 0.0) ? (OrdQty - RecQty) : 00.00;

                double UnitPriceBTax = Convert.ToDouble(StrArray[8]) * RecQty; 
                double LevelDisPer = Convert.ToDouble(TBItemLevelDisPer.Text);
                double LevelDisValue = (LevelDisPer / 100) * UnitPriceBTax;
                //double InvoiceDisPer = 00.00;
                //double InvoiceDisValue = (InvoiceDisPer / 100) * UnitPriceBTax;
                double SubTotalBTax = (UnitPriceBTax - LevelDisValue) ;
                double cgst = (Convert.ToDouble(StrArray[13]) * SubTotalBTax) / 100;
                double sgst = (Convert.ToDouble(StrArray[15]) * SubTotalBTax) / 100;
                double UnitPriceATax = SubTotalBTax + cgst + sgst;
                //double SubTotalATax = UnitPriceATax * RecQty;
                double EffRatePerUnit = UnitPriceATax;

                StrArray[5] = RecQty.ToString("00.00");
                StrArray[6] = OrdQty.ToString("00.00");
                StrArray[7] = PendingQty.ToString("00.00");
                StrArray[9] = UnitPriceBTax.ToString("00.00");
                StrArray[10] = LevelDisPer.ToString("00.00");
                StrArray[11] = LevelDisValue.ToString("00.00");
                //StrArray[11] = InvoiceDisPer.ToString("00.00");
                //StrArray[12] = InvoiceDisValue.ToString("00.00");
                StrArray[12] = SubTotalBTax.ToString("00.00");
                StrArray[13] = StrArray[13];
                StrArray[14] = cgst.ToString("00.00");
                StrArray[15] = StrArray[15];
                StrArray[16] = sgst.ToString("00.00");
                StrArray[17] = EffRatePerUnit.ToString("00.00");
                UpdateDTTable(Convert.ToInt32(HFIDQuantity.Value), StrArray);
            }
            BindInnerGV();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For LinkButton POItemClick
    protected void LBGetPOItemClick(object sender, EventArgs e)
    {
        string SelQuery = "SELECT * FROM POProducts WHERE POID='" + DDLPO.SelectedValue + "'";
        DataSet DSPOItems = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
        if (DSPOItems.Tables[0].Rows.Count > 0)
        {
            TempDT = ViewState["DTTable"] as DataTable;
            TempDT.Clear();
        }
    }

    // Method For Bind PO Items
    protected void BindPOItem(object sender, EventArgs e)
    {
        try
        {
            if (DDLPO.SelectedIndex > 0)
            {
                string Query = "SELECT * FROM POdetails WHERE POID='" + DDLPO.SelectedValue + "'";
                DataTable DTPOProduct = clsConnectionSql.filldt(Query);
                if (DTPOProduct.Rows.Count > 0)
                {
                    for (int i = 0; i < DTPOProduct.Rows.Count; i++)
                    {
                        double RecQty = Convert.ToDouble(DTPOProduct.Rows[i]["Qty"]);
                        double OrdQty = Convert.ToDouble(DTPOProduct.Rows[i]["Qty"]);
                        double LevelDis = Convert.ToDouble(DTPOProduct.Rows[i]["DiscountPer"]);
                        string[] TempArrProduct = new string[21];
                        string SelQuery = "SELECT * FROM View_ItemPoDetail WHERE View_ItemPoDetail.ID='" + DTPOProduct.Rows[i]["ItemId"] + "'";
                        DataSet DSItemDetails = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
                        BindInnerGVDataCalculation(DSItemDetails, RecQty, OrdQty, LevelDis, 0.0);
                    }
                }
                BindInnerGV();
            }
            else
                ShowMessage("Select PO First", MessageType.Error);
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Search In Receive Item Entry Records
    protected void LBSearch_Click(object sender, EventArgs e)
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "SEARCH");
            arrParam[1] = new SqlParameter("@SearchText", TBSearch.Text);
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveMasterView", arrParam);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                ViewState["ReceiveGrid"] = DSRecords.Tables[0];
                GVPOMaster.DataSource = DSRecords;
                GVPOMaster.DataBind();
            }
            else
            {
                GVPOMaster.ShowHeaderWhenEmpty = true;
                GVPOMaster.EmptyDataText = "No Matched Record Found !";
                GVPOMaster.DataBind();
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    // Method For Get Record Using Date Range
    protected void CBShowInDateRange_CheckedChanged(object sender, EventArgs e)
    {
        if (CBShowInDateRange.Checked && TBFromDate.Text != string.Empty && TBToDate.Text != string.Empty)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@Action", "SEARCH_BW_DATES");
                arrParam[1] = new SqlParameter("@FDate", Convert.ToDateTime(TBFromDate.Text));
                arrParam[2] = new SqlParameter("@TDate", Convert.ToDateTime(TBToDate.Text));
                DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveMaster_CRUD", arrParam);
                if (DSRecords.Tables[0].Rows.Count > 0)
                {
                    ViewState["ReceiveGrid"] = DSRecords.Tables[0];
                    GVPOMaster.DataSource = DSRecords;
                    GVPOMaster.DataBind();
                }
                else
                {
                    GVPOMaster.ShowHeaderWhenEmpty = true;
                    GVPOMaster.EmptyDataText = "No Matched Record Found !";
                    GVPOMaster.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        else
        {
            BindGVReceive();
        }
    }

    // Method For GVReceiveItem Row Command
    protected void GVPOMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int MRecordID = Convert.ToInt32(e.CommandArgument);
        Session["MRecordID"] = MRecordID;

        #region EditRecord

        if (e.CommandName == "EditRecord")
        {
            MVReceivedItem.ActiveViewIndex = 1;
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
            arrParam[1] = new SqlParameter("@ID", MRecordID);
            DataSet DSRecords1 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveMaster_CRUD", arrParam);
            if (DSRecords1.Tables[0].Rows.Count > 0)
            {
                LBLItemEntryNo.Text = DSRecords1.Tables[0].Rows[0]["RecNo"].ToString();
                BindBusinessLocation();
                DDLBusinessLocation.ClearSelection();
                DDLBusinessLocation.Items.FindByValue(DSRecords1.Tables[0].Rows[0]["NGOID"].ToString()).Selected = true; 
                //TBSupplierInvoiceNo.Text = DSRecords1.Tables[0].Rows[0]["SupplierInvoiceNo"].ToString();
                //TBSupplierInvoiceDate.Text = DSRecords1.Tables[0].Rows[0]["SupplierInvoiceDate"].ToString();
                LBLDate.Text = DSRecords1.Tables[0].Rows[0]["RecDate"].ToString();
                TBReceivedOnDate.Text = DSRecords1.Tables[0].Rows[0]["RecDate"].ToString();
                //CBReceivedViaPO.Checked = Convert.ToBoolean(DSRecords1.Tables[0].Rows[0]["RecViaPO"].ToString());
                if (DSRecords1.Tables[0].Rows[0]["PoNo"].ToString() != "0" && DSRecords1.Tables[0].Rows[0]["PoNo"].ToString() != "")
                {
                    BindDDLPO();
                    DDLPO.ClearSelection();
                    DDLPO.Items.FindByValue(DSRecords1.Tables[0].Rows[0]["PoNo"].ToString()).Selected = true;
                }
                else
                {
                    DDLPO.ClearSelection();
                }
                BindSupplier();
                DDLSupplier.ClearSelection();
                DDLSupplier.SelectedValue= DSRecords1.Tables[0].Rows[0]["Supplier"].ToString();
                TBPaymentTerms.Text = DSRecords1.Tables[0].Rows[0]["PaymentTerm"].ToString();
                BindGodown();
                DDLGodown.ClearSelection();
                DDLGodown.Items.FindByValue(DSRecords1.Tables[0].Rows[0]["Godown"].ToString()).Selected = true;
                LBLNoOfPackages.Text = DSRecords1.Tables[0].Rows[0]["TotalQty"].ToString();
                BindEmployee();
                DDLRecEmp.ClearSelection();
                DDLRecEmp.Items.FindByValue(DSRecords1.Tables[0].Rows[0]["receiveBy"].ToString()).Selected = true;
                TBNote.Text = DSRecords1.Tables[0].Rows[0]["Remarks"].ToString();
                TBSubTotalBTax.Text = DSRecords1.Tables[0].Rows[0]["SubTotal"].ToString();
                TBTotalTaxAmount.Text = DSRecords1.Tables[0].Rows[0]["TotalCGST"].ToString();
                TBSGST.Text = DSRecords1.Tables[0].Rows[0]["totalSGST"].ToString();
                TBTotalAmount.Text = DSRecords1.Tables[0].Rows[0]["Amount"].ToString();

                // bind inner gv
                string Query = "SELECT * FROM View_Receivedetail where View_Receivedetail.Rec_Id=" + MRecordID;
                DataSet dsfill = clsConnectionSql.fillds(Query);
                if (dsfill.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsfill.Tables[0].Rows.Count; i++)
                    {
                        string[] DataArray = new string[18];
                        DataArray[0] = dsfill.Tables[0].Rows[0]["ID"].ToString();
                        DataArray[1] = dsfill.Tables[0].Rows[0]["Image"].ToString();
                        DataArray[2] = dsfill.Tables[0].Rows[0]["Barcode"].ToString();
                        DataArray[3] = dsfill.Tables[0].Rows[0]["Name"].ToString();
                        DataArray[4] = dsfill.Tables[0].Rows[0]["UOM"].ToString();
                        DataArray[5] = dsfill.Tables[0].Rows[0]["Rec_Qty"].ToString();
                        DataArray[6] = dsfill.Tables[0].Rows[0]["Order_Qty"].ToString();
                        DataArray[7] = dsfill.Tables[0].Rows[0]["Pending_Qty"].ToString();
                        DataArray[8] = dsfill.Tables[0].Rows[0]["Unit_Peice"].ToString();
                        DataArray[9] = dsfill.Tables[0].Rows[0]["Total_Price"].ToString();
                        DataArray[10] = dsfill.Tables[0].Rows[0]["dis_per"].ToString();
                        DataArray[11] = dsfill.Tables[0].Rows[0]["dis_Value"].ToString();
                        DataArray[12] = dsfill.Tables[0].Rows[0]["TaxableAmt"].ToString();
                        DataArray[13] = dsfill.Tables[0].Rows[0]["cgst_per"].ToString();
                        DataArray[14] = dsfill.Tables[0].Rows[0]["cgst_Amt"].ToString();
                        DataArray[15] = dsfill.Tables[0].Rows[0]["sgst_per"].ToString();
                        DataArray[16] = dsfill.Tables[0].Rows[0]["sgst_amt"].ToString();
                        DataArray[17] = dsfill.Tables[0].Rows[0]["totalAmt"].ToString();
                        //DataArray[19] = SubTotalATax.ToString("00.00");
                        //DataArray[20] = EffRatePerUnit.ToString("00.00");
                        InsertDTTable(DataArray);
                        //TempDT.Rows.Add(DataArray[0], DataArray[1], DataArray[2], DataArray[3], DataArray[4],
                        //  DataArray[5], DataArray[6], DataArray[7], DataArray[8],
                        //  DataArray[9], DataArray[10], DataArray[11], DataArray[12],
                        //  DataArray[13], DataArray[14], DataArray[15], DataArray[16],
                        //  DataArray[17]);
                    }
                    //ViewState["DTTable"] = TempDT;
                    BindInnerGV();
                    btnSubmit.Text = "Update";
                }
               





 
            }
        }
        if (e.CommandName == "DeleteRecord")
        {
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "DELETE");
            arrParam[1] = new SqlParameter("@ID", MRecordID);
            DataSet DSRecords1 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveMaster_CRUD", arrParam);
            SqlParameter[] arrParam1 = new SqlParameter[2];
            arrParam1[0] = new SqlParameter("@Action", "DELETE");
            arrParam1[1] = new SqlParameter("@ID", MRecordID);
            DataSet DSRecords11 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ReceiveMaster_CRUD", arrParam);
            BindGVReceive();
        }
        #endregion

        #region DeleteRecord
        #endregion

    }

    private void BindDDLPO()
    {
        try
        {
            SqlParameter[] arrparm = new SqlParameter[1];
            arrparm[0] = new SqlParameter("@Action", "SELECT");
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_POMaster_CRUD", arrparm);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                DDLPO.DataSource = DSRecords.Tables[0];
                DDLPO.DataTextField = "PONo";
                DDLPO.DataValueField = "ID";
                DDLPO.DataBind();
                DDLPO.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }
}