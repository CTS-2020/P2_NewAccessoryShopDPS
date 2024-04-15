using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Net;
using dpant;
using Itenso.Rtf;
using Itenso.Rtf.Converter.Html;
using Itenso.Rtf.Converter.Image;
using Itenso.Rtf.Converter.Text;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;

public partial class PlcConvResult : System.Web.UI.Page
{
    public bool renderPDF = false;

    protected override void Render(HtmlTextWriter output)
    {
        if (renderPDF == true)
        {
            MemoryStream mem = new MemoryStream();
            StreamWriter twr = new StreamWriter(mem);
            HtmlTextWriter myWriter = new HtmlTextWriter(twr);
            base.Render(myWriter);
            myWriter.Flush();
            myWriter.Dispose();
            StreamReader strmRdr = new StreamReader(mem);
            strmRdr.BaseStream.Position = 0;
            string pageContent = strmRdr.ReadToEnd();
            strmRdr.Dispose();
            mem.Dispose();
            //myWriter.Write(pageContent);
            CreatePDFDocument(pageContent);
        }
        else
        {
            //StringBuilder sb = new StringBuilder();
            //HtmlTextWriter tw = new HtmlTextWriter(new System.IO.StringWriter(sb));
            base.Render(output);
            // get the captured markup as a string
            //string pageSource = tw.ToString();
            //Get the rendered content
            //string sContent = sb.ToString();
            //Now output it to the page, if you want
            //writer.Write(sContent);
        }
    }

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
                genTable(strRackMstRow, strRackMstCol);

                if (strFlag == "export")
                {
                    renderPDF = true;
                }
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
        DataTable dtRackMst = new DataTable();

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

    #region Generate Table
    private void genTable(String strRackMstRow, String strRackMstCol)
    {
        try
        {
            AisPreview.Controls.Clear();
            Table tbAis = new Table();
            tbAis.Width = Unit.Percentage(100);
            tbAis.BorderWidth = Unit.Pixel(1);

            int rowCnt = 0;
            int colCnt = 0;
            int rowCtr = 0;
            int colCtr = 0;

            rowCnt = int.Parse(strRackMstRow);
            colCnt = int.Parse(strRackMstCol);

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                TableRow tRow = new TableRow();
                tRow.Width = Unit.Percentage(100);
                tRow.Height = Unit.Pixel(120);
                tRow.BorderWidth = Unit.Pixel(1);
                tbAis.Rows.Add(tRow);

                for (colCtr = 1; colCtr <= colCnt; colCtr++)
                {
                    String strAisDet = "";
                    String strModDet = "";
                    String strRackName = Convert.ToString(txtRackName.Text);
                    String strRackDetId = strRackName + "^" + rowCtr + "^" + colCtr;

                    TableCell tCell = new TableCell();
                    tCell.BorderWidth = Unit.Pixel(1);
                    tCell.Wrap = false;
                    tCell.Style.Add("table-layout", "fixed");
                    tCell.Style.Add("padding", "0px 0px 0px 0px");
                    tCell.Style.Add("min-width", "180px");
                    tCell.Style.Add("min-height", "120px");
                    tCell.Width = Unit.Pixel(180);
                    tCell.HorizontalAlign = HorizontalAlign.Left;
                    tCell.VerticalAlign = VerticalAlign.Top;

                    tRow.Cells.Add(tCell);

                    #region Get and Assign Ais Detail
                    LiteralControl hlAisData = new LiteralControl();
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
                    if (tmpAisValCnt == 1)
                    {
                        hlAisData.Text = strAisDet;
                    }
                    else
                    {
                        String strSymbol = Itenso.Solutions.Community.Rtf2Html.RtfConverterDemo.ConvertRtf2Txt(tmpAisVal[8]);
                        strSymbol = strSymbol.Replace("\r", "");
                        strSymbol = strSymbol.Replace("\n", "");

                        String strBgColor = csDatabase.GetPartsBgColor(tmpAisVal[5], tmpAisVal[6]);
                        String strFontColor = csDatabase.GetPartsFontColor(tmpAisVal[5], tmpAisVal[6]);

                        hlAisData.Text = tmpAisVal[4] + "</br>" +
                                         //"Row No : " + tmpAisVal[1] + "    Col No : " + tmpAisVal[2] + "</br>" +
                                         tmpAisVal[5] + " - " + tmpAisVal[6] + "</br>" +
                                         "<p style='text-align:center;'><font face='arial' size='6' color='" + strFontColor + "'><span style='background-color:" + strBgColor + ";'>&nbsp;" + strSymbol + "&nbsp;</span></font></p>";
                    }

                    tCell.Controls.Add(hlAisData);
                    #endregion

                    #region Get and Assign Module Address Detail
                    LiteralControl hlModuleAdd = new LiteralControl();
                    strModDet = getModDetString(strRackDetId);

                    #region Sample Data Retrieve
                    // 0 strModuleAdd
                    // 1 strModuleName
                    // 2 strLightColorDuringInstruction
                    #endregion
                    String[] tmpModVal = strModDet.Split('~');
                    int tmpModValCnt = tmpModVal.Length;
                    if (tmpModValCnt == 1)
                    {
                        hlModuleAdd.Text = strModDet;
                    }
                    else
                    {
                        hlModuleAdd.Text = "Module Address : " + tmpModVal[0];
                    }

                    #region Rack Light Simulation
                    if (tmpAisValCnt != 1)
                    {
                        if (tmpModValCnt != 1)
                        {
                            if (RackLightSimulation(tmpAisVal[5], tmpAisVal[6]) == true)
                            {
                                String strLightColor = GlobalFunc.getColorCode(tmpModVal[2]);

                                if (strLightColor != "")
                                {
                                    tCell.BackColor = System.Drawing.Color.FromName(strLightColor);
                                }
                                else
                                {
                                    tCell.BackColor = System.Drawing.Color.Gray;
                                }
                            }
                        }
                    }
                    #endregion

                    tCell.Controls.Add(hlModuleAdd);
                    #endregion
                }
            }
            AisPreview.Controls.Add(tbAis);
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
        String strAisRackDetAis = "</br></br></br></br></br>";
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
        String strAisRackDetModule = "</br></br>";
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

    public void CreatePDFDocument(string strHtml)
    {
        //NReco.PdfGenerator.HtmlToPdfConverter pdfGen = new NReco.PdfGenerator.HtmlToPdfConverter();
        //pdfGen.Size = NReco.PdfGenerator.PageSize.A3;
        //Byte[] pdfBytes = pdfGen.GeneratePdf(strHtml);
        //File.WriteAllBytes(@"C:\testpdf.pdf", pdfBytes);

        Response.AppendHeader("content-disposition", "attachment;filename=ExportedHtml.xls");
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        this.EnableViewState = true;
        Response.Write(strHtml);
        Response.End();

        //string strHTMLpath = Server.MapPath("MyHTML.html");
        //StreamWriter strWriter = new StreamWriter(strHTMLpath, false, Encoding.UTF8);
        //strWriter.Write(strHtml);
        //strWriter.Close();
        //string strFileName = HttpContext.Current.Server.MapPath("map1.pdf"); //    strFileName    "C:\\Inetpub\\wwwroot\\Test\\map1.pdf" 
        //// step 1: creation of a document-object
        //Document document = new Document();
        ////step 2:
        //// we create a writer that listens to the document
        //var writer = PdfWriter.GetInstance(document, new FileStream(strFileName, FileMode.Create));
        //StringReader se = new StringReader(strHtml);
        //TextReader tr = new StreamReader(Server.MapPath("MyHTML.html"),true);

        ////add the collection to the document
        //document.Open();
        //var htmlContext = new iTextSharp.tool.xml.pipeline.html.HtmlPipelineContext(null);
        //htmlContext.SetTagFactory(iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory());

        //iTextSharp.tool.xml.pipeline.css.ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
        //cssResolver.AddCssFile("stylesheet.css", true);
        //cssResolver.AddCssFile("webfunction/webfunc.css", true);

        //var pipeline = new iTextSharp.tool.xml.pipeline.css.CssResolverPipeline(cssResolver, new iTextSharp.tool.xml.pipeline.html.HtmlPipeline(htmlContext, new iTextSharp.tool.xml.pipeline.end.PdfWriterPipeline(document, writer)));
        //var worker = new XMLWorker(pipeline, true);
        //var p = new iTextSharp.tool.xml.parser.XMLParser(true, worker);
        //p.Parse(tr);
        //p.Flush();
        ////iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, tr);
        //document.Close();
        //ShowPdf(strFileName);
    }

    public void ShowPdf(string strFileName)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);
        Response.ContentType = "application/pdf";
        Response.WriteFile(strFileName);
        Response.Flush();
        Response.Clear();
    }   
    #endregion
}
