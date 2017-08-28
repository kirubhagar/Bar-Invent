using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

public partial class RTInventory : System.Web.UI.Page
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
        if (DTStockItem.Columns.Count == 0)
        {
            DTStockItem.Columns.Add("ID", typeof(int));
            DTStockItem.Columns.Add("Image", typeof(string));
            DTStockItem.Columns.Add("Name", typeof(string));
            DTStockItem.Columns.Add("PCode", typeof(string));
            DTStockItem.Columns.Add("HSNCode", typeof(string));
            DTStockItem.Columns.Add("BCode", typeof(string));
            DTStockItem.Columns.Add("Price", typeof(string));
            DTStockItem.Columns.Add("TotalStock", typeof(string));
            DTStockItem.Columns.Add("KitQty", typeof(string));
            DTStockItem.Columns.Add("FinishQty", typeof(string));
        }

        DataSet DSItem = new DataSet();

        switch (DDLItemType.SelectedItem.Text)
        {
            case "Raw Material":
                SqlParameter[] arrparm = new SqlParameter[1];
                arrparm[0] = new SqlParameter("@Action", "SELECT");
                DSItem = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_RawMaterial_CRUD", arrparm);
                break;
            
            default:
                break;
        }

        DataTable DTItem = DSItem.Tables[0];

        if (DTItem.Rows.Count > 0)
        {
            for (int i = 0; i < DTItem.Rows.Count; i++)
            {
                DTStockItem.Rows.Add(DTItem.Rows[i]["ID"],
                                     DTItem.Rows[i]["Image"],
                                     DTItem.Rows[i]["Name"],
                                     DTItem.Rows[i]["ProductCode"],
                                     DTItem.Rows[i]["HSNCode"],
                                     DTItem.Rows[i]["Barcode"],
                                     DTItem.Rows[i]["PurPrice"],
                                     GetTotalStock(Convert.ToInt32(DTItem.Rows[i]["ID"]),Convert.ToInt32(DDLGodown.SelectedValue)),
                                     0,
                                     0);
            }
            GVItem.DataSource = DTStockItem;
            GVItem.DataBind();
        }
    }

    private int GetTotalStock(int ItemID, int GodownID)
    {
        SqlParameter[] arrparm = new SqlParameter[3];
        arrparm[0] = new SqlParameter("@Action", "SELECT_ByItemID");
        arrparm[1] = new SqlParameter("@ItemID", ItemID);
        arrparm[2] = new SqlParameter("@GodownID", GodownID);
        DataSet  DSItem = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_ItemStock_CRUD", arrparm);
        if (DSItem.Tables[0].Rows.Count > 0)
        {
            return Convert.ToInt32(DSItem.Tables[0].Rows[0]["TotalStock"]);
        }
        else
            return 0;
    }

    #region Coding

    protected void RBCheckedChanged(object sender, EventArgs e)
    {
        if (RBKit.Checked)
            BindDDLMaterial("Kit");       
        else if (RBFinish.Checked)
            BindDDLMaterial("Finish");
    }

    private void BindDDLMaterial(string p)
    {
        switch (p)
        { 
            case "Kit":
                DDLMaterial.Items.Clear();
                LBLMaterialName.Text = "Kit Material";
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@Action", "SELECT");
                DataSet DSRecordKit = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_KitMaterial_CRUD", arrParam);
                if (DSRecordKit.Tables[0].Rows.Count > 0)
                {
                    DDLMaterial.DataSource = DSRecordKit.Tables[0];
                    DDLMaterial.DataTextField = "Name";
                    DDLMaterial.DataValueField = "ID";
                    DDLMaterial.DataBind();
                    DDLMaterial.Items.Insert(0, new ListItem("Select", "0"));
                }
                break;
            case "Finish":
                DDLMaterial.Items.Clear();
                LBLMaterialName.Text = "Finish Material";
                SqlParameter[] arrparam = new SqlParameter[1];
                arrparam[0] = new SqlParameter("@Action", "SELECT");
                DataSet DSRecordFinish = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_FinishMaterial_CRUD", arrparam);
                if (DSRecordFinish.Tables[0].Rows.Count > 0)
                {
                    DDLMaterial.DataSource = DSRecordFinish.Tables[0];
                    DDLMaterial.DataTextField = "Name";
                    DDLMaterial.DataValueField = "ID";
                    DDLMaterial.DataBind();
                    DDLMaterial.Items.Insert(0, new ListItem("Select", "0"));
                }
                break;
            default :
                break;
        }
    }

    protected void LBMaterial_Click(object sender, EventArgs e)
    {
        int MaterialID=Convert.ToInt32(DDLMaterial.SelectedValue);
        
        string RBText="";
        if(RBKit.Checked)   RBText="Kit";
        else if(RBFinish.Checked) RBText="Finish";

        switch (RBText)
        {
            case "Kit":
                LBLMaterialName.Text = "Kit Material";
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Action", "SELECTView_ByMID");
                arrParam[1] = new SqlParameter("@MID", MaterialID);
                DataSet DSRecordKit = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_KitDetails_CRUD", arrParam);
                if (DSRecordKit.Tables[0].Rows.Count > 0)
                {
                    if (DTStockItem.Columns.Count == 0)
                    {
                        DTStockItem.Columns.Add("ID", typeof(int));
                        DTStockItem.Columns.Add("Image", typeof(string));
                        DTStockItem.Columns.Add("Name", typeof(string));
                        DTStockItem.Columns.Add("PCode", typeof(string));
                        DTStockItem.Columns.Add("HSNCode", typeof(string));
                        DTStockItem.Columns.Add("BCode", typeof(string));
                        DTStockItem.Columns.Add("Price", typeof(string));
                        DTStockItem.Columns.Add("TotalStock", typeof(string));
                        DTStockItem.Columns.Add("KitQty", typeof(string));
                        DTStockItem.Columns.Add("FinishQty", typeof(string));
                    }

                    DataSet DSItem = new DataSet();

                    switch (DDLItemType.SelectedItem.Text)
                    {
                        case "Raw Material":
                            string str = " SELECT dbo.RawMaterial.*,dbo.KitDetails.* FROM dbo.RawMaterial INNER JOIN dbo.KitDetails ON ";
                            str += " dbo.RawMaterial.ID = dbo.KitDetails.PID INNER JOIN dbo.ItemStock ON dbo.RawMaterial.ID = dbo.ItemStock.ItemID ";
                            str += " WHERE (dbo.KitDetails.MID = '" + MaterialID + "') AND (dbo.ItemStock.GodownID = '" + DDLGodown.SelectedValue + "')";
                            DSItem = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, str);
                            break;
                        default:
                            break;
                    }

                    DataTable DTItem = DSItem.Tables[0];

                    if (DTItem.Rows.Count > 0)
                    {
                        for (int i = 0; i < DTItem.Rows.Count; i++)
                        {
                            DTStockItem.Rows.Add(DTItem.Rows[i]["ID"],
                                                 DTItem.Rows[i]["Image"],
                                                 DTItem.Rows[i]["Name"],
                                                 DTItem.Rows[i]["ProductCode"],
                                                 DTItem.Rows[i]["HSNCode"],
                                                 DTItem.Rows[i]["Barcode"],
                                                 DTItem.Rows[i]["PurPrice"],
                                                 GetTotalStock(Convert.ToInt32(DTItem.Rows[i]["ID"]), Convert.ToInt32(DDLGodown.SelectedValue)),
                                                 GetTotalStock(Convert.ToInt32(DTItem.Rows[i]["ID"]), Convert.ToInt32(DDLGodown.SelectedValue))/Convert.ToInt32(DSRecordKit.Tables[0].Rows[i]["Qty"]),
                                                 0);
                        }
                        GVItem.DataSource = DTStockItem;
                        GVItem.DataBind();
                    }
                }
                break;
            case "Finish":
                LBLMaterialName.Text = "Finish Material";
                SqlParameter[] arrparam = new SqlParameter[2];
                arrparam[0] = new SqlParameter("@Action", "SELECTView_ByMID");
                arrparam[1] = new SqlParameter("@MID", MaterialID);
                DataSet DSRecordFinish = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_FinishDetails_CRUD", arrparam);
                if (DSRecordFinish.Tables[0].Rows.Count > 0)
                {
                    if (DTStockItem.Columns.Count == 0)
                    {
                        DTStockItem.Columns.Add("ID", typeof(int));
                        DTStockItem.Columns.Add("Image", typeof(string));
                        DTStockItem.Columns.Add("Name", typeof(string));
                        DTStockItem.Columns.Add("PCode", typeof(string));
                        DTStockItem.Columns.Add("HSNCode", typeof(string));
                        DTStockItem.Columns.Add("BCode", typeof(string));
                        DTStockItem.Columns.Add("Price", typeof(string));
                        DTStockItem.Columns.Add("TotalStock", typeof(string));
                        DTStockItem.Columns.Add("KitQty", typeof(string));
                        DTStockItem.Columns.Add("FinishQty", typeof(string));
                    }

                    DataSet DSItem = new DataSet();

                    switch (DDLItemType.SelectedItem.Text)
                    {
                        case "Raw Material":
                            string str = " SELECT dbo.RawMaterial.*,dbo.FinishDetails.* FROM dbo.RawMaterial INNER JOIN dbo.FinishDetails ON ";
                            str += " dbo.RawMaterial.ID = dbo.FinishDetails.PID INNER JOIN dbo.ItemStock ON dbo.RawMaterial.ID = dbo.ItemStock.ItemID ";
                            str += " WHERE (dbo.FinishDetails.MID = '" + MaterialID + "') AND (dbo.ItemStock.GodownID = '" + DDLGodown.SelectedValue + "')";
                            DSItem = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, str);
                            break;
                        default:
                            break;
                    }

                    DataTable DTItem = DSItem.Tables[0];

                    if (DTItem.Rows.Count > 0)
                    {
                        for (int i = 0; i < DTItem.Rows.Count; i++)
                        {
                            DTStockItem.Rows.Add(DTItem.Rows[i]["ID"],
                                                 DTItem.Rows[i]["Image"],
                                                 DTItem.Rows[i]["Name"],
                                                 DTItem.Rows[i]["ProductCode"],
                                                 DTItem.Rows[i]["HSNCode"],
                                                 DTItem.Rows[i]["Barcode"],
                                                 DTItem.Rows[i]["PurPrice"],
                                                 GetTotalStock(Convert.ToInt32(DTItem.Rows[i]["ID"]), Convert.ToInt32(DDLGodown.SelectedValue)),
                                                 0,
                                                 GetTotalStock(Convert.ToInt32(DTItem.Rows[i]["ID"]), Convert.ToInt32(DDLGodown.SelectedValue)) / Convert.ToInt32(DSRecordFinish.Tables[0].Rows[i]["Qty"]));
                        }
                        GVItem.DataSource = DTStockItem;
                        GVItem.DataBind();
                    }
                }
                break;
            default:
                break;
        }
    }

    #endregion

}