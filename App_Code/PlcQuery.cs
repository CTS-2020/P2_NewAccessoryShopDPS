using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Microsoft.CSharp;
using ActUtlTypeLib;
using ActSupportMsgLib;

public class PlcConnQuery
{
    public PlcConnQuery()
	{
    }

    #region Write Device Random (Per Device Name)
    public static String WriteDeviceByRandom(String[] strDeviceName, int[] strDeviceValue, int intLogicalStationNumber)
    {
        object oReturnCode;
        String szDeviceName = "";		//List data for 'DeviceName'
        int iNumberOfData = 0;			//Data for 'DeviceSize'
        short[] arrDeviceValue;		    //Data for 'DeviceValue'
        int iNumber;					//Loop counter
        String strErrorMsg = "";

        ActUtlTypeClass comActUtlTypeClass = new ActUtlTypeClass();
        ActUtlType comActUtlType = comActUtlTypeClass;
        comActUtlType.ActLogicalStationNumber = intLogicalStationNumber;
        comActUtlType.ActPassword = "";

        oReturnCode = comActUtlType.Open();

        if (Convert.ToInt32(oReturnCode) == 0)
        {
            szDeviceName = String.Join("\n", strDeviceName);
            iNumberOfData = strDeviceName.Length;
            arrDeviceValue = new short[iNumberOfData];

            for (iNumber = 0; iNumber < 3; iNumber++)
            {
                arrDeviceValue[iNumber] = Convert.ToInt16(strDeviceValue[iNumber]);
            }

            try
            {
                oReturnCode = comActUtlType.WriteDeviceRandom2(szDeviceName, iNumberOfData, ref arrDeviceValue[0]);
                if (Convert.ToInt32(oReturnCode) == 0)
                {
                    GlobalFunc.ShowMessage("Update into PLC were successfull.");
                }
                else
                {
                    strErrorMsg = "Update into PLC were not successfull.";
                    GlobalFunc.ShowErrorMessage("Update into PLC were not successfull.");
                }
            }
            catch (Exception exception)
            {
                oReturnCode = comActUtlType.Close();
                GlobalFunc.ShowErrorMessage(Convert.ToString(exception));
                strErrorMsg = "Update into PLC were not successfull.";
            }
        }
        else
        {
            strErrorMsg = "Update into PLC were not successfull.";
            GlobalFunc.ShowErrorMessage("Cannot open connection with PLC. Please check.");
        }
        oReturnCode = comActUtlType.Close();
        return strErrorMsg;
    }
    #endregion

    #region Write Device Block (Per Device Count)
    public static String WriteDeviceByBlock(String strDeviceName, int iTotArrayCount, List<short> strDeviceValue, int intLogicalStationNumber)
    {
        object oReturnCode;
        short[] arrDeviceValue;
        String strErrorMsg = "";

        //ActUtlTypeClass comActUtlTypeClass = new ActUtlTypeClass();
        //ActUtlType comActUtlType = comActUtlTypeClass;
        //comActUtlType.ActLogicalStationNumber = intLogicalStationNumber;
        //comActUtlType.ActPassword = "";

        //oReturnCode = comActUtlType.Open();

        //if (Convert.ToInt32(oReturnCode) == 0)
        //{
        //    arrDeviceValue = strDeviceValue.ToArray();

        //    try
        //    {
        //        oReturnCode = comActUtlType.WriteDeviceBlock2(strDeviceName, iTotArrayCount, ref arrDeviceValue[0]);
        //        if (Convert.ToInt32(oReturnCode) == 0)
        //        {
        //            GlobalFunc.ShowMessage("Update into PLC were successfull.");
        //        }
        //        else
        //        {
        //            strErrorMsg = "Update into PLC were not successfull.";
        //            GlobalFunc.ShowErrorMessage("Update into PLC were not successfull.");
        //        }
        //    }
        //    catch (Exception exExcepion)
        //    {
        //        oReturnCode = comActUtlType.Close();
        //        GlobalFunc.ShowErrorMessage(Convert.ToString(exExcepion));
        //        strErrorMsg = "Update into PLC were not successfull.";
        //    }
        //}
        //else
        //{
        //    strErrorMsg = "Update into PLC were not successfull.";
        //    GlobalFunc.ShowErrorMessage("Cannot open connection with PLC. Please check.");
        //}
        //oReturnCode = comActUtlType.Close();
        return strErrorMsg;
    }
    #endregion

    #region Write Device Block (Per Device Count)
    public static String WriteDeviceRefreshBlock(String strRefreshDevName, short iVal, int intLogicalStationNumber)
    {
        object oReturnCode;
        String strErrorMsg = "";

        //ActUtlTypeClass comActUtlTypeClass = new ActUtlTypeClass();
        //ActUtlType comActUtlType = comActUtlTypeClass;
        //comActUtlType.ActLogicalStationNumber = intLogicalStationNumber;
        //comActUtlType.ActPassword = "";

        //oReturnCode = comActUtlType.Open();

        //if (Convert.ToInt32(oReturnCode) == 0)
        //{
        //    try
        //    {
        //        oReturnCode = comActUtlType.SetDevice2(strRefreshDevName, iVal);
        //        if (Convert.ToInt32(oReturnCode) == 0)
        //        {
        //            //GlobalFunc.ShowMessage("Update into PLC were successfull.");
        //        }
        //        else
        //        {
        //            strErrorMsg = "Update into PLC were not successfull.";
        //            GlobalFunc.ShowErrorMessage("Update into PLC were not successfull.");
        //        }
        //    }
        //    catch (Exception exExcepion)
        //    {
        //        oReturnCode = comActUtlType.Close();
        //        strErrorMsg = "Update into PLC were not successfull " + exExcepion;
        //        GlobalFunc.ShowErrorMessage(Convert.ToString(exExcepion));
        //    }
        //}
        //else
        //{
        //    strErrorMsg = "Update into PLC were not successfull.";
        //    GlobalFunc.ShowErrorMessage("Cannot open connection with PLC. Please check.");
        //}
        //oReturnCode = comActUtlType.Close();
        return strErrorMsg;
    }
    #endregion
}