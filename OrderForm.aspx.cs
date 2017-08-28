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
public partial class OrderForm : System.Web.UI.Page
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
            BindOrderFormGV();
            BindSupplier();
        }
    }
    public void BindOrderFormGV()
    {
        try
        {
            string str = "SELECT dbo.OrderRecord.ID, dbo.OrderRecord.ComId, dbo.OrderRecord.NGOId, dbo.OrderRecord.GodId, dbo.OrderRecord.ItemId,";
            str += " dbo.OrderRecord.Balance, dbo.RawMaterial.Name,dbo.RawMaterial.HSNCode, dbo.RawMaterial.Barcode, dbo.RawMaterial.UOM, dbo.RawMaterial.PurPrice,";
            str += " dbo.RawMaterial.Category, dbo.CatMaster.CatName, dbo.CatMaster.TaxPer FROM dbo.OrderRecord INNER JOIN ";
            str += " dbo.RawMaterial ON dbo.OrderRecord.ItemId = dbo.RawMaterial.ID INNER JOIN";
            str += " dbo.CatMaster ON dbo.RawMaterial.Category = dbo.CatMaster.ID";
            DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GVOrderMaster.DataSource = ds;
                GVOrderMaster.DataBind();
            }
            else
            {
                GVOrderMaster.EmptyDataText = " No Record Found";
                GVOrderMaster.DataBind();
            }
        }
        catch(Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Error);
        }
       
    }
    public void BindSupplier()
    {
        try
        {
            SqlParameter[] arr = new SqlParameter[1];
            arr[0] = new SqlParameter("@Action", "Select");
            DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.StoredProcedure, "SP_Supplier_CRUD", arr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLSuplList.DataSource = ds;

                DDLSuplList.DataTextField = "Name";
                DDLSuplList.DataValueField = "ID";
                DDLSuplList.DataBind();
                DDLSuplList.Items.Insert(0, new ListItem("-Select-"));
            }
            else
            {
                DDLSuplList.Text = "No Item";
                DDLSuplList.DataBind();
            }
            
        }

        catch (Exception e)
        {
            ShowMessage(e.Message, MessageType.Error);
        }
    }


    protected void chkHeader_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkHeader = (CheckBox)GVOrderMaster.HeaderRow.FindControl("HeaderCheck");
        if (chkHeader.Checked)
        {
            foreach (GridViewRow gvrow in GVOrderMaster.Rows)
            {
                CheckBox chkRow = (CheckBox)gvrow.FindControl("ChkSelection");
                chkRow.Checked = true;
            }
        }
        else
        {
            foreach (GridViewRow gvrow in GVOrderMaster.Rows)
            {
                CheckBox chkRow = (CheckBox)gvrow.FindControl("ChkSelection");
                chkRow.Checked = false;
            }
        }

    }

    protected void chkRow_CheckedChanged(object sender, EventArgs e)
    {
        int count = 0;
        int totalRowCountGrid = GVOrderMaster.Rows.Count;

        CheckBox chkHeader = (CheckBox)GVOrderMaster.HeaderRow.FindControl("HeaderCheck");

        foreach (GridViewRow gvrow in GVOrderMaster.Rows)
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
    protected void Generate_PO(object sender, EventArgs e)
    {
        try
        {
            decimal[] TotalQty = new decimal[2];
            TotalQty = GetTotal();


            int MID = 0;
            #region InsertPO Master Data
            SqlParameter[] arrParam = new SqlParameter[16];
            arrParam[0] = new SqlParameter("@Action", "INSERT");
            arrParam[1] = new SqlParameter("@ComID", Session["ComID"]);
            arrParam[2] = new SqlParameter("@NGOID", Session["NGOID"]);
            arrParam[3] = new SqlParameter("@PONo", GetPONO());
            arrParam[4] = new SqlParameter("@PODate", DateTime.Now.ToString("dd/MM/yyyy"));
            arrParam[5] = new SqlParameter("@DeliveryDate", DateTime.Now.ToString("dd/MM/yyyy"));
            arrParam[6] = new SqlParameter("@DeliverTo", GetGodown());
            arrParam[7] = new SqlParameter("@Supplier", DDLSuplList.SelectedValue);
            arrParam[8] = new SqlParameter("@SupplierRef", "");
            arrParam[9] = new SqlParameter("@TermsCondition", "");
            arrParam[10] = new SqlParameter("@PaymentTerms", GetPaymentTerms(DDLSuplList.SelectedValue));
            arrParam[11] = new SqlParameter("@TotalQty", TotalQty[0]);
            arrParam[12] = new SqlParameter("@TotalValue", TotalQty[1]);
            arrParam[13] = new SqlParameter("@Remark", "");
            arrParam[14] = new SqlParameter("@POStatus", "0");
            arrParam[15] = new SqlParameter("@CreatedBy", Session["UserName"].ToString());
            int result = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_POMaster_CRUD", arrParam);

            #endregion

            string SelQuery = "SELECT TOP(1) ID FROM POMaster ORDER BY ID DESC";
            DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, SelQuery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                MID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            foreach (GridViewRow row in GVOrderMaster.Rows)
            {
                CheckBox check = (CheckBox)row.FindControl("ChkSelection");
                HiddenField hfid = (HiddenField)row.FindControl("HDFD");
                Label LB_Id = (Label)row.FindControl("LBItem");
                Label LB_Cat = (Label)row.FindControl("LB_Cat");
                Label LB_HSN = (Label)row.FindControl("LB_HSN");
                Label LB_Barcode = (Label)row.FindControl("LBI_Barcode");
                Label LB_UOM = (Label)row.FindControl("LBI_UOM");
                Label LB_Price = (Label)row.FindControl("LBI_PRice");
                Label LB_Tax = (Label)row.FindControl("LB_tax");
                Label LB_Bal = (Label)row.FindControl("LB_Bal");
                if (check.Checked)
                {
                    string ItemId = hfid.Value;
                    decimal Balance = Convert.ToDecimal(LB_Bal.Text);
                    string HSN = LB_HSN.Text;
                    string Barcode = LB_Barcode.Text;
                    decimal Price = Convert.ToDecimal(LB_Price.Text);
                    string Tax = LB_Tax.Text;
                    string Cat = LB_Cat.Text;
                    string UOM = LB_UOM.Text;

                    decimal CGSTPer = Convert.ToDecimal(Tax) / 2;
                    decimal CGSTValue = (Price * CGSTPer) / 100;
                    decimal SGSTPer = Convert.ToDecimal(Tax) / 2;
                    decimal SGSTValue = (Price * SGSTPer) / 100;

                    decimal TotalPrice = Balance * Price;

                    SqlParameter[] arrParamDetails = new SqlParameter[11];
                    arrParamDetails[0] = new SqlParameter("@Action", "INSERT");
                    arrParamDetails[1] = new SqlParameter("@POID", MID);
                    arrParamDetails[2] = new SqlParameter("@ItemID", ItemId);
                    arrParamDetails[3] = new SqlParameter("@Qty", Balance);
                    arrParamDetails[4] = new SqlParameter("@DiscountPer", "0.00");
                    arrParamDetails[5] = new SqlParameter("@UnitPrice", Price);
                    arrParamDetails[6] = new SqlParameter("@CGSTPer", CGSTPer);
                    arrParamDetails[7] = new SqlParameter("@CGSTValue", CGSTValue);
                    arrParamDetails[8] = new SqlParameter("@SGSTPer", SGSTPer);
                    arrParamDetails[9] = new SqlParameter("@SGSTValue", SGSTValue);
                    arrParamDetails[10] = new SqlParameter("@TotalPrice", TotalPrice);
                    int resultInner = SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.StoredProcedure, "SP_PODetail_CRUD", arrParamDetails);
                }
               
               
            }
        } 
        catch (Exception ex)
        {
            ShowMessage(ex.Message, MessageType.Warning);
        }
        clearCheck();
       // Response.Redirect("POManager.aspx");
        Response.Redirect("POManager.aspx?View=1");
        //string s= Response.QueryString[MVPO.ActiveViewIndex=1]);

        
    }

    private string GetPaymentTerms(string SupID)
    {
        string qry = "SELECT PaymentTerms FROM Supplier WHERE ID='" + SupID + "'";
        DataSet DSPyTerms = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, qry);
        if (DSPyTerms.Tables[0].Rows.Count > 0)
            return DSPyTerms.Tables[0].Rows[0]["PaymentTerms"].ToString();
        else
            return "0";
    }

    private string GetGodown()
    {
        string str = "Select ID From Godown where NGOID='" + Session["NGOId"] + "' and ComID='" + Session["ComId"] + "'";
        DataSet ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
            return ds.Tables[0].Rows[0][0].ToString();
        else
            return "0";
    }

    private string GetPONO()
    {
        string PONO = "";
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
            PONO = DocumentCode.ToString();
        }
        return PONO;
    }
    
    private decimal[] GetTotal()
    {
        decimal[] Array = new decimal[2];

        foreach (GridViewRow gvr in GVOrderMaster.Rows)
        {
            CheckBox cb = (CheckBox)gvr.FindControl("ChkSelection");
           
            if (cb.Checked)
            {
                Label lblqty = (Label)gvr.FindControl("LB_Bal");
                Label lblvalue = (Label)gvr.FindControl("LBI_PRice");
                Array[0] += Convert.ToDecimal(lblqty.Text);
                Array[1] += Convert.ToDecimal(lblqty.Text) * Convert.ToDecimal(lblvalue.Text);
            }
        }

        return Array;
    }
    protected void GVOrderMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVOrderMaster.PageIndex = e.NewPageIndex;
        BindOrderFormGV();
      //  GVOrderMaster.DataBind();
    }
    public void clearCheck()
    {
        foreach (GridViewRow row in GVOrderMaster.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("ChkSelection");
            HiddenField hfid = (HiddenField)row.FindControl("HDFD");
            if (chk.Checked)
            {   
                string id1=hfid.Value;
                //int id = Convert.ToInt32(row.Cells[1].Text);
                DeleteRecord(id1);
            }
         }
    }
    private void DeleteRecord(string id)
    {
        string str = "Delete from OrderRecord where ItemId='" + id + "'";
        int result= SqlHelper.ExecuteNonQuery(Connection.connect(), CommandType.Text, str);
        //if (result > 0)
        //{
        //    BindOrderFormGV();
        //}
    }
}