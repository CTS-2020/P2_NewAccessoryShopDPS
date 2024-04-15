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
        String tempPlcNo = Convert.ToString(Session["SessTempPlcNo"]);                  //Added by YanTeng 08/12/2020
        String tempProcName = Convert.ToString(Session["SessTempProName"]);             //Added by YanTeng 08/12/2020
        String tempBlockName = Convert.ToString(Session["SessTempBlockName"]);

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
                getLampLighting();
                getLampColor();

                if (tempPlcNo != "" && tempProcName != "" && tempBlockName != "") 
                {
                    ddProcName.Enabled = false;
                    btnUpdate.Visible = true;
                    btnNewBlock.Visible = false;

                    DataSet dsBlockMst = new DataSet();
                    DataTable dtBlockMst = new DataTable();

                    //Modified by YanTeng 08/12/2020
                    //dsBlockMst = csDatabase.SrcBlockMst("", "", Convert.ToString(tempBlockName), "", "", "", "", "", "", "");
                    dsBlockMst = csDatabase.SrcBlockMst(Convert.ToString(tempPlcNo), "", Convert.ToString(tempBlockName), "", "", "", Convert.ToString(tempProcName), "", "", "");
                    dtBlockMst = dsBlockMst.Tables[0];
                    BindtoText(dtBlockMst);
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnNewBlock.Visible = true;
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
            ddStartModuleType.DataValueField = "Value";
            ddStartModuleType.DataBind();
            ddStartModuleType.Items.Insert(0, " ");

            ddEndModuleType.DataSource = GlobalFunc.getModuleType("End LM");
            ddEndModuleType.DataTextField = "Description";
            ddEndModuleType.DataValueField = "Value";
            ddEndModuleType.DataBind();
            ddEndModuleType.Items.Insert(0, " ");
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
            #region Wait Instruction
            ddLampLightingWI.DataSource = GlobalFunc.getLampLighting();
            ddLampLightingWI.DataTextField = "Description";
            ddLampLightingWI.DataValueField = "Description";
            ddLampLightingWI.DataBind();
            ddLampLightingWI.Items.Insert(0, " ");
            #endregion

            #region Error
            ddLampLightingERR.DataSource = GlobalFunc.getLampLighting();
            ddLampLightingERR.DataTextField = "Description";
            ddLampLightingERR.DataValueField = "Description";
            ddLampLightingERR.DataBind();
            ddLampLightingERR.Items.Insert(0, " ");
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
            #region Wait Instruction
                ddLampColorWI.DataSource = GlobalFunc.getLampColor();
                ddLampColorWI.DataTextField = "Description";
                ddLampColorWI.DataValueField = "Description";
                ddLampColorWI.DataBind();
                ddLampColorWI.Items.Insert(0, " ");
            #endregion

            #region Error
                ddLampColorERR.DataSource = GlobalFunc.getLampColor();
                ddLampColorERR.DataTextField = "Description";
                ddLampColorERR.DataValueField = "Description";
                ddLampColorERR.DataBind();
                ddLampColorERR.Items.Insert(0, " ");
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
                if (Convert.ToString(dt.Rows[0]["plc_no"]).Trim() != "")
                {
                    ddProcName.SelectedValue = Convert.ToString(dt.Rows[0]["plc_no"]);
                    getGroupName();
                    getLmAdd();
                }
                if (Convert.ToString(dt.Rows[0]["block_name"]).Trim() != "")
                {
                    lblTmpBlockName.Text = Convert.ToString(dt.Rows[0]["block_name"]).Trim();
                    txtBlockName.Text = Convert.ToString(dt.Rows[0]["block_name"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["block_seq"]).Trim() != "")
                {
                    txtBlockSeq.Text = Convert.ToString(dt.Rows[0]["block_seq"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["gw_no"]).Trim() != "")
                {
                    txtGwNo.Text = Convert.ToString(dt.Rows[0]["gw_no"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["start_lm"]).Trim() != "")
                {
                    ddStartLm.SelectedValue = Convert.ToString(dt.Rows[0]["start_lm"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["end_lm"]).Trim() != "")
                {
                    ddEndLm.SelectedValue = Convert.ToString(dt.Rows[0]["end_lm"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["group_name"]).Trim() != "")
                {
                    ddGroupName.SelectedValue = Convert.ToString(dt.Rows[0]["group_name"]);
                }
                if (Convert.ToString(dt.Rows[0]["start_module_type"]).Trim() != "")
                {
                    ddStartModuleType.SelectedValue = Convert.ToString(dt.Rows[0]["start_module_type"]);
                }
                if (Convert.ToString(dt.Rows[0]["end_module_type"]).Trim() != "")
                {
                    ddEndModuleType.SelectedValue = Convert.ToString(dt.Rows[0]["end_module_type"]);
                }
                if (Convert.ToString(dt.Rows[0]["light_wi"]).Trim() != "")
                {
                    ddLampLightingWI.SelectedValue = Convert.ToString(dt.Rows[0]["light_wi"]);
                }
                if (Convert.ToString(dt.Rows[0]["color_wi"]).Trim() != "")
                {
                    ddLampColorWI.SelectedValue = Convert.ToString(dt.Rows[0]["color_wi"]);
                }
                if (Convert.ToString(dt.Rows[0]["light_err"]).Trim() != "")
                {
                    ddLampLightingERR.SelectedValue = Convert.ToString(dt.Rows[0]["light_err"]);
                }
                if (Convert.ToString(dt.Rows[0]["color_err"]).Trim() != "")
                {
                    ddLampColorERR.SelectedValue = Convert.ToString(dt.Rows[0]["color_err"]);
                }
            }
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
            if (ddProcName.Enabled.Equals(true))
            {
                ddProcName.SelectedIndex = 0;
            }
            txtBlockSeq.Text = "";
            txtBlockName.Text = "";
            txtGwNo.Text = "";
            ddStartLm.SelectedIndex = 0;
            ddEndLm.SelectedIndex = 0;
            ddGroupName.SelectedIndex = 0;
            ddStartModuleType.SelectedIndex = 0;
            ddEndModuleType.SelectedIndex = 0;
            ddLampLightingWI.SelectedIndex = 0;
            ddLampColorWI.SelectedIndex = 0;
            ddLampLightingERR.SelectedIndex = 0;
            ddLampColorERR.SelectedIndex = 0;
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
            if (ddProcName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Process Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtBlockName.Text) == "")
            {
                lblMsg.Text = "Please enter Block Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicateBlockName(Convert.ToString(ddProcName.SelectedValue), Convert.ToString(ddProcName.SelectedItem), Convert.ToString(txtBlockName.Text), Convert.ToString(lblTmpBlockName.Text)))
            {
                lblMsg.Text = "Duplicate Block Name not allowed. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtBlockSeq.Text) == "")
            {
                lblMsg.Text = "Please enter Block Seq.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Int32.Parse(txtBlockSeq.Text.Trim()) < 1 || Int32.Parse(txtBlockSeq.Text.Trim()) > 12)
            {
                lblMsg.Text = "Block Seq must within range 1 - 12.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddGroupName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Group Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtGwNo.Text) == "")
            {
                lblMsg.Text = "Please enter G/W No.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Int32.Parse(txtGwNo.Text.Trim()) < 1 || Int32.Parse(txtGwNo.Text.Trim()) > 12)
            {
                lblMsg.Text = "Gw No must within range 1 - 12.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkGwDuplicate(Convert.ToString(ddProcName.SelectedItem), Convert.ToString(txtGwNo.Text), Convert.ToString(lblTmpBlockName.Text)))
            {
                lblMsg.Text = "G/W No " + Convert.ToString(txtGwNo.Text) + " existed in Process " + Convert.ToString(ddProcName.SelectedItem)  + ". Please use a different G/W No";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(ddStartLm.SelectedIndex) == "")
            {
                lblMsg.Text = "Please select Start LM.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddStartModuleType.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Start LM Module Type.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddLampLightingWI.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Lamp Lighting Wait Instruction.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddLampLightingERR.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Lamp Lighting Error.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddLampColorWI.SelectedIndex == 0 && ddLampLightingWI.SelectedValue != "No Lighting")
            {
                lblMsg.Text = "Please select Lamp Color Wait Instruction.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddLampColorERR.SelectedIndex == 0 && ddLampLightingERR.SelectedValue != "No Lighting")
            {
                lblMsg.Text = "Please select Lamp Color Error.";
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

    #region showRefGrid
    private void showRefGrid()
    {
        try
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

            if (Convert.ToString(Request.QueryString["bs"]) != "") strBlockSeq = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["bs"]));
            if (Convert.ToString(Request.QueryString["bn"]) != "") strBlockName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["bn"]));
            if (Convert.ToString(Request.QueryString["gwn"]) != "") strGwNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["gwn"]));
            if (Convert.ToString(Request.QueryString["slm"]) != "") strStartLm = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["slm"]));
            if (Convert.ToString(Request.QueryString["elm"]) != "") strEndLm = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["elm"]));
            if (Convert.ToString(Request.QueryString["pno"]) != "") strPlcNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pno"]));
            if (Convert.ToString(Request.QueryString["pnm"]) != "") strProcName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pnm"]));
            if (Convert.ToString(Request.QueryString["gnm"]) != "") strGroupName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["gnm"]));
            if (Convert.ToString(Request.QueryString["smt"]) != "") strStartModuleType = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["smt"]));
            if (Convert.ToString(Request.QueryString["emt"]) != "") strEndModuleType = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["emt"]));
            if (Convert.ToString(Request.QueryString["lwi"]) != "") strLightWI = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["lwi"]));
            if (Convert.ToString(Request.QueryString["cwi"]) != "") strColorWI = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["cwi"]));
            if (Convert.ToString(Request.QueryString["le"]) != "") strLightErr = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["le"]));
            if (Convert.ToString(Request.QueryString["ce"]) != "") strColorErr = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["ce"]));

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = csDatabase.SrcBlockMst(strPlcNo, strBlockSeq, strBlockName, strGwNo, strStartLm, strEndLm, strProcName, strGroupName, strStartModuleType, strEndModuleType);
            dtSearch = dsSearch.Tables[0];

            DataView dvBlockMst = new DataView(dtSearch);

            gvBlockMst.DataSource = dvBlockMst;
            gvBlockMst.PageIndex = NewPageIndex;
            gvBlockMst.DataBind();
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
            Boolean boolValid = ChkDataValid();
            if (boolValid)
            {
                String strBlockSeq = Convert.ToString(txtBlockSeq.Text);
                String strBlockName = Convert.ToString(txtBlockName.Text);
                String strGwNo = Convert.ToString(txtGwNo.Text);
                String strStartLm = Convert.ToString(ddStartLm.SelectedValue);
                String strEndLm = Convert.ToString(ddEndLm.SelectedValue);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strGroupName = Convert.ToString(ddGroupName.SelectedValue);
                String strStartModuleType = Convert.ToString(ddStartModuleType.SelectedValue);
                String strEndModuleType = Convert.ToString(ddEndModuleType.SelectedValue);
                String strLightingWI = Convert.ToString(ddLampLightingWI.SelectedValue);
                String strLightingERR = Convert.ToString(ddLampLightingERR.SelectedValue);
                String strColorWI = Convert.ToString(ddLampColorWI.SelectedValue);
                String strColorERR = Convert.ToString(ddLampColorERR.SelectedValue);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                if (csDatabase.ChkBlockMaxCnt(strProcName))
                {
                    Response.Write("<script language='javascript'>alert('Maximum Limit Block Count of 12 per Process reached. Please delete/edit unused block.')</script>");
                    return;
                }
                

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.SvBlockMst(strPlcNo, strBlockSeq, strBlockName, strGwNo, strStartLm, strEndLm, strProcName, strGroupName, strStartModuleType, strEndModuleType, strLightingWI, strLightingERR, strColorWI, strColorERR, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to save Block Name: " + strBlockName + " .");
                    }
                    else
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> created Block Name['" + strBlockName + "'] " +
                        "PLC No['" + strPlcNo + "'] Proc Name['" + strProcName + "'] " +
                        "Group Name['" + strGroupName + "'] Block Seq['" + strBlockSeq + "'] " +
                        "G/W No['" + strGwNo + "'] Start LM['" + strStartLm + "'] " +
                        "Start LM Module Type['" + strStartModuleType + "'] End LM['" + strEndLm + "'] " +
                        "End LM Module Type['" + strEndModuleType + "'] Wait Inst Light['" + strLightingWI + "'] " +
                        "Wait Inst Color['" + strColorWI + "'] Error Light['" + strLightingERR + "']" +
                        "Error Color['" + strColorERR + "']");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Block Name: " + strBlockName + " saved.')){window.location.href='BlockMst.aspx'}</script>";
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
                String strBlockSeq = Convert.ToString(txtBlockSeq.Text);
                String strBlockName = Convert.ToString(txtBlockName.Text);
                String strGwNo = Convert.ToString(txtGwNo.Text);
                String strStartLm = Convert.ToString(ddStartLm.SelectedValue);
                String strEndLm = Convert.ToString(ddEndLm.SelectedValue);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strGroupName = Convert.ToString(ddGroupName.SelectedValue);
                String strStartModuleType = Convert.ToString(ddStartModuleType.SelectedValue);
                String strEndModuleType = Convert.ToString(ddEndModuleType.SelectedValue);
                String tempBlockName = Convert.ToString(lblTmpBlockName.Text);
                String strColorWI = Convert.ToString(ddLampColorWI.SelectedValue);
                String strColorERR = Convert.ToString(ddLampColorERR.SelectedValue);
                String strLightingWI = Convert.ToString(ddLampLightingWI.SelectedValue);
                String strLightingERR = Convert.ToString(ddLampLightingERR.SelectedValue);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.UpdBlockMst(strPlcNo, strBlockSeq, strBlockName, strGwNo, strStartLm, strEndLm, strProcName, strGroupName, strStartModuleType, strEndModuleType, strLightingWI, strLightingERR, strColorWI, strColorERR, strCurUser, tempBlockName);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to update Block Name: " + tempBlockName + " for Process Name: " + strProcName + ".");
                    }
                    else
                    {
                        //csDatabase.UpdBlockMstRem(strPlcNo, strProcName, strGroupName, strBlockName, strGwNo, strBlockSeq, tempBlockName);
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> updated ['" + tempBlockName + "'] to Block Name['" + strBlockName + "'] " +
                        "PLC No['" + strPlcNo + "'] Proc Name['" + strProcName + "'] " +
                        "Group Name['" + strGroupName + "'] Block Seq['" + strBlockSeq + "'] " +
                        "G/W No['" + strGwNo + "'] Start LM['" + strStartLm + "'] " +
                        "Start LM Module Type['" + strStartModuleType + "'] End LM['" + strEndLm + "'] " +
                        "End LM Module Type['" + strEndModuleType + "'] Wait Inst Light['" + strLightingWI + "'] " +
                        "Wait Inst Color['" + strColorWI + "'] Error Light['" + strLightingERR + "']" +
                        "Error Color['" + strColorERR + "']");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Block Name: " + strBlockName + " has updated for Process Name: " + strProcName + ".')){window.location.href='BlockMst.aspx'}</script>";
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
            Response.Redirect("BlockMst.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Selected Index Change
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

    protected void ddLampLightingWI_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddLampLightingWI.SelectedValue == "No Lighting")
        {
            try
            {
                ddLampColorWI.Items.Clear();
                ddLampColorWI.DataSource = "";
                ddLampColorWI.DataBind();
                ddLampColorWI.Items.Insert(0, " ");
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
                ddLampColorWI.DataSource = GlobalFunc.getLampColor();
                ddLampColorWI.DataTextField = "Description";
                ddLampColorWI.DataValueField = "Description";
                ddLampColorWI.DataBind();
                ddLampColorWI.Items.Insert(0, " ");
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
    }

    protected void ddLampLightingERR_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddLampLightingERR.SelectedValue == "No Lighting")
        {
            try
            {
                ddLampColorERR.Items.Clear();
                ddLampColorERR.DataSource = "";
                ddLampColorERR.DataBind();
                ddLampColorERR.Items.Insert(0, " ");
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
                ddLampColorERR.DataSource = GlobalFunc.getLampColor();
                ddLampColorERR.DataTextField = "Description";
                ddLampColorERR.DataValueField = "Description";
                ddLampColorERR.DataBind();
                ddLampColorERR.Items.Insert(0, " ");
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
    }

    protected void ddStartLm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            String strStartLm = Convert.ToString(ddStartLm.SelectedItem).Trim();
            if (strStartLm != "")
            {
                String sqlQuery = sqlQuery = "SELECT DISTINCT LA.module_type as ReturnField, len(LA.module_type) FROM dt_LampModuleAddMst LA, dt_LampModuleTypeMst LT WHERE LT.equip_type = 'Start LM' AND LT.module_type = LA.module_type AND LA.module_add = '" + strStartLm + "' ORDER BY len(LA.module_type), LA.module_type";
                ddStartModuleType.SelectedValue = Convert.ToString(ConnQuery.getReturnFieldExecuteReader(sqlQuery));
            }
            else
            {
                ddStartModuleType.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddEndLm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            String strEndLm = Convert.ToString(ddEndLm.SelectedItem).Trim();
            if (strEndLm != "")
            {
                String sqlQuery = sqlQuery = "SELECT DISTINCT LA.module_type as ReturnField, len(LA.module_type) FROM dt_LampModuleAddMst LA, dt_LampModuleTypeMst LT WHERE LT.equip_type = 'End LM' AND LT.module_type = LA.module_type AND LA.module_add = '" + strEndLm + "' ORDER BY len(LA.module_type), LA.module_type";
                ddEndModuleType.SelectedValue = Convert.ToString(ConnQuery.getReturnFieldExecuteReader(sqlQuery));
            }
            else
            {
                ddEndModuleType.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddStartModuleType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddStartLm.Items.Clear();
            String strStartModuleType = Convert.ToString(ddStartModuleType.SelectedItem).Trim();
            String sqlQuery = "";

            if (strStartModuleType == "")
            {
                sqlQuery = "SELECT DISTINCT LA.module_add as Description, len(LA.module_add) FROM dt_LampModuleAddMst LA, dt_LampModuleTypeMst LT WHERE LT.equip_type = 'Start LM' AND LT.module_type = LA.module_type ORDER BY len(LA.module_type), LA.module_type";
            }
            else
            {
                sqlQuery = "SELECT DISTINCT LA.module_add as Description, len(LA.module_add) FROM dt_LampModuleAddMst LA, dt_LampModuleTypeMst LT WHERE LT.equip_type = 'Start LM' AND LT.module_type = LA.module_type AND LA.module_type = '" + strStartModuleType + "' ORDER BY len(LA.module_add), LA.module_add";
            }
            ddStartLm.DataSource = ConnQuery.getBindingDatasetData(sqlQuery);
            ddStartLm.DataTextField = "Description";
            ddStartLm.DataValueField = "Description";
            ddStartLm.DataBind();
            ddStartLm.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddEndModuleType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddEndLm.Items.Clear();
            String strEndModuleType = Convert.ToString(ddEndModuleType.SelectedItem).Trim();
            String sqlQuery = "";

            if (strEndModuleType == "")
            {
                sqlQuery = "SELECT DISTINCT LA.module_add as Description, len(LA.module_add) FROM dt_LampModuleAddMst LA, dt_LampModuleTypeMst LT WHERE LT.equip_type = 'End LM' AND LT.module_type = LA.module_type ORDER BY len(LA.module_type), LA.module_type";
            }
            else
            {
                sqlQuery = "SELECT DISTINCT LA.module_add as Description, len(LA.module_add) FROM dt_LampModuleAddMst LA, dt_LampModuleTypeMst LT WHERE LT.equip_type = 'End LM' AND LT.module_type = LA.module_type AND LA.module_type = '" + strEndModuleType + "' ORDER BY len(LA.module_add), LA.module_add";
            }
            ddEndLm.DataSource = ConnQuery.getBindingDatasetData(sqlQuery);
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

    #region Paging Index
    protected void gvBlockMst_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
