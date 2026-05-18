using System;
using System.Collections.Generic;

namespace tms_acl_api.Models
{
    //for dropdown main page
    public class StockTake
    {
        public int GROUP_D_ID { get; set; }
        public string GROUP_D_NAME { get; set; }
    }

    //after select dropdown in main page (save in table DE_CHECK_SHEET_LST)
    public class StockTake_NEW
    {
        public string LOT_NO { get; set; }
        public string WIP_TYPE { get; set; }
        public string PRODUCT_TYPE { get; set; }
        public string STANDARD_CODE { get; set; }
        public decimal WIDTH { get; set; }
        public decimal ACTUAL_LENGTH { get; set; }
        public decimal ACTUAL_WEIGHT { get; set; }
    }

    public class StockTake_ADD
    {
        public int GROUP_D_ID { get; set; }
        public string GROUP_D_NAME { get; set; }
        public List<DE_SCAN_STK_D> DATA { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_LOC { get; set; }

    }

    //to get listing screen & to add/update table DE_CHECK_SHEET_H
    public class DE_SCAN_STK_D
    {
        public int SCAN_STK_ID { get; set; }
        public string SCAN_STK_LOC { get; set; }
        public string SCAN_STK_TYP { get; set; }
        public string SCAN_STK_LOT_NO { get; set; }
        public string SCAN_STK_PROD_TYP { get; set; }
        public string SCAN_STK_STD_CODE { get; set; }
        public decimal SCAN_STK_WIDTH { get; set; }
        public decimal SCAN_STK_ACT_WEIGHT { get; set; }
        public decimal SCAN_STK_ACT_LENGTH { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_LOC { get; set; }
        public string CREATED_DATE { get; set; }
    }

    //to get listing screen 
    public class DE_SCAN_STK_LIST
    {
        public int SCAN_STK_ID { get; set; }
        public string SCAN_STK_LOC { get; set; }
        public string CREATED_DATE { get; set; }
        public int TOTAL_STOCK { get; set; }
    }

    // Exceptional Case
    public class StockTake_Exceptional
    {
        public List<StockTake_Exceptional_DDL> LOCATION { get; set; }
        public List<StockTake_Exceptional_DDL> PRODUCT_NAME { get; set; }
        public List<StockTake_Exceptional_DDL> UNIT_NAME { get; set; }
    }

    public class StockTake_Exceptional_DDL
    {
        public int GROUP_D_ID { get; set; }
        public string GROUP_D_NAME { get; set; }
    }


    public class StockTakeExceptional_ADD
    {
        public int GROUP_D_ID { get; set; }
        public string GROUP_D_NAME { get; set; }
        public List<DE_EXCEPTIONAL_STK_D> DATA { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_LOC { get; set; }

    }

    //to get listing screen & to add/update table DE_CHECK_SHEET_H
    public class DE_EXCEPTIONAL_STK_D
    {
        public int EXCEPTIONAL_STK_ID { get; set; }
        public string EXCEPTIONAL_STK_LOC { get; set; }
        public string EXCEPTIONAL_DATE { get; set; }
        public string PROD_NAME { get; set; }
        public int EXCEPTIONAL_STK_QTY { get; set; }
        public string UOM { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_LOC { get; set; }
        public string CREATED_DATE { get; set; }
    }

    public class MESSAGE_RESPONSE
    {
        public string Message { get; set; }
        public int insertRow { get; set; }
        public int duplicateRow { get; set; }
    }

}