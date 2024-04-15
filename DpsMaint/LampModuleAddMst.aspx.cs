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

public partial class LampModuleAddMst : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["SessTempModuleAdd"] = "";

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
                getModuleType();
                getPlcModel();                      //Added by YanTeng 15/09/2020
                getBlockforModuleName();            //Added by YanTeng 06/12/2020
                NewPageIndex = 0;
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
        //SearchLampModuleAddMst();
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

    #region getModuleType
    private void getModuleType()
    {
        try
        {
            ddModuleType.DataSource = GlobalFunc.getModuleType("");
            ddModuleType.DataTextField = "Description";
            ddModuleType.DataValueField = "Value";
            ddModuleType.DataBind();
            ddModuleType.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getPlcModel
    private void getPlcModel()              //Added by YanTeng 15/09/2020
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

    #region getBlockforModuleName
    private void getBlockforModuleName()              //Added by YanTeng 06/12/2020
    {
        try
        {
            String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            ddModuleName.DataSource = GlobalFunc.getBlockforModuleName(strPlcNo, strProcName);
            ddModuleName.DataTextField = "Description";
            ddModuleName.DataValueField = "Description";
            ddModuleName.DataBind();
            ddModuleName.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region BindGridView
    private bool BindGridView(DataTable dtLampModuleAddMst)
    {
        try
        {
            DataView dvLampModuleAddMst = new DataView(dtLampModuleAddMst);

            gvLampModuleAddMst.DataSource = dvLampModuleAddMst;
            gvLampModuleAddMst.PageIndex = NewPageIndex;
            gvLampModuleAddMst.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region SearchLampModuleAddMst
    private void SearchLampModuleAddMst()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strPlcModel = Convert.ToString(ddPlcModel.SelectedValue).Trim();         //Added by YanTeng 14/09/2020
            String strModuleAddFrm = Convert.ToString(txtModuleAddFrm.Text).Trim();
            String strModuleAddTo = Convert.ToString(txtModuleAddTo.Text).Trim();
            String strModuleName = Convert.ToString(ddModuleName.SelectedValue).Trim();
            String strModuleType = Convert.ToString(ddModuleType.SelectedValue).Trim();
            String strPlcNo = Convert.ToString(ddProcName.SelectedValue).Trim();
            String strProcName = Convert.ToString(ddProcName.SelectedItem).Trim();

            dsSearch = csDatabase.SrcLampModuleAddMst(strPlcModel, strModuleAddFrm, strModuleAddTo, strModuleName, strModuleType, strPlcNo, strProcName);
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
            //Added by YanTeng 17/09/2020
            getProcName();
            getModuleType();
            getPlcModel();
            getBlockforModuleName();

            txtModuleAddFrm.Text = "";
            txtModuleAddTo.Text = "";
            ddModuleName.SelectedIndex = 0;
            ddModuleType.SelectedIndex = 0;
            ddProcName.SelectedIndex = 0;
            ddPlcModel.SelectedIndex = 0;           //Added by YanTeng 17/09/2020

            SearchLampModuleAddMst();
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
        String strModuleAddFrm = "";
        String strModuleAddTo = "";
        String strModuleName = "";
        String strModuleType = "";
        String strPlcNo = "";
        String strProcName = "";
        String strPlcModel = "";            //Added by YanTeng 15/09/2020

        if (Convert.ToString(txtModuleAddFrm.Text).Trim() != "") strModuleAddFrm = GlobalFunc.getReplaceToUrl(Convert.ToString(txtModuleAddFrm.Text));
        if (Convert.ToString(txtModuleAddTo.Text).Trim() != "") strModuleAddTo = GlobalFunc.getReplaceToUrl(Convert.ToString(txtModuleAddTo.Text));
        if (Convert.ToString(ddModuleName.SelectedValue).Trim() != "") strModuleName = GlobalFunc.getReplaceToUrl(Convert.ToString(ddModuleName.SelectedValue));
        if (Convert.ToString(ddModuleType.SelectedValue).Trim() != "") strModuleType = GlobalFunc.getReplaceToUrl(Convert.ToString(ddModuleType.SelectedValue));
        if (Convert.ToString(ddProcName.SelectedValue).Trim() != "") strPlcNo = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedValue));
        if (Convert.ToString(ddProcName.SelectedItem).Trim() != "") strProcName = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedItem));
        if (Convert.ToString(ddPlcModel.SelectedValue).Trim() != "") strPlcModel = GlobalFunc.getReplaceToUrl(Convert.ToString(ddPlcModel.SelectedValue));       //Added by YanTeng 15/09/2020

        String strRedirect = "LampModuleAddMstReg.aspx?";

        strRedirect = strRedirect + "maf=" + strModuleAddFrm + "&";
        strRedirect = strRedirect + "mat=" + strModuleAddTo + "&";
        strRedirect = strRedirect + "mn=" + strModuleName + "&";
        strRedirect = strRedirect + "mt=" + strModuleType + "&";
        strRedirect = strRedirect + "pno=" + strPlcNo + "&";
        strRedirect = strRedirect + "pnm=" + strProcName + "&";
        strRedirect = strRedirect + "plcm=" + strPlcModel + "&";            //Added by YanTeng 15/09/2020

        return strRedirect;
    }
    #endregion

    #endregion

    #region Events

    #region btnNewLmAdd
    protected void btnNewLmAdd_Click(object sender, EventArgs e)
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
            SearchLampModuleAddMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvLampModuleAddMst_RowCommand
    protected void gvLampModuleAddMst_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.Cells[10].Controls[0];         //Modified by YanTeng 14/09/2020 (Change cells no.8 to no.10)
                lb.OnClientClick = "return confirm('Confirm delete? ATTENTION : THIS WILL DELETE LAMP MODULE ADDRESS ASSIGNED TO RACK MASTER DETAIL IF ITS BEING USED IN RACK MASTER DETAIL!');";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvLampModuleAddMst_RowCommand
    protected void gvLampModuleAddMst_RowCommand(object sender, GridViewCommandEventArgs e)
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
                Session["SessTempModuleProcName"] = Convert.ToString(selectedRow.Cells[1].Text);
                Session["SessTempModuleAdd"] = Convert.ToString(selectedRow.Cells[3].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit LM Address['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Module Name['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Module Type['" + Convert.ToString(selectedRow.Cells[6].Text) + "']");
                Response.Redirect(GetRedirectString());
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String strModuleProcName = Convert.ToString(selectedRow.Cells[1].Text);
                String strModuleAdd = Convert.ToString(selectedRow.Cells[3].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete LM Address['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Module Name['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Module Type['" + Convert.ToString(selectedRow.Cells[6].Text) + "']");
                //Boolean blDelLmAdd = csDatabase.DelLampModuleAddMst(strModuleProcName, strModuleAdd);  //***ace_20160416_001
                Boolean blDelLmAdd = csDatabase.DelLampModuleAddMst(strModuleProcName, strModuleAdd, strCurUser);
                if (!blDelLmAdd)
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to delete LM Address['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Module Name['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Module Type['" + Convert.ToString(selectedRow.Cells[6].Text) + "']");
                    GlobalFunc.ShowErrorMessage("Unable to delete Lamp Module Type: " + strModuleAdd + ".");
                    //Response.Write("<script language='javascript'>alert('Unable to delete Module Address: " + ModuleAdd + ".')</script>");
                }
                else
                {
                    //csDatabase.DelLampModuleAddMstRem(strModuleProcName, strModuleAdd);
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted LM Address['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Module Name['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Module Type['" + Convert.ToString(selectedRow.Cells[6].Text) + "']");
                    GlobalFunc.ShowMessage("Lamp Module Address: " + strModuleAdd + " deleted.");
                    //ClientScriptManager CSM = Page.ClientScript;
                    //string strconfirm = "<script>if(!window.alert('Module Address: " + ModuleAdd + " deleted.'))</script>";
                    //CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
                SearchLampModuleAddMst();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvLampModuleAddMst_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchLampModuleAddMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ddProcName_OnSelectedIndexChanged          
    protected void ddProcName_OnSelectedIndexChanged(object sender, EventArgs e)            //Added by YanTeng 17/09/2020
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

                getBlockforModuleName();
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
    protected void ddPlcModel_OnSelectedIndexChanged(object sender, EventArgs e)            //Added by YanTeng 17/09/2020
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
