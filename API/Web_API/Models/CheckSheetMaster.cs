using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tms_acl_api.Models
{
    //for dropdown main page
    public class CheckSheetMaster
    {
        public int MODULE_H_ID { get; set; }
        public string MODULE_DISPLAY_NAME { get; set; }
        public int MM_MasterCS_H_ID { get; set; }
        public string TITLE { get; set; }
        public int GROUP_H_ID { get; set; }
        public string GROUP_DISPLAY_NAME { get; set; }
        public List<Master_Checksheet_D> LIST_GROUP_D { get; set; }

    }

    //for dropdown main page
    public class Master_Checksheet_D {
        public int GROUP_D_ID { get; set; }
        public string GROUP_D_NAME { get; set; }
    }

    public class DE_CHECK_SHEET_MASTER_LST
    {
        public int MODULE_H_ID { get; set; }
        public int MM_MasterCS_H_ID { get; set; }
        public List<DE_CHECK_SHEET_MASTER_LST_D> GROUP_H_DATA { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_LOC { get; set; }
    }

    //after select dropdown in main page (save in table DE_CHECK_SHEET_LST)
    public class DE_CHECK_SHEET_MASTER_LST_D
    {
        public int GROUP_H_ID { get; set; }
        public int GROUP_D_ID { get; set; }
    }


    public class Master_TABLE_HEADER
    {
        public int TotalRow { get; set; }
        public int TotalColumn { get; set; }
        public int REMARK_IND { get; set; }
        public string ISONo { get; set; }
        public string RevNo { get; set; }
        public string RevDate { get; set; }
        public List<Master_ModelHeader> MODEL_HEADER { get; set; }
        //public List<Master_FixTable> MODEL_FIX_TBL { get; set; }
        //public List<Master_RptTable> MODEL_RPT_TBL { get; set; }
        //public List<ModelColumn> MODEL_COLUMN { get; set; }
        //public List<ModelRow> MODEL_ROW { get; set; }
        public List<GroupedFixTable> GROUP_FIX { get; set; }
        public List<GroupedRptTable> GROUP_RPT { get; set; }
        public List<GroupedRptCol> GROUP_RPT_COL { get; set; }
        public List<Master_CS_SELECTION> SELECTION { get; set; }
        public string REMARK { get; set; }
    }


    public class GroupedFixTable
    {
        public int MM_CSTable_ID { get; set; }
        public List<Master_FixTable> Data { get; set; }
    }

    public class GroupedRptTable
    {
        public int MM_CSTable_ID { get; set; }
        public int ITEM_NO { get; set; }
        public List<Master_RptTable> Data { get; set; }
    }

    public class GroupedRptCol
    {
        public int MM_CSTable_ID { get; set; }
        public int ITEM_NO { get; set; }
        public List<Master_RptCol> Data { get; set; }
    }

    public class Master_ModelHeader
    {
        public int MM_MasterCS_D_ID { get; set; }
        public string Label_Text { get; set; }
        public int MM_FieldType_H_ID { get; set; }
        public int Label_Text_D_ID { get; set; }
        public string ITEM_NULLABLE { get; set; }
        public string Text_Length { get; set; }
        public string Int_Max { get; set; }
        public string Int_Min { get; set; }
        public string Int_Tolerance { get; set; }
        public string RESULTS { get; set; }
        public int RowNo { get; set; }
        public int ColNo { get; set; }
        public int DE_CUSTOM_CS_H_ID { get; set; }

    }

    public class Master_FixTable
    {
        public int MM_CSTable_ID { get; set; }
        public string Table_Name { get; set; }
        public int Table_Fix_Column { get; set; }
        public int MM_FTable_ID { get; set; }
        public string FTable_Item { get; set; }
        public int RowNo { get; set; }
        public int ColNo { get; set; }

    }

    public class Master_RptCol
    {
        public int MM_CSTable_ID { get; set; }
        public string Table_Name { get; set; }
        public int Table_Column { get; set; }
        public int Table_Fix_Column { get; set; }
        public int MM_ColTitle_ID { get; set; }
        public string ColTitle_Item { get; set; }
        public int RowNo { get; set; }
        public int ColNo { get; set; }

    }

    public class Master_RptTable
    {
        public int MM_CSTable_ID { get; set; }
        public string Table_Name { get; set; }
        public int Table_Column { get; set; }
        public int MM_DTable_ID { get; set; }
        public string DTable_Item { get; set; }
        public int MM_FieldType_H_ID { get; set; }
        public int DTable_Item_D_ID { get; set; }
        public string ITEM_NULLABLE { get; set; }
        public string Text_Length { get; set; }
        public string Int_Max { get; set; }
        public string Int_Min { get; set; }
        public string Int_Tolerance { get; set; }
        public string ITEM_RESULT { get; set; }
        public int CS_CUSTOM_ID { get; set; }
        public int DE_CUSTOM_CS_D_ID { get; set; }
        public int RowNo { get; set; }
        public int ITEM_NO { get; set; }

    }


    public class Master_HEADER
    {
        public int DE_CHECK_SHEET_H_ID { get; set; }
        public int DE_CHECK_SHEET_LST_ID { get; set; }
        public int EMPLOYEE_NO { get; set; }
        public string SHIFT { get; set; }
        public string STATUS { get; set; }
        public DateTime TRANS_DATETIME { get; set; }
        public string REMARK { get; set; }
        public int APPROVAL_IND { get; set; }
        public int TotalRow { get; set; }
        public int TotalColumn { get; set; }
        public List<ModelColumn> MODEL_COLUMN { get; set; }
        public List<ModelRowTbl> MODEL_ROW { get; set; }
    }

    public class ModelColumn
    {
        public int indexCol { get; set; }
        public string columnTitle { get; set; }
    }

    public class ModelRowTbl
    {
        public string columnTitle { get; set; }
        public string Value { get; set; }
        public int CS_SETUP_D_ID { get; set; }
        public string ITEM_TYPE { get; set; }
        public string ITEM_NULLABLE { get; set; }
        public string ITEM_WRITTABLE { get; set; }
        public int DE_CHECK_SHEET_D_ID { get; set; }
        public string ITEM_RESULT { get; set; }
        public int indexCol { get; set; }
        public int indexRow { get; set; }
    }

    public class Master_ModelColumn
    {
        public int indexCol { get; set; }
        public string columnTitle { get; set; }
    }

    public class Master_ModelRow
    {
        public string columnTitle { get; set; }
        public string Value { get; set; }
        public int CS_SETUP_D_ID { get; set; }
        public string ITEM_TYPE { get; set; }
        public string ITEM_NULLABLE { get; set; }
        public string ITEM_WRITTABLE { get; set; }
        public string ATTACHMENT_IND { get; set; }
        public int indexCol { get; set; }
        public int indexRow { get; set; }
    }

    public class Master_ModelRowTbl
    {
        public string columnTitle { get; set; }
        public string Value { get; set; }
        public int CS_SETUP_D_ID { get; set; }
        public string ITEM_TYPE { get; set; }
        public string ITEM_NULLABLE { get; set; }
        public string ITEM_WRITTABLE { get; set; }
        public int DE_CHECK_SHEET_D_ID { get; set; }
        public string ITEM_RESULT { get; set; }
        public int indexCol { get; set; }
        public int indexRow { get; set; }
    }


    //to get listing screen & to add/update table DE_CHECK_SHEET_H
    public class Master_DE_CHECK_SHEET_DH
    {
        public int MM_MasterCS_H_ID { get; set; }
        public int DE_CUSTOM_CS_H_ID { get; set; }
        public int DE_CUSTOM_CS_ID { get; set; }
        public int DE_CUSTOM_CS_LST_ID { get; set; }
        public int EMPLOYEE_NO { get; set; }
        public string TRANS_DATETIME { get; set; }
        public string SHIFT { get; set; }
        public string STATUS { get; set; }
        public string APPROVAL_EMPLOYEE_NO { get; set; }
        public string APPROVAL_EMPLOYEE_NAME { get; set; }
        public string REMARK { get; set; }
        public string ATTACHMENT { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_LOC { get; set; }
        public string RECORD_TYP { get; set; }
        public string ACTION_IND { get; set; }
        public string ITEM_NO { get; set; }
        public Image SKETCH { get; set; }
        public List<SAVE_MASTER_DE_CHECK_SHEET_H> DATA_HEADER { get; set; }
        public List<SAVE_MASTER_DE_CHECK_SHEET_DH> DATA { get; set; }
        public string COL1 { get; set; }
        public string COL2 { get; set; }
        public string COL3 { get; set; }
        public string COL4 { get; set; }
        public string COL5 { get; set; }
        public string COL1_VALUE { get; set; }
        public string COL2_VALUE { get; set; }
        public string COL3_VALUE { get; set; }
        public string COL4_VALUE { get; set; }
        public string COL5_VALUE { get; set; }
        public string ISONo { get; set; }
    }

    //to add/update table DE_CHECK_SHEET_H
    public class SAVE_MASTER_DE_CHECK_SHEET_H
    {
        public int DE_CUSTOM_CS_H_ID { get; set; }
        public int MM_MasterCS_D_ID { get; set; }
        public string ITEM_NO { get; set; }
        public string RESULTS { get; set; }
    }

    //to add/update table DE_CHECK_SHEET_D
    public class SAVE_MASTER_DE_CHECK_SHEET_DH
    {
        public int DE_CUSTOM_CS_D_ID { get; set; }
        public int CS_CUSTOM_ID { get; set; }
        public string ITEM_SKETCH { get; set; }
        public string ITEM_RESULT { get; set; }
        public string ITEM_NO { get; set; }
        public Image ATTACHMENT { get; set; }
        public Image SKETCH { get; set; }
    }

    public class Image
    {
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Master_CS_SELECTION
    {
        public int MM_Selection_H_ID { get; set; }
        public string Selection_Name { get; set; }
        public int MM_Selection_D_ID { get; set; }
        public string Item_Name { get; set; }
    }
}