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
        Session["SessTempGroupId"] = "";

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
                getLineType();
                NewPageIndex = 0;
                //SearchGroupMst();
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
    }
    #endregion

    #region Method

    #region getLineType
    private void getLineType()
    {
        try
        {
            ddLineType.DataSource = GlobalFunc.getLineType();
            ddLineType.DataTextField = "Description";
            ddLineType.DataValueField = "Description";
            ddLineType.DataBind();
            ddLineType.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

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

    #region SearchGroupMst
    private void SearchGroupMst()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strGroupID = Convert.ToString(txtGroupId.Text).Trim();
            String strGroupName = Convert.ToString(txtGroupName.Text).Trim();
            String strPlcNo = Convert.ToString(ddProcName.SelectedValue).Trim();
            String strProcName = Convert.ToString(ddProcName.SelectedItem).Trim();
            String strGroupLine = Convert.ToString(ddLineType.SelectedValue).Trim();

            dsSearch = csDatabase.SrcGroupMst(strGroupID, strGroupName, strPlcNo, strProcName, strGroupLine);
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
    private bool BindGridView(DataTable dtGroupMst)
    {
        try
        {
            DataView dvGroupMst = new DataView(dtGroupMst);

            gvGroupMst.DataSource = dvGroupMst;
            gvGroupMst.PageIndex = NewPageIndex;
            gvGroupMst.DataBind();
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
            txtGroupId.Text = "";
            txtGroupName.Text = "";
            //txtGroupLine.Text = "";
            ddLineType.SelectedIndex = 0;
            ddProcName.SelectedIndex = 0;
            SearchGroupMst();
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
        String strGroupID = "";
        String strGroupName = "";
        String strPlcNo = "";
        String strProcName = "";
        String strGroupLine = "";

        if (Convert.ToString(txtGroupId.Text).Trim() != "") strGroupID = GlobalFunc.getReplaceToUrl(Convert.ToString(txtGroupId.Text));
        if (Convert.ToString(txtGroupName.Text).Trim() != "") strGroupName = GlobalFunc.getReplaceToUrl(Convert.ToString(txtGroupName.Text));
        if (Convert.ToString(ddProcName.SelectedValue).Trim() != "") strPlcNo = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedValue));
        if (Convert.ToString(ddProcName.SelectedItem).Trim() != "") strProcName = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedItem));
        //if (Convert.ToString(txtGroupLine.Text).Trim() != "") strGroupLine = GlobalFunc.getReplaceToUrl(Convert.ToString(txtGroupLine.Text));
        if (Convert.ToString(ddLineType.SelectedValue).Trim() != "") strGroupLine = GlobalFunc.getReplaceToUrl(Convert.ToString(ddLineType.SelectedValue));

        String strRedirect = "GroupMstReg.aspx?";

        strRedirect = strRedirect + "gid=" + strGroupID + "&";
        strRedirect = strRedirect + "gnm=" + strGroupName + "&";
        strRedirect = strRedirect + "pno=" + strPlcNo + "&";
        strRedirect = strRedirect + "pnm=" + strProcName + "&";
        strRedirect = strRedirect + "gln=" + strGroupLine + "&";

        return strRedirect;
    }
    #endregion

    #endregion

    #region Events

    #region btnNewGroup
    protected void btnNewGroup_Click(object sender, EventArgs e)
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
            SearchGroupMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvGroupMst_RowDataBound
    protected void gvGroupMst_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.Cells[GROUPMST_COLUMN.Delete].Controls[0];
                lb.OnClientClick = "return confirm('Confirm delete? ATTENTION : THIS WILL DELETE ALL BLOCK MASTER, RACK MASTER DATA REGISTERED UNDER CURRENT GROUP!');";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvGroupMst_RowCommand
    protected void gvGroupMst_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 index = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                Session["SessTempGroupId"] = Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_ID].Text);
                Session["SessTempPlcNo"] = Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.PLC_No].Text);
                Session["SessTempProcName"] = Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Process_Name].Text);
                Session["SessTempGroupLine"] = Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_Line].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit Group ID['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_ID].Text) + "'] Group Name['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_Name].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.PLC_No].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Process_Name].Text) + "'] Group Line['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_Line].Text) + "']");
                Response.Redirect(GetRedirectString());
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String tmpGroupID = Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_ID].Text);
                String tmpGroupName = Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_Name].Text);
                String tmpPlcNo = Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.PLC_No].Text);
                String tmpProcName = Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Process_Name].Text);
                String tempGroupLine = Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_Line].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete Group ID['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_ID].Text) + "'] Group Name['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_Name].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.PLC_No].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Process_Name].Text) + "'] Group Line['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_Line].Text) + "']");
                Boolean blDeleteGroup = csDatabase.DelGroupMst(tmpGroupID, tmpPlcNo, tmpProcName, tmpGroupName, tempGroupLine);
                if (blDeleteGroup)
                {
                    //csDatabase.DelGroupMstRem(tmpPlcNo, tmpProcName, tmpGroupID, tmpGroupName);
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted Group ID['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_ID].Text) + "'] Group Name['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_Name].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.PLC_No].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Process_Name].Text) + "']");
                    GlobalFunc.ShowMessage("Group Master ID: " + tmpGroupID + " deleted.");
                }
                else
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to deleted Group ID['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_ID].Text) + "'] Group Name['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Group_Name].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.PLC_No].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[GROUPMST_COLUMN.Process_Name].Text) + "']");
                    GlobalFunc.ShowErrorMessage("Unable to delete Group Master ID: " + tmpGroupID + ".");
                }
                SearchGroupMst();
            }
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
            SearchGroupMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion


}

public static class GROUPMST_COLUMN
{
    public static readonly int Group_ID = 0;
    public static readonly int Group_Name = 1;
    public static readonly int Group_Line = 2;
    public static readonly int PLC_No = 3;
    public static readonly int Process_Name = 4;
    public static readonly int Last_Updated_By = 5;
    public static readonly int Last_Updated_DateTime = 6;
    public static readonly int Edit = 7;
    public static readonly int Delete = 8;
}
