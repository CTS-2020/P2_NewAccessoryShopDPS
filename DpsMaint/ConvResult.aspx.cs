using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using dpant;

public partial class ConvResult : System.Web.UI.Page
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

        if (Convert.ToString(Request.QueryString["pn"]) != "") lblTmpPlcNo.Text = Convert.ToString(Request.QueryString["pn"]);

        if (Convert.ToString(lblTmpPlcNo.Text) == "")
        {
            Response.Write("<script language='javascript'>alert('PLC No not found. Please try again.');window.close();</script>");
        }

        if (!IsPostBack)
        {
            try
            {
                NewPageIndex = 0;
                getProcName();
                SearchConvResult();
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }

        if (IsPostBack)
        {
            SetPaging();
        }
    }
    #endregion

    #region Method

    #region getProcName
    private void getProcName()
    {
        try
        {
            String strProcName = csDatabase.GetProcName(Convert.ToString(lblTmpPlcNo.Text));
            txtProcName.Text = strProcName;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region SearchConvResult
    private void SearchConvResult()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strPlcNo = Convert.ToString(lblTmpPlcNo.Text).Trim();

            dsSearch = csDatabase.SrcConvResult(strPlcNo);
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
    private bool BindGridView(DataTable dtConvResult)
    {
        try
        {
            DataView dvConvResult = new DataView(dtConvResult);

            gvConvResult.DataSource = dvConvResult;
            gvConvResult.PageIndex = NewPageIndex;
            gvConvResult.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region SetPaging
    private void SetPaging()
    {
        GridViewRow row = gvConvResult.BottomPagerRow;
        int alphaStart = 1;
        
        PlaceHolder phPager = row.FindControl("phPager") as PlaceHolder;
        phPager.Controls.Clear();

        for (int i = 1; i <= gvConvResult.PageCount; i++)
        {
            LinkButton btn = new LinkButton();
            btn.CommandName = "Page";
            btn.CommandArgument = i.ToString();

            if (i == gvConvResult.PageIndex + 1)
            {
                btn.BackColor = System.Drawing.Color.BlanchedAlmond;
            }

            btn.Text = Convert.ToString(alphaStart) + "-" + Convert.ToString(alphaStart + 99);
            btn.ToolTip = "Page " + i.ToString();
            alphaStart = alphaStart + 100;
            phPager.Controls.Add(btn);

            Label lbl = new Label();
            lbl.Text = " ";
            phPager.Controls.Add(lbl);
        }
    }
    #endregion

    #region GetStatus
    protected String GetStatus(Object objReadFlag)
    {
        try
        {
            if (Convert.ToString(objReadFlag) == "1")
            {
                return "Already";
            }
            else if (Convert.ToString(objReadFlag) == "0")
            {
                return "Not Yet";
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

    #region btnRefresh
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("ConvResult.aspx?pn=" + Convert.ToString(lblTmpPlcNo.Text));
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region btnClose
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Close();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region DataBound
    protected void gvConvResult_DataBound(object sender, EventArgs e)
    {
        SetPaging();
    }
    #endregion

    #region RowDataBound
    protected void gvConvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "read_flag")) == 1)
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#A4A4A4");
            }
            else
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F7FE2E");
            }
        }
    }
    #endregion

    #region Paging Index
    protected void gvConvResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchConvResult();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
