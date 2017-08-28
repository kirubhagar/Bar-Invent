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

public partial class ItemKit : System.Web.UI.Page
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
    string[] ItemArray = new string[6];

    // Create / Load DataTable
    private void CreateDTTable()
    {
        TempDT = new DataTable();
        if (TempDT.Columns.Count == 0)
        {
            TempDT.Columns.Add("ID", typeof(int));
            TempDT.Columns.Add("ItemName", typeof(string));
            TempDT.Columns.Add("Barcode", typeof(string));
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
        TempDT.Rows.Add(ItemArray[0], ItemArray[1], ItemArray[2], ItemArray[3], ItemArray[4], ItemArray[5]);
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
                ItemArray[1] = dr["ItemName"].ToString();
                ItemArray[2] = dr["Barcode"].ToString();
                ItemArray[3] = dr["Quantity"].ToString();
                ItemArray[4] = dr["UnitPrice"].ToString();
                ItemArray[5] = dr["TotalPrice"].ToString();
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
                dr["Quantity"] = ItemArray[3].ToString();
                dr["UnitPrice"] = ItemArray[4].ToString();
                dr["TotalPrice"] = ItemArray[5].ToString();
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
            Qty += Convert.ToDouble(TempDT.Rows[i][3]);
            value += Convert.ToDouble(TempDT.Rows[i][5]);
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
            BindGVKitMaterial();
        }
    }

    private void BindGVKitMaterial()
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[1];
            arrParam[0] = new SqlParameter("@Action", "SELECT");
            DataSet DSUser = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arrParam);
            if (DSUser.Tables[0].Rows.Count > 0)
            {
                GVKitMaterial.DataSource = DSUser.Tables[0];
                GVKitMaterial.DataBind();
            }
            else
            {
                GVKitMaterial.EmptyDataText = "No Record Found !";
                GVKitMaterial.DataBind();
            }
            foreach (GridViewRow Row in GVKitMaterial.Rows)
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
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void LBAddNew_Click(object sender, EventArgs e)
    {
        MVMatrialKit.ActiveViewIndex = 1;
        CreateDTTable();
        BindInnerGV();
        BindRowItemList();
    }

    private void BindRowItemList()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT");
        DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParam);
        if (DSRecords.Tables[0].Rows.Count > 0)
        {
            DDLItemList.DataSource = DSRecords.Tables[0];
            DDLItemList.DataTextField = "Name";
            DDLItemList.DataValueField = "ID";
            DDLItemList.DataBind();
            DDLItemList.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void BindInnerGV()
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
            lblTotalQty.Text = Convert.ToDouble(array[0]).ToString("0.00");
            lblTotalValue.Text = Convert.ToDouble(array[1]).ToString("0.00");
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void ItemList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string SelQuery = string.Empty;
        if (sender == TBSearchBarcode && TBSearchBarcode.Text != string.Empty)
        {
            SelQuery = "SELECT * FROM RawMaterial WHERE Barcode='" + TBSearchBarcode.Text.Trim() + "'";
        }
        else if (sender == DDLItemList && DDLItemList.SelectedIndex > 0)
        {
            SelQuery = "SELECT * FROM RawMaterial WHERE ID='" + DDLItemList.SelectedValue + "'";
        }

        DataTable DTT = clsConnectionSql.filldt(SelQuery);
        if (DTT.Rows.Count > 0)
        {
            double Quantity = 1.0;
            double DisPer = 0.00;
            string[] DArray = new string[6];
            string[] DataArray = new string[6];
            DArray = SelectDTTable(Convert.ToInt32(DTT.Rows[0]["ID"]));
            if (DArray[0] == DTT.Rows[0]["ID"].ToString())
            {
                Quantity = Convert.ToDouble(DArray[3]) + Quantity;
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
        TBSearchBarcode.Text = string.Empty;
    }

    private string[] BindInnerData(DataTable DTT, double Quantity, double DisPer)
    {
        double PurPrice = Convert.ToDouble(DTT.Rows[0]["PurPrice"]);

        string[] RecArray = new string[6];
        RecArray[0] = DTT.Rows[0]["ID"].ToString();
        RecArray[1] = DTT.Rows[0]["Name"].ToString();
        RecArray[2] = DTT.Rows[0]["Barcode"].ToString();
        RecArray[3] = Quantity.ToString("0.00");
        RecArray[4] = PurPrice.ToString("0.00");
        RecArray[5] = (Convert.ToDouble(RecArray[4]) * Convert.ToDouble(RecArray[3])).ToString("0.00");
        return RecArray;
    }

    protected void BindInnerData2(object sender, EventArgs e)
    {
        for (int i = 0; i < GVInner.Rows.Count; i++)
        {
            TextBox TBQty = (TextBox)GVInner.Rows[i].FindControl("TBItemQuantity");
            HiddenField HFIDQuantity = (HiddenField)GVInner.Rows[i].FindControl("HFID");
            string[] StrArray = new string[6];
            StrArray = SelectDTTable(Convert.ToInt32(HFIDQuantity.Value));
            double RecQty = Convert.ToDouble(TBQty.Text);
            StrArray[3] = RecQty.ToString("0.00");
            StrArray[5] = (Convert.ToDouble(StrArray[4]) * Convert.ToDouble(StrArray[3])).ToString("0.00");
            UpdateDTTable(Convert.ToInt32(HFIDQuantity.Value), StrArray);
        }
        BindInnerGV();
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

    protected void CBShowImage_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (CBShowImage.Checked)
                GVKitMaterial.Columns[1].Visible = true;
            else
                GVKitMaterial.Columns[1].Visible = false;
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void LBSearch_Click(object sender, EventArgs e)
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "SEARCH");
            arrParam[1] = new SqlParameter("@SearchText", TBSearch.Text);
            DataSet DSUser = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arrParam);

            if (DSUser.Tables[0].Rows.Count > 0)
            {
                GVKitMaterial.DataSource = DSUser.Tables[0];
                GVKitMaterial.DataBind();
            }
            else
            {
                GVKitMaterial.EmptyDataText = "No Record Found !";
                GVKitMaterial.DataBind();
            }
            foreach (GridViewRow Row in GVKitMaterial.Rows)
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
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            if (sender == btnSubmit)
            {
                string ImagePath = GetImagePath(FULogo);

                int MID = 0;

                if (btnSubmit.Text == "Submit")
                {
                    SqlParameter[] arrParam = new SqlParameter[13];
                    arrParam[0] = new SqlParameter("@Action", "INSERT");
                    arrParam[1] = new SqlParameter("@Image", ImagePath);
                    arrParam[2] = new SqlParameter("@Name", TBName.Text);
                    arrParam[3] = new SqlParameter("@ProductCode", TBProductCode.Text);
                    arrParam[4] = new SqlParameter("@HSNCode", TBHSN.Text);
                    arrParam[5] = new SqlParameter("@Barcode", TBBarcode.Text);
                    arrParam[6] = new SqlParameter("@UOM", TBUOM.Text);
                    arrParam[7] = new SqlParameter("@Category", TBCategory.Text);
                    arrParam[8] = new SqlParameter("@Description", TBDescription.Text);
                    arrParam[9] = new SqlParameter("@TotalQty", lblTotalQty.Text);
                    arrParam[10] = new SqlParameter("@TotalValue", lblTotalValue.Text);
                    arrParam[11] = new SqlParameter("@CreatedOn", DateTime.Now);
                    arrParam[12] = new SqlParameter("@CreatedBy", Session["FullName"]);

                    int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arrParam);
                    if (result > 0)
                    {
                        string SelQuery = "SELECT TOP(1) ID FROM KitMaterial ORDER BY ID DESC";
                        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        }

                        TempDT = ViewState["DTTable"] as DataTable;
                        for (int i = 0; i < TempDT.Rows.Count; i++)
                        {
                            DataRow dr = TempDT.Rows[i];
                            SqlParameter[] arrParamDetails = new SqlParameter[5];
                            arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                            arrParamDetails[1] = new SqlParameter("@MID", MID);
                            arrParamDetails[2] = new SqlParameter("@PID", dr[0]);
                            arrParamDetails[3] = new SqlParameter("@Qty", dr[3]);
                            arrParamDetails[4] = new SqlParameter("@Price", dr[5]);
                            int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_KitDetails_CRUD", arrParamDetails);
                        }
                        ShowMessage("Added Suceess", MessageType.Success);
                    }
                    else
                        ShowMessage("Some Technical Error Occur!!!", MessageType.Error);
                    ClearUserForm();
                    MVMatrialKit.ActiveViewIndex = 0;
                    BindGVKitMaterial();
                }
                else if (btnSubmit.Text == "Update")
                {
                    SqlParameter[] arrParam = new SqlParameter[12];
                    arrParam[0] = new SqlParameter("@Action", "UPDATE");
                    arrParam[1] = new SqlParameter("@Image", ImagePath);
                    arrParam[2] = new SqlParameter("@Name", TBName.Text);
                    arrParam[3] = new SqlParameter("@ProductCode", TBProductCode.Text);
                    arrParam[4] = new SqlParameter("@HSNCode", TBHSN.Text);
                    arrParam[5] = new SqlParameter("@Barcode", TBBarcode.Text);
                    arrParam[6] = new SqlParameter("@UOM", TBUOM.Text);
                    arrParam[7] = new SqlParameter("@Category", TBCategory.Text);
                    arrParam[8] = new SqlParameter("@Description", TBDescription.Text);
                    arrParam[9] = new SqlParameter("@TotalQty", lblTotalQty.Text);
                    arrParam[10] = new SqlParameter("@TotalValue", lblTotalValue.Text);
                    arrParam[11] = new SqlParameter("@ID", Session["RecordId"]);

                    int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arrParam);

                    MID = Convert.ToInt32(Session["RecordID"]);

                    // Delete All Records From POProducts Details
                    SqlParameter[] arrParamDel = new SqlParameter[2];
                    arrParamDel[0] = new SqlParameter("@Action", "DELETE_ByMasterID");
                    arrParamDel[1] = new SqlParameter("@MID", MID);
                    int Delresult = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_KitDetails_CRUD", arrParamDel);

                    #region Insert PO Product Details
                    TempDT = ViewState["DTTable"] as DataTable;
                    for (int i = 0; i < TempDT.Rows.Count; i++)
                    {
                        DataRow dr = TempDT.Rows[i];
                        SqlParameter[] arrParamDetails = new SqlParameter[5];
                        arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                        arrParamDetails[1] = new SqlParameter("@MID", MID);
                        arrParamDetails[2] = new SqlParameter("@PID", dr[0]);
                        arrParamDetails[3] = new SqlParameter("@Qty", dr[3]);
                        arrParamDetails[4] = new SqlParameter("@Price", dr[5]);
                        int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_KitDetails_CRUD", arrParamDetails);
                    }
                    #endregion

                    if (result > 0)
                        ShowMessage("Updated Successfully", MessageType.Success);
                    else
                        ShowMessage("Error", MessageType.Error);

                    ClearUserForm();
                    MVMatrialKit.ActiveViewIndex = 0;
                    BindGVKitMaterial();
                    btnSubmit.Text = "Submit";
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private string GetImagePath(FileUpload FULogo)
    {
        string ImagePath = "";
        if (FULogo.HasFile)
        {
            string NameImage = DateTime.Now.Millisecond + FULogo.FileName;
            FULogo.SaveAs(Server.MapPath("~/AdminImages/KitMaterial/" + NameImage));
            ImagePath = "~/AdminImages/KitMaterial/" + NameImage;
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearUserForm();
        MVMatrialKit.ActiveViewIndex = 0;
        BindGVKitMaterial();
    }

    private void ClearUserForm()
    {
        TBName.Text = string.Empty;
        TBProductCode.Text = string.Empty;
        TBHSN.Text = string.Empty;
        TBBarcode.Text = string.Empty;
        TBUOM.Text = string.Empty;
        TBCategory.Text = string.Empty;
        TBDescription.Text = string.Empty;
    }

    protected void GVKitMaterial_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int recordId = Convert.ToInt32(e.CommandArgument);
            Session["RecordId"] = recordId;

            #region For Edit Record
            if (e.CommandName == "EditRecord")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arrParam);
                DataTable DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    MVMatrialKit.ActiveViewIndex = 1;
                    TBName.Text = DTRecords.Rows[0]["Name"].ToString();
                    TBBarcode.Text = DTRecords.Rows[0]["Barcode"].ToString();
                    TBProductCode.Text = DTRecords.Rows[0]["ProductCode"].ToString();
                    TBHSN.Text = DTRecords.Rows[0]["HSNCode"].ToString();
                    TBUOM.Text = DTRecords.Rows[0]["UOM"].ToString();
                    TBCategory.Text = DTRecords.Rows[0]["Category"].ToString();
                    TBDescription.Text = DTRecords.Rows[0]["Description"].ToString();
                    lblTotalQty.Text = DTRecords.Rows[0]["TotalQty"].ToString();
                    lblTotalValue.Text = DTRecords.Rows[0]["TotalValue"].ToString();
                    ImgLogo.ImageUrl = DTRecords.Rows[0]["Image"].ToString();
                    BindRowItemList();

                    SqlParameter[] arrParam2 = new SqlParameter[2];
                    arrParam2[0] = new SqlParameter("@Action", "SELECT_ByMID");
                    arrParam2[1] = new SqlParameter("@MID", recordId);
                    DataSet DSInner = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_KitDetails_CRUD", arrParam2);
                    // Bind Table
                    CreateDTTable();
                    TempDT = ViewState["DTTable"] as DataTable;
                    for (int i = 0; i < DSInner.Tables[0].Rows.Count; i++)
                    {
                        string SelQuery = "SELECT * FROM RawMaterial Where ID='" + DSInner.Tables[0].Rows[i]["PID"] + "'";
                        DataTable DTProduct = clsConnectionSql.filldt(SelQuery);

                        double ItemPrice, Quantity, GTotal;
                        Quantity = Convert.ToDouble(DSInner.Tables[0].Rows[i]["Qty"].ToString());
                        ItemPrice = Convert.ToDouble(DTProduct.Rows[0]["PurPrice"].ToString());
                        GTotal = ItemPrice * Quantity;

                        TempDT.Rows.Add(DTProduct.Rows[0]["ID"],
                            DTProduct.Rows[0]["Name"].ToString(),
                            DTProduct.Rows[0]["Barcode"].ToString(),
                            Quantity.ToString("0.00"),
                            ItemPrice.ToString("0.00"),
                            GTotal.ToString("0.00"));
                    }
                    GVInner.DataSource = TempDT;
                    GVInner.DataBind();
                    ViewState["DTTable"] = TempDT;
                    btnSubmit.Text = "Update";
                }
            }
            #endregion

            #region For Delete Record
            if (e.CommandName == "DeleteRecord")
            {
                // Delete Image From Folder
                SqlParameter[] arrParamDel = new SqlParameter[2];
                arrParamDel[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParamDel[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arrParamDel);
                if (DSRecords.Tables[0].Rows[0]["Image"].ToString() != "" && DSRecords.Tables[0].Rows[0]["Image"].ToString() != "~/AdminImages/noimg.png")
                {
                    File.Delete(Server.MapPath(DSRecords.Tables[0].Rows[0]["Image"].ToString()));
                }

                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "DELETE");
                arrParam[1] = new SqlParameter("@ID", recordId);
                int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arrParam);
            }
            #endregion

            #region For Change Status
            if (e.CommandName == "ChangeStatus")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arrParam);
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
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arrParam2);
                    }
                    #endregion

                    #region Change Status False TO True
                    if (DTRecords.Rows[0]["Status"].ToString() == "False")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", true);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arrParam2);
                    }
                    #endregion
                }
            }
            #endregion

            BindGVKitMaterial();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void GVKitMaterial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVKitMaterial.PageIndex = e.NewPageIndex;
        BindGVKitMaterial();
        GVKitMaterial.DataBind();
    }

}