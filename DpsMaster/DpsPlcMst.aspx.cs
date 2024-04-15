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

public partial class DpsPlcMst : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["SessTempPlcNo"] = "";

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
                getPlcModel();              //Added by YanTeng 29/09/2020
                //SearchPlcMstList();
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
    }
    #endregion

    #region Method

    #region BindGridView
    private bool BindGridView(DataTable dtPlcMstList)
    {
        try
        {
            DataView dvPlcMstList = new DataView(dtPlcMstList);

            gvPlcMstList.DataSource = dvPlcMstList;
            gvPlcMstList.PageIndex = NewPageIndex;
            gvPlcMstList.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region SearchPlcMstList
    private void SearchPlcMstList()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strPlcNo = Convert.ToString(txtPlcNo.Text);
            String strProcName = Convert.ToString(txtProcName.Text);
            String strIpAdd = Convert.ToString(txtIpAdd.Text);
            String strPlcNwStation = Convert.ToString(txtPlcNwStation.Text);
            String strPlcLogicalStation = Convert.ToString(txtPlcLogicalStationNo.Text);
            String strPlcModel = Convert.ToString(ddPlcModel.SelectedItem).Trim();            //Added by YanTeng 10/09/2020

            dsSearch = csDatabase.SrcPlcMstList(strPlcNo, strProcName, strIpAdd, strPlcModel, strPlcNwStation, strPlcLogicalStation);
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
            txtPlcNo.Text = "";
            txtProcName.Text = "";
            txtIpAdd.Text = "";
            txtPlcNwStation.Text = "";
            ddPlcModel.SelectedIndex = 0;          //Added by YanTeng 10/09/2020
            cbEnable.Checked = false;
            SearchPlcMstList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Get Redirect Pass-Out
    private String GetRedirectString()
    {

        String strPlcNo = "";
        String strProcName = "";
        String strIpAdd = "";
        String strPlcNwStation = "";
        String strPlcLogicalStation = "";
        String strPlcModel = "";

        if (Convert.ToString(txtPlcNo.Text).Trim() != "") strPlcNo = GlobalFunc.getReplaceToUrl(Convert.ToString(txtPlcNo.Text));
        if (Convert.ToString(txtProcName.Text).Trim() != "") strProcName = GlobalFunc.getReplaceToUrl(Convert.ToString(txtProcName.Text));
        if (Convert.ToString(txtIpAdd.Text).Trim() != "") strIpAdd = GlobalFunc.getReplaceToUrl(Convert.ToString(txtIpAdd.Text));
        if (Convert.ToString(txtPlcNwStation.Text).Trim() != "") strPlcNwStation = GlobalFunc.getReplaceToUrl(Convert.ToString(txtPlcNwStation.Text));
        if (Convert.ToString(txtPlcLogicalStationNo.Text).Trim() != "") strPlcLogicalStation = GlobalFunc.getReplaceToUrl(Convert.ToString(txtPlcLogicalStationNo.Text));
        if (Convert.ToString(ddPlcModel.SelectedItem).Trim() != "") strPlcModel = GlobalFunc.getReplaceToUrl(Convert.ToString(ddPlcModel.SelectedItem));          //Added by YanTeng 10/09/2020

        String strRedirect = "DpsPlcMstReg.aspx?";

        strRedirect = strRedirect + "plcno=" + strPlcNo + "&";
        strRedirect = strRedirect + "procname=" + strProcName + "&";
        strRedirect = strRedirect + "ipadd=" + strIpAdd + "&";
        strRedirect = strRedirect + "nwstation=" + strPlcNwStation + "&";
        strRedirect = strRedirect + "lsn=" + strPlcLogicalStation + "&";
        strRedirect = strRedirect + "plcmodel=" + strPlcModel + "&";            //Added by YanTeng 10/09/2020

        return strRedirect;
    }
    #endregion

    #region getPlcModel
    private void getPlcModel()              //Added by YanTeng 29/09/2020
    {
        try
        {
            ddPlcModel.DataSource = GlobalFunc.getAllPlcModel();
            ddPlcModel.DataTextField = "Code";
            ddPlcModel.DataValueField = "Value";
            ddPlcModel.DataBind();
            ddPlcModel.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion

    #region Events

    #region btnNewPlc
    protected void btnNewPlc_Click(object sender, EventArgs e)
    {
        try
        {
            if (!csDatabase.ChkPlcMstMaxCnt())
            {
                Response.Redirect(GetRedirectString());
            }
            else
            {
                GlobalFunc.ShowMessage("Maximum PLC Count of 12 reached. Please edit/delete unused PLC.");
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

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
            SearchPlcMstList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvPlcMstList_RowDataBound
    protected void gvPlcMstList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.Cells[10].Controls[0];
                lb.OnClientClick = "return confirm('Confirm delete? ATTENTION : THIS WILL DELETE ALL GROUP MASTER, BLOCK MASTER, RACK MASTER DATA REGISTERED UNDER CURRENT PLC!');";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvPlcMstList_RowCommand
    protected void gvPlcMstList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 index = Convert.ToInt32(e.CommandArgument);            

            if (e.CommandName == "EditRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                Session["SessTempPlcNo"] = Convert.ToString(selectedRow.Cells[0].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] IP Address['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] PLC Logical Station No['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] N/W Station No['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Enabled['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                Response.Redirect(GetRedirectString(), false);
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String strPlcNo = Convert.ToString(selectedRow.Cells[0].Text);
                String strProcName = Convert.ToString(selectedRow.Cells[1].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] IP Address['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] PLC Logical Station No['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] N/W Station No['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Enabled['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                Boolean blDelPlc = csDatabase.DelPlcMst(strPlcNo, strProcName);
                if (blDelPlc)
                {
                    //csDatabase.DelPlcAllRem(strPlcNo, strProcName);
                    GlobalFunc.ShowMessage("PLC No: " + strPlcNo + " deleted.");
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] IP Address['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] PLC Logical Station No['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] N/W Station No['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Enabled['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                }
                else
                {
                    GlobalFunc.ShowMessage("Unable to delete Plc No: " + strPlcNo + ".");
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to deleted PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] IP Address['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] PLC Logical Station No['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] N/W Station No['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Enabled['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                }
                SearchPlcMstList();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvPlcMstList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchPlcMstList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
