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
        Session["SessTempUserId"] = "";

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
                NewPageIndex = 0;
                //SearchUserList();
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

    #region BindGridView
    private bool BindGridView(DataTable dtUserList)
    {
        try
        {
            DataView dvUserList = new DataView(dtUserList);

            gvUserList.DataSource = dvUserList;
            gvUserList.PageIndex = NewPageIndex;
            gvUserList.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region SearchUserList
    private void SearchUserList()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String UserID = Convert.ToString(txtUserID.Text);
            String UserName = Convert.ToString(txtUserName.Text);
            String Role = Convert.ToString(ddRole.SelectedValue).Trim();

            dsSearch = csDatabase.searchUser(UserID, UserName, Role);
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
            txtUserID.Text = "";
            txtUserName.Text = "";
            ddRole.SelectedIndex = 0;
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
        String strUserID = "";
        String strUserName = "";
        String strRole = "";

        if (Convert.ToString(txtUserID.Text).Trim() != "") strUserID = GlobalFunc.getReplaceToUrl(Convert.ToString(txtUserID.Text));
        if (Convert.ToString(txtUserName.Text).Trim() != "") strUserName = GlobalFunc.getReplaceToUrl(Convert.ToString(txtUserName.Text));
        if (Convert.ToString(ddRole.SelectedValue).Trim() != "") strRole = GlobalFunc.getReplaceToUrl(Convert.ToString(ddRole.SelectedValue));

        String strRedirect = "UserMaintEntryReg.aspx?";

        strRedirect = strRedirect + "uid=" + strUserID + "&";
        strRedirect = strRedirect + "un=" + strUserName + "&";
        strRedirect = strRedirect + "role=" + strRole + "&";

        return strRedirect;
    }
    #endregion

    #endregion

    #region Events

    #region btnNewUser
    protected void btnNewUser_Click(object sender, EventArgs e)
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
            SearchUserList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvUserList_RowDataBound
    protected void gvUserList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.Cells[7].Controls[0];
                lb.OnClientClick = "return confirm('Confirm delete?');";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvUserList_RowCommand
    protected void gvUserList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (Convert.ToString(e.CommandArgument) == "First") return;
            if (Convert.ToString(e.CommandArgument) == "Last") return;
            Int32 index = Convert.ToInt32(e.CommandArgument);            

            if (e.CommandName == "EditRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                Session["SessTempUserId"] = Convert.ToString(selectedRow.Cells[0].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit UID['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Password['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Role['" + Convert.ToString(selectedRow.Cells[3].Text) + "']");
                Response.Redirect(GetRedirectString());
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String userID = Convert.ToString(selectedRow.Cells[0].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete UID['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Password['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Role['" + Convert.ToString(selectedRow.Cells[3].Text) + "']");
                Boolean blDelUser = csDatabase.deleteUser(Convert.ToString(userID));
                if (blDelUser)
                {
                    GlobalFunc.ShowMessage("User ID: " + userID + " deleted.");
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted UID['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Name['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Password['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] Role['" + Convert.ToString(selectedRow.Cells[3].Text) + "']");
                }
                SearchUserList();
            }
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
            SearchUserList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
