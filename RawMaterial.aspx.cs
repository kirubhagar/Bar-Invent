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

public partial class RawMaterial : System.Web.UI.Page
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
            BindGVRawMaterial();
            BindDDLWarehouse();
        }
    }

    private void BindGVRawMaterial()
    {
        try
        {
            SqlParameter[] arrParam = new SqlParameter[1];
            arrParam[0] = new SqlParameter("@Action", "SELECT_ForGV");
            DataSet DSUser = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParam);
            if (DSUser.Tables[0].Rows.Count > 0)
            {
                GVRawMaterial.DataSource = DSUser.Tables[0];
                GVRawMaterial.DataBind();
            }
            else
            {
                GVRawMaterial.EmptyDataText = "No Record Found !";
                GVRawMaterial.DataBind();
            }
            foreach (GridViewRow Row in GVRawMaterial.Rows)
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
        MVRawMatrial.ActiveViewIndex = 1;
        BindCategory();
        BindSupplier();
    }

    private void BindSupplier()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT");
        DataSet DSSupplier = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Supplier_CRUD", arrParam);
        if (DSSupplier.Tables[0].Rows.Count > 0)
        {
            DDLSupplier.DataSource = DSSupplier.Tables[0];
            DDLSupplier.DataTextField = "Name";
            DDLSupplier.DataValueField = "ID";
            DDLSupplier.DataBind();
            DDLSupplier.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void BindCategory()
    {
        SqlParameter[] arrParam = new SqlParameter[1];
        arrParam[0] = new SqlParameter("@Action", "SELECT");
        DataSet DSCat = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_CatMaster_CRUD", arrParam);
        if (DSCat.Tables[0].Rows.Count > 0)
        {
            DDLCat.DataSource = DSCat.Tables[0];
            DDLCat.DataTextField = "CatName";
            DDLCat.DataValueField = "ID";
            DDLCat.DataBind();
            DDLCat.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    protected void DDlCat_SelectedIndexChang(object sender, EventArgs e)
    {
        if (DDLCat.SelectedIndex > 0)
        {
            SqlParameter[] arrParam = new SqlParameter[2];
            arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
            arrParam[1] = new SqlParameter("@ID", DDLCat.SelectedValue);
            DataSet DSCat = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_CatMaster_CRUD", arrParam);
            if (DSCat.Tables[0].Rows.Count > 0)
            {
                TBHSN.Text = DSCat.Tables[0].Rows[0]["HSNCode"].ToString();
            }
        }
        else
            TBHSN.Text = string.Empty;
    }

    protected void CBShowImage_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CBShowImage.Checked)
                GVRawMaterial.Columns[1].Visible = true;
            else
                GVRawMaterial.Columns[1].Visible = false;
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
            DataSet DSUser = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParam);

            if (DSUser.Tables[0].Rows.Count > 0)
            {
                GVRawMaterial.DataSource = DSUser.Tables[0];
                GVRawMaterial.DataBind();
            }
            else
            {
                GVRawMaterial.EmptyDataText = "No Record Found !";
                GVRawMaterial.DataBind();
            }
            foreach (GridViewRow Row in GVRawMaterial.Rows)
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

                if (btnSubmit.Text == "Submit")
                {
                    //CheckBin();
                    SqlParameter[] arrParam = new SqlParameter[16];
                    arrParam[0] = new SqlParameter("@Action", "INSERT");
                    arrParam[1] = new SqlParameter("@Image", ImagePath);
                    arrParam[2] = new SqlParameter("@Name", TBName.Text);
                    arrParam[3] = new SqlParameter("@ProductCode", TBProductCode.Text);
                    arrParam[4] = new SqlParameter("@HSNCode", TBHSN.Text);
                    arrParam[5] = new SqlParameter("@Barcode", TBBarcode.Text);
                    arrParam[6] = new SqlParameter("@UOM", TBUOM.Text);
                    arrParam[7] = new SqlParameter("@Category", DDLCat.SelectedValue);
                    arrParam[8] = new SqlParameter("@Supplier", DDLSupplier.SelectedValue);
                    arrParam[9] = new SqlParameter("@Description", TBDescription.Text);
                    arrParam[10] = new SqlParameter("@PurPrice", TBPurPrice.Text);
                    arrParam[11] = new SqlParameter("@SalePrice", TBSalePrice.Text);
                    //arrParam[12] = new SqlParameter("@OpeningStock", Convert.ToDecimal(string.IsNullOrEmpty(TBOpeningStock.Text) ? "0.00" : TBOpeningStock.Text));
                    //arrParam[13] = new SqlParameter("@OpeningStockDate", TBOSDate.Text);
                    //arrParam[14] = new SqlParameter("@ReorderQty", Convert.ToDecimal(string.IsNullOrEmpty(TBReorderQty.Text) ? "0.00" : TBReorderQty.Text));
                    //arrParam[15] = new SqlParameter("@BinLocation", TBBinLocation.Text);
                    arrParam[12] = new SqlParameter("@MFGDate", TBMFGDate.Text);
                    arrParam[13] = new SqlParameter("@ExpireDate", TBExpireDate.Text);
                    arrParam[14] = new SqlParameter("@CreatedOn", DateTime.Now);
                    arrParam[15] = new SqlParameter("@CreatedBy", Session["FullName"].ToString());

                    int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParam);
                    if (result > 0)
                    {
                        int RawMaterialID = GetLastID();
                        int GodownID = GetGodownID();
                        InsertStockEntry(RawMaterialID,GodownID);
                        InsertStockLedgerDetails(RawMaterialID,GodownID);

                        ShowMessage("Added Suceess", MessageType.Success);
                    }
                    else
                        ShowMessage("Some Technical Error Occur!!!", MessageType.Error);
                    ClearUserForm();
                    MVRawMatrial.ActiveViewIndex = 0;
                    BindGVRawMaterial();
                }
                else if (btnSubmit.Text == "Update")
                {
                    SqlParameter[] arrParam = new SqlParameter[15];
                    arrParam[0] = new SqlParameter("@Action", "UPDATE");
                    arrParam[1] = new SqlParameter("@Image", ImagePath);
                    arrParam[2] = new SqlParameter("@Name", TBName.Text);
                    arrParam[3] = new SqlParameter("@ProductCode", TBProductCode.Text);
                    arrParam[4] = new SqlParameter("@HSNCode", TBHSN.Text);
                    arrParam[5] = new SqlParameter("@Barcode", TBBarcode.Text);
                    arrParam[6] = new SqlParameter("@UOM", TBUOM.Text);
                    arrParam[7] = new SqlParameter("@Category", DDLCat.SelectedValue);
                    arrParam[8] = new SqlParameter("@Supplier", DDLSupplier.SelectedValue);
                    arrParam[9] = new SqlParameter("@Description", TBDescription.Text);
                    arrParam[10] = new SqlParameter("@PurPrice", TBPurPrice.Text);
                    arrParam[11] = new SqlParameter("@SalePrice", TBSalePrice.Text);
                    //arrParam[12] = new SqlParameter("@OpeningStock", Convert.ToDecimal(string.IsNullOrEmpty(TBOpeningStock.Text) ? "0.00" : TBOpeningStock.Text));
                    //arrParam[13] = new SqlParameter("@OpeningStockDate", TBOSDate.Text);
                    //arrParam[14] = new SqlParameter("@ReorderQty", Convert.ToDecimal(string.IsNullOrEmpty(TBReorderQty.Text) ? "0.00" : TBReorderQty.Text));
                    //arrParam[15] = new SqlParameter("@BinLocation", TBBinLocation.Text);
                    arrParam[12] = new SqlParameter("@MFGDate", TBMFGDate.Text);
                    arrParam[13] = new SqlParameter("@ExpireDate", TBExpireDate.Text);
                    arrParam[14] = new SqlParameter("@ID", Session["RecordId"]);
                    int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParam);
                    if (result > 0)
                    {
                        int RawMaterialID = Convert.ToInt32(Session["RecordId"]);
                        int GodownID = GetGodownID();
                        InsertUpdateStockEntry(RawMaterialID, GodownID);
                        InsertUpdateStockLedgerDetails(RawMaterialID, GodownID);
                        ShowMessage("Updated Successfully", MessageType.Success);
                    }
                    else
                        ShowMessage("Error", MessageType.Error);

                    ClearUserForm();
                    MVRawMatrial.ActiveViewIndex = 0;
                    BindGVRawMaterial();
                    btnSubmit.Text = "Submit";
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    private void InsertStockEntry(int RawMaterialID, int GodownID)
    {
        SqlParameter[] arrParam = new SqlParameter[9];
        arrParam[0] = new SqlParameter("@Action", "INSERT");
        arrParam[1] = new SqlParameter("@GodownID",GodownID );
        arrParam[2] = new SqlParameter("@ItemID", RawMaterialID);
        arrParam[3] = new SqlParameter("@ItemType", "Raw Material");
        arrParam[4] = new SqlParameter("@OpeningStock", TBOpeningStock.Text);
        arrParam[5] = new SqlParameter("@OSDate", TBOSDate.Text);
        arrParam[6] = new SqlParameter("@ReorderQty", TBReorderQty.Text);
        arrParam[7] = new SqlParameter("@TotalStock", TBOpeningStock.Text);
        arrParam[8] = new SqlParameter("@BinLocation", TBBinLocation.Text);
        
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemStock_CRUD", arrParam);
                    
    }

    private void InsertUpdateStockEntry(int RawMaterialID, int GodownID)
    {
        string Qry = "SELECT * FROM ItemStock WHERE ItemID='" + RawMaterialID + "' AND GodownID='" + GodownID + "'";
        DataSet DSItem = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, Qry);
        if (DSItem.Tables[0].Rows.Count > 0)
        {
            SqlParameter[] arrParam = new SqlParameter[10];
            arrParam[0] = new SqlParameter("@Action", "UPDATE");
            arrParam[1] = new SqlParameter("@GodownID", GodownID);
            arrParam[2] = new SqlParameter("@ItemID", RawMaterialID);
            arrParam[3] = new SqlParameter("@ItemType", "Raw Material");
            arrParam[4] = new SqlParameter("@OpeningStock", TBOpeningStock.Text);
            arrParam[5] = new SqlParameter("@OSDate", TBOSDate.Text);
            arrParam[6] = new SqlParameter("@ReorderQty", TBReorderQty.Text);
            arrParam[7] = new SqlParameter("@TotalStock", TBOpeningStock.Text);
            arrParam[8] = new SqlParameter("@BinLocation", TBBinLocation.Text);
            arrParam[9] = new SqlParameter("@ID", Convert.ToInt32(DSItem.Tables[0].Rows[0]["ID"]));
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemStock_CRUD", arrParam);
        }
        else
        {
            SqlParameter[] arrParam = new SqlParameter[9];
            arrParam[0] = new SqlParameter("@Action", "INSERT");
            arrParam[1] = new SqlParameter("@GodownID", GodownID);
            arrParam[2] = new SqlParameter("@ItemID", RawMaterialID);
            arrParam[3] = new SqlParameter("@ItemType", "Raw Material");
            arrParam[4] = new SqlParameter("@OpeningStock", TBOpeningStock.Text);
            arrParam[5] = new SqlParameter("@OSDate", TBOSDate.Text);
            arrParam[6] = new SqlParameter("@ReorderQty", TBReorderQty.Text);
            arrParam[7] = new SqlParameter("@TotalStock", TBOpeningStock.Text);
            arrParam[8] = new SqlParameter("@BinLocation", TBBinLocation.Text);
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_ItemStock_CRUD", arrParam);
        }
    }

    private void InsertStockLedgerDetails(int RawMaterialID , int GodownID)
    {
        SqlParameter[] arrParam = new SqlParameter[10];
        arrParam[0] = new SqlParameter("@Action", "INSERT");
        arrParam[1] = new SqlParameter("@GodownID", GodownID);
        arrParam[2] = new SqlParameter("@ItemID", RawMaterialID);
        arrParam[3] = new SqlParameter("@ItemType", "Raw Material");
        arrParam[4] = new SqlParameter("@Date", TBOSDate.Text);
        arrParam[5] = new SqlParameter("@StockIN", TBOpeningStock.Text);
        arrParam[6] = new SqlParameter("@StockOut", "0.00");
        arrParam[7] = new SqlParameter("@Narration", "Opening Stock");
        arrParam[8] = new SqlParameter("@DocType", "None");
        arrParam[9] = new SqlParameter("@DocNo", "None");
        int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_StockLedger_CRUD", arrParam);
    }

    private void InsertUpdateStockLedgerDetails(int RawMaterialID, int GodownID)
    {
        string Qry = "SELECT * FROM StockLedger WHERE ItemID='" + RawMaterialID + "' AND GodownID='" + GodownID + "'";
        DataSet DSItem = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, Qry);
        if (DSItem.Tables[0].Rows.Count > 0)
        {
            SqlParameter[] arrParam = new SqlParameter[11];
            arrParam[0] = new SqlParameter("@Action", "UPDATE");
            arrParam[1] = new SqlParameter("@GodownID", GodownID);
            arrParam[2] = new SqlParameter("@ItemID", RawMaterialID);
            arrParam[3] = new SqlParameter("@ItemType", "Raw Material");
            arrParam[4] = new SqlParameter("@Date", TBOSDate.Text);
            arrParam[5] = new SqlParameter("@StockIN", TBOpeningStock.Text);
            arrParam[6] = new SqlParameter("@StockOut", "0.00");
            arrParam[7] = new SqlParameter("@Narration", "Opening Stock");
            arrParam[8] = new SqlParameter("@DocType", "None");
            arrParam[9] = new SqlParameter("@DocNo", "None");
            arrParam[10] = new SqlParameter("@ID", Convert.ToInt32(DSItem.Tables[0].Rows[0]["ID"]));
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_StockLedger_CRUD", arrParam);
        }
        else
        {
            SqlParameter[] arrParam = new SqlParameter[10];
            arrParam[0] = new SqlParameter("@Action", "INSERT");
            arrParam[1] = new SqlParameter("@GodownID", GodownID);
            arrParam[2] = new SqlParameter("@ItemID", RawMaterialID);
            arrParam[3] = new SqlParameter("@ItemType", "Raw Material");
            arrParam[4] = new SqlParameter("@Date", TBOSDate.Text);
            arrParam[5] = new SqlParameter("@StockIN", TBOpeningStock.Text);
            arrParam[6] = new SqlParameter("@StockOut", "0.00");
            arrParam[7] = new SqlParameter("@Narration", "Opening Stock");
            arrParam[8] = new SqlParameter("@DocType", "None");
            arrParam[9] = new SqlParameter("@DocNo", "None");
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_StockLedger_CRUD", arrParam);
        }
    }

    private int GetLastID()
    {
        string SelectRMID = "SELECT TOP(1) ID FROM RawMaterial ORDER BY ID DESC";
        DataSet DSComID = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelectRMID);
        if (DSComID.Tables[0].Rows.Count > 0)
            return Convert.ToInt32(DSComID.Tables[0].Rows[0]["ID"]);
        else
            return 0;
    }

    private int GetGodownID()
    {
        string SelectRMID = "SELECT ID FROM Godown WHERE ComID='" + Convert.ToInt32(Session["ComID"]) + "' AND NGOID='" + Convert.ToInt32(Session["NGOID"]) + "'";
        DataSet DSComID = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelectRMID);
        if (DSComID.Tables[0].Rows.Count > 0)
            return Convert.ToInt32(DSComID.Tables[0].Rows[0]["ID"]);
        else
            return 0;
    }

    private string GetImagePath(FileUpload FULogo)
    {
        string ImagePath = "";
        if (FULogo.HasFile)
        {
            string NameImage = DateTime.Now.Millisecond + FULogo.FileName;
            FULogo.SaveAs(Server.MapPath("~/AdminImages/RawMaterial/" + NameImage));
            ImagePath = "~/AdminImages/RawMaterial/" + NameImage;
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
        MVRawMatrial.ActiveViewIndex = 0;
        BindGVRawMaterial();
    }

    private void ClearUserForm()
    {
        TBName.Text = string.Empty;
        TBProductCode.Text = string.Empty;
        TBHSN.Text = string.Empty;
        TBBarcode.Text = string.Empty;
        TBUOM.Text = string.Empty;
        DDLCat.ClearSelection();
        TBOpeningStock.Text = string.Empty;
        TBOSDate.Text = string.Empty;
        TBReorderQty.Text = string.Empty;
        TBBinLocation.Text = string.Empty;
        TBMFGDate.Text = string.Empty;
        TBExpireDate.Text = string.Empty;
        TBDescription.Text = string.Empty;
        TBPurPrice.Text = string.Empty;
        TBSalePrice.Text = string.Empty;
        ImgLogo.ImageUrl = string.Empty;
        btnSubmit.Text = "Submit";
    }

    protected void GVRawMaterial_RowCommand(object sender, GridViewCommandEventArgs e)
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
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParam);
                DataTable DTRecords = DSRecords.Tables[0];
                if (DTRecords.Rows.Count > 0)
                {
                    MVRawMatrial.ActiveViewIndex = 1;
                    TBName.Text = DTRecords.Rows[0]["Name"].ToString();
                    TBProductCode.Text = DTRecords.Rows[0]["ProductCode"].ToString();
                    TBHSN.Text = DTRecords.Rows[0]["HSNCode"].ToString();
                    TBBarcode.Text = DTRecords.Rows[0]["Barcode"].ToString();
                    TBUOM.Text = DTRecords.Rows[0]["UOM"].ToString();
                    BindCategory();
                    DDLCat.ClearSelection();
                    DDLCat.Items.FindByValue(DTRecords.Rows[0]["Category"].ToString()).Selected = true;
                    BindSupplier();
                    DDLSupplier.ClearSelection();
                    DDLSupplier.Items.FindByValue(DTRecords.Rows[0]["Supplier"].ToString()).Selected = true;
                    TBDescription.Text = DTRecords.Rows[0]["Description"].ToString();
                    TBPurPrice.Text = DTRecords.Rows[0]["PurPrice"].ToString();
                    TBSalePrice.Text = DTRecords.Rows[0]["SalePrice"].ToString();
                    int GodownID = GetGodownID();
                    string Query = "SELECT * FROM ItemStock WHERE ItemID='" + recordId + "' AND GodownID='" + GodownID + "'";
                    DataSet DSStockItem = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, Query);
                    if (DSStockItem.Tables[0].Rows.Count > 0)
                    {
                        TBOpeningStock.Text = DSStockItem.Tables[0].Rows[0]["OpeningStock"].ToString();
                        TBOSDate.Text = DSStockItem.Tables[0].Rows[0]["OSDate"].ToString();
                        TBReorderQty.Text = DSStockItem.Tables[0].Rows[0]["ReorderQty"].ToString();
                        TBBinLocation.Text = DSStockItem.Tables[0].Rows[0]["BinLocation"].ToString();    
                    }
                    TBMFGDate.Text = DTRecords.Rows[0]["MFGDate"].ToString();
                    TBExpireDate.Text = DTRecords.Rows[0]["ExpireDate"].ToString();
                    ImgLogo.ImageUrl = DTRecords.Rows[0]["Image"].ToString();
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
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParamDel);
                if (DSRecords.Tables[0].Rows[0]["Image"].ToString() != "" && DSRecords.Tables[0].Rows[0]["Image"].ToString() != "~/AdminImages/noimg.png")
                {
                    File.Delete(Server.MapPath(DSRecords.Tables[0].Rows[0]["Image"].ToString()));
                }

                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "DELETE");
                arrParam[1] = new SqlParameter("@ID", recordId);
                int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParam);
            }
            #endregion

            #region For Change Status
            if (e.CommandName == "ChangeStatus")
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECT_ByID");
                arrParam[1] = new SqlParameter("@ID", recordId);
                DataSet DSRecords = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParam);
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
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParam2);
                    }
                    #endregion

                    #region Change Status False TO True
                    if (DTRecords.Rows[0]["Status"].ToString() == "False")
                    {
                        SqlParameter[] arrParam2 = new SqlParameter[3];
                        arrParam2[0] = new SqlParameter("@Action", "UPDATE_Status");
                        arrParam2[1] = new SqlParameter("@Status", true);
                        arrParam2[2] = new SqlParameter("@ID", recordId);
                        int Result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrParam2);
                    }
                    #endregion
                }
            }
            #endregion

            BindGVRawMaterial();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
    }

    protected void GVRawMaterial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVRawMaterial.PageIndex = e.NewPageIndex;
        BindGVRawMaterial();
        GVRawMaterial.DataBind();
    }

    protected void ModelSubmitBtnClickEvent(object sender, EventArgs e)
    {
        if (sender == ModelCatSubmit)
            BindCategory();
    
        if (sender == ModelSupplierSubmit)
            BindSupplier();

        if (sender == ModelBinSubmit)
        {
            string str = "SELECT * FROM RawMaterial WHERE BinLocation='" + TBModelBinLocation.Text + "'";
            DataSet dsBin = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, str);
            if (dsBin.Tables[0].Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Bin Is not Empty Sorry!')", true);
            }
            else
                TBBinLocation.Text = TBModelBinLocation.Text;
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

}