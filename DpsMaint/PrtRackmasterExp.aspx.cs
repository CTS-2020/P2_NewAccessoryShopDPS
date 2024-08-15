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
using System.Drawing;

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
            String strFlag = "";

            try
            {
                if (Convert.ToString(Request.QueryString["plc_no"]) != "") strPlcNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["plc_no"]));
                if (Convert.ToString(Request.QueryString["proc_name"]) != "") strProcName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["proc_name"]));
                if (Convert.ToString(Request.QueryString["group_name"]) != "") strGroupName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["group_name"]));
                if (Convert.ToString(Request.QueryString["block_name"]) != "") strBlockName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["block_name"]));
                if (Convert.ToString(Request.QueryString["rack_name"]) != "") strRackName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["rack_name"]));
                if (Convert.ToString(Request.QueryString["flag"]) != "") strFlag = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["flag"]));
                BindToText(strPlcNo, strProcName, strGroupName, strBlockName, strRackName);

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
    private void BindToText(String strPlcNo, String strProcName, String strGroupName, String strBlockName, String strRackName)
    {
        try
        {
            lblPlcNo.Text = strPlcNo;
            txtProcName.Text = strProcName;
            txtGroupName.Text = strGroupName;
            txtBlockName.Text = strBlockName;
            txtRackName.Text = strRackName;
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
        DataTable dtRackMst = new DataTable();

        dsRackMst = csDatabase.SrcRackMstDet(strRackName, strPlcNo, strProcName, strGroupName, strBlockName);
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
            // Set up the new culture
            System.Globalization.CultureInfo newCulture = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo oldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;

            // Load the template file using EPPlus
            string templatePath = Server.MapPath("~/template/RackMaster_template.xlsx");
            string strFileName = "DPSRackMaster-" + txtProcName.Text + "-" + txtGroupName.Text + "-" + DateTime.Now.ToString("ddMMyy");
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

            // Load the template
            using (var templatePackage = new ExcelPackage(templateFile))
            {
                if (templatePackage.Workbook.Worksheets.Count == 0)
                {
                    throw new Exception("The template file does not contain any worksheets.");
                }

                ExcelWorksheet templateWorksheet = templatePackage.Workbook.Worksheets[0];
                // Create a new package based on the template
                using (var package = new ExcelPackage())
                {

                    var newWorksheet = package.Workbook.Worksheets.Add("Sheet1", templateWorksheet);

                    newWorksheet.Cells[1, 1].Value = "DPS RACK MASTER :" + txtProcName.Text;

                    DataSet dsSearch = csDatabase.SrcRackMstDet(txtRackName.Text.Trim(), "", txtProcName.Text.Trim(), txtGroupName.Text.Trim(), txtBlockName.Text.Trim());
                    DataTable dtSearch = dsSearch.Tables[0];

                    if (dtSearch.Rows.Count > 0)
                    {
                        string oldrack = null;

                        int plcrow = 3;
                        int grprow = 4;
                        int procrow = 5;
                        int blkrow = 6;

                        int rnrow = 3;
                        int rrow = 4;
                        int crow = 5;

                        int startRow = 7;
                        int nxRackRow = 22;

                        foreach (DataRow row in dtSearch.Rows)
                        {
                            string newRack = row["proc_name"].ToString() + row["group_name"].ToString() + row["block_name"].ToString() + row["rack_name"].ToString() + row["plc_no"].ToString() + row["col_cnt"].ToString() + row["row_cnt"].ToString();

                            if (newRack != oldrack)
                            {
                                newWorksheet.Cells[plcrow, 5].Value = row["plc_no"].ToString();
                                newWorksheet.Cells[grprow, 5].Value = row["group_name"].ToString();
                                newWorksheet.Cells[procrow, 5].Value = row["proc_name"].ToString();
                                newWorksheet.Cells[blkrow, 5].Value = row["block_name"].ToString();

                                newWorksheet.Cells[rnrow, 13].Value = row["rack_name"].ToString();
                                newWorksheet.Cells[rrow, 13].Value = row["row_cnt"].ToString();
                                newWorksheet.Cells[crow, 13].Value = row["col_cnt"].ToString();


                                if (oldrack != null)
                                {
                                    startRow += nxRackRow;
                                }

                                oldrack = newRack;
                            }

                            String strAisDet = "";
                            String strModDet = "";
                            String strModuleAdd = "";
                            String strPartName = "";
                            String strPartNumber = "";
                            strModDet = getModDetString(row["rack_det_id"].ToString());

                            String[] tmpModVal = strModDet.Split('~');
                            int tmpModValCnt = tmpModVal.Length;
                            if (tmpModValCnt != 1)
                            {
                                strModuleAdd = tmpModVal[0];
                            }

                            strAisDet = getAisDetString(row["rack_det_id"].ToString());

                            String[] tmpAisVal = strAisDet.Split('~');
                            int tmpAisValCnt = tmpAisVal.Length;
                            if (tmpAisValCnt != 1)
                            {
                                strPartName = tmpAisVal[4];
                                strPartNumber = tmpAisVal[5] + " - " + tmpAisVal[6];
                            }

                            int celRow = Convert.ToInt32(row["rack_det_id"].ToString().Split('^')[1]);
                            int celCol = Convert.ToInt32(row["rack_det_id"].ToString().Split('^')[2]);

                            int nxCol = celCol > 1 ? ((celCol * 5) - 5) + 1 : 1;
                            int nxRow = celRow > 1 ? ((celRow * 3) - 3) + 1 : 1;

                            newWorksheet.Cells[startRow + nxRow, nxCol].Value = strPartName;
                            newWorksheet.Cells[startRow + nxRow + 1, nxCol].Value = strPartNumber;
                            newWorksheet.Cells[startRow + nxRow + 2, nxCol].Value = strModuleAdd;

                            newWorksheet.Cells[startRow + nxRow, nxCol, startRow + nxRow + 2, nxCol].Style.Font.Color.SetColor(Color.Black);
                        }
                    }

                    // Save the package to the file
                    package.SaveAs(newFile);
                }
            }

            // Restore the old culture
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCulture;

            // Show the generated Excel file
            ShowExcel(strFilePath, strFileName);
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(ex.Message + " " + ex.TargetSite.ToString());
        }


    }
    #endregion

    #region Get AIS String
    protected String getAisDetString(String strRackDetId)
    {
        String strRackName = Convert.ToString(txtRackName.Text);
        String strAisRackDetAis = "";
        DataSet dsRackDetAis = new DataSet();
        DataTable dtRackDetAis = new DataTable();

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
        DataTable dtRackDetModule = new DataTable();

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


    #region SearchInsCodeMstList
    private DataTable SearchInsCodeMstList(String DpsInsCode, String Model, String Katashiki, String Sfx, String Color, String Comment)
    {
        DataSet dsSearch = new DataSet();
        DataTable dtSearch = new DataTable();

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

    public String GetCellAdd(Excel.Worksheet xlRef, int row, int col)
    {
        try
        {
            xlWorkSheet_range = xlRef.get_Range(xlRef.Cells[row, col], xlRef.Cells[row, col]);
            return GetRangeAdd(xlWorkSheet_range);
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage("Exception Occured while transmit to user " + ex.ToString());
            return "";
        }
    }

    public int GetCellColor(Excel.Range xlRange, String strColor)
    {
        try
        {
            switch (strColor)
            {
                case "SteelBlue":
                    return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SteelBlue);
                case "MediumSeaGreen":
                    return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.MediumSeaGreen);
                case "PowderBlue":
                    return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PowderBlue);
                case "Crimson":
                    return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Crimson);
                case "MediumOrchid":
                    return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.MediumOrchid);
                case "Yellow":
                    return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                case "Gainsboro":
                    return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gainsboro);
                case "Black":
                    return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                case "White":
                    return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                default:
                    return System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage("Exception Occured while converting color " + ex.ToString());
            return 0;
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
