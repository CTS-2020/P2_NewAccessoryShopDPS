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

public partial class PhysicalAddMst : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["SessTempPhysicalAdd"] = "";

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
                getPlcModel();
                NewPageIndex = 0;
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

    #region getPlcModel
    private void getPlcModel()
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

    #region BindGridView
    private bool BindGridView(DataTable dtPhysicalAddMst)
    {
        try
        {
            DataView dvPhysicalAddMst = new DataView(dtPhysicalAddMst);

            gvPhysicalAddMst.DataSource = dvPhysicalAddMst;
            gvPhysicalAddMst.PageIndex = NewPageIndex;
            gvPhysicalAddMst.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region SearchPhysicalAddMst
    private void SearchPhysicalAddMst()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strPlcNo = Convert.ToString(ddProcName.SelectedValue).Trim();
            String strProcName = Convert.ToString(ddProcName.SelectedItem).Trim();
            String strModuleAddFrm = Convert.ToString(txtModuleAddFrm.Text).Trim();
            String strModuleAddTo = Convert.ToString(txtModuleAddTo.Text).Trim();
            String strPhyAddFrm = Convert.ToString(txtPhysicalAddFrm.Text).Trim();
            String strPhyAddTo = Convert.ToString(txtPhysicalAddTo.Text).Trim();

            dsSearch = csDatabase.SrcPhysicalAddMst(strPlcNo, strProcName, strModuleAddFrm, strModuleAddTo, strPhyAddFrm, strPhyAddTo);
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
            getProcName();
            getPlcModel();

            ddProcName.SelectedIndex = 0;
            ddPlcModel.SelectedIndex = 0;
            txtModuleAddFrm.Text = "";
            txtModuleAddTo.Text = "";
            txtPhysicalAddFrm.Text = "";
            txtPhysicalAddTo.Text = "";

            SearchPhysicalAddMst();
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
        String strModuleAddFrm = "";
        String strModuleAddTo = "";
        String strPhysicalAddFrm = "";
        //String strPhysicalAddTo = "";

        if (Convert.ToString(ddProcName.SelectedValue).Trim() != "") strPlcNo = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedValue));
        if (Convert.ToString(ddProcName.SelectedItem).Trim() != "") strProcName = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedItem));
        if (Convert.ToString(txtModuleAddFrm.Text).Trim() != "") strModuleAddFrm = GlobalFunc.getReplaceToUrl(Convert.ToString(txtModuleAddFrm.Text));
        if (Convert.ToString(txtModuleAddTo.Text).Trim() != "") strModuleAddTo = GlobalFunc.getReplaceToUrl(Convert.ToString(txtModuleAddTo.Text));
        if (Convert.ToString(txtPhysicalAddFrm.Text).Trim() != "") strPhysicalAddFrm = GlobalFunc.getReplaceToUrl(Convert.ToString(txtPhysicalAddFrm.Text));
        //if (Convert.ToString(txtPhysicalAddTo.Text).Trim() != "") strPhysicalAddTo = GlobalFunc.getReplaceToUrl(Convert.ToString(txtPhysicalAddTo.Text));

        String strRedirect = "PhysicalAddMstReg.aspx?";

        strRedirect = strRedirect + "pno=" + strPlcNo + "&";
        strRedirect = strRedirect + "pnm=" + strProcName + "&";
        strRedirect = strRedirect + "maf=" + strModuleAddFrm + "&";
        strRedirect = strRedirect + "mat=" + strModuleAddTo + "&";
        strRedirect = strRedirect + "phyadd=" + strPhysicalAddFrm + "&";
        //strRedirect = strRedirect + "pat=" + strPhysicalAddTo + "&";

        return strRedirect;
    }
    #endregion

    #region Check Data Valid

    private Boolean ChkAddressValue()
    {
        try
        {
            if (txtModuleAddFrm.Text != "" && !GlobalFunc.IsTextAValidInteger(Convert.ToString(txtModuleAddFrm.Text)))
            {
                lblMsg.Text = "Module Address From must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            else if (txtModuleAddTo.Text != "" && !GlobalFunc.IsTextAValidInteger(Convert.ToString(txtModuleAddTo.Text)))
            {
                lblMsg.Text = "Module Address To must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            else if (txtPhysicalAddFrm.Text != "" && !GlobalFunc.IsTextAValidInteger(Convert.ToString(txtPhysicalAddFrm.Text)))
            {
                lblMsg.Text = "Physical Address From must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            else if (txtPhysicalAddTo.Text != "" && !GlobalFunc.IsTextAValidInteger(Convert.ToString(txtPhysicalAddTo.Text)))
            {
                lblMsg.Text = "Physical Address To must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #endregion

    #region Events

    #region btnNewPhysical
    protected void btnNewPhysical_Click(object sender, EventArgs e)
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
            Boolean boolValid = ChkAddressValue();
            if (boolValid)
            {
                NewPageIndex = 0;
                SearchPhysicalAddMst();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvPhysicalAddMst_RowCommand
    protected void gvPhysicalAddMst_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.Cells[9].Controls[0]; 
                lb.OnClientClick = "return confirm('Confirm delete?');";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvPhysicalAddMst_RowCommand
    protected void gvPhysicalAddMst_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (Convert.ToString(e.CommandArgument) == "First") return;
            if (Convert.ToString(e.CommandArgument) == "Last") return;
            Int32 index = Convert.ToInt32(e.CommandArgument);
            String strCurUser = Convert.ToString(Session["SessUserId"]);  //***ace_20160416_001

            if (e.CommandName == "EditRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                Session["SessTempPhysicalUid"] = Convert.ToString(selectedRow.Cells[0].Text);
                Session["SessTempPlcNo"] = Convert.ToString(selectedRow.Cells[1].Text);
                Session["SessTempProcName"] = Convert.ToString(selectedRow.Cells[2].Text);
                Session["SessTempModuleAdd"] = Convert.ToString(selectedRow.Cells[4].Text);
                Session["SessTempPhysicalAdd"] = Convert.ToString(selectedRow.Cells[5].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit PLC No['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] GW Model['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Module Address['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Physical Address['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                Response.Redirect(GetRedirectString());
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String strPhysicalUid = Convert.ToString(selectedRow.Cells[0].Text);
                String strProcName = Convert.ToString(selectedRow.Cells[2].Text);
                String strModuleAdd = Convert.ToString(selectedRow.Cells[4].Text);
                String strPhysicalAdd = Convert.ToString(selectedRow.Cells[5].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete PLC No['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] GW Model['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Module Address['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Physical Address['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                Boolean blDelLmAdd = csDatabase.DelPhysicalAddMst(strPhysicalUid);
                if (!blDelLmAdd)
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to delete PLC No['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] GW Model['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Module Address['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Physical Address['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                    GlobalFunc.ShowErrorMessage("Unable to delete PLC No['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] GW Model['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Module Address['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Physical Address['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                }
                else
                {
                    //string tmpMsg = "PLC No['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] GW Model['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Module Address['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Physical Address['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] deleted.";
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted PLC No['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] GW Model['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Module Address['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Physical Address['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                    //GlobalFunc.ShowMessage(tmpMsg);
                    //GlobalFunc.ShowMessage("Physical Address deleted.");
                    GlobalFunc.ShowMessage("PLC No['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] GW Model['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Module Address['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Physical Address['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] deleted.");
                }
                SearchPhysicalAddMst();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvPhysicalAddMst_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchPhysicalAddMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ddProcName_OnSelectedIndexChanged          
    protected void ddProcName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(ddProcName.SelectedItem.Text.Trim()))
            {
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                ddPlcModel.DataSource = GlobalFunc.getPlcModelbyProcName(strProcName);
                ddPlcModel.DataTextField = "Description";
                ddPlcModel.DataValueField = "Value";
                ddPlcModel.DataBind();
            }
            else
            {
                getPlcModel();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ddPlcModel_OnSelectedIndexChanged          
    protected void ddPlcModel_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(ddPlcModel.SelectedItem.Text.Trim()))
            {
                String strPlcModel = Convert.ToString(ddPlcModel.SelectedItem);
                ddProcName.DataSource = GlobalFunc.getProcNamebyPlcModel(strPlcModel);
                ddProcName.DataTextField = "Description";
                ddProcName.DataValueField = "Code";
                ddProcName.DataBind();
                ddProcName.Items.Insert(0, " ");
            }
            else
            {
                getProcName();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
