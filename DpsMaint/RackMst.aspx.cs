using System;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using dpant;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;

public partial class RackMst : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["SessTempRackName"] = "";
        Session["SessAisName"] = "";
        Session["SessAisType"] = "";

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
                getBlockName();
                getPlcModel();          //Added by YanTeng 15/09/2020
                NewPageIndex = 0;
                //SearchRackMst();
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

    #region getBlockName
    private void getBlockName()
    {
        try
        {
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            String strGroupName = Convert.ToString(ddGroupName.SelectedValue);

            ddBlockName.DataSource = GlobalFunc.getBlockName(strProcName, strGroupName);
            ddBlockName.DataTextField = "Description";
            ddBlockName.DataValueField = "Description";
            ddBlockName.DataBind();
            ddBlockName.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getPlcModel
    private void getPlcModel()              //Added by YanTeng 15/09/2020
    {
        try
        {
            ddPlcModel.DataSource = GlobalFunc.getAllPlcModel();
            ddPlcModel.DataTextField = "Code";
            ddPlcModel.DataValueField = "Value";
            ddPlcModel.DataBind();
            ddPlcModel.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region SearchRackMst
    private void SearchRackMst()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String RackName = Convert.ToString(txtRackName.Text).Trim();
            String ProcName = Convert.ToString(ddProcName.SelectedItem).Trim();
            String PlcNo = Convert.ToString(ddProcName.SelectedValue).Trim();
            String GroupName = Convert.ToString(ddGroupName.SelectedValue).Trim();
            String BlockName = Convert.ToString(ddBlockName.SelectedValue).Trim();

            dsSearch = csDatabase.SrcRackMst(RackName, PlcNo, ProcName, GroupName, BlockName);
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
    private bool BindGridView(DataTable dtRack)
    {
        try
        {
            DataView dvRack = new DataView(dtRack);

            gvRack.DataSource = dvRack;
            gvRack.PageIndex = NewPageIndex;
            gvRack.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    private bool BindGridErrorlog(DataTable dtRacklog)
    {
        try
        {
            DataView dvRack = new DataView(dtRacklog);
            gvErrorLog.DataSource = dvRack;
            gvErrorLog.PageIndex = NewPageIndex;
            gvErrorLog.DataBind();

            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public void Errorlogformatting(object sender, GridViewRowEventArgs e)
    {
        
        string status = Convert.ToString(e.Row.Cells[12].Text);

        //string data_errorlog = Convert.ToString(e.Row.Cells[13].Text);

        //data_errorlog = data_errorlog.Replace(",", "<br/>");

        //e.Row.Cells[13].Text = data_errorlog;

        foreach (TableCell cell in e.Row.Cells)
        {
        
            if (status == "NOT OK")
            {
                cell.BackColor = Color.Red;
                cell.ForeColor = Color.White;
            } 
        }
 

    }


    #endregion

    #region ClearAll
    private void ClearAll()
    {
        try
        {
            getPlcModel();

            ddProcName.SelectedIndex = 0;
            ddBlockName.SelectedIndex = 0;
            ddGroupName.SelectedIndex = 0;
            ddPlcModel.SelectedIndex = 0;           //Added by YanTeng 17/09/2020
            txtRackName.Text = "";
            SearchRackMst();

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
        String RackName = "";
        String ProcName = "";
        String PlcNo = "";
        String GroupName = "";
        String BlockName = "";

        if (Convert.ToString(txtRackName.Text).Trim() != "") RackName = GlobalFunc.getReplaceToUrl(Convert.ToString(txtRackName.Text));
        if (Convert.ToString(ddProcName.SelectedItem).Trim() != "") ProcName = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedItem));
        if (Convert.ToString(ddProcName.SelectedValue).Trim() != "") PlcNo = GlobalFunc.getReplaceToUrl(Convert.ToString(ddProcName.SelectedValue));
        if (Convert.ToString(ddGroupName.SelectedValue).Trim() != "") GroupName = GlobalFunc.getReplaceToUrl(Convert.ToString(ddGroupName.SelectedValue));
        if (Convert.ToString(ddBlockName.SelectedValue).Trim() != "") BlockName = GlobalFunc.getReplaceToUrl(Convert.ToString(ddBlockName.SelectedValue));

        String strRedirect = "RackMstReg.aspx?";

        strRedirect = strRedirect + "rnm=" + RackName + "&";
        strRedirect = strRedirect + "pnm=" + ProcName + "&";
        strRedirect = strRedirect + "pno=" + PlcNo + "&";
        strRedirect = strRedirect + "gnm=" + GroupName + "&";
        strRedirect = strRedirect + "bnm=" + BlockName + "&";

        return strRedirect;
    }
    #endregion

    #endregion

    #region Events

    #region btnNewRack
    protected void btnNewRack_Click(object sender, EventArgs e)
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
            //ClearAll();
            Response.Redirect("Rackmst.aspx");
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
            SearchRackMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvRack_RowDataBound
    protected void gvRack_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        try
        {

 

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = (LinkButton)e.Row.Cells[11].Controls[0];
                lb.OnClientClick = "return confirm('Confirm delete?');";
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region gvRack_RowCommand
    protected void gvRack_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 index = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                Session["SessTempRackName"] = Convert.ToString(selectedRow.Cells[0].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit Rack Name['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Collumn['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Row['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Group Name['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Block Name['" + Convert.ToString(selectedRow.Cells[6].Text) + "']");
                Response.Redirect(GetRedirectString());
            }
            if (e.CommandName == "EditPartsRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                Session["SessTempRackName"] = Convert.ToString(selectedRow.Cells[0].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to edit Rack Details for Rack Name['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Collumn['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Row['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Group Name['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Block Name['" + Convert.ToString(selectedRow.Cells[6].Text) + "']");
                Response.Write("<script>window.open('RackMstDet.aspx')</script>");
            }
            if (e.CommandName == "DeleteRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                String strRackName = Convert.ToString(selectedRow.Cells[0].Text);
                String strPlcNo = Convert.ToString(selectedRow.Cells[3].Text);
                String strProcName = Convert.ToString(selectedRow.Cells[4].Text);
                String strBlockName = Convert.ToString(selectedRow.Cells[6].Text);
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete Rack Name['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Collumn['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Row['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Group Name['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Block Name['" + Convert.ToString(selectedRow.Cells[6].Text) + "']");
                Boolean blDelRack = csDatabase.DelRackMst(strRackName, strPlcNo, strProcName);
                if (!blDelRack)
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to delete Rack Name['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Collumn['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Row['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Group Name['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Block Name['" + Convert.ToString(selectedRow.Cells[6].Text) + "']");
                    GlobalFunc.ShowErrorMessage("Unable to delete Rack Name: " + strRackName + ".");
                    //Response.Write("<script language='javascript'>alert('Unable to delete Rack Name: " + strRackName + ".')</script>");
                }
                else
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted Rack Name['" + Convert.ToString(selectedRow.Cells[0].Text) + "'] Collumn['" + Convert.ToString(selectedRow.Cells[1].Text) + "'] Row['" + Convert.ToString(selectedRow.Cells[2].Text) + "'] PLC No['" + Convert.ToString(selectedRow.Cells[3].Text) + "'] Process Name['" + Convert.ToString(selectedRow.Cells[4].Text) + "'] Group Name['" + Convert.ToString(selectedRow.Cells[5].Text) + "'] Block Name['" + Convert.ToString(selectedRow.Cells[6].Text) + "']");
                    GlobalFunc.ShowMessage("Rack Name: " + strRackName + " deleted.");
                    //ClientScriptManager CSM = Page.ClientScript;
                    //string strconfirm = "<script>if(!window.alert('Rack Name: " + strRackName + " deleted.'))</script>";
                    //CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
                SearchRackMst();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvRack_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchRackMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region On Drop Down List Selected Index Change
    protected void ddProcName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddProcName.SelectedItem.Text.Trim()))
        {
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            ddPlcModel.DataSource = GlobalFunc.getPlcModelbyProcName(strProcName);
            ddPlcModel.DataTextField = "Description";
            ddPlcModel.DataValueField = "Value";
            ddPlcModel.DataBind();
        }
        else
        {
            getPlcModel();
        }

        getGroupName();
        getBlockName();
    }

    protected void ddGroupName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getBlockName();
    }
    #endregion

    #endregion

    #region btnExport
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            #region Check Searching Criteria Selected
            if (ddProcName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Process Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddGroupName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Group Name.";
                lblMsg.Visible = true;
                return;
            }
            //else if (ddBlockName.SelectedIndex == 0)
            //{
            //    lblMsg.Text = "Please select Block Name.";
            //    lblMsg.Visible = true;
            //    return;
            //}
            //else if txtRackName.Text = ""
            //{
            //    lblMsg.Text = "Please select Rack Name.";
            //    lblMsg.Visible = true;
            //    return;
            //}
            #endregion

            String strPlcNo = Convert.ToString(ddProcName.SelectedValue).Trim();
            String strProcName = Convert.ToString(ddProcName.SelectedItem).Trim();
            String strGroupName = Convert.ToString(ddGroupName.SelectedValue).Trim();
            String strBlockName = Convert.ToString(ddBlockName.SelectedValue).Trim();
            String strRackName = Convert.ToString(txtRackName.Text).Trim();

            Response.Write("<script>window.open('PrtRackmasterExp.aspx?" +
                        "plc_no=" + GlobalFunc.getReplaceToUrl(strPlcNo) +
                        "&proc_name=" + GlobalFunc.getReplaceToUrl(strProcName) +
                        "&group_name=" + GlobalFunc.getReplaceToUrl(strGroupName) +
                        "&block_name=" + GlobalFunc.getReplaceToUrl(strBlockName) +
                        "&rack_name=" + GlobalFunc.getReplaceToUrl(strRackName) +
                        "&flag=export');</script>");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (IsPostBack == true)
        {
            string[] validFileTypes = { "xls" };

            string ext = System.IO.Path.GetExtension(btnBrowse.PostedFile.FileName);

            bool isValidFile = false;

            for (int i = 0; i < validFileTypes.Length; i++)
            {

                if (ext == "." + validFileTypes[i])
                {

                    isValidFile = true;

                    break;

                }

            }


            if (!isValidFile)
            {

                lblUploadStatus.ForeColor = System.Drawing.Color.Red;
                lblUploadStatus.Text = "Invalid File. Please upload a File with extension " +

                               string.Join(",", validFileTypes);

            }

            else
            {

                lblUploadStatus.ForeColor = System.Drawing.Color.Green;

                lblUploadStatus.Text = "Processing Excel File...";
                string folderpath = ConfigurationManager.AppSettings["ExcelPath"];
                string fileName = Server.MapPath(folderpath + btnBrowse.FileName);
                btnBrowse.SaveAs(fileName);
                try
                {
                    importExcel();
                    String dir = Server.MapPath(folderpath + btnBrowse.FileName);
                    FileInfo file = new FileInfo(dir);
                    file.Delete();
                }
                catch (Exception ex)
                {
                    lblUploadStatus.Text = "Excel file being used by another person. Please closed it ";

                }
 
            }
        }
    }

    private void importExcel()
    {
        //DataTable dtBAgentExcel = null;
        DataTable dtExcelCheck = new DataTable();

        DataTable dsRackmstListTb = new DataTable();

        DataTable dsRackmstTb = new DataTable();

        dsRackmstTb.Columns.Add("User_id", typeof(string));

        dsRackmstTb.Columns.Add("plc_no", typeof(Int32));
        dsRackmstTb.Columns.Add("rack_name", typeof(string));
        dsRackmstTb.Columns.Add("group_name", typeof(string));
        dsRackmstTb.Columns.Add("row", typeof(Int32));
        dsRackmstTb.Columns.Add("proc_name", typeof(string));
        dsRackmstTb.Columns.Add("column", typeof(Int32));
        dsRackmstTb.Columns.Add("block", typeof(string));
        dsRackmstTb.Columns.Add("AisData", typeof(string));
        dsRackmstTb.Columns.Add("PartColor", typeof(string));
        dsRackmstTb.Columns.Add("Module", typeof(string));
        dsRackmstTb.Columns.Add("actcol", typeof(Int32));
        dsRackmstTb.Columns.Add("actrow", typeof(Int32));
        dsRackmstTb.Columns.Add("Result", typeof(string));
        dsRackmstTb.Columns.Add("Error_log", typeof(string));
        dsRackmstTb.Columns.Add("upload_date", typeof(DateTime));

        try
        {
            string folderpath = ConfigurationManager.AppSettings["ExcelPath"];
            String path = Server.MapPath(folderpath + btnBrowse.FileName);
            OleDbConnection strConn = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path + "';Extended Properties='Excel 8.0;IMEX=1;HDR=NO'");
            //OleDbConnection strConn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties=Excel 12.0 Xml;HDR=YES;IMEX=1");

            //OleDbDataAdapter dsExcelCheck = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
            //dsExcelCheck.TableMappings.Add("Table", "ExcelTable");
            //dsExcelCheck.Fill(dtExcelCheck);

            OleDbDataAdapter dsRackmstList = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
            dsRackmstList.TableMappings.Add("Table", "dsRackmstListTb");
            dsRackmstList.Fill(dsRackmstListTb);


            //int startrow = 2;
            int plcrow = 2;
            int grprow = 3;
            int procrow = 4;
            int blkrow = 5;
            int RackdatarowStarting = 7;

            int datacolumn1Hdg = 4;
            int datacolumn2Hdg = 12;

            int maxRack = 20;
            int maxColumn = 10;
            int maxRowperRack = 5;

            DataRow dr;

            int nxCol = 0;
            string[] rack1 = new string[20];

            string[] detailrecordrackchecking = new string[20];
            int detailrackcnt = 0;

            string Temp_Rack = "";
            int rack_cnt = 0;

            for (int d = 0; d <= maxRack - 1; d++)
            {
 
                try
                {
                    Temp_Rack = Convert.ToString(dsRackmstListTb.Rows[plcrow].ItemArray[datacolumn2Hdg]);

                }
                catch (Exception ex3)
                {
                    Temp_Rack = "";

                }
                if (Temp_Rack != "")
                {
                    rack1[rack_cnt] = Temp_Rack;
                    rack_cnt = rack_cnt + 1;
                }

                plcrow = plcrow + 22;

            }

            string old_rack = "";
            string new_rack = "";
            string strrackcriteria1 = "";
            string strrackcriteria2 = "";
            string strrackcriteria3 = "";

            if (rack1[0] != "" && rack1[0] != null)
            {
                strrackcriteria1 = strrackcriteria1 + "rack_name = '" + rack1[0] + "' ";
                strrackcriteria2 = strrackcriteria2 + "substring(rack_det_id,1, CHARINDEX('^',rack_det_id)-1)  = '" + rack1[0] + "'";

                strrackcriteria3 = strrackcriteria3 + "AND (substring(rack_det_id,1, CHARINDEX('^',rack_det_id)-1)  != '" + rack1[0] + "'";

            
            }


            for (int z = 1; z < rack1.Length; z++)
            {

                if (rack1[z] != "" && rack1[z] != null)
                {
                    strrackcriteria1 = strrackcriteria1 + " OR rack_name = '" + rack1[z] + "'";
                    strrackcriteria2 = strrackcriteria2 + " OR substring(rack_det_id,1, CHARINDEX('^',rack_det_id)-1)  = '" + rack1[z] + "'";

                    strrackcriteria3 = strrackcriteria3 + " AND substring(rack_det_id,1, CHARINDEX('^',rack_det_id)-1)  != '" + rack1[z] + "'";
                
                }

            }
            plcrow = 2;

            strrackcriteria3 = strrackcriteria3 + ")";

            for (int i = 0; i <= maxRack - 1; i++)
            {
                string strGrpname = "";
                Int32 strplc_no = 0;
                string strRack_name = "";
                string strproc_name = "";
                Int32 strRow = 0;
                Int32 strcolumn = 0;
                string strblock_name = "";

                int col = 0;
                int row = 0;


                try
                {
                    strGrpname = dsRackmstListTb.Rows[grprow].ItemArray[datacolumn1Hdg].ToString();
                }
                catch (Exception ex1)
                {
                    strGrpname = "";
                }


                try
                {
                    strplc_no = Convert.ToInt32(dsRackmstListTb.Rows[plcrow].ItemArray[datacolumn1Hdg]);

                }
                catch (Exception ex2)
                {
                    strplc_no = 0;

                }

                try
                {
                    strRack_name = Convert.ToString(dsRackmstListTb.Rows[plcrow].ItemArray[datacolumn2Hdg]);

                }
                catch (Exception ex3)
                {
                    strRack_name = "";

                }

                try
                {
                    strproc_name = Convert.ToString(dsRackmstListTb.Rows[procrow].ItemArray[datacolumn1Hdg]);

                }
                catch (Exception ex4)
                {
                    strproc_name = "";

                }

                try
                {
                    strRow = Convert.ToInt32(dsRackmstListTb.Rows[grprow].ItemArray[datacolumn2Hdg]);

                }
                catch (Exception ex5)
                {
                    strRow = 0;

                }

                try
                {
                    strcolumn = Convert.ToInt32(dsRackmstListTb.Rows[procrow].ItemArray[datacolumn2Hdg]);

                }
                catch (Exception ex6)
                {
                    strcolumn = 0;

                }

                try
                {
                    strblock_name = Convert.ToString(dsRackmstListTb.Rows[blkrow].ItemArray[datacolumn1Hdg]);

                }
                catch (Exception ex7)
                {
                    strblock_name = "";

                }

                if (strGrpname != "")
                {

                    string errmsg1 = "";
                    string result = "";


                    string strvalidrecord = csDatabase.validaterackmstExport(strRack_name, strplc_no, strproc_name, strGrpname, strblock_name);

                    if (strvalidrecord.Trim() == "")
                    {

                        errmsg1 = errmsg1 + "This entire record is not exist in rack master. Please check.<br/>";

                    }


                    if (i > 0)
                    {
                        for (int z = 0; z < rack1.Length; z++)
                        {
                            if (strRack_name == detailrecordrackchecking[z])
                            {
                                errmsg1 = "Rack Name duplicate,";
                            }
                        }

 

                        DataRow[] group_name = dsRackmstTb.Select("group_name = '" + strGrpname.ToUpper() + "'");
                        if (group_name.Length == 0)
                        {
                            errmsg1 = errmsg1 + ", Group Name should not be different <br/>";
                        }

                        DataRow[] plc_no = dsRackmstTb.Select("plc_no = '" + strplc_no + "'");
                        if (plc_no.Length == 0)
                        {
                            errmsg1 = errmsg1 + ", plc_no should not be different <br/>";
                        }

                    }



                    if (strRow > 5)
                    {
                        errmsg1 = errmsg1 + ", Row should not be more than 5 <br/> ";
                   
                    }

                    if (strcolumn > 10)
                    {
                        errmsg1 = errmsg1 + ", Column should not be more than 10 <br/> ";

                    }


                    for (int j = 0; j <= maxRowperRack - 1; j++)
                    {
                        string errmsg = "";

                        int nxcolCnt = 1;

                        row = row + 1;

                        for (int k = 0; k <= maxColumn - 1; k++)
                        {
                            col = col + 1;


                            if (nxcolCnt > 1)
                            {
                                nxCol = ((nxcolCnt * 5) - 5);
                            }
                            else
                            {
                                col = 1;
                                nxCol = 0;
                            }

                            string strAisData = dsRackmstListTb.Rows[RackdatarowStarting].ItemArray[nxCol].ToString();
                            string strPartColor = dsRackmstListTb.Rows[RackdatarowStarting + 1].ItemArray[nxCol].ToString();
                            string strModule = dsRackmstListTb.Rows[RackdatarowStarting + 2].ItemArray[nxCol].ToString().Split('(')[0].Trim();

                            if ((strAisData != "Input AIS Data") && (strAisData != ""))
                            {


                                DataRow[] PartColor = dsRackmstTb.Select("PartColor = '" + strPartColor.ToUpper() + "'");
                                if (PartColor.Length != 0)
                                {
                                    errmsg = errmsg + ", Part/Color Duplicated <br/>";
                                }

                                DataRow[] Module = dsRackmstTb.Select("Module = '" + strModule.ToUpper() + "'");
                                if (Module.Length != 0)
                                {
                                    errmsg = errmsg + ", Module Duplicated <br/>";
                                }


                                if (row > strRow)
                                {
                                    errmsg = errmsg + ", AIS Data Row is exceeded the rack row <br/> ";
                               
                                }

                                if (col > strcolumn)
                                {
                                    errmsg = errmsg + ", AIS Data column is exceeded the rack column <br/>";

                                }

                                try
                                {
                                    string[] partno_colors = Convert.ToString(strPartColor).Split('-');

                                    string rm_partno = partno_colors[0] + '-' + partno_colors[1];
                                    string rm_color = partno_colors[2];


                                    // it should not exist in other rack, exist in same rack doesnt matter will remove and repopulate
                                    //
                                    //
                                    //  still pending ... dont know why return nothing
                                    //
                                    String strCurRackLoc = csDatabase.GetPartsRackLocExport(rm_partno, rm_color, strrackcriteria3);
                                    string rm_rackid = strRack_name.ToUpper() + "^" + row + "^" + col;


                                    if (strCurRackLoc != "")
                                    {
                                        String strRackLoc = "";
                                        String[] tmpRackMstDetIdVal = rm_rackid.Split('^');
                                        int tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
                                        if (tmpRackMstDetIdCnt > 1)
                                        {
                                            strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
                                        }
                                        if (strCurRackLoc != strRackLoc)
                                        {
                                            errmsg = errmsg + "This Part had already been selected in rack location : [" + strCurRackLoc + "-" + rm_partno + "-" + rm_color + "]. Please check. <br/>";
                                        }

                                    }
                                    else
                                    {
                                        if (!csDatabase.ChkAisPartsNumExist(rm_partno, rm_color)) // ***ace_20160407_001
                                        {
                                            errmsg = errmsg + " Harigami Parts No not exist in Parts No master data." + rm_partno + " " + rm_color + " <br/>";

                                        }

                                    }





                                }
                                catch (Exception ex)
                                {
                                    errmsg = errmsg + ",Part/Color data got problem <br/>";

                                }


 

                                try
                                {
                                    string[] strModule_temp = Convert.ToString(strModule).Split('-');

                                    string rm_module = strModule_temp[0];
                                    string rm_module_name = strModule_temp[1];

                                    // it should not exist in other rack, exist in same rack doesnt matter will remove and repopulate
                                    //
                                    //
                                    String strCurModuleloc = csDatabase.GetModuleLocExport(strproc_name, rm_module, strrackcriteria3);


                                    if (strCurModuleloc != "")
                                    {

                                        errmsg = errmsg + "This module address had already been selected in rack location." + strCurModuleloc + "  Please check.<br/>";

                                    }

                                    else
                                    {
                                        string strCurModuletype = csDatabase.GetModuletypeLocExport(strproc_name, rm_module);

                                        if (strCurModuletype.Trim() != rm_module_name.Trim())
                                        {

                                            errmsg = errmsg + "This module type invalid. Please check.<br/>";

                                        }
                                    }




                                }
                                catch (Exception ex)
                                {
                                    errmsg = errmsg + " Module data.got probem <br/> " ;
                                }






                                // --- end of pending ---

                                if (errmsg != "")
                                {
                                    result = "NOT OK";

                                }
                                else
                                {
                                    if (errmsg1 != "")
                                    {
                                        result = "NOT OK";
                                    }
                                    else
                                    {
                                        result = "OK";
                                    }


                                }
                                DateTime upload_date = DateTime.Now;

                                dr = dsRackmstTb.NewRow();
                                dr["User_id"] = Session["SessUserID"];
                                dr["plc_no"] = strplc_no + 0;
                                dr["rack_name"] = strRack_name.ToUpper();
                                dr["group_name"] = strGrpname.ToUpper();
                                dr["row"] = strRow + 0;
                                dr["proc_name"] = strproc_name.ToUpper();
                                dr["column"] = strcolumn + 0;
                                dr["block"] = strblock_name.ToUpper();
                                dr["AisData"] = strAisData.ToUpper();
                                dr["PartColor"] = strPartColor.ToUpper();
                                dr["Module"] = strModule.ToUpper();
                                dr["actcol"] = col;
                                dr["actrow"] = row;
                                dr["Result"] = result.ToUpper();
                                dr["Error_log"] = errmsg1.ToUpper() + errmsg.ToUpper();
                                dr["upload_date"] = upload_date;


                                dsRackmstTb.Rows.Add(dr);


                                string log_message = "";

                                log_message = log_message + "Record plc_no : " + strplc_no;
                                log_message = log_message + " rack_name : " + strRack_name;
                                log_message = log_message + " group_name : "+ strGrpname;
                                log_message = log_message + " row : " + strRow;
                                log_message = log_message + " proc_name : " + strproc_name;
                                log_message = log_message + " column :" + strcolumn;
                                log_message = log_message + " block : " + strblock_name;
                                log_message = log_message + " AisData : " + strAisData;
                                log_message = log_message + " PartColor : " + strPartColor;
                                log_message = log_message + " Module : " + strModule;
                                log_message = log_message + " actcol : " + col;
                                log_message = log_message + " actrow  " + row;
                                log_message = log_message + " Result : " + result;
                                log_message = log_message + " Error_log : " + errmsg1.ToUpper() + errmsg.ToUpper();
                                log_message = log_message + " upload_date : " + upload_date;


                                GlobalFunc.Log(log_message);

                            }
                            nxcolCnt = nxcolCnt + 1;
                        }
                        RackdatarowStarting = RackdatarowStarting + 3;
                    }

                    plcrow = plcrow + 22;
                    grprow = grprow + 22;
                    procrow = procrow + 22;
                    blkrow = blkrow + 22;
                    RackdatarowStarting = RackdatarowStarting + 7;
                    detailrecordrackchecking[detailrackcnt] = strRack_name;
                    detailrackcnt = detailrackcnt + 1;

               }
            }


            if (dsRackmstTb.Rows.Count > 0)
            {

                // update to dpsrackexcel for log purposes
                //
                string consString = ConfigurationManager.ConnectionStrings["ConnDpsSQL"].ConnectionString;
                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name
                        sqlBulkCopy.DestinationTableName = "dbo.dpsRackexcel";

                        //[OPTIONAL]: Map the DataTable columns with that of the database table

                        sqlBulkCopy.ColumnMappings.Add("User_id", "User_id");
                        sqlBulkCopy.ColumnMappings.Add("plc_no", "plc_no");
                        sqlBulkCopy.ColumnMappings.Add("rack_name", "rack_name");
                        sqlBulkCopy.ColumnMappings.Add("group_name", "group_name");
                        sqlBulkCopy.ColumnMappings.Add("row", "row");
                        sqlBulkCopy.ColumnMappings.Add("proc_name", "proc_name");
                        sqlBulkCopy.ColumnMappings.Add("column", "column");
                        sqlBulkCopy.ColumnMappings.Add("block", "block");
                        sqlBulkCopy.ColumnMappings.Add("AisData", "AisData");
                        sqlBulkCopy.ColumnMappings.Add("PartColor", "PartColor");
                        sqlBulkCopy.ColumnMappings.Add("Module", "Module");
                        sqlBulkCopy.ColumnMappings.Add("actcol", "actcol");
                        sqlBulkCopy.ColumnMappings.Add("actrow", "actrow");
                        sqlBulkCopy.ColumnMappings.Add("Result", "Result");
                        sqlBulkCopy.ColumnMappings.Add("Error_log", "Error_log");
                        sqlBulkCopy.ColumnMappings.Add("upload_date", "upload_date");

                        con.Open();
                        sqlBulkCopy.WriteToServer(dsRackmstTb);
                        con.Close();
                    }

                }
                // ------------ end ------------
                //

                List<String> sqlQuery = new List<String>();
                //

                DataRow[] Result = dsRackmstTb.Select("Result = 'NOT OK'");
                if (Result.Length == 0)
                {


                    //INITIALISE ALL RECORD PERTAINING TO THE EXCEL SHEET
                    //
                    sqlQuery.Add("delete from dt_RackMstDet where " + strrackcriteria1);
                    sqlQuery.Add("update ais_PartsNum set rack_det_id = null, rack_loc = null WHERE " + strrackcriteria2);
                    sqlQuery.Add("update dt_LampModuleAddMst SET rack_det_id = null, rack_loc = null WHERE " + strrackcriteria2);



                    // update into rackmst and rackmstdet if no error
                    for (int v = 0; v <= dsRackmstTb.Rows.Count - 1; v++)
                    {
                        string rm_rack_name = Convert.ToString(dsRackmstTb.Rows[v]["rack_name"]).Trim();
                        string rm_col_cnt = Convert.ToString(dsRackmstTb.Rows[v]["column"]).Trim();
                        string rm_row_cnt = Convert.ToString(dsRackmstTb.Rows[v]["row"]).Trim();
                        string rm_plc_no = Convert.ToString(dsRackmstTb.Rows[v]["plc_no"]).Trim();
                        string rm_proc_name = Convert.ToString(dsRackmstTb.Rows[v]["proc_name"]).Trim();
                        string rm_group_name = Convert.ToString(dsRackmstTb.Rows[v]["group_name"]).Trim();
                        string rm_block_name = Convert.ToString(dsRackmstTb.Rows[v]["block"]).Trim();
                        string rm_AisData = Convert.ToString(dsRackmstTb.Rows[v]["AisData"]).Trim();

                        string rm_last_upd_by = Convert.ToString(dsRackmstTb.Rows[v]["User_id"]);


                        // update rack master ----------------



                        string[] partno_colors = Convert.ToString(dsRackmstTb.Rows[v]["PartColor"]).Split('-');

                        string rm_partno = partno_colors[0] + '-' + partno_colors[1];
                        string rm_color = partno_colors[2];

                        string rm_col = Convert.ToString(dsRackmstTb.Rows[v]["actcol"]);
                        string rm_row = Convert.ToString(dsRackmstTb.Rows[v]["actrow"]);

                        string rm_rackid = rm_rack_name + "^" + rm_row + "^" + rm_col;

                        string rm_symbol_code = csDatabase.getSinglefield("ais_PartsNum", "part_no", rm_partno, "color_sfx", rm_color, "symbol_code");
                        string rm_symbol_rtf = csDatabase.getSinglefield("ais_PartsNum", "part_no", rm_partno, "color_sfx", rm_color, "symbol");

                        DateTime rm_last_upd_dtt = DateTime.Now;

                        string[] rm_modules = Convert.ToString(dsRackmstTb.Rows[v]["Module"]).Trim().Split('-');

                        string module = rm_modules[0].Trim();
                        string strModuleName = rm_modules[1].Trim();

                        String strRackLoc = "";
                        String[] tmpRackMstDetIdVal = rm_rackid.Split('^');
                        int tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
                        if (tmpRackMstDetIdCnt > 1)
                        {
                            strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
                        }


                        // initialise and remove rack detail before re update


                        new_rack = rm_rack_name;

                        if (new_rack != old_rack)
                        {

                            //  not sure whether need to initialise block mst submit all to null
                            //  csDatabase.UpdLmChange(strPlcNo, strProcName, strBlockName, "");
                            //
                            sqlQuery.Add("UPDATE dt_RackMst SET rack_name = '" + rm_rack_name + "', col_cnt = '" + rm_col_cnt + "', row_cnt = '" + rm_row_cnt + "', plc_no = '" + rm_plc_no + "', proc_name = '" + rm_proc_name + "', group_name = '" + rm_group_name + "', block_name = '" + rm_block_name + "', last_upd_by = '" + rm_last_upd_by + "', last_upd_dt = CURRENT_TIMESTAMP WHERE rack_name = '" + rm_rack_name + "'");

                            old_rack = new_rack;

                        }

                        // update rack master
                        //


                        try
                        {
                            // update part


                            tmpRackMstDetIdVal = rm_rackid.Split('^');
                            tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
                            if (tmpRackMstDetIdCnt > 1)
                            {
                                strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
                                //blUpdFlg =  csDatabase.UpdPartsLoc      (strPartsNo, strColorSfx, strRackMstDetId, strRackLoc);
                                //
                                // add by vincent
                                //
                                rm_partno = rm_partno.Replace("'", "''");
                                rm_color = rm_color.Replace("'", "''");
                                rm_rackid = rm_rackid.Replace("'", "''");
                                strRackLoc = strRackLoc.Replace("'", "''");

                                sqlQuery.Add("UPDATE ais_PartsNum SET rack_det_id = '" + rm_rackid + "', rack_loc = '" + strRackLoc + "' WHERE part_no = '" + rm_partno + "' AND color_sfx = '" + rm_color + "'");


                                // add by vincent end

                                rm_rackid = rm_rackid.Replace("'", "''");
                                string strHjId = "";
                                string strHjItemId = "";
                                string strHjRow = "";
                                string strHjCol = "";
                                string strPartsTitle = rm_AisData.Replace("'", "''");
                                string strPartsNo = rm_partno.Replace("'", "''");
                                string strColorSfx = rm_color.Replace("'", "''");
                                string strPartsNumSymbolCode = rm_symbol_code.Replace("'", "''");
                                string strPartsNumSymbolRtf = rm_symbol_rtf.Replace("'", "''");

                                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to insert Rack Master Location '" + strRackLoc + "'");

                                sqlQuery.Add("INSERT INTO dt_RackMstDet (rack_det_id, hj_id, hj_item_id, hj_row, hj_col, parts_title, parts_no, color_sfx, symbol_code, symbol_rtf, rack_name, plc_no, proc_name) VALUES ('" + rm_rackid + "', '" + strHjId + "','" + strHjItemId + "','" + strHjRow + "','" + strHjCol + "','" + strPartsTitle + "','" + strPartsNo + "','" + strColorSfx + "','" + strPartsNumSymbolCode + "','" + strPartsNumSymbolRtf + "','" + rm_rack_name + "','" + rm_plc_no + "','" + rm_proc_name + "')");


                                sqlQuery.Add("UPDATE dt_RackMstDet SET rack_name = '" + rm_rack_name + "', module_add = '" + module + "', module_name = '" + strModuleName + "', last_upd_by = '" + rm_last_upd_by + "', last_upd_dt = CURRENT_TIMESTAMP WHERE rack_det_id = '" + rm_rackid + "'");

                                if (module == "" && strModuleName == "" && strRackLoc == "")
                                {
                                    sqlQuery.Add("UPDATE dt_LampModuleAddMst SET rack_det_id = null, rack_loc = null WHERE plc_no = '" + rm_plc_no + "' AND module_add = '" + module + "'");
                                }
                                else
                                {
                                    sqlQuery.Add("UPDATE dt_LampModuleAddMst SET rack_det_id = '" + rm_rackid + "', rack_loc = '" + strRackLoc + "' WHERE plc_no = '" + rm_plc_no + "' AND module_add = '" + module + "'");
                                }

                            }

                            // end add
                        }
                        catch (Exception ex)
                        {
                            GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> Not able to update Rack Master Location '" + strRackLoc + "'");

                        }


                    }


                    //sqlQuery.Add("delete from dpsRackexcel where User_id = ''" + Session["SessUserID"] + "'");

                    //  still pending ... need to verify the query is all in order 

                    //open this section when ready to transact - by vincent

                    Boolean strExeFlag = false;

                    try
                    {

                        strExeFlag = ConnQuery.ExecuteTransSaveQuery(sqlQuery);


                    }
                    catch (Exception ex)
                    {

                        GlobalFunc.Log(ex);
                    }

 
                    NewPageIndex = 0;
                    BindGridErrorlog(dsRackmstTb);

                    lblUploadStatus.Text = "";
                    //after validating and conversion of the date save into database\
                    if (strExeFlag == true)
                    {
                        String JsScript = "alert(\'Process Successful \'); ";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertscript", JsScript, true);
                    }
                    else
                    {
                        String JsScript = "alert(\'Not able to Process. Please refer to log \'); ";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertscript", JsScript, true);
                    }


                    // bind data grid to disploay log still pending ....
                    // and out put to text file

                    // --- end of pending ---

                }
                else
                {
                    NewPageIndex = 0;
                    BindGridErrorlog(dsRackmstTb);
                    lblUploadStatus.Text = "";

                    String JsScript = "alert(\'One of the Excel data is not valid, please check the log file \'); ";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertscript", JsScript, true);

                }
            }
            else
            {
                String JsScript = "alert(\'No data found in the Excel data, please check \'); ";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertscript", JsScript, true);

            }

        }
        catch (Exception ex)
        {
            throw (ex);
        }


    }
}