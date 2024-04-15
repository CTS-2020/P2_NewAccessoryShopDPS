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

public partial class ManUpdDpsInsData : System.Web.UI.Page
{
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        String tempDpsRsConvId = Convert.ToString(Session["SessDpsRsConvId"]);

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

                if (tempDpsRsConvId != "")
                {
                    DataSet dsDpsRsConv = new DataSet();
                    DataTable dtDpsRsConv = new DataTable();
                    dsDpsRsConv = csDatabase.searchDpsRsConv(Convert.ToString(tempDpsRsConvId), "", "", "", "", "", "", "", "", "", "");
                    dtDpsRsConv = dsDpsRsConv.Tables[0];
                    BindtoText(dtDpsRsConv);
                }
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
                if (Convert.ToString(dt.Rows[0]["dps_rs_conv_id"]).Trim() != "")
                {
                    lblDpsRsConvId.Text = Convert.ToString(dt.Rows[0]["dps_rs_conv_id"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["dps_ins_code"]).Trim() != "")
                {
                    txtInsCode.Text = Convert.ToString(dt.Rows[0]["dps_ins_code"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["write_pointer"]).Trim() != "")
                {
                    txtPointer.Text = Convert.ToString(dt.Rows[0]["write_pointer"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["id_no"]).Trim() != "")
                {
                    txtIdn.Text = Convert.ToString(dt.Rows[0]["id_no"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["id_ver"]).Trim() != "")
                {
                    txtIdVer.Text = Convert.ToString(dt.Rows[0]["id_ver"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["chassis_no"]).Trim() != "")
                {
                    txtChasNo.Text = Convert.ToString(dt.Rows[0]["chassis_no"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["bseq"]).Trim() != "")
                {
                    txtBseq.Text = Convert.ToString(dt.Rows[0]["bseq"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["model"]).Trim() != "")
                {
                    txtModel.Text = Convert.ToString(dt.Rows[0]["model"]);
                }
                if (Convert.ToString(dt.Rows[0]["sfx"]).Trim() != "")
                {
                    txtSfx.Text = Convert.ToString(dt.Rows[0]["sfx"]);
                }
                if (Convert.ToString(dt.Rows[0]["color_code"]).Trim() != "")
                {
                    txtColor.Text = Convert.ToString(dt.Rows[0]["color_code"]);
                }
                if (Convert.ToString(dt.Rows[0]["plc_no"]).Trim() != "")
                {
                    ddProcName.SelectedValue = Convert.ToString(dt.Rows[0]["plc_no"]);
                    txtPlcNo.Text = Convert.ToString(dt.Rows[0]["plc_no"]).Trim();
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
            txtInsCode.Text = "";
            txtIdn.Text = "";
            txtIdVer.Text = "";
            txtChasNo.Text = "";
            txtBseq.Text = "";
            txtModel.Text = "";
            txtSfx.Text = "";
            txtColor.Text = "";
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ChkValid
    private Boolean ChkValid()
    {
        try
        {
            if (ddProcName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Process Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtPlcNo.Text) == "")
            {
                lblMsg.Text = "Please enter Plc No.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtPointer.Text) == "")
            {
                lblMsg.Text = "Please enter Pointer.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsTextAValidInteger(Convert.ToString(txtPointer.Text)))
            {
                lblMsg.Text = "Pointer must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Int32.Parse(txtPointer.Text) < 1)
            {
                lblMsg.Text = "Write Pointer must be more than 0.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Int32.Parse(txtPointer.Text) > 1000)
            {
                lblMsg.Text = "Write Pointer must be less than 999.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtInsCode.Text) == "")
            {
                lblMsg.Text = "Please enter DPS Instruction Code.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsTextAValidInteger(Convert.ToString(txtInsCode.Text)))
            {
                lblMsg.Text = "Instruction Code must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtIdn.Text) == "")
            {
                lblMsg.Text = "Please enter ID No.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtIdVer.Text) == "")
            {
                lblMsg.Text = "Please enter ID Ver.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsTextAValidInteger(Convert.ToString(txtIdVer.Text)))
            {
                lblMsg.Text = "ID Ver must be a valid integer.";
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
    #region btnUpdate
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (ChkValid())
            {
                String strPointer = Convert.ToString(txtPointer.Text);
                String strModel = Convert.ToString(txtModel.Text);
                String strSfx = Convert.ToString(txtSfx.Text);
                String strColor = Convert.ToString(txtColor.Text);
                String strInsCode = Convert.ToString(txtInsCode.Text);
                String strBseq = Convert.ToString(txtBseq.Text);
                String strIdNo = Convert.ToString(txtIdn.Text);
                String strIdVer = Convert.ToString(txtIdVer.Text);
                String strChassisNo = Convert.ToString(txtChasNo.Text);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strDpsRsConvId = Convert.ToString(lblDpsRsConvId.Text);

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.updateDpsRsConv(strPlcNo, strPointer, strModel, strSfx, strColor, strInsCode, strBseq, strIdNo, strIdVer, strChassisNo, strProcName, strDpsRsConvId);
                    if (!tmpFlag)
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> Unable to update DPS Instruction Data for ID No: " + Convert.ToString(txtIdn.Text) + ".'");
                        lblMsg.Text = "Unable to update DPS Instruction Data for ID No: " + Convert.ToString(txtIdn.Text) + ".";
                        lblMsg.Visible = true;
                    }
                    else
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> DPS Instruction Data for ID No: " + Convert.ToString(txtIdn.Text) + " updated.");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('DPS Instruction Data for ID No: " + Convert.ToString(txtIdn.Text) + " updated.')){window.location.href='ManUpdDpsInsData.aspx'}</script>";
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
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Info",
            "alert('" + ex + "');", true);
        }
    }
    #endregion

    #region BtnBack
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("ManUpdDpsInsData.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Selected Index Changed
    protected void ddProcName_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPlcNo.Text = Convert.ToString(ddProcName.SelectedValue).Trim();
    }
    #endregion
    #endregion
}
