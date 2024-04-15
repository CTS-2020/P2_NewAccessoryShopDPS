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

public partial class UserMaintEntry : System.Web.UI.Page
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

        String tempUserID = Convert.ToString(Session["SessTempUserId"]);

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
                getRole();

                if (tempUserID != "")
                {
                    btnUpdate.Visible = true;
                    btnNewUser.Visible = false;

                    DataSet dsUser = new DataSet();
                    DataTable dtUser = new DataTable();
                    dsUser = csDatabase.searchUser(Convert.ToString(tempUserID),"","");
                    dtUser = dsUser.Tables[0];
                    BindtoText(dtUser);
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnNewUser.Visible = true;
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

    #region getRole
    private void getRole()
    {
        try
        {
            ddRole.DataSource = GlobalFunc.getRolelist();
            ddRole.DataTextField = "Description";
            ddRole.DataValueField = "Description";
            ddRole.DataBind();
            ddRole.Items.Insert(0, " ");
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
                if (Convert.ToString(dt.Rows[0]["user_id"]).Trim() != "")
                {
                    lblTmpUserId.Text = Convert.ToString(dt.Rows[0]["user_id"]).Trim();
                    txtUserID.Text = Convert.ToString(dt.Rows[0]["user_id"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["user_name"]).Trim() != "")
                {
                    txtUserName.Text = Convert.ToString(dt.Rows[0]["user_name"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["user_password"]).Trim() != "")
                {
                    txtUserPassword.Text = Convert.ToString(dt.Rows[0]["user_password"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["Role_Code"]).Trim() != "")
                {
                    ddRole.SelectedValue = Convert.ToString(dt.Rows[0]["Role_Code"]);
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
            String strUserId = "";
            String strUserName = "";
            String strRole = "";

            if (Convert.ToString(Request.QueryString["uid"]) != "") strUserId = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["uid"]));
            if (Convert.ToString(Request.QueryString["un"]) != "") strUserName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["un"]));
            if (Convert.ToString(Request.QueryString["role"]) != "") strRole = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["role"]));

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = csDatabase.searchUser(strUserId, strUserName, strRole);
            dtSearch = dsSearch.Tables[0];

            DataView dvUserList = new DataView(dtSearch);

            gvUserList.DataSource = dvUserList;
            gvUserList.PageIndex = NewPageIndex;
            gvUserList.DataBind();
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
            txtUserID.Text = "";
            txtUserName.Text = "";
            txtUserPassword.Text = "";
            ddRole.SelectedIndex = 0;
            lblMsg.Visible = false;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Checking
    private Boolean CheckValid()
    {
        try
        {
            if (Convert.ToString(txtUserID.Text.Trim()) == "")
            {
                lblMsg.Text = "Please enter User ID.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicateUid(Convert.ToString(txtUserID.Text), Convert.ToString(lblTmpUserId.Text)))
            {
                lblMsg.Text = "User ID existed.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsAlphaNum(Convert.ToString(txtUserID.Text)))
            {
                lblMsg.Text = "Only Alpha-Numeric Character is allowed for User ID.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddRole.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Role.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtUserName.Text.Trim()) == "")
            {
                lblMsg.Text = "Please enter User Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtUserPassword.Text.Trim()) == "")
            {
                lblMsg.Text = "Please enter User Password.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsAlphaNum(Convert.ToString(txtUserPassword.Text)))
            {
                lblMsg.Text = "Only Alpha-Numeric Character is allowed for User Password.";
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
    #region btnNewUser
    protected void btnNewUser_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckValid())
            {
                String strUserID = Convert.ToString(txtUserID.Text.Trim());
                String strUserName = Convert.ToString(txtUserName.Text.Trim());
                String strPassword = Convert.ToString(txtUserPassword.Text.Trim());
                String strRoleCode = Convert.ToString(ddRole.SelectedValue.Trim());
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.saveUser(strUserID, strUserName, strPassword, strRoleCode, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to save User ID: " + strUserID + " .");
                    }
                    else
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> created UID['" + strUserID + "'] Name['" + strUserName + "'] Password['" + strPassword + "'] Role['" + strRoleCode + "']");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('User ID: " + strUserID + " saved')){window.location.href='UserMaintEntry.aspx'}</script>";
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
            if (CheckValid())
            {
                String strUserID = Convert.ToString(txtUserID.Text.Trim());
                String strUserName = Convert.ToString(txtUserName.Text.Trim());
                String strPassword = Convert.ToString(txtUserPassword.Text.Trim());
                String strRoleCode = Convert.ToString(ddRole.SelectedValue.Trim());
                String tempUserID = Convert.ToString(lblTmpUserId.Text.Trim());
                String strCurUser = Convert.ToString(Session["SessUserId"]);
                
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.updateUser(strUserID, strUserName, strPassword, strRoleCode, tempUserID, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to update User ID: " + tempUserID + " .");
                    }
                    else
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> edited UID['" + tempUserID + "'] to UID['" + strUserID + "'] Name['" + strUserName + "'] Password['" + strPassword + "'] Role['" + strRoleCode + "']");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('User ID: " + tempUserID + " updated to " + strUserID + "')){window.location.href='UserMaintEntry.aspx'}</script>";
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
            Response.Redirect("UserMaintEntry.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
