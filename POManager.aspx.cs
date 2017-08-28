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
using System.Data.OleDb;

public partial class POManager : System.Web.UI.Page
{
    #region Common Fields

    DataSet DSRecords = new DataSet();
    DataTable DTRecords = new DataTable();

    DataTable DTGV = new DataTable();

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
    string[] ItemArray = new string[21];

    // Create / Load DataTable
    private void CreateDTTable()
    {
        TempDT = new DataTable();
        if (TempDT.Columns.Count == 0)
        {
            TempDT.Columns.Add("ID", typeof(int));
            TempDT.Columns.Add("ItemName", typeof(string));
            TempDT.Columns.Add("Barcode", typeof(string));
            TempDT.Columns.Add("HSNCode", typeof(string));
            TempDT.Columns.Add("UOM", typeof(string));
            TempDT.Columns.Add("Quantity", typeof(string));
            TempDT.Columns.Add("UnitPrice", typeof(string));
            TempDT.Columns.Add("TotalPriceInitial", typeof(string));
            TempDT.Columns.Add("DiscountPer", typeof(string));
            TempDT.Columns.Add("DiscountValue", typeof(string));
            TempDT.Columns.Add("UnitPriceWithDiscount", typeof(string));
            TempDT.Columns.Add("TotalPriceWithDiscount", typeof(string));
            //TempDT.Columns.Add("TaxName", typeof(string));
            TempDT.Columns.Add("CGSTPer", typeof(string));
            TempDT.Columns.Add("CGSTValue", typeof(string));
            TempDT.Columns.Add("SGSTPer", typeof(string));
            TempDT.Columns.Add("SGSTValue", typeof(string));         
            TempDT.Columns.Add("UnitPriceWithTaxDiscount", typeof(string));
            TempDT.Columns.Add("TotalPriceWithTaxDiscount", typeof(string));
            TempDT.Columns.Add("EffectiveRatePerUnit", typeof(string));
            TempDT.Columns.Add("GrandTotal", typeof(string));
            TempDT.Columns.Add("RemarkInner", typeof(string));
        }
        ViewState["DTTable"] = TempDT;
    }

    // For Insert Into DataTable
    private void InsertDTTable(string[] ItemArray)
    {
        TempDT = ViewState["DTTable"] as DataTable;
        TempDT.Rows.Add(ItemArray[0], ItemArray[1], ItemArray[2], ItemArray[3], ItemArray[4], ItemArray[5], ItemArray[6], ItemArray[7], ItemArray[8], ItemArray[9], ItemArray[10], ItemArray[11], ItemArray[12], ItemArray[13], ItemArray[14], ItemArray[15], ItemArray[16], ItemArray[17], ItemArray[18], ItemArray[19], ItemArray[20]);
        ViewState["DTTable"] = TempDT;
    }

    // For Select Frome DataTable
    private string[] SelectDTTable(int ID)
    {
        TempDT = ViewState["DTTable"] as DataTable;
        for (int i = TempDT.Rows.Count-1 ; i >= 0; i--)
        {
            DataRow dr = TempDT.Rows[i];
            if ((Int32)dr[0] == ID)
            {
                ItemArray[0] = dr["ID"].ToString();
                ItemArray[1] = dr["ItemName"].ToString();
                ItemArray[2] = dr["Barcode"].ToString();
                ItemArray[3] = dr["HSNCode"].ToString();
                ItemArray[4] = dr["UOM"].ToString();
                ItemArray[5] = dr["Quantity"].ToString();
                ItemArray[6] = dr["UnitPrice"].ToString();
                ItemArray[7] = dr["TotalPriceInitial"].ToString();
                ItemArray[8] = dr["DiscountPer"].ToString();
                ItemArray[9] = dr["DiscountValue"].ToString();
                ItemArray[10] = dr["UnitPriceWithDiscount"].ToString();
                ItemArray[11] = dr["TotalPriceWithDiscount"].ToString();
               // ItemArray[12] = dr["TaxName"].ToString();
                ItemArray[12] = dr["CGSTPer"].ToString();
                ItemArray[13] = dr["CGSTValue"].ToString();
                ItemArray[14] = dr["SGSTPer"].ToString();
                ItemArray[15] = dr["SGSTPer"].ToString();
                //ItemArray[1] = dr["TaxValue"].ToString();
                ItemArray[16] = dr["UnitPriceWithTaxDiscount"].ToString();
                ItemArray[17] = dr["TotalPriceWithTaxDiscount"].ToString();
                ItemArray[18] = dr["EffectiveRatePerUnit"].ToString();
                ItemArray[19] = dr["GrandTotal"].ToString();
                ItemArray[20] = dr["RemarkInner"].ToString();
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
                dr["ItemName"] = ItemArray[1].ToString();
                dr["Barcode"] = ItemArray[2].ToString();
                dr["HSNCode"] = ItemArray[3].ToString();
                dr["UOM"] = ItemArray[4].ToString();
                dr["Quantity"] = ItemArray[5].ToString();
                dr["UnitPrice"] = ItemArray[6].ToString();
                dr["TotalPriceInitial"] = ItemArray[7].ToString();
                dr["DiscountPer"] = ItemArray[8].ToString();
                dr["DiscountValue"] = ItemArray[9].ToString();
                dr["UnitPriceWithDiscount"] = ItemArray[10].ToString(); ;
                dr["TotalPriceWithDiscount"] = ItemArray[11].ToString();
                dr["CGSTPer"] = ItemArray[12].ToString();
                dr["CGSTValue"] = ItemArray[13].ToString();
                dr["SGSTPer"] = ItemArray[14].ToString();
                dr["SGSTValue"] = ItemArray[15].ToString();               
                dr["UnitPriceWithTaxDiscount"] = ItemArray[16].ToString();
                dr["TotalPriceWithTaxDiscount"] = ItemArray[17].ToString();
                dr["EffectiveRatePerUnit"] = ItemArray[18].ToString();
                dr["GrandTotal"] = ItemArray[19].ToString();
                dr["RemarkInner"] = ItemArray[20].ToString();
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
        string[] DataArray = new string[2];
        double Qty = 0.0;
        double value = 0.0;
        TempDT = ViewState["DTTable"] as DataTable;
        for (int i = TempDT.Rows.Count - 1; i >= 0; i--)
        {
            Qty += Convert.ToDouble(TempDT.Rows[i][5]);
            value += Convert.ToDouble(TempDT.Rows[i][19]);
        }
        DataArray[0] = Qty.ToString();
        DataArray[1] = value.ToString();
        return DataArray;
    }

    #endregion
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGVPOMaster();
        }
    }
    
    private void BindGVPOMaster()
    {
        try
        {
            string STR = "SELECT * From View_PO_Details";
            DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, STR);
            if (DSRecords.Tables[0].Rows.Count > 0)
            {
                ViewState["POGrid"] = DSRecords.Tables[0];
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
    
    protected void LBAddNew_Click(object sender, EventArgs e)
    {
        MVPO.ActiveViewIndex = 1;
        BindGeneralDetails();
        CreateDTTable();
        BindInnerGV();
    }
    
    private void BindGeneralDetails()
    {
        GetPOCode();
        LBLPODate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        TBDeliveryDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        BindCompany();
        BindNGO();
        BindDeliveryTo();
        BindSupplierDetail();
        BindProductDetails();
    }

    private void BindCompany()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT");
        DataSet DSCompany = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arrParam);
        DataTable DTCompany = DSCompany.Tables[0];
        if (DTCompany.Rows.Count > 0)
        {
            DDLCompany.DataSource = DTCompany;
            DDLCompany.DataTextField = "CompanyName";
            DDLCompany.DataValueField = "ID";
            DDLCompany.DataBind();
            DDLCompany.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
        }
    }

    private void BindDeliveryTo()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT_ByDefault");
        DataSet DSCompany = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arrParam);
        DataTable DTCompany = DSCompany.Tables[0];
        if (DTCompany.Rows.Count > 0)
        {
            DDLDeliverTo.DataSource = DTCompany;
            DDLDeliverTo.DataTextField = "Name";
            DDLDeliverTo.DataValueField = "ID";
            DDLDeliverTo.DataBind();
            DDLDeliverTo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
        }
    }

    private void BindNGO()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT");
        DataSet DSCompany = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_BusinessLocation_CRUD", arrParam);
        DataTable DTCompany = DSCompany.Tables[0];
        if (DTCompany.Rows.Count > 0)
        {
            DDLRetailOutlet.DataSource = DTCompany;
            DDLRetailOutlet.DataTextField = "BusinessLocationName";
            DDLRetailOutlet.DataValueField = "ID";
            DDLRetailOutlet.DataBind();
            DDLRetailOutlet.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
        }
    }

    private void BindProductDetails()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT");
        DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParam);
        if (DSRecords.Tables[0].Rows.Count > 0)
        {
            DDLItemList.DataSource = DSRecords.Tables[0];
            DDLItemList.DataTextField = "Name";
            DDLItemList.DataValueField = "ID";
            DDLItemList.DataBind();
            DDLItemList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
        }
    }
    
    private void BindSupplierDetail()
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
            DDLSupplier.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
        }
    }
    
    private void GetPOCode()
    {
        try
        {
            string DocumentCode = "PO"; string PrvID = ""; int NextID = 1;
            string GetMaxId = "SELECT PONo From POMaster Order By ID DESC ";
            DataTable dtBill = clsConnectionSql.filldt(GetMaxId);
            if (dtBill.Rows.Count > 0)
            {
                PrvID = dtBill.Rows[0]["PONo"].ToString();
                string[] PrvCon = PrvID.Split('O');
                NextID = Convert.ToInt32(PrvCon[1]) + 1;
                string Zero = new String('0', (5 - NextID.ToString().Length));
                DocumentCode = DocumentCode + Zero + NextID.ToString();
                LBLPONo.Text = DocumentCode.ToString();
            }
            else
                LBLPONo.Text = "PO00001";
        }
        catch (Exception ex)
        {
        }
    }
    
    protected void DDLItemList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string SelQuery = string.Empty;
        if (sender == TBBarcode && TBBarcode.Text != string.Empty)
        {
            SelQuery = "SELECT * FROM View_RowMeterial WHERE View_RowMeterial.Barcode='" + TBBarcode.Text.Trim() + "'";
        }
        else if (sender == DDLItemList && DDLItemList.SelectedIndex > 0)
        {
            SelQuery = "SELECT * FROM View_RowMeterial WHERE View_RowMeterial.ID=" + DDLItemList.SelectedValue + "";
        }

        DataTable DTT = clsConnectionSql.filldt(SelQuery);
        if (DTT.Rows.Count > 0)
        {
            double Quantity = 1.0;
            double DisPer = 0.00;
            string[] DArray = new string[21];
            string[] DataArray = new string[21];
            DArray = SelectDTTable(Convert.ToInt32(DTT.Rows[0]["ID"]));
            if (DArray[0] == DTT.Rows[0]["ID"].ToString())
            {
                Quantity = Convert.ToDouble(DArray[5]) + Quantity;
                DataArray = BindInnerData(DTT, Quantity, DisPer);
                UpdateDTTable(DataArray[0], DataArray);
            }
            else
            {
                DataArray = BindInnerData(DTT, Quantity, DisPer);
                InsertDTTable(DataArray);
            }
            BindInnerGV();
        }
        DDLItemList.ClearSelection();

    }
    
    private string[] BindInnerData(DataTable DTT, double Quantity, double DisPer)
    {
        double PurPrice = Convert.ToDouble(DTT.Rows[0]["PurPrice"]);
        double PurTax = Convert.ToDouble(DTT.Rows[0]["TaxPer"]);
        double TaxValue = (PurTax / 100) * PurPrice;
        double UnitPriceBTax = (100 / (100 + PurTax)) * PurPrice;

        string[] RecArray = new string[21];
        RecArray[0] = DTT.Rows[0]["ID"].ToString();
        RecArray[1] = DTT.Rows[0]["Name"].ToString();
        RecArray[2] = DTT.Rows[0]["Barcode"].ToString();
        RecArray[3] = DTT.Rows[0]["HSNCode"].ToString();
        RecArray[4] = DTT.Rows[0]["UOM"].ToString();
        RecArray[5] = Quantity.ToString("00.00");
        RecArray[6] = PurPrice.ToString("00.00");
        RecArray[7] = (Convert.ToDouble(RecArray[6]) * Convert.ToDouble(RecArray[5])).ToString("00.00");
        RecArray[8] = DisPer.ToString("00.00");
        RecArray[9] = ((Convert.ToDouble(RecArray[7]) * Convert.ToDouble(RecArray[8])) / 100).ToString("00.00");
        RecArray[10] = (Convert.ToDouble(RecArray[7]) - Convert.ToDouble(RecArray[9])).ToString("00.00");
        RecArray[11] = "0.00";
     //  RecArray[13] = DTT.Rows[0]["PurchaseTaxName"].ToString();
        RecArray[12] = ((Convert.ToDouble(PurTax)/2).ToString("00.00"));
        RecArray[13] = ((Convert.ToDouble(RecArray[10]) * Convert.ToDouble(RecArray[12])) / 100).ToString("00.00");
        RecArray[14] = ((Convert.ToDouble(PurTax) / 2).ToString("00.00"));
        RecArray[15] = ((Convert.ToDouble(RecArray[10]) * Convert.ToDouble(RecArray[12])) / 100).ToString("00.00");
        RecArray[16] = (Convert.ToDouble(RecArray[10]) + Convert.ToDouble(RecArray[13]) + Convert.ToDouble(RecArray[15])).ToString("00.00");
        RecArray[17] = "00.00";
        RecArray[18] = Convert.ToDouble(RecArray[16]).ToString("00.00");
        RecArray[19] = Convert.ToDouble(RecArray[16]).ToString("00.00");
        RecArray[20] = "00.00";
     
        return RecArray;
    }

    private void BindInnerGV()
    {
        GVPO.DataSource = ViewState["DTTable"] as DataTable;
        GVPO.DataBind();
        InnerGVData();
    }

    protected void InnerGVData()
    {
        try
        {
            string[] array = new string[2];
            array = TotalDetails();
            LBLTotalQuantity.Text = Convert.ToDouble(array[0]).ToString("00.00");
            LBLTotalValues.Text = Convert.ToDouble(array[1]).ToString("00.00");
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void BindInnerData2(object sender, EventArgs e)
    {
        for (int i = 0; i < GVPO.Rows.Count; i++)
        {
            TextBox TBQty = (TextBox)GVPO.Rows[i].FindControl("TBItemQuantity");
            TextBox TBDisPer = (TextBox)GVPO.Rows[i].FindControl("TBDisPer");
            HiddenField HFIDQuantity = (HiddenField)GVPO.Rows[i].FindControl("HFID");
            string[] StrArray = new string[21];
            StrArray = SelectDTTable(Convert.ToInt32(HFIDQuantity.Value));
            double TaxValue = Convert.ToDouble(StrArray[13]);
            double RecQty = Convert.ToDouble(TBQty.Text);
            double LevelDisPer = Convert.ToDouble(TBDisPer.Text);
            StrArray[5] = RecQty.ToString("00.00");
            StrArray[7] = (Convert.ToDouble(StrArray[6]) * Convert.ToDouble(StrArray[5])).ToString("00.00");
            StrArray[8] = LevelDisPer.ToString("00.00");
            StrArray[9] = ((Convert.ToDouble(StrArray[7]) * Convert.ToDouble(StrArray[8])) / 100).ToString("00.00");
            StrArray[10] = (Convert.ToDouble(StrArray[7]) - Convert.ToDouble(StrArray[9])).ToString("00.00");
            StrArray[11] = "00.00";
            StrArray[12] = (Convert.ToDouble(StrArray[12]).ToString("00.00"));
            StrArray[13] = ((Convert.ToDouble(StrArray[10]) * Convert.ToDouble(StrArray[12])) / 100).ToString("00.00");
            StrArray[14] = (Convert.ToDouble(StrArray[14]).ToString("00.00"));
            StrArray[15] = ((Convert.ToDouble(StrArray[10]) * Convert.ToDouble(StrArray[12])) / 100).ToString("00.00");
            StrArray[16] = (Convert.ToDouble(StrArray[10]) + Convert.ToDouble(StrArray[13]) + Convert.ToDouble(StrArray[15])).ToString("00.00");
            StrArray[17] = "00.00";
            StrArray[18] = Convert.ToDouble(StrArray[16]).ToString("00.00");
            StrArray[19] = Convert.ToDouble(StrArray[16]).ToString("00.00");
       
            UpdateDTTable(Convert.ToInt32(HFIDQuantity.Value), StrArray);
        }
        BindInnerGV();
    }

    protected void BottomBtnControls(object sender, EventArgs e)
    {
        int RecID;
        if (sender == LBSubmitPrint)
        {
            RecID = SubmitPORecord();
            PrintPORecord(RecID);
        }
        else if (sender == btnSubmit)
        {
            RecID = SubmitPORecord();
            ClearRecord();
            MVPO.ActiveViewIndex = 0;
            BindGVPOMaster();
        }
        else if (sender == LBCancel)
        {
            ClearRecord();
            TempDT.Columns.Clear();
            MVPO.ActiveViewIndex = 0;
        }
    }

    private void PrintPORecord(int RecID)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "PrintPage()", true);
        //string ss = ((LinkButton)sender).CommandArgument;

        string SelPoDetail = "SELECT * FROM POMaster WHERE (ID = '" + RecID + "' )";
        DataSet DSPODetail = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelPoDetail);
        if (DSPODetail.Tables[0].Rows.Count > 0)
        {
            // Get Company Detail
            string SelCompany = "SELECT * FROM Company";
            DataSet DSCompanyDetails = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelCompany);

            // Get Business Location
            string SelBL = "SELECT * FROM View_BusinessLocationDetails WHERE View_BusinessLocationDetails.ID='" + DSPODetail.Tables[0].Rows[0]["BusinessLocation"] + "'";
            DataSet DSBL = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelBL);

            // Get Supplier 
            string SelSupplier = "SELECT * FROM View_SupplierDetails WHERE View_SupplierDetails.ID='" + DSPODetail.Tables[0].Rows[0]["Supplier"] + "'";
            DataSet DSSupplier = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelSupplier);

            // Get Delivery Location
            string SelDL = "SELECT * FROM View_BusinessLocationDetails WHERE View_BusinessLocationDetails.ID='" + DSPODetail.Tables[0].Rows[0]["DelivarTo"] + "'";
            DataSet DSDL = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelDL);

            if (DSBL.Tables[0].Rows.Count > 0)
            {
                lblCompanyName.Text = DSBL.Tables[0].Rows[0]["BusinessLocationName"].ToString();
                lblCompanyAddressLine1.Text = DSBL.Tables[0].Rows[0]["Address1"].ToString();
                lblCompanyAddressLine2.Text = DSBL.Tables[0].Rows[0]["Address2"].ToString();
                lblCompanyCity.Text = DSBL.Tables[0].Rows[0]["City"].ToString();
                lblCompanyState.Text = DSBL.Tables[0].Rows[0]["State"].ToString();
                lblCompanyPin.Text = DSBL.Tables[0].Rows[0]["ZipCode"].ToString();
                lblCompanyPhon.Text = DSBL.Tables[0].Rows[0]["MobileNumber"].ToString();
            }

            if (DSDL.Tables[0].Rows.Count > 0)
            {
                lblShipName.Text = DSDL.Tables[0].Rows[0]["BusinessLocationName"].ToString();
                lblShipCompany.Text = DSDL.Tables[0].Rows[0]["CompanyName"].ToString();
                lblShipAddress1.Text = DSDL.Tables[0].Rows[0]["Address1"].ToString();
                lblShipAddress2.Text = DSDL.Tables[0].Rows[0]["Address2"].ToString();
                lblShipCity.Text = DSDL.Tables[0].Rows[0]["City"].ToString();
                lblShipState.Text = DSDL.Tables[0].Rows[0]["State"].ToString();
                lblShipZip.Text = DSDL.Tables[0].Rows[0]["MobileNumber"].ToString();
            }

            if (DSSupplier.Tables[0].Rows.Count > 0)
            {
                lblToName.Text = DSSupplier.Tables[0].Rows[0]["Name"].ToString();
                lblToCompany.Text = DSSupplier.Tables[0].Rows[0]["Name"].ToString();
                lblToAddress1.Text = DSSupplier.Tables[0].Rows[0]["Address1"].ToString();
                lblToAddress2.Text = DSSupplier.Tables[0].Rows[0]["Address2"].ToString();
                lblToCity.Text = DSSupplier.Tables[0].Rows[0]["City"].ToString();
                lblToState.Text = DSSupplier.Tables[0].Rows[0]["State"].ToString();
                lblToZip.Text = DSSupplier.Tables[0].Rows[0]["ZipCode"].ToString();
                lblToPhon.Text = DSSupplier.Tables[0].Rows[0]["MobileNumber"].ToString();
            }

            string SelItem = "SELECT * FROM POProducts WHERE POID='" + DSPODetail.Tables[0].Rows[0]["ID"] + "'";
            DataSet DSItem = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelItem);

            double TTotal = 0.00;
            double SellTax = 0.00;

            if (DSItem.Tables[0].Rows.Count > 0)
            {
                DataTable DTPP = new DataTable();
                DTPP.Columns.Add("ID");
                DTPP.Columns.Add("Quantity");
                DTPP.Columns.Add("ProductID");
                DTPP.Columns.Add("UnitPriceWithTaxDiscount");
                DTPP.Columns.Add("TotalPriceWithTaxDiscount");
                for (int i = 0; i < DSItem.Tables[0].Rows.Count; i++)
                {
                    string SelQuery = "SELECT * FROM View_ItemDetails Where View_ItemDetails.ID='" + DSItem.Tables[0].Rows[i]["ProductID"] + "'";
                    DataTable DTProduct = clsConnectionSql.filldt(SelQuery);
                    for (int j = 0; j < DTProduct.Rows.Count; j++)
                    {
                        double QTY = Convert.ToDouble(DSItem.Tables[0].Rows[i]["Quantity"].ToString());
                        double UnitPrice = Convert.ToDouble(DTProduct.Rows[j]["BasicOriginalPrice"].ToString());
                        double TotalPrice = UnitPrice * QTY;
                        double DisPer = Convert.ToDouble(DSItem.Tables[0].Rows[i]["DiscountPer"].ToString());
                        double DisValue = (UnitPrice * DisPer) / 100;
                        double UnitPriceWDis = UnitPrice - DisValue;
                        double TotalUnitPriceWDis = UnitPriceWDis * QTY;
                        double TaxPer = Convert.ToDouble(DTProduct.Rows[j]["PurchaseTaxValue"].ToString());
                        double TaxValue = (UnitPriceWDis * TaxPer) / 100;
                        double TotalTaxValue = TaxValue * QTY;
                        double UnitPriceWTDis = UnitPriceWDis + TaxValue;
                        double TotalPriceWTDis = UnitPriceWTDis * QTY;
                        double GTotal = TotalPriceWTDis;

                        TTotal += TotalUnitPriceWDis;
                        SellTax += TotalTaxValue;

                        DTPP.Rows.Add(j + 1, QTY.ToString("00.00"), DTProduct.Rows[j]["ItemName"], UnitPrice.ToString("00.00"), GTotal.ToString("00.00"));
                    }
                }
                GVInvPrint.DataSource = DTPP;
                GVInvPrint.DataBind();

                lblSubtotal.Text = TTotal.ToString("00.00");
                lblSalesTax.Text = SellTax.ToString("00.00");

                lblTotal.Text = (Convert.ToDouble(lblSubtotal.Text) + Convert.ToDouble(lblSalesTax.Text) + Convert.ToDouble(lblShipping.Text) + Convert.ToDouble(lblOther.Text)).ToString("00.00");

                DetailID.InnerText = DSPODetail.Tables[0].Rows[0]["Remark"].ToString();
                lblPONumber.Text = DSPODetail.Tables[0].Rows[0]["PONo"].ToString();
            }
        }
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename='" + DSPODetail.Tables[0].Rows[0]["PONo"].ToString() + "'.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        PrintPODocument.RenderControl(hw);
        StringReader sr = new StringReader(sw.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();
        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.End();
    }

    private int SubmitPORecord()
    {
        int MID = 0;
        if (btnSubmit.Text == "Submit")
        {
            #region Insert PO Master Details
            SqlParameter[] arrParam = new SqlParameter[16];
            arrParam[0] = new SqlParameter("@Action", "INSERT");
            arrParam[1] = new SqlParameter("@ComID", DDLCompany.SelectedValue);
            arrParam[2] = new SqlParameter("@NGOID", DDLRetailOutlet.SelectedValue);
            arrParam[3] = new SqlParameter("@PONo", LBLPONo.Text);
            arrParam[4] = new SqlParameter("@PODate", LBLPODate.Text);
            arrParam[5] = new SqlParameter("@DeliveryDate", TBDeliveryDate.Text);
            arrParam[6] = new SqlParameter("@DeliverTo", DDLDeliverTo.SelectedValue);
            arrParam[7] = new SqlParameter("@Supplier", DDLSupplier.SelectedValue);
            arrParam[8] = new SqlParameter("@SupplierRef", TBSupplierRefrence.Text);
            arrParam[9] = new SqlParameter("@TermsCondition", TBTermsCondition.Text);
            arrParam[10] = new SqlParameter("@PaymentTerms", TBPaymentTerms.Text);
            arrParam[11] = new SqlParameter("@TotalQty", LBLTotalQuantity.Text);
            arrParam[12] = new SqlParameter("@TotalValue", LBLTotalValues.Text);
            arrParam[13] = new SqlParameter("@Remark", TBRemarks.Text);
            arrParam[14] = new SqlParameter("@POStatus", "0");
            arrParam[15] = new SqlParameter("@CreatedBy", Session["UserName"].ToString());
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_POMaster_CRUD", arrParam);

            #endregion

            //Get Master Data ID

            string SelQuery = "SELECT TOP(1) ID FROM POMaster ORDER BY ID DESC";
            DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                MID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }

            #region Insert PO Product Details
            TempDT = ViewState["DTTable"] as DataTable;
            for (int i = 0; i < TempDT.Rows.Count; i++)
            {
                DataRow dr = TempDT.Rows[i];
                SqlParameter[] arrParamDetails = new SqlParameter[11];
                arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                arrParamDetails[1] = new SqlParameter("@POID", MID);
                arrParamDetails[2] = new SqlParameter("@ItemID", dr[0]);
                arrParamDetails[3] = new SqlParameter("@Qty", dr[5]);
                arrParamDetails[4] = new SqlParameter("@DiscountPer", dr[8]);
                arrParamDetails[5] = new SqlParameter("@UnitPrice", dr[10]);
                arrParamDetails[6] = new SqlParameter("@CGSTPer", dr[12]);
                arrParamDetails[7] = new SqlParameter("@CGSTValue", dr[13]);
                arrParamDetails[8] = new SqlParameter("@SGSTPer", dr[14]);
                arrParamDetails[9] = new SqlParameter("@SGSTValue", dr[15]);
                arrParamDetails[10] = new SqlParameter("@TotalPrice", dr[16]);

                int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_PODetail_CRUD", arrParamDetails);

            
            
            }
            #endregion
        }
        else if (btnSubmit.Text == "Update")
        {
            #region Update PO Master Details
            SqlParameter[] arrParam = new SqlParameter[16];
            arrParam[0] = new SqlParameter("@Action", "UPDATE");
            arrParam[1] = new SqlParameter("@ComID", DDLCompany.SelectedValue);
            arrParam[2] = new SqlParameter("@NGOID", DDLRetailOutlet.SelectedValue);
            arrParam[3] = new SqlParameter("@PONo", LBLPONo.Text);
            arrParam[4] = new SqlParameter("@PODate", LBLPODate.Text);
            arrParam[5] = new SqlParameter("@NGOID", DDLRetailOutlet.SelectedValue);
            arrParam[6] = new SqlParameter("@DeliveryDate", TBDeliveryDate.Text);
            arrParam[7] = new SqlParameter("@DeliverTo", DDLDeliverTo.SelectedValue);
            arrParam[8] = new SqlParameter("@Supplier", DDLSupplier.SelectedValue);
            arrParam[9] = new SqlParameter("@SupplierRef", TBSupplierRefrence.Text);
            arrParam[10] = new SqlParameter("@TermsCondition", TBTermsCondition.Text);
            arrParam[11] = new SqlParameter("@PaymentTerms", TBPaymentTerms.Text);
            arrParam[12] = new SqlParameter("@TotalQty", LBLTotalQuantity.Text);
            arrParam[13] = new SqlParameter("@TotalValue", LBLTotalValues.Text);
            arrParam[14] = new SqlParameter("@Remark", TBRemarks.Text);
            arrParam[15] = new SqlParameter("@ID", Convert.ToInt32(Session["MRecordID"]));
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_POMaster_CRUD", arrParam);
            #endregion

            MID = Convert.ToInt32(Session["MRecordID"]);

            // Delete All Records From POProducts Details
            SqlParameter[] arrParamDel = new SqlParameter[2];
            arrParamDel[0] = new SqlParameter("@Action", "DELETE_ByMasterID");
            arrParamDel[1] = new SqlParameter("@POID", Convert.ToInt32(Session["MRecordID"]));
            int Delresult = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_PODetail_CRUD", arrParamDel);

            #region Insert PO Product Details
            TempDT = ViewState["DTTable"] as DataTable;
            for (int i = 0; i < TempDT.Rows.Count; i++)
            {
                DataRow dr = TempDT.Rows[i];
                SqlParameter[] arrParamDetails = new SqlParameter[11];
                arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                arrParamDetails[1] = new SqlParameter("@POID", Convert.ToInt32(Session["MRecordID"]));
                arrParamDetails[2] = new SqlParameter("@ItemID", dr[0]);
                arrParamDetails[3] = new SqlParameter("@Qty", dr[5]);
                arrParamDetails[4] = new SqlParameter("@DiscountPer", dr[8]);
                arrParamDetails[5] = new SqlParameter("@UnitPrice", dr[10]);
                arrParamDetails[6] = new SqlParameter("@CGSTPer", dr[12]);
                arrParamDetails[7] = new SqlParameter("@CGSTValue", dr[13]);
                arrParamDetails[8] = new SqlParameter("@SGSTPer", dr[14]);
                arrParamDetails[9] = new SqlParameter("@SGSTValue", dr[15]);
                arrParamDetails[10] = new SqlParameter("@TotalPrice", dr[16]);

                int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_PODetail_CRUD", arrParamDetails);
            }
            #endregion
            btnSubmit.Text = "Submit";
        }
        return MID;

    }

    private void ClearRecord()
    {
        DDLRetailOutlet.ClearSelection();
        TBDeliveryDate.Text = string.Empty;
        DDLSupplier.ClearSelection();
        DDLDeliverTo.ClearSelection();
        TBSupplierRefrence.Text = string.Empty;
        TBTermsCondition.Text = string.Empty;
        TBPaymentTerms.Text = string.Empty;
        LBLTotalQuantity.Text = "0.00";
        LBLTotalValues.Text = "0.00";
        TBRemarks.Text = string.Empty;
        btnSubmit.Text = "Submit";
    }

    protected void GVPOMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int MRecordID = Convert.ToInt32(e.CommandArgument);
        Session["MRecordID"] = MRecordID;

        #region EditRecord

        if (e.CommandName == "EditRecord")
        {
            MVPO.ActiveViewIndex = 1;
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
            arrParam[1] = new SqlParameter("@ID", MRecordID);
            DataSet DSRecords1 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_POMaster_CRUD", arrParam);
            if (DSRecords1.Tables[0].Rows.Count > 0)
            {
                LBLPONo.Text = DSRecords1.Tables[0].Rows[0]["PONo"].ToString();
                LBLPODate.Text = DSRecords1.Tables[0].Rows[0]["PODate"].ToString();
                BindCompany();
                BindNGO();
                BindDeliveryTo();
                DDLCompany.ClearSelection();
                DDLCompany.Items.FindByValue(DSRecords1.Tables[0].Rows[0]["ComID"].ToString()).Selected = true;
                DDLRetailOutlet.ClearSelection();
                DDLRetailOutlet.Items.FindByValue(DSRecords1.Tables[0].Rows[0]["NGOID"].ToString()).Selected = true;
                TBDeliveryDate.Text = DSRecords1.Tables[0].Rows[0]["DeliveryDate"].ToString();
                DDLDeliverTo.ClearSelection();
                DDLDeliverTo.Items.FindByValue(DSRecords1.Tables[0].Rows[0]["DeliverTo"].ToString()).Selected = true;
                BindSupplierDetail();
                DDLSupplier.ClearSelection();
                DDLSupplier.Items.FindByValue(DSRecords1.Tables[0].Rows[0]["Supplier"].ToString()).Selected = true;
                TBSupplierRefrence.Text = DSRecords1.Tables[0].Rows[0]["SupplierRef"].ToString();
                TBTermsCondition.Text = DSRecords1.Tables[0].Rows[0]["TermsCondition"].ToString();
                TBPaymentTerms.Text = DSRecords1.Tables[0].Rows[0]["PaymentTerms"].ToString();
                LBLTotalQuantity.Text = DSRecords1.Tables[0].Rows[0]["TotalQty"].ToString();
                LBLTotalValues.Text = DSRecords1.Tables[0].Rows[0]["TotalValue"].ToString();
                TBRemarks.Text = DSRecords1.Tables[0].Rows[0]["Remark"].ToString();
                BindProductDetails();
            }
            SqlParameter[] arrParam2 = new SqlParameter[2];
            arrParam2[0] = new SqlParameter("@Action", "SELECT_ByMID");
            arrParam2[1] = new SqlParameter("@POID", MRecordID);
            DataSet DSRecords2 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_PODetail_CRUD", arrParam2);
            // Bind Table
            CreateDTTable();
            TempDT = ViewState["DTTable"] as DataTable;
            for (int i = 0; i < DSRecords2.Tables[0].Rows.Count; i++)
            {
                string SelQuery = "SELECT * FROM View_ItemDetails Where View_ItemDetails.ID='" + DSRecords2.Tables[0].Rows[i]["ItemID"] + "'";
                DataTable DTProduct = clsConnectionSql.filldt(SelQuery);

                double ItemPrice, Quantity, DisPer, DisValue, UnitPriceWDis, TotalPriceWDis, TaxPer, TaxValue, UnitPriceWTDis, TotalPriceWTDis, EffRatePUnit, GTotal;
                Quantity = Convert.ToDouble(DSRecords2.Tables[0].Rows[i]["Qty"].ToString());
                ItemPrice = Convert.ToDouble(DTProduct.Rows[0]["PurPrice"].ToString());
                DisPer = Convert.ToDouble(DSRecords2.Tables[0].Rows[i]["DiscountPer"].ToString());
                DisValue = (ItemPrice * DisPer) / 100;
                UnitPriceWDis = ItemPrice - DisValue;
                TotalPriceWDis = UnitPriceWDis * Quantity;
                TaxPer = Convert.ToDouble(DTProduct.Rows[0]["TaxPer"].ToString());
                TaxValue = (UnitPriceWDis * TaxPer) / 100;
                UnitPriceWTDis = UnitPriceWDis + TaxValue;
                TotalPriceWTDis = UnitPriceWTDis * Quantity;
                EffRatePUnit = UnitPriceWTDis;
                GTotal = TotalPriceWTDis;


                TempDT.Rows.Add(DTProduct.Rows[0]["ID"],
                DTProduct.Rows[0]["Name"].ToString(),
                DTProduct.Rows[0]["Barcode"].ToString(),
                DTProduct.Rows[0]["HSNCode"].ToString(),
                DTProduct.Rows[0]["UOM"].ToString(),
                Quantity.ToString("0.00"),
                ItemPrice.ToString("0.00"),
                (ItemPrice * Quantity).ToString("0.00"),
                DisPer.ToString("0.00"),
                DisValue.ToString("0.00"),
                UnitPriceWDis.ToString("0.00"),
                TotalPriceWDis.ToString("0.00"),
                (TaxPer / 2).ToString("0.00"),
                (TaxValue / 2).ToString("0.00"),
                (TaxPer / 2).ToString("0.00"),
                (TaxValue / 2).ToString("0.00"),
                UnitPriceWTDis.ToString("0.00"),
                TotalPriceWTDis.ToString("0.00"),
                EffRatePUnit.ToString("0.00"),
                GTotal.ToString("0.00"),
                0);
            }
            GVPO.DataSource = TempDT;
            GVPO.DataBind();
            ViewState["DTTable"] = TempDT;
            btnSubmit.Text = "Update";
        }
        #endregion

        #region DeleteRecord
        if (e.CommandName == "DeleteRecord")
        {
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "DELETE");
            arrParam[1] = new SqlParameter("@POID", MRecordID);
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_PODetail_CRUD", arrParam);

            SqlParameter[] arrParam1 = new SqlParameter[2];
            arrParam1[0] = new SqlParameter("@Action", "DELETE");
            arrParam1[1] = new SqlParameter("@ID", MRecordID);
            int result1 = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_POMaster_CRUD", arrParam1);

            BindGVPOMaster();
        }
        #endregion

        #region Print PO
        if (e.CommandName == "PrintPO")
        {
            PrintPORecord(MRecordID);
        }
        #endregion
    }

    protected void GVPO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int IRecordID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "DeleteRecord")
        {
            DeletDTTable(IRecordID);
            BindInnerGV();
        }
    }

    // Import & Upload Section From Here.........................

    protected void LBExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string fileName = "PO_Report" + System.DateTime.Now + ".xls";
            string Extension = ".xls";
            if (Extension == ".xls")
            {
                for (int i = 0; i < GVPOMaster.Columns.Count; i++)
                {
                    GVPOMaster.Columns[i].Visible = true;
                }

                BindGVPOMaster();

                clsConnectionSql.PrepareControlForExport(GVPOMaster);
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
                            table.GridLines = GVPOMaster.GridLines;

                            // var logo = iTextSharp.text.Image.GetInstance(Server.MapPath(dsss.Tables[0].Rows[0][0].ToString()));
                            //add the header row to the table
                            if (GVPOMaster.HeaderRow != null)
                            {
                                clsConnectionSql.PrepareControlForExport(GVPOMaster.HeaderRow);
                                table.Rows.Add(GVPOMaster.HeaderRow);
                            }

                            //add each of the data rows to the table
                            foreach (GridViewRow row in GVPOMaster.Rows)
                            {
                                clsConnectionSql.PrepareControlForExport(row);
                                table.Rows.Add(row);
                            }

                            //add the footer row to the table
                            if (GVPOMaster.FooterRow != null)
                            {
                                clsConnectionSql.PrepareControlForExport(GVPOMaster.FooterRow);
                                table.Rows.Add(GVPOMaster.FooterRow);
                            }
                            // adding this code
                            //string str = "select * from CompanyInfo";
                            //DataSet dsss = clsConnectionSql.fillds(str);
                            //if (dsss.Tables[0].Rows.Count > 0)
                            //{

                            //}
                            //string a = ddlcom.SelectedItem.Text;
                            //render the table into the htmlwriter
                            GVPOMaster.GridLines = GridLines.Both;
                            table.RenderControl(htw);
                            //string headerTable = @"<Table><tr><td></td><td></td><td></td><td></td><td width='50' height='10' ><img src='" + Server.MapPath(dsss.Tables[0].Rows[0][0].ToString()) + "' width='150' height='50'/></td></tr><tr><td></td></tr><tr><td></td></tr></Table>";
                            //Response.Write(headerTable);
                            //string header2table = @"<Table><tr><td></td><td></td><td></td><td></td><td>" + a + " </td></tr><tr><td></td></tr><tr><td></td></tr></Table>";
                            //Response.Write(header2table);
                            //string header1Table = @"<Table><tr><td></td><td></td><td></td><td></td><td>Attendance Report</td></tr><tr><td></td></tr><tr><td></td></tr></Table>";
                            //Response.Write(header1Table);
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
            MVPO.ActiveViewIndex = 0;
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
                        FillPOMaster(dt_sheet);
                        //GridView1.DataSource = dt_sheet;
                        //GridView1.DataBind();
                        break;
                    case 1:
                        FillPODetails(dt_sheet);
                        //GridView2.DataSource = dt_sheet;
                        //GridView2.DataBind();
                        break;
                }
                i++;
            }
        }
    }

    private void FillPOMaster(DataTable dt_sheet)
    {
        int BL = 0, DT = 0, Supp = 0;
        if (dt_sheet.Rows.Count > 0)
        {
            for (int i = 0; i < dt_sheet.Rows.Count; i++)
            {
                //get BL Id
                string getcomid = "Select ID From BusinessLocation Where BusinessLocationName='" + dt_sheet.Rows[i]["Business Location"] + "'";
                DataTable DTComid = clsConnectionSql.filldt(getcomid);
                if (DTComid.Rows.Count > 0)
                {
                    BL = Convert.ToInt32(DTComid.Rows[0][0]);
                }

                //Get DT Id
                string getbranchid = "Select ID From BusinessLocation Where BusinessLocationName='" + dt_sheet.Rows[i]["Delivar To"] + "' ";
                DataTable DTgetbranchid = clsConnectionSql.filldt(getbranchid);
                if (DTgetbranchid.Rows.Count > 0)
                {
                    DT = Convert.ToInt32(DTgetbranchid.Rows[0][0]);
                }

                //Get Supplier Id
                string getdepid = "Select ID From Supplier Where Name='" + dt_sheet.Rows[i]["Supplier"] + "'";
                DataTable DTDepid = clsConnectionSql.filldt(getdepid);
                if (DTDepid.Rows.Count > 0)
                {
                    Supp = Convert.ToInt32(DTDepid.Rows[0][0]);
                }

                //Get PO Id
                string chkempid = "Select PONo From POMaster Where PONo='" + dt_sheet.Rows[i][0] + "'";
                DataTable DTPONO = clsConnectionSql.filldt(chkempid);
                int PONo;
                if (DTPONO.Rows.Count > 0)
                {
                    PONo = Convert.ToInt32(DTPONO.Rows[0][0]);
                    //Process For Update Emp Work Details
                }
                else
                {
                    //Process for Insert New Emp Work Details
                    string insertPOMaster = "Insert into POMaster (PONo,PODate,BusinessLocation,DelivaryDate,DelivarTo,Supplier, ";
                    insertPOMaster += " SupplierRefrence,TermsConditions,PaymentTerms,TotalQuantity,TotalValue,Remark,POStatus, ";
                    insertPOMaster += "CreatedBy) Values ('" + dt_sheet.Rows[i][0] + "','" + dt_sheet.Rows[i][1] + "','" + BL + "','"
                        + dt_sheet.Rows[i][3] + "','" + DT + "','" + Supp + "','" + dt_sheet.Rows[i][6] + "','"
                        + dt_sheet.Rows[i][7] + "','" + dt_sheet.Rows[i][8] + "','" + dt_sheet.Rows[i][9] + "','" + dt_sheet.Rows[i][10] + "','"
                        + dt_sheet.Rows[i][11] + "','" + dt_sheet.Rows[i][12] + "','" + dt_sheet.Rows[i][13] + "')";
                    clsConnectionSql.IUD(insertPOMaster);
                }
            }
        }
    }

    private void FillPODetails(DataTable dt_sheet)
    {
        int PN = 0;
        if (dt_sheet.Rows.Count > 0)
        {
            for (int i = 0; i < dt_sheet.Rows.Count; i++)
            {
                //Get Product Id
                string getGenShift = "Select ID From Item Where Name='" + dt_sheet.Rows[i][1] + "' ";
                DataTable DTGenShift = clsConnectionSql.filldt(getGenShift);
                if (DTGenShift.Rows.Count > 0)
                {
                    PN = Convert.ToInt32(DTGenShift.Rows[0][0]);
                }

                //Get PO Id
                string chkempid = "Select ID From POMaster Where PONo='" + dt_sheet.Rows[i][0] + "'";
                DataTable dtEmpCode = clsConnectionSql.filldt(chkempid);
                int POID;
                if (dtEmpCode.Rows.Count > 0)
                {
                    POID = Convert.ToInt32(dtEmpCode.Rows[0][0]);

                    //Get Record Related to EmpID From EmpShiftManage
                    string chkExRecord = "Select POID From POProducts Where POID='" + POID + "'";
                    DataTable ExeEmpRecord = clsConnectionSql.filldt(chkExRecord);
                    if (ExeEmpRecord.Rows.Count > 0)
                    {
                        //Update Record Using New Data
                        string InsertPOProducts = "Insert into POProducts(POID,ProductID,Quantity,DiscountPer) Values ('" + POID + "','" + PN + "','" + dt_sheet.Rows[i][2] + "','" + dt_sheet.Rows[i][3] + "')";
                        clsConnectionSql.IUD(InsertPOProducts);
                    }
                    else
                    {
                        //Insert New Record
                        string InsertPOProducts = "Insert into POProducts(POID,ProductID,Quantity,DiscountPer) Values ('" + POID + "','" + PN + "','" + dt_sheet.Rows[i][2] + "','" + dt_sheet.Rows[i][3] + "')";
                        clsConnectionSql.IUD(InsertPOProducts);
                    }
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
        MVPO.ActiveViewIndex = 2;
    }

    protected void LBFile_Click(object sender, EventArgs e)
    {
        string filename = "~/Uploader/POUploader.xlsx";
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

    protected void SearchEvent(object sender, EventArgs e)
    {
        string SelQuery = "SELECT dbo.POMaster.*,dbo.Supplier.Name,dbo.BusinessLocation.BusinessLocationName,";
        SelQuery += " BusinessLocation_1.BusinessLocationName AS Expr1 FROM dbo.POMaster INNER JOIN ";
        SelQuery += " dbo.Supplier ON dbo.POMaster.Supplier = dbo.Supplier.ID LEFT OUTER JOIN ";
        SelQuery += " dbo.BusinessLocation ON dbo.POMaster.BusinessLocation = dbo.BusinessLocation.ID ";
        SelQuery += " LEFT OUTER JOIN  dbo.BusinessLocation AS BusinessLocation_1 ON dbo.POMaster.DelivarTo = BusinessLocation_1.ID";
        SelQuery += " WHERE dbo.POMaster.PONo LIKE '" + TBSearch.Text + "%'";

        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GVPOMaster.DataSource = ds.Tables[0];
            GVPOMaster.DataBind();
        }
        else
        {
            GVPOMaster.EmptyDataText = "No Record Found !!";
            GVPOMaster.DataBind();
        }
    }

    protected void SuppChangeEvent(object sender, EventArgs e)
    {
        if (DDLSupplier.SelectedIndex > 0)
        {
            string Query = "SELECT * FROM Supplier WHERE ID='" + DDLSupplier.SelectedValue + "'";
            DataSet DSSupp = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, Query);
            if (DSSupp.Tables[0].Rows.Count > 0)
            {
                TBPaymentTerms.Text = DSSupp.Tables[0].Rows[0]["PaymentTerms"].ToString() + "Days";
            }
            else
                TBPaymentTerms.Text = string.Empty;
        }
    }

}