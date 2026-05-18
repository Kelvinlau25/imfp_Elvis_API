using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using tms_acl_api.Helpers;
using tms_acl_api.Methods;
using tms_acl_api.Models;

namespace tms_acl_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/StockTake")]
    public class StockTakeController : ApiController
    {
        CommonFunction db = new CommonFunction("", ConfigurationManager.ConnectionStrings["MSSQL_PFRIMFP_ELVIS"].ConnectionString);


        #region NORMAL_CASE
        // get main screen stock take dropdown
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> GetNormal()
        {
            try
            {
                List<StockTake> model = new List<StockTake>();

                List<SqlParameter> SqlParam = new List<SqlParameter>();
                SqlParam.Add(new SqlParameter("@pTypeSelect", "dropdown"));
                DataTable dt = await db.PSP_COMMON_SQL("PSP_API_GET_STOCKTAKE_SEL", System.Data.CommandType.StoredProcedure, SqlParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        StockTake c = new StockTake();
                        c.GROUP_D_ID = Convert.ToInt32(row["GROUP_D_ID"]);
                        c.GROUP_D_NAME = row["GROUP_D_NAME"].ToString();                      
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
                return BadRequest(ex.ToString());
            }

        }


        // get data from oracle for create new
        [AllowAnonymous]
        [HttpGet]
        [Route("GetLotNo/LOTNO={pLOTNO}")]
        public async Task<IHttpActionResult> GetLotNo(string pLOTNO)
        {
            try
            {
                List<StockTake_NEW> model = new List<StockTake_NEW>();

                List<OracleParameter> OraParam = new List<OracleParameter>();
                OraParam.Add(new OracleParameter("@pLOTNO", pLOTNO));
                OraParam.Add(new OracleParameter("SREF", OracleDbType.RefCursor, ParameterDirection.Output));
                DataTable dt = await db.PSP_COMMON_ORA("PSP_API_GET_LOTDETAIL", System.Data.CommandType.StoredProcedure, OraParam, ConfigurationManager.ConnectionStrings["NEW_IMFP"].ConnectionString);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        StockTake_NEW c = new StockTake_NEW();
                        
                        c.LOT_NO = row["LOTNO"].ToString();
                        c.WIP_TYPE = row["WIP_TYPE"].ToString();
                        c.PRODUCT_TYPE = row["PRODUCTTYPE"].ToString();
                        c.STANDARD_CODE = row["PACKINGCODE"].ToString();
                        c.WIDTH = Convert.ToDecimal(row["WIDTH"]);
                        c.ACTUAL_LENGTH = Convert.ToDecimal(row["ACTUALLENGTH"]);                     
                        c.ACTUAL_WEIGHT = Convert.ToDecimal(row["ACTUALWEIGHT"]);
                       
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
                return BadRequest(ex.ToString());
            }

        }


        // POST: Save Data into DE_SCAN_STK table for button Submit
        [AllowAnonymous]
        [HttpPost]
        [Route("Save_DE_SCAN_STK")]
        public async Task<IHttpActionResult> Save_DE_SCAN_STK([FromBody]StockTake_ADD dto)
        {
            try
            {
                var obj = dto;

                //Create a new DataTable object
                DataTable dt = new DataTable();

                // Add columns to the DataTable
                dt.Columns.Add("SCAN_STK_ID");
                dt.Columns.Add("SCAN_STK_LOC");
                dt.Columns.Add("SCAN_STK_TYP");
                dt.Columns.Add("SCAN_STK_LOT_NO");
                dt.Columns.Add("SCAN_STK_PROD_TYP");
                dt.Columns.Add("SCAN_STK_STD_CODE");
                dt.Columns.Add("SCAN_STK_WIDTH");
                dt.Columns.Add("SCAN_STK_ACT_WEIGHT");
                dt.Columns.Add("SCAN_STK_ACT_LENGTH");

                // Add rows to the DataTable
                foreach (var d in dto.DATA)
                {
                    DataRow dr = dt.NewRow();
                    dr["SCAN_STK_ID"] = 0;
                    dr["SCAN_STK_LOC"] = d.SCAN_STK_LOC;
                    dr["SCAN_STK_TYP"] = d.SCAN_STK_TYP;
                    dr["SCAN_STK_LOT_NO"] = d.SCAN_STK_LOT_NO;
                    dr["SCAN_STK_PROD_TYP"] = d.SCAN_STK_PROD_TYP;
                    dr["SCAN_STK_STD_CODE"] = d.SCAN_STK_STD_CODE;
                    dr["SCAN_STK_WIDTH"] = d.SCAN_STK_WIDTH;
                    dr["SCAN_STK_ACT_WEIGHT"] = d.SCAN_STK_ACT_WEIGHT;
                    dr["SCAN_STK_ACT_LENGTH"] = d.SCAN_STK_ACT_LENGTH;
                    dt.Rows.Add(dr);
                }

                List<SqlParameter> Listparam = new List<SqlParameter>();
                Listparam.Add(new SqlParameter("@pTbl", dt));
                Listparam.Add(new SqlParameter("@pREC_TYPE", "1"));
                Listparam.Add(new SqlParameter("@pUPDATED_BY", dto.UPDATED_BY));
                Listparam.Add(new SqlParameter("@pUPDATED_LOC", dto.UPDATED_LOC));
                DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_POST_STOCKTAKE_NORMAL_MAINT", CommandType.StoredProcedure, Listparam);


                if (dt2 != null && dt2.Rows.Count > 0 && dt2.Rows[0]["Status"].ToString() == "1" && dt2.Rows[0]["Match_LotNo"].ToString() == "0")
                {
                    return Ok("OK");
                }
                if (dt2 != null && dt2.Rows.Count > 0 && dt2.Rows[0]["Status"].ToString() == "1" && dt2.Rows[0]["Match_LotNo"].ToString() == "1")
                {
                    int rowCount = Convert.ToInt32(dt2.Rows[0]["ROW_COUNT"]);
                    int duplicateCount = Convert.ToInt32(dt2.Rows[0]["DUPLICATE_COUNT"]);
                    return Ok("Error when saving, Lot No is already existed. " + rowCount + " new row(s) have been added. " + duplicateCount + " row(s) is duplicated.");
                }
                else
                {
                    return BadRequest("Error when saving");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }



        // get listing page stock take normal
        [AllowAnonymous]
        [HttpGet]
        [Route("ListingNormal")]
        public async Task<IHttpActionResult> ListingNormal()
        {
            try
            {
                List<DE_SCAN_STK_LIST> model = new List<DE_SCAN_STK_LIST>();

                List<SqlParameter> SqlParam = new List<SqlParameter>();
                SqlParam.Add(new SqlParameter("@pTypeSelect", "list"));
                DataTable dt = await db.PSP_COMMON_SQL("PSP_API_GET_STOCKTAKE_SEL", System.Data.CommandType.StoredProcedure, SqlParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DE_SCAN_STK_LIST c = new DE_SCAN_STK_LIST();
                        c.SCAN_STK_LOC = row["SCAN_STK_LOC"].ToString();
                        c.CREATED_DATE = row["CREATED_DATE"].ToString();
                        c.TOTAL_STOCK = Convert.ToInt32(row["TOTAL_STOCK"]);
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
                return BadRequest(ex.ToString());
            }

        }


        // view detail
        [AllowAnonymous]
        [HttpGet]
        [Route("GetDetailNormal/DATE={pDATE}")]
        public async Task<IHttpActionResult> GetDetailNormal(string pDATE)
        {
            try
            {
                List<DE_SCAN_STK_D> model = new List<DE_SCAN_STK_D>();

               string date = pDATE.Substring(6, 4) + "-" + pDATE.Substring(3, 2) + "-" + pDATE.Substring(0, 2);

                List<SqlParameter> SqlParam = new List<SqlParameter>();
                SqlParam.Add(new SqlParameter("@pDATE", date));
                SqlParam.Add(new SqlParameter("@pTypeSelect", "detail"));
                DataTable dt = await db.PSP_COMMON_SQL("PSP_API_GET_STOCKTAKE_SEL", System.Data.CommandType.StoredProcedure, SqlParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DE_SCAN_STK_D c = new DE_SCAN_STK_D();

                        c.SCAN_STK_ID = Convert.ToInt32(row["SCAN_STK_ID"]);
                        c.SCAN_STK_LOC = row["SCAN_STK_LOC"].ToString();
                        c.SCAN_STK_TYP = row["SCAN_STK_TYP"].ToString();
                        c.SCAN_STK_LOT_NO = row["SCAN_STK_LOT_NO"].ToString();
                        c.SCAN_STK_PROD_TYP = row["SCAN_STK_PROD_TYP"].ToString();
                        c.SCAN_STK_STD_CODE = row["SCAN_STK_STD_CODE"].ToString();
                        c.SCAN_STK_WIDTH = Convert.ToDecimal(row["SCAN_STK_WIDTH"]);
                        c.SCAN_STK_ACT_WEIGHT = Convert.ToDecimal(row["SCAN_STK_ACT_WEIGHT"]);
                        c.SCAN_STK_ACT_LENGTH = Convert.ToDecimal(row["SCAN_STK_ACT_LENGTH"]);

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
                return BadRequest(ex.ToString());
            }

        }

        // POST: Save Data into DE_SCAN_STK table for button Delete on listing page
        [AllowAnonymous]
        [HttpPost]
        [Route("Delete_DE_SCAN_STK")]
        public async Task<IHttpActionResult> Delete_DE_SCAN_STK([FromBody]StockTake_ADD dto)
        {
            try
            {
                var obj = dto;

                //Create a new DataTable object
                DataTable dt = new DataTable();

                // Add columns to the DataTable
                dt.Columns.Add("SCAN_STK_ID");
                dt.Columns.Add("SCAN_STK_LOC");
                dt.Columns.Add("SCAN_STK_TYP");
                dt.Columns.Add("SCAN_STK_LOT_NO");
                dt.Columns.Add("SCAN_STK_PROD_TYP");
                dt.Columns.Add("SCAN_STK_STD_CODE");
                dt.Columns.Add("SCAN_STK_WIDTH");
                dt.Columns.Add("SCAN_STK_ACT_WEIGHT");
                dt.Columns.Add("SCAN_STK_ACT_LENGTH");
                dt.Columns.Add("CREATED_DATE");

                // Add rows to the DataTable
                foreach (var d in dto.DATA)
                {
                    DataRow dr = dt.NewRow();
                    dr["SCAN_STK_ID"] = "";
                    dr["SCAN_STK_LOC"] = d.SCAN_STK_LOC;
                    dr["SCAN_STK_TYP"] = "";
                    dr["SCAN_STK_LOT_NO"] = "";
                    dr["SCAN_STK_PROD_TYP"] = "";
                    dr["SCAN_STK_STD_CODE"] = "";
                    dr["SCAN_STK_WIDTH"] = "";
                    dr["SCAN_STK_ACT_WEIGHT"] = "";
                    dr["SCAN_STK_ACT_LENGTH"] = "";
                    dr["CREATED_DATE"] = d.CREATED_DATE;
                    dt.Rows.Add(dr);
                }

                List<SqlParameter> Listparam = new List<SqlParameter>();
                Listparam.Add(new SqlParameter("@pTbl", dt));
                Listparam.Add(new SqlParameter("@pREC_TYPE", "5"));
                Listparam.Add(new SqlParameter("@pUPDATED_BY", dto.UPDATED_BY));
                Listparam.Add(new SqlParameter("@pUPDATED_LOC", dto.UPDATED_LOC));
                DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_POST_STOCKTAKE_NORMAL_MAINT", CommandType.StoredProcedure, Listparam);

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
                return BadRequest(ex.ToString());
            }

        }

        #endregion


        #region EXCEPTIONAL_CASE

        // get main screen stock take dropdown
        [AllowAnonymous]
        [HttpGet]
        [Route("GetExceptional")]
        public async Task<IHttpActionResult> GetExceptional()
        {
            try
            {
                List<StockTake_Exceptional_DDL> model = new List<StockTake_Exceptional_DDL>();

                List<SqlParameter> SqlParam = new List<SqlParameter>();
                SqlParam.Add(new SqlParameter("@pTypeSelect", "dropdown"));
                DataTable dt = await db.PSP_COMMON_SQL("PSP_API_GET_STOCKTAKE_SEL", System.Data.CommandType.StoredProcedure, SqlParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        StockTake_Exceptional_DDL c = new StockTake_Exceptional_DDL();
                        c.GROUP_D_ID = Convert.ToInt32(row["GROUP_D_ID"]);
                        c.GROUP_D_NAME = row["GROUP_D_NAME"].ToString();
                        model.Add(c);
                    }

                }
                else
                {
                    return BadRequest("No record found.");
                }

                List<StockTake_Exceptional_DDL> m = new List<StockTake_Exceptional_DDL>();

                List<SqlParameter> SqlParam2 = new List<SqlParameter>();
                SqlParam2.Add(new SqlParameter("@pTypeSelect", "prodname"));
                DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_GET_STOCKTAKE_SEL", System.Data.CommandType.StoredProcedure, SqlParam2);

                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    foreach (DataRow row in dt2.Rows)
                    {
                        StockTake_Exceptional_DDL c = new StockTake_Exceptional_DDL();
                        c.GROUP_D_ID = Convert.ToInt32(row["GROUP_D_ID"]);
                        c.GROUP_D_NAME = row["GROUP_D_NAME"].ToString();
                        m.Add(c);
                    }

                }
                else
                {
                    return BadRequest("No record found.");
                }

                List<StockTake_Exceptional_DDL> mdl = new List<StockTake_Exceptional_DDL>();

                List<SqlParameter> SqlParam3 = new List<SqlParameter>();
                SqlParam3.Add(new SqlParameter("@pTypeSelect", "uom"));
                DataTable dt3 = await db.PSP_COMMON_SQL("PSP_API_GET_STOCKTAKE_SEL", System.Data.CommandType.StoredProcedure, SqlParam3);

                if (dt3 != null && dt3.Rows.Count > 0)
                {
                    foreach (DataRow row in dt3.Rows)
                    {
                        StockTake_Exceptional_DDL c = new StockTake_Exceptional_DDL();
                        c.GROUP_D_ID = Convert.ToInt32(row["GROUP_D_ID"]);
                        c.GROUP_D_NAME = row["GROUP_D_NAME"].ToString();
                        mdl.Add(c);
                    }

                }
                else
                {
                    return BadRequest("No record found.");
                }

                StockTake_Exceptional stockTakeExceptionalModel = new StockTake_Exceptional
                {
                    LOCATION = model,
                    PRODUCT_NAME = m,
                    UNIT_NAME = mdl
                };

                return Ok(stockTakeExceptionalModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }


        // get listing page stock take normal
        [AllowAnonymous]
        [HttpGet]
        [Route("ListingExceptional")]
        public async Task<IHttpActionResult> ListingExceptional()
        {
            try
            {
                List<DE_EXCEPTIONAL_STK_D> model = new List<DE_EXCEPTIONAL_STK_D>();

                List<SqlParameter> SqlParam = new List<SqlParameter>();
                SqlParam.Add(new SqlParameter("@pTypeSelect", "detailExceptional"));
                DataTable dt = await db.PSP_COMMON_SQL("PSP_API_GET_STOCKTAKE_SEL", System.Data.CommandType.StoredProcedure, SqlParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DE_EXCEPTIONAL_STK_D c = new DE_EXCEPTIONAL_STK_D();
                        c.EXCEPTIONAL_STK_ID = Convert.ToInt32(row["EXCEPTIONAL_STK_ID"]);
                        c.EXCEPTIONAL_STK_LOC = row["EXCEPTIONAL_STK_LOC"].ToString();
                        c.EXCEPTIONAL_DATE = row["EXCEPTIONAL_DATE"].ToString();
                        c.PROD_NAME = row["PROD_NAME"].ToString();
                        c.EXCEPTIONAL_STK_QTY = Convert.ToInt32(row["EXCEPTIONAL_STK_QTY"]);
                        c.UOM = row["UOM"].ToString();
                        c.CREATED_DATE = row["CREATED_DATE"].ToString();
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
                return BadRequest(ex.ToString());
            }

        }

        // view detail
        [AllowAnonymous]
        [HttpGet]
        [Route("GetDetailExceptional/ID={pID}")]
        public async Task<IHttpActionResult> GetDetailExceptional(int pID)
        {
            try
            {
                List<DE_EXCEPTIONAL_STK_D> model = new List<DE_EXCEPTIONAL_STK_D>();

                List<SqlParameter> SqlParam = new List<SqlParameter>();
                SqlParam.Add(new SqlParameter("@pID", pID));
                SqlParam.Add(new SqlParameter("@pTypeSelect", "detailExceptional"));
                DataTable dt = await db.PSP_COMMON_SQL("PSP_API_GET_STOCKTAKE_SEL", System.Data.CommandType.StoredProcedure, SqlParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DE_EXCEPTIONAL_STK_D c = new DE_EXCEPTIONAL_STK_D();
                        c.EXCEPTIONAL_STK_ID = Convert.ToInt32(row["EXCEPTIONAL_STK_ID"]);
                        c.EXCEPTIONAL_STK_LOC = row["EXCEPTIONAL_STK_LOC"].ToString();
                        c.EXCEPTIONAL_DATE = row["EXCEPTIONAL_DATE"].ToString();
                        c.PROD_NAME = row["PROD_NAME"].ToString();
                        c.EXCEPTIONAL_STK_QTY = Convert.ToInt32(row["EXCEPTIONAL_STK_QTY"]);
                        c.UOM = row["UOM"].ToString();
                        c.CREATED_DATE = row["CREATED_DATE"].ToString();
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
                return BadRequest(ex.ToString());
            }

        }


        // POST: Save/Update Data into DE_EXCEPTIONAL_STK table for button Submit
        [AllowAnonymous]
        [HttpPost]
        [Route("Save_DE_EXCEPTIONAL_STK")]
        public async Task<IHttpActionResult> Save_DE_EXCEPTIONAL_STK([FromBody]DE_EXCEPTIONAL_STK_D dto)
        {
            try
            {
                var obj = dto;

                //Create a new DataTable object
                DataTable dt = new DataTable();

                // Add columns to the DataTable
                dt.Columns.Add("EXCEPTIONAL_STK_ID");
                dt.Columns.Add("EXCEPTIONAL_STK_LOC");
                dt.Columns.Add("EXCEPTIONAL_DATE");
                dt.Columns.Add("PROD_NAME");
                dt.Columns.Add("EXCEPTIONAL_STK_QTY");
                dt.Columns.Add("UOM");

                string date = dto.EXCEPTIONAL_DATE.Substring(6, 4) + "-" + dto.EXCEPTIONAL_DATE.Substring(3, 2) + "-" + dto.EXCEPTIONAL_DATE.Substring(0, 2);

                DataRow dr = dt.NewRow();

                dr["EXCEPTIONAL_STK_ID"] = dto.EXCEPTIONAL_STK_ID;
                dr["EXCEPTIONAL_STK_LOC"] = dto.EXCEPTIONAL_STK_LOC;
                dr["EXCEPTIONAL_DATE"] = date;
                dr["PROD_NAME"] = dto.PROD_NAME;
                dr["EXCEPTIONAL_STK_QTY"] = dto.EXCEPTIONAL_STK_QTY;
                dr["UOM"] = dto.UOM;
                dt.Rows.Add(dr);

                List<SqlParameter> Listparam = new List<SqlParameter>();
                Listparam.Add(new SqlParameter("@pTbl", dt));
                Listparam.Add(new SqlParameter("@pREC_TYPE", "1"));
                Listparam.Add(new SqlParameter("@pUPDATED_BY", dto.UPDATED_BY));
                Listparam.Add(new SqlParameter("@pUPDATED_LOC", dto.UPDATED_LOC));
                DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_POST_STOCKTAKE_EXCEPTIONAL_MAINT", CommandType.StoredProcedure, Listparam);

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
                return BadRequest(ex.ToString());
            }

        }


        // POST: Save Data into DE_EXCEPTIONAL_STK table for button Delete on listing page
        [AllowAnonymous]
        [HttpPost]
        [Route("Delete_DE_EXCEPTIONAL_STK")]
        public async Task<IHttpActionResult> Delete_DE_EXCEPTIONAL_STK([FromBody]DE_EXCEPTIONAL_STK_D dto)
        {
            try
            {
                var obj = dto;

                //Create a new DataTable object
                DataTable dt = new DataTable();

                // Add columns to the DataTable
                dt.Columns.Add("EXCEPTIONAL_STK_ID");
                dt.Columns.Add("EXCEPTIONAL_STK_LOC");
                dt.Columns.Add("EXCEPTIONAL_DATE");
                dt.Columns.Add("PROD_NAME");
                dt.Columns.Add("EXCEPTIONAL_STK_QTY");
                dt.Columns.Add("UOM");

                DataRow dr = dt.NewRow();
                dr["EXCEPTIONAL_STK_ID"] = dto.EXCEPTIONAL_STK_ID;
                dr["EXCEPTIONAL_STK_LOC"] = "";
                dr["EXCEPTIONAL_DATE"] = "";
                dr["PROD_NAME"] = "";
                dr["EXCEPTIONAL_STK_QTY"] = "";
                dr["UOM"] = "";
                dt.Rows.Add(dr);

                List<SqlParameter> Listparam = new List<SqlParameter>();
                Listparam.Add(new SqlParameter("@pTbl", dt));
                Listparam.Add(new SqlParameter("@pREC_TYPE", "5"));
                Listparam.Add(new SqlParameter("@pUPDATED_BY", dto.UPDATED_BY));
                Listparam.Add(new SqlParameter("@pUPDATED_LOC", dto.UPDATED_LOC));
                DataTable dt2 = await db.PSP_COMMON_SQL("PSP_API_POST_STOCKTAKE_EXCEPTIONAL_MAINT", CommandType.StoredProcedure, Listparam);


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
                return BadRequest(ex.ToString());
            }

        }

        #endregion
    }
}