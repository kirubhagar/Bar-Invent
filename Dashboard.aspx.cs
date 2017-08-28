using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

public partial class Dashboard : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetCounting();
            GetKnobCounting();
            BindLatestPO();
        }
    }

    private void BindLatestPO()
    {
        try
        {
            string STR = "SELECT TOP (5) ID,ComID,NGOID,PONo,PODate,DeliveryDate,DeliverTo,SupplierRef,TermsCondition,";
            STR += " PaymentTerms, TotalQty, TotalValue, Remark, POStatus, CreatedBy, Name ";
            STR += " FROM dbo.View_PO_Details WHERE (POStatus IN (0, 1)) ORDER BY ID DESC";
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

    private void GetKnobCounting()
    {
        // Customer Counting
        string CountCustomer = "SELECT Count(ID) FROM Customer";
        DataTable DTCustomer = clsConnectionSql.filldt(CountCustomer);
        if (DTCustomer.Rows.Count > 0)
        {
            numberCustomer.InnerText = DTCustomer.Rows[0][0].ToString();
            knobcustomer.Value = DTCustomer.Rows[0][0].ToString();
        }
        else
            numberCustomer.InnerText = "0";

        // Sales Counting
        //string CountSales = "SELECT GrandTotal FROM InvoiceMaster";
        //DataTable DTSales = clsConnectionSql.filldt(CountSales);
        //if (DTSales.Rows.Count > 0)
        //{
        //    decimal InvTotal = Convert.ToDecimal(0.0);
        //    for (int i = 0; i < DTSales.Rows.Count; i++)
        //    {
        //        InvTotal += Convert.ToDecimal(DTSales.Rows[i][0]);
        //    }
        //    numberSales.InnerText = InvTotal.ToString() + " " + "/-";
        //}
        //else
        //    numberSales.InnerText = "0";
    }

    private void GetCounting()
    {
        // Supplier Counting
        string CountSupp = "SELECT Count(ID) FROM Supplier";
        DataTable DTSupp = clsConnectionSql.filldt(CountSupp);
        if (DTSupp.Rows.Count > 0)
            SpanSupp.InnerText = DTSupp.Rows[0][0].ToString();
        else
            SpanSupp.InnerText = "0";

        // Customer Counting
        string CountCust = "SELECT Count(ID) FROM Customer";
        DataTable DTCust = clsConnectionSql.filldt(CountCust);
        if (DTCust.Rows.Count > 0)
            SpanCust.InnerText = DTCust.Rows[0][0].ToString();
        else
            SpanCust.InnerText = "0";

        // Employee Counting
        string CountEmp = "SELECT Count(ID) FROM Employee";
        DataTable DTEmp = clsConnectionSql.filldt(CountEmp);
        if (DTEmp.Rows.Count > 0)
            SpanEmp.InnerText = DTEmp.Rows[0][0].ToString();
        else
            SpanEmp.InnerText = "0";

        // Item Counting
        //string CountItem = "SELECT Count(ID) FROM Item";
        //DataTable DTItem = clsConnectionSql.filldt(CountItem);
        //if (DTItem.Rows.Count > 0)
        //    SpanItem.InnerText = DTItem.Rows[0][0].ToString();
        //else
        //    SpanItem.InnerText = "0";

        // Item Categories Counting
        //string CountItemCat = "SELECT Count(ID) FROM ItemCategories";
        //DataTable DTItemCat = clsConnectionSql.filldt(CountItemCat);
        //if (DTItemCat.Rows.Count > 0)
        //    SpanItemCategory.InnerText = DTItemCat.Rows[0][0].ToString();
        //else
        //    SpanItemCategory.InnerText = "0";

        // Offers Counting
        //string CountOffers = "SELECT Count(ID) FROM OfferMaster";
        //DataTable DTOffers = clsConnectionSql.filldt(CountOffers);
        //if (DTOffers.Rows.Count > 0)
        //    SpanOffers.InnerText = DTOffers.Rows[0][0].ToString();
        //else
        //    SpanOffers.InnerText = "0";

        // Sales Invoice Counting
        //string CountSales = "SELECT Count(ID) FROM InvoiceMaster";
        //DataTable DTSales = clsConnectionSql.filldt(CountSales);
        //if (DTSales.Rows.Count > 0)
        //    SpanSales.InnerText = DTSales.Rows[0][0].ToString();
        //else
        //    SpanSales.InnerText = "0";

        // PO Counting
        //string CountPO = "SELECT Count(ID) FROM POMaster";
        //DataTable DTPO = clsConnectionSql.filldt(CountPO);
        //if (DTPO.Rows.Count > 0)
        //    SpanPO.InnerText = DTPO.Rows[0][0].ToString();
        //else
        //    SpanPO.InnerText = "0";
    }

    #region Code For Kit Vs Raw Material Charts
    protected void BindRawDataChart(object sender, EventArgs e)
    {
        //GetData(Convert.ToInt32(DDLKit.SelectedValue));
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<Data> GetData()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string cmdstr = "SELECT dbo.KitDetails.ID, dbo.KitDetails.MID, dbo.RawMaterial.Name, dbo.KitDetails.Qty, dbo.KitDetails.Price";
        cmdstr += " FROM dbo.KitDetails INNER JOIN dbo.RawMaterial ON dbo.KitDetails.PID = dbo.RawMaterial.ID";
        cmdstr += " WHERE dbo.KitDetails.MID='" + 3+ "'";

        ds = SqlHelper.ExecuteDataset(Connection.connect(), CommandType.Text, cmdstr);
        dt = ds.Tables[0];
        List<Data> dataList = new List<Data>();
        string cat = "";
        int val = 0;
        foreach (DataRow dr in dt.Rows)
        {
            cat = dr[2].ToString();
            val = Convert.ToInt32(dr[3]);
            dataList.Add(new Data(cat, val));
        }
        return dataList;
    }
    #endregion
}

public class Data
{
    public string ColumnName = "";
    public int Value = 0;
    public Data(string columnName, int value)
    {
        ColumnName = columnName;
        Value = value;
    }
}
