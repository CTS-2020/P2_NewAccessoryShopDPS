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

public partial class DpsInsCodeMst : System.Web.UI.Page
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
        String tempInsCode = Convert.ToString(Session["SessTempInsCode"]);

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
                getDdModel();

                if (tempInsCode != "")
                {
                    btnUpdate.Visible = true;
                    btnNewInsCode.Visible = false;

                    DataSet dsInsCodeMst = new DataSet();
                    DataTable dtInsCodeMst = new DataTable();
                    dsInsCodeMst = csDatabase.SrcInsCode(Convert.ToString(tempInsCode), "", "", "", "", "");
                    dtInsCodeMst = dsInsCodeMst.Tables[0];
                    BindtoText(dtInsCodeMst);
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnNewInsCode.Visible = true;
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

    #region Get Drop Down List
    #region getDdModel
    private void getDdModel()
    {
        try
        {
            ddModel.DataSource = GlobalFunc.getModel();
            ddModel.DataTextField = "Description";
            ddModel.DataValueField = "Description";
            ddModel.DataBind();
            ddModel.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getDdKatashiki
    private void getDdKatashiki()
    {
        try
        {
            String strModel = ddModel.SelectedValue;
            ddKatashiki.DataSource = GlobalFunc.getKatashiki(strModel);
            ddKatashiki.DataTextField = "Description";
            ddKatashiki.DataValueField = "Description";
            ddKatashiki.DataBind();
            ddKatashiki.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getDdSfx
    private void getDdSfx()
    {
        try
        {
            String strModel = ddModel.SelectedValue;
            String strKatashiki = ddKatashiki.SelectedValue;
            ddSfx.DataSource = GlobalFunc.getSfx(strModel, strKatashiki);
            ddSfx.DataTextField = "Description";
            ddSfx.DataValueField = "Description";
            ddSfx.DataBind();
            ddSfx.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getDdColor
    private void getDdColor()
    {
        try
        {
            String strModel = ddModel.SelectedValue;
            String strKatashiki = ddKatashiki.SelectedValue;
            String strSfx = ddSfx.SelectedValue;
            ddColor.DataSource = GlobalFunc.getColor(strModel, strKatashiki, strSfx);
            ddColor.DataTextField = "Description";
            ddColor.DataValueField = "Description";
            ddColor.DataBind();
            ddColor.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion
    #endregion

    #region BindtoText
    public void BindtoText(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["ins_code"]).Trim() != "")
                {
                    lblTmpInsCode.Text = Convert.ToString(dt.Rows[0]["ins_code"]);
                    txtInsCode.Text = Convert.ToString(dt.Rows[0]["ins_code"]);
                }
                if (Convert.ToString(dt.Rows[0]["comment"]).Trim() != "")
                {
                    txtComment.Text = Convert.ToString(dt.Rows[0]["comment"]);
                }
                if (Convert.ToString(dt.Rows[0]["model"]).Trim() != "")
                {
                    ddModel.SelectedValue = Convert.ToString(dt.Rows[0]["model"]);
                    getDdKatashiki();
                }
                if (Convert.ToString(dt.Rows[0]["katashiki"]).Trim() != "")
                {
                    ddKatashiki.SelectedValue = Convert.ToString(dt.Rows[0]["katashiki"]);
                    getDdSfx();
                }
                if (Convert.ToString(dt.Rows[0]["sfx"]).Trim() != "")
                {
                    ddSfx.SelectedValue = Convert.ToString(dt.Rows[0]["sfx"]);
                    getDdColor();
                }
                if (Convert.ToString(dt.Rows[0]["color"]).Trim() != "")
                {
                    ddColor.SelectedValue = Convert.ToString(dt.Rows[0]["color"]);
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
            String strModel = "";
            String strKatashiki = "";
            String strSfx = "";
            String strColor = "";
            String strDpsInsCode = "";
            String strComment = "";

            if (Convert.ToString(Request.QueryString["model"]) != "") strModel = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["model"]));
            if (Convert.ToString(Request.QueryString["katashiki"]) != "") strKatashiki = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["katashiki"]));
            if (Convert.ToString(Request.QueryString["sfx"]) != "") strSfx = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["sfx"]));
            if (Convert.ToString(Request.QueryString["color"]) != "") strColor = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["color"]));
            if (Convert.ToString(Request.QueryString["inscode"]) != "") strDpsInsCode = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["inscode"]));
            if (Convert.ToString(Request.QueryString["comment"]) != "") strComment = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["comment"]));

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = csDatabase.SrcInsCode(strDpsInsCode, strModel, strKatashiki, strSfx, strColor, strComment);
            dtSearch = dsSearch.Tables[0];

            DataView dvInsCodeMstList = new DataView(dtSearch);

            gvInsCodeMstList.DataSource = dvInsCodeMstList;
            gvInsCodeMstList.PageIndex = NewPageIndex;
            gvInsCodeMstList.DataBind();
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
            txtInsCode.Text = "";
            ddModel.SelectedIndex = 0;
            ddColor.SelectedIndex = 0;
            ddSfx.SelectedIndex = 0;
            ddKatashiki.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Check Data Valid
    private Boolean ChkDataValid()
    {
        try
        {
            if (ddModel.SelectedIndex == 0)
            {
                lblMsg.Text = "Please enter Model.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddKatashiki.SelectedIndex == 0)
            {
                lblMsg.Text = "Please enter Katashiki.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddSfx.SelectedIndex == 0)
            {
                lblMsg.Text = "Please enter SFX.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddColor.SelectedIndex == 0)
            {
                lblMsg.Text = "Please enter Color.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtInsCode.Text) == "")
            {
                lblMsg.Text = "Please enter DPS Instruction Code.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToInt32(txtInsCode.Text) == 0)
            {
                lblMsg.Text = "(" + Convert.ToString(txtInsCode.Text) + ") is reserve for special case.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicateInsCodeCombi(Convert.ToString(ddModel.SelectedValue), Convert.ToString(ddKatashiki.SelectedValue), Convert.ToString(ddSfx.SelectedValue), Convert.ToString(ddColor.SelectedValue), Convert.ToString(lblTmpInsCode.Text)))
            {
                lblMsg.Text = "No duplicate combination of Model, Katashiki, Sfx, Color allowed. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicateInsCode(Convert.ToString(txtInsCode.Text), Convert.ToString(lblTmpInsCode.Text)))
            {
                lblMsg.Text = "No duplicate Instruction Code allowed. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            else if (txtComment.Text.Length > 14)
            {
                lblMsg.Text = "Comment can only be 14 characters. Please check.";
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
    #region On Selected Index Changed
    protected void ddModel_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getDdKatashiki();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddKatashiki_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getDdSfx();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddSfx_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getDdColor();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region btnNewInsCode
    protected void btnNewInsCode_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid();
            if (boolValid)
            {
                String strInsCode = Convert.ToString(txtInsCode.Text);
                String strComment = Convert.ToString(txtComment.Text);
                String strModel = Convert.ToString(ddModel.SelectedValue);
                String strKatashiki = Convert.ToString(ddKatashiki.SelectedValue);
                String strSfx = Convert.ToString(ddSfx.SelectedValue);
                String strColor = Convert.ToString(ddColor.SelectedValue);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.SvInsCode(strInsCode, strModel, strKatashiki, strSfx, strColor, strComment, strCurUser);
                    if (!tmpFlag)
                    {
                        lblMsg.Text = "Unable to save Instruction Code: " + strInsCode + " .";
                        lblMsg.Visible = true;
                    }
                    else
                    {
                        //csDatabase.UpdInsCodeAscii(strInsCode, strModel, strKatashiki, strSfx, strColor, strComment);
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> created Instruction Code['" + strInsCode + "'] Model['" + strModel + "'] Katashiki['" + strKatashiki + "'] Sfx['" + strSfx + "'] Color['" + strColor + "'] Comment['" + strComment + "']");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Instruction Code: " + strInsCode + " saved.')){window.location.href='DpsInsCodeMst.aspx'}</script>";
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

    #region btnUpdate
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid();
            if (boolValid)
            {
                String strInsCode = Convert.ToString(txtInsCode.Text);
                String strComment = Convert.ToString(txtComment.Text);
                String strModel = Convert.ToString(ddModel.SelectedValue);
                String strKatashiki = Convert.ToString(ddKatashiki.SelectedValue);
                String strSfx = Convert.ToString(ddSfx.SelectedValue);
                String strColor = Convert.ToString(ddColor.SelectedValue);
                String tempInsCode = Convert.ToString(lblTmpInsCode.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.UpdInsCode(strInsCode, strModel, strKatashiki, strSfx, strColor, strComment, tempInsCode, strCurUser);
                    if (!tmpFlag)
                    {
                        lblMsg.Text = "Unable to update Instruction Code: " + tempInsCode + " .";
                        lblMsg.Visible = true;
                    }
                    else
                    {
                        //csDatabase.UpdInsCodeAscii(strInsCode, strModel, strKatashiki, strSfx, strColor, strComment);
                        //csDatabase.UpdInsCodeConvResult(strInsCode, strModel, strKatashiki, strSfx, strColor, strComment, tempInsCode);
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> updated '" + tempInsCode + "' to Instruction Code['" + strInsCode + "'] Model['" + strModel + "'] Katashiki['" + strKatashiki + "'] Sfx['" + strSfx + "'] Color['" + strColor + "'] Comment['" + strComment + "']");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Instruction Code: " + tempInsCode + " updated.')){window.location.href='DpsInsCodeMst.aspx'}</script>";
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
            Response.Redirect("DpsInsCodeMst.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvInsCodeMstList_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
