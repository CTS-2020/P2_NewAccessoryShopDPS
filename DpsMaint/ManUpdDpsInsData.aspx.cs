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

public partial class ManUpdDpsInsData : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["SessDpsRsConvId"] = "";

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
                getProcName();
                NewPageIndex = 0;
                //SearchDpsRsConv();
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
    }
    #endregion

    #region Method

    #region getProcName
    private void getProcName()
    {
        try
        {
            ddProcName.DataSource = GlobalFunc.getProcName();

            ddProcName.DataTextField = "Description";
            ddProcName.DataValueField = "Code";
            ddProcName.DataBind();
            ddProcName.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region BindGridView
    private bool BindGridView(DataTable dtDpsRsConv)
    {
        try
        {
            DataView dvDpsRsConv = new DataView(dtDpsRsConv);

            gvDpsRsConv.DataSource = dvDpsRsConv;
            gvDpsRsConv.PageIndex = NewPageIndex;
            gvDpsRsConv.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region Search DPS Result Conversion
    private void SearchDpsRsConv()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strInsCode = Convert.ToString(txtInsCode.Text);
            String strPointer = Convert.ToString(txtPointer.Text);
            String strIdNo = Convert.ToString(txtIdn.Text);
            String strIdVer = Convert.ToString(txtIdVer.Text);
            String strChassisNo = Convert.ToString(txtChasNo.Text);
            String strBseq = Convert.ToString(txtBseq.Text);
            String strModel = Convert.ToString(txtModel.Text);
            String strSfx = Convert.ToString(txtSfx.Text);
            String strColor = Convert.ToString(txtColor.Text);
            String strPlcNo = Convert.ToString(ddProcName.SelectedValue).Trim();

            dsSearch = csDatabase.searchDpsRsConv("", strInsCode, strPointer, strIdNo, strIdVer, strChassisNo, strBseq, strModel, strSfx, strColor, strPlcNo);
            dtSearch = dsSearch.Tables[0];
            BindGridView(dtSearch);
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ClearAll
    private void ClearAll()
    {
        try
        {
            ddProcName.SelectedIndex = 0;
            txtInsCode.Text = "";
            txtPointer.Text = "";
            txtIdn.Text = "";
            txtChasNo.Text = "";
            txtBseq.Text = "";
            txtModel.Text = "";
            txtSfx.Text = "";
            txtColor.Text = "";
            SearchDpsRsConv();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion

    #region Events

    #region BtnClear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region BtnSearch
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            NewPageIndex = 0;
            SearchDpsRsConv();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvDpsRsConv_RowDataBound
    protected void gvDpsRsConv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.Cells[13].Controls[0];
                lb.OnClientClick = "return confirm('Confirm delete?');";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvDpsRsConv_RowCommand
    protected void gvDpsRsConv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 index = Convert.ToInt32(e.CommandArgument);            

            if (e.CommandName == "EditRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                Session["SessDpsRsConvId"] = Convert.ToString(selectedRow.Cells[0].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit ID No '" + Convert.ToString(selectedRow.Cells[4].Text) + "'");
                Response.Redirect("ManUpdDpsInsDataReg.aspx");
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String strDpsRsConvId = Convert.ToString(selectedRow.Cells[0].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete ID No '" + Convert.ToString(selectedRow.Cells[4].Text) + "'");
                csDatabase.deleteDpsRsConv(strDpsRsConvId);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted ID No '" + Convert.ToString(selectedRow.Cells[4].Text) + "'");
                SearchDpsRsConv();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvDpsRsConv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchDpsRsConv();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
