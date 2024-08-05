using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DpsMaint_GearUpLM : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["SessTempPartId"] = "";

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

    #region Get Redirect Pass-Out
    private String GetRedirectString()
    {
        String strGearID = "";
        String strPartID = "";
        String strPlcNo = "";
        String strProcName = "";
        String strLine = "";
        String strGwNo = "";
        String strModAddr = "";
        String strPhysAddr = "";

        if (Convert.ToString(txtPartId.Text).Trim() != "") strPartID = GlobalFunc.getReplaceToUrl(Convert.ToString(txtPartId.Text));
        if (Convert.ToString(ddProcName.SelectedValue).Trim() != "") strPlcNo = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedValue));
        if (Convert.ToString(ddProcName.SelectedItem).Trim() != "") strProcName = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedItem));
        if (Convert.ToString(ddLineType.SelectedValue).Trim() != "") strLine = GlobalFunc.getReplaceToUrl(Convert.ToString(ddLineType.SelectedValue));
        if (Convert.ToString(txt_ModuleAddr.Text).Trim() != "") strModAddr = GlobalFunc.getReplaceToUrl(Convert.ToString(txt_ModuleAddr.Text));
        if (Convert.ToString(txt_PhysAddr.Text).Trim() != "") strPhysAddr = GlobalFunc.getReplaceToUrl(Convert.ToString(txt_PhysAddr.Text));
        if (Convert.ToString(txt_GwNo.Text).Trim() != "") strGwNo = GlobalFunc.getReplaceToUrl(Convert.ToString(txt_GwNo.Text));
        if (Convert.ToString(txt_GearUpID.Text).Trim() != "") strGearID = GlobalFunc.getReplaceToUrl(Convert.ToString(txt_GearUpID.Text));

        String strRedirect = "GearUpLMReg.aspx?";
        strRedirect = strRedirect + "gid=" + strGearID + "&";
        strRedirect = strRedirect + "pid=" + strPartID + "&";
        strRedirect = strRedirect + "pno=" + strPlcNo + "&";
        strRedirect = strRedirect + "pnm=" + strProcName + "&";
        strRedirect = strRedirect + "line=" + strLine + "&";
        strRedirect = strRedirect + "gwn=" + strGwNo + "&";
        strRedirect = strRedirect + "mad=" + strModAddr + "&";
        strRedirect = strRedirect + "pad=" + strPhysAddr + "&";
        return strRedirect;



    }
    #endregion

    #region ClearAll
    private void ClearAll()
    {
        try
        {
            txt_GwNo.Text = "";
            txt_ModuleAddr.Text = "";
            txt_PhysAddr.Text = "";
            txt_GearUpID.Text = "";
            //txt_Line.Text = "";
            ddLineType.SelectedIndex = 0;
            txtPartId.Text = "";
            ddProcName.SelectedIndex = 0;
            SearchGearUpLm();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region SearchGearUpLm
    private void SearchGearUpLm()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            lblMsg.Text = "";
            lblMsg.Visible = false;

            String strPlcNo = Convert.ToString(ddProcName.SelectedValue).Trim();
            String strProcName = Convert.ToString(ddProcName.SelectedItem).Trim();
            String strPartID = Convert.ToString(txtPartId.Text).Trim();
            String strLine = Convert.ToString(ddLineType.SelectedValue).Trim();
            String strGwNo = Convert.ToString(txt_GwNo.Text).Trim();
            String strModAddr = Convert.ToString(txt_ModuleAddr.Text).Trim();
            String strPhysAddr = Convert.ToString(txt_PhysAddr.Text).Trim();
            String strGearID = Convert.ToString(txt_GearUpID.Text).Trim();

            if (isIntergerValue())
            {
                dsSearch = csDatabase.SrcGearUpLM(strPlcNo, strPartID, strProcName, strLine, strGwNo, strModAddr, strPhysAddr, strGearID);
                if (dsSearch != null && dsSearch.Tables.Count > 0 && dsSearch.Tables[0].Rows.Count > 0)
                {
                    dtSearch = dsSearch.Tables[0];
                    BindGridView(dtSearch);
                }
                else
                {
                    DataTable dtSearch2 = new DataTable();
                    BindGridView(dtSearch2);
                }
            }
            else
            {
                return;
            }

        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    public bool isIntergerValue()
    {
        String strModAddr = Convert.ToString(txt_ModuleAddr.Text).Trim();
        String strPhysAddr = Convert.ToString(txt_PhysAddr.Text).Trim();
        String strGearID = Convert.ToString(txt_GearUpID.Text).Trim();

        if (strGearID != "" && !GlobalFunc.IsTextAValidInteger(strGearID))
        {
            lblMsg.Text = "Gear Up ID must be Integer. Please check.";
            lblMsg.Visible = true;
            return false;
        }

        if (strModAddr != "" && !GlobalFunc.IsTextAValidInteger(strModAddr))
        {
            lblMsg.Text = "Lamp Module Address must be Integer. Please check.";
            lblMsg.Visible = true;
            return false;
        }

        if (strPhysAddr != "" && !GlobalFunc.IsTextAValidInteger(strPhysAddr))
        {
            lblMsg.Text = "Lamp Physical Address must be Integer. Please check.";
            lblMsg.Visible = true;
            return false;
        }

        return true;
    }
    #endregion

    #region BindGridView
    private bool BindGridView(DataTable dtGearUp)
    {
        try
        {
            DataView dvGearUp = new DataView(dtGearUp);

            gvGearUp.DataSource = dvGearUp;
            gvGearUp.PageIndex = NewPageIndex;
            gvGearUp.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion


    #region Events

    #region btnNewGroup
    protected void btnNewGroup_Click(object sender, EventArgs e)
    {
        try
        {
            if (isIntergerValue())
            {
                Response.Redirect(GetRedirectString());
            }
            else
            {
                return;
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

    #region BtnSearch
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            NewPageIndex = 0;
            SearchGearUpLm();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvGearUp_RowDataBound
    protected void gvGearUp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.Cells[GEARUPMST_COLUMN.Delete].Controls[0];
                lb.OnClientClick = "return confirm('Confirm delete?');";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvGearUp_RowCommand
    protected void gvGearUp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 index = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                Session["SessTempGearID"] = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Gear_ID].Text);
                Session["SessTempPartId"] = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Part_ID].Text);
                Session["SessTempPlcNo"] = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.PLC_No].Text);
                Session["SessTempProcName"] = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Process_Name].Text);
                Session["SessTempLine"] = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Line].Text);
                Session["SessTempLineGwNo"] = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.GWNo].Text);

                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit Gear Up ID ['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Gear_ID].Text) + "'] Part ID['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Part_ID].Text) + "'] GwNo['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.GWNo].Text) + "'] Part ID['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Part_ID].Text) + "'] Line['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Line].Text) + "'] Lamp Module Address['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.LmModAddr].Text) + "'] Lamp Physical Address['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.LmPhysAddr].Text) + "']");
                Response.Redirect(GetRedirectString());
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String tmpPartID = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Part_ID].Text);
                String tmpLine = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Line].Text);
                String tmpModAddr = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.LmModAddr].Text);
                String tmpPhysAddr = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.LmPhysAddr].Text);
                String tempGwNo = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.GWNo].Text);
                String tempGearUpID = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Gear_ID].Text);
                String tempPlcNo = Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.PLC_No].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete Gear Up ID ['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Gear_ID].Text) + "'] Plc No['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.PLC_No].Text) + "'] GwNo['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.GWNo].Text) + "'] Part ID['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Part_ID].Text) + "'] Line['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Line].Text) + "'] Lamp Module Address['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.LmModAddr].Text) + "'] Lamp Physical Address['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.LmPhysAddr].Text) + "']");
                Boolean blDeleteGroup = csDatabase.DelGearUpLM(tmpPartID, tempPlcNo, tempGwNo, tempGearUpID, tmpLine);
                if (blDeleteGroup)
                {
                    //csDatabase.DelGroupMstRem(tmpPlcNo, tmpProcName, tmpGroupID, tmpGroupName);
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted Gear Up ID ['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Gear_ID].Text) + "'] Plc No['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.PLC_No].Text) + "'] GwNo['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.GWNo].Text) + "'] Part ID['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Part_ID].Text) + "'] Line['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Line].Text) + "'] Lamp Module Address['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.LmModAddr].Text) + "'] Lamp Physical Address['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.LmPhysAddr].Text) + "']");
                    GlobalFunc.ShowMessage("Gear Up Gear Up ID: " + tempGearUpID + " deleted.");
                }
                else
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to delete Gear Up ID ['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Gear_ID].Text) + "'] Plc No['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.PLC_No].Text) + "'] GwNo['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.GWNo].Text) + "'] Part ID['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Part_ID].Text) + "'] Line['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.Line].Text) + "'] Lamp Module Address['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.LmModAddr].Text) + "'] Lamp Physical Address['"
                    + Convert.ToString(selectedRow.Cells[GEARUPMST_COLUMN.LmPhysAddr].Text) + "']");
                    GlobalFunc.ShowErrorMessage("Unable to delete Gear Up ID: " + tempGearUpID + ".");
                }
                SearchGearUpLm();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvGearUp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchGearUpLm();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion

}

public static class GEARUPMST_COLUMN
{
    public static readonly int Gear_ID = 0;
    public static readonly int PLC_No = 1;
    public static readonly int Process_Name = 2;
    public static readonly int Part_ID = 3;
    public static readonly int Line = 4;
    public static readonly int GWNo = 5;
    public static readonly int LmModAddr = 6;
    public static readonly int LmPhysAddr = 7;
    public static readonly int Last_Updated_By = 8;
    public static readonly int Last_Updated_DateTime = 9;
    public static readonly int Edit = 10;
    public static readonly int Delete = 11;
}