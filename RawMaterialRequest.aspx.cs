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

public partial class RawMaterialRequest : System.Web.UI.Page
{
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
            TempDT.Columns.Add("ProductCode", typeof(string));
            TempDT.Columns.Add("HSNCode", typeof(string));
            TempDT.Columns.Add("Barcode", typeof(string));
            TempDT.Columns.Add("UOM", typeof(string));
            TempDT.Columns.Add("Quantity", typeof(string));
            TempDT.Columns.Add("UnitPrice", typeof(string));
            TempDT.Columns.Add("TotalPrice", typeof(string));
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
                ItemArray[3] = dr["ProductCode"].ToString();
                ItemArray[4] = dr["HSNCode"].ToString();
                ItemArray[5] = dr["Barcode"].ToString();
                ItemArray[6] = dr["UOM"].ToString();
                ItemArray[7] = dr["Quantity"].ToString();
                ItemArray[8] = dr["UnitPrice"].ToString();
                ItemArray[9] = dr["TotalPrice"].ToString();
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
                dr["UnitPrice"] = ItemArray[8].ToString();
                dr["TotalPrice"] = ItemArray[9].ToString();

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
            Qty += Convert.ToDouble(TempDT.Rows[i][7]);
            value += Convert.ToDouble(TempDT.Rows[i][9]);
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
            BindGVDetail();
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
                    LBEdit.Enabled = true;
                    LBDelete.Enabled = true;
                }
                else
                {
                    LblStatus.CssClass = "label label-danger";
                    LBEdit.Enabled = false;
                    LBDelete.Enabled = false;
                }
            }

        }
    }

    protected void BtnSubmitCancel(object sender, EventArgs e)
    {
        if (sender == btnSubmit)
        {
            int ReqId = 0;
            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] arr = new SqlParameter[13];
                arr[0] = new SqlParameter("@Action", "INSERT");
                arr[1] = new SqlParameter("@ReqNo", LBLReqNo.Text);
                arr[2] = new SqlParameter("@ReqDate", LBReqDate.Text);
                arr[3] = new SqlParameter("@DeliverDate", txtDelDate.Text);
                arr[4] = new SqlParameter("@DeliverTo", DDLDelTo.SelectedValue);
                arr[5] = new SqlParameter("@TermAndCondition", txtTermCon.Text);
                arr[6] = new SqlParameter("@ComId", DDLComLoc.SelectedValue);
                arr[7] = new SqlParameter("@TotalQty", LBLTOtQut.Text);
                arr[8] = new SqlParameter("@Totalvalue", LBLTotVal.Text);
                arr[9] = new SqlParameter("@Remark", txtRemarks.Text);
                arr[10] = new SqlParameter("@NgoId", DDLNgoLoc.SelectedValue);
                arr[11] = new SqlParameter("@ItemList", DDLItemList.SelectedValue);
                arr[12] = new SqlParameter("@ItemType", DDLItemType.SelectedValue);
                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestMaster_CRUD", arr);
                if (result > 0)
                {
                    string SelQuery = "SELECT TOP(1) ID FROM ItemReqeustMaster ORDER BY ID DESC";
                    DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReqId = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    }

                    TempDT = ViewState["DTTable"] as DataTable;
                    for (int i = 0; i < TempDT.Rows.Count; i++)
                    {
                        DataRow dr = TempDT.Rows[i];
                        SqlParameter[] arrParamDetails = new SqlParameter[5];
                        arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                        arrParamDetails[1] = new SqlParameter("@ReqId", ReqId);
                        arrParamDetails[2] = new SqlParameter("@ItemId", dr[0]);
                        arrParamDetails[3] = new SqlParameter("@Qty", dr[7]);
                        arrParamDetails[4] = new SqlParameter("@Price", dr[9]);
                        int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestDetail_CRUD", arrParamDetails);
                    }
                    ShowMessage("Added Suceess", MessageType.Success);
                }
                else
                    ShowMessage("Some Technical Error Occur!!!", MessageType.Error);
                BindGVDetail();
                BindInnerGV();
                MVRequest.ActiveViewIndex = 0;

            }
            if (btnSubmit.Text == "Update")
            {


                SqlParameter[] arr = new SqlParameter[14];
                arr[0] = new SqlParameter("@Action", "UPDATE");
                arr[1] = new SqlParameter("@ReqNo", LBLReqNo.Text);
                arr[2] = new SqlParameter("@ReqDate", LBReqDate.Text);
                arr[3] = new SqlParameter("@DeliverDate", txtDelDate.Text);
                arr[4] = new SqlParameter("@DeliverTo", DDLDelTo.SelectedValue);
                arr[5] = new SqlParameter("@TermAndCondition", txtTermCon.Text);
                arr[6] = new SqlParameter("@ComId", DDLComLoc.SelectedValue);
                arr[7] = new SqlParameter("@TotalQty", LBLTOtQut.Text);
                arr[8] = new SqlParameter("@Totalvalue", LBLTotVal.Text);
                arr[9] = new SqlParameter("@Remark", txtRemarks.Text);
                arr[10] = new SqlParameter("@NgoId", DDLNgoLoc.SelectedValue);
                arr[11] = new SqlParameter("@ItemList", DDLItemList.SelectedValue);
                arr[12] = new SqlParameter("@ItemType", DDLItemType.SelectedValue);
                arr[13] = new SqlParameter("@ID", Session["recordID"]);
                int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestMaster_CRUD", arr);
                ReqId = Convert.ToInt32(Session["RecordID"]);

                // Delete All Records From POProducts Details
                SqlParameter[] arrParamDel = new SqlParameter[2];
                arrParamDel[0] = new SqlParameter("@Action", "DELETE_ByReqId");
                arrParamDel[1] = new SqlParameter("@ReqId", ReqId);
                int Delresult = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestDetail_CRUD", arrParamDel);


                TempDT = ViewState["DTTable"] as DataTable;
                for (int i = 0; i < TempDT.Rows.Count; i++)
                {
                    DataRow dr = TempDT.Rows[i];
                    SqlParameter[] arrParamDetails = new SqlParameter[5];
                    arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                    arrParamDetails[1] = new SqlParameter("@ReqId", ReqId);
                    arrParamDetails[2] = new SqlParameter("@ItemId", dr[0]);
                    arrParamDetails[3] = new SqlParameter("@QTY", dr[7]);
                    arrParamDetails[4] = new SqlParameter("@Price", dr[9]);
                    int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestDetail_CRUD", arrParamDetails);
                }
                if (result > 0)
                    ShowMessage("Added Suceess", MessageType.Success);
                else
                    ShowMessage("Some Technical Error Occur!!!", MessageType.Error);

                btnSubmit.Text = "Submit";
                BindGVDetail();
                BindInnerGV();
                ClearForm();
                MVRequest.ActiveViewIndex = 0;
            }
        }
        if (sender == btnCancel)
        {
            ClearForm();
            MVRequest.ActiveViewIndex = 0;
            BindGVDetail();
        }
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

    protected void GVItemDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int recordId = Convert.ToInt32(e.CommandArgument);
            Session["RecordID"] = recordId;
            if (e.CommandName == "EditRecord")
            {
                SqlParameter[] arr = new SqlParameter[2];
                arr[0] = new SqlParameter("@Action", "SELECT_ByID");
                arr[1] = new SqlParameter("@ID", recordId);
                DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestMaster_CRUD", arr);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    MVRequest.ActiveViewIndex = 1;
                    LBLReqNo.Text = dt.Rows[0]["ReqNo"].ToString();
                    LBLTOtQut.Text = dt.Rows[0]["TotalQty"].ToString();
                    DDLComLoc.SelectedValue = dt.Rows[0]["ComId"].ToString();
                    BindComForRequest();
                    LBLTotVal.Text = dt.Rows[0]["TotalValue"].ToString();
                    LBReqDate.Text = dt.Rows[0]["ReqDate"].ToString();
                    txtDelDate.Text = dt.Rows[0]["DeliverDate"].ToString();
                    BindDDLNGO(DDLComLoc.SelectedValue);
                    DDLNgoLoc.ClearSelection();
                    DDLNgoLoc.SelectedValue = dt.Rows[0]["NgoId"].ToString();
                    DDLItemType.ClearSelection();
                    DDLItemList.ClearSelection();
                    txtRemarks.Text = dt.Rows[0]["Remark"].ToString();
                    txtTermCon.Text = dt.Rows[0]["TermAndCondition"].ToString();
                    BindDDLGodown(DDLComLoc.SelectedValue);
                    DDLDelTo.SelectedValue = dt.Rows[0]["DeliverTo"].ToString();
                    DDLItemType.SelectedValue = dt.Rows[0]["ItemType"].ToString();
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
                    btnSubmit.Text = "Update";
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
                            ItemPrice.ToString("0.00"),
                            GTotal.ToString("0.00"));
                    }
                    GVInner.DataSource = TempDT;
                    GVInner.DataBind();
                    ViewState["DTTable"] = TempDT;
                }
                BindInnerGV();
            }
            else if (e.CommandName == "DeleteRecord")
            {
                SqlParameter[] arr = new SqlParameter[2];
                arr[0] = new SqlParameter("@Action", "DELETE");
                arr[1] = new SqlParameter("@ID", recordId);
                DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestMaster_CRUD", arr);
            }

            if (e.CommandName == "CheckStatus")
            {
                SqlParameter[] arr = new SqlParameter[2];
                arr[0] = new SqlParameter("@Action", "SELECT_ByID");
                arr[1] = new SqlParameter("@ID", recordId);
                DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestMaster_CRUD", arr);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["ReqStatus"].ToString() == "True")
                    {
                        SqlParameter[] arr1 = new SqlParameter[3];
                        arr1[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arr1[1] = new SqlParameter("@ReqStatus", false);
                        arr1[2] = new SqlParameter("@ID", recordId);
                        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestMaster_CRUD", arr1);
                    }
                    if (dt.Rows[0]["ReqStatus"].ToString() == "False")
                    {
                        SqlParameter[] arr1 = new SqlParameter[3];
                        arr1[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arr1[1] = new SqlParameter("@ReqStatus", true);
                        arr1[2] = new SqlParameter("@ID", recordId);
                        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestMaster_CRUD", arr1);
                    }
                }
            }

            BindGVDetail();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void TopEventSet(object sender, EventArgs e)
    {
        MVRequest.ActiveViewIndex = 1;
        LBReqDate.Text = DateTime.Now.ToString();
        BindComForRequest();
        GetReqNo();
        CreateDTTable();
        BindInnerGV();
    }

    private void GetReqNo()
    {
        try
        {
            string DocumentCode = "REQ"; string PrvID = ""; int NextID = 1;
            string GetMaxId = "SELECT ReqNo From ItemReqeustMaster Order By ID DESC ";
            DataTable dtBill = clsConnectionSql.filldt(GetMaxId);
            if (dtBill.Rows.Count > 0)
            {
                PrvID = dtBill.Rows[0]["ReqNo"].ToString();
                string[] PrvCon = PrvID.Split('Q');
                NextID = Convert.ToInt32(PrvCon[1]) + 1;
                string Zero = new String('0', (5 - NextID.ToString().Length));
                DocumentCode = DocumentCode + Zero + NextID.ToString();
                LBLReqNo.Text = DocumentCode.ToString();
            }
            else
                LBLReqNo.Text = "REQ00001";
        }
        catch (Exception ex)
        {
        }
    }

    protected void LBSearch_Click(object sender, EventArgs e)
    {
        SqlParameter[] arrParamDel = new SqlParameter[2];
        arrParamDel[0] = new SqlParameter("@Action", "SEARCH");
        arrParamDel[1] = new SqlParameter("@SearchText", TBSearch.Text);
        DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemRequestMaster_CRUD", arrParamDel);
        if (DSRecords.Tables[0].Rows.Count > 0)
        {
            GVItemDetail.DataSource = DSRecords.Tables[0];
            GVItemDetail.DataBind();
        }
        else
        {
            GVItemDetail.EmptyDataText = "No record Found";
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
                    LBEdit.Enabled = true;
                    LBDelete.Enabled = true;
                }
                else
                {
                    LblStatus.CssClass = "label label-danger";
                    LBEdit.Enabled = false;
                    LBDelete.Enabled = false;
                }
            }

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

    protected void DDLComLoc_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDDLNGO(DDLComLoc.SelectedValue);
        BindDDLGodown(DDLComLoc.SelectedValue);
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
                DDLNgoLoc.DataSource = DSRecord.Tables[0];
                DDLNgoLoc.DataTextField = "BusinessLocationName";
                DDLNgoLoc.DataValueField = "ID";
                DDLNgoLoc.DataBind();
                DDLNgoLoc.Items.Insert(0, new ListItem("Select", "0"));
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
                DDLDelTo.DataSource = DSRecord.Tables[0];
                DDLDelTo.DataTextField = "Name";
                DDLDelTo.DataValueField = "ID";
                DDLDelTo.DataBind();
                DDLDelTo.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
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
                CreateDTTable();
                BindInnerGV();
                break;
            case "2":
                BindItemFinishTypeList();
                CreateDTTable();
                BindInnerGV();
                break;
            default:
                break;
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

    protected void GVInner_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int IRecordID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "DeleteRecord")
        {
            DeletDTTable(IRecordID);
            BindInnerGV();
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

    protected void GVItemDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVItemDetail.PageIndex = e.NewPageIndex;
        BindGVDetail();
        GVItemDetail.DataBind();
    }
}


