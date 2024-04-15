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
        lblMsg.Visible = false;

        String tempModuleType = Convert.ToString(Session["SessTempModuleType"]);

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

                if (tempModuleType != "")
                {
                    btnUpdate.Visible = true;
                    btnNewLmType.Visible = false;

                    DataSet dsLampModuleTypeMst = new DataSet();
                    DataTable dtLampModuleTypeMst = new DataTable();
                    dsLampModuleTypeMst = csDatabase.SrcLampModuleTypeMst(Convert.ToString(tempModuleType), "", "", "", "", "");
                    dtLampModuleTypeMst = dsLampModuleTypeMst.Tables[0];
                    BindtoText(dtLampModuleTypeMst);
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnNewLmType.Visible = true;
                }

                NewPageIndex = 0;
                showRefGrid();
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
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

    #region BindtoText
    public void BindtoText(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["module_type"]).Trim() != "")
                {
                    lblTmpModuleType.Text = Convert.ToString(dt.Rows[0]["module_type"]).Trim();
                    txtModuleType.Text = Convert.ToString(dt.Rows[0]["module_type"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["equip_type"]).Trim() != "")
                {
                    ddEquipType.SelectedValue = Convert.ToString(dt.Rows[0]["equip_type"]);
                }
                if (Convert.ToString(dt.Rows[0]["light_di"]).Trim() != "")
                {
                    ddLampLightingDI.SelectedValue = Convert.ToString(dt.Rows[0]["light_di"]);
                }
                if (Convert.ToString(dt.Rows[0]["color_di"]).Trim() != "")
                {
                    ddLampColorDI.SelectedValue = Convert.ToString(dt.Rows[0]["color_di"]);
                }
                if (Convert.ToString(dt.Rows[0]["light_ai"]).Trim() != "")
                {
                    ddLampLightingAI.SelectedValue = Convert.ToString(dt.Rows[0]["light_ai"]);
                }
                if (Convert.ToString(dt.Rows[0]["color_ai"]).Trim() != "")
                {
                    ddLampColorAI.SelectedValue = Convert.ToString(dt.Rows[0]["color_ai"]);
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region showRefGrid
    private void showRefGrid()
    {
        try
        {
            String strModuleType = "";
            String strEquipType = "";
            String strLightDI = "";
            String strColorDI = "";
            String strLightAI = "";
            String strColorAI = "";

            if (Convert.ToString(Request.QueryString["mt"]) != "") strModuleType = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["mt"]));
            if (Convert.ToString(Request.QueryString["et"]) != "") strEquipType = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["et"]));
            if (Convert.ToString(Request.QueryString["ldi"]) != "") strLightDI = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["ldi"]));
            if (Convert.ToString(Request.QueryString["cdi"]) != "") strColorDI = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["cdi"]));
            if (Convert.ToString(Request.QueryString["lai"]) != "") strLightAI = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["lai"]));
            if (Convert.ToString(Request.QueryString["cai"]) != "") strColorAI = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["cai"]));

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = csDatabase.SrcLampModuleTypeMst(strModuleType, strEquipType, strLightDI, strColorDI, strLightAI, strColorAI);
            dtSearch = dsSearch.Tables[0];

            DataView dvLampModuleTypeMst = new DataView(dtSearch);

            gvLampModuleTypeMst.DataSource = dvLampModuleTypeMst;
            gvLampModuleTypeMst.PageIndex = NewPageIndex;
            gvLampModuleTypeMst.DataBind();
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
            txtModuleType.Text = "";
            ddEquipType.SelectedIndex = 0;
            ddLampLightingDI.SelectedIndex = 0;
            ddLampColorDI.SelectedIndex = 0;
            ddLampLightingAI.SelectedIndex = 0;
            ddLampColorAI.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Check Data valid
    private Boolean ChkDataValid()
    {
        try
        {
            if (ddEquipType.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Equipment Type.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtModuleType.Text) == "")
            {
                lblMsg.Text = "Please enter Module Type.";
                lblMsg.Visible = true;
                return false;
            }
            else if (txtModuleType.Text.Length > 20)
            {
                lblMsg.Text = "Module Type text length cannot be more than 20 characters.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicateLampModuleType(Convert.ToString(txtModuleType.Text), Convert.ToString(lblTmpModuleType.Text)))
            {
                lblMsg.Text = "No duplicate Lamp Module Type allowed. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddLampLightingDI.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Lamp Lighting During Instruction.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddLampLightingAI.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Lamp Lighting After Instruction.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddLampColorDI.SelectedIndex == 0 && ddLampLightingDI.SelectedValue != "No Lighting")
            {
                lblMsg.Text = "Please select Lamp Color During Instruction.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddLampColorAI.SelectedIndex == 0 && ddLampLightingAI.SelectedValue != "No Lighting")
            {
                lblMsg.Text = "Please select Lamp Color After Instruction.";
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

    #region btnNewLmType
    protected void btnNewLmType_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid();

            if (boolValid)
            {
                String strModuleType = Convert.ToString(txtModuleType.Text);
                String strEquipType = Convert.ToString(ddEquipType.SelectedValue);
                String strLightingDI = Convert.ToString(ddLampLightingDI.SelectedValue);
                String strLightingAI = Convert.ToString(ddLampLightingAI.SelectedValue);
                String strColorDI = Convert.ToString(ddLampColorDI.SelectedValue);
                String strColorAI = Convert.ToString(ddLampColorAI.SelectedValue);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.SvLampModuleTypeMst(strModuleType, strEquipType, strLightingDI, strLightingAI, strColorDI, strColorAI, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to save Lamp Module Type: " + strModuleType + " .");
                    }
                    else
                    {
                        GlobalFunc.Log("<" + strCurUser + "> created Lamp Module Type '" + strModuleType + "'");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Lamp Module Type: " + strModuleType + " saved.')){window.location.href='LampModuleTypeMst.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                }
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

    #region btnUpdate_Click
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid();

            if (boolValid)
            {
                String strModuleType = Convert.ToString(txtModuleType.Text);
                String strEquipType = Convert.ToString(ddEquipType.SelectedValue);
                String strLightingDI = Convert.ToString(ddLampLightingDI.SelectedValue);
                String strLightingAI = Convert.ToString(ddLampLightingAI.SelectedValue);
                String strColorDI = Convert.ToString(ddLampColorDI.SelectedValue);
                String strColorAI = Convert.ToString(ddLampColorAI.SelectedValue);
                String tempModuleType = Convert.ToString(lblTmpModuleType.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.UpdLampModuleTypeMst(strModuleType, strEquipType, strLightingDI, strLightingAI, strColorDI, strColorAI, tempModuleType, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to update Lamp Module Type: " + tempModuleType + " .");
                    }
                    else
                    {
                        //csDatabase.UpdLampModuleTypeRem(strModuleType, tempModuleType);
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> updated Lamp Module Type '" + tempModuleType + "' to '" + strModuleType + "'");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Lamp Module Type: " + strModuleType + " updated.')){window.location.href='LampModuleTypeMst.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region BtnBack
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("LampModuleTypeMst.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Selected Index Change
    protected void ddLampLightingDI_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddLampLightingDI.SelectedValue == "No Lighting")
        {
            try
            {
                ddLampColorDI.Items.Clear();
                ddLampColorDI.DataSource = "";
                ddLampColorDI.DataBind();
                ddLampColorDI.Items.Insert(0, " ");
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
        else
        {
            try
            {
                ddLampColorDI.DataSource = GlobalFunc.getLampColor();
                ddLampColorDI.DataTextField = "Description";
                ddLampColorDI.DataValueField = "Description";
                ddLampColorDI.DataBind();
                ddLampColorDI.Items.Insert(0, " ");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Info",
                "alert('" + ex + "');", true);
            }
        }
    }

    protected void ddLampLightingAI_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddLampLightingAI.SelectedValue == "No Lighting")
        {
            try
            {
                ddLampColorAI.Items.Clear();
                ddLampColorAI.DataSource = "";
                ddLampColorAI.DataBind();
                ddLampColorAI.Items.Insert(0, " ");
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
        else
        {
            try
            {
                ddLampColorAI.DataSource = GlobalFunc.getLampColor();
                ddLampColorAI.DataTextField = "Description";
                ddLampColorAI.DataValueField = "Description";
                ddLampColorAI.DataBind();
                ddLampColorAI.Items.Insert(0, " ");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Info",
                "alert('" + ex + "');", true);
            }
        }
    }
    #endregion

    #region Paging Index
    protected void gvLampModuleTypeMst_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            showRefGrid();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
