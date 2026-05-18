using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tms_acl_api.DAL;
using tms_acl_api.Helpers;
using tms_acl_api.Methods;
using tms_acl_api.Models;
using tms_acl_api.Infrastructure;

namespace tms_acl_api.Controllers
{
    [Authorize]
    [Route("api/ChecksheetMaster")]
    [ApiController]
    public class CheckSheetMasterController : ControllerBase
    {
        CommonFunction db = new CommonFunction("", AppConfiguration.GetConnectionString("MSSQL_PFRIMFP_ELVIS"));

        // get main screen checksheet dropdown
        [AllowAnonymous]
        [HttpGet]
        //[Route("GetChecksheet")]
        public async Task<IActionResult> GetChecksheet()
        {
            try
            {
                List<CheckSheetMaster> model = new List<CheckSheetMaster>();

                List<SqlParameter> SqlParam = new List<SqlParameter>();
                SqlParam.Add(new SqlParameter("@pMM_MasterCS_H_ID", 0));
                SqlParam.Add(new SqlParameter("@PUSERID", 0));
                SqlParam.Add(new SqlParameter("@PTypeSelect", ""));
                DataTable dt = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_SEL", System.Data.CommandType.StoredProcedure, SqlParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        CheckSheetMaster c = new CheckSheetMaster();
                        c.MODULE_H_ID = Convert.ToInt32(row["MODULE_H_ID"]);
                        c.MODULE_DISPLAY_NAME = row["MODULE_DISPLAY_NAME"].ToString();
                        c.MM_MasterCS_H_ID = Convert.ToInt32(row["MM_MasterCS_H_ID"]);
                        c.TITLE = row["TITLE"].ToString();
                        c.GROUP_H_ID = Convert.ToInt32(row["GROUP_H_ID"]);
                        c.GROUP_DISPLAY_NAME = row["GROUP_DISPLAY_NAME"].ToString();

                        var obj = new { pMM_MasterCS_H_ID = Convert.ToInt32(row["MM_MasterCS_H_ID"]), PHID = Convert.ToInt32(row["GROUP_H_ID"]), PTypeSelect = "getItem", };
                        c.LIST_GROUP_D = await db.PSP_COMMON_DAPPER<Master_Checksheet_D>("PSP_API_GET_MASTER_CHECKSHEET_SEL", CommandType.StoredProcedure, obj);
                        model.Add(c);
                    }
                    return Ok(model);
                }
                else
                {
                    return BadRequest("No record found.");
                }
                
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2("Api - Master Checksheet " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                err = null;
                return BadRequest(ex.ToString());
            }

        }


        // get main screen checksheet dropdown after select module id
        [AllowAnonymous]
        [HttpGet]
        [Route("GetMasterChecksheet/ID={pMM_MasterCS_H_ID}")]
        public async Task<IActionResult> Get(int pMM_MasterCS_H_ID)
        {
            try
            {
                List<CheckSheetMaster> model = new List<CheckSheetMaster>();

                List<SqlParameter> SqlParam = new List<SqlParameter>();
                SqlParam.Add(new SqlParameter("@pMM_MasterCS_H_ID", pMM_MasterCS_H_ID));
                SqlParam.Add(new SqlParameter("@PTypeSelect", ""));
                DataTable dt = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_SEL", System.Data.CommandType.StoredProcedure, SqlParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        CheckSheetMaster c = new CheckSheetMaster();
                        c.MODULE_H_ID = Convert.ToInt32(row["MODULE_H_ID"]);
                        c.MODULE_DISPLAY_NAME = row["MODULE_DISPLAY_NAME"].ToString();
                        c.MM_MasterCS_H_ID = Convert.ToInt32(row["MM_MasterCS_H_ID"]);
                        c.TITLE = row["TITLE"].ToString();
                        c.GROUP_H_ID = Convert.ToInt32(row["GROUP_H_ID"]);
                        c.GROUP_DISPLAY_NAME = row["GROUP_DISPLAY_NAME"].ToString();

                        var obj = new { pMM_MasterCS_H_ID = pMM_MasterCS_H_ID, PHID = Convert.ToInt32(row["GROUP_H_ID"]), PTypeSelect = "detail", };
                        c.LIST_GROUP_D = await db.PSP_COMMON_DAPPER<Master_Checksheet_D>("PSP_API_GET_MASTER_CHECKSHEET_SEL", CommandType.StoredProcedure, obj);
                        model.Add(c);
                    }
                    return Ok(model);
                }
                else
                {
                    return BadRequest("No record found.");
                }

            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2("Api - Master Checksheet GetModule " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                err = null;
                return BadRequest(ex.ToString());
            }

        }


        // get listing page checksheet 
        [AllowAnonymous]
        [HttpGet]
        [Route("Listing/checksheetID={pMM_MasterCS_H_ID}")]
        public async Task<IActionResult> Listing(int pMM_MasterCS_H_ID)
        {
            try
            {
                List<Master_DE_CHECK_SHEET_DH> model = new List<Master_DE_CHECK_SHEET_DH>();

                List<SqlParameter> SqlParam = new List<SqlParameter>();
                SqlParam.Add(new SqlParameter("@pMM_MasterCS_H_ID", pMM_MasterCS_H_ID));
                SqlParam.Add(new SqlParameter("@pTypeSelect", "list"));
                DataTable dt = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_LIST", System.Data.CommandType.StoredProcedure, SqlParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Master_DE_CHECK_SHEET_DH c = new Master_DE_CHECK_SHEET_DH();
                        c.DE_CUSTOM_CS_ID = Convert.ToInt32(row["DE_CUSTOM_CS_ID"]);
                        c.DE_CUSTOM_CS_LST_ID = Convert.ToInt32(row["DE_CUSTOM_CS_LST_ID"]);
                        c.EMPLOYEE_NO = Convert.ToInt32(row["EMPLOYEE_NO"]);
                        c.TRANS_DATETIME = row["TRANS_DATETIME"].ToString();
                        c.SHIFT = row["SHIFT"].ToString();
                        c.STATUS = row["STATUS"].ToString();
                        c.RECORD_TYP = row["RECORD_TYP"].ToString();
                        c.APPROVAL_EMPLOYEE_NO = row["APPROVAL_EMPLOYEE_NO"].ToString();
                        c.APPROVAL_EMPLOYEE_NAME = row["APPROVAL_EMPLOYEE_NAME"].ToString();
                        c.ISONo = row["ISONo"].ToString();

                        //if (row["APPROVAL_IND"].ToString() == "1")
                        //{
                        //    c.APPROVAL_EMPLOYEE_NO =row["APPROVAL_EMPLOYEE_NO"].ToString();
                        //    c.APPROVAL_EMPLOYEE_NAME = row["APPROVAL_EMPLOYEE_NAME"].ToString();
                        //}

                        List<SqlParameter> SqlParam2 = new List<SqlParameter>();
                        SqlParam2.Add(new SqlParameter("@pMM_MasterCS_H_ID", pMM_MasterCS_H_ID));
                        SqlParam2.Add(new SqlParameter("@pDE_CUSTOM_CS_ID", Convert.ToInt32(row["DE_CUSTOM_CS_ID"])));
                        SqlParam2.Add(new SqlParameter("@pTypeSelect", "getField"));
                        DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_LIST", System.Data.CommandType.StoredProcedure, SqlParam2);

                        if (dt2.Rows.Count > 0)
                        {
                            int counter = 1;
                            foreach (DataRow row2 in dt2.Rows)
                            {
                                string propertyName = "COL" + counter;
                                string propertyName2 = "COL" + counter + "_VALUE";
                                PropertyInfo propertyInfo = c.GetType().GetProperty(propertyName);
                                PropertyInfo propertyInfo2 = c.GetType().GetProperty(propertyName2);
                                if (propertyInfo != null && propertyInfo2 != null)
                                {
                                    propertyInfo.SetValue(c, row2["Label_Text"].ToString());
                                    propertyInfo2.SetValue(c, row2["RESULTS"].ToString());
                                }

                                counter++;
                            }
                        }

                        model.Add(c);
                    }
                    return Ok(model);
                }
                else
                {
                    return BadRequest("No record found.");
                }

            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2("Api - Master Checksheet Listing " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                err = null;
                return BadRequest(ex.ToString());
            }

        }

        private class DuplicateCol
        {
            public int ITEM_NO { get; set; }
            public int CS_SETUP_FORMAT_ID { get; set; }
            public string columnTitle { get; set; }
        }
       

        // get format page checksheet after select dropdown in main screen
        [AllowAnonymous]
        [HttpGet]
        [Route("GetFormat/checksheetID={pMM_MasterCS_H_ID}")]
        public async Task<IActionResult> GetFormat(int pMM_MasterCS_H_ID)
        {
            try
            {
                Master_TABLE_HEADER mdl = new Master_TABLE_HEADER();

                List<SqlParameter> SqlParam = new List<SqlParameter>();
                SqlParam.Add(new SqlParameter("@pID", pMM_MasterCS_H_ID));
                SqlParam.Add(new SqlParameter("@pStatus", "GET_CS_HEADER"));
                DataTable dt = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_FORMAT", System.Data.CommandType.StoredProcedure, SqlParam);

                List<Master_ModelHeader> header = new List<Master_ModelHeader>();
                List<Master_FixTable> fixTbl = new List<Master_FixTable>();
                List<Master_RptTable> rptTbl = new List<Master_RptTable>();
                List<Master_RptCol> rptCol = new List<Master_RptCol>();
                List<Master_CS_SELECTION> selection = new List<Master_CS_SELECTION>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    mdl.ISONo = dt.Rows[0]["ISONo"].ToString();
                    mdl.RevNo = dt.Rows[0]["RevNo"].ToString();
                    mdl.RevDate = dt.Rows[0]["RevDate"].ToString();

                    foreach (DataRow csHeader in dt.Rows)
                    {
                        Master_ModelHeader t = new Master_ModelHeader();
                        t.MM_MasterCS_D_ID = Convert.ToInt32(csHeader["MM_MasterCS_D_ID"]);
                        t.Label_Text = csHeader["Label_Text"].ToString();
                        t.MM_FieldType_H_ID = Convert.ToInt32(csHeader["MM_FieldType_H_ID"].ToString());
                        t.Label_Text_D_ID = Convert.ToInt32(csHeader["Label_Text_D_ID"].ToString());
                        t.ITEM_NULLABLE = csHeader["ITEM_NULLABLE"].ToString();
                        t.Text_Length = csHeader["Text_Length"].ToString();
                        t.Int_Max = csHeader["Int_Max"].ToString();
                        t.Int_Min = csHeader["Int_Min"].ToString();
                        t.Int_Tolerance = csHeader["Int_Tolerance"].ToString();
                        t.RowNo = Convert.ToInt32(csHeader["RowNo"].ToString());
                        t.ColNo = Convert.ToInt32(csHeader["ColNo"].ToString());
                        header.Add(t);
                        mdl.MODEL_HEADER = header;

                        if (csHeader["MM_FieldType_H_ID"].ToString() == "9")
                        {
                            List<SqlParameter> SqlParam2 = new List<SqlParameter>();
                            SqlParam2.Add(new SqlParameter("@pID", csHeader["Label_Text_D_ID"].ToString()));
                            SqlParam2.Add(new SqlParameter("@pStatus", "GET_FIX_TBL"));
                            DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_FORMAT", System.Data.CommandType.StoredProcedure, SqlParam2);

                            if (dt2 != null && dt2.Rows.Count > 0)
                            {
                                mdl.TotalColumn = Convert.ToInt32(dt2.Rows[0]["Table_Column"].ToString());
                                mdl.TotalRow = Convert.ToInt32(dt2.Rows.Count);

                                foreach (DataRow fixItem in dt2.Rows)
                                {
                                    Master_FixTable fixMdl = new Master_FixTable();
                                    fixMdl.MM_CSTable_ID = Convert.ToInt32(fixItem["MM_CSTable_ID"]);
                                    fixMdl.Table_Name = fixItem["Table_Name"].ToString();
                                    fixMdl.Table_Fix_Column = Convert.ToInt32(fixItem["Table_Fix_Column"].ToString());
                                    fixMdl.MM_FTable_ID = Convert.ToInt32(fixItem["MM_FTable_ID"].ToString());
                                    fixMdl.FTable_Item = fixItem["FTable_Item"].ToString();
                                    fixMdl.RowNo = Convert.ToInt32(fixItem["RowNo"].ToString());
                                    fixMdl.ColNo = Convert.ToInt32(fixItem["ColNo"].ToString());
                                    fixTbl.Add(fixMdl);
                                    
                                }

                               // mdl.MODEL_FIX_TBL = fixTbl;

                                mdl.GROUP_FIX = fixTbl
                                .GroupBy(x => x.MM_CSTable_ID)
                                .Select(g => new GroupedFixTable
                                {
                                    MM_CSTable_ID = g.Key,
                                    Data = g.ToList()
                                })
                                .ToList();
                            }


                            List<SqlParameter> SqlParam3 = new List<SqlParameter>();
                            SqlParam3.Add(new SqlParameter("@pID", csHeader["Label_Text_D_ID"].ToString()));
                            SqlParam3.Add(new SqlParameter("@pStatus", "GET_RPT_TBL"));
                            DataTable dt3 = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_FORMAT", System.Data.CommandType.StoredProcedure, SqlParam3);

                            if (dt3 != null && dt3.Rows.Count > 0)
                            {
                                foreach (DataRow rptItem in dt3.Rows)
                                {
                                    Master_RptTable rptMdl = new Master_RptTable();
                                    rptMdl.MM_CSTable_ID = Convert.ToInt32(rptItem["MM_CSTable_ID"]);
                                    rptMdl.Table_Name = rptItem["Table_Name"].ToString();
                                    rptMdl.Table_Column = Convert.ToInt32(rptItem["Table_Column"].ToString());
                                    rptMdl.MM_DTable_ID = Convert.ToInt32(rptItem["MM_DTable_ID"].ToString());
                                    rptMdl.DTable_Item = rptItem["DTable_Item"].ToString();
                                    rptMdl.MM_FieldType_H_ID = Convert.ToInt32(rptItem["MM_FieldType_H_ID"].ToString());
                                    rptMdl.DTable_Item_D_ID = Convert.ToInt32(rptItem["DTable_Item_D_ID"].ToString());
                                    rptMdl.ITEM_NULLABLE = rptItem["ITEM_NULLABLE"].ToString();
                                    rptMdl.Text_Length = rptItem["Text_Length"].ToString();
                                    rptMdl.Int_Max = rptItem["Int_Max"].ToString();
                                    rptMdl.Int_Min = rptItem["Int_Min"].ToString();
                                    rptMdl.Int_Tolerance = rptItem["Int_Tolerance"].ToString();
                                    rptMdl.RowNo = Convert.ToInt32(rptItem["RowNo"].ToString());
                                    rptTbl.Add(rptMdl);
                                    
                                }

                                //mdl.MODEL_RPT_TBL = rptTbl;

                                mdl.GROUP_RPT = rptTbl
                                .GroupBy(x => x.MM_CSTable_ID)
                                .Select(g => new GroupedRptTable
                                {
                                    MM_CSTable_ID = g.Key,
                                    Data = g.ToList()
                                })
                                .ToList();
                            }

                            List<SqlParameter> SqlParam5 = new List<SqlParameter>();
                            SqlParam5.Add(new SqlParameter("@pID", csHeader["Label_Text_D_ID"].ToString()));
                            SqlParam5.Add(new SqlParameter("@pStatus", "GET_TBL_COL"));
                            DataTable dt5 = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_FORMAT", System.Data.CommandType.StoredProcedure, SqlParam5);

                            if (dt5 != null && dt5.Rows.Count > 0)
                            {
                                foreach (DataRow rptColRow in dt5.Rows)
                                {
                                    Master_RptCol rptColMdl = new Master_RptCol();
                                    rptColMdl.MM_CSTable_ID = Convert.ToInt32(rptColRow["MM_CSTable_ID"]);
                                    rptColMdl.Table_Name = rptColRow["Table_Name"].ToString();
                                    rptColMdl.Table_Fix_Column = Convert.ToInt32(rptColRow["Table_Fix_Column"].ToString());
                                    rptColMdl.Table_Column = Convert.ToInt32(rptColRow["Table_Column"].ToString());
                                    rptColMdl.MM_ColTitle_ID = Convert.ToInt32(
                                        rptColRow["MM_ColTitle_ID"] == null || rptColRow["MM_ColTitle_ID"] == DBNull.Value
                                                                ? "0"
                                                                : rptColRow["MM_ColTitle_ID"].ToString()
                                                                );
                                    rptColMdl.ColTitle_Item = rptColRow["ColTitle_Item"].ToString();
                                    rptColMdl.RowNo = Convert.ToInt32(rptColRow["RowNo"].ToString());
                                    rptColMdl.ColNo = Convert.ToInt32(rptColRow["ColNo"].ToString());
                                    rptCol.Add(rptColMdl);

                                }

                                //mdl.MODEL_RPT_TBL = rptTbl;

                                mdl.GROUP_RPT_COL = rptCol
                                .GroupBy(x => x.MM_CSTable_ID)
                                .Select(g => new GroupedRptCol
                                {
                                    MM_CSTable_ID = g.Key,
                                    Data = g.ToList()
                                })
                                .ToList();
                            }
                        }
                        else
                        {

                        }
                    }

                    // get selection
                    List<SqlParameter> SqlParam4 = new List<SqlParameter>();
                    SqlParam4.Add(new SqlParameter("@pMM_MasterCS_H_ID", pMM_MasterCS_H_ID));
                    SqlParam4.Add(new SqlParameter("@pTypeSelect", "getDropdown"));
                    DataTable dt4 = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_LIST", System.Data.CommandType.StoredProcedure, SqlParam4);

                    if (dt4 != null && dt4.Rows.Count > 0)
                    {
                        foreach (DataRow selectionItem in dt4.Rows)
                        {
                            Master_CS_SELECTION selectionMdl = new Master_CS_SELECTION();
                            selectionMdl.MM_Selection_H_ID = Convert.ToInt32(selectionItem["MM_Selection_H_ID"]);
                            selectionMdl.Selection_Name = selectionItem["Selection_Name"].ToString();
                            selectionMdl.MM_Selection_D_ID = Convert.ToInt32(selectionItem["MM_Selection_D_ID"].ToString());
                            selectionMdl.Item_Name = selectionItem["Item_Name"].ToString();
                            selection.Add(selectionMdl);
                        }

                        mdl.SELECTION = selection;
                    }

                    return Ok(mdl);
                }
                 else
                 {
                    return BadRequest("No record found.");
                 }
            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                //await err.ErrorLog_Add_V2("Api - Custom Checksheet GetFormat " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                err = null;
                return BadRequest(ex.ToString());
            }
        }

        // POST: Save/Update Data into DE_CHECK_SHEET_D and DE_CHECK_SHEET_H table for button Submit
        [AllowAnonymous]
        [HttpPost]
        [Route("Save_DE_CS_DH")]
        public async Task<IActionResult> Save_DE_CS_DH([FromBody] Master_DE_CHECK_SHEET_DH dto)
        {
            try
            {
                //var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());

                string location = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
                //DE_CUSTOM_CHECK_SHEET_DH dto = new DE_CUSTOM_CHECK_SHEET_DH();
                //if (provider.FormData != null)
                //{
                //dto = JsonConvert.DeserializeObject<DE_CUSTOM_CHECK_SHEET_DH>(provider.FormData["json"]);
                //Create a new DataTable object
                DataTable dt = new DataTable();

                // Add columns to the DataTable
                dt.Columns.Add("DE_CUSTOM_CS_ID");
                dt.Columns.Add("DE_CUSTOM_CS_H_ID");
                dt.Columns.Add("DE_CUSTOM_CS_LST_ID");
                dt.Columns.Add("CS_HEADER_D_ID");
                dt.Columns.Add("EMPLOYEE_NO");
                dt.Columns.Add("SHIFT");
                dt.Columns.Add("TRANS_DATETIME");
                dt.Columns.Add("RESULTS");
                dt.Columns.Add("REMARK");
                dt.Columns.Add("FOOTER_ATTACHMENT");
                dt.Columns.Add("DE_CUSTOM_CS_D_ID");
                dt.Columns.Add("CS_CUSTOM_ID");
                dt.Columns.Add("CS_CUSTOM_ID_IND");
                dt.Columns.Add("ITEM_RESULT");
                dt.Columns.Add("ITEM_ATTCH/SKETCH");
                dt.Columns.Add("CS_CUSTOM_H_ID");
                dt.Columns.Add("ITEM_NO");
                dt.Columns.Add("ITEM_SKETCH");
                //dt.Columns.Add("MM_MasterCS_H_ID");

                var random = new Random();

                // Add rows to the DataTable
                if (dto.DATA_HEADER.Count > 0)
                {
                    foreach (SAVE_MASTER_DE_CHECK_SHEET_H d in dto.DATA_HEADER)
                    {
                        DataRow dr = dt.NewRow();
                        dr["DE_CUSTOM_CS_ID"] = dto.DE_CUSTOM_CS_ID;
                        dr["DE_CUSTOM_CS_H_ID"] = d.DE_CUSTOM_CS_H_ID;
                        dr["DE_CUSTOM_CS_LST_ID"] = dto.DE_CUSTOM_CS_LST_ID;
                        dr["CS_HEADER_D_ID"] = d.MM_MasterCS_D_ID;
                        dr["EMPLOYEE_NO"] = dto.EMPLOYEE_NO;
                        dr["SHIFT"] = dto.SHIFT;
                        dr["TRANS_DATETIME"] = dto.TRANS_DATETIME;
                        var path = "";

                        if(d.RESULTS == "")
                        {
                            dr["RESULTS"] = d.RESULTS;
                        }
                        else
                        {
                            try
                            {

                                var imageObject = d.RESULTS;

                                if (imageObject.Contains(','))
                                {
                                    var base64ImageWithoutPrefix = imageObject.Split(',')[1];

                                    if (CheckValidBase64String(base64ImageWithoutPrefix))
                                    {
                                        // Convert base64 string to byte array
                                        byte[] imageBytes = Convert.FromBase64String(base64ImageWithoutPrefix);

                                        var fileName = random.Next().ToString() + "." + imageObject.Split('/')[1].Split(';')[0];

                                        var livePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "wwwroot", "img", "header", fileName);

                                        if (Directory.Exists(System.IO.Path.Combine(System.AppContext.BaseDirectory, "wwwroot", "img", "header")))
                                        {
                                            using (FileStream fileStream = new FileStream(livePath, FileMode.Create))
                                            {
                                                fileStream.Write(imageBytes, 0, imageBytes.Length);
                                            }
                                        }
                                        else
                                        {
                                            Directory.CreateDirectory(System.IO.Path.Combine(System.AppContext.BaseDirectory, "wwwroot", "img", "header"));


                                            using (FileStream fileStream = new FileStream(livePath, FileMode.Create))
                                            {
                                                fileStream.Write(imageBytes, 0, imageBytes.Length);
                                            }
                                        }

                                        path = "http://cld-pfr-app001.toray.my:128/img/header/" + fileName;

                                        dr["RESULTS"] = path;
                                    }
                                    else
                                    {
                                        dr["RESULTS"] = d.RESULTS;
                                    }
                                }
                                else
                                {
                                    dr["RESULTS"] = d.RESULTS;
                                }
                            }
                            catch (Exception ex)
                            {
                                //dr["RESULTS"] = d.RESULTS;
                                ErrorLogSys err = new ErrorLogSys();
                                await err.ErrorLog_Add_V2("Api - Master Checksheet Save Header Attch " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                                err = null;
                                return BadRequest(ex.ToString());
                            }
                        }
                        dr["REMARK"] = dto.REMARK;
                        dr["CS_CUSTOM_H_ID"] = dto.MM_MasterCS_H_ID;
                        dr["ITEM_NO"] = d.ITEM_NO;
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["DE_CUSTOM_CS_ID"] = dto.DE_CUSTOM_CS_ID;
                    dr["DE_CUSTOM_CS_LST_ID"] = dto.DE_CUSTOM_CS_LST_ID;
                    dr["EMPLOYEE_NO"] = dto.EMPLOYEE_NO;
                    dr["SHIFT"] = dto.SHIFT;
                    dr["TRANS_DATETIME"] = dto.TRANS_DATETIME;
                    dr["REMARK"] = dto.REMARK;
                    dr["CS_CUSTOM_H_ID"] = dto.MM_MasterCS_H_ID;
                    dt.Rows.Add(dr);
                }


                List<SqlParameter> Listparam = new List<SqlParameter>();
                Listparam.Add(new SqlParameter("@pTbl", dt));
                Listparam.Add(new SqlParameter("@pType", "saveHeader"));
                Listparam.Add(new SqlParameter("@pREC_TYPE", "3"));
                Listparam.Add(new SqlParameter("@pUPDATED_BY", dto.UPDATED_BY));
                Listparam.Add(new SqlParameter("@pUPDATED_LOC", dto.UPDATED_LOC));
                DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_POST_MASTER_CHECKSHEET_DE_CS_DH", CommandType.StoredProcedure, Listparam);
                

                foreach (SAVE_MASTER_DE_CHECK_SHEET_DH d in dto.DATA)
                {
                    DataRow dr = dt.NewRow();
                    dr["DE_CUSTOM_CS_ID"] = dt2.Rows[0]["DE_CUSTOM_CS_ID"].ToString();
                    dr["DE_CUSTOM_CS_LST_ID"] = dto.DE_CUSTOM_CS_LST_ID;
                    dr["DE_CUSTOM_CS_D_ID"] = d.DE_CUSTOM_CS_D_ID;
                    dr["CS_CUSTOM_ID"] = d.CS_CUSTOM_ID;
                    dr["CS_CUSTOM_ID_IND"] = "CS_SETUP_D_ID";

                    if (d.ITEM_RESULT == "")
                    {
                        dr["ITEM_RESULT"] = d.ITEM_RESULT;
                    }
                    else
                    {
                        try
                        {
                            var imageObject = d.ITEM_RESULT;

                            if (imageObject.Contains(','))
                            {

                                var base64ImageWithoutPrefix = imageObject.Split(',')[1];

                                if (CheckValidBase64String(base64ImageWithoutPrefix))
                                {
                                    // Convert base64 string to byte array
                                    byte[] imageBytes = Convert.FromBase64String(base64ImageWithoutPrefix);

                                    var fileName = random.Next().ToString() + "." + imageObject.Split('/')[1].Split(';')[0];

                                    var livePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "wwwroot", "img", "attch", fileName);

                                    if (Directory.Exists(System.IO.Path.Combine(System.AppContext.BaseDirectory, "wwwroot", "img", "attch")))
                                    {
                                        using (FileStream fileStream = new FileStream(livePath, FileMode.Create))
                                        {
                                            fileStream.Write(imageBytes, 0, imageBytes.Length);
                                        }
                                    }
                                    else
                                    {
                                        Directory.CreateDirectory(System.IO.Path.Combine(System.AppContext.BaseDirectory, "wwwroot", "img", "attch"));


                                        using (FileStream fileStream = new FileStream(livePath, FileMode.Create))
                                        {
                                            fileStream.Write(imageBytes, 0, imageBytes.Length);
                                        }
                                    }

                                    dr["ITEM_RESULT"] = "http://cld-pfr-app001.toray.my:128/img/attch/" + fileName;
                                }
                                else
                                {
                                    dr["ITEM_RESULT"] = d.ITEM_RESULT;
                                }
                            }
                            else
                            {
                                dr["ITEM_RESULT"] = d.ITEM_RESULT;
                            }
                        }
                        catch (Exception ex)
                        {
                            //dr["ITEM_RESULT"] = d.ITEM_RESULT;
                            ErrorLogSys err = new ErrorLogSys();
                            await err.ErrorLog_Add_V2("Api - Master Checksheet Save Header Attch " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                            err = null;
                            return BadRequest(ex.ToString());
                        }
                    }
                    dr["ITEM_NO"] = d.ITEM_NO;
                    //var pathAttch = "";
                    //var pathSketch = "";
                    //if (d.ATTACHMENT != null)
                    //{
                    //    try
                    //    {
                    //        var imageObject = d.ATTACHMENT;

                    //        if (imageObject.uri.Contains(','))
                    //        {

                    //            var base64ImageWithoutPrefix = imageObject.uri.Split(',')[1];

                    //            if (CheckValidBase64String(base64ImageWithoutPrefix))
                    //            {
                    //                // Convert base64 string to byte array
                    //                byte[] imageBytes = Convert.FromBase64String(base64ImageWithoutPrefix);

                    //                var fileName = random.Next().ToString() + "." + imageObject.type.Split('/')[1];

                    //                var livePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "wwwroot", "img", "attch", fileName);

                    //                if (Directory.Exists(System.IO.Path.Combine(System.AppContext.BaseDirectory, "wwwroot", "img", "attch")))
                    //                {
                    //                    using (FileStream fileStream = new FileStream(livePath, FileMode.Create))
                    //                    {
                    //                        fileStream.Write(imageBytes, 0, imageBytes.Length);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    Directory.CreateDirectory(System.IO.Path.Combine(System.AppContext.BaseDirectory, "wwwroot", "img", "attch"));


                    //                    using (FileStream fileStream = new FileStream(livePath, FileMode.Create))
                    //                    {
                    //                        fileStream.Write(imageBytes, 0, imageBytes.Length);
                    //                    }
                    //                }

                    //                pathAttch = "http://cld-pfr-app001.toray.my:168/img/attch/" + fileName;
                    //            }
                    //            else
                    //            {
                    //                pathAttch = d.ATTACHMENT.uri;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            pathAttch = d.ATTACHMENT.uri;
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        //path = d.ATTACHMENT.uri;
                    //        ErrorLogSys err = new ErrorLogSys();
                    //        await err.ErrorLog_Add_V2("Api - Master Checksheet Save Attach " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                    //        err = null;
                    //        return BadRequest(ex.ToString());
                    //    }

                    //}

                    //if (d.SKETCH != null)
                    //{
                    //    //dr["CS_CUSTOM_ID"] = d.CS_SETUP_FORMAT_ID;
                    //    //dr["CS_CUSTOM_ID_IND"] = "CS_SETUP_FORMAT_ID";

                    //    try
                    //    {
                    //        var imageObject = d.SKETCH;

                    //        if (imageObject.uri.Contains(','))
                    //        {
                    //            var base64ImageWithoutPrefix = imageObject.uri.Split(',')[1];

                    //            if (CheckValidBase64String(base64ImageWithoutPrefix))
                    //            {
                    //                // Convert base64 string to byte array
                    //                byte[] imageBytes = Convert.FromBase64String(base64ImageWithoutPrefix);

                    //                var fileName = random.Next().ToString() + "." + imageObject.type.Split('/')[1];

                    //                var livePath = HttpContext.Current.Server.MapPath("~/img/sketch/" + fileName);

                    //                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/img/sketch/")))
                    //                {
                    //                    using (FileStream fileStream = new FileStream(livePath, FileMode.Create))
                    //                    {
                    //                        fileStream.Write(imageBytes, 0, imageBytes.Length);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/img/sketch/"));


                    //                    using (FileStream fileStream = new FileStream(livePath, FileMode.Create))
                    //                    {
                    //                        fileStream.Write(imageBytes, 0, imageBytes.Length);
                    //                    }
                    //                }

                    //                pathSketch = "http://cld-pfr-app001.toray.my:168/img/sketch/" + fileName;
                    //            }
                    //            else
                    //            {
                    //                pathSketch = d.SKETCH.uri;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            pathSketch = d.SKETCH.uri;
                    //        }

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        //path = d.SKETCH.uri;
                    //        ErrorLogSys err = new ErrorLogSys();
                    //        await err.ErrorLog_Add_V2("Api - Master Checksheet Save Sketch " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                    //        err = null;
                    //        return BadRequest(ex.ToString());
                    //    }
                    //}

                    //dr["ITEM_ATTCH/SKETCH"] = pathAttch;
                    //dr["ITEM_SKETCH"] = pathSketch;
                    dt.Rows.Add(dr);
                }

                List<SqlParameter> Listparam2 = new List<SqlParameter>();
                Listparam2.Add(new SqlParameter("@pTbl", dt));
                Listparam2.Add(new SqlParameter("@pType", ""));
                Listparam2.Add(new SqlParameter("@pREC_TYPE", "3"));
                Listparam2.Add(new SqlParameter("@pUPDATED_BY", dto.UPDATED_BY));
                Listparam2.Add(new SqlParameter("@pUPDATED_LOC", dto.UPDATED_LOC));
                DataTable dt3 = await db.PSP_COMMON_SQL("PSP_API_POST_MASTER_CHECKSHEET_DE_CS_DH", CommandType.StoredProcedure, Listparam2);


                if (dt3 != null && dt3.Rows.Count > 0 && dt3.Rows[0]["Status"].ToString() == "1")
                {
                    return Ok("OK");
                }
                else
                {
                    return BadRequest("Error when saving");
                }

            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2("Api - Master Checksheet Save_DE_CS_DH " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                err = null;
                return BadRequest(ex.ToString());
            }

        }

        private bool CheckValidBase64String(string uri)
        {
            try
            {
                // Remove any whitespace to prevent false positives.
                uri = uri.Trim();

                // Decode the base64 string and verify it's non-empty.
                byte[] imageBytes = Convert.FromBase64String(uri);
                return imageBytes.Length > 0;
            }
            catch
            {
                // If any exception occurs, it's not a valid base64 string.
                return false;
            }
        }


        // POST: Delete Data in DE_CHECK_SHEET_D and DE_CHECK_SHEET_H table for button Delete
        [AllowAnonymous]
        [HttpPost]
        [Route("Delete_DE_CS_DH")]
        public async Task<IActionResult> Delete_DE_CS_DH([FromBody] Master_DE_CHECK_SHEET_DH dto)
        {
            try
            {
                var obj = dto;

                //Create a new DataTable object
                DataTable dt = new DataTable();

                // Add columns to the DataTable
                dt.Columns.Add("DE_CUSTOM_CS_ID");
                dt.Columns.Add("DE_CUSTOM_CS_H_ID");
                dt.Columns.Add("DE_CUSTOM_CS_LST_ID");
                dt.Columns.Add("CS_HEADER_D_ID");
                dt.Columns.Add("EMPLOYEE_NO");
                dt.Columns.Add("SHIFT");
                dt.Columns.Add("TRANS_DATETIME");
                dt.Columns.Add("RESULTS");
                dt.Columns.Add("REMARK");
                dt.Columns.Add("FOOTER_ATTACHMENT");
                dt.Columns.Add("DE_CUSTOM_CS_D_ID");
                dt.Columns.Add("CS_CUSTOM_ID");
                dt.Columns.Add("CS_CUSTOM_ID_IND");
                dt.Columns.Add("ITEM_RESULT");
                dt.Columns.Add("ITEM_ATTCH/SKETCH");
                dt.Columns.Add("CS_CUSTOM_H_ID");
                dt.Columns.Add("ITEM_NO");
                dt.Columns.Add("ITEM_SKETCH");

                DataRow dr = dt.NewRow();
                dr["DE_CUSTOM_CS_ID"] = dto.DE_CUSTOM_CS_ID;
                dt.Rows.Add(dr);

                List<SqlParameter> Listparam = new List<SqlParameter>();
                Listparam.Add(new SqlParameter("@pTbl", dt));
                Listparam.Add(new SqlParameter("@pREC_TYPE", "5"));
                Listparam.Add(new SqlParameter("@pUPDATED_BY", dto.UPDATED_BY));
                Listparam.Add(new SqlParameter("@pUPDATED_LOC", dto.UPDATED_LOC));
                DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_POST_MASTER_CHECKSHEET_DE_CS_DH", CommandType.StoredProcedure, Listparam);


                if (dt2 != null && dt2.Rows.Count > 0 && dt2.Rows[0]["Status"].ToString() == "1")
                {
                    return Ok("OK");
                }
                else
                {
                    return BadRequest("Error when deleting");
                }

            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2("Api - Master Checksheet Delete_DE_CS_DH " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                err = null;
                return BadRequest(ex.ToString());
            }

        }


        // POST: Save Dropdown Data into DE_CHECK_SHEET_LST table (after chose from main screen)
        [AllowAnonymous]
        [HttpPost]
        [Route("Save_DE_CS_List")]
        public async Task<IActionResult> Save_DE_CS_List([FromBody] DE_CHECK_SHEET_MASTER_LST dto)
        {
            try
            {
                 var obj = dto;

                //Create a new DataTable object
                DataTable dt = new DataTable();

                // Add columns to the DataTable
                dt.Columns.Add("DE_CUSTOM_CS_LST_ID");
                dt.Columns.Add("MODULE_H_ID");
                dt.Columns.Add("CS_CUSTOM_H_ID");
                dt.Columns.Add("GROUP_H_ID");
                dt.Columns.Add("GROUP_D_ID");


                // Add rows to the DataTable
                foreach (DE_CHECK_SHEET_MASTER_LST_D d in dto.GROUP_H_DATA)
                {
                    DataRow dr = dt.NewRow();
                    dr["DE_CUSTOM_CS_LST_ID"] = 0;
                    dr["MODULE_H_ID"] = dto.MODULE_H_ID;
                    dr["CS_CUSTOM_H_ID"] = dto.MM_MasterCS_H_ID;
                    dr["GROUP_H_ID"] = d.GROUP_H_ID;
                    dr["GROUP_D_ID"] = d.GROUP_D_ID;
                    dt.Rows.Add(dr);
                }

                List<SqlParameter> Listparam = new List<SqlParameter>();
                Listparam.Add(new SqlParameter("@pTbl", dt));
                Listparam.Add(new SqlParameter("@pUPDATED_BY", dto.UPDATED_BY));
                Listparam.Add(new SqlParameter("@pUPDATED_LOC", dto.UPDATED_LOC));
                DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_POST_MASTER_CHECKSHEET_DE_CS", CommandType.StoredProcedure, Listparam);
                

                if (dt2 != null && dt2.Rows.Count > 0 && dt2.Rows[0]["Status"].ToString() == "1")
                {
                    string value1 = dt2.Rows[0]["DE_CUSTOM_CS_LST_ID"].ToString();
                    string value2 = dt2.Rows[0]["CS_CUSTOM_H_ID"].ToString();
                    return Ok((DE_CHECK_SHEET_LST_ID: value1, MODULE_D_ID: value2));
                }
                else
                {
                    return BadRequest("Error when saving");
                }

            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2("Api - Master Checksheet Save_DE_CS_List " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                err = null;
                return BadRequest(ex.ToString());
            }

        }

        //APPROVAL
        // get view page checksheet from listing page (new request to include approval ind) 
        //if approval_ind > 0, ui should have reject/approve button, if approval_ind = 0, no reject/approve button
        [AllowAnonymous]
        [HttpGet]
        [Route("GetApproval/ChecksheetHeaderID={pMM_MasterCS_H_ID}/ChecksheetID={pDE_CUSTOM_CS_ID}")]
        public async Task<IActionResult> GetApproval(int pMM_MasterCS_H_ID, int pDE_CUSTOM_CS_ID)
        {
            try
            {
                Master_TABLE_HEADER mdl = new Master_TABLE_HEADER();

                List<SqlParameter> SqlParam = new List<SqlParameter>();
                SqlParam.Add(new SqlParameter("@pID", pMM_MasterCS_H_ID));
                SqlParam.Add(new SqlParameter("@pDE_CUSTOM_CS_ID", pDE_CUSTOM_CS_ID));
                SqlParam.Add(new SqlParameter("@pStatus", "GET_CS_HEADER"));
                DataTable dt = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_DTL", System.Data.CommandType.StoredProcedure, SqlParam);

                List<Master_ModelHeader> header = new List<Master_ModelHeader>();
                List<Master_FixTable> fixTbl = new List<Master_FixTable>();
                List<Master_RptTable> rptTbl = new List<Master_RptTable>();
                List<Master_CS_SELECTION> selection = new List<Master_CS_SELECTION>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    mdl.ISONo = dt.Rows[0]["ISONo"].ToString();
                    mdl.RevNo = dt.Rows[0]["RevNo"].ToString();
                    mdl.RevDate = dt.Rows[0]["RevDate"].ToString();
                    mdl.REMARK = dt.Rows[0]["REMARK"].ToString();

                    foreach (DataRow csHeader in dt.Rows)
                    {
                        Master_ModelHeader t = new Master_ModelHeader();
                        t.MM_MasterCS_D_ID = Convert.ToInt32(csHeader["MM_MasterCS_D_ID"]);
                        t.Label_Text = csHeader["Label_Text"].ToString();
                        t.MM_FieldType_H_ID = Convert.ToInt32(csHeader["MM_FieldType_H_ID"].ToString());
                        t.Label_Text_D_ID = Convert.ToInt32(csHeader["Label_Text_D_ID"].ToString());
                        t.ITEM_NULLABLE = csHeader["ITEM_NULLABLE"].ToString();
                        t.Text_Length = csHeader["Text_Length"].ToString();
                        t.Int_Max = csHeader["Int_Max"].ToString();
                        t.Int_Min = csHeader["Int_Min"].ToString();
                        t.Int_Tolerance = csHeader["Int_Tolerance"].ToString();
                        t.RowNo = Convert.ToInt32(csHeader["RowNo"].ToString());
                        t.ColNo = Convert.ToInt32(csHeader["ColNo"].ToString());
                        t.RESULTS = csHeader["RESULTS"].ToString();
                        t.DE_CUSTOM_CS_H_ID = Convert.ToInt32(csHeader["DE_CUSTOM_CS_H_ID"].ToString());
                        header.Add(t);
                        mdl.MODEL_HEADER = header;

                        if (csHeader["MM_FieldType_H_ID"].ToString() == "9")
                        {
                            List<SqlParameter> SqlParam2 = new List<SqlParameter>();
                            SqlParam2.Add(new SqlParameter("@pID", csHeader["Label_Text_D_ID"].ToString()));
                            SqlParam2.Add(new SqlParameter("@pStatus", "GET_FIX_TBL"));
                            DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_DTL", System.Data.CommandType.StoredProcedure, SqlParam2);

                            if (dt2 != null && dt2.Rows.Count > 0)
                            {
                                mdl.TotalColumn = Convert.ToInt32(dt2.Rows[0]["Table_Column"].ToString());
                                mdl.TotalRow = Convert.ToInt32(dt2.Rows.Count);

                                foreach (DataRow fixItem in dt2.Rows)
                                {
                                    Master_FixTable fixMdl = new Master_FixTable();
                                    fixMdl.MM_CSTable_ID = Convert.ToInt32(fixItem["MM_CSTable_ID"]);
                                    fixMdl.Table_Name = fixItem["Table_Name"].ToString();
                                    fixMdl.Table_Fix_Column = Convert.ToInt32(fixItem["Table_Fix_Column"].ToString());
                                    fixMdl.MM_FTable_ID = Convert.ToInt32(fixItem["MM_FTable_ID"].ToString());
                                    fixMdl.FTable_Item = fixItem["FTable_Item"].ToString();
                                    fixMdl.RowNo = Convert.ToInt32(fixItem["RowNo"].ToString());
                                    fixMdl.ColNo = Convert.ToInt32(fixItem["ColNo"].ToString());
                                    fixTbl.Add(fixMdl);

                                }

                                // mdl.MODEL_FIX_TBL = fixTbl;

                                mdl.GROUP_FIX = fixTbl
                                .GroupBy(x => x.MM_CSTable_ID)
                                .Select(g => new GroupedFixTable
                                {
                                    MM_CSTable_ID = g.Key,
                                    Data = g.ToList()
                                })
                                .ToList();
                            }


                            List<SqlParameter> SqlParam3 = new List<SqlParameter>();
                            SqlParam3.Add(new SqlParameter("@pID", csHeader["Label_Text_D_ID"].ToString()));
                            SqlParam3.Add(new SqlParameter("@pDE_CUSTOM_CS_ID", pDE_CUSTOM_CS_ID));
                            SqlParam3.Add(new SqlParameter("@pStatus", "GET_RPT_TBL"));
                            DataTable dt3 = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_DTL", System.Data.CommandType.StoredProcedure, SqlParam3);

                            if (dt3 != null && dt3.Rows.Count > 0)
                            {
                                foreach (DataRow rptItem in dt3.Rows)
                                {
                                    Master_RptTable rptMdl = new Master_RptTable();
                                    rptMdl.MM_CSTable_ID = Convert.ToInt32(rptItem["MM_CSTable_ID"]);
                                    rptMdl.Table_Name = rptItem["Table_Name"].ToString();
                                    rptMdl.Table_Column = Convert.ToInt32(rptItem["Table_Column"].ToString());
                                    rptMdl.MM_DTable_ID = Convert.ToInt32(rptItem["MM_DTable_ID"].ToString());
                                    rptMdl.DTable_Item = rptItem["DTable_Item"].ToString();
                                    rptMdl.MM_FieldType_H_ID = Convert.ToInt32(rptItem["MM_FieldType_H_ID"].ToString());
                                    rptMdl.DTable_Item_D_ID = Convert.ToInt32(rptItem["DTable_Item_D_ID"].ToString());
                                    rptMdl.ITEM_NULLABLE = rptItem["ITEM_NULLABLE"].ToString();
                                    rptMdl.Text_Length = rptItem["Text_Length"].ToString();
                                    rptMdl.Int_Max = rptItem["Int_Max"].ToString();
                                    rptMdl.Int_Min = rptItem["Int_Min"].ToString();
                                    rptMdl.Int_Tolerance = rptItem["Int_Tolerance"].ToString();
                                    rptMdl.RowNo = Convert.ToInt32(rptItem["RowNo"].ToString());
                                    rptMdl.ITEM_RESULT = rptItem["RESULTS"].ToString();
                                    rptMdl.CS_CUSTOM_ID = Convert.ToInt32(rptItem["CS_CUSTOM_ID"].ToString());
                                    rptMdl.DE_CUSTOM_CS_D_ID = Convert.ToInt32(rptItem["DE_CUSTOM_CS_D_ID"].ToString());
                                    rptMdl.ITEM_NO = Convert.ToInt32(rptItem["ITEM_NO"].ToString());
                                    rptTbl.Add(rptMdl);

                                }

                                //mdl.MODEL_RPT_TBL = rptTbl;

                                mdl.GROUP_RPT = rptTbl
                                 .GroupBy(x => new { x.MM_CSTable_ID, x.ITEM_NO })
                                 .Select(g => new GroupedRptTable
                                 {
                                     MM_CSTable_ID = g.Key.MM_CSTable_ID,
                                     ITEM_NO = g.Key.ITEM_NO,
                                    Data = g.ToList()
                                 })
                                 .ToList();
                            }
                        }
                        else
                        {

                        }
                    }

                    // get selection
                    List<SqlParameter> SqlParam4 = new List<SqlParameter>();
                    SqlParam4.Add(new SqlParameter("@pMM_MasterCS_H_ID", pMM_MasterCS_H_ID));
                    SqlParam4.Add(new SqlParameter("@pTypeSelect", "getDropdown"));
                    DataTable dt4 = await db.PSP_COMMON_SQL("PSP_API_GET_MASTER_CHECKSHEET_LIST", System.Data.CommandType.StoredProcedure, SqlParam4);

                    if (dt4 != null && dt4.Rows.Count > 0)
                    {
                        foreach (DataRow selectionItem in dt4.Rows)
                        {
                            Master_CS_SELECTION selectionMdl = new Master_CS_SELECTION();
                            selectionMdl.MM_Selection_H_ID = Convert.ToInt32(selectionItem["MM_Selection_H_ID"]);
                            selectionMdl.Selection_Name = selectionItem["Selection_Name"].ToString();
                            selectionMdl.MM_Selection_D_ID = Convert.ToInt32(selectionItem["MM_Selection_D_ID"].ToString());
                            selectionMdl.Item_Name = selectionItem["Item_Name"].ToString();
                            selection.Add(selectionMdl);
                        }

                        mdl.SELECTION = selection;
                    }

                    return Ok(mdl);
                }
                else
                {
                    return BadRequest("No record found.");
                }

            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2("Api - Master Checksheet GetApproval " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                err = null;
                return BadRequest(ex.ToString());
            }
        }


        // POST: Save/Update Data into DE_CHECK_SHEET_D and DE_CHECK_SHEET_H table and DE_APPROVAL_TRANS if Approved
        [AllowAnonymous]
        [HttpPost]
        [Route("Approve_Approval")]
        public async Task<IActionResult> Approve_Approval([FromBody] Master_DE_CHECK_SHEET_DH dto)
        {
            try
            {
                var obj = dto;

                List<SqlParameter> Listparam = new List<SqlParameter>();
                Listparam.Add(new SqlParameter("@pDE_CUSTOM_CS_ID", dto.DE_CUSTOM_CS_ID));
                Listparam.Add(new SqlParameter("@pDE_CUSTOM_CS_LST_ID", dto.DE_CUSTOM_CS_LST_ID));
                Listparam.Add(new SqlParameter("@pAPPROVAL_IND", 1));
                Listparam.Add(new SqlParameter("@pUPDATED_BY", dto.UPDATED_BY));
                Listparam.Add(new SqlParameter("@pUPDATED_LOC", dto.UPDATED_LOC));
                DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_POST_MASTER_CHECKSHEET_APPROVAL", CommandType.StoredProcedure, Listparam);


                if (dt2 != null && dt2.Rows.Count > 0 && dt2.Rows[0]["Status"].ToString() == "1")
                {
                    return Ok("OK");
                }
                else
                {
                    return BadRequest("Error when saving");
                }

            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2("Api - Custom Checksheet Approve_Approval " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                err = null;
                return BadRequest(ex.ToString());
            }

        }


        // POST: Save/Update Data into DE_CHECK_SHEET_D and DE_CHECK_SHEET_H table and DE_APPROVAL_TRANS if Reject
        [AllowAnonymous]
        [HttpPost]
        [Route("Reject_Approval")]
        public async Task<IActionResult> Reject_Approval([FromBody] Master_DE_CHECK_SHEET_DH dto)
        {
            try
            {
                var obj = dto;

                List<SqlParameter> Listparam = new List<SqlParameter>();
                Listparam.Add(new SqlParameter("@pDE_CUSTOM_CS_ID", dto.DE_CUSTOM_CS_ID));
                Listparam.Add(new SqlParameter("@pDE_CUSTOM_CS_LST_ID", dto.DE_CUSTOM_CS_LST_ID));
                Listparam.Add(new SqlParameter("@pUPDATED_BY", dto.UPDATED_BY));
                Listparam.Add(new SqlParameter("@pUPDATED_LOC", dto.UPDATED_LOC));
                DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_POST_MASTER_CHECKSHEET_APPROVAL", CommandType.StoredProcedure, Listparam);


                if (dt2 != null && dt2.Rows.Count > 0 && dt2.Rows[0]["Status"].ToString() == "1")
                {
                    return Ok("OK");
                }
                else
                {
                    return BadRequest("Error when saving");
                }

            }
            catch (Exception ex)
            {
                ErrorLogSys err = new ErrorLogSys();
                await err.ErrorLog_Add_V2("Api - Custom Checksheet Reject_Approval " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex, "");
                err = null;
                return BadRequest(ex.ToString());
            }

        }


    }
}