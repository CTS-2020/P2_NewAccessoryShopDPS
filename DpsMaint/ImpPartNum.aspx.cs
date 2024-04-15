using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Runtime.InteropServices;
using System.Text;
using dpant;
using Itenso.Rtf;
using Itenso.Rtf.Converter.Html;
using Itenso.Rtf.Converter.Image;
using Itenso.Rtf.Converter.Text;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;
//using NReco.ImageGenerator;
//using NReco.PdfGenerator;

public partial class ImpPartNum : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!ClientScript.IsStartupScriptRegistered("redirect"))
        {
            if (Convert.ToString(Session["SessUserId"]) == "")
            {
                Response.Write("<script language='javascript'>alert('Your user session has timed out. Please login again.');window.top.location ='../Login.aspx';</script>");
            }
        }

        if (!IsPostBack)
        {
            try
            {
                NewPageIndex = 0;
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }

        SearchPartsNumList();
    }
    #endregion

    #region Method

    #region SearchPartsNumList
    private void SearchPartsNumList()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = csDatabase.SrcPartsNumList();
            dtSearch = dsSearch.Tables[0];
            BindGridView(dtSearch);
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region BindGridView
    private bool BindGridView(DataTable dtPartsNumList)
    {
        try
        {
            DataView dvPartsNumList = new DataView(dtPartsNumList);

            gvPartsNumList.DataSource = dvPartsNumList;
            gvPartsNumList.PageIndex = NewPageIndex;
            gvPartsNumList.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion
    
    #region GetSymbol
    protected String GetSymbol(Object objSymbolRtf, Object objPartNo, Object objColorSfx)
    {
        try
        {
            String strText = Itenso.Solutions.Community.Rtf2Html.RtfConverterDemo.ConvertRtf2Txt(Convert.ToString(objSymbolRtf));
            strText = strText.Replace("\r", "");
            strText = strText.Replace("\n", "");

            String strBgColor = csDatabase.GetPartsBgColor(Convert.ToString(objPartNo), Convert.ToString(objColorSfx));
            String strFontColor = csDatabase.GetPartsFontColor(Convert.ToString(objPartNo), Convert.ToString(objColorSfx));

            String strSymbol = "<p style='text-align:center;'><font face='arial' size='6' color='" + strFontColor + "'><span style='background-color:" + strBgColor + ";'>&nbsp;" + strText + "&nbsp;</span></font></p>";
            return strSymbol;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }
    #endregion

    #region GetPartType
    protected String GetPartType(Object objPartType)
    {
        try
        {
            if (Convert.ToString(objPartType) == "1")
            {
                return "Jundate";
            }
            else if (Convert.ToString(objPartType) == "0")
            {
                return "Harigami";
            }
            else
            {
                return "Data Error";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "Data Error";
        }
    }
    #endregion

    #endregion

    #region Events

    #region btnImport
    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to Import Parts Number");
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                Boolean blImpPartNum = csDatabase.ImpPartNumber();

                if (blImpPartNum)
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> successfully Imported Parts Number");
                    GlobalFunc.ShowMessage("Part Number imported successfully.");
                }
                else
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to Import Parts Number");
                    GlobalFunc.ShowMessage("Part Number import FAIL. Please check.");
                }
            }
            else
            {
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> canceled to Import Parts Number");
            }

            NewPageIndex = 0;
            SearchPartsNumList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvPartsNumList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchPartsNumList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
