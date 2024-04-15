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
        //Session["SessTempModelID"] = "";
        Session["SessTempModelNo"] = "";

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
    private bool BindGridView(DataTable dtPlcModelList)
    {
        try
        {
            DataView dvPlcModelList = new DataView(dtPlcModelList);

            gvPlcModelList.DataSource = dvPlcModelList;
            gvPlcModelList.PageIndex = NewPageIndex;
            gvPlcModelList.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region SearchPlcModelList
    private void SearchPlcModelList()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strPlcModelNo = Convert.ToString(txtPlcModelNo.Text);
            String strPlcModelDesc = Convert.ToString(txtPlcModelDesc.Text);
            String strPhyAddrFrom = Convert.ToString(txtPhyAddrFrom.Text);
            String strPhyAddrTo = Convert.ToString(txtPhyAddrTo.Text);
            //String strDigitNo = Convert.ToString(ddDigitNo.SelectedValue);
            //String strPlcModel = Convert.ToString(txtPlcModel.Text); 

            dsSearch = csDatabase.SrcPlcModelList(strPlcModelNo, strPlcModelDesc, strPhyAddrFrom, strPhyAddrTo);
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
            txtPlcModelNo.Text = "";
            txtPlcModelDesc.Text = "";
            txtPhyAddrFrom.Text = "";
            txtPhyAddrTo.Text = "";
            ddConvType.SelectedIndex = 0;
            //ddDigitNo.SelectedIndex = 0;
            cbEnable.Checked = false;
            SearchPlcModelList();
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

        String strPlcModelNo = "";
        String strPlcModelDesc = "";
        String strPhyAddrFrom = "";
        String strPhyAddrTo = "";
        //String strDigitNo = "";

        if (Convert.ToString(txtPlcModelNo.Text).Trim() != "") strPlcModelNo = GlobalFunc.getReplaceToUrl(Convert.ToString(txtPlcModelNo.Text));
        if (Convert.ToString(txtPlcModelDesc.Text).Trim() != "") strPlcModelDesc = GlobalFunc.getReplaceToUrl(Convert.ToString(txtPlcModelDesc.Text));
        if (Convert.ToString(txtPhyAddrFrom.Text).Trim() != "") strPhyAddrFrom = GlobalFunc.getReplaceToUrl(Convert.ToString(txtPhyAddrFrom.Text));
        if (Convert.ToString(txtPhyAddrTo.Text).Trim() != "") strPhyAddrTo = GlobalFunc.getReplaceToUrl(Convert.ToString(txtPhyAddrTo.Text));
        //if (Convert.ToString(ddDigitNo.SelectedValue).Trim() != "") strDigitNo = GlobalFunc.getReplaceToUrl(Convert.ToString(ddDigitNo.SelectedValue));
        
        String strRedirect = "DpsPlcModelReg.aspx?";

        strRedirect = strRedirect + "modelno=" + strPlcModelNo + "&";
        strRedirect = strRedirect + "desc=" + strPlcModelDesc + "&";
        strRedirect = strRedirect + "addfrm=" + strPhyAddrFrom + "&";
        strRedirect = strRedirect + "addto=" + strPhyAddrTo + "&";
        //strRedirect = strRedirect + "digit=" + strDigitNo + "&";

        return strRedirect;
    }
    #endregion

    #endregion

    #region Events

    #region btnNewModel
    protected void btnNewModel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(GetRedirectString());
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
            SearchPlcModelList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvPlcModelList_RowDataBound
    protected void gvPlcModelList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.Cells[10].Controls[0];
                lb.OnClientClick = "return confirm('Confirm delete?');";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvPlcModelList_RowCommand
    protected void gvPlcModelList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 index = Convert.ToInt32(e.CommandArgument);            

            if (e.CommandName == "EditRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                //Session["SessTempModelID"] = Convert.ToString(selectedRow.Cells[0].Text);
                Session["SessTempModelNo"] = Convert.ToString(selectedRow.Cells[1].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit GW Model['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] GW Model Description['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Physical Address From['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Physical Address To['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Enabled['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                Response.Redirect(GetRedirectString(), false);
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String strPlcModelId = Convert.ToString(selectedRow.Cells[0].Text);
                String strPlcModelNo = Convert.ToString(selectedRow.Cells[1].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete GW Model['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] GW Model Description['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Physical Address From['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Physical Address To['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Enabled['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                Boolean blDelPlcModel = csDatabase.DelPlcModelMst(strPlcModelId, strPlcModelNo);
                if (blDelPlcModel)
                {
                    //csDatabase.DelPlcAllRem(strPlcNo, strProcName);
                    GlobalFunc.ShowMessage("GW Model No: " + strPlcModelNo + " deleted.");
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted GW Model['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] GW Model Description['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Physical Address From['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Physical Address To['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Enabled['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                }
                else
                {
                    GlobalFunc.ShowMessage("Unable to delete GW Model No: " + strPlcModelNo + ".");
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to deleted GW Model['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] GW Model Description['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Physical Address From['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Physical Address To['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Enabled['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                }
                SearchPlcModelList();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvPlcModelList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchPlcModelList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
