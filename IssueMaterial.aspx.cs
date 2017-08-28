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

public partial class IssueMaterial : System.Web.UI.Page
{
    #region Dynamic DataTable Section

    // Basic Data
    DataTable TempDT = new DataTable();
    string[] ItemArray = new string[11];

    // Create / Load DataTable
    private void CreateDTTable()
    {
        TempDT = new DataTable();
        if (TempDT.Columns.Count == 0)
        {
            TempDT.Columns.Add("ID", typeof(int));
            TempDT.Columns.Add("Image", typeof(string));
            TempDT.Columns.Add("Name", typeof(string));
            TempDT.Columns.Add("ProductCode", typeof(string));
            TempDT.Columns.Add("HSNCode", typeof(string));
            TempDT.Columns.Add("Barcode", typeof(string));
            TempDT.Columns.Add("UOM", typeof(string));
            TempDT.Columns.Add("Quantity", typeof(string));
            TempDT.Columns.Add("IssueQty", typeof(string));
            TempDT.Columns.Add("UnitPrice", typeof(string));
            TempDT.Columns.Add("TotalPrice", typeof(string));
        }
        ViewState["DTTable"] = TempDT;
    }

    // For Insert Into DataTable
    private void InsertDTTable(string[] ItemArray)
    {
        TempDT = ViewState["DTTable"] as DataTable;
        TempDT.Rows.Add(ItemArray[0], ItemArray[1], ItemArray[2], ItemArray[3], ItemArray[4], ItemArray[5], ItemArray[6], ItemArray[7], ItemArray[8], ItemArray[9],ItemArray[10]);
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
                ItemArray[3] = dr["ProductCode"].ToString();
                ItemArray[4] = dr["HSNCode"].ToString();
                ItemArray[5] = dr["Barcode"].ToString();
                ItemArray[6] = dr["UOM"].ToString();
                ItemArray[7] = dr["Quantity"].ToString();
                ItemArray[8] = dr["IssueQty"].ToString();
                ItemArray[9] = dr["UnitPrice"].ToString();
                ItemArray[10] = dr["TotalPrice"].ToString();
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
                dr["ProductCode"] = ItemArray[3].ToString();
                dr["HSNCode"] = ItemArray[4].ToString();
                dr["BarCode"] = ItemArray[5].ToString();
                dr["UOM"] = ItemArray[6].ToString();
                dr["Quantity"] = ItemArray[7].ToString();
                dr["IssueQty"] = ItemArray[8].ToString();
                dr["UnitPrice"] = ItemArray[9].ToString();
                dr["TotalPrice"] = ItemArray[10].ToString();

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
            Qty += Convert.ToDouble(TempDT.Rows[i][8]);
            value += Convert.ToDouble(TempDT.Rows[i][10]);
        }
        DataArray[0] = Qty.ToString();
        DataArray[1] = value.ToString();
        return DataArray;
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
            BindGVDetail();
            BindIssueGVDetails();
        }
    }

    private void BindIssueGVDetails()
    {
        SqlParameter[] arr = new SqlParameter[1];
        arr[0] = new SqlParameter("@Action", "SELECT_ByView");
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_IssueMaster_CRUD", arr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();

        }
        else
        {
            GridView1.EmptyDataText = "No Record Found";
            GridView1.DataBind();
        }
    }

    public void BindGVDetail()
    {
        SqlParameter[] arr = new SqlParameter[1];
        arr[0] = new SqlParameter("@Action", "SELECT_ByView");
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestMaster_CRUD", arr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GVItemDetail.DataSource = ds.Tables[0];
            GVItemDetail.DataBind();

        }
        else
        {
            GVItemDetail.EmptyDataText = "No Record Found";
            GVItemDetail.DataBind();
        }
        foreach (GridViewRow gvr in GVItemDetail.Rows)
        {
            foreach (GridViewRow Row in GVItemDetail.Rows)
            {
                Label LblStatus = (Label)Row.FindControl("LblStatus");
                LinkButton LBEdit = (LinkButton)Row.FindControl("LBEdit");
                LinkButton LBDelete = (LinkButton)Row.FindControl("LBDelete");
                if (LblStatus.Text == "Active")
                {
                    LblStatus.CssClass = "label label-success";
                    
                }
                else
                {
                    LblStatus.CssClass = "label label-danger";
                    
                }
            }

        }
    }
    
    protected void GVItemDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int recordId = Convert.ToInt32(e.CommandArgument);
            Session["RecordID"] = recordId;

            if (e.CommandName == "ViewRecord")
            {
                SqlParameter[] arr = new SqlParameter[2];
                arr[0] = new SqlParameter("@Action", "SELECT_ByID");
                arr[1] = new SqlParameter("@ID", recordId);
                DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestMaster_CRUD", arr);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    MVIssueMaterial.ActiveViewIndex = 1;
                    LBLReqNo.Text = BindIssueNo();
                    LBLTOtQut.Text = dt.Rows[0]["TotalQty"].ToString();
                    DDLComLoc.SelectedValue = dt.Rows[0]["ComId"].ToString();
                    BindComForRequest();
                    // BindItemFinishTypeList();
                    LBLTotVal.Text = dt.Rows[0]["TotalValue"].ToString();
                    LBReqDate.Text = dt.Rows[0]["ReqDate"].ToString();
                    txtDelDate.Text = dt.Rows[0]["DeliverDate"].ToString();
                    // BindItemKitTypeList();
                    BindNgoForRequest();
                    DDLNgoLoc.ClearSelection();
                    DDLNgoLoc.SelectedValue = dt.Rows[0]["NgoId"].ToString();
                    DDLItemType.ClearSelection();
                    DDLItemList.ClearSelection();
                    //DDLItemType.SelectedValue = dt.Rows[0]["ItemType"].ToString();
                    //DDLItemList.SelectedValue = dt.Rows[0]["ItemList"].ToString();
                    txtRemarks.Text = dt.Rows[0]["Remark"].ToString();
                    txtTermCon.Text = dt.Rows[0]["TermAndCondition"].ToString();
                    BindDeliverToName();
                    DDLDelTo.SelectedValue = dt.Rows[0]["DeliverTo"].ToString();
                    
                    SqlParameter[] arrParam2 = new SqlParameter[2];
                    arrParam2[0] = new SqlParameter("@Action", "Select_ByReqId");
                    arrParam2[1] = new SqlParameter("@ReqId", recordId);
                    DataSet DSInner = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestDetail_CRUD", arrParam2);
                    // Bind Table
                    CreateDTTable();
                    TempDT = ViewState["DTTable"] as DataTable;
                    for (int i = 0; i < DSInner.Tables[0].Rows.Count; i++)
                    {
                        string SelQuery = "SELECT * FROM KitMaterial Where ID='" + DSInner.Tables[0].Rows[i]["ItemId"] + "'";
                        DataTable DTProduct = clsConnectionSql.filldt(SelQuery);

                        double ItemPrice, Quantity, GTotal;
                        Quantity = Convert.ToDouble(DSInner.Tables[0].Rows[i]["Qty"].ToString());
                        ItemPrice = Convert.ToDouble(DTProduct.Rows[0]["TotalValue"].ToString());
                        GTotal = ItemPrice * Quantity;

                        TempDT.Rows.Add(DTProduct.Rows[0]["ID"],
                            DTProduct.Rows[0]["Image"].ToString(),
                            DTProduct.Rows[0]["Name"].ToString(),
                            DTProduct.Rows[0]["ProductCode"].ToString(),
                            DTProduct.Rows[0]["HSNCode"].ToString(),
                            DTProduct.Rows[0]["BarCode"].ToString(),
                            DTProduct.Rows[0]["UOM"].ToString(),
                            Quantity.ToString("0.00"),
                            "0.00",
                            ItemPrice.ToString("0.00"),
                            "0.00");
                    }

                    GVInner.DataSource = TempDT;
                    GVInner.DataBind();
                    ViewState["DTTable"] = TempDT;
                }
                BindInnerGV();
            }
            BindGVDetail();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private string BindIssueNo()
    {
        try
        {
            string DocumentCode = "Issue"; string PrvID = ""; int NextID = 1;
            string GetMaxId = "SELECT IssueNo From IssueMaster Order By ID DESC ";
            DataTable dtBill = clsConnectionSql.filldt(GetMaxId);
            if (dtBill.Rows.Count > 0)
            {
                PrvID = dtBill.Rows[0]["IssueNo"].ToString();
                string[] PrvCon = PrvID.Split('e');
                NextID = Convert.ToInt32(PrvCon[1]) + 1;
                string Zero = new String('0', (5 - NextID.ToString().Length));
                DocumentCode = DocumentCode + Zero + NextID.ToString();
                return DocumentCode.ToString();
            }
            else
                return "Issue00001";
        }
        catch (Exception ex)
        {
            return "0";
        }
    }
    
    protected void GVItemDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVItemDetail.PageIndex = e.NewPageIndex;
        BindGVDetail();
        GVItemDetail.DataBind();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindIssueGVDetails();
        GridView1.DataBind();
    }
    
    private void BindDeliverToName()
    {
        try
        {

            SqlParameter[] arr = new SqlParameter[2];
            arr[0] = new SqlParameter("@Action", "SELECT_ByNGOID");
            arr[1] = new SqlParameter("@NGOID", DDLNgoLoc.SelectedValue);
            DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLDelTo.DataSource = ds.Tables[0];
                DDLDelTo.DataTextField = "Name";
                DDLDelTo.DataValueField = "ID";
                DDLDelTo.DataBind();
                DDLDelTo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);

        }
    }

    private void BindComForRequest()
    {
        try
        {
            SqlParameter[] arr = new SqlParameter[1];
            arr[0] = new SqlParameter("@Action", "SELECT");
            DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Company_CRUD", arr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLComLoc.DataSource = ds.Tables[0];
                DDLComLoc.DataTextField = "CompanyName";
                DDLComLoc.DataValueField = "ID";
                DDLComLoc.DataBind();
                DDLComLoc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }
    
    private void BindNgoForRequest()
    {
        try
        {
            SqlParameter[] arr = new SqlParameter[1];
            arr[0] = new SqlParameter("@Action", "SELECT");
            DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLNgoLoc.DataSource = ds.Tables[0];
                DDLNgoLoc.DataTextField = "Name";
                DDLNgoLoc.DataValueField = "ID";
                DDLNgoLoc.DataBind();
                DDLNgoLoc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }
    
    public void BindInnerGV()
    {
        GVInner.DataSource = ViewState["DTTable"] as DataTable;
        GVInner.DataBind();
        InnerGVData();

    }
    
    protected void InnerGVData()
    {
        try
        {
            string[] array = new string[2];
            array = TotalDetails();
            LBLTOtQut.Text = Convert.ToDouble(array[0]).ToString("0.00");
            LBLTotVal.Text = Convert.ToDouble(array[1]).ToString("0.00");
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }
    
    protected void DDLItemList_SelectedIndexChanged(object sender, EventArgs e)
    {

        string strquery = " ";
        if (DDLItemType.SelectedValue == "1")
        {
            if (sender == txtBarCode && txtBarCode.Text != string.Empty)
                strquery = "SELECT * FROM kitMaterial WHERE Barcode='" + txtBarCode.Text.Trim() + "'";

            else if (sender == DDLItemList && DDLItemList.SelectedIndex > 0)
                strquery = "SELECT * FROM kitMaterial WHERE ID='" + DDLItemList.SelectedValue + "'";

        }
        else if (DDLItemType.SelectedValue == "2")
        {
            if (sender == txtBarCode && txtBarCode.Text != string.Empty)
                strquery = "Select * from FinishMaterial where Barcode='" + txtBarCode.Text.Trim() + "'";

            else if (sender == DDLItemList && DDLItemList.SelectedIndex > 0)
                strquery = "Select * from FinishMaterial where ID='" + DDLItemList.SelectedValue + "'";
        }
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, strquery);
        DataTable DTT = ds.Tables[0];
        if (DTT.Rows.Count > 0)
        {
            double Quantity = 1.0;

            string[] DArray = new string[10];
            string[] DataArray = new string[10];
            DArray = SelectDTTable(Convert.ToInt32(DTT.Rows[0]["ID"]));
            if (DArray[0] == DTT.Rows[0]["ID"].ToString())
            {
                Quantity = Convert.ToDouble(DArray[7]) + Quantity;
                DataArray = BindInnerData(DTT, Quantity);
                UpdateDTTable(DataArray[0], DataArray);
            }
            else
            {
                DataArray = BindInnerData(DTT, Quantity);
                InsertDTTable(DataArray);
            }
            BindInnerGV();
        }

        txtBarCode.Text = string.Empty;
        DDLItemList.SelectedIndex = 0;
    }
    
    private string[] BindInnerData(DataTable DTT, double Quantity)
    {
        double PurPrice = Convert.ToDouble(DTT.Rows[0]["TotalValue"]);

        string[] RecArray = new string[10];
        RecArray[0] = DTT.Rows[0]["ID"].ToString();
        RecArray[1] = DTT.Rows[0]["Image"].ToString();
        RecArray[2] = DTT.Rows[0]["Name"].ToString();
        RecArray[3] = DTT.Rows[0]["ProductCode"].ToString();
        RecArray[4] = DTT.Rows[0]["HSNCode"].ToString();
        RecArray[5] = DTT.Rows[0]["BarCode"].ToString();
        RecArray[6] = DTT.Rows[0]["UOM"].ToString();
        RecArray[7] = Quantity.ToString();
        RecArray[8] = PurPrice.ToString();
        RecArray[9] = (Quantity * PurPrice).ToString();

        return RecArray;
    }
    
    protected void BindInnerData2(object sender, EventArgs e)
    {
        for (int i = 0; i < GVInner.Rows.Count; i++)
        {
            TextBox TBQty = (TextBox)GVInner.Rows[i].FindControl("TBItemQuantity");
            HiddenField HFIDQuantity = (HiddenField)GVInner.Rows[i].FindControl("HFID");
            string[] StrArray = new string[10];
            StrArray = SelectDTTable(Convert.ToInt32(HFIDQuantity.Value));
            double RecQty = Convert.ToDouble(TBQty.Text);
            StrArray[7] = RecQty.ToString("0.00");
            StrArray[9] = (Convert.ToDouble(StrArray[7]) * Convert.ToDouble(StrArray[8])).ToString("0.00");
            UpdateDTTable(Convert.ToInt32(HFIDQuantity.Value), StrArray);
        }
        BindInnerGV();
    }
    
    protected void DDLComLoc_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlParameter[] arr = new SqlParameter[2];
        arr[0] = new SqlParameter("@Action", "SELECT_ByComID");
        arr[1] = new SqlParameter("@ComID", DDLComLoc.SelectedValue);
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arr);

        DDLNgoLoc.DataSource = ds;
        DDLNgoLoc.DataTextField = "Name";
        DDLNgoLoc.DataValueField = "ID";
        DDLNgoLoc.DataBind();
        DDLNgoLoc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

    }
    
    protected void DDLNgoLoc_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDeliverToName();
        //SqlParameter[] arr = new SqlParameter[2];
        //arr[0] = new SqlParameter("@Action", "SELECT_ByComID");
        //arr[1] = new SqlParameter("@NGOID", DDLNgoLoc.SelectedValue);
        //DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Godown_CRUD", arr);
        //DDLDelTo.DataSource = ds;
        //DDLDelTo.DataTextField = "Name";
        //DDLDelTo.DataValueField = "ID";
        //DDLDelTo.DataBind();
        //DDLDelTo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
    }
    
    private void ClearForm()
    {
        LBReqDate.Text = string.Empty;
        LBLTotVal.Text = string.Empty;
        LBLTOtQut.Text = string.Empty;
        LBLReqNo.Text = string.Empty;
        txtDelDate.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        txtTermCon.Text = string.Empty;
        btnSubmit.Text = "Submit";
    }
    
    protected void BtnSubmitCancel(object sender, EventArgs e)
    {
        if (sender == btnSubmit)
        {
            int ReqId = 0;

            SqlParameter[] arr = new SqlParameter[12];
            arr[0] = new SqlParameter("@Action", "INSERT");
            arr[1] = new SqlParameter("@ComID", DDLComLoc.SelectedValue);
            arr[2] = new SqlParameter("@NgoID", DDLNgoLoc.SelectedValue);
            arr[3] = new SqlParameter("@IssueNo", LBLReqNo.Text);
            arr[4] = new SqlParameter("@IssueDate", LBReqDate.Text);
            arr[5] = new SqlParameter("@DeliveryDate", txtDelDate.Text);
            arr[6] = new SqlParameter("@DeliverTo", DDLDelTo.SelectedValue);
            arr[7] = new SqlParameter("@TermAndCondition", txtTermCon.Text);
            arr[8] = new SqlParameter("@TotalQty", LBLTOtQut.Text);
            arr[9] = new SqlParameter("@TotalValue", LBLTotVal.Text);
            arr[10] = new SqlParameter("@Remark", txtRemarks.Text);
            arr[11] = new SqlParameter("@CreatedBy", Session["UserName"]);
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_IssueMaster_CRUD", arr);
            if (result > 0)
            {
                string SelQuery = "SELECT TOP(1) ID FROM IssueMaster ORDER BY ID DESC";
                DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReqId = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }

                TempDT = ViewState["DTTable"] as DataTable;
                for (int i = 0; i < TempDT.Rows.Count; i++)
                {
                    DataRow dr = TempDT.Rows[i];
                    SqlParameter[] arrParamDetails = new SqlParameter[4];
                    arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                    arrParamDetails[1] = new SqlParameter("@IssueId", ReqId);
                    arrParamDetails[2] = new SqlParameter("@ItemId", dr[0]);
                    arrParamDetails[3] = new SqlParameter("@Qty", dr[8]);

                    string str = "SELECT * FROM KitDetails WHERE MID='" + dr[0] + "' ";
                    DataSet dsDD = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, str);
                    if (dsDD.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsDD.Tables[0].Rows.Count; j++)
                        {
                            decimal STQty = Convert.ToDecimal(dsDD.Tables[0].Rows[j]["Qty"]) * Convert.ToDecimal(dr[8]);

                            // code for Issue From Stock
                            string Sttr = "UPDATE ItemStock SET TotalStock=TotalStock-'" + STQty + "' WHERE GodownID='" + DDLDelTo.SelectedValue + "' AND ItemID='" + dsDD.Tables[0].Rows[j]["PID"] + "' ";
                            int result1 = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.Text, Sttr);

                            //Code For Entry in Stock Ledger
                            SqlParameter[] arrParam = new SqlParameter[10];
                            arrParam[0] = new SqlParameter("@Action", "INSERT");
                            arrParam[1] = new SqlParameter("@GodownID", DDLDelTo.SelectedValue);
                            arrParam[2] = new SqlParameter("@ItemID", dsDD.Tables[0].Rows[j]["PID"]);
                            arrParam[3] = new SqlParameter("@ItemType", "Raw Material");
                            arrParam[4] = new SqlParameter("@Date", Convert.ToDateTime(LBReqDate.Text));
                            arrParam[5] = new SqlParameter("@StockIN", "0.00");
                            arrParam[6] = new SqlParameter("@StockOut", STQty);
                            arrParam[7] = new SqlParameter("@Narration", "Issue Stock");
                            arrParam[8] = new SqlParameter("@DocType", "Issue");
                            arrParam[9] = new SqlParameter("@DocNo", LBLReqNo.Text);

                            int result3 = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_StockLedger_CRUD", arrParam);
                        }
                    }

                    int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_IssueDetails_CRUD", arrParamDetails);
                }
                ShowMessage("Added Suceess", MessageType.Success);
            }
            else
                ShowMessage("Some Technical Error Occur!!!", MessageType.Error);
            BindGVDetail();
            BindInnerGV();
            MVIssueMaterial.ActiveViewIndex = 0;

        }
        if (sender == btnCancel)
        {
            ClearForm();
            MVIssueMaterial.ActiveViewIndex = 0;
            BindGVDetail();
        }
    }
    
    public void BindItemKitTypeList()
    {
        SqlParameter[] arr = new SqlParameter[1];
        arr[0] = new SqlParameter("@Action", "SELECT");
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDLItemList.DataSource = ds.Tables[0];
            DDLItemList.DataTextField = "Name";
            DDLItemList.DataValueField = "ID";
            DDLItemList.DataBind();
            DDLItemList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
        }
    }
    
    public void BindItemFinishTypeList()
    {
        SqlParameter[] arr = new SqlParameter[1];
        arr[0] = new SqlParameter("@Action", "SELECT");
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_FinishMaterial_CRUD", arr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDLItemList.DataSource = ds.Tables[0];
            DDLItemList.DataTextField = "Name";
            DDLItemList.DataValueField = "ID";
            DDLItemList.DataBind();
            DDLItemList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
        }
    }
    
    protected void DDLItemType_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (DDLItemType.SelectedValue)
        {
            case "1":
                BindItemKitTypeList();
                break;
            case "2":
                BindItemFinishTypeList();
                break;

            default:

                break;
        }
    }
    
    protected void GVInner_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        TextBox txt=new TextBox();
        txt.Text=txtAllot.Text;
        Session["Index"] = "";
        int RowIndex = Convert.ToInt32(e.CommandName);
        
        GridViewRow Row=GVInner.Rows[RowIndex];
        int IRecordID = Convert.ToInt32(e.CommandArgument);
        Session["Index"] = IRecordID;
        MPE2.Show();
        
        Label lblName = (Label)Row.FindControl("LBLNAMe");
        Label lblBarcode = (Label)Row.FindControl("lblBarcode");
        Label lblItemQty = (Label)Row.FindControl("LB");
        TextBox lblIssueQty = (TextBox)Row.FindControl("TBIssud");
        LBItemName.Text = lblName.Text;
        LBBarcode.Text = lblBarcode.Text;
        LBItemQty.Text = lblItemQty.Text;
        HFIndex.Value = RowIndex.ToString();
        //lblIssueQty.Text = txt.Text;
        BindGVRawMaterial(IRecordID);
        //lblIssueQty.Text = btnOk_Click();
    }

    public void BindGVRawMaterial(int KitID)
    {
        if (DTRowM.Columns.Count == 0)
        {
            DTRowM.Columns.Add("ID", typeof(Int32));
            DTRowM.Columns.Add("Name", typeof(string));
            DTRowM.Columns.Add("Qty", typeof(string));
            DTRowM.Columns.Add("TotalStock", typeof(string));
            DTRowM.Columns.Add("Balance", typeof(string));
        }

        SqlParameter[] arr = new SqlParameter[3];
        arr[0] = new SqlParameter("@Action", "SELECT_Byview");
        arr[1] = new SqlParameter("@KitID", KitID);
        arr[2] = new SqlParameter("@GodownID", DDLDelTo.SelectedValue);
        DataSet ds1 = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arr);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            int[] Arr = new int[ds1.Tables[0].Rows.Count];
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                DTRowM.Rows.Add(Convert.ToInt32(ds1.Tables[0].Rows[i]["PID"].ToString()),
                                ds1.Tables[0].Rows[i]["Name"].ToString(),
                                Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qty"].ToString()) * Convert.ToDecimal(LBItemQty.Text),
                                ds1.Tables[0].Rows[i]["TotalStock"].ToString(),
                                Convert.ToDecimal(ds1.Tables[0].Rows[i]["TotalStock"].ToString()) - (Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qty"].ToString()) * Convert.ToDecimal(LBItemQty.Text)));
                Arr[i] = Convert.ToInt32(ds1.Tables[0].Rows[i]["TotalStock"]) / Convert.ToInt32(ds1.Tables[0].Rows[i]["Qty"]);
            }
            Array.Sort(Arr);
            if (Convert.ToDecimal(Arr[0]) > Convert.ToDecimal(LBItemQty.Text))
                txtAllot.Text = LBItemQty.Text;
            else
                txtAllot.Text = Arr[0].ToString();

            GVPopUpData.DataSource = DTRowM;
            GVPopUpData.DataBind();
            ViewState["DTMROW"] = DTRowM;
            foreach (GridViewRow Row in GVPopUpData.Rows)
            {
                Label LBBalance = (Label)Row.FindControl("LBBalance");
                //LinkButton LBOrder=(LinkButton)Row.FindControl("LBOrder");

                if (Convert.ToDecimal(LBBalance.Text) < 0)
                {
                    //LBOrder.Visible = true;
                }
                else
                {
                    //LBOrder.Visible = false;
                }
            }
        }
        else 
        {
            GVPopUpData.EmptyDataText = "No record Found";
            GVPopUpData.DataBind();
        }

    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        //int RowIndex = Convert.ToInt32(HFIndex.Value);
        //GridViewRow Row = GVInner.Rows[RowIndex];
        //TextBox lblIssueQty = (TextBox)Row.FindControl("TBIssud");
        string strTaskName = txtAllot.Text.ToString();
        //lblIssueQty.Text = strTaskName;


        foreach (GridViewRow row in GVInner.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                TextBox lblIssueQty = (TextBox)row.FindControl("TBIssud");
                lblIssueQty.Text = strTaskName;
            }
        }
    }

    #region DataTable Section
    DataTable DTRowM = new DataTable();
    #endregion
    
    protected void ModelSubmitButton(object sender, EventArgs e)
    {
        //MPE2.Hide();
        for (int i = 0; i < GVInner.Rows.Count; i++)
        {
            string[] StrArray = new string[11];
            StrArray = SelectDTTable(Convert.ToInt32(Session["Index"]));
            StrArray[8]= Convert.ToDouble(txtAllot.Text).ToString("0.00");
            StrArray[10] = (Convert.ToDecimal(StrArray[8]) * Convert.ToDecimal(StrArray[9])).ToString("0.00");
            UpdateDTTable(Convert.ToInt32(Session["Index"]) , StrArray);
        }
        BindInnerGV();
    }

    //protected void GVPopUpData_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    DTRowM = ViewState["DTMROW"] as DataTable;
    //    //int RowIndex = Convert.ToInt32(e.CommandName);
    //    //GridViewRow Row = GVInner.Rows[RowIndex];
    //    int itemId = Convert.ToInt32(e.CommandArgument);
    //    Session["ItemId"] = itemId.ToString();
    //    //for (int i =1; i<=DTRowM.Rows.Count; i++)
    //    //{
    //        decimal ReqQty = Convert.ToDecimal(DTRowM.Rows[0]["Balance"].ToString()) * Convert.ToDecimal(-1);
    //        Session["ReqQty"] = ReqQty;
    //    //}
    //    SqlParameter[] arr = new SqlParameter[6];
    //   arr[0]=new SqlParameter("@Action","INSERT");
    //   arr[1] = new SqlParameter("@ItemId", itemId);
    //   arr[2] = new SqlParameter("@Balance",ReqQty);
    //   arr[3] = new SqlParameter("@ComId", Session["ComId"]);
    //   arr[4] = new SqlParameter("@NGOId", Session["NGOId"]);
    //   string str = "Select ID From Godown where NGOID='"+Session["NGOId"]+"' and ComID='"+Session["ComId"]+"'";
    //   DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, str);
    //   arr[5] = new SqlParameter("@GodId",ds.Tables[0].Rows[0][0]);
    //   int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ORDER_CRUD", arr);
    //    Response.Redirect("OrderForm.aspx");
    //}

    protected void chkHeader_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkHeader = (CheckBox)GVPopUpData.HeaderRow.FindControl("HeaderCheck");
        if (chkHeader.Checked)
        {
            foreach (GridViewRow gvrow in GVPopUpData.Rows)
            {
                CheckBox chkRow = (CheckBox)gvrow.FindControl("ChkSelection");
                chkRow.Checked = true;
            }
        }
        else
        {
            foreach (GridViewRow gvrow in GVPopUpData.Rows)
            {
                CheckBox chkRow = (CheckBox)gvrow.FindControl("ChkSelection");
                chkRow.Checked = false;
            }
        }

    }

    protected void chkRow_CheckedChanged(object sender, EventArgs e)
    {
        int count = 0;
        int totalRowCountGrid = GVPopUpData.Rows.Count;

        CheckBox chkHeader = (CheckBox)GVPopUpData.HeaderRow.FindControl("HeaderCheck");

        foreach (GridViewRow gvrow in GVPopUpData.Rows)
        {
            CheckBox chkRow = (CheckBox)gvrow.FindControl("ChkSelection");
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
    
    protected void Generate_Order(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in GVPopUpData.Rows)
        {
            CheckBox check = (CheckBox)gvr.FindControl("ChkSelection");
            HiddenField hfid = (HiddenField)gvr.FindControl("HDFD");
            Label LB_Name = (Label)gvr.FindControl("LB_Name");
            Label LB_Quantity = (Label)gvr.FindControl("LB_Quantity");
            Label LB_Availaibility = (Label)gvr.FindControl("LB_Availavility");
            Label LB_Balance = (Label)gvr.FindControl("LBBalance");
            if (check.Checked)
            {
                string name = LB_Name.Text;
                string Quantity = LB_Quantity.Text;
                string Availaivility = LB_Availaibility.Text;
                decimal ReqQty = Convert.ToDecimal(LB_Balance.Text) * Convert.ToDecimal(-1);
                string ItemId = hfid.Value;
                //decimal ReqQty = Convert.ToDecimal(DTRowM.Rows[0]["Balance"].ToString()) * Convert.ToDecimal(-1);
                //Session["ReqQty"] = ReqQty;
                //}
                SqlParameter[] arr = new SqlParameter[6];
                arr[0] = new SqlParameter("@Action", "INSERT");
                arr[1] = new SqlParameter("@ItemId",ItemId);
                arr[2] = new SqlParameter("@Balance", ReqQty);
                arr[3] = new SqlParameter("@ComId", Session["ComId"]);
                arr[4] = new SqlParameter("@NGOId", Session["NGOId"]);
                string str = "Select ID From Godown where NGOID='" + Session["NGOId"] + "' and ComID='" + Session["ComId"] + "'";
                DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, str);
                arr[5] = new SqlParameter("@GodId", ds.Tables[0].Rows[0][0]);
                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ORDER_CRUD", arr);
                Response.Redirect("OrderForm.aspx");
            }
        }
    }
    
}