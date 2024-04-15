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
        Session["SessTempInsCode"] = "";

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
                //getDdKatashiki();
                //getDdSfx();
                //getDdColor();
                NewPageIndex = 0;
                //SearchInsCodeMstList();
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

    #region BindGridView
    private bool BindGridView(DataTable dtInsCodeMstList)
    {
        try
        {
            DataView dvInsCodeMstList = new DataView(dtInsCodeMstList);

            gvInsCodeMstList.DataSource = dvInsCodeMstList;
            gvInsCodeMstList.PageIndex = NewPageIndex;
            gvInsCodeMstList.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region SearchInsCodeMstList
    private void SearchInsCodeMstList()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strModel = Convert.ToString(ddModel.SelectedValue).Trim();
            String strKatashiki = Convert.ToString(ddKatashiki.SelectedValue).Trim();
            String strSfx = Convert.ToString(ddSfx.SelectedValue).Trim();
            String strColor = Convert.ToString(ddColor.SelectedValue).Trim();
            String strDpsInsCode = Convert.ToString(txtInsCode.Text).Trim();
            String strComment = Convert.ToString(txtComment.Text).Trim();

            dsSearch = csDatabase.SrcInsCode(strDpsInsCode, strModel, strKatashiki, strSfx, strColor, strComment);
            dtSearch = dsSearch.Tables[0];
            BindGridView(dtSearch);

            //if (dtSearch.Rows.Count > 0)
            //{
            //    for (int iRackCnt = 0; iRackCnt < dtSearch.Rows.Count; iRackCnt++)
            //    {
            //        String tmpInsCode = Convert.ToString(dtSearch.Rows[iRackCnt]["ins_code"]);
            //        String tmpComment = Convert.ToString(dtSearch.Rows[iRackCnt]["comment"]);
            //        String tmpModel = Convert.ToString(dtSearch.Rows[iRackCnt]["model"]);
            //        String tmpKatashiki = Convert.ToString(dtSearch.Rows[iRackCnt]["katashiki"]);
            //        String tmpSfx = Convert.ToString(dtSearch.Rows[iRackCnt]["sfx"]);
            //        String tmpColor = Convert.ToString(dtSearch.Rows[iRackCnt]["color"]);
            //        csDatabase.UpdInsCodeAscii(tmpInsCode, tmpModel, tmpKatashiki, tmpSfx, tmpColor, tmpComment);
            //    }
            //}
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
            SearchInsCodeMstList();
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
        String strModel = "";
        String strKatashiki = "";
        String strSfx = "";
        String strColor = "";
        String strDpsInsCode = "";
        String strComment = "";

        if (Convert.ToString(ddModel.SelectedValue).Trim() != "") strModel = GlobalFunc.getReplaceToUrl(Convert.ToString(ddModel.SelectedValue));
        if (Convert.ToString(ddKatashiki.SelectedValue).Trim() != "") strKatashiki = GlobalFunc.getReplaceToUrl(Convert.ToString(ddKatashiki.SelectedValue));
        if (Convert.ToString(ddSfx.SelectedValue).Trim() != "") strSfx = GlobalFunc.getReplaceToUrl(Convert.ToString(ddSfx.SelectedValue));
        if (Convert.ToString(ddColor.SelectedValue).Trim() != "") strColor = GlobalFunc.getReplaceToUrl(Convert.ToString(ddColor.SelectedValue));
        if (Convert.ToString(txtInsCode.Text).Trim() != "") strDpsInsCode = GlobalFunc.getReplaceToUrl(Convert.ToString(txtInsCode.Text));
        if (Convert.ToString(txtComment.Text).Trim() != "") strComment = GlobalFunc.getReplaceToUrl(Convert.ToString(txtComment.Text));

        String strRedirect = "DpsInsCodeMstReg.aspx?";

        strRedirect = strRedirect + "model=" + strModel + "&";
        strRedirect = strRedirect + "katashiki=" + strKatashiki + "&";
        strRedirect = strRedirect + "sfx=" + strSfx + "&";
        strRedirect = strRedirect + "color=" + strColor + "&";
        strRedirect = strRedirect + "inscode=" + strDpsInsCode + "&";
        strRedirect = strRedirect + "comment=" + strComment + "&";

        return strRedirect;
    }
    #endregion

    #endregion

    #region Events

    #region btnNewInsCode
    protected void btnNewInsCode_Click(object sender, EventArgs e)
    {
        if (!csDatabase.ChkInsCodeMaxCnt())
        {
            Response.Redirect(GetRedirectString());
        }
        else
        {
            Response.Write("<script language='javascript'>alert('Maximum Instruction Code of 299 reached (Excluding reserved Instruction Code [0]). Please edit/delete unused Instruction Code.')</script>");
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
            SearchInsCodeMstList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvInsCodeMstList_RowDataBound
    protected void gvInsCodeMstList_RowDataBound(object sender, GridViewRowEventArgs e)
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

    #region gvInsCodeMstList_RowCommand
    protected void gvInsCodeMstList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 index = Convert.ToInt32(e.CommandArgument);            

            if (e.CommandName == "EditRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                Session["SessTempInsCode"] = Convert.ToString(selectedRow.Cells[0].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit Instruction Code['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Sfx['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Color['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Katashiki['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Model['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Comment['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                Response.Redirect(GetRedirectString());
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String InsCode = Convert.ToString(selectedRow.Cells[0].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete Instruction Code['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Sfx['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Color['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Katashiki['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Model['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Comment['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                Boolean blDelInsCode = csDatabase.DelInsCode(InsCode);
                if (blDelInsCode)
                {
                    //csDatabase.DelInsCodeConvResult(InsCode);
                    GlobalFunc.ShowMessage("Instruction Code: " + InsCode + " deleted.");
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted Instruction Code['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Sfx['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Color['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Katashiki['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Model['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Comment['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                }
                else
                {
                    GlobalFunc.ShowMessage("Unable to delete Instruction Code: " + InsCode + ".");
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to delete Instruction Code['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Sfx['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Color['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Katashiki['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Model['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Comment['" + Convert.ToString(selectedRow.Cells[5].Text) + "']");
                }
                SearchInsCodeMstList();
            }
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
            SearchInsCodeMstList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

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

    #endregion
}
