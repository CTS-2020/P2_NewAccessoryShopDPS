using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.Adapters;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.UI.WebControls;
using System.Activities.Expressions;

/// <summary>
/// Summary description for csDatabase
/// </summary>
public class csDatabase
{
    public csDatabase()
    {

    }

    #region User Maintenance

    public static DataSet searchUser(String strUserID, String strUserName, String strRole)
    {
        String filterCriteria = "";

        if (strUserID != "")
        {
            filterCriteria = filterCriteria + " AND user_id = '" + strUserID + "'";
        }
        if (strUserName != "")
        {
            filterCriteria = filterCriteria + " AND user_name = '" + strUserName + "'";
        }
        if (strRole != "")
        {
            filterCriteria = filterCriteria + " AND role_code = '" + strRole + "'";
        }

        String sqlQuery = "SELECT * FROM dps_User WHERE user_id != '' " + filterCriteria + " ORDER BY user_name";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean deleteUser(String strUserID)
    {
        String sqlQuery = "DELETE FROM dps_User WHERE user_id = '" + strUserID + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean saveUser(String strUserID, String strUserName, String strPassword, String strRoleCode, String strCurUser)
    {
        strUserID = strUserID.Replace("'", "''");
        strUserName = strUserName.Replace("'", "''");
        strPassword = strPassword.Replace("'", "''");
        strRoleCode = strRoleCode.Replace("'", "''");
        strCurUser = strCurUser.Replace("'", "''");

        String sqlQuery = "INSERT INTO dps_User (user_id, user_name, user_password, role_code, last_upd_by, last_upd_dt) VALUES ('" + strUserID + "', '" + strUserName + "','" + strPassword + "','" + strRoleCode + "','" + strCurUser + "',CURRENT_TIMESTAMP)";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean updateUser(String strUserID, String strUserName, String strPassword, String strRoleCode, String strTempUserID, String strCurUser)
    {
        strUserID = strUserID.Replace("'", "''");
        strUserName = strUserName.Replace("'", "''");
        strPassword = strPassword.Replace("'", "''");
        strRoleCode = strRoleCode.Replace("'", "''");
        strCurUser = strCurUser.Replace("'", "''");

        String sqlQuery = "UPDATE dps_User SET user_id = '" + strUserID + "', user_name = '" + strUserName + "', user_password = '" + strPassword + "', role_code = '" + strRoleCode + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE user_id = '" + strTempUserID + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkDuplicateUid(String strUserID, String strTmpUserID)
    {
        String sqlQuery = "SELECT COUNT(user_id) AS CountRow FROM dps_User WHERE user_id = '" + strUserID + "' AND user_id != '" + strTmpUserID + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return true;
        }
    }
    #endregion

    #region DPS Master Entry

    #region Instruction Code Master
    public static DataSet SrcInsCode(String strDpsInsCode, String strModel, String strKatashiki, String strSfx, String strColor, String strComment)
    {
        String filterCriteria = "";

        if (strDpsInsCode != "")
        {
            filterCriteria = filterCriteria + " AND ins_code = " + Convert.ToInt32(strDpsInsCode) + "";
        }
        if (strModel != "")
        {
            filterCriteria = filterCriteria + " AND model = '" + strModel + "'";
        }
        if (strKatashiki != "")
        {
            filterCriteria = filterCriteria + " AND katashiki = '" + strKatashiki + "'";
        }
        if (strSfx != "")
        {
            filterCriteria = filterCriteria + " AND sfx = '" + strSfx + "'";
        }
        if (strColor != "")
        {
            filterCriteria = filterCriteria + " AND color = '" + strColor + "'";
        }
        if (strComment != "")
        {
            filterCriteria = filterCriteria + " AND comment = '" + strComment + "'";
        }

        String sqlQuery = "SELECT * FROM dt_DpsInsCodeMst WHERE ins_code != '' " + filterCriteria + " ORDER BY sfx, color, model, katashiki, ins_code";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean DelInsCode(String strDpsInsCode)
    {
        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("DELETE FROM dt_DpsInsCodeMst WHERE ins_code = '" + strDpsInsCode + "'");

        String strQuery = "SELECT * FROM conv_InsCode WHERE ins_code = '" + strDpsInsCode + "'";

        DataSet dsInsCodeConv = new DataSet();
        DataTable dtInsCodeConv = new DataTable();

        dsInsCodeConv = ConnQuery.getBindingDatasetData(strQuery);
        dtInsCodeConv = dsInsCodeConv.Tables[0];

        if (dtInsCodeConv.Rows.Count > 0)
        {
            for (int iInsCodeConvCnt = 0; iInsCodeConvCnt < dtInsCodeConv.Rows.Count; iInsCodeConvCnt++)
            {
                int iInsCodeCnt = 0;
                String strPlcNo = "";
                String strProcName = "";

                if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["ins_code_cnt"]).Trim() != "")
                {
                    iInsCodeCnt = Convert.ToInt32(dtInsCodeConv.Rows[iInsCodeConvCnt]["ins_code_cnt"]);
                }
                if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["plc_no"]).Trim() != "")
                {
                    strPlcNo = Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["plc_no"]).Trim();
                }
                if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["proc_name"]).Trim() != "")
                {
                    strProcName = Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["proc_name"]).Trim();
                }

                sqlQuery.Add("UPDATE conv_InsCode SET ins_code = NULL, model = NULL, katashiki = NULL, sfx = NULL, color = NULL, comment = NULL WHERE ins_code_cnt = '" + iInsCodeCnt + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");
                sqlQuery.Add("UPDATE conv_InsCodeMap SET [1] = NULL, [2] = NULL, [3] = NULL, [4] = NULL, [5] = NULL, [6] = NULL, [7] = NULL, [8] = NULL, [9] = NULL, [10] = NULL, [11] = NULL, [12] = NULL WHERE ins_code_cnt = '" + iInsCodeCnt + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");
            }
        }

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static void DelInsCodeConvResult(String strDpsInsCode)
    //{
    //    String sqlQuery = "SELECT * FROM conv_InsCode WHERE ins_code = '" + strDpsInsCode + "'";

    //    DataSet dsInsCodeConv = new DataSet();
    //    DataTable dtInsCodeConv = new DataTable();

    //    dsInsCodeConv = ConnQuery.getBindingDatasetData(sqlQuery);
    //    dtInsCodeConv = dsInsCodeConv.Tables[0];

    //    if (dtInsCodeConv.Rows.Count > 0)
    //    {
    //        for (int iInsCodeConvCnt = 0; iInsCodeConvCnt < dtInsCodeConv.Rows.Count; iInsCodeConvCnt++)
    //        {
    //            int iInsCodeCnt = 0;
    //            String strPlcNo = "";
    //            String strProcName = "";

    //            if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["ins_code_cnt"]).Trim() != "")
    //            {
    //                iInsCodeCnt = Convert.ToInt32(dtInsCodeConv.Rows[iInsCodeConvCnt]["ins_code_cnt"]);
    //            }
    //            if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["plc_no"]).Trim() != "")
    //            {
    //                strPlcNo = Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["plc_no"]).Trim();
    //            }
    //            if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["proc_name"]).Trim() != "")
    //            {
    //                strProcName = Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["proc_name"]).Trim();
    //            }

    //            DelInsCodeConv(iInsCodeCnt, strPlcNo, strProcName);
    //            DelInsCodeMapConv(iInsCodeCnt, strPlcNo, strProcName, "");
    //        }
    //    }
    //}

    public static Boolean SvInsCode(String strDpsInsCode, String strModel, String strKatashiki, String strSfx, String strColor, String strComment, String strCurUser)
    {
        strDpsInsCode = strDpsInsCode.Replace("'", "''");
        strModel = strModel.Replace("'", "''");
        strKatashiki = strKatashiki.Replace("'", "''");
        strSfx = strSfx.Replace("'", "''");
        strColor = strColor.Replace("'", "''");
        strComment = strComment.Replace("'", "''");

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("INSERT INTO dt_DpsInsCodeMst (ins_code, model, katashiki, sfx, color, comment, last_upd_by, last_upd_dt) VALUES ('" + strDpsInsCode + "', '" + strModel + "','" + strKatashiki + "','" + strSfx + "','" + strColor + "', '" + strComment + "', '" + strCurUser + "', CURRENT_TIMESTAMP)");

        short[] sInsCode = GlobalFunc.convInt32ToInt16(Convert.ToInt32(strDpsInsCode), 2);
        short[] sModel = GlobalFunc.convStrToAscii(strModel, 2);
        short[] sKatashiki = GlobalFunc.convStrToAscii(strKatashiki, 6);
        short[] sSfx = GlobalFunc.convStrToAscii(strSfx, 1);
        short[] sColor = GlobalFunc.convStrToAscii(strColor, 2);
        short[] sComment = GlobalFunc.convStrToAscii(strComment, 7);

        String byteDpsInsCode = string.Join("*", Array.ConvertAll(sInsCode, x => x.ToString()));
        String byteModel = string.Join("*", Array.ConvertAll(sModel, x => x.ToString()));
        String byteKatashiki = string.Join("*", Array.ConvertAll(sKatashiki, x => x.ToString()));
        String byteSfx = string.Join("*", Array.ConvertAll(sSfx, x => x.ToString()));
        String byteColor = string.Join("*", Array.ConvertAll(sColor, x => x.ToString()));
        String byteComment = string.Join("*", Array.ConvertAll(sComment, x => x.ToString()));

        sqlQuery.Add("UPDATE dt_DpsInsCodeMst SET byte_ins_code = '" + byteDpsInsCode + "', byte_model = '" + byteModel + "', byte_katashiki = '" + byteKatashiki + "', byte_sfx = '" + byteSfx + "', byte_color = '" + byteColor + "', byte_comment = '" + byteComment + "' WHERE ins_code = '" + strDpsInsCode + "'");
        try
        {
            return ConnQuery.ExecuteTransSaveQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdInsCode(String strDpsInsCode, String strModel, String strKatashiki, String strSfx, String strColor, String strComment, String strTempInsCode, String strCurUser)
    {
        strDpsInsCode = strDpsInsCode.Replace("'", "''");
        strModel = strModel.Replace("'", "''");
        strKatashiki = strKatashiki.Replace("'", "''");
        strSfx = strSfx.Replace("'", "''");
        strColor = strColor.Replace("'", "''");
        strComment = strComment.Replace("'", "''");

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("UPDATE dt_DpsInsCodeMst SET ins_code = '" + strDpsInsCode + "', model = '" + strModel + "', katashiki = '" + strKatashiki + "', sfx = '" + strSfx + "', color = '" + strColor + "', comment = '" + strComment + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE ins_code = '" + strTempInsCode + "'");

        short[] sInsCode = GlobalFunc.convInt32ToInt16(Convert.ToInt32(strDpsInsCode), 2);
        short[] sModel = GlobalFunc.convStrToAscii(strModel, 2);
        short[] sKatashiki = GlobalFunc.convStrToAscii(strKatashiki, 6);
        short[] sSfx = GlobalFunc.convStrToAscii(strSfx, 1);
        short[] sColor = GlobalFunc.convStrToAscii(strColor, 2);
        short[] sComment = GlobalFunc.convStrToAscii(strComment, 7);

        String byteDpsInsCode = string.Join("*", Array.ConvertAll(sInsCode, x => x.ToString()));
        String byteModel = string.Join("*", Array.ConvertAll(sModel, x => x.ToString()));
        String byteKatashiki = string.Join("*", Array.ConvertAll(sKatashiki, x => x.ToString()));
        String byteSfx = string.Join("*", Array.ConvertAll(sSfx, x => x.ToString()));
        String byteColor = string.Join("*", Array.ConvertAll(sColor, x => x.ToString()));
        String byteComment = string.Join("*", Array.ConvertAll(sComment, x => x.ToString()));

        sqlQuery.Add("UPDATE dt_DpsInsCodeMst SET byte_ins_code = '" + byteDpsInsCode + "', byte_model = '" + byteModel + "', byte_katashiki = '" + byteKatashiki + "', byte_sfx = '" + byteSfx + "', byte_color = '" + byteColor + "', byte_comment = '" + byteComment + "' WHERE ins_code = '" + strDpsInsCode + "'");

        String strQuery = "SELECT * FROM conv_InsCode WHERE ins_code = " + Convert.ToInt32(strTempInsCode) + "";
        DataSet dsInsCodeConv = new DataSet();
        DataTable dtInsCodeConv = new DataTable();
        dsInsCodeConv = ConnQuery.getBindingDatasetData(strQuery);
        dtInsCodeConv = dsInsCodeConv.Tables[0];

        if (dtInsCodeConv.Rows.Count > 0)
        {
            for (int iInsCodeConvCnt = 0; iInsCodeConvCnt < dtInsCodeConv.Rows.Count; iInsCodeConvCnt++)
            {
                int iInsCodeCnt = 0;
                String strPlcNo = "";
                String strProcName = "";

                if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["ins_code_cnt"]).Trim() != "")
                {
                    iInsCodeCnt = Convert.ToInt32(dtInsCodeConv.Rows[iInsCodeConvCnt]["ins_code_cnt"]);
                }
                if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["plc_no"]).Trim() != "")
                {
                    strPlcNo = Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["plc_no"]).Trim();
                }
                if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["proc_name"]).Trim() != "")
                {
                    strProcName = Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["proc_name"]).Trim();
                }

                sqlQuery.Add("UPDATE conv_InsCode SET ins_code = '" + strDpsInsCode + "', model = '" + strModel + "', katashiki = '" + strKatashiki + "', sfx = '" + strSfx + "', color = '" + strColor + "', comment = '" + strComment + "', byte_ins_code = '" + byteDpsInsCode + "', byte_model = '" + byteModel + "', byte_katashiki = '" + byteKatashiki + "', byte_sfx = '" + byteSfx + "', byte_color = '" + byteColor + "', byte_comment = '" + byteComment + "' WHERE ins_code_cnt = '" + iInsCodeCnt + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");
            }
        }
        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static void UpdInsCodeConvResult(String strInsCode, String strModel, String strKatashiki, String strSfx, String strColor, String strComment, String tempInsCode)
    //{
    //    String sqlQuery = "SELECT * FROM conv_InsCode WHERE ins_code = " + Convert.ToInt32(tempInsCode) + "";

    //    DataSet dsInsCodeConv = new DataSet();
    //    DataTable dtInsCodeConv = new DataTable();

    //    dsInsCodeConv = ConnQuery.getBindingDatasetData(sqlQuery);
    //    dtInsCodeConv = dsInsCodeConv.Tables[0];

    //    if (dtInsCodeConv.Rows.Count > 0)
    //    {
    //        for (int iInsCodeConvCnt = 0; iInsCodeConvCnt < dtInsCodeConv.Rows.Count; iInsCodeConvCnt++)
    //        {
    //            int iInsCodeCnt = 0;
    //            String strPlcNo = "";
    //            String strProcName = "";

    //            if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["ins_code_cnt"]).Trim() != "")
    //            {
    //                iInsCodeCnt = Convert.ToInt32(dtInsCodeConv.Rows[iInsCodeConvCnt]["ins_code_cnt"]);
    //            }
    //            if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["plc_no"]).Trim() != "")
    //            {
    //                strPlcNo = Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["plc_no"]).Trim();
    //            }
    //            if (Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["proc_name"]).Trim() != "")
    //            {
    //                strProcName = Convert.ToString(dtInsCodeConv.Rows[iInsCodeConvCnt]["proc_name"]).Trim();
    //            }

    //            short[] sInsCode = GlobalFunc.convInt32ToInt16(Convert.ToInt32(strInsCode), 2);
    //            short[] sModel = GlobalFunc.convStrToAscii(strModel, 2);
    //            short[] sKatashiki = GlobalFunc.convStrToAscii(strKatashiki, 6);
    //            short[] sSfx = GlobalFunc.convStrToAscii(strSfx, 1);
    //            short[] sColor = GlobalFunc.convStrToAscii(strColor, 2);
    //            short[] sComment = GlobalFunc.convStrToAscii(strComment, 7);

    //            String byteInsCode = string.Join("*", Array.ConvertAll(sInsCode, x => x.ToString()));
    //            String byteModel = string.Join("*", Array.ConvertAll(sModel, x => x.ToString()));
    //            String byteKatashiki = string.Join("*", Array.ConvertAll(sKatashiki, x => x.ToString()));
    //            String byteSfx = string.Join("*", Array.ConvertAll(sSfx, x => x.ToString()));
    //            String byteColor = string.Join("*", Array.ConvertAll(sColor, x => x.ToString()));
    //            String byteComment = string.Join("*", Array.ConvertAll(sComment, x => x.ToString()));

    //            sqlQuery = "UPDATE conv_InsCode SET ins_code = '" + strInsCode + "', model = '" + strModel + "', katashiki = '" + strKatashiki + "', sfx = '" + strSfx + "', color = '" + strColor + "', comment = '" + strComment + "', byte_ins_code = '" + byteInsCode + "', byte_model = '" + byteModel + "', byte_katashiki = '" + byteKatashiki + "', byte_sfx = '" + byteSfx + "', byte_color = '" + byteColor + "', byte_comment = '" + byteComment + "' WHERE ins_code_cnt = '" + iInsCodeCnt + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //            try
    //            {
    //                ConnQuery.ExecuteQuery(sqlQuery);
    //            }
    //            catch (Exception ex)
    //            {
    //                GlobalFunc.Log(ex);
    //                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
    //            }
    //        }
    //    }
    //}

    public static Boolean ChkDuplicateInsCodeCombi(String strModel, String strKatashiki, String strSfx, String strColor, String strCurInsCode)
    {
        strModel = strModel.Replace("'", "''");
        strKatashiki = strKatashiki.Replace("'", "''");
        strSfx = strSfx.Replace("'", "''");
        strColor = strColor.Replace("'", "''");

        String sqlQuery = "SELECT COUNT(ins_code) AS CountRow FROM dt_DpsInsCodeMst WHERE model = '" + strModel + "' AND katashiki = '" + strKatashiki + "' AND sfx = '" + strSfx + "' AND color = '" + strColor + "' AND ins_code != '" + strCurInsCode + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return true;
        }
    }

    public static Boolean ChkDuplicateInsCode(String strInsCode, String strCurInsCode)
    {
        strInsCode = strInsCode.Replace("'", "''");
        strCurInsCode = strCurInsCode.Replace("'", "''");

        String sqlQuery = "SELECT COUNT(ins_code) AS CountRow FROM dt_DpsInsCodeMst WHERE ins_code = '" + strInsCode + "' AND ins_code != '" + strCurInsCode + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return true;
        }
    }

    public static Boolean ChkInsCodeMaxCnt()
    {
        String sqlQuery = "SELECT COUNT(ins_code) AS ReturnField FROM dt_DpsInsCodeMst";
        try
        {
            int iInsCodeCnt = Convert.ToInt32(ConnQuery.getReturnFieldExecuteReader(sqlQuery));
            if (iInsCodeCnt >= 299)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static Boolean UpdInsCodeAscii(String strDpsInsCode, String strModel, String strKatashiki, String strSfx, String strColor, String strComment)
    //{
    //    short[] sInsCode = GlobalFunc.convInt32ToInt16(Convert.ToInt32(strDpsInsCode), 2);
    //    short[] sModel = GlobalFunc.convStrToAscii(strModel, 2);
    //    short[] sKatashiki = GlobalFunc.convStrToAscii(strKatashiki, 6);
    //    short[] sSfx = GlobalFunc.convStrToAscii(strSfx, 1);
    //    short[] sColor = GlobalFunc.convStrToAscii(strColor, 2);
    //    short[] sComment = GlobalFunc.convStrToAscii(strComment, 7);

    //    String byteDpsInsCode = string.Join("*", Array.ConvertAll(sInsCode, x => x.ToString()));
    //    String byteModel = string.Join("*", Array.ConvertAll(sModel, x => x.ToString()));
    //    String byteKatashiki = string.Join("*", Array.ConvertAll(sKatashiki, x => x.ToString()));
    //    String byteSfx = string.Join("*", Array.ConvertAll(sSfx, x => x.ToString()));
    //    String byteColor = string.Join("*", Array.ConvertAll(sColor, x => x.ToString()));
    //    String byteComment = string.Join("*", Array.ConvertAll(sComment, x => x.ToString()));

    //    String sqlQuery = "UPDATE dt_DpsInsCodeMst SET byte_ins_code = '" + byteDpsInsCode + "', byte_model = '" + byteModel + "', byte_katashiki = '" + byteKatashiki + "', byte_sfx = '" + byteSfx + "', byte_color = '" + byteColor + "', byte_comment = '" + byteComment + "' WHERE ins_code = '" + strDpsInsCode + "'";
    //    try
    //    {
    //        return ConnQuery.ExecuteQuery(sqlQuery);
    //    }
    //    catch (Exception ex)
    //    {
    //        GlobalFunc.Log(ex);
    //        GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
    //        return false;
    //    }
    //}
    #endregion

    #region PLC Model      
    //public static DataSet SrcPlcModelList(String strPlcModelNo, String strPlcModelDesc, String strPhyAddrFrom, String strPhyAddrTo, String strDigitNo)
    public static DataSet SrcPlcModelList(String strPlcModelNo, String strPlcModelDesc, String strPhyAddrFrom, String strPhyAddrTo)
    {
        String filterCriteria = "";

        if (strPlcModelNo != "")
        {
            filterCriteria = filterCriteria + " AND plcmodel_no = '" + strPlcModelNo + "'";
        }
        if (strPlcModelDesc != "")
        {
            filterCriteria = filterCriteria + " AND plcmodel_desc LIKE '%" + strPlcModelDesc + "%'";
        }
        if (strPhyAddrFrom != "")
        {
            filterCriteria = filterCriteria + " AND phyaddr_from = '" + strPhyAddrFrom + "'";
        }
        if (strPhyAddrTo != "")
        {
            filterCriteria = filterCriteria + " AND phyaddr_to = '" + strPhyAddrTo + "'";
        }
        //if (strDigitNo != "")
        //{
        //    filterCriteria = filterCriteria + " AND digit_no = '" + strDigitNo + "'";
        //}

        String sqlQuery = "SELECT * FROM dt_PlcModelMst WHERE plcmodel_no != '' " + filterCriteria + " ORDER BY uid";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean DelPlcModelMst(String strPlcModelId, String strPlcModelNo)
    {
        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("DELETE FROM dt_PlcModelMst WHERE uid = '" + strPlcModelId + "' AND plcmodel_no = '" + strPlcModelNo + "'");

        //Update DpsPlcMst with related Model (30/09/2020)
        sqlQuery.Add("UPDATE dt_DpsPlcMst SET plc_model = '' WHERE plc_model = '" + strPlcModelNo + "'");

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static Boolean SvPlcModelMst(String strPlcModelNo, String strPlcModelDesc, String strPhyAddrFrom, String strPhyAddrTo, String strDigitNo, String strEnable, String strCurUser)
    public static Boolean SvPlcModelMst(String strPlcModelNo, String strPlcModelDesc, String strPhyAddrFrom, String strPhyAddrTo, String strConvType, String strEnable, String strCurUser)
    {
        strPlcModelNo = strPlcModelNo.Replace("'", "''");
        strPlcModelDesc = strPlcModelDesc.Replace("'", "''");
        strPhyAddrFrom = strPhyAddrFrom.Replace("'", "''");
        strPhyAddrTo = strPhyAddrTo.Replace("'", "''");
        strConvType = strConvType.Replace("'", "'");
        //strDigitNo = strDigitNo.Replace("'", "''");
        strEnable = strEnable.Replace("'", "''");
        strCurUser = strCurUser.Replace("'", "''");

        String sqlQuery = "INSERT INTO dt_PlcModelMst (plcmodel_no, plcmodel_desc, phyaddr_from, phyaddr_to, conv_type, enable, last_upd_by, last_upd_dt) VALUES ('" + strPlcModelNo + "', '" + strPlcModelDesc + "','" + strPhyAddrFrom + "','" + strPhyAddrTo + "','" + strConvType + "','" + strEnable + "','" + strCurUser + "', CURRENT_TIMESTAMP)";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdPlcModelMst(String strPlcModelNo, String strPlcModelDesc, String strPhyAddrFrom, String strPhyAddrTo, String strConvType, String strEnable, String tempPlcModelNo, String strCurUser)
    {
        strPlcModelNo = strPlcModelNo.Replace("'", "''");
        strPlcModelDesc = strPlcModelDesc.Replace("'", "''");
        strPhyAddrFrom = strPhyAddrFrom.Replace("'", "''");
        strPhyAddrTo = strPhyAddrTo.Replace("'", "''");
        strConvType = strConvType.Replace("'", "''");
        //strDigitNo = strDigitNo.Replace("'", "''");
        strEnable = strEnable.Replace("'", "''");
        strCurUser = strCurUser.Replace("'", "''");

        List<String> sqlQuery = new List<String>();

        //Modified by YanTeng 11/09/2020 (Added plc_model)
        sqlQuery.Add("UPDATE dt_PlcModelMst SET plcmodel_no = '" + strPlcModelNo + "', plcmodel_desc = '" + strPlcModelDesc + "', phyaddr_from = '" + strPhyAddrFrom + "', phyaddr_to = '" + strPhyAddrTo + "', conv_type = '" + strConvType + "', enable = '" + strEnable + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plcmodel_no = '" + tempPlcModelNo + "'");

        //Update All DpsPlcMst with related Model
        sqlQuery.Add("UPDATE dt_DpsPlcMst SET plc_model = '" + strPlcModelNo + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_model = '" + tempPlcModelNo + "'");

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkDuplicatePlcModel(String strPlcModelNew, String strPlcModel)
    {
        strPlcModelNew = strPlcModelNew.Replace("'", "''");

        String sqlQuery = "SELECT COUNT(plcmodel_no) AS CountRow FROM dt_PlcModelMst WHERE plcmodel_no = '" + strPlcModelNew + "' AND plcmodel_no != '" + strPlcModel + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return true;
        }
    }



    #endregion

    #region PLC Master
    public static DataSet SrcPlcMstList(String strPlcNo, String strProcName, String strIpAdd, String strPlcModel, String strPlcNwStation, String strPlcLogicalStation)
    {
        String filterCriteria = "";

        if (strPlcNo != "")
        {
            filterCriteria = filterCriteria + " AND plc_no = '" + strPlcNo + "'";
        }
        if (strProcName != "")
        {
            filterCriteria = filterCriteria + " AND proc_name = '" + strProcName + "'";
        }
        if (strIpAdd != "")
        {
            filterCriteria = filterCriteria + " AND ip_add = '" + strIpAdd + "'";
        }
        if (strPlcModel != "")          //Added by YanTeng 10/09/2020
        {
            filterCriteria = filterCriteria + " AND plc_model = '" + strPlcModel + "'";
        }
        if (strPlcNwStation != "")
        {
            filterCriteria = filterCriteria + " AND plc_nw_station = '" + strPlcNwStation + "'";
        }
        if (strPlcLogicalStation != "")
        {
            filterCriteria = filterCriteria + " AND plc_logical_station_no = '" + strPlcLogicalStation + "'";
        }

        String sqlQuery = "SELECT * FROM dt_DpsPlcMst WHERE plc_no != '' " + filterCriteria + " ORDER BY plc_no";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean DelPlcMst(String strPlcNo, String strProcName)
    {
        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("DELETE FROM dt_DpsPlcMst WHERE plc_no = '" + strPlcNo + "'");

        //Delete All Group
        sqlQuery.Add("DELETE FROM dt_GroupMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        //Delete All Block
        sqlQuery.Add("DELETE FROM dt_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        DataSet dsRackMst = new DataSet();
        DataTable dtRackMst = new DataTable();

        dsRackMst = SrcRackMst("", strPlcNo, strProcName, "", "");
        dtRackMst = dsRackMst.Tables[0];

        if (dtRackMst.Rows.Count > 0)
        {
            for (int iRackCnt = 0; iRackCnt < dtRackMst.Rows.Count; iRackCnt++)
            {
                String strRackName = "";

                if (Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]).Trim() != "")
                {
                    strRackName = Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]);
                }

                //Delete All Rack Detail
                sqlQuery.Add("DELETE FROM dt_RackMstDet WHERE rack_name = '" + strRackName + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

                //Delete All Part Rack Loc
                sqlQuery.Add("UPDATE ais_PartsNum SET rack_det_id = NULL, rack_loc = NULL WHERE rack_det_id LIKE '" + strRackName + "^%'");
            }
        }

        //Delete All Rack
        sqlQuery.Add("DELETE FROM dt_RackMst WHERE proc_name = '" + strProcName + "' AND plc_no = '" + strPlcNo + "'");

        //Delete All Lamp Module Address
        sqlQuery.Add("DELETE FROM dt_LampModuleAddMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        //Delete All Lamp Module Mode Conversion
        sqlQuery.Add("DELETE FROM conv_LampModuleMode WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        //Delete All Instruction Code Conversion
        sqlQuery.Add("DELETE FROM conv_InsCode WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        //Delete All Instruction Code Map Conversion
        sqlQuery.Add("DELETE FROM conv_InsCodeMap WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        //Delete All Block Conversion
        sqlQuery.Add("DELETE FROM conv_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static void DelPlcAllRem(String strPlcNo, String strProcName)
    //{
    //    String sqlQuery = "";

    //    //Delete All Group
    //    sqlQuery = "DELETE FROM dt_GroupMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    Boolean blDelGroup = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Block
    //    sqlQuery = "DELETE FROM dt_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    Boolean blDelBlock = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Rack Detail
    //    DataSet dsRackMst = new DataSet();
    //    DataTable dtRackMst = new DataTable();

    //    dsRackMst = SrcRackMst("", strPlcNo, strProcName, "", "");
    //    dtRackMst = dsRackMst.Tables[0];

    //    if (dtRackMst.Rows.Count > 0)
    //    {
    //        for (int iRackCnt = 0; iRackCnt < dtRackMst.Rows.Count; iRackCnt++)
    //        {
    //            String strRackName = "";

    //            if (Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]).Trim() != "")
    //            {
    //                strRackName = Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]);
    //            }

    //            sqlQuery = "DELETE FROM dt_RackMstDet WHERE rack_name = '" + strRackName + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //            Boolean blDelRackDet = ConnQuery.ExecuteQuery(sqlQuery);

    //            //Delete All Part Rack Loc
    //            sqlQuery = "UPDATE ais_PartsNum SET rack_det_id = NULL, rack_loc = NULL WHERE rack_det_id LIKE '" + strRackName + "^%'";
    //            Boolean blDelPartsNum = ConnQuery.ExecuteQuery(sqlQuery);
    //        }
    //    }

    //    //Delete All Rack
    //    sqlQuery = "DELETE FROM dt_RackMst WHERE proc_name = '" + strProcName + "' AND plc_no = '" + strPlcNo + "'";
    //    Boolean blDelRack = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Lamp Module Address
    //    sqlQuery = "DELETE FROM dt_LampModuleAddMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    Boolean blDelLmAdd = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Lamp Module Mode Conversion
    //    sqlQuery = "DELETE FROM conv_LampModuleMode WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    Boolean blDelLmMode = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Instruction Code Conversion
    //    sqlQuery = "DELETE FROM conv_InsCode WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    Boolean blDelInsCodeConv = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Instruction Code Map Conversion
    //    sqlQuery = "DELETE FROM conv_InsCodeMap WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    Boolean blDelInsCodeMap = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Block Conversion
    //    sqlQuery = "DELETE FROM conv_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    Boolean blUpdBlockConv = ConnQuery.ExecuteQuery(sqlQuery);
    //}

    public static Boolean SvPlcMst(String strPlcNo, String strProcName, String strIpAdd, String strPlcModel, String strPlcNwStation, String strPlcLogicalStation, String strEnable, String strCurUser)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strIpAdd = strIpAdd.Replace("'", "''");
        strPlcModel = strPlcModel.Replace("'", "''");           //Added by YanTeng 10/09/2020
        strPlcNwStation = strPlcNwStation.Replace("'", "''");
        strPlcLogicalStation = strPlcLogicalStation.Replace("'", "''");
        strEnable = strEnable.Replace("'", "''");
        strCurUser = strCurUser.Replace("'", "''");

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("INSERT INTO dt_DpsPlcMst (plc_no, proc_name, ip_add, plc_model, plc_nw_station, plc_logical_station_no, enable, last_upd_by, last_upd_dt) VALUES ('" + strPlcNo + "', '" + strProcName + "','" + strIpAdd + "','" + strPlcModel + "','" + strPlcNwStation + "','" + strPlcLogicalStation + "','" + strEnable + "','" + strCurUser + "',CURRENT_TIMESTAMP)");

        String sqlChkQuery = "SELECT COUNT(ins_code_cnt) AS CountRow FROM conv_InsCode WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
        if (!ConnQuery.chkExistData(sqlChkQuery))
        {
            for (int iInsCodeCnt = 1; iInsCodeCnt <= 299; iInsCodeCnt++)
            {
                sqlQuery.Add("INSERT INTO conv_InsCode (plc_no, proc_name, ins_code_cnt) VALUES ('" + strPlcNo + "', '" + strProcName + "', '" + iInsCodeCnt + "')");
            }
        }

        sqlChkQuery = "SELECT COUNT(ins_code_cnt) AS CountRow FROM conv_InsCodeMap WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
        if (!ConnQuery.chkExistData(sqlChkQuery))
        {
            for (int iInsCodeCnt = 1; iInsCodeCnt <= 299; iInsCodeCnt++)
            {
                sqlQuery.Add("INSERT INTO conv_InsCodeMap (plc_no, proc_name, ins_code_cnt) VALUES ('" + strPlcNo + "', '" + strProcName + "', '" + iInsCodeCnt + "')");
            }
        }

        for (int iGwNo = 1; iGwNo <= 12; iGwNo++)
        {
            sqlChkQuery = "SELECT COUNT(mode_cnt) AS CountRow FROM conv_LampModuleMode WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + iGwNo + "'";
            if (!ConnQuery.chkExistData(sqlChkQuery))
            {
                for (int iModeCnt = 1; iModeCnt <= 32; iModeCnt++)
                {
                    sqlQuery.Add("INSERT INTO conv_LampModuleMode (plc_no, proc_name, gw_no, mode_cnt) VALUES ('" + strPlcNo + "', '" + strProcName + "', '" + iGwNo + "', '" + iModeCnt + "')");
                }
            }
        }

        for (int iGwNo = 1; iGwNo <= 12; iGwNo++)
        {
            sqlChkQuery = "SELECT COUNT(gw_no) AS CountRow FROM conv_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + iGwNo + "'";
            if (!ConnQuery.chkExistData(sqlChkQuery))
            {
                sqlQuery.Add("INSERT INTO conv_BlockMst (plc_no, proc_name, gw_no) VALUES ('" + strPlcNo + "', '" + strProcName + "', '" + iGwNo + "')");
            }
        }

        sqlChkQuery = "SELECT COUNT(plc_no) AS CountRow FROM dt_PlcPointerMst WHERE plc_no = '" + strPlcNo + "'";
        if (ConnQuery.chkExistData(sqlChkQuery))
        {
            sqlQuery.Add("UPDATE dt_PlcPointerMst SET pointer = '1' WHERE plc_no = '" + strPlcNo + "'");
        }
        else
        {
            sqlQuery.Add("INSERT INTO dt_PlcPointerMst (plc_no, flag_type, pointer) VALUES ('" + strPlcNo + "', 'R', '1');INSERT INTO dt_PlcPointerMst (plc_no, flag_type, pointer) VALUES ('" + strPlcNo + "', 'W', '1');");
        }

        try
        {
            return ConnQuery.ExecuteTransSaveQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdPlcMst(String strPlcNo, String strProcName, String strIpAdd, String strPlcModel, String strPlcNwStation, String strPlcLogicalStation, String strEnable, String tempPlcNo, String strCurUser)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strIpAdd = strIpAdd.Replace("'", "''");
        strPlcModel = strPlcModel.Replace("'", "''");           //Added by YanTeng 11/09/2020
        strPlcNwStation = strPlcNwStation.Replace("'", "''");
        strPlcLogicalStation = strPlcLogicalStation.Replace("'", "''");
        strEnable = strEnable.Replace("'", "''");
        strCurUser = strCurUser.Replace("'", "''");

        List<String> sqlQuery = new List<String>();

        //Modified by YanTeng 11/09/2020 (Added plc_model)
        sqlQuery.Add("UPDATE dt_DpsPlcMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', ip_add = '" + strIpAdd + "', plc_model = '" + strPlcModel + "', plc_nw_station = '" + strPlcNwStation + "', plc_logical_station_no = '" + strPlcLogicalStation + "', enable = '" + strEnable + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + tempPlcNo + "'");

        //Update All Group
        sqlQuery.Add("UPDATE dt_GroupMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + tempPlcNo + "'");

        //Update All Block
        sqlQuery.Add("UPDATE dt_BlockMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + tempPlcNo + "'");

        //Update All Rack
        sqlQuery.Add("UPDATE dt_RackMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + tempPlcNo + "'");

        //Update All Rack Det
        sqlQuery.Add("UPDATE dt_RackMstDet SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + tempPlcNo + "'");

        //Update All Lamp Module Address
        sqlQuery.Add("UPDATE dt_LampModuleAddMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + tempPlcNo + "'");

        //Update All Block Conversion
        sqlQuery.Add("UPDATE conv_BlockMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "' WHERE plc_no = '" + tempPlcNo + "'");

        //Update All Lamp Module Mode Conversion
        sqlQuery.Add("UPDATE conv_LampModuleMode SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "' WHERE plc_no = '" + tempPlcNo + "'");

        //Update All Instruction Code Conversion
        sqlQuery.Add("UPDATE conv_InsCode SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "' WHERE plc_no = '" + tempPlcNo + "'");

        //Update All Instruction Code Map Conversion
        sqlQuery.Add("UPDATE conv_InsCodeMap SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "' WHERE plc_no = '" + tempPlcNo + "'");

        //Update Pointer
        sqlQuery.Add("UPDATE dt_PlcPointerMst SET plc_no = '" + strPlcNo + "' WHERE plc_no = '" + tempPlcNo + "'");
        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static void UpdPlcAllRem(String strPlcNo, String strProcName, String tempPlcNo, String strCurUser)
    //{
    //    String sqlQuery = "";

    //    //Update All Group
    //    sqlQuery = "UPDATE dt_GroupMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + tempPlcNo + "'";
    //    Boolean blUpdGroup = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Block
    //    sqlQuery = "UPDATE dt_BlockMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + tempPlcNo + "'";
    //    Boolean blUpdBlock = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Rack
    //    sqlQuery = "UPDATE dt_RackMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + tempPlcNo + "'";
    //    Boolean blUpdRack = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Rack Det
    //    sqlQuery = "DELETE FROM dt_RackMstDet SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + tempPlcNo + "'";
    //    Boolean blDelRackDet = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Lamp Module Address
    //    sqlQuery = "UPDATE dt_LampModuleAddMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + tempPlcNo + "'";
    //    Boolean blUpdLmAdd = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Block Conversion
    //    sqlQuery = "UPDATE conv_BlockMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "' WHERE plc_no = '" + tempPlcNo + "'";
    //    Boolean blUpdBlockConv = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Lamp Module Mode Conversion
    //    sqlQuery = "UPDATE conv_LampModuleMode SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "' WHERE plc_no = '" + tempPlcNo + "'";
    //    Boolean blUpdLmMode = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Instruction Code Conversion
    //    sqlQuery = "UPDATE conv_InsCode SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "' WHERE plc_no = '" + tempPlcNo + "'";
    //    Boolean blUpdInsCodeConv = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Instruction Code Map Conversion
    //    sqlQuery = "UPDATE conv_InsCodeMap SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "' WHERE plc_no = '" + tempPlcNo + "'";
    //    Boolean blUpdInsCodeMap = ConnQuery.ExecuteQuery(sqlQuery);

    //    sqlQuery = "UPDATE dt_PlcPointerMst SET plc_no = '" + strPlcNo + "' WHERE plc_no = '" + tempPlcNo + "'";
    //    Boolean blUpdPlcPointerMst = ConnQuery.ExecuteQuery(sqlQuery);
    //}

    //public static void SvPlcMstConv(String strPlcNo, String strProcName)
    //{
    //    strPlcNo = strPlcNo.Replace("'", "''");
    //    strProcName = strProcName.Replace("'", "''");

    //    String sqlQuery = "SELECT COUNT(ins_code_cnt) AS CountRow FROM conv_InsCode WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    if (!ConnQuery.chkExistData(sqlQuery))
    //    {
    //        for (int iInsCodeCnt = 1; iInsCodeCnt <= 299; iInsCodeCnt++)
    //        {
    //            sqlQuery = "INSERT INTO conv_InsCode (plc_no, proc_name, ins_code_cnt) VALUES ('" + strPlcNo + "', '" + strProcName + "', '" + iInsCodeCnt + "')";
    //            ConnQuery.ExecuteQuery(sqlQuery);
    //        }
    //    }
    //}

    //public static void SvPlcMstConvMap(String strPlcNo, String strProcName)
    //{
    //    strPlcNo = strPlcNo.Replace("'", "''");
    //    strProcName = strProcName.Replace("'", "''");

    //    String sqlQuery = "SELECT COUNT(ins_code_cnt) AS CountRow FROM conv_InsCodeMap WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    if (!ConnQuery.chkExistData(sqlQuery))
    //    {
    //        for (int iInsCodeCnt = 1; iInsCodeCnt <= 299; iInsCodeCnt++)
    //        {
    //            sqlQuery = "INSERT INTO conv_InsCodeMap (plc_no, proc_name, ins_code_cnt) VALUES ('" + strPlcNo + "', '" + strProcName + "', '" + iInsCodeCnt + "')";
    //            ConnQuery.ExecuteQuery(sqlQuery);
    //        }
    //    }
    //}

    //public static void SvLmMode(String strPlcNo, String strProcName)
    //{
    //    strPlcNo = strPlcNo.Replace("'", "''");
    //    strProcName = strProcName.Replace("'", "''");

    //    for (int iGwNo = 1; iGwNo <= 12; iGwNo++)
    //    {
    //        String sqlQuery = "SELECT COUNT(mode_cnt) AS CountRow FROM conv_LampModuleMode WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + iGwNo + "'";
    //        if (!ConnQuery.chkExistData(sqlQuery))
    //        {
    //            for (int iModeCnt = 1; iModeCnt <= 32; iModeCnt++)
    //            {
    //                sqlQuery = "INSERT INTO conv_LampModuleMode (plc_no, proc_name, gw_no, mode_cnt) VALUES ('" + strPlcNo + "', '" + strProcName + "', '" + iGwNo + "', '" + iModeCnt + "')";
    //                ConnQuery.ExecuteQuery(sqlQuery);
    //            }
    //        }
    //    }
    //}

    //public static void SvBlockMstConv(String strPlcNo, String strProcName)
    //{
    //    strPlcNo = strPlcNo.Replace("'", "''");
    //    strProcName = strProcName.Replace("'", "''");

    //    for (int iGwNo = 1; iGwNo <= 12; iGwNo++)
    //    {
    //        String sqlQuery = "SELECT COUNT(gw_no) AS CountRow FROM conv_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + iGwNo + "'";
    //        if (!ConnQuery.chkExistData(sqlQuery))
    //        {
    //            sqlQuery = "INSERT INTO conv_BlockMst (plc_no, proc_name, gw_no) VALUES ('" + strPlcNo + "', '" + strProcName + "', '" + iGwNo + "')";
    //            ConnQuery.ExecuteQuery(sqlQuery);
    //        }
    //    }
    //}

    public static Boolean ChkPlcMstMaxCnt()
    {
        String sqlQuery = "SELECT COUNT(plc_no) AS ReturnField FROM dt_DpsPlcMst";
        try
        {
            int iInsCodeCnt = Convert.ToInt32(ConnQuery.getReturnFieldExecuteReader(sqlQuery));
            if (iInsCodeCnt >= 12)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static Boolean ResetPointer(String strPlcNo)
    //{
    //    String sqlQuery = "SELECT COUNT(plc_no) AS CountRow FROM dt_PlcPointerMst WHERE plc_no = '" + strPlcNo + "'";
    //    if (ConnQuery.chkExistData(sqlQuery))
    //    {
    //        sqlQuery = "UPDATE dt_PlcPointerMst SET pointer = '1' WHERE plc_no = '" + strPlcNo + "'";
    //    }
    //    else
    //    {
    //        sqlQuery = "INSERT INTO dt_PlcPointerMst (plc_no, flag_type, pointer) VALUES ('" + strPlcNo + "', 'R', '1');INSERT INTO dt_PlcPointerMst (plc_no, flag_type, pointer) VALUES ('" + strPlcNo + "', 'W', '1');";
    //    }
    //    try
    //    {
    //        return ConnQuery.ExecuteQuery(sqlQuery);
    //    }
    //    catch (Exception ex)
    //    {
    //        GlobalFunc.Log(ex);
    //        GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
    //        return false;
    //    }
    //}

    public static Boolean ChkDuplicateIpAdd(String strIpAdd, String strPlcNo)
    {
        strIpAdd = strIpAdd.Replace("'", "''");

        String sqlQuery = "SELECT COUNT(ip_add) AS CountRow FROM dt_DpsPlcMst WHERE ip_add = '" + strIpAdd + "' AND plc_no != '" + strPlcNo + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return true;
        }
    }

    public static Boolean ChkDuplicatePlcLsn(String strPlcLsn, String strPlcNo)
    {
        strPlcLsn = strPlcLsn.Replace("'", "''");

        String sqlQuery = "SELECT COUNT(plc_logical_station_no) AS CountRow FROM dt_DpsPlcMst WHERE plc_logical_station_no = '" + strPlcLsn + "' AND plc_no != '" + strPlcNo + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return true;
        }
    }

    public static Boolean ChkDuplicatePlcNo(String strPlcNoNew, String strPlcNo)
    {
        strPlcNoNew = strPlcNoNew.Replace("'", "''");

        String sqlQuery = "SELECT COUNT(plc_no) AS CountRow FROM dt_DpsPlcMst WHERE plc_no = '" + strPlcNoNew + "' AND plc_no != '" + strPlcNo + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return true;
        }
    }


    public static Boolean ChkDuplicateProcName(String strProcName, String strPlcNo)
    {
        strProcName = strProcName.Replace("'", "''");

        String sqlQuery = "SELECT COUNT(proc_name) AS CountRow FROM dt_DpsPlcMst WHERE proc_name = '" + strProcName + "' AND plc_no != '" + strPlcNo + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return true;
        }
    }

    public static String GetProcName(String strPlcNo)
    {
        strPlcNo = strPlcNo.Replace("'", "''");

        String sqlQuery = "SELECT proc_name AS ReturnField FROM dt_DpsPlcMst WHERE plc_no = '" + strPlcNo + "'";
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    public static String GetPlcNo(String strProcName)
    {
        strProcName = strProcName.Replace("'", "''");

        String sqlQuery = "SELECT plc_no AS ReturnField FROM dt_DpsPlcMst WHERE proc_name = '" + strProcName + "'";
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    public static String GetLsn(String strPlcNo)
    {
        strPlcNo = strPlcNo.Replace("'", "''");

        String sqlQuery = "SELECT plc_logical_station_no AS ReturnField FROM dt_DpsPlcMst WHERE plc_no = '" + strPlcNo + "'";
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }
    #endregion

    #endregion

    #region DPS Maintenance Entry

    #region Import Parts Number
    public static DataSet SrcPartsNumList()
    {
        String sqlQuery = "SELECT * FROM ais_PartsNum WHERE part_no != '' ORDER BY part_no";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static String GetPartsBgColor(String strPartNum, String strColorSfx)
    {
        String sqlQuery = "SELECT CASE WHEN r = '0' AND g = '0' AND b = '0' THEN 'Black' ELSE 'White' END AS ReturnField FROM ais_PartsNum WHERE part_no = '" + strPartNum + "' AND color_sfx = '" + strColorSfx + "' ORDER BY part_no";
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    public static String GetPartsFontColor(String strPartNum, String strColorSfx)
    {
        String sqlQuery = "SELECT CASE WHEN r = '0' AND g = '0' AND b = '0' THEN 'White' ELSE 'Black' END AS ReturnField FROM ais_PartsNum WHERE part_no = '" + strPartNum + "' AND color_sfx = '" + strColorSfx + "' ORDER BY part_no";
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    public static Boolean ImpPartNumber()
    {
        String sqlQuery = "SELECT PartNo, PartName, PartType, ColorSfx, SymbolCode, Symbol, R, G, B FROM vw_PartMst";

        DataSet dsSearch = new DataSet();
        DataTable dtSearch = new DataTable();

        dsSearch = ConnQuery.getPisBindingDatasetData(sqlQuery);
        dtSearch = dsSearch.Tables[0];

        for (int i = 0; i < dtSearch.Rows.Count; i++)
        {
            String strPartNo = Convert.ToString(dtSearch.Rows[i]["PartNo"]);
            String strPartName = Convert.ToString(dtSearch.Rows[i]["PartName"]);
            String strPartType = Convert.ToString(dtSearch.Rows[i]["PartType"]);
            String strColorSfx = Convert.ToString(dtSearch.Rows[i]["ColorSfx"]);
            String strSymbolCode = Convert.ToString(dtSearch.Rows[i]["SymbolCode"]);
            String strSymbol = Convert.ToString(dtSearch.Rows[i]["Symbol"]);
            String strR = Convert.ToString(dtSearch.Rows[i]["R"]);
            String strG = Convert.ToString(dtSearch.Rows[i]["G"]);
            String strB = Convert.ToString(dtSearch.Rows[i]["B"]);

            sqlQuery = "SELECT COUNT(part_no) AS CountRow FROM ais_PartsNum WHERE part_no = '" + strPartNo + "' AND color_sfx = '" + strColorSfx + "'";
            if (!ConnQuery.chkExistData(sqlQuery))
            {
                sqlQuery = "INSERT INTO ais_PartsNum (part_no, part_name, part_type, color_sfx, symbol_code, symbol, r, g, b) VALUES ('" + strPartNo + "','" + strPartName + "','" + strPartType + "','" + strColorSfx + "','" + strSymbolCode + "','" + strSymbol + "','" + strR + "','" + strG + "','" + strB + "')";
            }
            else
            {
                sqlQuery = "UPDATE ais_PartsNum SET part_no = '" + strPartNo + "', part_name = '" + strPartName + "', part_type = '" + strPartType + "', color_sfx = '" + strColorSfx + "', symbol_code = '" + strSymbolCode + "', symbol = '" + strSymbol + "', r = '" + strR + "', g = '" + strG + "', b = '" + strB + "' WHERE part_no = '" + strPartNo + "' AND color_sfx = '" + strColorSfx + "'";
            }
            try
            {
                ConnQuery.ExecuteQuery(sqlQuery);
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
                return false;
            }
        }
        return true;
    }
    #endregion

    #region Import Harigami/Jundate Data
    public static DataSet SrcDataHJList(String strAisType, String strAisItemId, String strRevNo)
    {
        String filterCriteria = "";

        if (strAisType != "")
        {
            if (strAisType == "Jundate")
            {
                filterCriteria = filterCriteria + " AND row IS NULL";
            }
            else
            {
                filterCriteria = filterCriteria + " AND row IS NOT NULL";
            }
        }
        if (strAisItemId != "")
        {
            //filterCriteria = filterCriteria + " AND item_id = '" + strAisItemId + "'";
            String[] tmpAisCond = strAisItemId.Split('~');
            int tmpAisCondCnt = tmpAisCond.Length;
            if (tmpAisCondCnt == 3)
            {
                filterCriteria = filterCriteria + " AND name = '" + tmpAisCond[0].Trim() + "' AND sfx = '" + tmpAisCond[1].Trim() + "' AND color = '" + tmpAisCond[2].Trim() + "'";
            }
        }
        if (strRevNo != "")
        {
            filterCriteria = filterCriteria + " AND rev_no = '" + strRevNo + "'";
        }


        String sqlQuery = "SELECT * FROM ais_DataHJ WHERE id != '' " + filterCriteria + " ORDER BY id, item_id, name, row, col, part_no, color_sfx";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean ImpHjData(String strAisType, String strAisItemId)
    {
        String sqlQuery = "";

        if (strAisType == "Jundate")
        {
            sqlQuery = "SELECT * FROM vw_Jundate";

            if (strAisItemId != "ALL")
            {
                String[] tmpAisCond = strAisItemId.Split('~');
                int tmpAisCondCnt = tmpAisCond.Length;
                if (tmpAisCondCnt == 3)
                {
                    sqlQuery = sqlQuery + " WHERE JdtName = '" + tmpAisCond[0].Trim() + "' AND Sfx = '" + tmpAisCond[1].Trim() + "' AND ColorCode = '" + tmpAisCond[2].Trim() + "'";
                }
            }

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = ConnQuery.getPisBindingDatasetData(sqlQuery);
            dtSearch = dsSearch.Tables[0];

            sqlQuery = "DELETE FROM ais_DataHj WHERE row IS NULL";

            if (strAisItemId != "ALL")
            {
                //sqlQuery = sqlQuery + " AND item_id = '" + strAisItemId + "'";
                String[] tmpAisCond = strAisItemId.Split('~');
                int tmpAisCondCnt = tmpAisCond.Length;
                if (tmpAisCondCnt == 3)
                {
                    sqlQuery = sqlQuery + " AND name = '" + tmpAisCond[0].Trim() + "' AND sfx = '" + tmpAisCond[1].Trim() + "' AND color = '" + tmpAisCond[2].Trim() + "'";
                }
            }
            ConnQuery.ExecuteQuery(sqlQuery);

            for (int i = 0; i < dtSearch.Rows.Count; i++)
            {
                String strJdtID = Convert.ToString(dtSearch.Rows[i]["JdtID"]);
                String strJdtItmID = Convert.ToString(dtSearch.Rows[i]["JdtItmID"]);
                String strJdtName = Convert.ToString(dtSearch.Rows[i]["JdtName"]);
                String strColNo = Convert.ToString(dtSearch.Rows[i]["ColNo"]);
                String strPartTitle = Convert.ToString(dtSearch.Rows[i]["PartTitle"]);
                String strPartNo = Convert.ToString(dtSearch.Rows[i]["PartNo"]);
                String strColorSfx = Convert.ToString(dtSearch.Rows[i]["ColorSfx"]);
                String strModelCode = Convert.ToString(dtSearch.Rows[i]["ModelCode"]);
                String strKtsk = Convert.ToString(dtSearch.Rows[i]["Ktsk"]);
                String strSfx = Convert.ToString(dtSearch.Rows[i]["Sfx"]);
                String strColorCode = Convert.ToString(dtSearch.Rows[i]["ColorCode"]);
                String strRevNo = Convert.ToString(dtSearch.Rows[i]["RevNo"]);

                sqlQuery = "INSERT INTO ais_DataHj (id, item_id, rev_no, name, col, parts_title, part_no, color_sfx, model, katashiki, sfx, color) VALUES ('" + strJdtID + "','" + strJdtItmID + "','" + strRevNo + "','" + strJdtName + "','" + strColNo + "','" + strPartTitle + "','" + strPartNo + "','" + strColorSfx + "','" + strModelCode + "','" + strKtsk + "','" + strSfx + "','" + strColorCode + "')";
                try
                {
                    ConnQuery.ExecuteQuery(sqlQuery);
                }
                catch (Exception ex)
                {
                    GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
                    return false;
                }
            }
        }
        else if (strAisType == "Harigami")
        {
            sqlQuery = "SELECT * FROM vw_Harigami WHERE HrgmName LIKE '%DPS%' ";

            if (strAisItemId != "ALL")
            {
                String[] tmpAisCond = strAisItemId.Split('~');
                int tmpAisCondCnt = tmpAisCond.Length;
                if (tmpAisCondCnt == 3)
                {
                    sqlQuery = sqlQuery + " AND HrgmName = '" + tmpAisCond[0].Trim() + "' AND Sfx = '" + tmpAisCond[1].Trim() + "' AND ColorCode = '" + tmpAisCond[2].Trim() + "'";
                }
            }

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = ConnQuery.getPisBindingDatasetData(sqlQuery);
            dtSearch = dsSearch.Tables[0];

            sqlQuery = "DELETE FROM ais_DataHj WHERE row IS NOT NULL";

            if (strAisItemId != "ALL")
            {
                //sqlQuery = sqlQuery + " AND item_id = '" + strAisItemId + "'";
                String[] tmpAisCond = strAisItemId.Split('~');
                int tmpAisCondCnt = tmpAisCond.Length;
                if (tmpAisCondCnt == 3)
                {
                    sqlQuery = sqlQuery + " AND name = '" + tmpAisCond[0].Trim() + "' AND sfx = '" + tmpAisCond[1].Trim() + "' AND color = '" + tmpAisCond[2].Trim() + "'";
                }
            }
            ConnQuery.ExecuteQuery(sqlQuery);

            for (int i = 0; i < dtSearch.Rows.Count; i++)
            {
                String strHrgmID = Convert.ToString(dtSearch.Rows[i]["HrgmID"]);
                String strHrgmItmID = Convert.ToString(dtSearch.Rows[i]["HrgmItmID"]);
                String strHrgmName = Convert.ToString(dtSearch.Rows[i]["HrgmName"]);
                String strColNo = Convert.ToString(dtSearch.Rows[i]["ColPos"]);
                String strRowNo = Convert.ToString(dtSearch.Rows[i]["RowNo"]);
                String strPartTitle = Convert.ToString(dtSearch.Rows[i]["PartTitle"]);
                String strPartNo = Convert.ToString(dtSearch.Rows[i]["PartNo"]);
                String strColorSfx = Convert.ToString(dtSearch.Rows[i]["ColorSfx"]);
                String strModelCode = Convert.ToString(dtSearch.Rows[i]["ModelCode"]);
                String strKtsk = Convert.ToString(dtSearch.Rows[i]["Ktsk"]);
                String strSfx = Convert.ToString(dtSearch.Rows[i]["Sfx"]);
                String strColorCode = Convert.ToString(dtSearch.Rows[i]["ColorCode"]);
                String strRevNo = Convert.ToString(dtSearch.Rows[i]["RevNo"]);

                sqlQuery = "INSERT INTO ais_DataHj (id, item_id, rev_no, name, col, row, parts_title, part_no, color_sfx, model, katashiki, sfx, color) VALUES ('" + strHrgmID + "','" + strHrgmItmID + "','" + strRevNo + "','" + strHrgmName + "','" + strColNo + "','" + strRowNo + "','" + strPartTitle + "','" + strPartNo + "','" + strColorSfx + "','" + strModelCode + "','" + strKtsk + "','" + strSfx + "','" + strColorCode + "')";
                try
                {
                    ConnQuery.ExecuteQuery(sqlQuery);
                }
                catch (Exception ex)
                {
                    GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
                    return false;
                }
            }
        }
        return true;
    }
    #endregion

    #region Physical Address Master
    public static DataSet SrcPhysicalAddMst(String strPlcNo, String strProcName, String strModuleAddFrm, String strModuleAddTo, String strPhyAddFrm, String strPhyAddTo)
    {
        String filterCriteria = "";

        if (strPlcNo != "")
        {
            filterCriteria = filterCriteria + " AND B.plc_no = '" + strPlcNo + "'";
        }
        if (strProcName != "")
        {
            filterCriteria = filterCriteria + " AND B.proc_name = '" + strProcName + "'";
        }
        if (strModuleAddFrm != "" && strModuleAddTo != "")
        {
            filterCriteria = filterCriteria + " AND module_add >= " + strModuleAddFrm + " AND module_add <= " + strModuleAddTo + "";
        }
        else if (strModuleAddFrm != "")
        {
            filterCriteria = filterCriteria + " AND module_add = '" + strModuleAddFrm + "'";
        }
        else if (strModuleAddTo != "")
        {
            filterCriteria = filterCriteria + " AND module_add = '" + strModuleAddTo + "'";
        }
        if (strPhyAddFrm != "" && strPhyAddTo != "")
        {
            filterCriteria = filterCriteria + " AND physical_add >= " + strPhyAddFrm + " AND physical_add <= " + strPhyAddTo + "";
        }
        else if (strPhyAddFrm != "")
        {
            filterCriteria = filterCriteria + " AND physical_add = '" + strPhyAddFrm + "'";
        }
        else if (strPhyAddTo != "")
        {
            filterCriteria = filterCriteria + " AND physical_add = '" + strPhyAddTo + "'";
        }

        String sqlQuery = "SELECT A.plc_no, A.plc_model, B.* FROM dt_DpsPlcMst A INNER JOIN dt_PhysicalAddMst B ON A.proc_name = B.proc_name WHERE module_add != ''" + filterCriteria + " ORDER BY A.plc_no, module_add, physical_add";
        //String sqlQuery = "SELECT A.plc_no, A.proc_name, A.plc_model, B.module_add, B.physical_add, B.last_upd_by, B.last_upd_dt FROM dt_DpsPlcMst A INNER JOIN dt_PhysicalAddMst B ON A.proc_name = B.proc_name WHERE module_add != ''" + filterCriteria + " ORDER BY module_add, physical_add";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean DelPhysicalAddMst(String strPhysicalUid)
    {
        String sqlQuery = "DELETE FROM dt_PhysicalAddMst WHERE uid = '" + strPhysicalUid + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean SvPhysicalAddMst(String strPlcNo, String strProcName, String strModuleAdd, String strPhysicalAdd, String strCurUser)
    {
        strProcName = strProcName.Replace("'", "''");
        strModuleAdd = strModuleAdd.Replace("'", "''");
        strPhysicalAdd = strPhysicalAdd.Replace("'", "''");

        String sqlQuery = "INSERT INTO dt_PhysicalAddMst (plc_no, proc_name, module_add, physical_add, last_upd_by, last_upd_dt) VALUES ('" + strPlcNo + "','" + strProcName + "','" + strModuleAdd + "','" + strPhysicalAdd + "','" + strCurUser + "', CURRENT_TIMESTAMP)";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdPhysicalAddMst(String strPhysicalUid, String strPlcNo, String strProcName, String strModuleAdd, String strPhysicalAdd, String strCurUser)
    {
        strPhysicalUid = strPhysicalUid.Replace("'", "''");
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strModuleAdd = strModuleAdd.Replace("'", "''");
        strPhysicalAdd = strPhysicalAdd.Replace("'", "''");

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("UPDATE dt_PhysicalAddMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', module_add = '" + strModuleAdd + "', physical_add = '" + strPhysicalAdd + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE uid = '" + strPhysicalUid + "'");

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static DataSet GetPhysicalAddbyPlcModel(string strPlcModel)
    {
        String sqlQuery = "SELECT phyaddr_from, phyaddr_to, digit_no, enable FROM dt_PlcModelMst WHERE plcmodel_no = '" + strPlcModel + "'";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean ChkDuplicatePhysicalAddMst(String tempPhysicalUid, String strProcName, String strModuleAddFrm, String strModuleAddTo)
    {
        String sqlQuery = "";

        if (strModuleAddTo == "")
        {
            sqlQuery = "SELECT COUNT(uid) AS CountRow FROM dt_PhysicalAddMst WHERE uid != '" + tempPhysicalUid + "' AND proc_name = '" + strProcName + "' AND module_add = '" + strModuleAddFrm + "'";
        }
        else
        {
            sqlQuery = "SELECT COUNT(uid) AS CountRow FROM dt_PhysicalAddMst WHERE uid != '" + tempPhysicalUid + "' AND proc_name = '" + strProcName + "' AND (module_add >= '" + strModuleAddFrm + "' AND module_add <= '" + strModuleAddTo + "')";
        }

        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region Lamp Module Type Master
    public static DataSet SrcLampModuleTypeMst(String strModuleType, String strEquipType, String strLightDI, String strColorDI, String strLightAI, String strColorAI)
    {
        String filterCriteria = "";

        if (strModuleType != "")
        {
            filterCriteria = filterCriteria + " AND module_type = '" + strModuleType + "'";
        }
        if (strEquipType != "")
        {
            filterCriteria = filterCriteria + " AND equip_type = '" + strEquipType + "'";
        }
        if (strLightDI != "")
        {
            filterCriteria = filterCriteria + " AND light_di = '" + strLightDI + "'";
        }
        if (strColorDI != "")
        {
            filterCriteria = filterCriteria + " AND color_di = '" + strColorDI + "'";
        }
        if (strLightAI != "")
        {
            filterCriteria = filterCriteria + " AND light_ai = '" + strLightAI + "'";
        }
        if (strColorAI != "")
        {
            filterCriteria = filterCriteria + " AND color_ai = '" + strColorAI + "'";
        }

        String sqlQuery = "SELECT * FROM dt_LampModuleTypeMst WHERE module_type != '' " + filterCriteria + " ORDER BY equip_type, module_type";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean DelLampModuleTypeMst(String strModuleType)
    {
        String sqlQuery = "DELETE FROM dt_LampModuleTypeMst WHERE module_type = '" + strModuleType + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkLampModuleTypeUsed(String strModuleType)
    {
        String sqlQuery = "SELECT COUNT(module_type) AS CountRow FROM dt_LampModuleAddMst WHERE module_type = '" + strModuleType + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean SvLampModuleTypeMst(String strModuleType, String strEquipType, String strLightDI, String strLightAI, String strColorDI, String strColorAI, String strCurUser)
    {
        strModuleType = strModuleType.Replace("'", "''");
        strEquipType = strEquipType.Replace("'", "''");
        strLightDI = strLightDI.Replace("'", "''");
        strLightAI = strLightAI.Replace("'", "''");
        strColorDI = strColorDI.Replace("'", "''");
        strColorAI = strColorAI.Replace("'", "''");

        String sqlQuery = "INSERT INTO dt_LampModuleTypeMst (module_type, equip_type, light_di, color_di, light_ai, color_ai, last_upd_by, last_upd_dt) VALUES ('" + strModuleType + "', '" + strEquipType + "','" + strLightDI + "','" + strColorDI + "','" + strLightAI + "','" + strColorAI + "','" + strCurUser + "', CURRENT_TIMESTAMP)";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdLampModuleTypeMst(String strModuleType, String strEquipType, String strLightDI, String strLightAI, String strColorDI, String strColorAI, String strTempModuleName, String strCurUser)
    {
        strModuleType = strModuleType.Replace("'", "''");
        strEquipType = strEquipType.Replace("'", "''");
        strLightDI = strLightDI.Replace("'", "''");
        strLightAI = strLightAI.Replace("'", "''");
        strColorDI = strColorDI.Replace("'", "''");
        strColorAI = strColorAI.Replace("'", "''");

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("UPDATE dt_LampModuleTypeMst SET module_type = '" + strModuleType + "', equip_type = '" + strEquipType + "', light_di = '" + strLightDI + "', light_ai = '" + strLightAI + "', color_di = '" + strColorDI + "', color_ai = '" + strColorAI + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE module_type = '" + strTempModuleName + "'");

        //Update All Lamp Module Address
        sqlQuery.Add("UPDATE dt_LampModuleAddMst SET module_type = '" + strModuleType + "' WHERE module_type = '" + strTempModuleName + "'");

        //Update All Lamp Module Mode Conversion
        sqlQuery.Add("UPDATE conv_LampModuleModeRack SET module_type = '" + strModuleType + "' WHERE module_type = '" + strTempModuleName + "'");
        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static void UpdLampModuleTypeRem(String strModuleType, String strTempModuleType)
    //{
    //    String sqlQuery = "";

    //    //Update All Lamp Module Address
    //    sqlQuery = "UPDATE dt_LampModuleAddMst SET module_type = '" + strModuleType + "' WHERE module_type = '" + strTempModuleType + "'";
    //    Boolean blUpdLmAdd = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Lamp Module Mode Conversion
    //    sqlQuery = "UPDATE conv_LampModuleModeRack SET module_type = '" + strModuleType + "' WHERE module_type = '" + strTempModuleType + "'";
    //    Boolean blUpdLmMode = ConnQuery.ExecuteQuery(sqlQuery);
    //}

    public static Boolean ChkDuplicateLampModuleType(String strCurModuleType, String strModuleType)
    {
        String sqlQuery = "SELECT COUNT(module_type) AS CountRow FROM dt_LampModuleTypeMst WHERE module_type = '" + strCurModuleType + "' AND module_type != '" + strModuleType + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region Lamp Module Address Master
    public static DataSet SrcLampModuleAddMst(String strPlcModel, String strModuleAddFrm, String strModuleAddTo, String strModuleName, String strModuleType, String strPlcNo, String strProcName)
    {
        String filterCriteria = "";

        if (strPlcModel != "")              //Added by YanTeng 14/09/2020
        {
            filterCriteria = filterCriteria + " AND PlcMst.plc_model = '" + strPlcModel + "'";
        }
        if (strModuleAddFrm != "" && strModuleAddTo != "")
        {
            filterCriteria = filterCriteria + " AND LmAdd.module_add >= " + strModuleAddFrm + " AND LmAdd.module_add <= " + strModuleAddTo + "";
        }
        else if (strModuleAddFrm != "")
        {
            filterCriteria = filterCriteria + " AND LmAdd.module_add = '" + strModuleAddFrm + "'";
        }
        else if (strModuleAddTo != "")
        {
            filterCriteria = filterCriteria + " AND LmAdd.module_add = '" + strModuleAddTo + "'";
        }
        if (strModuleName != "")
        {
            filterCriteria = filterCriteria + " AND LmAdd.module_name = '" + strModuleName + "'";
        }
        if (strModuleType != "")
        {
            filterCriteria = filterCriteria + " AND LmAdd.module_type = '" + strModuleType + "'";
        }
        if (strPlcNo != "")
        {
            filterCriteria = filterCriteria + " AND LmAdd.plc_no = '" + strPlcNo + "'";
        }
        if (strProcName != "")
        {
            filterCriteria = filterCriteria + " AND LmAdd.proc_name = '" + strProcName + "'";
        }

        //Modified by YanTeng 14/09/2020
        //String sqlQuery = "SELECT * FROM dt_LampModuleAddMst WHERE module_add != '' " + filterCriteria + " ORDER BY plc_no, proc_name, len(module_add), module_add";
        //String sqlQuery = "SELECT A.plc_model, B.* FROM dt_DpsPlcMst A INNER JOIN dt_LampModuleAddMst B ON A.plc_no = B.plc_no WHERE B.module_add != '' " + filterCriteria + " ORDER BY B.plc_no, B.proc_name, len(B.module_add), B.module_add";
        String sqlQuery = "SELECT PlcMst.plc_model, LmAdd.*, PhyAdd.physical_add FROM dt_DpsPlcMst PlcMst INNER JOIN dt_LampModuleAddMst LmAdd ON PlcMst.plc_no = LmAdd.plc_no LEFT JOIN dt_PhysicalAddMst PhyAdd ON LmAdd.proc_name = PhyAdd.proc_name AND LmAdd.module_add = PhyAdd.module_add WHERE LmAdd.module_add != '' " + filterCriteria + " ORDER BY LmAdd.plc_no, LmAdd.proc_name, len(LmAdd.module_add), LmAdd.module_add ";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean DelLampModuleAddMst(String strModuleProcName, String strModuleAdd, String strCurUser)  //***ace_20160416_001
    {
        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("DELETE FROM dt_LampModuleAddMst WHERE module_add = '" + strModuleAdd + "' AND proc_name = '" + strModuleProcName + "'");

        //Delete All Lamp Module Address Matching
        sqlQuery.Add("UPDATE conv_LampModuleAddMatching SET lm_add = NULL WHERE lm_add = '" + strModuleAdd + "' and proc_name = '" + strModuleProcName + "'");

        //Delete All Rack Master Detail
        //sqlQuery.Add("UPDATE dt_RackMstDet SET module_add = NULL, module_name = NULL WHERE module_add = '" + strModuleAdd + "'");  //***ace_20160416_001
        sqlQuery.Add("UPDATE dt_RackMstDet SET module_add = NULL, module_name = NULL, last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE module_add = '" + strModuleAdd + "'");
        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean SvLampModuleAddMst(String strPlcNo, String strProcName, String strModuleAdd, String strModuleName, String strModuleType, String strCurUser)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strModuleAdd = strModuleAdd.Replace("'", "''");
        strModuleName = strModuleName.Replace("'", "''");
        strModuleType = strModuleType.Replace("'", "''");

        String sqlQuery = "INSERT INTO dt_LampModuleAddMst (plc_no, proc_name, module_add, module_name, module_type, last_upd_by, last_upd_dt) VALUES ('" + strPlcNo + "', '" + strProcName + "','" + strModuleAdd + "','" + strModuleName + "','" + strModuleType + "','" + strCurUser + "', CURRENT_TIMESTAMP)";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdLampModuleAddMst(String strModuleAdd, String strModuleName, String strPlcNo, String strProcName, String strModuleType, String tempModuleProcName, String tempModuleAdd, String strCurUser)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strModuleAdd = strModuleAdd.Replace("'", "''");
        strModuleName = strModuleName.Replace("'", "''");
        strModuleType = strModuleType.Replace("'", "''");

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("UPDATE dt_LampModuleAddMst SET plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', module_add = '" + strModuleAdd + "', module_name = '" + strModuleName + "', module_type = '" + strModuleType + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE module_add = '" + tempModuleAdd + "' AND proc_name = '" + tempModuleProcName + "'");

        //Update All Lamp Module Address Matching
        sqlQuery.Add("UPDATE conv_LampModuleAddMatching SET lm_add = '" + strModuleAdd + "' WHERE lm_add = '" + tempModuleAdd + "'");

        //Update All Rack Master Detail
        //sqlQuery.Add("UPDATE dt_RackMstDet SET module_add = '" + strModuleAdd + "', module_name = '" + strModuleName + "' WHERE module_add = '" + tempModuleAdd + "'");  //***ace_20160416_001
        sqlQuery.Add("UPDATE dt_RackMstDet SET module_add = '" + strModuleAdd + "', module_name = '" + strModuleName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE module_add = '" + tempModuleAdd + "'");
        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkDuplicateLampModuleAddMst(String ModuleAdd, String ProcName, String tempModuleProcName, String tempModuleAdd, String ModuleAddTo)
    {
        String sqlQuery = "";
        if (ModuleAddTo == "")
        {
            sqlQuery = "SELECT COUNT(module_add) AS CountRow FROM dt_LampModuleAddMst WHERE module_add = '" + ModuleAdd + "' AND module_add != '" + tempModuleAdd + "' AND proc_name = '" + ProcName + "'";
        }
        else
        {
            sqlQuery = "SELECT COUNT(module_add) AS CountRow FROM dt_LampModuleAddMst WHERE (module_add >= " + ModuleAdd + " AND module_add <= " + ModuleAddTo + ") AND proc_name = '" + ProcName + "' AND (module_add != '" + tempModuleAdd + "' AND proc_name != '" + tempModuleProcName + "')";
        }

        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static void UpdLampModuleAddRem(String strModuleAdd, String strModuleName, String tempModuleAdd)
    //{
    //    String sqlQuery = "";

    //    //Update All Lamp Module Address Matching
    //    sqlQuery = "UPDATE conv_LampModuleAddMatching SET lm_add = '" + strModuleAdd + "' WHERE lm_add = '" + tempModuleAdd + "'";
    //    Boolean blUpdLmAdd = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Rack Master Detail
    //    sqlQuery = "UPDATE dt_RackMstDet SET module_add = '" + strModuleAdd + "', module_name = '" + strModuleName + "' WHERE module_add = '" + tempModuleAdd + "'";
    //    Boolean blUpdLmMode = ConnQuery.ExecuteQuery(sqlQuery);
    //}

    //public static void DelLampModuleAddMstRem(String strModuleProcName, String strModuleAdd)
    //{
    //    String sqlQuery = "";

    //    //Delete All Lamp Module Address Matching
    //    sqlQuery = "UPDATE conv_LampModuleAddMatching SET lm_add = NULL WHERE lm_add = '" + strModuleAdd + "' and proc_name = '" + strModuleProcName + "'";
    //    Boolean blUpdLmAdd = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Rack Master Detail
    //    sqlQuery = "UPDATE dt_RackMstDet SET module_add = NULL, module_name = NULL WHERE module_add = '" + strModuleAdd + "'";
    //    Boolean blUpdLmMode = ConnQuery.ExecuteQuery(sqlQuery);
    //}
    #endregion

    #region Group Master
    public static DataSet SrcGroupMst(String strGroupID, String strGroupName, String strPlcNo, String strProcName, String strGroupLine = "")
    {
        String filterCriteria = "";

        if (strGroupID != "")
        {
            filterCriteria = filterCriteria + " AND group_id = '" + strGroupID + "'";
        }
        if (strGroupName != "")
        {
            filterCriteria = filterCriteria + " AND group_name = '" + strGroupName + "'";
        }
        if (strPlcNo != "")
        {
            filterCriteria = filterCriteria + " AND plc_no = '" + strPlcNo + "'";
        }
        if (strProcName != "")
        {
            filterCriteria = filterCriteria + " AND proc_name = '" + strProcName + "'";
        }
        if (strGroupLine != "&nbsp;" && strGroupLine != "")
        {
            filterCriteria = filterCriteria + " AND group_line = '" + strGroupLine + "'";
        }

        String sqlQuery = "SELECT * FROM dt_GroupMst WHERE group_id != '' " + filterCriteria + " ORDER BY plc_no, proc_name, group_id, group_name, group_line";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean DelGroupMst(String strGroupID, String strPlcNo, String strProcName, String strGroupName, String strGroupLine)
    {

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("DELETE FROM dt_GroupMst WHERE group_id = '" + strGroupID + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' " + (strGroupLine == "&nbsp;" ? "" : " AND group_line = '" + strGroupLine + "'"));

        //Delete All Block
        sqlQuery.Add("DELETE FROM dt_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "'");

        //Delete All Rack Detail
        DataSet dsRackMst = new DataSet();
        DataTable dtRackMst = new DataTable();

        dsRackMst = SrcRackMst("", strPlcNo, strProcName, strGroupName, "");
        dtRackMst = dsRackMst.Tables[0];

        if (dtRackMst.Rows.Count > 0)
        {
            for (int iRackCnt = 0; iRackCnt < dtRackMst.Rows.Count; iRackCnt++)
            {
                String strRackName = "";

                if (Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]).Trim() != "")
                {
                    strRackName = Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]);
                }

                sqlQuery.Add("DELETE FROM dt_RackMstDet WHERE rack_name = '" + strRackName + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

                //Delete All Lamp Module Address
                sqlQuery.Add("UPDATE dt_LampModuleAddMst SET rack_det_id = NULL, rack_loc = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND rack_det_id LIKE '" + strRackName + "^%'");

                //Delete All Part Rack Loc
                sqlQuery.Add("UPDATE ais_PartsNum SET rack_det_id = NULL, rack_loc = NULL WHERE rack_det_id LIKE '" + strRackName + "^%'");
            }
        }

        //Delete All Rack
        sqlQuery.Add("DELETE FROM dt_RackMst WHERE proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        //Delete All Group Conversion
        sqlQuery.Add("DELETE FROM conv_GroupMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strGroupID + "' AND group_name = '" + strGroupName + "'");

        //Select All Gateway then clear conv_InsCodeMap , conv_LampModuleAddMatching , conv_LampModuleMode , conv_LampModuleModeRack
        DataSet dsConv = new DataSet();
        DataTable dtConv = new DataTable();

        String strQuery = "SELECT * FROM conv_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strGroupID + "'";
        dsConv = ConnQuery.getBindingDatasetData(strQuery);
        dtConv = dsConv.Tables[0];

        if (dtConv.Rows.Count > 0)
        {
            for (int iRackCnt = 0; iRackCnt < dtConv.Rows.Count; iRackCnt++)
            {
                String tmpGwNo = "";

                if (Convert.ToString(dtConv.Rows[iRackCnt]["gw_no"]).Trim() != "")
                {
                    tmpGwNo = Convert.ToString(dtConv.Rows[iRackCnt]["gw_no"]);
                }

                //Delete Instruction Code Mapping Conversion
                sqlQuery.Add("UPDATE conv_InsCodeMap SET [" + tmpGwNo + "] = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

                //Delete Lamp Module Address Matching Conversation
                sqlQuery.Add("DELETE FROM conv_LampModuleAddMatching WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + tmpGwNo + "'");

                //Delete All Lamp Module Mode
                sqlQuery.Add("UPDATE conv_LampModuleMode SET mode_data = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + tmpGwNo + "'");

                //Delete All Lamp Module Mode Rack
                sqlQuery.Add("DELETE FROM conv_LampModuleModeRack WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + tmpGwNo + "'");
            }
        }

        //Delete All Block Conversion
        sqlQuery.Add("UPDATE conv_BlockMst SET group_id = NULL , block_seq = NULL , block_name = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strGroupID + "'");

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static void DelGroupMstRem(String strPlcNo, String strProcName, String strGroupID, String strGroupName)
    //{
    //    String sqlQuery = "";

    //    //Delete All Block
    //    sqlQuery = "DELETE FROM dt_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "'";
    //    Boolean blDelBlock = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Rack Detail
    //    DataSet dsRackMst = new DataSet();
    //    DataTable dtRackMst = new DataTable();

    //    dsRackMst = SrcRackMst("", strPlcNo, strProcName, strGroupName, "");
    //    dtRackMst = dsRackMst.Tables[0];

    //    if (dtRackMst.Rows.Count > 0)
    //    {
    //        for (int iRackCnt = 0; iRackCnt < dtRackMst.Rows.Count; iRackCnt++)
    //        {
    //            String strRackName = "";

    //            if (Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]).Trim() != "")
    //            {
    //                strRackName = Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]);
    //            }

    //            sqlQuery = "DELETE FROM dt_RackMstDet WHERE rack_name = '" + strRackName + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //            Boolean blDelRackDet = ConnQuery.ExecuteQuery(sqlQuery);

    //            //Delete All Lamp Module Address
    //            sqlQuery = "UPDATE dt_LampModuleAddMst SET rack_det_id = NULL, rack_loc = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND rack_det_id LIKE '" + strRackName + "^%'";
    //            Boolean blDelLmAdd = ConnQuery.ExecuteQuery(sqlQuery);

    //            //Delete All Part Rack Loc
    //            sqlQuery = "UPDATE ais_PartsNum SET rack_det_id = NULL, rack_loc = NULL WHERE rack_det_id LIKE '" + strRackName + "^%'";
    //            Boolean blDelPartsNum = ConnQuery.ExecuteQuery(sqlQuery);
    //        }
    //    }

    //    //Delete All Rack
    //    sqlQuery = "DELETE FROM dt_RackMst WHERE proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    Boolean blDelRack = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Group Conversion
    //    sqlQuery = "DELETE FROM conv_GroupMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strGroupID + "' AND group_name = '" + strGroupName + "'";
    //    Boolean blDelGroupConv = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Select All Gateway then clear conv_InsCodeMap , conv_LampModuleAddMatching , conv_LampModuleMode , conv_LampModuleModeRack
    //    DataSet dsConv = new DataSet();
    //    DataTable dtConv = new DataTable();

    //    sqlQuery = "SELECT * FROM conv_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strGroupID + "'";
    //    dsConv = ConnQuery.getBindingDatasetData(sqlQuery);
    //    dtConv = dsConv.Tables[0];

    //    if (dtConv.Rows.Count > 0)
    //    {
    //        for (int iRackCnt = 0; iRackCnt < dtConv.Rows.Count; iRackCnt++)
    //        {
    //            String tmpGwNo = "";

    //            if (Convert.ToString(dtConv.Rows[iRackCnt]["gw_no"]).Trim() != "")
    //            {
    //                tmpGwNo = Convert.ToString(dtConv.Rows[iRackCnt]["gw_no"]);
    //            }

    //            //Delete Instruction Code Mapping Conversion
    //            sqlQuery = "UPDATE conv_InsCodeMap SET [" + tmpGwNo + "] = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //            Boolean blClrInsCodeMap = ConnQuery.ExecuteQuery(sqlQuery);

    //            //Delete Lamp Module Address Matching Conversation
    //            sqlQuery = "DELETE FROM conv_LampModuleAddMatching WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + tmpGwNo + "'";
    //            Boolean blClrLmAddMatchConv = ConnQuery.ExecuteQuery(sqlQuery);

    //            //Delete All Lamp Module Mode
    //            sqlQuery = "UPDATE conv_LampModuleMode SET mode_data = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + tmpGwNo + "'";
    //            Boolean blClrLmModeConv = ConnQuery.ExecuteQuery(sqlQuery);

    //            //Delete All Lamp Module Mode Rack
    //            sqlQuery = "DELETE FROM conv_LampModuleModeRack WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + tmpGwNo + "'";
    //            Boolean blClrLmModeRackConv = ConnQuery.ExecuteQuery(sqlQuery);
    //        }
    //    }

    //    //Delete All Block Conversion
    //    sqlQuery = "UPDATE conv_BlockMst SET group_id = NULL , block_seq = NULL , block_name = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strGroupID + "'";
    //    Boolean blDelBlockConv = ConnQuery.ExecuteQuery(sqlQuery);
    //}

    public static Boolean SvGroupMst(String strGroupID, String strGroupName, String strPlcNo, String strProcName, String strCurUser, String strGroupLine)
    {
        strGroupID = strGroupID.Replace("'", "''");
        strGroupName = strGroupName.Replace("'", "''");
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strGroupLine = strGroupLine.Replace("'", "''");

        String sqlQuery = "INSERT INTO dt_GroupMst (group_id, group_name, plc_no, proc_name, last_upd_by, last_upd_dt, group_line) VALUES ('" + strGroupID + "', '" + strGroupName + "','" + strPlcNo + "','" + strProcName + "','" + strCurUser + "', CURRENT_TIMESTAMP, '" + strGroupLine + "')";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdGroupMst(String strGroupID, String strGroupName, String strPlcNo, String strProcName, String strTempGroupID, String strCurUser, String tempGroupName, String strGroupLine)
    {
        strGroupID = strGroupID.Replace("'", "''");
        strGroupName = strGroupName.Replace("'", "''");
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strGroupLine = strGroupLine.Replace("'", "''");

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("UPDATE dt_GroupMst SET group_id = '" + strGroupID + "', group_name = '" + strGroupName + "', plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP, group_line = '" + strGroupLine + "' WHERE group_id = '" + strTempGroupID + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        //Update All Block
        sqlQuery.Add("UPDATE dt_BlockMst SET group_name = '" + strGroupName + "' WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_name = '" + tempGroupName + "'");

        //Update All Rack
        //sqlQuery.Add("UPDATE dt_RackMst SET group_name = '" + strGroupName + "' WHERE proc_name = '" + strProcName + "' AND group_name = '" + tempGroupName + "'");  //***ace_20160416_001
        sqlQuery.Add("UPDATE dt_RackMst SET group_name = '" + strGroupName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE proc_name = '" + strProcName + "' AND group_name = '" + tempGroupName + "'");

        //Update All Group Conversion
        sqlQuery.Add("UPDATE conv_GroupMst SET group_id = '" + strGroupID + "', group_name = '" + strGroupName + "' WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strTempGroupID + "'");

        //Update All Block Conversion
        sqlQuery.Add("UPDATE conv_BlockMst SET group_id = '" + strGroupID + "' WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strTempGroupID + "'");

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static void UpdGroupMstRem(String strPlcNo, String strProcName, String strGroupID, String strGroupName, String strTempGroupID, String tempGroupName)
    //{
    //    String sqlQuery = "";

    //    //Update All Block
    //    sqlQuery = "UPDATE dt_BlockMst SET group_name = '" + strGroupName + "' WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_name = '" + tempGroupName + "'";
    //    Boolean blUpdBlock = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Rack
    //    sqlQuery = "UPDATE dt_RackMst SET group_name = '" + strGroupName + "' WHERE proc_name = '" + strProcName + "' AND group_name = '" + tempGroupName + "'";;
    //    Boolean blUpdRack = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Group Conversion
    //    sqlQuery = "UPDATE conv_GroupMst SET group_id = '" + strGroupID + "', group_name = '" + strGroupName + "' WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strTempGroupID + "'";
    //    Boolean blUpdGroupConv = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Block Conversion
    //    sqlQuery = "UPDATE conv_BlockMst SET group_id = '" + strGroupID + "' WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strTempGroupID + "'";
    //    Boolean blUpdBlockConv = ConnQuery.ExecuteQuery(sqlQuery);
    //}

    public static Boolean ChkGroupMaxCnt(String strProcName)
    {
        String sqlQuery = "SELECT COUNT(group_id) AS ReturnField FROM dt_GroupMst WHERE group_id != '' AND proc_name = '" + strProcName + "'";
        try
        {
            int iGroupCnt = Convert.ToInt32(ConnQuery.getReturnFieldExecuteReader(sqlQuery));
            if (iGroupCnt >= 6)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkDuplicateGroupID(String strGroupId, String strTmpGroupId, String strProcName, String strGroupLine)
    {
        String sqlQuery = "SELECT COUNT(group_id) AS CountRow FROM dt_GroupMst WHERE group_id != '" + strTmpGroupId + "' AND group_id = '" + strGroupId + "' AND proc_name = '" + strProcName + "' AND group_line = '" + strGroupLine + "' ";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkDuplicateGroupName(String strGroupName, String strTmpGroupName, String strProcName, String strGroupLine)
    {
        String sqlQuery = "SELECT COUNT(group_name) AS CountRow FROM dt_GroupMst WHERE group_name != '" + strTmpGroupName + "' AND group_name = '" + strGroupName + "' AND proc_name = '" + strProcName + "' AND group_line = '" + strGroupLine + "' ";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region Block Master
    public static DataSet SrcBlockMst(String strPlcNo, String strBlockSeq, String strBlockName, String strGwNo, String strStartLm, String strEndLm, String strProcName, String strGroupName, String strStartModuleType, String strEndModuleType)
    {
        String filterCriteria = "";

        if (strPlcNo != "")
        {
            filterCriteria = filterCriteria + " AND blk.plc_no = '" + strPlcNo + "'";
        }
        if (strBlockSeq != "")
        {
            filterCriteria = filterCriteria + " AND blk.block_seq = '" + strBlockSeq + "'";
        }
        if (strBlockName != "")
        {
            filterCriteria = filterCriteria + " AND blk.block_name = '" + strBlockName + "'";
        }
        if (strGwNo != "")
        {
            filterCriteria = filterCriteria + " AND blk.gw_no = '" + strGwNo + "'";
        }
        if (strStartLm != "")
        {
            filterCriteria = filterCriteria + " AND blk.start_lm = '" + strStartLm + "'";
        }
        if (strEndLm != "")
        {
            filterCriteria = filterCriteria + " AND blk.end_lm = '" + strEndLm + "'";
        }
        if (strProcName != "")
        {
            filterCriteria = filterCriteria + " AND blk.proc_name = '" + strProcName + "'";
        }
        if (strGroupName != "")
        {
            filterCriteria = filterCriteria + " AND blk.group_name = '" + strGroupName + "'";
        }
        if (strStartModuleType != "")
        {
            filterCriteria = filterCriteria + " AND blk.start_module_type = '" + strStartModuleType + "'";
        }
        if (strEndModuleType != "")
        {
            filterCriteria = filterCriteria + " AND blk.end_module_type = '" + strEndModuleType + "'";
        }

        String sqlQuery = "SELECT blk.*, grp.group_line FROM dt_BlockMst blk JOIN dt_GroupMst grp ON blk.group_name = grp.group_name WHERE blk.block_name != '' " + filterCriteria + " ORDER BY gw_no";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean DelBlockMst(String strPlcNo, String strProcName, String strGroupName, String strBlockName, String strGwNo)
    {

        List<String> sqlQuery = new List<String>();

        //Modified by YanTeng 08/12/2020 - Added plc_no & proc_name
        //sqlQuery.Add("DELETE FROM dt_BlockMst WHERE block_name = '" + strBlockName + "'");
        sqlQuery.Add("DELETE FROM dt_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND block_name = '" + strBlockName + "'");

        String strQuery = "SELECT group_id AS ReturnField FROM conv_GroupMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "'";
        String strGroupID = ConnQuery.getReturnFieldExecuteReader(strQuery);

        //Delete All Rack Detail
        DataSet dsRackMst = new DataSet();
        DataTable dtRackMst = new DataTable();

        dsRackMst = SrcRackMst("", strPlcNo, strProcName, strGroupName, strBlockName);
        dtRackMst = dsRackMst.Tables[0];

        if (dtRackMst.Rows.Count > 0)
        {
            for (int iRackCnt = 0; iRackCnt < dtRackMst.Rows.Count; iRackCnt++)
            {
                String strRackName = "";

                if (Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]).Trim() != "")
                {
                    strRackName = Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]);
                }

                sqlQuery.Add("DELETE FROM dt_RackMstDet WHERE rack_name = '" + strRackName + "'");

                //Delete All Lamp Module Address
                sqlQuery.Add("UPDATE dt_LampModuleAddMst SET rack_det_id = NULL, rack_loc = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND rack_det_id LIKE '" + strRackName + "^%'");

                //Delete All Part Rack Loc
                sqlQuery.Add("UPDATE ais_PartsNum SET rack_det_id = NULL, rack_loc = NULL WHERE rack_det_id LIKE '" + strRackName + "^%'");
            }
        }

        //Delete All Rack
        sqlQuery.Add("DELETE FROM dt_RackMst WHERE proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "' AND block_name = '" + strBlockName + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        //Delete All Block Conversion
        sqlQuery.Add("UPDATE conv_BlockMst SET group_id = NULL, block_seq = NULL, block_name = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND block_name = '" + strBlockName + "' AND gw_no = '" + strGwNo + "'");

        //Delete All Ins Code Map Conversion
        sqlQuery.Add("UPDATE conv_InsCodeMap SET [" + strGwNo + "] = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        //Delete All Lamp Module Address Matching Conversion
        sqlQuery.Add("DELETE FROM conv_LampModuleAddMatching WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + strGwNo + "'");

        //Delete All Lamp Module Mode
        sqlQuery.Add("UPDATE conv_LampModuleMode SET mode_data = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + strGwNo + "'");

        //Delete All Lamp Module Mode Rack
        sqlQuery.Add("DELETE FROM conv_LampModuleModeRack WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + strGwNo + "'");

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean SvBlockMst(String strPlcNo, String strBlockSeq, String strBlockName, String strGwNo, String strStartLm, String strEndLm, String strProcName, String strGroupName, String strStartModuleType, String strEndModuleType, String strLightingWI, String strLightingERR, String strColorWI, String strColorERR, String strCurUser)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strBlockSeq = strBlockSeq.Replace("'", "''");
        strBlockName = strBlockName.Replace("'", "''");
        strGwNo = strGwNo.Replace("'", "''");
        strStartLm = strStartLm.Replace("'", "''");
        strEndLm = strEndLm.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strGroupName = strGroupName.Replace("'", "''");
        strStartModuleType = strStartModuleType.Replace("'", "''");
        strEndModuleType = strEndModuleType.Replace("'", "''");
        strLightingWI = strLightingWI.Replace("'", "''");
        strLightingERR = strLightingERR.Replace("'", "''");
        strColorWI = strColorWI.Replace("'", "''");
        strColorERR = strColorERR.Replace("'", "''");

        String sqlQuery = "INSERT INTO dt_BlockMst (plc_no, proc_name, group_name, block_seq, block_name, gw_no, start_lm, start_module_type, end_lm, end_module_type, light_wi, light_err, color_wi, color_err, last_upd_by, last_upd_dt) VALUES ('" + strPlcNo + "', '" + strProcName + "','" + strGroupName + "','" + strBlockSeq + "','" + strBlockName + "','" + strGwNo + "','" + strStartLm + "','" + strStartModuleType + "','" + strEndLm + "','" + strEndModuleType + "','" + strLightingWI + "','" + strLightingERR + "','" + strColorWI + "','" + strColorERR + "','" + strCurUser + "', CURRENT_TIMESTAMP)";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdBlockMst(String strPlcNo, String strBlockSeq, String strBlockName, String strGwNo, String strStartLm, String strEndLm, String strProcName, String strGroupName, String strStartModuleType, String strEndModuleType, String strLightingWI, String strLightingERR, String strColorWI, String strColorERR, String strCurUser, String tempBlockName)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strBlockSeq = strBlockSeq.Replace("'", "''");
        strBlockName = strBlockName.Replace("'", "''");
        strGwNo = strGwNo.Replace("'", "''");
        strStartLm = strStartLm.Replace("'", "''");
        strEndLm = strEndLm.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strGroupName = strGroupName.Replace("'", "''");
        strStartModuleType = strStartModuleType.Replace("'", "''");
        strEndModuleType = strEndModuleType.Replace("'", "''");
        strLightingWI = strLightingWI.Replace("'", "''");
        strLightingERR = strLightingERR.Replace("'", "''");
        strColorWI = strColorWI.Replace("'", "''");
        strColorERR = strColorERR.Replace("'", "''");

        List<String> sqlQuery = new List<String>();

        //Modified by YanTeng 08/12/2020
        //sqlQuery.Add("UPDATE dt_BlockMst SET plc_no = '" + strPlcNo + "', block_seq = '" + strBlockSeq + "', block_name = '" + strBlockName + "', gw_no = '" + strGwNo + "', start_lm = '" + strStartLm + "', end_lm = '" + strEndLm + "', proc_name = '" + strProcName + "', group_name = '" + strGroupName + "', start_module_type = '" + strStartModuleType + "', end_module_type = '" + strEndModuleType + "', light_wi = '" + strLightingWI + "', light_err = '" + strLightingERR + "', color_wi = '" + strColorWI + "', color_err = '" + strColorERR + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE block_name = '" + strstrTempBlockName + "'");
        sqlQuery.Add("UPDATE dt_BlockMst SET plc_no = '" + strPlcNo + "', block_seq = '" + strBlockSeq + "', block_name = '" + strBlockName + "', gw_no = '" + strGwNo + "', start_lm = '" + strStartLm + "', end_lm = '" + strEndLm + "', proc_name = '" + strProcName + "', group_name = '" + strGroupName + "', start_module_type = '" + strStartModuleType + "', end_module_type = '" + strEndModuleType + "', light_wi = '" + strLightingWI + "', light_err = '" + strLightingERR + "', color_wi = '" + strColorWI + "', color_err = '" + strColorERR + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND block_name = '" + tempBlockName + "'");

        String strQuery = "SELECT group_id AS ReturnField FROM conv_GroupMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "'";
        String strGroupID = ConnQuery.getReturnFieldExecuteReader(strQuery);

        strQuery = "SELECT gw_no AS ReturnField FROM conv_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strGroupID + "' AND block_name = '" + tempBlockName + "'";
        String tmpGwNo = ConnQuery.getReturnFieldExecuteReader(strQuery);

        //Update All Rack
        //sqlQuery.Add("UPDATE dt_RackMst SET block_name = '" + strBlockName + "', group_name = '" + strGroupName + "' WHERE proc_name = '" + strProcName + "' AND block_name = '" + tempBlockName + "'");  //***ace_20160416_001
        sqlQuery.Add("UPDATE dt_RackMst SET block_name = '" + strBlockName + "', group_name = '" + strGroupName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE proc_name = '" + strProcName + "' AND block_name = '" + tempBlockName + "'");

        //Update All Block Conversion
        sqlQuery.Add("UPDATE conv_BlockMst SET group_id = '" + strGroupID + "', block_seq = '" + strBlockSeq + "', block_name = '" + strBlockName + "' WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND block_name = '" + tempBlockName + "'");

        //Update All Ins Code Map
        if (tmpGwNo != "")
        {
            for (int iInsCodeCnt = 1; iInsCodeCnt <= 299; iInsCodeCnt++)
            {
                sqlQuery.Add("UPDATE conv_InsCodeMap SET [" + strGwNo + "] = (SELECT [" + tmpGwNo + "] FROM conv_InsCodeMap WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND ins_code_cnt = '" + iInsCodeCnt + "') WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND ins_code_cnt = '" + iInsCodeCnt + "'");
            }
        }

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkBlockMaxCnt(String strProcName)
    {
        String sqlQuery = "SELECT COUNT(block_name) AS ReturnField FROM dt_BlockMst WHERE block_name != '' AND proc_name = '" + strProcName + "'";
        try
        {
            int iGroupCnt = Convert.ToInt32(ConnQuery.getReturnFieldExecuteReader(sqlQuery));
            if (iGroupCnt >= 12)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkGwDuplicate(String strProcName, String strGwNo, String strTmpBlockName)
    {
        String sqlQuery = "SELECT COUNT(gw_no) AS CountRow FROM dt_BlockMst WHERE block_name != '" + strTmpBlockName + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + strGwNo + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return true;
        }
    }

    public static Boolean ChkDuplicateBlockName(String strPlcNo, String strProcName, String strBlockName, String strCurBlockName)
    {
        //Modified by YanTeng (08/12/2020) - Added plc_no & proc_name
        //String sqlQuery = "SELECT COUNT(block_name) AS CountRow FROM dt_BlockMst WHERE block_name = '" + strBlockName + "' AND block_name != '" + strCurBlockName + "'";
        String sqlQuery = "SELECT COUNT(block_name) AS CountRow FROM dt_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND block_name = '" + strBlockName + "' AND block_name != '" + strCurBlockName + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return true;
        }
    }

    //public static void UpdBlockMstRem(String strPlcNo, String strProcName, String strGroupName, String strBlockName, String strGwNo, String strBlockSeq, String tempBlockName)
    //{
    //    String sqlQuery = "";

    //    sqlQuery = "SELECT group_id AS ReturnField FROM conv_GroupMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "'";
    //    String strGroupID = ConnQuery.getReturnFieldExecuteReader(sqlQuery);

    //    sqlQuery = "SELECT gw_no AS ReturnField FROM conv_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_id = '" + strGroupID + "' AND block_name = '" + tempBlockName + "'";
    //    String tmpGwNo = ConnQuery.getReturnFieldExecuteReader(sqlQuery);

    //    //Update All Rack
    //    sqlQuery = "UPDATE dt_RackMst SET block_name = '" + strBlockName + "', group_name = '" + strGroupName + "' WHERE proc_name = '" + strProcName + "' AND block_name = '" + tempBlockName + "'";
    //    Boolean blUpdRack = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Block Conversion
    //    sqlQuery = "UPDATE conv_BlockMst SET group_id = '" + strGroupID + "', gw_no = '" + strGwNo + "', block_seq = '" + strBlockSeq + "', block_name = '" + strBlockName + "' WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND block_name = '" + tempBlockName + "'";
    //    Boolean blUpdBlockConv = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Update All Ins Code Map
    //    if (tmpGwNo != "")
    //    {
    //        for (int iInsCodeCnt = 1; iInsCodeCnt <= 299; iInsCodeCnt++)
    //        {
    //            sqlQuery = "UPDATE conv_InsCodeMap SET [" + strGwNo + "] = (SELECT [" + tmpGwNo + "] FROM conv_InsCodeMap WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND ins_code_cnt = '" + iInsCodeCnt + "') WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND ins_code_cnt = '" + iInsCodeCnt + "'";
    //            Boolean blUpdInsCodeMapConv = ConnQuery.ExecuteQuery(sqlQuery);
    //        }
    //    }
    //}

    //public static void DelBlockMstRem(String strPlcNo, String strProcName, String strGroupName, String strBlockName, String strGwNo)
    //{
    //    String sqlQuery = "";

    //    sqlQuery = "SELECT group_id AS ReturnField FROM conv_GroupMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "'";
    //    String strGroupID = ConnQuery.getReturnFieldExecuteReader(sqlQuery);

    //    //Delete All Rack Detail
    //    DataSet dsRackMst = new DataSet();
    //    DataTable dtRackMst = new DataTable();

    //    dsRackMst = SrcRackMst("", strPlcNo, strProcName, strGroupName, strBlockName);
    //    dtRackMst = dsRackMst.Tables[0];

    //    if (dtRackMst.Rows.Count > 0)
    //    {
    //        for (int iRackCnt = 0; iRackCnt < dtRackMst.Rows.Count; iRackCnt++)
    //        {
    //            String strRackName = "";

    //            if (Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]).Trim() != "")
    //            {
    //                strRackName = Convert.ToString(dtRackMst.Rows[iRackCnt]["rack_name"]);
    //            }

    //            sqlQuery = "DELETE FROM dt_RackMstDet WHERE rack_name = '" + strRackName + "'";
    //            Boolean blDelRackDet = ConnQuery.ExecuteQuery(sqlQuery);

    //            //Delete All Lamp Module Address
    //            sqlQuery = "UPDATE dt_LampModuleAddMst SET rack_det_id = NULL, rack_loc = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND rack_det_id LIKE '" + strRackName + "^%'";
    //            Boolean blDelLmAdd = ConnQuery.ExecuteQuery(sqlQuery);

    //            //Delete All Part Rack Loc
    //            sqlQuery = "UPDATE ais_PartsNum SET rack_det_id = NULL, rack_loc = NULL WHERE rack_det_id LIKE '" + strRackName + "^%'";
    //            Boolean blDelPartsNum = ConnQuery.ExecuteQuery(sqlQuery);
    //        }
    //    }

    //    //Delete All Rack
    //    sqlQuery = "DELETE FROM dt_RackMst WHERE proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "' AND block_name = '" + strBlockName + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    Boolean blDelRack = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Block Conversion
    //    sqlQuery = "UPDATE conv_BlockMst SET group_id = NULL, block_seq = NULL, block_name = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND block_name = '" + strBlockName + "' AND gw_no = '" + strGwNo + "'";
    //    Boolean blUpdBlockConv = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Ins Code Map Conversion
    //    sqlQuery = "UPDATE conv_InsCodeMap SET [" + strGwNo + "] = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    Boolean blClrInsCodeMapConv = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Lamp Module Address Matching Conversion
    //    sqlQuery = "DELETE FROM conv_LampModuleAddMatching WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + strGwNo + "'";
    //    Boolean blClrLmAddMatchConv = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Lamp Module Mode
    //    sqlQuery = "UPDATE conv_LampModuleMode SET mode_data = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + strGwNo + "'";
    //    Boolean blClrLmModeConv = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Lamp Module Mode Rack
    //    sqlQuery = "DELETE FROM conv_LampModuleModeRack WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + strGwNo + "'";
    //    Boolean blClrLmModeRackConv = ConnQuery.ExecuteQuery(sqlQuery);
    //}
    #endregion

    #region Rack Master

    // vincent 01/12/2016
    public static DataSet SrcRackMstDet(String strRackName, String strPlcNo, String strProcName, String strGroupName, String strBlockName)
    {
        String filterCriteria = "";

        if (strRackName != "")
        {
            filterCriteria = filterCriteria + " AND a.rack_name = '" + strRackName + "'";
        }
        if (strProcName != "")
        {
            filterCriteria = filterCriteria + " AND a.proc_name = '" + strProcName + "'";
        }
        if (strGroupName != "")
        {
            filterCriteria = filterCriteria + " AND a.group_name= '" + strGroupName + "'";
        }
        if (strBlockName != "")
        {
            filterCriteria = filterCriteria + " AND a.block_name = '" + strBlockName + "'";
        }

        //String sqlQuery = "SELECT a.*, b.* FROM dt_RackMst a INNER JOIN dt_RackMstDet b ON a.plc_no=b.plc_no  WHERE a.rack_name != '' " + filterCriteria + " ORDER BY a.proc_name, a.group_name, a.block_name, a.rack_name, a.plc_no, a.col_cnt, a.row_cnt ";
        String sqlQuery = "SELECT a.*, b.* FROM dt_RackMst a INNER JOIN dt_RackMstDet b ON  A.proc_name=B.proc_name AND A.rack_name=B.rack_name AND A.plc_no=b.plc_no  WHERE a.rack_name != '' " + filterCriteria + " ORDER BY a.proc_name, a.group_name, a.block_name, a.rack_name, a.plc_no, a.col_cnt, a.row_cnt ";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static DataSet SrcRackMst(String strRackName, String strPlcNo, String strProcName, String strGroupName, String strBlockName)
    {
        String filterCriteria = "";

        if (strRackName != "")
        {
            filterCriteria = filterCriteria + " AND rack_name = '" + strRackName + "'";
        }
        if (strProcName != "")
        {
            filterCriteria = filterCriteria + " AND proc_name = '" + strProcName + "'";
        }
        if (strGroupName != "")
        {
            filterCriteria = filterCriteria + " AND group_name= '" + strGroupName + "'";
        }
        if (strBlockName != "")
        {
            filterCriteria = filterCriteria + " AND block_name = '" + strBlockName + "'";
        }

        String sqlQuery = "SELECT * FROM dt_RackMst WHERE rack_name != '' " + filterCriteria + " ORDER BY proc_name, group_name, block_name, rack_name";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean DelRackMst(String strRackName, String strPlcNo, String strProcName)
    {

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("DELETE FROM dt_RackMst WHERE rack_name = '" + strRackName + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        //Delete All Lamp Module Address
        sqlQuery.Add("UPDATE dt_LampModuleAddMst SET rack_det_id = NULL, rack_loc = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND rack_det_id LIKE '" + strRackName + "^%'");

        //Delete All Part Rack Loc
        sqlQuery.Add("UPDATE ais_PartsNum SET rack_det_id = NULL, rack_loc = NULL WHERE rack_det_id LIKE '" + strRackName + "^%'");

        sqlQuery.Add("DELETE FROM dt_RackMstDet WHERE rack_name = '" + strRackName + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");
        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static Boolean DelRackMstDetAll(String strRackName, String strPlcNo, String strProcName)
    //{
    //    String sqlQuery = "";

    //    //Delete All Lamp Module Address
    //    sqlQuery = "UPDATE dt_LampModuleAddMst SET rack_det_id = NULL, rack_loc = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND rack_det_id LIKE '" + strRackName + "^%'";
    //    Boolean blDelLmAdd = ConnQuery.ExecuteQuery(sqlQuery);

    //    //Delete All Part Rack Loc
    //    sqlQuery = "UPDATE ais_PartsNum SET rack_det_id = NULL, rack_loc = NULL WHERE rack_det_id LIKE '" + strRackName + "^%'";
    //    Boolean blDelPartsNum = ConnQuery.ExecuteQuery(sqlQuery);

    //    sqlQuery = "DELETE FROM dt_RackMstDet WHERE rack_name = '" + strRackName + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    try
    //    {
    //        return ConnQuery.ExecuteQuery(sqlQuery);
    //    }
    //    catch (Exception ex)
    //    {
    //        GlobalFunc.Log(ex);
    //        GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
    //        return false;
    //    }
    //}

    public static Boolean SvRackMst(String strRackName, String strColCnt, String strRowCnt, String strPlcNo, String strProcName, String strGroupName, String strBlockName, String strCurUser)
    {
        strRackName = strRackName.Replace("'", "''");
        strColCnt = strColCnt.Replace("'", "''");
        strRowCnt = strRowCnt.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strGroupName = strGroupName.Replace("'", "''");
        strBlockName = strBlockName.Replace("'", "''");

        String sqlQuery = "INSERT INTO dt_RackMst (rack_name, col_cnt, row_cnt, plc_no, proc_name, group_name, block_name, last_upd_by, last_upd_dt) VALUES ('" + strRackName + "', '" + strColCnt + "','" + strRowCnt + "','" + strPlcNo + "','" + strProcName + "','" + strGroupName + "','" + strBlockName + "','" + strCurUser + "', CURRENT_TIMESTAMP)";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdRackMst(String strRackName, String strColCnt, String strRowCnt, String strPlcNo, String strProcName, String strGroupName, String strBlockName, String strTmpRackName, String strCurUser)
    {
        strRackName = strRackName.Replace("'", "''");
        strColCnt = strColCnt.Replace("'", "''");
        strRowCnt = strRowCnt.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strGroupName = strGroupName.Replace("'", "''");
        strBlockName = strBlockName.Replace("'", "''");

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("UPDATE dt_RackMst SET rack_name = '" + strRackName + "', col_cnt = '" + strColCnt + "', row_cnt = '" + strRowCnt + "', plc_no = '" + strPlcNo + "', proc_name = '" + strProcName + "', group_name = '" + strGroupName + "', block_name = '" + strBlockName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE rack_name = '" + strTmpRackName + "'");

        sqlQuery.Add("UPDATE ais_PartsNum SET rack_det_id = REPLACE(rack_det_id, '" + strTmpRackName + "', '" + strRackName + "') where rack_det_id LIKE '" + strTmpRackName + "^%'");

        sqlQuery.Add("UPDATE dt_LampModuleAddMst SET rack_det_id = REPLACE(rack_det_id, '" + strTmpRackName + "', '" + strRackName + "') where rack_det_id LIKE '" + strTmpRackName + "^%' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'");

        //sqlQuery.Add("UPDATE dt_RackMstDet SET rack_det_id = REPLACE(rack_det_id, '" + strTmpRackName + "', '" + strRackName + "'), rack_name = '" + strRackName + "' where rack_name = '" + strTmpRackName + "'");  //***ace_20160416_001
        sqlQuery.Add("UPDATE dt_RackMstDet SET rack_det_id = REPLACE(rack_det_id, '" + strTmpRackName + "', '" + strRackName + "'), rack_name = '" + strRackName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE rack_name = '" + strTmpRackName + "'");
        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    //public static Boolean UpdRackDetAll(String strRackName, String strTmpRackName, String strPlcNo, String strProcName)
    //{
    //    strRackName = strRackName.Replace("'", "''");
    //    strTmpRackName = strTmpRackName.Replace("'", "''");

    //    String sqlQuery = "UPDATE ais_PartsNum SET rack_det_id = REPLACE(rack_det_id, '" + strTmpRackName + "', '" + strRackName + "') where rack_det_id LIKE '" + strTmpRackName + "^%'";
    //    Boolean blUpdPartsNum = ConnQuery.ExecuteQuery(sqlQuery);

    //    sqlQuery = "UPDATE dt_LampModuleAddMst SET rack_det_id = REPLACE(rack_det_id, '" + strTmpRackName + "', '" + strRackName + "') where rack_det_id LIKE '" + strTmpRackName + "^%' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
    //    Boolean blUpdLmAdd = ConnQuery.ExecuteQuery(sqlQuery);

    //    sqlQuery = "UPDATE dt_RackMstDet SET rack_det_id = REPLACE(rack_det_id, '" + strTmpRackName + "', '" + strRackName + "'), rack_name = '" + strRackName + "' where rack_name = '" + strTmpRackName + "'";
    //    try
    //    {
    //        return ConnQuery.ExecuteQuery(sqlQuery);
    //    }
    //    catch (Exception ex)
    //    {
    //        GlobalFunc.Log(ex);
    //        GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
    //        return false;
    //    }
    //}

    public static Boolean ChkDuplicateRackName(String strRackName, String strCurRackName)
    {
        String sqlQuery = "SELECT COUNT(rack_name) AS CountRow FROM dt_RackMst WHERE rack_name = '" + strRackName + "' AND rack_name != '" + strCurRackName + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return true;
        }
    }

    public static Boolean ChkRackLocOcc(String strTmpRackName, String strTmpRowCnt, String strTmpColCnt, String strRowCnt, String strColCnt)
    {
        Boolean blExist = false;

        if ((strRowCnt == strTmpRowCnt) && (strColCnt == strTmpColCnt)) return blExist;

        if (strRowCnt != strTmpRowCnt)
        {
            for (int iRow = Convert.ToInt32(strRowCnt) + 1; iRow <= Convert.ToInt32(strTmpRowCnt); iRow++)
            {
                for (int iCol = 1; iCol <= Convert.ToInt32(strTmpColCnt); iCol++)
                {
                    String sqlQuery = "SELECT COUNT(rack_name) AS CountRow FROM dt_RackMstDet WHERE rack_det_id = '" + strTmpRackName + "^" + iRow + "^" + iCol + "'";
                    try
                    {
                        blExist = ConnQuery.chkExistData(sqlQuery);

                        if (blExist == true)
                        {
                            return blExist;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobalFunc.Log(ex);
                        GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
                        return true;
                    }
                }
            }
        }

        if (strColCnt != strTmpColCnt)
        {
            for (int iRow = 1; iRow <= Convert.ToInt32(strTmpRowCnt); iRow++)
            {
                for (int iCol = Convert.ToInt32(strColCnt) + 1; iCol <= Convert.ToInt32(strTmpColCnt); iCol++)
                {
                    String sqlQuery = "SELECT COUNT(rack_name) AS CountRow FROM dt_RackMstDet WHERE rack_det_id = '" + strTmpRackName + "^" + iRow + "^" + iCol + "'";
                    try
                    {
                        blExist = ConnQuery.chkExistData(sqlQuery);

                        if (blExist == true)
                        {
                            return blExist;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobalFunc.Log(ex);
                        GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
                        return true;
                    }
                }
            }
        }
        return blExist;
    }
    #endregion

    #region Rack Master Detail

    public static DataSet SrcRackMstDet(String strRackName, String strProcName, String strGroupName, String strBlockName)
    {
        String filterCriteria = "";

        if (strRackName != "")
        {
            filterCriteria = filterCriteria + " AND rack_name = '" + strRackName + "'";
        }
        if (strProcName != "")
        {
            filterCriteria = filterCriteria + " AND proc_name = '" + strProcName + "'";
        }
        if (strGroupName != "")
        {
            filterCriteria = filterCriteria + " AND group_name= '" + strGroupName + "'";
        }
        if (strBlockName != "")
        {
            filterCriteria = filterCriteria + " AND block_name = '" + strBlockName + "'";
        }

        String sqlQuery = "SELECT * FROM dt_RackMstDet WHERE rack_name != '' " + filterCriteria + " ORDER BY proc_name, group_name, block_name, rack_name, hj_row, hj_col";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean ChkRackMstDetExist(String strRackMstDetId)
    {
        strRackMstDetId = strRackMstDetId.Replace("'", "''");

        String sqlQuery = "SELECT COUNT(rack_det_id) AS CountRow FROM dt_RackMstDet WHERE rack_det_id = '" + strRackMstDetId + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    #region ChangeLog ***ace_20160405_001
    public static Boolean ChkAisPartsNumExist(String strPartsNo, String strColorSfx) // ***ace_20160407_001
    {
        strPartsNo = strPartsNo.Replace("'", "''");
        strColorSfx = strColorSfx.Replace("'", "''"); // ***ace_20160407_001

        String sqlQuery = "SELECT COUNT(part_no) AS CountRow FROM ais_PartsNum WHERE part_no = '" + strPartsNo + "' AND color_sfx = '" + strColorSfx + "'"; // ***ace_20160407_001
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    public static Boolean ChkLmLocExist(String strRackMstDetId)
    {
        strRackMstDetId = strRackMstDetId.Replace("'", "''");

        String sqlQuery = "SELECT COUNT(rack_loc) AS CountRow FROM dt_LampModuleAddMst WHERE rack_det_id = '" + strRackMstDetId + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static String GetAisRackLoc(String strHjId, String strHjItemId, String strHjRow, String strHjCol, String strPartsNo, String strColorSfx)
    {
        strHjId = strHjId.Replace("'", "''");
        strHjItemId = strHjItemId.Replace("'", "''");
        strHjRow = strHjRow.Replace("'", "''");
        strHjCol = strHjCol.Replace("'", "''");
        strPartsNo = strPartsNo.Replace("'", "''");
        strColorSfx = strColorSfx.Replace("'", "''");

        String sqlQuery = "SELECT rack_loc AS ReturnField FROM ais_DataHJ WHERE id = '" + strHjId + "' AND item_id = '" + strHjItemId + "' AND row = '" + strHjRow + "' AND col = '" + strHjCol + "' AND part_no = '" + strPartsNo + "' AND color_sfx = '" + strColorSfx + "'";
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    public static String GetPartsRackLoc(String strPartsNo, String strColorSfx)
    {
        strPartsNo = strPartsNo.Replace("'", "''");
        strColorSfx = strColorSfx.Replace("'", "''");

        String sqlQuery = "SELECT rack_loc AS ReturnField FROM ais_PartsNum WHERE part_no = '" + strPartsNo + "' AND color_sfx = '" + strColorSfx + "'";
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    #region Rack Master Detail - AIS Selection
    public static String GetHjRowCnt(String strAisType, String strAisItemId)
    {
        String filterCriteria = "";

        if (strAisType != "")
        {
            if (strAisType == "Jundate")
            {
                filterCriteria = filterCriteria + " AND row IS NULL";
                return "1";
            }
            else
            {
                filterCriteria = filterCriteria + " AND row IS NOT NULL";
            }
        }
        if (strAisItemId != "")
        {
            //filterCriteria = filterCriteria + " AND item_id = '" + strAisItemId + "'";
            String[] tmpAisCond = strAisItemId.Split('-');
            int tmpAisCondCnt = tmpAisCond.Length;
            if (tmpAisCondCnt == 3)
            {
                filterCriteria = filterCriteria + " AND name = '" + tmpAisCond[0].Trim() + "' AND sfx = '" + tmpAisCond[1].Trim() + "' AND color = '" + tmpAisCond[2].Trim() + "'";
            }
        }
        //if (strRevNo != "")
        //{
        //    filterCriteria = filterCriteria + " AND rev_no = '" + strRevNo + "'";
        //}
        String sqlQuery = "SELECT MAX(row) as ReturnField FROM ais_DataHJ WHERE item_id !='' " + filterCriteria;
        return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
    }

    public static String GetHjColCnt(String strAisType, String strAisItemId)
    {
        String filterCriteria = "";

        if (strAisType == "Jundate")
        {
            filterCriteria = filterCriteria + " AND row IS NULL";
        }
        else
        {
            filterCriteria = filterCriteria + " AND row IS NOT NULL";
        }
        if (strAisItemId != "")
        {
            //filterCriteria = filterCriteria + " AND item_id = '" + strAisItemId + "'";
            String[] tmpAisCond = strAisItemId.Split('-');
            int tmpAisCondCnt = tmpAisCond.Length;
            if (tmpAisCondCnt == 3)
            {
                filterCriteria = filterCriteria + " AND name = '" + tmpAisCond[0].Trim() + "' AND sfx = '" + tmpAisCond[1].Trim() + "' AND color = '" + tmpAisCond[2].Trim() + "'";
            }
        }
        //if (strRevNo != "")
        //{
        //    filterCriteria = filterCriteria + " AND rev_no = '" + strRevNo + "'";
        //}
        String sqlQuery = "SELECT MAX(col) as ReturnField FROM ais_DataHJ WHERE item_id != ''" + filterCriteria;
        return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
    }

    public static String GetPartsNumSymbolRtf(String strPartNo, String strColorSfx)
    {
        String sqlQuery = "SELECT symbol as ReturnField FROM ais_PartsNum WHERE part_no = '" + strPartNo + "' AND color_sfx = '" + strColorSfx + "'";
        return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
    }

    public static String GetPartsNumSymbolCode(String strPartNo, String strColorSfx)
    {
        String sqlQuery = "SELECT symbol_code as ReturnField FROM ais_PartsNum WHERE part_no = '" + strPartNo + "' AND color_sfx = '" + strColorSfx + "'";
        return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
    }

    public static Boolean DelRackMstDetAis(String strRackMstDetId)
    {
        String sqlQuery = "DELETE FROM dt_RackMstDet WHERE rack_det_id = '" + strRackMstDetId + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean SvRackDetAis(String strRackMstDetId, String strHjId, String strHjItemId, String strHjRow, String strHjCol, String strPartsTitle, String strPartsNo, String strColorSfx, String strPartsNumSymbolCode, String strPartsNumSymbolRtf, String strRackName, String strPlcNo, String strProcName)
    {
        strRackMstDetId = strRackMstDetId.Replace("'", "''");
        strHjId = strHjId.Replace("'", "''");
        strHjItemId = strHjItemId.Replace("'", "''");
        strHjRow = strHjRow.Replace("'", "''");
        strHjCol = strHjCol.Replace("'", "''");
        strPartsTitle = strPartsTitle.Replace("'", "''");
        strPartsNo = strPartsNo.Replace("'", "''");
        strColorSfx = strColorSfx.Replace("'", "''");
        strPartsNumSymbolCode = strPartsNumSymbolCode.Replace("'", "''");
        strPartsNumSymbolRtf = strPartsNumSymbolRtf.Replace("'", "''");

        String sqlQuery = "INSERT INTO dt_RackMstDet (rack_det_id, hj_id, hj_item_id, hj_row, hj_col, parts_title, parts_no, color_sfx, symbol_code, symbol_rtf, rack_name, plc_no, proc_name) VALUES ('" + strRackMstDetId + "', '" + strHjId + "','" + strHjItemId + "','" + strHjRow + "','" + strHjCol + "','" + strPartsTitle + "','" + strPartsNo + "','" + strColorSfx + "','" + strPartsNumSymbolCode + "','" + strPartsNumSymbolRtf + "','" + strRackName + "','" + strPlcNo + "','" + strProcName + "')";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdRackDetAis(String strRackMstDetId, String strHjId, String strHjItemId, String strHjRow, String strHjCol, String strPartsTitle, String strPartsNo, String strColorSfx, String strPartsNumSymbolCode, String strPartsNumSymbolRtf, String strRackName, String strPlcNo, String strProcName, String strCurUser)  //***ace_20160416_001
    {
        strRackMstDetId = strRackMstDetId.Replace("'", "''");
        strHjId = strHjId.Replace("'", "''");
        strHjItemId = strHjItemId.Replace("'", "''");
        strHjRow = strHjRow.Replace("'", "''");
        strHjCol = strHjCol.Replace("'", "''");
        strPartsTitle = strPartsTitle.Replace("'", "''");
        strPartsNo = strPartsNo.Replace("'", "''");
        strColorSfx = strColorSfx.Replace("'", "''");
        strPartsNumSymbolCode = strPartsNumSymbolCode.Replace("'", "''");
        strPartsNumSymbolRtf = strPartsNumSymbolRtf.Replace("'", "''");

        //String sqlQuery = "UPDATE dt_RackMstDet SET hj_id = '" + strHjId + "', hj_item_id = '" + strHjItemId + "', hj_row = '" + strHjRow + "', hj_col = '" + strHjCol + "', parts_title = '" + strPartsTitle + "', parts_no = '" + strPartsNo + "', color_sfx = '" + strColorSfx + "', symbol_code = '" + strPartsNumSymbolCode + "', symbol_rtf = '" + strPartsNumSymbolRtf + "', rack_name = '" + strRackName + "', plc_no = '" + strPlcNo + "', proc_name= '" + strProcName + "' WHERE rack_det_id = '" + strRackMstDetId + "'";  //***ace_20160416_001
        String sqlQuery = "UPDATE dt_RackMstDet SET hj_id = '" + strHjId + "', hj_item_id = '" + strHjItemId + "', hj_row = '" + strHjRow + "', hj_col = '" + strHjCol + "', parts_title = '" + strPartsTitle + "', parts_no = '" + strPartsNo + "', color_sfx = '" + strColorSfx + "', symbol_code = '" + strPartsNumSymbolCode + "', symbol_rtf = '" + strPartsNumSymbolRtf + "', rack_name = '" + strRackName + "', plc_no = '" + strPlcNo + "', proc_name= '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE rack_det_id = '" + strRackMstDetId + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdPartsLoc(String strPartsNo, String strColorSfx, String strRackMstDetId, String strRackLoc)
    {
        String sqlQuery = "";
        strPartsNo = strPartsNo.Replace("'", "''");
        strColorSfx = strColorSfx.Replace("'", "''");
        strRackMstDetId = strRackMstDetId.Replace("'", "''");
        strRackLoc = strRackLoc.Replace("'", "''");

        if (strPartsNo == "" && strColorSfx == "" && strRackLoc == "")
        {
            sqlQuery = "UPDATE ais_PartsNum SET rack_det_id = NULL, rack_loc = NULL WHERE rack_det_id = '" + strRackMstDetId + "'";
        }
        else
        {
            sqlQuery = "UPDATE ais_PartsNum SET rack_det_id = '" + strRackMstDetId + "', rack_loc = '" + strRackLoc + "' WHERE part_no = '" + strPartsNo + "' AND color_sfx = '" + strColorSfx + "'";
        }

        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region Rack Master Detail - Module Address Selection
    public static DataSet SrcLmAddRackType(String strPlcNo, String strProcName, String strShowUsed, String strModuleAddFrm, String strModuleAddTo)
    {
        String sqlQuery = "";

        if (strShowUsed == "true")
        {
            sqlQuery = "SELECT LmAdd.* FROM dt_LampModuleTypeMst LmType, dt_LampModuleAddMst LmAdd WHERE LmAdd.proc_name = '" + strProcName + "' AND LmAdd.plc_no = '" + strPlcNo + "' AND LmAdd.module_type = LmType.module_type AND LmType.equip_type = 'Rack LM'";
        }
        else
        {
            sqlQuery = "SELECT LmAdd.* FROM dt_LampModuleTypeMst LmType, dt_LampModuleAddMst LmAdd WHERE LmAdd.proc_name = '" + strProcName + "' AND LmAdd.plc_no = '" + strPlcNo + "' AND LmAdd.module_type = LmType.module_type AND LmType.equip_type = 'Rack LM' AND rack_det_id IS NULL";
        }

        if (strModuleAddFrm != "")
        {
            if (strModuleAddTo != "")
            {
                sqlQuery = sqlQuery + " AND LmAdd.module_add >= " + strModuleAddFrm + " AND LmAdd.module_add <= " + strModuleAddTo;
            }
            else
            {
                sqlQuery = sqlQuery + " AND LmAdd.module_add = '" + strModuleAddFrm + "'";
            }
        }

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean DelRackMstDetModule(String strRackMstDetId, String strCurUser)  //***ace_20160416_001
    {
        //String sqlQuery = "UPDATE dt_RackMstDet SET module_add = NULL, module_name = NULL WHERE rack_det_id = '" + strRackMstDetId + "'";  //***ace_20160416_001
        String sqlQuery = "UPDATE dt_RackMstDet SET module_add = NULL, module_name = NULL, last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE rack_det_id = '" + strRackMstDetId + "'";

        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdRackDetModule(String strRackMstDetId, String strRackName, String strModuleAdd, String strModuleName, String strCurUser)  //***ace_20160416_001
    {
        strRackMstDetId = strRackMstDetId.Replace("'", "''");
        strRackName = strRackName.Replace("'", "''");
        strModuleAdd = strModuleAdd.Replace("'", "''");
        strModuleName = strModuleName.Replace("'", "''");

        //String sqlQuery = "UPDATE dt_RackMstDet SET rack_name = '" + strRackName + "', module_add = '" + strModuleAdd + "', module_name = '" + strModuleName + "' WHERE rack_det_id = '" + strRackMstDetId + "'";  //***ace_20160416_001
        String sqlQuery = "UPDATE dt_RackMstDet SET rack_name = '" + strRackName + "', module_add = '" + strModuleAdd + "', module_name = '" + strModuleName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE rack_det_id = '" + strRackMstDetId + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static String GetRackMstProcName(String strRackName)
    {
        String sqlQuery = "SELECT proc_name as ReturnField FROM dt_RackMst WHERE rack_name = '" + strRackName + "'";
        return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
    }

    public static String GetRackMstPlcNo(String strRackName)
    {
        String sqlQuery = "SELECT plc_no as ReturnField FROM dt_RackMst WHERE rack_name = '" + strRackName + "'";
        return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
    }

    public static String GetRackMstBlockName(String strRackName)
    {
        String sqlQuery = "SELECT block_name as ReturnField FROM dt_RackMst WHERE rack_name = '" + strRackName + "'";
        return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
    }

    public static String UpdLampModuleLoc(String strProcName, String strModuleAdd, String strModuleName, String strRackMstDetId, String strRackLoc)
    {
        String sqlQuery = "";
        strProcName = strProcName.Replace("'", "''");
        strModuleAdd = strModuleAdd.Replace("'", "''");
        strModuleName = strModuleName.Replace("'", "''");
        strRackMstDetId = strRackMstDetId.Replace("'", "''");
        strRackLoc = strRackLoc.Replace("'", "''");

        if (strModuleAdd == "" && strModuleName == "" && strRackLoc == "")
        {
            sqlQuery = "UPDATE dt_LampModuleAddMst SET rack_det_id = null, rack_loc = null WHERE proc_name = '" + strProcName + "' AND rack_det_id = '" + strRackMstDetId + "'";
        }
        else
        {
            sqlQuery = "UPDATE dt_LampModuleAddMst SET rack_det_id = '" + strRackMstDetId + "', rack_loc = '" + strRackLoc + "' WHERE proc_name = '" + strProcName + "' AND module_add = '" + strModuleAdd + "' AND module_name = '" + strModuleName + "'";
        }

        try
        {
            return ConnQuery.getIdentityExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }
    #endregion

    #endregion

    #region PLC Conversion Master
    public static DataSet SrcHjParts(String strModel, String strSfx, String strColor)
    {
        String sqlQuery = "SELECT DISTINCT part_no, color_sfx FROM ais_DataHJ WHERE model = '" + strModel + "' AND sfx = '" + strSfx + "' AND (color = '" + strColor + "' OR color = '*')";
        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean GetRackLightAvailability(String strModel, String strSfx, String strColor, String strRefPartsNum, String strRefColorSfx)
    {
        String sqlQuery = "SELECT Count(part_no) AS CountRow FROM ais_DataHJ WHERE model = '" + strModel + "' AND sfx = '" + strSfx + "' AND (color = '" + strColor + "' OR color = '*') AND part_no = '" + strRefPartsNum + "' AND color_sfx = '" + strRefColorSfx + "'";
        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static DataSet GetRackMstDetAis(String strRackMstDetId)
    {
        String sqlQuery = "SELECT hj_id, hj_item_id, hj_row, hj_col, parts_title, parts_no, color_sfx, symbol_code, symbol_rtf FROM dt_RackMstDet WHERE rack_det_id = '" + strRackMstDetId + "'";
        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static DataSet GetRackMstDetModule(String strRackMstDetId)
    {
        //***ace_20160419_001
        //String sqlQuery = "SELECT (RackDet.module_add + ' - ' + LmAdd.module_type) as module_add, RackDet.module_name, LmType.color_di FROM PGMDPS.dbo.dt_RackMstDet RackDet, PGMDPS.dbo.dt_LampModuleAddMst LmAdd, PGMDPS.dbo.dt_LampModuleTypeMst LmType WHERE RackDet.rack_det_id = '" + strRackMstDetId + "' AND LmAdd.module_add = RackDet.module_add AND LmAdd.module_type = LmType.module_type";
        //Modify WFKHOR 21-10-2020
        //String sqlQuery = "SELECT (RackDet.module_add + ' - ' + LmAdd.module_type) as module_add, RackDet.module_name, LmType.color_di, LmAdd.proc_name FROM dbo.dt_RackMstDet RackDet, dbo.dt_LampModuleAddMst LmAdd, dbo.dt_LampModuleTypeMst LmType WHERE RackDet.rack_det_id = '" + strRackMstDetId + "' AND LmAdd.module_add = RackDet.module_add AND LmAdd.module_type = LmType.module_type";
        String sqlQuery = "SELECT (RackDet.module_add + ' - ' + LmAdd.module_type + ' (' + LTRIM(RTRIM(STR(ISNULL(PhyAdd.physical_add,RackDet.module_add)))) + ')' ) as module_add, RackDet.module_name, LmType.color_di, LmAdd.proc_name FROM dbo.dt_RackMstDet RackDet INNER JOIN dbo.dt_LampModuleAddMst LmAdd ON LmAdd.module_add = RackDet.module_add AND LmAdd.proc_name = RackDet.proc_name INNER JOIN dbo.dt_LampModuleTypeMst LmType ON LmAdd.module_type = LmType.module_type LEFT JOIN dbo.dt_PhysicalAddMst PhyAdd ON RackDet.module_add = PhyAdd.module_add AND RackDet.proc_name = PhyAdd.proc_name WHERE RackDet.rack_det_id = '" + strRackMstDetId + "' ";
        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static void GetLightColor(String strModuleAdd, String strPlcNo, String strProcName, out String tmpLightDi, out String tmpColorDi, out String tmpLightAi, out String tmpColorAi)
    {
        tmpLightDi = "";
        tmpColorDi = "";
        tmpLightAi = "";
        tmpColorAi = "";

        String sqlQuery = "SELECT LmAdd.*, LmType.* FROM dt_LampModuleAddMst LmAdd, dt_LampModuleTypeMst LmType WHERE LmAdd.module_type = LmType.module_type and LmAdd.plc_no = '" + strPlcNo + "' AND LmAdd.proc_name = '" + strProcName + "' and LmAdd.module_add = '" + strModuleAdd + "' ORDER BY plc_no, proc_name, module_add";
        try
        {
            DataSet dsLmSearch = new DataSet();
            DataTable dtLmSearch = new DataTable();

            dsLmSearch = ConnQuery.getBindingDatasetData(sqlQuery);
            dtLmSearch = dsLmSearch.Tables[0];

            if (dtLmSearch.Rows.Count > 0)
            {
                tmpLightDi = Convert.ToString(dtLmSearch.Rows[0]["light_di"]).Trim();
                tmpColorDi = Convert.ToString(dtLmSearch.Rows[0]["color_di"]).Trim();
                tmpLightAi = Convert.ToString(dtLmSearch.Rows[0]["light_ai"]).Trim();
                tmpColorAi = Convert.ToString(dtLmSearch.Rows[0]["color_ai"]).Trim();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    public static int[] GetRGB(String strLighting, String strColor)
    {
        String sqlQuery = "SELECT r, g, b FROM dps_ColorChart WHERE lighting = '" + strLighting + "'";
        if (strColor != "")
        {
            sqlQuery = sqlQuery + " AND color = '" + strColor + "'";
        }

        try
        {
            DataSet dsColorChart = new DataSet();
            DataTable dtColorChart = new DataTable();
            int[] iColorChart = new int[3];

            dsColorChart = ConnQuery.getBindingDatasetData(sqlQuery);
            dtColorChart = dsColorChart.Tables[0];
            if (dtColorChart.Rows.Count > 0)
            {
                if (Convert.ToString(dtColorChart.Rows[0]["r"]).Trim() != "")
                {
                    iColorChart[0] = Convert.ToInt32(dtColorChart.Rows[0]["r"]);
                }
                if (Convert.ToString(dtColorChart.Rows[0]["g"]).Trim() != "")
                {
                    iColorChart[1] = Convert.ToInt32(dtColorChart.Rows[0]["g"]);
                }
                if (Convert.ToString(dtColorChart.Rows[0]["b"]).Trim() != "")
                {
                    iColorChart[2] = Convert.ToInt32(dtColorChart.Rows[0]["b"]);
                }
            }
            return iColorChart;
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static DataSet SrcInsCode(String strProcName, String strGroupName, String strBlockName, String strInsCode)
    {
        String strInsCodeCond = "";
        if (strInsCode != "")
        {
            strInsCodeCond = " InsCode.ins_code = '" + strInsCode + "' AND ";
        }

        String sqlQuery = "SELECT DISTINCT InsCode.* FROM dt_DpsInsCodeMst InsCode, dt_RackMst RackMst, dt_RackMstDet RackMstDet, ais_DataHJ DataHj WHERE RackMst.proc_name = '" + strProcName + "' AND RackMst.group_name = '" + strGroupName + "' AND RackMst.block_name = '" + strBlockName + "' AND " + strInsCodeCond + "RackMst.rack_name = RackMstDet.rack_name AND (InsCode.color = DataHj.color OR DataHj.color = '*') AND InsCode.model = DataHj.model AND InsCode.sfx = DataHj.sfx AND RackMstDet.parts_no = DataHj.part_no AND RackMstDet.color_sfx = DataHj.color_sfx AND RackMstDet.module_add IS NOT NULL ORDER BY InsCode.ins_code";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static DataSet SrcLmAddPerBlock(String strPlcNo, String strProcName, String strBlockName)
    {
        //String sqlQuery = "SELECT LmAdd.*, LmType.* from dt_LampModuleAddMst LmAdd, dt_LampModuleTypeMst LmType, dt_RackMst RackMst, dt_RackMstDet RackMstDet WHERE LmAdd.rack_det_id != '' AND LmAdd.plc_no = '" + strPlcNo + "' AND LmAdd.proc_name = '" + strProcName + "' AND RackMst.block_name = '" + strBlockName + "' AND LmAdd.proc_name = RackMst.proc_name AND LmAdd.rack_det_id = RackMstDet.rack_det_id AND RackMst.rack_name = RackMstDet.rack_name AND LmAdd.module_type = LmType.module_type ORDER BY LmAdd.rack_loc";
        String sqlQuery = "SELECT LmAdd.*, LmType.* , LTRIM(RTRIM(STR(ISNULL(PhyAdd.physical_add,LmAdd.module_add)))) AS PhyAdd, CONVERT(INT, LTRIM(RTRIM(STR(ISNULL(PhyAdd.physical_add,LmAdd.module_add))))) AS PhyAddOrder FROM dt_LampModuleAddMst LmAdd INNER JOIN dt_LampModuleTypeMst LmType ON LmAdd.module_type = LmType.module_type INNER JOIN dt_RackMst RackMst ON LmAdd.proc_name = RackMst.proc_name  INNER JOIN dt_RackMstDet RackMstDet ON LmAdd.rack_det_id = RackMstDet.rack_det_id AND  RackMst.rack_name = RackMstDet.rack_name LEFT JOIN dt_PhysicalAddMst PhyAdd ON LmAdd.proc_name = PhyAdd.proc_name AND LmAdd.module_add = PhyAdd.module_add WHERE LmAdd.rack_det_id != '' AND LmAdd.plc_no = '" + strPlcNo + "' AND LmAdd.proc_name = '" + strProcName + "' AND RackMst.block_name = '" + strBlockName + "' ORDER BY PhyAddOrder";
        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static DataSet GetLmAddPerBlock(String strPlcNo, String strProcName, String strGroupName, String strBlockName, String strInsCode)
    {
        //***ace_20160419_001
        //String sqlQuery = "SELECT DISTINCT RackMstDet.module_add, InsCode.ins_code, BlockMst.gw_no FROM dt_DpsInsCodeMst InsCode, PGMDPS.dbo.dt_BlockMst BlockMst, dt_RackMst RackMst, dt_RackMstDet RackMstDet, ais_DataHJ DataHj WHERE RackMst.proc_name = '" + strProcName + "' AND RackMst.group_name = '" + strGroupName + "' AND RackMst.block_name = '" + strBlockName + "' AND RackMst.block_name = BlockMst.block_name AND RackMstDet.module_add IS NOT NULL AND RackMst.rack_name = RackMstDet.rack_name AND InsCode.model = DataHj.model AND InsCode.sfx = DataHj.sfx AND (InsCode.color = DataHj.color OR DataHj.color = '*') AND RackMstDet.parts_no = DataHj.part_no AND RackMstDet.color_sfx = DataHj.color_sfx";
        //String sqlQuery = "SELECT DISTINCT RackMstDet.module_add, InsCode.ins_code, BlockMst.gw_no FROM dt_DpsInsCodeMst InsCode, dbo.dt_BlockMst BlockMst, dt_RackMst RackMst, dt_RackMstDet RackMstDet, ais_DataHJ DataHj WHERE RackMst.proc_name = '" + strProcName + "' AND RackMst.group_name = '" + strGroupName + "' AND RackMst.block_name = '" + strBlockName + "' AND RackMst.block_name = BlockMst.block_name AND RackMstDet.module_add IS NOT NULL AND RackMst.rack_name = RackMstDet.rack_name AND InsCode.model = DataHj.model AND InsCode.sfx = DataHj.sfx AND (InsCode.color = DataHj.color OR DataHj.color = '*') AND RackMstDet.parts_no = DataHj.part_no AND RackMstDet.color_sfx = DataHj.color_sfx";
        String sqlQuery = "SELECT A.*, LTRIM(RTRIM(STR(ISNULL(B.physical_add, A.module_add)))) AS PhyAdd  FROM (SELECT DISTINCT RackMstDet.module_add, InsCode.ins_code, BlockMst.gw_no FROM dt_DpsInsCodeMst InsCode, dbo.dt_BlockMst BlockMst, dt_RackMst RackMst, dt_RackMstDet RackMstDet, ais_DataHJ DataHj WHERE RackMst.proc_name = '" + strProcName + "' AND RackMst.group_name = '" + strGroupName + "' AND RackMst.block_name = '" + strBlockName + "' AND RackMst.block_name = BlockMst.block_name AND RackMstDet.module_add IS NOT NULL AND RackMst.rack_name = RackMstDet.rack_name AND InsCode.model = DataHj.model AND InsCode.sfx = DataHj.sfx AND (InsCode.color = DataHj.color OR DataHj.color = '*') AND RackMstDet.parts_no = DataHj.part_no AND RackMstDet.color_sfx = DataHj.color_sfx ";
        if (strInsCode != "")
        {
            sqlQuery = sqlQuery + " AND InsCode.ins_code = '" + strInsCode + "'";
        }

        sqlQuery = sqlQuery + ") AS A LEFT JOIN dt_PhysicalAddMst B ON A.module_add = B.module_add AND B.proc_name = '" + strProcName + "'";
        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean ChkLmTypeMaxCnt(String strPlcNo, String strProcName, String strBlockName)
    {
        String sqlQuery = "SELECT COUNT(DISTINCT(LmAdd.module_type)) AS ReturnField from dt_LampModuleAddMst LmAdd, dt_RackMst RackMst, dt_RackMstDet RackMstDet WHERE LmAdd.rack_det_id != '' AND LmAdd.plc_no = '" + strPlcNo + "' AND LmAdd.proc_name = '" + strProcName + "' AND RackMst.block_name = '" + strBlockName + "' AND LmAdd.proc_name = RackMst.proc_name AND LmAdd.rack_det_id = RackMstDet.rack_det_id AND RackMst.rack_name = RackMstDet.rack_name";
        try
        {
            int iInsCodeCnt = Convert.ToInt32(ConnQuery.getReturnFieldExecuteReader(sqlQuery));
            if (iInsCodeCnt > 12)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkLmAddMaxCnt(String strPlcNo, String strProcName, String strBlockName)
    {
        String sqlQuery = "SELECT COUNT(DISTINCT(LmAdd.module_add) AS ReturnField from dt_LampModuleAddMst LmAdd, dt_RackMst RackMst, dt_RackMstDet RackMstDet WHERE LmAdd.rack_det_id != '' AND LmAdd.plc_no = '" + strPlcNo + "' AND LmAdd.proc_name = '" + strProcName + "' AND RackMst.block_name = '" + strBlockName + "' AND LmAdd.proc_name = RackMst.proc_name AND LmAdd.rack_det_id = RackMstDet.rack_det_id AND RackMst.rack_name = RackMstDet.rack_name";
        try
        {
            int iLmAddCnt = Convert.ToInt32(ConnQuery.getReturnFieldExecuteReader(sqlQuery));
            //if (iLmAddCnt > 64)
            if (iLmAddCnt > 55)  //***ace_20160419_002
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkDuplicateLmAddPhysical(String strPlcNo, String strProcName, String strBlockName)
    {
        //Modify WFKHOR 21-10-2020
        String sqlQuery = "SELECT COUNT (physical_add) AS ReturnField FROM dt_LampModuleAddMst A  INNER JOIN dt_PhysicalAddMst B ON A.proc_name = B.proc_name AND A.module_add = B.module_add INNER JOIN dt_RackMst C ON C.plc_no = A.plc_no AND C.proc_name = A.proc_name AND A.module_name = C.block_name WHERE A.proc_name = '" + strProcName + "' AND A.plc_no = '" + strPlcNo + "'  AND C.block_name = '" + strBlockName + "' GROUP BY B.physical_add ORDER BY ReturnField DESC";
        try
        {
            int iLmAddCnt = Convert.ToInt32(ConnQuery.getReturnFieldExecuteReader(sqlQuery));
            if (iLmAddCnt > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkLmAddPhysicalNotOneNTwo(String strPlcNo, String strProcName, String strBlockName)
    {
        //Modify WFKHOR 21-10-2020
        String sqlQuery = "SELECT COUNT (physical_add) AS ReturnField FROM dt_LampModuleAddMst A  INNER JOIN dt_PhysicalAddMst B ON A.proc_name = B.proc_name AND A.module_add = B.module_add INNER JOIN dt_RackMst C ON C.plc_no = A.plc_no AND C.proc_name = A.proc_name WHERE A.rack_det_id != '' AND A.proc_name = '" + strProcName + "' AND A.plc_no = '" + strPlcNo + "'  AND C.block_name = '" + strBlockName + "' AND B.physical_add IN ('1', '2')";
        try
        {
            int iLmAddCnt = Convert.ToInt32(ConnQuery.getReturnFieldExecuteReader(sqlQuery));
            if (iLmAddCnt > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkLmChange(String strPlcNo, String strProcName, String strBlockName)
    {
        String sqlQuery = "SELECT submit_all_flag AS ReturnField from dt_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND block_name = '" + strBlockName + "'";
        try
        {
            String strSubmitAllFlg = ConnQuery.getReturnFieldExecuteReader(sqlQuery);
            if (strSubmitAllFlg == "True")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdLmChange(String strPlcNo, String strProcName, String strBlockName, String strFlag)
    {
        String sqlQuery = "";

        if (strFlag == "NULL")
        {
            sqlQuery = "UPDATE dt_BlockMst SET submit_all_flag = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND block_name = '" + strBlockName + "'";
        }
        else
        {
            sqlQuery = "UPDATE dt_BlockMst SET submit_all_flag = 'True' WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND block_name = '" + strBlockName + "'";
        }

        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region Conversion Result

    #region Group Master Conversion
    public static Boolean SvGroupMstConv(String strPlcNo, String strProcName, String tmpGroupId, String tmpGroupName, String tmpGroupLine)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        tmpGroupId = tmpGroupId.Replace("'", "''");
        tmpGroupName = tmpGroupName.Replace("'", "''");
        tmpGroupLine = tmpGroupLine.Replace("'", "''");

        String sqlQuery = "INSERT INTO conv_GroupMst (plc_no, proc_name, group_id, group_name, group_line) VALUES ('" + strPlcNo + "', '" + strProcName + "','" + tmpGroupId + "','" + tmpGroupName + "','" + tmpGroupLine + "')";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ClrGroupMstConv(String strPlcNo, String strProcName)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");

        String sqlQuery = "DELETE FROM conv_GroupMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static DataSet SrcGroupMstConv(String strPlcNo, String strProcName)
    {
        String sqlQuery = "SELECT * FROM conv_GroupMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }
    #endregion

    #region Block Master Conversion
    public static Boolean SvBlockMstConv(String strPlcNo, String strProcName, String tmpGwNo, String tmpGroupId, String tmpBlockSeq, String tmpBlockName)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        tmpGwNo = tmpGwNo.Replace("'", "''");
        tmpGroupId = tmpGroupId.Replace("'", "''");
        tmpBlockSeq = tmpBlockSeq.Replace("'", "''");
        tmpBlockName = tmpBlockName.Replace("'", "''");

        String sqlQuery = "UPDATE conv_BlockMst SET group_id = '" + tmpGroupId + "', block_seq = '" + tmpBlockSeq + "', block_name = '" + tmpBlockName + "' WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + tmpGwNo + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ClrBlockMstConv(String strPlcNo, String strProcName)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");

        String sqlQuery = "UPDATE conv_BlockMst SET group_id = NULL, block_seq = NULL, block_name = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static DataSet SrcBlockMstConv(String strPlcNo, String strProcName)
    {
        String sqlQuery = "SELECT * FROM conv_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' ORDER BY gw_no";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }
    #endregion

    #region Lamp Module Conversion
    public static String GetLmModeId(String strPlcNo, String strProcName, String tmpGwNo)
    {
        String sqlQuery = "SELECT MIN(mode_cnt) AS ReturnField FROM conv_LampModuleMode WHERE mode_data IS NULL AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + tmpGwNo + "'";
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    public static Boolean SvLmModeConv(String strPlcNo, String strProcName, String tmpGwNo, String tmpModeCnt, String tmpData)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        tmpGwNo = tmpGwNo.Replace("'", "''");
        tmpData = tmpData.Replace("'", "''");

        String sqlQuery = "UPDATE conv_LampModuleMode SET mode_data = '" + tmpData + "' WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + tmpGwNo + "' AND mode_cnt = '" + tmpModeCnt + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ClrLmModeConv(String strPlcNo, String strProcName, String strGwNo)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");

        String sqlQuery = "UPDATE conv_LampModuleMode SET mode_data = NULL WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + strGwNo + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static DataSet SrcLmAddModeConv(String strPlcNo, String strProcName, int iGwNo)
    {
        //***ace_20160419_001
        //String sqlQuery = "SELECT * FROM conv_LampModuleMode WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + iGwNo + "'";
        String sqlQuery = "SELECT * FROM conv_LampModuleMode WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + iGwNo + "' ORDER BY gw_no, mode_cnt";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean UpdLmAddMatchingConv(String strPlcNo, String strProcName, String tmpGwNo, String tmpModeCnt, String tmpLmAdd)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        tmpGwNo = tmpGwNo.Replace("'", "''");
        tmpLmAdd = tmpLmAdd.Replace("'", "''");

        String sqlQuery = "INSERT INTO conv_LampModuleAddMatching (lm_add, lm_mode_cnt, plc_no, proc_name, gw_no) VALUES ('" + tmpLmAdd + "', '" + tmpModeCnt + "', '" + strPlcNo + "', '" + strProcName + "', '" + tmpGwNo + "')";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ClrLmAddMatchingConv(String strPlcNo, String strProcName, String strGwNo)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");

        String sqlQuery = "DELETE FROM conv_LampModuleAddMatching WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + strGwNo + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static DataSet SrcLmAddMatchingConv(String strPlcNo, String strProcName, int iGwNo)
    {
        String sqlQuery = "SELECT * FROM conv_LampModuleAddMatching WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + iGwNo + "'";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }
    #endregion

    #region Lamp Module Rack Mode Conversion
    public static Boolean SaveLmRackMode(String strPlcNo, String strProcName, String tmpGwNo, String strModuleType, int iModeCnt)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        tmpGwNo = tmpGwNo.Replace("'", "''");
        strModuleType = strModuleType.Replace("'", "''");

        String sqlQuery = "INSERT INTO conv_LampModuleModeRack (plc_no, proc_name, gw_no, module_type, mode_cnt) VALUES ('" + strPlcNo + "', '" + strProcName + "','" + tmpGwNo + "','" + strModuleType + "','" + iModeCnt + "')";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdLmRackMode(String strPlcNo, String strProcName, String tmpGwNo, String strModuleType, int iModeCnt)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        tmpGwNo = tmpGwNo.Replace("'", "''");
        strModuleType = strModuleType.Replace("'", "''");

        String sqlQuery = "INSERT INTO conv_LampModuleModeRack (plc_no, proc_name, gw_no, module_type, mode_cnt) VALUES ('" + strPlcNo + "', '" + strProcName + "','" + tmpGwNo + "','" + strModuleType + "','" + iModeCnt + "')";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean DeleteLmRackMode(String strPlcNo, String strProcName, String tmpGwNo)
    {
        String sqlQuery = "DELETE FROM conv_LampModuleModeRack WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + tmpGwNo + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static String GetLmRackMode(String strPlcNo, String strProcName, String tmpGwNo, String strModuleType)
    {
        String sqlQuery = "SELECT MAX(mode_cnt) AS ReturnField FROM conv_LampModuleModeRack WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND gw_no = '" + tmpGwNo + "'";

        if (strModuleType != "")
        {
            sqlQuery = sqlQuery + " AND module_type = '" + strModuleType + "'";
        }

        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }
    #endregion

    #region Instruction Code Conversion
    public static String GetInsCodeConvId(String tmpInscode, String strPlcNo, String strProcName)
    {
        tmpInscode = tmpInscode.Replace("'", "''");

        String sqlQuery = "SELECT ins_code_cnt AS ReturnField FROM conv_InsCode WHERE ins_code = '" + tmpInscode + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
        try
        {
            String strInsCnt = ConnQuery.getReturnFieldExecuteReader(sqlQuery);

            if (strInsCnt == "")
            {
                sqlQuery = "SELECT MIN(ins_code_cnt) AS ReturnField FROM conv_InsCode WHERE ins_code IS NULL AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
                strInsCnt = ConnQuery.getReturnFieldExecuteReader(sqlQuery);
            }

            return strInsCnt;
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    public static Boolean UpdInsCodeConv(String tmpInscode, String tmpModel, String tmpKatashiki, String tmpSfx, String tmpColor, String tmpComment, String byteInsCode, String byteModel, String byteKatashiki, String byteSfx, String byteColor, String byteComment, int iInsCodeCnt, String strPlcNo, String strProcName)
    {
        tmpInscode = tmpInscode.Replace("'", "''");
        tmpModel = tmpModel.Replace("'", "''");
        tmpKatashiki = tmpKatashiki.Replace("'", "''");
        tmpSfx = tmpSfx.Replace("'", "''");
        tmpColor = tmpColor.Replace("'", "''");
        tmpComment = tmpComment.Replace("'", "''");

        String sqlQuery = "UPDATE conv_InsCode SET ins_code = '" + tmpInscode + "', model = '" + tmpModel + "', katashiki = '" + tmpKatashiki + "', sfx = '" + tmpSfx + "', color = '" + tmpColor + "', comment = '" + tmpComment + "', byte_ins_code = '" + byteInsCode + "', byte_model = '" + byteModel + "', byte_katashiki = '" + byteKatashiki + "', byte_sfx = '" + byteSfx + "', byte_color = '" + byteColor + "', byte_comment = '" + byteComment + "' WHERE ins_code_cnt = '" + iInsCodeCnt + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean DelInsCodeConv(int iInsCodeCnt, String strPlcNo, String strProcName)
    {
        String sqlQuery = "UPDATE conv_InsCode SET ins_code = NULL, model = NULL, katashiki = NULL, sfx = NULL, color = NULL, comment = NULL WHERE ins_code_cnt = '" + iInsCodeCnt + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static DataSet SrcInsCodeConv(String tmpInsCode, String strPlcNo, String strProcName)
    {
        String sqlQuery = "";

        if (tmpInsCode == "")
        {
            sqlQuery = "SELECT * FROM conv_InsCode WHERE ins_code IS NOT NULL AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' order by ins_code_cnt";
        }
        else
        {
            sqlQuery = "SELECT * FROM conv_InsCode WHERE ins_code = '" + tmpInsCode + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' order by ins_code_cnt";
        }

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean UpdInsCodeMapConv(int tmpInsCodeCnt, String strPlcNo, String strProcName, String tmpGwNo, String tmpInsCodeMap)
    {
        tmpGwNo = tmpGwNo.Replace("'", "''");
        tmpInsCodeMap = tmpInsCodeMap.Replace("'", "''");

        String sqlQuery = "UPDATE conv_InsCodeMap SET [" + tmpGwNo + "] = '" + tmpInsCodeMap + "' WHERE ins_code_cnt = '" + tmpInsCodeCnt + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean DelInsCodeMapConv(int tmpInsCodeCnt, String strPlcNo, String strProcName, String tmpGwNo)
    {
        tmpGwNo = tmpGwNo.Replace("'", "''");
        String strGwCond = "[" + tmpGwNo + "] = NULL";

        if (tmpGwNo == "")
        {
            strGwCond = "[1] = NULL, [2] = NULL, [3] = NULL, [4] = NULL, [5] = NULL, [6] = NULL, [7] = NULL, [8] = NULL, [9] = NULL, [10] = NULL, [11] = NULL, [12] = NULL";
        }
        String sqlQuery = "UPDATE conv_InsCodeMap SET " + strGwCond + " WHERE ins_code_cnt = '" + tmpInsCodeCnt + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static DataSet SrcInsCodeMapConv(int tmpInsCodeCnt, String strPlcNo, String strProcName)
    {
        String sqlQuery = "SELECT * FROM conv_InsCodeMap WHERE ins_code_cnt = '" + tmpInsCodeCnt + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "'";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }
    #endregion

    #endregion

    #region Manual Pointer Change
    public static Boolean updPlcPointerMst(String strPlcNo, String strWritePointer)
    {
        String sqlQuery = "UPDATE dt_PlcPointerMst SET pointer = '" + strWritePointer + "' WHERE plc_no = '" + strPlcNo + "' AND flag_type = 'W'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static DataSet SrcConvResult(String strPlcNo)
    {
        String sqlQuery = "SELECT * FROM dt_DpsResultConv WHERE plc_no = '" + strPlcNo + "' ORDER BY write_pointer";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    #endregion

    #region Manual Update DPS Instruction Data
    public static DataSet searchDpsRsConv(String strDpsRsConvId, String strInsCode, String strPointer, String strIdNo, String strIdVer, String strChassisNo, String strBseq, String strModel, String strSfx, String strColor, String strPlcNo)
    {
        String filterCriteria = "";

        if (strDpsRsConvId != "")
        {
            filterCriteria = filterCriteria + " AND dps_rs_conv_id = '" + strDpsRsConvId + "'";
        }

        if (strInsCode != "")
        {
            filterCriteria = filterCriteria + " AND dps_ins_code = '" + strInsCode + "'";
        }

        if (strPointer != "")
        {
            filterCriteria = filterCriteria + " AND write_pointer= '" + strPointer + "'";
        }

        if (strIdNo != "")
        {
            filterCriteria = filterCriteria + " AND id_no = '" + strIdNo + "'";
        }

        if (strIdVer != "")
        {
            filterCriteria = filterCriteria + " AND id_ver = '" + strIdVer + "'";
        }

        if (strChassisNo != "")
        {
            filterCriteria = filterCriteria + " AND chassis_no = '" + strChassisNo + "'";
        }

        if (strBseq != "")
        {
            filterCriteria = filterCriteria + " AND bseq = '" + strBseq + "'";
        }

        if (strModel != "")
        {
            filterCriteria = filterCriteria + " AND model= '" + strModel + "'";
        }

        if (strSfx != "")
        {
            filterCriteria = filterCriteria + " AND sfx = '" + strSfx + "'";
        }

        if (strColor != "")
        {
            filterCriteria = filterCriteria + " AND color_code = '" + strColor + "'";
        }

        if (strPlcNo != "")
        {
            filterCriteria = filterCriteria + " AND plc_no = '" + strPlcNo + "'";
        }

        String sqlQuery = "SELECT * FROM dt_DpsResultConv WHERE dps_rs_conv_id != '' AND read_flag != '1' " + filterCriteria + " ORDER BY plc_no, write_pointer, read_flag";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean deleteDpsRsConv(String strDpsRsConvId)
    {
        String sqlQuery = "DELETE FROM dt_DpsResultConv WHERE dps_rs_conv_id = '" + strDpsRsConvId + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            return false;
        }
    }

    public static Boolean updateDpsRsConv(String strPlcNo, String strPointer, String strModel, String strSfx, String strColor, String strInsCode, String strBseq, String strIdNo, String strIdVer, String strChassisNo, String strProcName, String strDpsRsConvId)
    {
        strPlcNo = strPlcNo.Replace("'", "''");
        strPointer = strPointer.Replace("'", "''");
        strModel = strModel.Replace("'", "''");
        strSfx = strSfx.Replace("'", "''");
        strColor = strColor.Replace("'", "''");
        strInsCode = strInsCode.Replace("'", "''");
        strBseq = strBseq.Replace("'", "''");
        strIdNo = strIdNo.Replace("'", "''");
        strIdVer = strIdVer.Replace("'", "''");
        strChassisNo = strChassisNo.Replace("'", "''");
        strProcName = strProcName.Replace("'", "''");
        strDpsRsConvId = strDpsRsConvId.Replace("'", "''");

        String sqlQuery = "UPDATE dt_DpsResultConv SET plc_no = '" + strPlcNo + "', write_pointer = '" + strPointer + "', model = '" + strModel + "', sfx = '" + strSfx + "', color_code = '" + strColor + "', dps_ins_code = '" + strInsCode + "', bseq = '" + strBseq + "', id_no = '" + strIdNo + "', id_ver = '" + strIdVer + "', chassis_no = '" + strChassisNo + "', last_updated = CURRENT_TIMESTAMP WHERE dps_rs_conv_id = '" + strDpsRsConvId + "'";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            return false;
        }
    }
    #endregion

    #region Save Rack Master Export

    public static String validaterackmstExport(string rack_name, Int32 plc_no, string proc_name, string group_name, string block_name)
    {


        rack_name = rack_name.Replace("'", "''");
        proc_name = proc_name.Replace("'", "''");
        group_name = group_name.Replace("'", "''");
        block_name = block_name.Replace("'", "''");


        String sqlQuery = "SELECT rack_name AS ReturnField FROM dt_RackMst WHERE rack_name = '" + rack_name + "' AND plc_no = '" + plc_no + "' AND proc_name = '" + proc_name + "' AND group_name = '" + group_name + "' AND block_name = '" + block_name + "'  ";
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    public static String GetModuleLocExport(string procname, String Molude, string strCriteria)
    {


        Molude = Molude.Replace("'", "''");
        procname = procname.Replace("'", "''");

        String sqlQuery = "SELECT rack_det_id AS ReturnField FROM dt_LampModuleAddMst WHERE module_add = '" + Molude + "' AND proc_name = '" + procname + "' " + strCriteria;
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    public static String GetModuletypeLocExport(String procname, string Molude)
    {


        Molude = Molude.Replace("'", "''");
        procname = procname.Replace("'", "''");

        String sqlQuery = "SELECT module_type AS ReturnField FROM dt_LampModuleAddMst WHERE module_add = '" + Molude + "' AND proc_name = '" + procname + "'  ";
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    public static String GetPartsRackLocExport(String strPartsNo, String strColorSfx, string strCriteria)
    {


        // to make sure the part no and color is not exist in 
        strPartsNo = strPartsNo.Replace("'", "''");
        strColorSfx = strColorSfx.Replace("'", "''");

        String sqlQuery = "SELECT rack_loc AS ReturnField FROM ais_PartsNum WHERE part_no = '" + strPartsNo + "' AND color_sfx = '" + strColorSfx + "' " + strCriteria;
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }

    //public static String GetPartsRackLocExport(String strPartsNo, String strColorSfx, string Rackname)
    //{


    //    // to make sure the part no and color is not exist in 
    //    strPartsNo = strPartsNo.Replace("'", "''");
    //    strColorSfx = strColorSfx.Replace("'", "''");

    //    String sqlQuery = "SELECT rack_loc AS ReturnField FROM ais_PartsNum WHERE part_no = '" + strPartsNo + "' AND color_sfx = '" + strColorSfx + "' AND substring(rack_det_id,1, CHARINDEX('^',rack_det_id)-1)  != '" + Rackname + "'";
    //    try
    //    {
    //        return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
    //    }
    //    catch (Exception ex)
    //    {
    //        GlobalFunc.Log(ex);
    //        GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
    //        return "";
    //    }
    //}

    public static String getSinglefield(String strFileName, string dictname, string strsearchvalue, string dictname2, string strsearchvalue2, string returnvalue)
    {
        String sqlQuery = "SELECT " + returnvalue + " as ReturnField  FROM " + strFileName + " WHERE " + dictname + " = '" + strsearchvalue + "'  AND " + dictname2 + " = '" + strsearchvalue2 + "'";
        return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
    }



    #endregion

    #endregion

    public static String getPlcNo(String strProcName)
    {
        String sqlQuery = "SELECT plc_no as ReturnField FROM dt_DpsPlcMst WHERE proc_name = '" + strProcName + "'";
        return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
    }

    #region Get PLC Conversion Type
    public static String GetPLCConvType(String strPlcNo, String strProcName)
    {
        String sqlQuery = "SELECT A.conv_type AS ReturnField FROM dt_PlcModelMst A INNER JOIN dt_DpsPlcMst B ON A.plcmodel_no = B.plc_model WHERE proc_name = '" + strProcName + "' AND B.plc_no	= '" + strPlcNo + "' ";
        try
        {
            return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return "";
        }
    }
    #endregion

    public static DataSet InitialiaseTempLapMappingData(String strPlcNo, String strProcName, String strBlockName, String strInsCode)
    {
        String SubsqlQuery = "SELECT LTRIM(RTRIM(STR(ISNULL(PhyAdd.physical_add, LmAdd.module_add)))) AS Select_physical_add, LmAdd.module_add AS Select_module_add  FROM dt_LampModuleAddMst LmAdd INNER JOIN dt_LampModuleTypeMst LmType ON LmAdd.module_type = LmType.module_type INNER JOIN dt_RackMst RackMst ON LmAdd.proc_name = RackMst.proc_name  INNER JOIN dt_RackMstDet RackMstDet ON LmAdd.rack_det_id = RackMstDet.rack_det_id AND  RackMst.rack_name = RackMstDet.rack_name LEFT JOIN dt_PhysicalAddMst PhyAdd ON LmAdd.proc_name = PhyAdd.proc_name AND LmAdd.module_add = PhyAdd.module_add WHERE LmAdd.rack_det_id != '' AND LmAdd.plc_no = '" + strPlcNo + "' AND LmAdd.proc_name = '" + strProcName + "' AND RackMst.block_name = '" + strBlockName + "' ";



        String sqlQuery = "DELETE FROM dbo.conv_TempLampMapping WHERE plc_no='" + strPlcNo + "' AND proc_name = '" + strProcName + "'; ";

        for (int i = 3; i <= 64; i++)
        {
            sqlQuery = sqlQuery + "INSERT INTO dbo.conv_TempLampMapping(plc_no, proc_name, ins_code, gw_no, module_add, physical_add) values ('" + strPlcNo + "', '" + strProcName + "', '" + strInsCode + "', NULL, '0', '" + i.ToString() + "'); ";

        }
        sqlQuery = sqlQuery + "UPDATE dbo.conv_TempLampMapping SET module_add = Select_module_add FROM (" + SubsqlQuery + ") AS A WHERE physical_add = Select_physical_add;";
        sqlQuery = sqlQuery + " DELETE FROM dbo.conv_TempLampMapping WHERE plc_no='" + strPlcNo + "' AND proc_name = '" + strProcName + "' AND physical_add > (SELECT MAX(physical_add) FROM dbo.conv_TempLampMapping WHERE module_add !='0' AND plc_no='" + strPlcNo + "' AND proc_name = '" + strProcName + "');";




        ConnQuery.ExecuteQuery(sqlQuery);


        sqlQuery = "SELECT * FROM dbo.conv_TempLampMapping WHERE plc_no='" + strPlcNo + "' AND proc_name = '" + strProcName + "';";


        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    #region GearUpLM2 

    public static DataSet SrcGearUp(String strPlcNo, String strPartID, String strLineAGwNo, String strLineAModAddr, String strLineAPhysAddr, String strLineBGwNo, String strLineBModAddr, String strLineBPhysAddr)
    {
        String filterCriteria = "";

        if (strPlcNo != "")
        {
            filterCriteria = filterCriteria + " AND plc_no = '" + strPlcNo + "'";
        }
        if (strPartID != "")
        {
            filterCriteria = filterCriteria + " AND PartsID = '" + strPartID + "'";
        }
        if (strLineAGwNo != "")
        {
            filterCriteria = filterCriteria + " AND LineA_GwNo = '" + strLineAGwNo + "'";
        }
        if (strLineAModAddr != "")
        {
            filterCriteria = filterCriteria + " AND LineA_LmModuleAddress = '" + strLineAModAddr + "'";
        }
        if (strLineAPhysAddr != "")
        {
            filterCriteria = filterCriteria + " AND LineA_LmPhysicalAddress = '" + strLineAPhysAddr + "'";
        }
        if (strLineBGwNo != "")
        {
            filterCriteria = filterCriteria + " AND LineB_GwNo = '" + strLineBGwNo + "'";
        }
        if (strLineBModAddr != "")
        {
            filterCriteria = filterCriteria + " AND LineB_LmModuleAddress = '" + strLineBModAddr + "'";
        }
        if (strLineBPhysAddr != "")
        {
            filterCriteria = filterCriteria + " AND LineB_LmPhysicalAddress = '" + strLineBPhysAddr + "'";
        }

        String sqlQuery = "SELECT PartsID ,plc_no ,proc_name ,LineA_GwNo ,LineA_LmModuleAddress ,LineA_LmPhysicalAddress ,LineB_GwNo ,LineB_LmModuleAddress ,LineB_LmPhysicalAddress ,gu.last_upd_by ,gu.last_upd_dt FROM GearUpLM2 gu JOIN dt_DpsPlcMst plc ON gu.PlcNo = plc.plc_no WHERE PartsID != ''  " + filterCriteria + " ORDER BY plc_no, PartsID ";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean ChkAllDuplicateGWNo(String strPlcNo, String strLineAGwNo, String strLineBGwNo)
    {

        //String sqlQuery = "SELECT COUNT(*) AS CountRow FROM GearUpLM2 WHERE PlcNo = '" + strPlcNo + "' "
        //    + (strLine == "A" ? "AND LineA_GwNo" : strLine == "B" ? "AND LineB_GwNo" : "") + " = '" + strGWNo + "'";

        String sqlQuery = "SELECT COUNT(*) AS CountRow FROM GearUpLM2 WHERE PlcNo = '" + strPlcNo + "' AND (LineA_GwNo = '"
            + GlobalFunc.getReplaceFrmUrl(strLineAGwNo) + "' OR LineB_GwNo = '" + GlobalFunc.getReplaceFrmUrl(strLineAGwNo) + "' OR LineA_GwNo = '"
            + GlobalFunc.getReplaceFrmUrl(strLineBGwNo) + "' OR LineB_GwNo = '" + GlobalFunc.getReplaceFrmUrl(strLineBGwNo) + "') ";

        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkLineDuplicateGWNo(String strPlcNo, String strLine, String strGWNo)
    {

        String sqlQuery = "SELECT COUNT(*) AS CountRow FROM GearUpLM2 WHERE PlcNo = '" + strPlcNo + "' "
            + (strLine == "A" ? "AND LineA_GwNo" : strLine == "B" ? "AND LineB_GwNo" : "") + " = '" + strGWNo + "'";

        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean SvGearUpMst(String strPlcNo, String strPartID, String strLineAGwNo, String strLineAModAddr, String strLineAPhysAddr, String strLineBGwNo, String strLineBModAddr, String strLineBPhysAddr, String strCurUser)
    {
        //SvGearUpMst(strPlcNo, strPartID, strLineAGwNo, strLineAModAddr, strLineAPhysAddr, strLineBGwNo, strLineBModAddr, strLineBPhysAddr, strCurUser);
        strPlcNo = GlobalFunc.ReplaceSingleQuotes(strPlcNo);
        strPartID = GlobalFunc.ReplaceSingleQuotes(strPartID);
        strLineAGwNo = GlobalFunc.ReplaceSingleQuotes(strLineAGwNo);
        strLineAModAddr = GlobalFunc.ReplaceSingleQuotes(strLineAModAddr);
        strLineAPhysAddr = GlobalFunc.ReplaceSingleQuotes(strLineAPhysAddr);
        strLineBGwNo = GlobalFunc.ReplaceSingleQuotes(strLineBGwNo);
        strLineBModAddr = GlobalFunc.ReplaceSingleQuotes(strLineBModAddr);
        strLineBPhysAddr = GlobalFunc.ReplaceSingleQuotes(strLineBPhysAddr);
        strCurUser = GlobalFunc.ReplaceSingleQuotes(strCurUser);

        String sqlQuery = "INSERT INTO GearUpLM2 (PlcNo, PartsID, LineA_GwNo, LineA_LmModuleAddress, LineA_LmPhysicalAddress, LineB_GwNo, LineB_LmModuleAddress,LineB_LmPhysicalAddress,last_upd_by,last_upd_dt) " +
            "VALUES ('" + strPlcNo + "', '" + strPartID
            + "', " + (strLineAGwNo == "" ? " null " : "'" + strLineAGwNo + "'")
            + ", " + (strLineAModAddr == "" ? " null " : "'" + strLineAModAddr + "'")
            + ", " + (strLineAPhysAddr == "" ? " null " : "'" + strLineAPhysAddr + "'")
            + ", " + (strLineBGwNo == "" ? " null " : "'" + strLineBGwNo + "'")
            + ", " + (strLineBModAddr == "" ? " null " : "'" + strLineBModAddr + "'")
            + ", " + (strLineBPhysAddr == "" ? " null " : "'" + strLineBPhysAddr + "'")
            + ", '" + strCurUser + "',CURRENT_TIMESTAMP)";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdGearUpMst(String strPlcNo, String strPartID, String strLineAGwNo, String strLineAModAddr, String strLineAPhysAddr, String strLineBGwNo, String strLineBModAddr, String strLineBPhysAddr, String strCurUser, String tempPartID, String tempLineAGwNo, String tempLineBGwNo)
    {
        //strPlcNo, strPartID, strLineAGwNo, strLineAModAddr, strLineAPhysAddr, strLineBGwNo, strLineBModAddr, strLineBPhysAddr, strCurUser);
        strPlcNo = GlobalFunc.ReplaceSingleQuotes(strPlcNo);
        strPartID = GlobalFunc.ReplaceSingleQuotes(strPartID);
        strLineAGwNo = GlobalFunc.ReplaceSingleQuotes(strLineAGwNo);
        strLineAModAddr = GlobalFunc.ReplaceSingleQuotes(strLineAModAddr);
        strLineAPhysAddr = GlobalFunc.ReplaceSingleQuotes(strLineAPhysAddr);
        strLineBGwNo = GlobalFunc.ReplaceSingleQuotes(strLineBGwNo);
        strLineBModAddr = GlobalFunc.ReplaceSingleQuotes(strLineBModAddr);
        strLineBPhysAddr = GlobalFunc.ReplaceSingleQuotes(strLineBPhysAddr);
        strCurUser = GlobalFunc.ReplaceSingleQuotes(strCurUser);
        tempPartID = GlobalFunc.ReplaceSingleQuotes(tempPartID);

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("UPDATE GearUpLM2 SET PartsID = '" + strPartID + "', "
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strLineAGwNo) == null ? " LineA_GwNo = null, " : " LineA_GwNo = '" + strLineAGwNo + "', ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strLineAModAddr) == null ? " LineA_LmModuleAddress = null, " : " LineA_LmModuleAddress = '" + strLineAModAddr + "', ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strLineAPhysAddr) == null ? " LineA_LmPhysicalAddress = null, " : " LineA_LmPhysicalAddress = '" + strLineAPhysAddr + "', ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strLineBGwNo) == null ? " LineB_GwNo = null, " : " LineB_GwNo = '" + strLineBGwNo + "', ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strLineBModAddr) == null ? " LineB_LmModuleAddress = null, " : " LineB_LmModuleAddress = '" + strLineBModAddr + "', ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strLineBPhysAddr) == null ? " LineB_LmPhysicalAddress = null, " : " LineB_LmPhysicalAddress = '" + strLineBPhysAddr + "', ")
            + " last_upd_by = '" + strCurUser
            + "', last_upd_dt = CURRENT_TIMESTAMP"
            + " WHERE PlcNo = '" + strPlcNo + "' "
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(tempLineAGwNo) == null ? "" : " AND LineA_GwNo = '" + tempLineAGwNo + "' ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(tempLineBGwNo) == null ? "" : " AND LineB_GwNo = '" + tempLineBGwNo + "' ")
            + " AND PartsID = '" + tempPartID + "'");

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean DelGearUp(String strPartID, String strPlcNo, String strLineAGwNo, String strLineBGwNo)
    {

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("DELETE FROM GearUpLM2 WHERE PartsID = '" + strPartID + "' AND PlcNo = '" + strPlcNo + "' "
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strLineAGwNo) == null ? " " : " AND LineA_GwNo = '" + strLineAGwNo + "'")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strLineBGwNo) == null ? " " : " AND LineB_GwNo = '" + strLineBGwNo + "'"));

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    #endregion

    #region GearUpLM

    public static DataSet SrcGearUpLM(String strPlcNo, String strPartID, String strProcName, String strLine, String strGwNo, String strModAddr, String strPhysAddr, String strGearID)
    {
        //SrcGearUpLM(strPlcNo, strPartID, strProcName, strLine, strGwNo, strModAddr, strPhysAddr, strGearID);
        String filterCriteria = "";

        if (strPlcNo != "")
        {
            filterCriteria = filterCriteria + " AND plc_no = '" + strPlcNo + "'";
        }
        if (strProcName != "")
        {
            filterCriteria = filterCriteria + " AND proc_name = '" + strProcName + "'";
        }
        if (strPartID != "")
        {
            filterCriteria = filterCriteria + " AND PartsID = '" + strPartID + "'";
        }
        if (strLine != "")
        {
            filterCriteria = filterCriteria + " AND Line = '" + strLine + "'";
        }
        if (strGwNo != "")
        {
            filterCriteria = filterCriteria + " AND GwNo = '" + strGwNo + "'";
        }
        if (strModAddr != "")
        {
            filterCriteria = filterCriteria + " AND LmModuleAddress = '" + strModAddr + "'";
        }
        if (strPhysAddr != "")
        {
            filterCriteria = filterCriteria + " AND LmPhysicalAddress = '" + strPhysAddr + "'";
        }
        if (strGearID != "")
        {
            filterCriteria = filterCriteria + " AND Gear_id = '" + strGearID + "'";
        }

        String sqlQuery = "SELECT Gear_id, PartsID ,plc_no ,proc_name ,Line, GwNo ,LmModuleAddress ,LmPhysicalAddress ,gu.last_upd_by ,gu.last_upd_dt FROM GearUpLm gu JOIN dt_DpsPlcMst plc ON gu.PlcNo = plc.plc_no WHERE PartsID != ''  " + filterCriteria + " ORDER BY plc_no,Gear_id, PartsID ";

        try
        {
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
    }

    public static Boolean ChkAllDuplicateGWNoGearUp(String strPlcNo, String strGwNo, String strLine, String strLmPhyAddr)
    {

        String sqlQuery = "SELECT COUNT(*) AS CountRow FROM GearUpLm WHERE PlcNo = '" + strPlcNo + "' AND GwNo = '" + strGwNo + "' AND LmPhysicalAddress = " + strLmPhyAddr + "";

        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean ChkDuplicateGearUpID(String GearUpID)
    {

        String sqlQuery = "SELECT COUNT(*) AS CountRow FROM GearUpLm WHERE Gear_id = '" + GearUpID + "' ";

        try
        {
            return ConnQuery.chkExistData(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean SvGearUpLM(String strPlcNo, String strPartID, String strLine, String strGwNo, String strModAddr, String strPhysAddr, String strGearID, String strCurUser)
    {
        strPlcNo = GlobalFunc.ReplaceSingleQuotes(strPlcNo);
        strPartID = GlobalFunc.ReplaceSingleQuotes(strPartID);
        strLine = GlobalFunc.ReplaceSingleQuotes(strLine);
        strGwNo = GlobalFunc.ReplaceSingleQuotes(strGwNo);
        strModAddr = GlobalFunc.ReplaceSingleQuotes(strModAddr);
        strPhysAddr = GlobalFunc.ReplaceSingleQuotes(strPhysAddr);
        strGearID = GlobalFunc.ReplaceSingleQuotes(strGearID);
        strCurUser = GlobalFunc.ReplaceSingleQuotes(strCurUser);

        String sqlQuery = "INSERT INTO GearUpLm (PlcNo, PartsID, Gear_id, Line, GwNo, LmModuleAddress, LmPhysicalAddress, last_upd_by, last_upd_dt) " +
            "VALUES ('" + strPlcNo + "', '" + strPartID
            + "', " + (strGearID == "" ? " null " : "'" + strGearID + "'")
            + ", " + (strLine == "" ? " null " : "'" + strLine + "'")
            + ", " + (strGwNo == "" ? " null " : "'" + strGwNo + "'")
            + ", " + (strModAddr == "" ? " null " : "'" + strModAddr + "'")
            + ", " + (strPhysAddr == "" ? " null " : "'" + strPhysAddr + "'")
            + ", '" + strCurUser + "',CURRENT_TIMESTAMP)";
        try
        {
            return ConnQuery.ExecuteQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean UpdGearUpLM(String strPlcNo, String strPartID, String strLine, String strGwNo, String strModAddr, String strPhysAddr, String strGearID, String strCurUser, String tempPartID, String tempGearID, String tempLine, String tempGwNo)
    {
        strPlcNo = GlobalFunc.ReplaceSingleQuotes(strPlcNo);
        strPartID = GlobalFunc.ReplaceSingleQuotes(strPartID);
        strLine = GlobalFunc.ReplaceSingleQuotes(strLine);
        strGwNo = GlobalFunc.ReplaceSingleQuotes(strGwNo);
        strModAddr = GlobalFunc.ReplaceSingleQuotes(strModAddr);
        strPhysAddr = GlobalFunc.ReplaceSingleQuotes(strPhysAddr);
        strGearID = GlobalFunc.ReplaceSingleQuotes(strGearID);
        strCurUser = GlobalFunc.ReplaceSingleQuotes(strCurUser);
        tempPartID = GlobalFunc.ReplaceSingleQuotes(tempPartID);
        tempGearID = GlobalFunc.ReplaceSingleQuotes(tempGearID);
        tempLine = GlobalFunc.ReplaceSingleQuotes(tempLine);
        tempGwNo = GlobalFunc.ReplaceSingleQuotes(tempGwNo);

        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("UPDATE GearUpLm SET PartsID = '" + strPartID + "', "
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strGearID) == null ? " Gear_id = null, " : " Gear_id = '" + strGearID + "', ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strLine) == null ? " Line = null, " : " Line = '" + strLine + "', ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strModAddr) == null ? " LmModuleAddress = null, " : " LmModuleAddress = '" + strModAddr + "', ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strPhysAddr) == null ? " LmPhysicalAddress = null, " : " LmPhysicalAddress = '" + strPhysAddr + "', ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(strGwNo) == null ? " GwNo = null, " : " GwNo = '" + strGwNo + "', ")
            + " last_upd_by = '" + strCurUser
            + "', last_upd_dt = CURRENT_TIMESTAMP"
            + " WHERE PlcNo = '" + strPlcNo + "' "
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(tempLine) == null ? "" : " AND Line = '" + tempLine + "' ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(tempGwNo) == null ? "" : " AND GwNo = '" + tempGwNo + "' ")
            + " " + (GlobalFunc.ReplaceEmptyStringToNull(tempGearID) == null ? "" : " AND Gear_id = '" + tempGearID + "' "));

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    public static Boolean DelGearUpLM(String strPartID, String strPlcNo, String strGwNo, String strGearUpID, String strLine)
    {
        //DelGearUpLM(tmpPartID, tempPlcNo, tempGwNo, tempGearUpID, tmpLine);
        List<String> sqlQuery = new List<String>();

        sqlQuery.Add("DELETE FROM GearUpLm WHERE PartsID = '" + strPartID
            + "' AND PlcNo = '" + strPlcNo
            + "' AND Line = '" + strLine
            + "' AND Gear_id = '" + strGearUpID
            + "' AND GwNo = '" + strGwNo
            + "' ");

        try
        {
            return ConnQuery.ExecuteTransQuery(sqlQuery);
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }

    #endregion
}