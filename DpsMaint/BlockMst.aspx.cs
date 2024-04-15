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

public partial class BlockMst : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["SessTempBlockName"] = "";

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
                getGroupName();
                getLmAdd();
                getModuleType();
                //getLampLighting();
                //getLampColor();
                NewPageIndex = 0;
                //SearchBlockMst();
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

    #region getGroupName
    private void getGroupName()
    {
        try
        {
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            ddGroupName.DataSource = GlobalFunc.getGroupName(strProcName);
            ddGroupName.DataTextField = "Description";
            ddGroupName.DataValueField = "Description";
            ddGroupName.DataBind();
            ddGroupName.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getLmAdd
    private void getLmAdd()
    {
        try
        {
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            String strPlcNo = Convert.ToString(ddProcName.SelectedValue);

            ddStartLm.DataSource = GlobalFunc.getLmAdd("Start LM", strProcName, strPlcNo);
            ddStartLm.DataTextField = "Description";
            ddStartLm.DataValueField = "Description";
            ddStartLm.DataBind();
            ddStartLm.Items.Insert(0, " ");

            ddEndLm.DataSource = GlobalFunc.getLmAdd("End LM", strProcName, strPlcNo);
            ddEndLm.DataTextField = "Description";
            ddEndLm.DataValueField = "Description";
            ddEndLm.DataBind();
            ddEndLm.Items.Insert(0, " ");
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
            ddStartModuleType.DataSource = GlobalFunc.getModuleType("Start LM");
            ddStartModuleType.DataTextField = "Description";
            ddStartModuleType.DataValueField = "Description";
            ddStartModuleType.DataBind();
            ddStartModuleType.Items.Insert(0, " ");

            ddEndModuleType.DataSource = GlobalFunc.getModuleType("End LM");
            ddEndModuleType.DataTextField = "Description";
            ddEndModuleType.DataValueField = "Description";
            ddEndModuleType.DataBind();
            ddEndModuleType.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region GetLight
    protected String GetLight(Object objLight)
    {
        try
        {
            if (Convert.ToString(objLight) == "Lighting")
            {
                return "Light";
            }
            else if (Convert.ToString(objLight) == "Blinking")
            {
                return "Blink";
            }
            else
            {
                return "None";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "Data Error";
        }
    }
    #endregion

    //#region getLampLighting
    //private void getLampLighting()
    //{
    //    try
    //    {
    //        #region Wait Instruction
    //        ddLampLightingWI.DataSource = GlobalFunc.getLampLighting();
    //        ddLampLightingWI.DataTextField = "Description";
    //        ddLampLightingWI.DataValueField = "Description";
    //        ddLampLightingWI.DataBind();
    //        ddLampLightingWI.Items.Insert(0, " ");
    //        #endregion

    //        #region Error
    //        ddLampLightingERR.DataSource = GlobalFunc.getLampLighting();
    //        ddLampLightingERR.DataTextField = "Description";
    //        ddLampLightingERR.DataValueField = "Description";
    //        ddLampLightingERR.DataBind();
    //        ddLampLightingERR.Items.Insert(0, " ");
    //        #endregion
    //    }
    //    catch (Exception ex)
    //    {
    //        GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
    //    }
    //}
    //#endregion

    //#region getLampColor
    //private void getLampColor()
    //{
    //    try
    //    {
    //        #region Wait Instruction
    //        ddLampColorWI.DataSource = GlobalFunc.getLampColor();
    //        ddLampColorWI.DataTextField = "Description";
    //        ddLampColorWI.DataValueField = "Description";
    //        ddLampColorWI.DataBind();
    //        ddLampColorWI.Items.Insert(0, " ");
    //        #endregion

    //        #region Error
    //        ddLampColorERR.DataSource = GlobalFunc.getLampColor();
    //        ddLampColorERR.DataTextField = "Description";
    //        ddLampColorERR.DataValueField = "Description";
    //        ddLampColorERR.DataBind();
    //        ddLampColorERR.Items.Insert(0, " ");
    //        #endregion
    //    }
    //    catch (Exception ex)
    //    {
    //        GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
    //    }
    //}
    //#endregion

    #region SearchBlockMst
    private void SearchBlockMst()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strBlockSeq = Convert.ToString(txtBlockSeq.Text).Trim();
            String strBlockName = Convert.ToString(txtBlockName.Text).Trim();
            String strGwNo = Convert.ToString(txtGwNo.Text).Trim();
            String strStartLm = Convert.ToString(ddStartLm.SelectedValue).Trim();
            String strEndLm = Convert.ToString(ddEndLm.SelectedValue).Trim();
            String strPlcNo = Convert.ToString(ddProcName.SelectedValue).Trim();
            String strProcName = Convert.ToString(ddProcName.SelectedItem).Trim();
            String strGroupName = Convert.ToString(ddGroupName.SelectedValue).Trim();
            String strStartModuleType = Convert.ToString(ddStartModuleType.SelectedValue).Trim();
            String strEndModuleType = Convert.ToString(ddEndModuleType.SelectedValue).Trim();
            //String strLightWI = Convert.ToString(ddLampLightingWI.SelectedValue).Trim();
            //String strColorWI = Convert.ToString(ddLampColorWI.SelectedValue).Trim();
            //String strLightErr = Convert.ToString(ddLampLightingERR.SelectedValue).Trim();
            //String strColorErr = Convert.ToString(ddLampColorERR.SelectedValue).Trim();

            dsSearch = csDatabase.SrcBlockMst(strPlcNo, strBlockSeq, strBlockName, strGwNo, strStartLm, strEndLm, strProcName, strGroupName, strStartModuleType, strEndModuleType);
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
    private bool BindGridView(DataTable dtBlockMst)
    {
        try
        {
            DataView dvBlockMst = new DataView(dtBlockMst);

            gvBlockMst.DataSource = dvBlockMst;
            gvBlockMst.PageIndex = NewPageIndex;
            gvBlockMst.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region Get Redirect Pass-Out
    private String GetRedirectString()
    {
        String strBlockSeq = "";
        String strBlockName = "";
        String strGwNo = "";
        String strStartLm = "";
        String strEndLm = "";
        String strPlcNo = "";
        String strProcName = "";
        String strGroupName = "";
        String strStartModuleType = "";
        String strEndModuleType = "";
        String strLightWI = "";
        String strColorWI = "";
        String strLightErr = "";
        String strColorErr = "";

        if (Convert.ToString(txtBlockSeq.Text).Trim() != "") strBlockSeq = GlobalFunc.getReplaceToUrl(Convert.ToString(txtBlockSeq.Text));
        if (Convert.ToString(txtBlockName.Text).Trim() != "") strBlockName = GlobalFunc.getReplaceToUrl(Convert.ToString(txtBlockName.Text));
        if (Convert.ToString(txtGwNo.Text).Trim() != "") strGwNo = GlobalFunc.getReplaceToUrl(Convert.ToString(txtGwNo.Text));
        if (Convert.ToString(ddStartLm.SelectedValue).Trim() != "") strStartLm = GlobalFunc.getReplaceToUrl(Convert.ToString(ddStartLm.SelectedValue));
        if (Convert.ToString(ddEndLm.SelectedValue).Trim() != "") strEndLm = GlobalFunc.getReplaceToUrl(Convert.ToString(ddEndLm.SelectedValue));
        if (Convert.ToString(ddProcName.SelectedValue).Trim() != "") strPlcNo = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedValue));
        if (Convert.ToString(ddProcName.SelectedItem).Trim() != "") strProcName = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedItem));
        if (Convert.ToString(ddGroupName.SelectedValue).Trim() != "") strGroupName = GlobalFunc.getReplaceToUrl(Convert.ToString(ddGroupName.SelectedValue));
        if (Convert.ToString(ddStartModuleType.SelectedValue).Trim() != "") strStartModuleType = GlobalFunc.getReplaceToUrl(Convert.ToString(ddStartModuleType.SelectedValue));
        if (Convert.ToString(ddEndModuleType.SelectedValue).Trim() != "") strEndModuleType = GlobalFunc.getReplaceToUrl(Convert.ToString(ddEndModuleType.SelectedValue));
        //if (Convert.ToString(ddLampLightingWI.SelectedValue).Trim() != "") strLightWI = GlobalFunc.getReplaceToUrl(Convert.ToString(ddLampLightingWI.SelectedValue));
        //if (Convert.ToString(ddLampColorWI.SelectedValue).Trim() != "") strColorWI = GlobalFunc.getReplaceToUrl(Convert.ToString(ddLampColorWI.SelectedValue));
        //if (Convert.ToString(ddLampLightingERR.SelectedValue).Trim() != "") strLightErr = GlobalFunc.getReplaceToUrl(Convert.ToString(ddLampLightingERR.SelectedValue));
        //if (Convert.ToString(ddLampColorERR.SelectedValue).Trim() != "") strColorErr = GlobalFunc.getReplaceToUrl(Convert.ToString(ddLampColorERR.SelectedValue));

        String strRedirect = "BlockMstReg.aspx?";

        strRedirect = strRedirect + "bs=" + strBlockSeq + "&";
        strRedirect = strRedirect + "bn=" + strBlockName + "&";
        strRedirect = strRedirect + "gwn=" + strGwNo + "&";
        strRedirect = strRedirect + "slm=" + strStartLm + "&";
        strRedirect = strRedirect + "elm=" + strEndLm + "&";
        strRedirect = strRedirect + "pno=" + strPlcNo + "&";
        strRedirect = strRedirect + "pnm=" + strProcName + "&";
        strRedirect = strRedirect + "gnm=" + strGroupName + "&";
        strRedirect = strRedirect + "smt=" + strStartModuleType + "&";
        strRedirect = strRedirect + "emt=" + strEndModuleType + "&";
        strRedirect = strRedirect + "lwi=" + strLightWI + "&";
        strRedirect = strRedirect + "cwi=" + strColorWI + "&";
        strRedirect = strRedirect + "le=" + strLightErr + "&";
        strRedirect = strRedirect + "ce=" + strColorErr + "&";

        return strRedirect;
    }
    #endregion

    #region ClearAll
    private void ClearAll()
    {
        try
        {
            txtBlockSeq.Text = "";
            txtBlockName.Text = "";
            txtGwNo.Text = "";
            ddStartLm.SelectedIndex = 0;
            ddEndLm.SelectedIndex = 0;
            ddProcName.SelectedIndex = 0;
            ddGroupName.SelectedIndex = 0;
            ddStartModuleType.SelectedIndex = 0;
            ddEndModuleType.SelectedIndex = 0;
            //ddLampLightingWI.SelectedIndex = 0;
            //ddLampColorWI.SelectedIndex = 0;
            //ddLampLightingERR.SelectedIndex = 0;
            //ddLampColorERR.SelectedIndex = 0;
            SearchBlockMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion

    #region Events

    #region btnNewBlock
    protected void btnNewBlock_Click(object sender, EventArgs e)
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
            SearchBlockMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvBlockMst_RowDataBound
    protected void gvBlockMst_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.Cells[17].Controls[0];
                lb.OnClientClick = "return confirm('Confirm delete? ATTENTION : THIS WILL DELETE ALL RACK MASTER DATA REGISTERED UNDER CURRENT BLOCK!');";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvBlockMst_RowCommand
    protected void gvBlockMst_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 index = Convert.ToInt32(e.CommandArgument);            

            if (e.CommandName == "EditRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                Session["SessTempPlcNo"] = Convert.ToString(selectedRow.Cells[0].Text);             //Added by YanTeng 08/12/2020
                Session["SessTempProName"] = Convert.ToString(selectedRow.Cells[1].Text);           //Added by YanTeng 08/12/2020
                Session["SessTempBlockName"] = Convert.ToString(selectedRow.Cells[4].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit Block Name['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] " +
                "PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Proc Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] " + 
                "Group Name['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Block Seq['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] " +
                "G/W No['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Start LM['" + Convert.ToString(selectedRow.Cells[6].Text) + "'] " +
                "Start LM Module Type['" + Convert.ToString(selectedRow.Cells[7].Text) + "'] End LM['" + Convert.ToString(selectedRow.Cells[8].Text) + "'] " +
                "End LM Module Type['" + Convert.ToString(selectedRow.Cells[9].Text) + "'] Wait Inst Light['" + Convert.ToString(selectedRow.Cells[10].Text) + "'] " +
                "Wait Inst Color['" + Convert.ToString(selectedRow.Cells[11].Text) + "'] Error Light['" + Convert.ToString(selectedRow.Cells[12].Text) + "']" +
                "Error Color['" + Convert.ToString(selectedRow.Cells[13].Text) + "']");
                Response.Redirect(GetRedirectString());
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String strPlcNo = Convert.ToString(selectedRow.Cells[0].Text);
                String strProcName = Convert.ToString(selectedRow.Cells[1].Text);
                String strGroupName = Convert.ToString(selectedRow.Cells[2].Text);
                String strBlockName = Convert.ToString(selectedRow.Cells[4].Text);
                String strGwNo = Convert.ToString(selectedRow.Cells[5].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete Block Name['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] " +
                "PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Proc Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] " +
                "Group Name['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Block Seq['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] " +
                "G/W No['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Start LM['" + Convert.ToString(selectedRow.Cells[6].Text) + "'] " +
                "Start LM Module Type['" + Convert.ToString(selectedRow.Cells[7].Text) + "'] End LM['" + Convert.ToString(selectedRow.Cells[8].Text) + "'] " +
                "End LM Module Type['" + Convert.ToString(selectedRow.Cells[9].Text) + "'] Wait Inst Light['" + Convert.ToString(selectedRow.Cells[10].Text) + "'] " +
                "Wait Inst Color['" + Convert.ToString(selectedRow.Cells[11].Text) + "'] Error Light['" + Convert.ToString(selectedRow.Cells[12].Text) + "']" +
                "Error Color['" + Convert.ToString(selectedRow.Cells[13].Text) + "']");
                Boolean blDelBlock = csDatabase.DelBlockMst(strPlcNo, strProcName, strGroupName, strBlockName, strGwNo);
                if (!blDelBlock)
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to delete Block Name['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] " +
                    "PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Proc Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] " +
                    "Group Name['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Block Seq['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] " +
                    "G/W No['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Start LM['" + Convert.ToString(selectedRow.Cells[6].Text) + "'] " +
                    "Start LM Module Type['" + Convert.ToString(selectedRow.Cells[7].Text) + "'] End LM['" + Convert.ToString(selectedRow.Cells[8].Text) + "'] " +
                    "End LM Module Type['" + Convert.ToString(selectedRow.Cells[9].Text) + "'] Wait Inst Light['" + Convert.ToString(selectedRow.Cells[10].Text) + "'] " +
                    "Wait Inst Color['" + Convert.ToString(selectedRow.Cells[11].Text) + "'] Error Light['" + Convert.ToString(selectedRow.Cells[12].Text) + "']" +
                    "Error Color['" + Convert.ToString(selectedRow.Cells[13].Text) + "']");
                    GlobalFunc.ShowErrorMessage("Unable to delete Block name: " + strBlockName + ".");
                }
                else
                {
                    //csDatabase.DelBlockMstRem(strPlcNo, strProcName, strGroupName, strBlockName, strGwNo);
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted Block Name['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] " +
                    "PLC No['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Proc Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] " +
                    "Group Name['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Block Seq['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] " +
                    "G/W No['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Start LM['" + Convert.ToString(selectedRow.Cells[6].Text) + "'] " +
                    "Start LM Module Type['" + Convert.ToString(selectedRow.Cells[7].Text) + "'] End LM['" + Convert.ToString(selectedRow.Cells[8].Text) + "'] " +
                    "End LM Module Type['" + Convert.ToString(selectedRow.Cells[9].Text) + "'] Wait Inst Light['" + Convert.ToString(selectedRow.Cells[10].Text) + "'] " +
                    "Wait Inst Color['" + Convert.ToString(selectedRow.Cells[11].Text) + "'] Error Light['" + Convert.ToString(selectedRow.Cells[12].Text) + "']" +
                    "Error Color['" + Convert.ToString(selectedRow.Cells[13].Text) + "']");
                    GlobalFunc.ShowMessage("Block name: " + strBlockName + " deleted.");
                    //ClientScriptManager CSM = Page.ClientScript;
                    //string strconfirm = "<script>if(!window.alert('Block name: " + strBlockName + " deleted.'))</script>";
                    //CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
                SearchBlockMst();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvBlockMst_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchBlockMst();
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
            getGroupName();
            getLmAdd();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
