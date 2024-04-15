using System;
using System.Data;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using dpant;

public partial class ManPointerChg : System.Web.UI.Page
{
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
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
                lblPlcName_1.Text = "1 : PLC Not Available";
                lblPlcName_2.Text = "2 : PLC Not Available";
                lblPlcName_3.Text = "3 : PLC Not Available";
                lblPlcName_4.Text = "4 : PLC Not Available";
                lblPlcName_5.Text = "5 : PLC Not Available";
                lblPlcName_6.Text = "6 : PLC Not Available";
                lblPlcName_7.Text = "7 : PLC Not Available";
                lblPlcName_8.Text = "8 : PLC Not Available";
                lblPlcName_9.Text = "9 : PLC Not Available";
                lblPlcName_10.Text = "10 : PLC Not Available";
                lblPlcName_11.Text = "11 : PLC Not Available";
                lblPlcName_12.Text = "12 : PLC Not Available";

                lblPlcName_1.Enabled = false;
                lblPlcName_2.Enabled = false;
                lblPlcName_3.Enabled = false;
                lblPlcName_4.Enabled = false;
                lblPlcName_5.Enabled = false;
                lblPlcName_6.Enabled = false;
                lblPlcName_7.Enabled = false;
                lblPlcName_8.Enabled = false;
                lblPlcName_9.Enabled = false;
                lblPlcName_10.Enabled = false;
                lblPlcName_11.Enabled = false;
                lblPlcName_12.Enabled = false;

                btnPointerUpd_1.Enabled = false;
                btnPointerUpd_2.Enabled = false;
                btnPointerUpd_3.Enabled = false;
                btnPointerUpd_4.Enabled = false;
                btnPointerUpd_5.Enabled = false;
                btnPointerUpd_6.Enabled = false;
                btnPointerUpd_7.Enabled = false;
                btnPointerUpd_8.Enabled = false;
                btnPointerUpd_9.Enabled = false;
                btnPointerUpd_10.Enabled = false;
                btnPointerUpd_11.Enabled = false;
                btnPointerUpd_12.Enabled = false;

                txtPlcWritePointer_1.ReadOnly = true;
                txtPlcWritePointer_2.ReadOnly = true;
                txtPlcWritePointer_3.ReadOnly = true;
                txtPlcWritePointer_4.ReadOnly = true;
                txtPlcWritePointer_5.ReadOnly = true;
                txtPlcWritePointer_6.ReadOnly = true;
                txtPlcWritePointer_7.ReadOnly = true;
                txtPlcWritePointer_8.ReadOnly = true;
                txtPlcWritePointer_9.ReadOnly = true;
                txtPlcWritePointer_10.ReadOnly = true;
                txtPlcWritePointer_11.ReadOnly = true;
                txtPlcWritePointer_12.ReadOnly = true;

                getPlcName();
                getWritePointer();
                getReadPointer();
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
    }
    #endregion

    #region Method

    #region getPlcName
    private void getPlcName()
    {
        try
        {
            DataSet dsProcName = new DataSet();
            DataTable dtProcName = new DataTable();

            dsProcName = GlobalFunc.getPlcName();
            dtProcName = dsProcName.Tables[0];

            for (int i = 0; i < dtProcName.Rows.Count; i++)
            {
                int iPlcNo = Convert.ToInt32(dtProcName.Rows[i]["plc_no"]);

                switch (iPlcNo)
                {
                    case 1:
                        lblPlcName_1.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_1.Enabled = true;
                        lblPlcName_1.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_1.Enabled = true;
                        txtPlcWritePointer_1.ReadOnly = false;
                        break;

                    case 2:
                        lblPlcName_2.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_2.Enabled = true;
                        lblPlcName_2.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_2.Enabled = true;
                        txtPlcWritePointer_2.ReadOnly = false;
                        break;

                    case 3:
                        lblPlcName_3.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_3.Enabled = true;
                        lblPlcName_3.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_3.Enabled = true;
                        txtPlcWritePointer_3.ReadOnly = false;
                        break;

                    case 4:
                        lblPlcName_4.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_4.Enabled = true;
                        lblPlcName_4.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_4.Enabled = true;
                        txtPlcWritePointer_4.ReadOnly = false;
                        break;

                    case 5:
                        lblPlcName_5.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_5.Enabled = true;
                        lblPlcName_5.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_5.Enabled = true;
                        txtPlcWritePointer_5.ReadOnly = false;
                        break;

                    case 6:
                        lblPlcName_6.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_6.Enabled = true;
                        lblPlcName_6.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_6.Enabled = true;
                        txtPlcWritePointer_6.ReadOnly = false;
                        break;

                    case 7:
                        lblPlcName_7.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_7.Enabled = true;
                        lblPlcName_7.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_7.Enabled = true;
                        txtPlcWritePointer_7.ReadOnly = false;
                        break;

                    case 8:
                        lblPlcName_8.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_8.Enabled = true;
                        lblPlcName_8.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_8.Enabled = true;
                        txtPlcWritePointer_8.ReadOnly = false;
                        break;

                    case 9:
                        lblPlcName_9.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_9.Enabled = true;
                        lblPlcName_9.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_9.Enabled = true;
                        txtPlcWritePointer_9.ReadOnly = false;
                        break;

                    case 10:
                        lblPlcName_10.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_10.Enabled = true;
                        lblPlcName_10.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_10.Enabled = true;
                        txtPlcWritePointer_10.ReadOnly = false;
                        break;

                    case 11:
                        lblPlcName_11.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_11.Enabled = true;
                        lblPlcName_11.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_11.Enabled = true;
                        txtPlcWritePointer_11.ReadOnly = false;
                        break;

                    case 12:
                        lblPlcName_12.Text = iPlcNo + " : " + Convert.ToString(dtProcName.Rows[i]["Description"]);
                        lblPlcName_12.Enabled = true;
                        lblPlcName_12.NavigateUrl = "ConvResult.aspx?pn=" + iPlcNo;
                        btnPointerUpd_12.Enabled = true;
                        txtPlcWritePointer_12.ReadOnly = false;
                        break;

                    default:
                        lblPlcName_1.Text = "1 : Error";
                        lblPlcName_2.Text = "2 : Error";
                        lblPlcName_3.Text = "3 : Error";
                        lblPlcName_4.Text = "4 : Error";
                        lblPlcName_5.Text = "5 : Error";
                        lblPlcName_6.Text = "6 : Error";
                        lblPlcName_7.Text = "7 : Error";
                        lblPlcName_8.Text = "8 : Error";
                        lblPlcName_9.Text = "9 : Error";
                        lblPlcName_10.Text = "10 : Error";
                        lblPlcName_11.Text = "11 : Error";
                        lblPlcName_12.Text = "12 : Error";
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getWritePointer
    private void getWritePointer()
    {
        try
        {
            DataSet dsWritePointer = new DataSet();
            DataTable dtWritePointer = new DataTable();

            dsWritePointer = GlobalFunc.getWritePointer();
            dtWritePointer = dsWritePointer.Tables[0];

            for (int i = 0; i < dtWritePointer.Rows.Count; i++)
            {
                int iPlcNo = Convert.ToInt32(dtWritePointer.Rows[i]["plc_no"]);

                switch (iPlcNo)
                {
                    case 1:
                        txtPlcWritePointer_1.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_1.Text) == "1 : PLC Not Available") txtPlcWritePointer_1.Text = "";
                        break;

                    case 2:
                        txtPlcWritePointer_2.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_2.Text) == "2 : PLC Not Available") txtPlcWritePointer_2.Text = "";
                        break;

                    case 3:
                        txtPlcWritePointer_3.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_3.Text) == "3 : PLC Not Available") txtPlcWritePointer_3.Text = "";
                        break;

                    case 4:
                        txtPlcWritePointer_4.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_4.Text) == "4 : PLC Not Available") txtPlcWritePointer_4.Text = "";
                        break;

                    case 5:
                        txtPlcWritePointer_5.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_5.Text) == "5 : PLC Not Available") txtPlcWritePointer_5.Text = "";
                        break;

                    case 6:
                        txtPlcWritePointer_6.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_6.Text) == "6 : PLC Not Available") txtPlcWritePointer_6.Text = "";
                        break;

                    case 7:
                        txtPlcWritePointer_7.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_7.Text) == "7 : PLC Not Available") txtPlcWritePointer_7.Text = "";
                        break;

                    case 8:
                        txtPlcWritePointer_8.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_8.Text) == "8 : PLC Not Available") txtPlcWritePointer_8.Text = "";
                        break;

                    case 9:
                        txtPlcWritePointer_9.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_9.Text) == "9 : PLC Not Available") txtPlcWritePointer_9.Text = "";
                        break;

                    case 10:
                        txtPlcWritePointer_10.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_10.Text) == "10 : PLC Not Available") txtPlcWritePointer_10.Text = "";
                        break;

                    case 11:
                        txtPlcWritePointer_11.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_11.Text) == "11 : PLC Not Available") txtPlcWritePointer_11.Text = "";
                        break;

                    case 12:
                        txtPlcWritePointer_12.Text = Convert.ToString(dtWritePointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_12.Text) == "12 : PLC Not Available") txtPlcWritePointer_12.Text = "";
                        break;

                    default:
                        lblPlcName_1.Text = "Error";
                        lblPlcName_2.Text = "Error";
                        lblPlcName_3.Text = "Error";
                        lblPlcName_4.Text = "Error";
                        lblPlcName_5.Text = "Error";
                        lblPlcName_6.Text = "Error";
                        lblPlcName_7.Text = "Error";
                        lblPlcName_8.Text = "Error";
                        lblPlcName_9.Text = "Error";
                        lblPlcName_10.Text = "Error";
                        lblPlcName_11.Text = "Error";
                        lblPlcName_12.Text = "Error";
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getReadPointer
    private void getReadPointer()
    {
        try
        {
            DataSet dsReadPointer = new DataSet();
            DataTable dtReadPointer = new DataTable();

            dsReadPointer = GlobalFunc.getReadPointer();
            dtReadPointer = dsReadPointer.Tables[0];

            for (int i = 0; i < dtReadPointer.Rows.Count; i++)
            {
                int iPlcNo = Convert.ToInt32(dtReadPointer.Rows[i]["plc_no"]);

                switch (iPlcNo)
                {
                    case 1:
                        txtPlcReadPointer_1.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_1.Text) == "1 : PLC Not Available") txtPlcReadPointer_1.Text = "";
                        break;

                    case 2:
                        txtPlcReadPointer_2.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_2.Text) == "2 : PLC Not Available") txtPlcReadPointer_2.Text = "";
                        break;

                    case 3:
                        txtPlcReadPointer_3.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_3.Text) == "3 : PLC Not Available") txtPlcReadPointer_3.Text = "";
                        break;

                    case 4:
                        txtPlcReadPointer_4.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_4.Text) == "4 : PLC Not Available") txtPlcReadPointer_4.Text = "";
                        break;

                    case 5:
                        txtPlcReadPointer_5.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_5.Text) == "5 : PLC Not Available") txtPlcReadPointer_5.Text = "";
                        break;

                    case 6:
                        txtPlcReadPointer_6.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_6.Text) == "6 : PLC Not Available") txtPlcReadPointer_6.Text = "";
                        break;

                    case 7:
                        txtPlcReadPointer_7.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_7.Text) == "7 : PLC Not Available") txtPlcReadPointer_7.Text = "";
                        break;

                    case 8:
                        txtPlcReadPointer_8.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_8.Text) == "8 : PLC Not Available") txtPlcReadPointer_8.Text = "";
                        break;

                    case 9:
                        txtPlcReadPointer_9.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_9.Text) == "9 : PLC Not Available") txtPlcReadPointer_9.Text = "";
                        break;

                    case 10:
                        txtPlcReadPointer_10.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_10.Text) == "10 : PLC Not Available") txtPlcReadPointer_10.Text = "";
                        break;

                    case 11:
                        txtPlcReadPointer_11.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_11.Text) == "11 : PLC Not Available") txtPlcReadPointer_11.Text = "";
                        break;

                    case 12:
                        txtPlcReadPointer_12.Text = Convert.ToString(dtReadPointer.Rows[i]["Description"]);
                        if (Convert.ToString(lblPlcName_12.Text) == "12 : PLC Not Available") txtPlcReadPointer_12.Text = "";
                        break;

                    default:
                        lblPlcName_1.Text = "Error";
                        lblPlcName_2.Text = "Error";
                        lblPlcName_3.Text = "Error";
                        lblPlcName_4.Text = "Error";
                        lblPlcName_5.Text = "Error";
                        lblPlcName_6.Text = "Error";
                        lblPlcName_7.Text = "Error";
                        lblPlcName_8.Text = "Error";
                        lblPlcName_9.Text = "Error";
                        lblPlcName_10.Text = "Error";
                        lblPlcName_11.Text = "Error";
                        lblPlcName_12.Text = "Error";
                        break;
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
            lblPlcName_1.Text = "No PLC #1";
            lblPlcName_2.Text = "No PLC #2";
            lblPlcName_3.Text = "No PLC #3";
            lblPlcName_4.Text = "No PLC #4";
            lblPlcName_5.Text = "No PLC #5";
            lblPlcName_6.Text = "No PLC #6";
            lblPlcName_7.Text = "No PLC #7";
            lblPlcName_8.Text = "No PLC #8";
            lblPlcName_9.Text = "No PLC #9";
            lblPlcName_10.Text = "No PLC #10";
            lblPlcName_11.Text = "No PLC #11";
            lblPlcName_12.Text = "No PLC #12";

            getPlcName();
            getWritePointer();
            getReadPointer();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Update PLC Pointer
    private void updatePlcPointerMst(String iPlcNo)
    {
        try
        {
            GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to update PLC Pointer for '" + iPlcNo + "'");

            String strWritePointer = "";
            switch (iPlcNo)
            {
                case "1":
                    strWritePointer = txtPlcWritePointer_1.Text;
                    break;

                case "2":
                    strWritePointer = txtPlcWritePointer_2.Text;
                    break;

                case "3":
                    strWritePointer = txtPlcWritePointer_3.Text;
                    break;

                case "4":
                    strWritePointer = txtPlcWritePointer_4.Text;
                    break;

                case "5":
                    strWritePointer = txtPlcWritePointer_5.Text;
                    break;

                case "6":
                    strWritePointer = txtPlcWritePointer_6.Text;
                    break;

                case "7":
                    strWritePointer = txtPlcWritePointer_7.Text;
                    break;

                case "8":
                    strWritePointer = txtPlcWritePointer_8.Text;
                    break;

                case "9":
                    strWritePointer = txtPlcWritePointer_9.Text;
                    break;

                case "10":
                    strWritePointer = txtPlcWritePointer_10.Text;
                    break;

                case "11":
                    strWritePointer = txtPlcWritePointer_11.Text;
                    break;

                case "12":
                    strWritePointer = txtPlcWritePointer_12.Text;
                    break;
            }

            if (strWritePointer != "")
            {
                if (!GlobalFunc.IsTextAValidInteger(strWritePointer))
                {
                    Response.Write("<script language='javascript'>alert('Write Pointer must be a valid integer.')</script>");
                    return;
                }
                else if (Int32.Parse(strWritePointer) < 0)
                {
                    Response.Write("<script language='javascript'>alert('Write Pointer must be more than 0.')</script>");
                    return;
                }
                else if (Int32.Parse(strWritePointer) > 999)
                {
                    Response.Write("<script language='javascript'>alert('Write Pointer must be less than 999.')</script>");
                    return;
                }
                else
                {
                    Boolean blUpdPointer = csDatabase.updPlcPointerMst(iPlcNo, strWritePointer);

                    if (blUpdPointer)
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to update PLC Pointer to '" + strWritePointer + "' for PLC No '" + iPlcNo + "'");
                        Response.Write("<script language='javascript'>alert('Plc No " + iPlcNo + " write pointer updated to " + strWritePointer + ".')</script>");
                    }
                    else
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> Plc No " + iPlcNo + " write pointer NOT updated.");
                        Response.Write("<script language='javascript'>alert('Plc No " + iPlcNo + " write pointer NOT updated. Please check.')</script>");
                    }
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion

    #region Events

    #region btnUpdate
    protected void btnUpdate_Click(object sender, CommandEventArgs e)
    {
        try
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                String iPlcNo = (String)e.CommandArgument;
                updatePlcPointerMst(iPlcNo);
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
