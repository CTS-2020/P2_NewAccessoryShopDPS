using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Text;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using Lawson.M3.MvxSock;
using Itenso.Rtf;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;
using Itenso.Rtf.Converter.Html;

public class GlobalFunc
{
    public GlobalFunc()
    {
    }

    ~GlobalFunc()
    {
    }

    #region Message
    public static void ShowMessage(string msg)
    {

        Page page = HttpContext.Current.Handler as Page;
        if (page != null)
        {
            msg = msg.Replace("'", "\'");
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "Message", "alert('" + msg + "');", true);
        }
    }

    public static void ShowErrorMessage(string error)
    {

        Page page = HttpContext.Current.Handler as Page;
        if (page != null)
        {
            error = error.Replace("'", "\'");
            error = error.Replace("\r", "");
            error = error.Replace("\n", "");
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "Error Message", "alert('" + error + "');", true);
        }
    }
    #endregion

    #region Conversion
    #region Date Conversion
    public static String getConversionDatabaseDate(String strDate)
    {
        if (strDate != "")
        {
            strDate = strDate.Substring(6, 4) + "-" + strDate.Substring(3, 2) + "-" + strDate.Substring(0, 2);
        }
        return strDate;
    }
    #endregion

    #region HTML Conversion
    public static String getReplaceFrmUrl(String strReplace)
    {
        strReplace = strReplace.Replace("+", " ");
        strReplace = strReplace.Replace("&nbsp;", " ");

        return strReplace;
    }

    public static String getReplaceToUrl(String strReplace)
    {
        strReplace = strReplace.Replace(" ", "+");

        return strReplace;
    }
    #endregion

    #region ReplaceSingleQuotes
    public static string ReplaceSingleQuotes(string input)
    {
        return input.Replace("'", "''");
    }
    #endregion

    #region ReplaceEmptyStringToNull
    public static string ReplaceEmptyStringToNull(string input)
    {
        input = input.Replace("&nbsp;", "");
        return string.IsNullOrWhiteSpace(input) ? null : input;
    }
    #endregion

    #region ReplaceToEmptyString
    public static string ReplaceToEmptyString(string input)
    {
        input = input.Replace("&nbsp;", "");
        return input;
    }
    #endregion

    #region ASCII Conversion
    public static short[] convStrToAscii(String arrDeviceValue, int iDeviceSize)
    {
        Encoding objAsciiCodePageEncoding = Encoding.Default;                       //Create an instance for encoding to(or decoding from) ASCII Code Page.
        byte[] byarrBufferByte;                                                     //Array for using BitConverter/Encoding class
        short[] sharrBufferForDeviceValue = new short[iDeviceSize];                 //Array for writing data to the PLC - 1
        int iNumber;                                                                //Loop counter

        byarrBufferByte = objAsciiCodePageEncoding.GetBytes(arrDeviceValue);        //Convert the TextBox data to ASCII Code Page.

        int iMaxDeviceSize = iDeviceSize * 2;                                       //Set the size of encoded 'Word' data. (Maximum Device Size is 2 byte per Device Name) 
        int iLengthOfBuffer = Math.Min(byarrBufferByte.Length, iMaxDeviceSize);     //Size of encoded 'Word' data

        //Convert the 'byarrBufferByte' to the array for writing to the PLC.
        //  Step 2                :To copy 2 bytes data to a element of ShortType array.
        //  iLengthOfBuffer - 2   :Not to refer to out of 'byarrBufferByte', when the 'iLengthOfBuffer' is odd.
        for (iNumber = 0; iNumber <= iLengthOfBuffer - 2; iNumber += 2)
        {
            sharrBufferForDeviceValue[iNumber / 2] = BitConverter.ToInt16(byarrBufferByte, iNumber);
        }

        //Process the remained character, when the 'iLengthOfBuffer' is odd.
        if (iLengthOfBuffer % 2 == 1)
        {
            sharrBufferForDeviceValue[Microsoft.VisualBasic.Conversion.Fix(iLengthOfBuffer / 2)] = byarrBufferByte[iLengthOfBuffer - 1];
        }

        return sharrBufferForDeviceValue;
    }
    #endregion

    #region Short Conversion
    public static short[] convInt32ToInt16(Int32 arrDeviceValue, int iDeviceSize)
    {
        byte[] byarrBufferByte; //Array for using BitConverter class
        short[] sharrBufferForDeviceValue = new short[iDeviceSize]; //'Array for writing to the PLC

        byarrBufferByte = BitConverter.GetBytes(Convert.ToInt32(arrDeviceValue));

        sharrBufferForDeviceValue[0] = BitConverter.ToInt16(byarrBufferByte, 0);
        sharrBufferForDeviceValue[1] = BitConverter.ToInt16(byarrBufferByte, 2);

        return sharrBufferForDeviceValue;
    }
    #endregion
    #endregion

    #region Condition Checking
    #region Valid IP Address
    public static bool IsTextAValidIPAddress(string text)
    {
        bool result = true;
        Byte temp;
        string[] values = text.Split(new[] { "." }, StringSplitOptions.None); //keep empty strings when splitting
        result &= values.Length == 4; // aka string has to be like "xx.xx.xx.xx"
        if (result)
        {
            for (int i = 0; i < 4; i++)
            {
                result &= byte.TryParse(values[i], out temp); //each "xx" must be a byte (0-255)
            }
        }
        return result;
    }
    #endregion

    #region Valid Integer
    public static Boolean IsTextAValidInteger(string value)
    {
        int number;
        bool result = Int32.TryParse(value, out number);
        return result;
    }
    #endregion

    #region Valid AlphaNumeric
    public static Boolean IsAlphaNum(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }

        for (int i = 0; i < str.Length; i++)
        {
            if (!(char.IsLetter(str[i])) && (!(char.IsNumber(str[i]))))
            {
                return false;
            }
        }

        return true;
    }
    #endregion
    #endregion

    #region Dropdown List
    public static DataSet getRolelist()
    {
        String sqlQuery = "SELECT dd_descs as Description, dd_code as Code FROM dps_DropDownList WHERE dd_type = 'Roles'";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getAisType()
    {
        String sqlQuery = "SELECT dd_descs as Description, dd_code as Code FROM dps_DropDownList WHERE dd_type = 'AIS Types'";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getAisName(String strAisType)
    {
        String sqlQuery = "";

        if (strAisType == "Jundate")
        {
            sqlQuery = "SELECT DISTINCT (RTRIM(JdtName) + ' ~ ' + cast(Sfx AS varchar(2)) + ' ~ ' + cast(ColorCode AS varchar(4))) as Description, len((RTRIM(JdtName) + ' ~ ' + cast(Sfx AS varchar(2)) + ' ~ ' + cast(ColorCode AS varchar(4)))), JdtItmID AS Name FROM vw_Jundate ORDER BY len((RTRIM(JdtName) + ' ~ ' + cast(Sfx AS varchar(2)) + ' ~ ' + cast(ColorCode AS varchar(4)))), (RTRIM(JdtName)+ ' ~ ' + cast(Sfx AS varchar(2)) + ' ~ ' + cast(ColorCode AS varchar(4))), JdtItmID";
        }
        else if (strAisType == "Harigami")
        {
            sqlQuery = "SELECT DISTINCT (RTRIM(HrgmName) + ' ~ ' + cast(Sfx AS varchar(2)) + ' ~ ' + cast(ColorCode AS varchar(4))) as Description, len((RTRIM(HrgmName) + ' ~ ' + cast(Sfx AS varchar(2)) + ' ~ ' + cast(ColorCode AS varchar(4)))), HrgmItmID AS Name FROM vw_Harigami WHERE HrgmName like '%DPS%' ORDER BY len((RTRIM(HrgmName) + ' ~ ' + cast(Sfx AS varchar(2)) + ' ~ ' + cast(ColorCode AS varchar(4)))), (RTRIM(HrgmName) + ' ~ ' + cast(Sfx AS varchar(2)) + ' ~ ' + cast(ColorCode AS varchar(4))), HrgmItmID";
        }
        return ConnQuery.getPisBindingDatasetData(sqlQuery);
    }

    public static DataSet getAisNameDps(String strAisType)
    {
        String sqlQuery = "";

        if (strAisType == "Jundate")
        {
            //sqlQuery = "SELECT DISTINCT (RTRIM(name) + ' - ' + cast(sfx AS varchar(2)) + ' - ' + cast(color AS varchar(4))) as Description, len((RTRIM(name) + ' - ' + cast(sfx AS varchar(2)) + ' - ' + cast(color AS varchar(4)))), item_id AS Name FROM ais_DataHj WHERE row IS NULL ORDER BY len((RTRIM(name) + ' - ' + cast(sfx AS varchar(2)) + ' - ' + cast(color AS varchar(4)))), (RTRIM(name) + ' - ' + cast(sfx AS varchar(2)) + ' - ' + cast(color AS varchar(4))), item_id";
            sqlQuery = "SELECT DISTINCT (RTRIM(name) + ' ~ ' + cast(sfx AS varchar(2)) + ' ~ ' + cast(color AS varchar(4))) as Description, len((RTRIM(name) + ' ~ ' + cast(sfx AS varchar(2)) + ' ~ ' + cast(color AS varchar(4)))) FROM ais_DataHj WHERE row IS NULL ORDER BY len((RTRIM(name) + ' ~ ' + cast(sfx AS varchar(2)) + ' ~ ' + cast(color AS varchar(4)))), (RTRIM(name) + ' ~ ' + cast(sfx AS varchar(2)) + ' ~ ' + cast(color AS varchar(4)))";
        }
        else if (strAisType == "Harigami")
        {   //Mpdify by Khor, filter name like '%DPS%' AND
            //sqlQuery = "SELECT DISTINCT (RTRIM(name) + ' - ' + cast(sfx AS varchar(2)) + ' - ' + cast(color AS varchar(4))) as Description, len((RTRIM(name) + ' - ' + cast(sfx AS varchar(2)) + ' - ' + cast(color AS varchar(4)))), item_id AS Name FROM ais_DataHj WHERE row IS NOT NULL ORDER BY len((RTRIM(name) + ' - ' + cast(sfx AS varchar(2)) + ' - ' + cast(color AS varchar(4)))), (RTRIM(name) + ' - ' + cast(sfx AS varchar(2)) + ' - ' + cast(color AS varchar(4))), item_id";
            sqlQuery = "SELECT DISTINCT (RTRIM(name) + ' ~ ' + cast(sfx AS varchar(2)) + ' ~ ' + cast(color AS varchar(4))) as Description, len((RTRIM(name) + ' ~ ' + cast(sfx AS varchar(2)) + ' ~ ' + cast(color AS varchar(4)))) FROM ais_DataHj WHERE name like '%DPS%' AND row IS NOT NULL ORDER BY len((RTRIM(name) + ' ~ ' + cast(sfx AS varchar(2)) + ' ~ ' + cast(color AS varchar(4)))), (RTRIM(name) + ' ~ ' + cast(sfx AS varchar(2)) + ' ~ ' + cast(color AS varchar(4)))";
        }
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getAisRevNo(String strAisName)
    {
        if (strAisName != "")
        {
            String[] tmpAisCond = strAisName.Split('~');
            int tmpAisCondCnt = tmpAisCond.Length;
            if (tmpAisCondCnt != 3)
            {
                return null;
            }

            //String sqlQuery = "SELECT DISTINCT rev_no as Description, len(rev_no) FROM ais_DataHJ WHERE item_id = '" + strAisName + "' ORDER BY len(rev_no), rev_no";
            String sqlQuery = "SELECT DISTINCT rev_no as Description, len(rev_no) FROM ais_DataHJ WHERE name = '" + tmpAisCond[0].Trim() + "' AND sfx = '" + tmpAisCond[1].Trim() + "' AND color = '" + tmpAisCond[2].Trim() + "' ORDER BY len(rev_no), rev_no";
            return ConnQuery.getBindingDatasetData(sqlQuery);
        }
        else
        {
            return null;
        }
    }

    public static DataSet getModel()
    {
        String sqlQuery = "SELECT DISTINCT model as Description, len(model) FROM ais_DataHJ ORDER BY len(model), model";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getKatashiki(String strModel)
    {
        String sqlQuery = "SELECT DISTINCT katashiki as Description, len(katashiki) FROM ais_DataHJ WHERE model = '" + strModel + "' ORDER BY len(katashiki), katashiki";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getSfx(String strModel, String strKatashiki)
    {
        String sqlQuery = "SELECT DISTINCT sfx as Description, len(sfx) FROM ais_DataHJ WHERE model = '" + strModel + "' AND katashiki = '" + strKatashiki + "' ORDER BY len(sfx), sfx";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getColor(String strModel, String strKatashiki, String strSfx)
    {
        String sqlQuery = "SELECT DISTINCT color as Description, len(color) FROM ais_DataHJ WHERE color != '*' AND model = '" + strModel + "' AND katashiki = '" + strKatashiki + "' AND sfx = '" + strSfx + "' ORDER BY len(color), color";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getProcName()
    {
        String sqlQuery = "SELECT DISTINCT proc_name as Description, plc_no as Code FROM dt_DpsPlcMst ORDER BY plc_no";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getProcNamebyPlcModel(String strPlcModel)             //Added by YanTeng 17/09/2020
    {
        String sqlQuery = "SELECT proc_name as Description, plc_no as Code FROM dt_DpsPlcMst WHERE plc_model = '" + strPlcModel + "' ORDER BY plc_no";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getAllPlcModel()                                      //Added by YanTeng 14/09/2020, modified to dt_PlcModelMst 29/09/2020 
    {
        String sqlQuery = "SELECT DISTINCT uid as Value, plcmodel_no as Code FROM dt_PlcModelMst WHERE plcmodel_no IS NOT NULL";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getPlcModelbyProcName(String strProcName)             //Added by YanTeng 15/09/2020 
    {
        String sqlQuery = "SELECT plc_model as Description, plc_model as Value FROM dt_DpsPlcMst WHERE proc_name = '" + strProcName + "'";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getGroupName(String strProcName)
    {
        String sqlQuery = "SELECT DISTINCT group_name as Description, plc_no as Code, len(group_name) FROM dt_GroupMst WHERE proc_name = '" + strProcName + "' ORDER BY len(group_name), group_name";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getBlockName(String strProcName, String strGroupName)
    {
        String sqlQuery = "SELECT DISTINCT block_name as Description, block_seq as Code FROM dt_BlockMst WHERE proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "'";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getBlockforModuleName(String strPlcNo, String strProcName)               //Added by YanTeng 06/12/2020
    {
        String sqlQuery = "SELECT DISTINCT block_name as Description FROM dt_BlockMst WHERE plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' Order By Description";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getRackName(String strProcName, String strGroupName, String strBlockName)
    {
        String sqlQuery = "SELECT DISTINCT rack_name as Description FROM dt_RackMst WHERE proc_name = '" + strProcName + "' AND group_name = '" + strGroupName + "' AND block_name = '" + strBlockName + "'";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getModuleType(String strEquipType)
    {
        String sqlQuery = "SELECT DISTINCT module_type + ' - ' + cast(equip_type AS varchar(20)) as Description, module_type as Value, len(module_type + ' - ' + cast(equip_type AS varchar(20))) FROM dt_LampModuleTypeMst";

        if (strEquipType != "")
        {
            sqlQuery = sqlQuery + " WHERE equip_type = '" + strEquipType + "'";
        }

        sqlQuery = sqlQuery + " ORDER BY len(module_type + ' - ' + cast(equip_type AS varchar(20))), module_type + ' - ' + cast(equip_type AS varchar(20))";

        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getLmAdd(String strEquipType, String strProcName, String strPlcNo)
    {
        String sqlQuery = "SELECT DISTINCT LmAdd.module_add as Description, len(LmAdd.module_add) FROM dt_LampModuleTypeMst LmType, dt_LampModuleAddMst LmAdd WHERE LmAdd.module_type = LmType.module_type AND LmType.equip_type = '" + strEquipType + "' AND plc_no = '" + strPlcNo + "' AND proc_name = '" + strProcName + "' ORDER BY len(LmAdd.module_add), LmAdd.module_add";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getEquipType()
    {
        String sqlQuery = "SELECT dd_descs as Description, dd_code as Code FROM dps_DropDownList WHERE dd_type = 'Equip Types'";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getLampLighting()
    {
        String sqlQuery = "SELECT dd_descs as Description, dd_code as Code FROM dps_DropDownList WHERE dd_type = 'Lighting Patterns'";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getLampColor()
    {
        String sqlQuery = "SELECT dd_descs as Description, dd_code as Code FROM dps_DropDownList WHERE dd_type = 'Lighting Color'";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getInsCode()
    {
        String sqlQuery = "SELECT DISTINCT ins_code as Description FROM dt_DpsInsCodeMst";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getPlcName()
    {
        String sqlQuery = "SELECT proc_name as Description, plc_no FROM dt_DpsPlcMst ORDER BY plc_no";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getWritePointer()
    {
        String sqlQuery = "SELECT pointer as Description, plc_no FROM dt_PlcPointerMst WHERE flag_type = 'W' ORDER BY plc_no";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static DataSet getReadPointer()
    {
        String sqlQuery = "SELECT pointer as Description, plc_no FROM dt_PlcPointerMst WHERE flag_type = 'R' ORDER BY plc_no";
        return ConnQuery.getBindingDatasetData(sqlQuery);
    }

    public static String getColorCode(String strColor)
    {
        if (strColor == "")
        {
            return "";
        }

        String sqlQuery = "SELECT sys_color as ReturnField FROM dps_ColorChart WHERE lighting = 'Lighting' AND color = '" + strColor + "' ORDER BY color";
        return ConnQuery.getReturnFieldExecuteReader(sqlQuery);
    }
    #endregion

    #region Log
    public static void Log(string sEvent)
    {
        try
        {
            string path = @"C:\Logs\Webpage_Log\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = path + "DPS_Web_" + DateTime.Today.ToString("ddMMyyyy") + ".log";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] " + sEvent + "\r");
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] " + sEvent + "\r");
                    sw.Flush();
                    sw.Close();
                }
            }
        }
        catch { }
        {
        }
    }

    public static void Log(Exception Ex)
    {
        try
        {
            string path = @"C:\Logs\Webpage_Log\Web_Exception\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = path + "DPS_WebException_" + DateTime.Today.ToString("ddMMyyyy") + ".log";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] " + Ex + "\r");
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] " + Ex + "\r");
                    sw.Flush();
                    sw.Close();
                }
            }
        }
        catch { }
        {
        }
    }
    #endregion
}


