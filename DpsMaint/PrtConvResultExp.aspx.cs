using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Threading;
using System.Net;
using dpant;
using Itenso.Rtf;
using Itenso.Rtf.Converter.Html;
using Itenso.Rtf.Converter.Image;
using Itenso.Rtf.Converter.Text;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;
using Excel = Microsoft.Office.Interop.Excel;
using Marshal = System.Runtime.InteropServices.Marshal;
using OfficeOpenXml;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml.Style;


public partial class PlcConvResult : System.Web.UI.Page
{
    private Excel.Application xlApp = null;
    private Excel.Workbook xlWorkBook = null;
    private Excel.Worksheet xlWorkSheet = null;
    private Excel.Range xlWorkSheet_range = null;
    private object misValue = System.Reflection.Missing.Value;



    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String strPlcNo = "";
            String strProcName = "";
            String strGroupName = "";
            String strBlockName = "";
            String strRackName = "";
            String strInsCode = "";
            String strModel = "";
            String strKatashiki = "";
            String strSfx = "";
            String strColor = "";
            String strFlag = "";

            try
            {
                if (Convert.ToString(Request.QueryString["plc_no"]) != "") strPlcNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["plc_no"]));
                if (Convert.ToString(Request.QueryString["proc_name"]) != "") strProcName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["proc_name"]));
                if (Convert.ToString(Request.QueryString["group_name"]) != "") strGroupName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["group_name"]));
                if (Convert.ToString(Request.QueryString["block_name"]) != "") strBlockName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["block_name"]));
                if (Convert.ToString(Request.QueryString["rack_name"]) != "") strRackName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["rack_name"]));
                if (Convert.ToString(Request.QueryString["ins_code"]) != "") strInsCode = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["ins_code"]));
                if (Convert.ToString(Request.QueryString["model"]) != "") strModel = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["model"]));
                if (Convert.ToString(Request.QueryString["katashiki"]) != "") strKatashiki = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["katashiki"]));
                if (Convert.ToString(Request.QueryString["sfx"]) != "") strSfx = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["sfx"]));
                if (Convert.ToString(Request.QueryString["color"]) != "") strColor = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["color"]));
                if (Convert.ToString(Request.QueryString["flag"]) != "") strFlag = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["flag"]));

                BindToText(strPlcNo, strProcName, strGroupName, strBlockName, strRackName, strInsCode, strModel, strKatashiki, strSfx, strColor);

                String strRackMstRow = "";
                String strRackMstCol = "";

                getRackMst(out strRackMstRow, out strRackMstCol);
                genExcel(strRackMstRow, strRackMstCol);
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
        lblMsg.Visible = false;
    }
    #endregion

    #region Method

    #region BindToText
    private void BindToText(String strPlcNo, String strProcName, String strGroupName, String strBlockName, String strRackName, String strInsCode, String strModel, String strKatashiki, String strSfx, String strColor)
    {
        try
        {
            lblPlcNo.Text = strPlcNo;
            txtProcName.Text = strProcName;
            txtGroupName.Text = strGroupName;
            txtBlockName.Text = strBlockName;
            txtRackName.Text = strRackName;
            txtInsCode.Text = strInsCode;
            txtModel.Text = strModel;
            txtKatashiki.Text = strKatashiki;
            txtSfx.Text = strSfx;
            txtColor.Text = strColor;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Get Rack Master
    protected void getRackMst(out String strRackMstRow, out String strRackMstCol)
    {

        String strProcName = Convert.ToString(txtProcName.Text);
        String strPlcNo = Convert.ToString(lblPlcNo.Text);
        String strGroupName = Convert.ToString(txtGroupName.Text);
        String strBlockName = Convert.ToString(txtBlockName.Text);
        String strRackName = Convert.ToString(txtRackName.Text);

        strRackMstRow = "";
        strRackMstCol = "";

        DataSet dsRackMst = new DataSet();
        System.Data.DataTable dtRackMst = new System.Data.DataTable();

        dsRackMst = csDatabase.SrcRackMst(strRackName, strPlcNo, strProcName, strGroupName, strBlockName);
        dtRackMst = dsRackMst.Tables[0];

        if (dtRackMst.Rows.Count > 0)
        {
            if (Convert.ToString(dtRackMst.Rows[0]["row_cnt"]).Trim() != "")
            {
                strRackMstRow = Convert.ToString(dtRackMst.Rows[0]["row_cnt"]).Trim();
            }
            if (Convert.ToString(dtRackMst.Rows[0]["col_cnt"]).Trim() != "")
            {
                strRackMstCol = Convert.ToString(dtRackMst.Rows[0]["col_cnt"]).Trim();
            }
        }
        else
        {
            strRackMstRow = "";
            strRackMstCol = "";
        }
    }
    #endregion

    #region Generate Excel
    private void genExcel(String strRackMstRow, String strRackMstCol)
    {
        try
        {
            System.Globalization.CultureInfo newCulture = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo oldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;

            String strProcName = txtProcName.Text;
            String strGroupName = txtGroupName.Text;
            String strBlockName = txtBlockName.Text;
            String strRackName = txtRackName.Text;
            String strInsCode = txtInsCode.Text;
            String strModel = txtModel.Text;
            String strKatashiki = txtKatashiki.Text;
            String strSfx = txtSfx.Text;
            String strColor = txtColor.Text;

            string templatePath = Server.MapPath("~/template/template.xlsx");
            string strFileName = strProcName + "-" + strRackName + "-" + strInsCode + "-" + strSfx + "-" + strColor;
            string strFilePath = Server.MapPath("~/template/" + strFileName + ".xlsx");

            FileInfo templateFile = new FileInfo(templatePath);
            FileInfo newFile = new FileInfo(strFilePath);

            // Delete the file if it exists
            if (newFile.Exists)
            {
                newFile.Delete();
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (!templateFile.Exists)
            {
                throw new FileNotFoundException("Template file not found at: " + templatePath);
            }

            if (templateFile.Extension.ToLower() != ".xlsx")
            {
                throw new Exception("The template file is not a valid .xlsx file.");
            }

            using (var templatePackage = new ExcelPackage(templateFile))
            {
                if (templatePackage.Workbook.Worksheets.Count == 0)
                {
                    throw new Exception("The template file does not contain any worksheets.");
                }

                ExcelWorksheet templateWorksheet = templatePackage.Workbook.Worksheets[0];

                using (var package = new ExcelPackage())
                {
                    var newWorksheet = package.Workbook.Worksheets.Add("Sheet1", templateWorksheet);

                    newWorksheet.Cells[2, 2].Value = "Process : " + strProcName;
                    newWorksheet.Cells[2, 5].Value = "Group : " + strGroupName;
                    newWorksheet.Cells[2, 8].Value = "Block : " + strBlockName;
                    newWorksheet.Cells[2, 11].Value = "Rack : " + strRackName;
                    newWorksheet.Cells[4, 2].Value = "Instruction Code : " + strInsCode;
                    newWorksheet.Cells[4, 5].Value = "Model : " + strModel;
                    newWorksheet.Cells[4, 8].Value = "Sfx : " + strSfx;
                    newWorksheet.Cells[4, 11].Value = "Color : " + strColor;
                    newWorksheet.Cells[4, 14].Value = "Katashiki : " + strKatashiki;

                    int rowCnt = 0;
                    int colCnt = 0;
                    int rowCtr = 0;
                    int colCtr = 0;

                    int xVal = 0;
                    int yVal = 0;

                    rowCnt = int.Parse(strRackMstRow);
                    colCnt = int.Parse(strRackMstCol);

                    for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
                    {
                        if (rowCtr == 1)
                        {
                            xVal = rowCtr + 5;
                        }
                        else
                        {
                            xVal = xVal + 6;
                        }

                        for (colCtr = 1; colCtr <= colCnt; colCtr++)
                        {
                            String strAisDet = "";
                            String strModDet = "";
                            String strRackDetId = strRackName + "^" + rowCtr + "^" + colCtr;

                            String strPartName = "";
                            String strPartNumber = "";
                            String strSymbol = "";
                            String strSymbolFontColor = "";
                            String strSymbolBgColor = "";
                            String strModuleAdd = "";
                            String strRackLmColor = "";

                            if (colCtr == 1)
                            {
                                yVal = colCtr;
                            }
                            else
                            {
                                yVal = yVal + 3;
                            }

                            #region Get and Assign Ais Detail
                            strAisDet = getAisDetString(strRackDetId);

                            #region Sample Data Retrieve
                            // 0 strHjId
                            // 1 strHjItemId
                            // 2 strHjRow
                            // 3 strHjCol
                            // 4 strPartsTitle
                            // 5 strPartsNo
                            // 6 strColorSfx
                            // 7 strSymbolCode
                            // 8 strSymbolRtf;
                            #endregion
                            String[] tmpAisVal = strAisDet.Split('~');
                            int tmpAisValCnt = tmpAisVal.Length;
                            if (tmpAisValCnt != 1)
                            {
                                strSymbol = Itenso.Solutions.Community.Rtf2Html.RtfConverterDemo.ConvertRtf2Txt(tmpAisVal[8]);
                                strSymbol = strSymbol.Replace("\r", "");
                                strSymbol = strSymbol.Replace("\n", "");

                                strPartName = tmpAisVal[4];
                                strPartNumber = tmpAisVal[5] + " - " + tmpAisVal[6];

                                strSymbolFontColor = csDatabase.GetPartsFontColor(tmpAisVal[5], tmpAisVal[6]);
                                strSymbolBgColor = csDatabase.GetPartsBgColor(tmpAisVal[5], tmpAisVal[6]);
                            }
                            #endregion

                            #region Get and Assign Module Address Detail
                            strModDet = getModDetString(strRackDetId);

                            #region Sample Data Retrieve
                            // 0 strModuleAdd
                            // 1 strModuleName
                            // 2 strLightColorDuringInstruction
                            #endregion
                            String[] tmpModVal = strModDet.Split('~');
                            int tmpModValCnt = tmpModVal.Length;
                            if (tmpModValCnt != 1)
                            {
                                strModuleAdd = "Module Address : " + tmpModVal[0];
                            }

                            #region Rack Light Simulation
                            if (tmpAisValCnt != 1)
                            {
                                if (tmpModValCnt != 1)
                                {
                                    if (RackLightSimulation(tmpAisVal[5], tmpAisVal[6]) == true)
                                    {
                                        strRackLmColor = GlobalFunc.getColorCode(tmpModVal[2]);
                                    }
                                }
                            }
                            #endregion
                            #endregion

                            newWorksheet.Cells[xVal, yVal].Value = "[" + rowCtr + "-" + colCtr + "]  " + strPartName;
                            newWorksheet.Cells[xVal + 1, yVal].Value = strPartNumber;
                            newWorksheet.Cells[xVal + 2, yVal].Value = "";
                            newWorksheet.Cells[xVal + 4, yVal].Value = "";
                            newWorksheet.Cells[xVal + 5, yVal].Value = strModuleAdd;

                            String strRackLmCell_1 = GetCellAdd(newWorksheet, xVal, yVal);
                            String strRackLmCell_2 = GetCellAdd(newWorksheet, xVal + 5, yVal + 2);
                            // Get the range using the addresses
                            var xlWorkSheet_range = newWorksheet.Cells[strRackLmCell_1 + ":" + strRackLmCell_2];

                            // Apply the background color to the range
                            // Assuming GetCellColor returns a System.Drawing.Color object
                            xlWorkSheet_range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            xlWorkSheet_range.Style.Fill.BackgroundColor.SetColor(GetCellColor(strRackLmColor));

                            newWorksheet.Cells[xVal + 3, yVal + 1].Value = strSymbol;

                            String strSymbolCell_1 = GetCellAdd(newWorksheet, xVal + 3, yVal + 1);
                            //xlWorkSheet_range = null;
                            //xlWorkSheet_range = xlWorkSheet.get_Range(strSymbolCell_1, strSymbolCell_1);
                            //xlWorkSheet_range.Font.Size = "22";
                            //xlWorkSheet_range.Font.Color = GetCellColor(xlWorkSheet_range, strSymbolFontColor);
                            //xlWorkSheet_range.Interior.Color = GetCellColor(xlWorkSheet_range, strSymbolBgColor);
                            // Directly reference the cell using EPPlus
                            xlWorkSheet_range = null;
                            xlWorkSheet_range = newWorksheet.Cells[strSymbolCell_1];

                            // Set font size
                            xlWorkSheet_range.Style.Font.Size = 22;

                            // Set font color
                            xlWorkSheet_range.Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml(strSymbolFontColor));

                            // Set background color
                            xlWorkSheet_range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            xlWorkSheet_range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(strSymbolBgColor));

                            if (colCtr % 6 == 0)
                            {
                                yVal = yVal + 1;
                            }
                        }
                        if (rowCtr % 7 == 0)
                        {
                            xVal = xVal + 11;
                        }
                    }
                    package.SaveAs(newFile);
                }
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCulture;
            // Show the generated Excel file
            ShowExcel(strFilePath, strFileName);
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));

        }


    }
    #endregion

    #region Get AIS String
    protected String getAisDetString(String strRackDetId)
    {
        String strRackName = Convert.ToString(txtRackName.Text);
        String strAisRackDetAis = "";
        DataSet dsRackDetAis = new DataSet();
        System.Data.DataTable dtRackDetAis = new System.Data.DataTable();

        dsRackDetAis = csDatabase.GetRackMstDetAis(strRackDetId);
        dtRackDetAis = dsRackDetAis.Tables[0];

        #region Initialise Variable
        String strHjId = "";
        String strHjItemId = "";
        String strHjRow = "";
        String strHjCol = "";
        String strPartsTitle = "";
        String strPartsNo = "";
        String strColorSfx = "";
        String strSymbolCode = "";
        String strSymbolRtf = "";
        #endregion

        if (dtRackDetAis.Rows.Count > 0)
        {
            #region Assign Variable
            if (Convert.ToString(dtRackDetAis.Rows[0]["hj_id"]).Trim() != "")
            {
                strHjId = Convert.ToString(dtRackDetAis.Rows[0]["hj_id"]).Trim();
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["hj_item_id"]).Trim() != "")
            {
                strHjItemId = Convert.ToString(dtRackDetAis.Rows[0]["hj_item_id"]).Trim();
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["hj_row"]).Trim() != "")
            {
                strHjRow = Convert.ToString(dtRackDetAis.Rows[0]["hj_row"]).Trim();
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["hj_col"]).Trim() != "")
            {
                strHjCol = Convert.ToString(dtRackDetAis.Rows[0]["hj_col"]);
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["parts_title"]).Trim() != "")
            {
                strPartsTitle = Convert.ToString(dtRackDetAis.Rows[0]["parts_title"]);
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["parts_no"]).Trim() != "")
            {
                strPartsNo = Convert.ToString(dtRackDetAis.Rows[0]["parts_no"]).Trim();
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["color_sfx"]).Trim() != "")
            {
                strColorSfx = Convert.ToString(dtRackDetAis.Rows[0]["color_sfx"]);
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["symbol_code"]).Trim() != "")
            {
                strSymbolCode = Convert.ToString(dtRackDetAis.Rows[0]["symbol_code"]);
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["symbol_rtf"]).Trim() != "")
            {
                strSymbolRtf = Convert.ToString(dtRackDetAis.Rows[0]["symbol_rtf"]);
            }
            #endregion

            if (strPartsNo != "")
            {
                strAisRackDetAis = strHjId + "~" + strHjItemId + "~" + strHjRow + "~" + strHjCol + "~" + strPartsTitle + "~" + strPartsNo + "~" + strColorSfx + "~" + strSymbolCode + "~" + strSymbolRtf;

                return strAisRackDetAis;
            }
            else
            {
                return strAisRackDetAis;
            }
        }
        else
        {
            return strAisRackDetAis;
        }
    }
    #endregion

    #region Get Module Address String
    protected String getModDetString(String strRackDetId)
    {
        String strRackName = Convert.ToString(txtRackName.Text);
        String strAisRackDetModule = "";
        DataSet dsRackDetModule = new DataSet();
        System.Data.DataTable dtRackDetModule = new System.Data.DataTable();

        dsRackDetModule = csDatabase.GetRackMstDetModule(strRackDetId);
        dtRackDetModule = dsRackDetModule.Tables[0];

        #region Initialise Variable
        String strModuleAdd = "";
        String strModuleName = "";
        String strColorDI = "";
        #endregion

        if (dtRackDetModule.Rows.Count > 0)
        {
            #region Assign Variable
            if (Convert.ToString(dtRackDetModule.Rows[0]["module_add"]).Trim() != "")
            {
                strModuleAdd = Convert.ToString(dtRackDetModule.Rows[0]["module_add"]).Trim();
            }
            if (Convert.ToString(dtRackDetModule.Rows[0]["module_name"]).Trim() != "")
            {
                strModuleName = Convert.ToString(dtRackDetModule.Rows[0]["module_name"]);
            }
            if (Convert.ToString(dtRackDetModule.Rows[0]["color_di"]).Trim() != "")
            {
                strColorDI = Convert.ToString(dtRackDetModule.Rows[0]["color_di"]).Trim();
            }
            #endregion

            if (strModuleAdd != "")
            {
                strAisRackDetModule = strModuleAdd + "~" + strModuleName + "~" + strColorDI;
                return strAisRackDetModule;
            }
            else
            {
                return strAisRackDetModule;
            }
        }
        else
        {
            return strAisRackDetModule;
        }
    }
    #endregion

    #region Rack Light Simulation
    protected Boolean RackLightSimulation(String strRefPartsNum, String strRefColorSfx)
    {
        if (Convert.ToString(txtInsCode.Text) != "")
        {
            String strModel = Convert.ToString(txtModel.Text);
            String strSfx = Convert.ToString(txtSfx.Text);
            String strColor = Convert.ToString(txtColor.Text);
            //String strInsCode = Convert.ToString(ddInsCode.SelectedValue);

            return csDatabase.GetRackLightAvailability(strModel, strSfx, strColor, strRefPartsNum, strRefColorSfx);

            //DataSet dsHjParts = new DataSet();
            //DataTable dtHjParts = new DataTable();

            //dsHjParts = csDatabase.SrcHjParts(strModel, strSfx, strColor);
            //dtHjParts = dsHjParts.Tables[0];

            //#region Initialise Variable
            //String strPartsNo = "";
            //String strColorSfx = "";
            //#endregion

            //if (dtHjParts.Rows.Count > 0)
            //{
            //    for (int i = 0; i <= dtHjParts.Rows.Count - 1; i++)
            //    {
            //        if (Convert.ToString(dtHjParts.Rows[i]["part_no"]).Trim() != "")
            //        {
            //            strPartsNo = Convert.ToString(dtHjParts.Rows[i]["part_no"]).Trim();
            //            if (strPartsNo == strRefPartsNum)
            //            {
            //                if (Convert.ToString(dtHjParts.Rows[i]["color_sfx"]).Trim() != "")
            //                {
            //                    strColorSfx = Convert.ToString(dtHjParts.Rows[i]["color_sfx"]).Trim();
            //                    if (strColorSfx == strRefColorSfx)
            //                    {
            //                        return true;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //return false;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region SearchInsCodeMstList
    private System.Data.DataTable SearchInsCodeMstList(String DpsInsCode, String Model, String Katashiki, String Sfx, String Color, String Comment)
    {
        DataSet dsSearch = new DataSet();
        System.Data.DataTable dtSearch = new System.Data.DataTable();

        try
        {
            dsSearch = csDatabase.SrcInsCode(DpsInsCode, Model, Katashiki, Sfx, Color, Comment);
            dtSearch = dsSearch.Tables[0];
            return dtSearch;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return dtSearch;
        }
    }
    #endregion

    public String GetRangeAdd(Excel.Range xlRange)
    {
        try
        {
            return xlRange.get_AddressLocal(false, false, Excel.XlReferenceStyle.xlA1, misValue, misValue);
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage("Exception Occured while getting excel range address " + ex.ToString());
            return "";
        }
    }

    public String GetCellAdd(ExcelWorksheet worksheet, int row, int col)
    {
        //try
        //{
        //    xlWorkSheet_range = xlRef.get_Range(xlRef.Cells[row, col], xlRef.Cells[row, col]);
        //    return GetRangeAdd(xlWorkSheet_range);
        //}
        //catch (Exception ex)
        //{
        //    GlobalFunc.ShowErrorMessage("Exception Occured while transmit to user " + ex.ToString());
        //    return "";
        //}
        try
        {
            // In EPPlus, getting the address of a single cell is straightforward
            var cellAddress = worksheet.Cells[row, col].Address;
            return cellAddress;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage("Exception Occured while transmitting to user: " + ex.ToString());
            return "";
        }
    }

    //public int GetCellColor(Excel.Range xlRange, String strColor)
    //{
    //    try
    //    {
    //        switch (strColor)
    //        {
    //            case "SteelBlue":
    //                return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SteelBlue);
    //            case "MediumSeaGreen":
    //                return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.MediumSeaGreen);
    //            case "PowderBlue":
    //                return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PowderBlue);
    //            case "Crimson":
    //                return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Crimson);
    //            case "MediumOrchid":
    //                return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.MediumOrchid);
    //            case "Yellow":
    //                return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
    //            case "Gainsboro":
    //                return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gainsboro);
    //            case "Black":
    //                return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
    //            case "White":
    //                return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
    //            default:
    //                return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        GlobalFunc.ShowErrorMessage("Exception Occured while converting color " + ex.ToString());
    //        return 0;
    //    }
    //}

    public System.Drawing.Color GetCellColor(string strColor)
    {
        try
        {
            switch (strColor)
            {
                case "SteelBlue":
                    return System.Drawing.Color.SteelBlue;
                case "MediumSeaGreen":
                    return System.Drawing.Color.MediumSeaGreen;
                case "PowderBlue":
                    return System.Drawing.Color.PowderBlue;
                case "Crimson":
                    return System.Drawing.Color.Crimson;
                case "MediumOrchid":
                    return System.Drawing.Color.MediumOrchid;
                case "Yellow":
                    return System.Drawing.Color.Yellow;
                case "Gainsboro":
                    return System.Drawing.Color.Gainsboro;
                case "Black":
                    return System.Drawing.Color.Black;
                case "White":
                    return System.Drawing.Color.White;
                default:
                    return System.Drawing.Color.White;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage("Exception Occurred while converting color: " + ex.ToString());
            return System.Drawing.Color.White; // Return a default color in case of an error
        }
    }

    public void insData(int row, int col, string htext, string cell1, string cell2, int mergeColumns, string b, bool font, int size, string fcolor)
    {
        xlWorkSheet.Cells[row, col] = htext;
        xlWorkSheet_range = xlWorkSheet.get_Range(cell1, cell2);
        xlWorkSheet_range.Merge(mergeColumns);
        xlWorkSheet_range.Font.Name = "Arial";
        xlWorkSheet_range.Merge(mergeColumns);

        //xlWorkSheet_range.Font.Size = strFontSize;
        switch (b)
        {
            case "YELLOW":
                xlWorkSheet_range.Interior.Color = System.Drawing.Color.Yellow;
                break;
            case "GRAY":
                xlWorkSheet_range.Interior.Color = System.Drawing.Color.Gray.ToArgb();
                break;
            case "GAINSBORO":
                xlWorkSheet_range.Interior.Color = System.Drawing.Color.Gainsboro.ToArgb();
                break;
            case "Turquoise":
                xlWorkSheet_range.Interior.Color = System.Drawing.Color.Turquoise.ToArgb();
                break;
            case "PeachPuff":
                xlWorkSheet_range.Interior.Color = System.Drawing.Color.PeachPuff.ToArgb();
                break;
            default:
                //  workSheet_range.Interior.Color = System.Drawing.Color..ToArgb();
                break;
        }

        xlWorkSheet_range.Borders.Color = System.Drawing.Color.Black.ToArgb();
        xlWorkSheet_range.Font.Bold = font;
        xlWorkSheet_range.ColumnWidth = size;
        if (fcolor.Equals(""))
        {
            xlWorkSheet_range.Font.Color = System.Drawing.Color.White.ToArgb();
        }
        else
        {
            xlWorkSheet_range.Font.Color = System.Drawing.Color.Black.ToArgb();
        }//Change all cells' alignment to center
        //xlWorkSheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        //But then this line changes every cell style back to left alignment
        //xlWorkSheet.Cells[y + 1, x + 2].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
    }

    public void CreatePDFDocument(string strHtml)
    {
        Response.AppendHeader("content-disposition", "attachment;filename=ExportedHtml.xls");
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        this.EnableViewState = false;
        Response.Write(strHtml);
        Response.End();

        //ShowPdf(strFilePath);
    }

    public void ShowExcel(String strFilePath, String strFileName)
    {
        //try
        //{
        //Response.ClearContent();
        //Response.ClearHeaders();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName + ".xlsx");
        Response.TransmitFile(strFilePath);
        //File.Delete(strFilePath);
        //Response.WriteFile(strFilePath);
        //Response.Flush();
        //Response.Clear();
        Response.End();
        //}
        //catch (Exception ex)
        //{
        //    GlobalFunc.ShowErrorMessage("Exception Occured while transmit to user " + ex.ToString());
        //}
    }

    #region Release Excel from Memory
    private void releaseObject(object obj)
    {
        try
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            obj = null;
        }
        catch (Exception ex)
        {
            obj = null;
            GlobalFunc.ShowErrorMessage("Exception Occured while releasing object " + ex.ToString());
        }
        finally
        {
            GC.Collect();
        }
    }
    #endregion
    #endregion
}
