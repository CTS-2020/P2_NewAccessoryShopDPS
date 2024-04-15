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

public partial class LampModuleTypeMst : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["SessTempModuleType"] = "";

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
                getEquipType();
                getLampLighting();
                getLampColor();
                NewPageIndex = 0;
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
        //SearchLampModuleTypeMst();
    }
    #endregion

    #region Method

    #region getEquipType
    private void getEquipType()
    {
        try
        {
            ddEquipType.DataSource = GlobalFunc.getEquipType();
            ddEquipType.DataTextField = "Description";
            ddEquipType.DataValueField = "Description";
            ddEquipType.DataBind();
            ddEquipType.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getLampLighting
    private void getLampLighting()
    {
        try
        {
            #region During Instruction
            ddLampLightingDI.DataSource = GlobalFunc.getLampLighting();
            ddLampLightingDI.DataTextField = "Description";
            ddLampLightingDI.DataValueField = "Description";
            ddLampLightingDI.DataBind();
            ddLampLightingDI.Items.Insert(0, " ");
            #endregion

            #region After Instruction
            ddLampLightingAI.DataSource = GlobalFunc.getLampLighting();
            ddLampLightingAI.DataTextField = "Description";
            ddLampLightingAI.DataValueField = "Description";
            ddLampLightingAI.DataBind();
            ddLampLightingAI.Items.Insert(0, " ");
            #endregion
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getLampColor
    private void getLampColor()
    {
        try
        {
            #region During Instruction
            ddLampColorDI.DataSource = GlobalFunc.getLampColor();
            ddLampColorDI.DataTextField = "Description";
            ddLampColorDI.DataValueField = "Description";
            ddLampColorDI.DataBind();
            ddLampColorDI.Items.Insert(0, " ");
            #endregion

            #region After Instruction
            ddLampColorAI.DataSource = GlobalFunc.getLampColor();
            ddLampColorAI.DataTextField = "Description";
            ddLampColorAI.DataValueField = "Description";
            ddLampColorAI.DataBind();
            ddLampColorAI.Items.Insert(0, " ");
            #endregion
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region SearchLampModuleTypeMst
    private void SearchLampModuleTypeMst()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strModuleType = Convert.ToString(txtModuleType.Text).Trim();
            String strEquipType = Convert.ToString(ddEquipType.SelectedValue).Trim();
            String strLightDI = Convert.ToString(ddLampLightingDI.SelectedValue).Trim();
            String strColorDI = Convert.ToString(ddLampColorDI.SelectedValue).Trim();
            String strLightAI = Convert.ToString(ddLampLightingAI.SelectedValue).Trim();
            String strColorAI = Convert.ToString(ddLampColorAI.SelectedValue).Trim();

            dsSearch = csDatabase.SrcLampModuleTypeMst(strModuleType, strEquipType, strLightDI, strColorDI, strLightAI, strColorAI);
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
    private bool BindGridView(DataTable dtLampModuleTypeMst)
    {
        try
        {
            DataView dvLampModuleTypeMst = new DataView(dtLampModuleTypeMst);

            gvLampModuleTypeMst.DataSource = dvLampModuleTypeMst;
            gvLampModuleTypeMst.PageIndex = NewPageIndex;
            gvLampModuleTypeMst.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region ClearAll
    private void ClearAll()
    {
        try
        {
            txtModuleType.Text = "";
            ddEquipType.SelectedIndex = 0;
            ddLampLightingDI.SelectedIndex = 0;
            ddLampColorDI.SelectedIndex = 0;
            ddLampLightingAI.SelectedIndex = 0;
            ddLampColorAI.SelectedIndex = 0;
            SearchLampModuleTypeMst();
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
        String strModuleType = "";
        String strEquipType = "";
        String strLightDI = "";
        String strColorDI = "";
        String strLightAI = "";
        String strColorAI = "";

        if (Convert.ToString(txtModuleType.Text).Trim() != "") strModuleType = GlobalFunc.getReplaceToUrl(Convert.ToString(txtModuleType.Text));
        if (Convert.ToString(ddEquipType.SelectedValue).Trim() != "") strEquipType = GlobalFunc.getReplaceToUrl(Convert.ToString(ddEquipType.SelectedValue));
        if (Convert.ToString(ddLampLightingDI.SelectedValue).Trim() != "") strLightDI = GlobalFunc.getReplaceToUrl(Convert.ToString(ddLampLightingDI.SelectedValue));
        if (Convert.ToString(ddLampColorDI.SelectedValue).Trim() != "") strColorDI = GlobalFunc.getReplaceToUrl(Convert.ToString(ddLampColorDI.SelectedValue));
        if (Convert.ToString(ddLampLightingAI.SelectedValue).Trim() != "") strLightAI = GlobalFunc.getReplaceToUrl(Convert.ToString(ddLampLightingAI.SelectedValue));
        if (Convert.ToString(ddLampColorAI.SelectedValue).Trim() != "") strColorAI = GlobalFunc.getReplaceToUrl(Convert.ToString(ddLampColorAI.SelectedValue));

        String strRedirect = "LampModuleTypeMstReg.aspx?";

        strRedirect = strRedirect + "mt=" + strModuleType + "&";
        strRedirect = strRedirect + "et=" + strEquipType + "&";
        strRedirect = strRedirect + "ldi=" + strLightDI + "&";
        strRedirect = strRedirect + "cdi=" + strColorDI + "&";
        strRedirect = strRedirect + "lai=" + strLightAI + "&";
        strRedirect = strRedirect + "cai=" + strColorAI + "&";

        return strRedirect;
    }
    #endregion

    #endregion

    #region Events

    #region btnNewLampType
    protected void btnNewLampType_Click(object sender, EventArgs e)
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
            SearchLampModuleTypeMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvLampModuleTypeMst_RowDataBound
    protected void gvLampModuleTypeMst_RowDataBound(object sender, GridViewRowEventArgs e)
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

    #region gvLampModuleTypeMst_RowCommand
    protected void gvLampModuleTypeMst_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (Convert.ToString(e.CommandArgument) == "First") return;
            if (Convert.ToString(e.CommandArgument) == "Last") return;
            Int32 index = Convert.ToInt32(e.CommandArgument);            

            if (e.CommandName == "EditRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                Session["SessTempModuleType"] = Convert.ToString(selectedRow.Cells[0].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit Lamp Module Type '" + Convert.ToString(selectedRow.Cells[0].Text) + "'");
                Response.Redirect(GetRedirectString());
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String strModuleType = Convert.ToString(selectedRow.Cells[0].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete Lamp Module Type '" + strModuleType + "'");
                Boolean blChkLmTypeUsed = csDatabase.ChkLampModuleTypeUsed(strModuleType);
                if (!blChkLmTypeUsed)
                {
                    Boolean blDelLmType = csDatabase.DelLampModuleTypeMst(strModuleType);
                    if (!blDelLmType)
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to delete Lamp Module Type '" + strModuleType + "'");
                        Response.Write("<script language='javascript'>alert('Unable to delete Lamp Module Type: " + strModuleType + ".');</script>");
                    }
                    else
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted Lamp Module Type '" + strModuleType + "'");
                        Response.Write("<script language='javascript'>alert('Lamp Module Type: " + strModuleType + " deleted.');</script>");
                    }
                }
                else
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to delete Lamp Module Type '" + strModuleType + "' due to being used in some Lamp Module Address");
                    Response.Write("<script language='javascript'>alert('Unable to delete. Lamp Module Type : " + strModuleType + " is being used in some Lamp Module Address.');</script>");
                }
                SearchLampModuleTypeMst();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvLampModuleTypeMst_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchLampModuleTypeMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
