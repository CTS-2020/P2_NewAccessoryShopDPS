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

public partial class GroupMst : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }
    
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        String tempGroupId = Convert.ToString(Session["SessTempGroupId"]);
        String tempPlcNo = Convert.ToString(Session["SessTempPlcNo"]);
        String tempProcName = Convert.ToString(Session["SessTempProcName"]);

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

                if (tempGroupId != "")
                {
                    ddProcName.Enabled = false;
                    btnUpdate.Visible = true;
                    btnNewGroup.Visible = false;

                    DataSet dsGroupMst = new DataSet();
                    DataTable dtGroupMst = new DataTable();

                    dsGroupMst = csDatabase.SrcGroupMst(Convert.ToString(tempGroupId), "", Convert.ToString(tempPlcNo), Convert.ToString(tempProcName));
                    dtGroupMst = dsGroupMst.Tables[0];
                    BindtoText(dtGroupMst);
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnNewGroup.Visible = true;
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

    #region BindtoText
    public void BindtoText(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["group_id"]).Trim() != "")
                {
                    lblTmpGroupId.Text = Convert.ToString(dt.Rows[0]["group_id"]).Trim();
                    txtGroupId.Text = Convert.ToString(dt.Rows[0]["group_id"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["group_name"]).Trim() != "")
                {
                    txtGroupName.Text = Convert.ToString(dt.Rows[0]["group_name"]).Trim();
                    lblTmpGroupName.Text = Convert.ToString(dt.Rows[0]["group_name"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["plc_no"]).Trim() != "")
                {
                    ddProcName.SelectedValue = Convert.ToString(dt.Rows[0]["plc_no"]);
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
            txtGroupId.Text = "";
            txtGroupName.Text = "";
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
            else if (Convert.ToString(txtGroupId.Text) == "")
            {
                lblMsg.Text = "Please enter Group ID.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtGroupName.Text) == "")
            {
                lblMsg.Text = "Please enter Group Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicateGroupName(Convert.ToString(txtGroupName.Text), Convert.ToString(lblTmpGroupName.Text), Convert.ToString(ddProcName.SelectedItem)))
            {
                lblMsg.Text = "Duplicate Group Name is not allowed in same Process. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicateGroupID(Convert.ToString(txtGroupId.Text), Convert.ToString(lblTmpGroupId.Text), Convert.ToString(ddProcName.SelectedItem)))
            {
                lblMsg.Text = "Duplicate Group ID is not allowed in same Process. Please check.";
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
            String strGroupID = "";
            String strGroupName = "";
            String strPlcNo = "";
            String strProcName = "";

            if (Convert.ToString(Request.QueryString["gid"]) != "") strGroupID = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["gid"]));
            if (Convert.ToString(Request.QueryString["gnm"]) != "") strGroupName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["gnm"]));
            if (Convert.ToString(Request.QueryString["pno"]) != "") strPlcNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pno"]));
            if (Convert.ToString(Request.QueryString["pnm"]) != "") strProcName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pnm"]));

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = csDatabase.SrcGroupMst(strGroupID, strGroupName, strPlcNo, strProcName);
            dtSearch = dsSearch.Tables[0];

            DataView dvGroupMst = new DataView(dtSearch);

            gvGroupMst.DataSource = dvGroupMst;
            gvGroupMst.PageIndex = NewPageIndex;
            gvGroupMst.DataBind();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion

    #region Events

    #region btnNewGroup
    protected void btnNewGroup_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid();
            if (boolValid)
            {
                String strGroupID = Convert.ToString(txtGroupId.Text);
                String strGroupName = Convert.ToString(txtGroupName.Text);
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String strCurUser = Convert.ToString(Session["SessUserId"]);
                
                if (csDatabase.ChkGroupMaxCnt(strProcName))
                {
                    Response.Write("<script language='javascript'>alert('Maximum Limit Group Count of 6 per Process reached. Please delete/edit unused group.')</script>");
                    return;
                }

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.SvGroupMst(strGroupID, strGroupName, strPlcNo, strProcName, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to save Group ID: " + strGroupID + " .");
                    }
                    else
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> created Group ID['" + strGroupID + "'] Group Name['" + strGroupName + "'] PLC No['" + strPlcNo + "'] Process Name['" + strProcName + "']");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Group ID: " + strGroupID + " saved.')){window.location.href='GroupMst.aspx'}</script>";
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
                String strGroupID = Convert.ToString(txtGroupId.Text);
                String strGroupName = Convert.ToString(txtGroupName.Text);
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String tempGroupID = Convert.ToString(lblTmpGroupId.Text);
                String tempGroupName = Convert.ToString(lblTmpGroupName.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);
                
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.UpdGroupMst(strGroupID, strGroupName, strPlcNo, strProcName, tempGroupID, strCurUser, tempGroupName);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to update Group ID: " + tempGroupID + " .");
                    }
                    else
                    {
                        //csDatabase.UpdGroupMstRem(strPlcNo, strProcName, strGroupID, strGroupName, tempGroupID, tempGroupName);
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> updated ['" + tempGroupID + "'] to Group ID['" + strGroupID + "'] Group Name['" + strGroupName + "'] PLC No['" + strPlcNo + "'] Process Name['" + strProcName + "']");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Group ID: " + strGroupID + " updated.')){window.location.href='GroupMst.aspx'}</script>";
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
            Response.Redirect("GroupMst.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvGroupMst_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
