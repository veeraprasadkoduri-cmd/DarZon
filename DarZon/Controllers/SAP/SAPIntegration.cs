using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAPbobsCOM;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using DarZon.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace DarZon
{
    public class SAPIntegration
    {
        CultureInfo objDATE = new CultureInfo("en-US");
        public static SAPbobsCOM.Payments oIncomingPayments; // Payments object
        public static SAPbobsCOM.Payments ovendorPayments; // Payments object
        public static SAPbobsCOM.Documents oCreditMemo; // Invoice Object
        public static SAPbobsCOM.Documents oDelivery; // Delivery Object
        public static SAPbobsCOM.Recordset oRecordSet; // A recordset object
        public static SAPbobsCOM.Company oCompany; // The company object
        public static SAPbobsCOM.Documents oOrder;
        //public static SAPbobsCOM.BusinessPartners oBP;
        public static SAPbobsCOM.Documents oInvoice; // Invoice Object
        public static SAPbobsCOM.StockTransfer StockTransfer; //StockTransfer
        public static SAPbobsCOM.SalesPersons oSalesPerson;
        public static SAPbobsCOM.Items oIT;
        public static SAPbobsCOM.Documents oAPI;
        public static SAPbobsCOM.Payments oPayments; // Invoice Object
        public static SAPbobsCOM.Documents oGRPO;
        public static SAPbobsCOM.JournalEntries JE;
        public static SAPbobsCOM.JournalVouchers JV;
        public static SAPbobsCOM.ProductTrees BOM;
        public static SAPbobsCOM.Contacts oActivity;
        public static SAPbobsCOM.Documents oSaleorder;

        bool dbAvail = false;
        public string sErrMsg;
        public int lErrCode;
        public int lRetCode;
        public string querys;
        public string servername, DBname, Username, DocumentType, EndofRecord;
        string DBType = "", DBserver = "", DBUname = "", DBPwd = "", Sub = "", SAPUSER = "", SAPPWD = "", COMPANYDB = "";
        string V_Transaction_Type = "", V_Transaction_No = "", V_Error_Code = "", V_DML = "", V_Error_Msg = "", V_Remarks = "", V_Dummy1 = "";
        DateTime V_Date_Time;
        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]);
        SqlDataAdapter sqlDA = new SqlDataAdapter();
        SqlDataAdapter sqlDA1 = new SqlDataAdapter();
        SqlDataAdapter sqlDA2 = new SqlDataAdapter();
        DataSet ds = new DataSet();
        string query = "";
        int RetSts = 0;


        public string SAPConnect()
        {
            DBType = System.Configuration.ConfigurationManager.AppSettings["DBType"];
            DBserver = System.Configuration.ConfigurationManager.AppSettings["DBServer"];
            DBUname = System.Configuration.ConfigurationManager.AppSettings["DBUname"];
            DBPwd = System.Configuration.ConfigurationManager.AppSettings["DBPwd"];


            try
            {
                oCompany = new SAPbobsCOM.Company();
                //oRecordSet = new SAPbobsCOM.Recordset();

                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2017 ;
                oCompany.Server = DBserver; // change to your company server                
                oCompany.UseTrusted = false;
                oCompany.DbUserName = DBUname;
                oCompany.DbPassword = DBPwd;

                try
                {
                    oRecordSet = oCompany.GetCompanyList(); // get the company list
                }

                catch (Exception ex)
                {

                    //return 1;
                    return ex.Message;
                }

                int temp_int = lErrCode;
                string temp_string = sErrMsg;
                oCompany.GetLastError(out temp_int, out temp_string);

                if (lErrCode != 0)
                {

                }
                else
                {
                    if (!(oRecordSet.EoF == true))
                    {
                        dbAvail = true;
                    }
                }
                if (!dbAvail)
                {
                    //Logger.LogInfo("There was no Database Found...");
                    //return 1;
                    return "Connection Failed!";
                }
                if (oCompany.Connected) // if already connected
                {
                }
                return (Connect());
            }
            catch (Exception e1)
            {

                //return 1;
                return e1.Message;
            }
        }
        public string Connect()
        {
            // Set connection properties
            oCompany.CompanyDB = System.Configuration.ConfigurationManager.AppSettings["SAPDBName"];
            oCompany.UserName = System.Configuration.ConfigurationManager.AppSettings["SAPUSERUname"];
            oCompany.Password = System.Configuration.ConfigurationManager.AppSettings["SAPPwd"];

            lRetCode = oCompany.Connect();
            if (lRetCode != 0) // if the connection failed
            {
                oCompany.GetLastError(out lErrCode, out sErrMsg);
                //MessageBox.Show("Connection Failed - " + sErrMsg);1message
                //Logger.LogInfo("Connection Failed - " + sErrMsg);
                //                Konnect = false;
            }
            if (oCompany.Connected) // if connected
            {
                //MessageBox.Show("Connected to - " + Program.oCompany.CompanyDB);
                //Logger.LogInfo("Connected to - " + Program.oCompany.CompanyDB);1message
            }

            if (oCompany.Connected)
            {
                //Konnect = true;
                return "Connected";


            }
            else
            {
                //                Logger.LogInfo("Database Not connected.. Try Again");1message
                //              Konnect = false;
                return sErrMsg;
                //return 1;
            }

        }
        public string Customers(CustomerMaster objcus)
        {

            bool ELupFlg = false;
            bool a = true;
            bool b = true;
            //string upflg = "";
            String CardCd = "", Dml = "", bpcc = "";
            Int32 RecCnt = 0, LineNum = 0;
            string sapmeg = "";
            DataSet dsCUST = new DataSet();
            try
            {
                DateTime dtTmp;
                Boolean check = true;
                V_Transaction_Type = "Customer";
                conn.Open();
                //if (Connect() == 0)
                sapmeg = SAPConnect();
                if (sapmeg == "Connected")
                {
                    SAPbobsCOM.BusinessPartners oBP;
                    query = "select CardCode from [dbo].[OCRD] where Cellular='" + objcus.PhoneNumber + "'";
                    sqlDA = new SqlDataAdapter(query, conn);
                    sqlDA.Fill(dsCUST, "Cust");
                    oBP = ((SAPbobsCOM.BusinessPartners)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)));
                    if (dsCUST.Tables[0].Rows.Count > 0)
                    {
                        a = true;
                        //oBP.CardCode = dsCUST.Tables[0].Rows[0]["CardCode"].ToString();
                        bpcc = dsCUST.Tables[0].Rows[0]["CardCode"].ToString();
                    }
                    else
                    {
                        a = false;
                    }


                    if (a == false)
                    {
                        oBP.Series = 84;
                        oBP.CardName = objcus.CustomerName;
                        oBP.AliasName = objcus.CustTYpe;
                        oBP.CardType = BoCardTypes.cCustomer;
                        oBP.GroupCode = 100;
                        //oBP.Phone1 = Convert.ToString(dsCUST.Tables[0].Rows[0]["Tele_O"]);
                        oBP.Phone2 = objcus.AltrphoneNo;
                        oBP.SubjectToWithholdingTax = BoYesNoEnum.tNO;
                        oBP.EmailAddress = objcus.Emailid;
                        oBP.Cellular = objcus.PhoneNumber;

                        var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");

                        if ((objcus.Anniversery == null) || (objcus.Anniversery.ToString() == ""))
                        {

                        }
                        else
                        {
                            DateTime DT1 = new DateTime();
                            DT1 = DateTime.ParseExact(objcus.Anniversery, "dd/MM/yyyy", dutchCulture);
                            oBP.UserFields.Fields.Item("U_DOA").Value = DT1;
                        }

                        if ((objcus.DBO == null) || (objcus.DBO.ToString() == ""))
                        {

                        }
                        else
                        {
                            DateTime DT2 = new DateTime();
                            DT2 = DateTime.ParseExact(objcus.DBO, "dd/MM/yyyy", dutchCulture);
                            oBP.UserFields.Fields.Item("U_DOB").Value = DT2;
                        }

                        oBP.Addresses.Street = objcus.Street;
                        oBP.Addresses.BuildingFloorRoom = objcus.Area;
                        //  oBP.Addresses.ar

                        oBP.Addresses.Block = objcus.Block;
                        oBP.Addresses.ZipCode = objcus.Pincode;
                        oBP.Addresses.City = objcus.city;
                        oBP.Addresses.State = objcus.State;
                        oBP.Addresses.AddressName3 = objcus.DoorNo;
                        oBP.Addresses.AddressName2 = objcus.Landmark;
                        oBP.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo;
                        //if (a == true)
                        //    oBP.CardCode = dsCUST.Tables[0].Rows[0]["CardCode"].ToString();
                        oBP.Addresses.AddressName = objcus.CustomerName;
                        oBP.Addresses.Add();





                        oBP.Addresses.Street = objcus.Street;
                        oBP.Addresses.Block = objcus.Block;
                        oBP.Addresses.ZipCode = objcus.Pincode;
                        oBP.Addresses.City = objcus.city;
                        oBP.Addresses.State = objcus.State;
                        oBP.Addresses.AddressName3 = objcus.DoorNo;
                        oBP.Addresses.AddressName2 = objcus.Landmark;
                        oBP.Addresses.BuildingFloorRoom = objcus.Area;
                        oBP.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                        
                        
                        //if (a == true)
                        //    oBP.CardCode = dsCUST.Tables[0].Rows[0]["CardCode"].ToString();
                        oBP.Addresses.AddressName = objcus.CustomerName;
                        oBP.Addresses.Add();
                    }
                    if (a == true)
                    {
                        b = oBP.GetByKey(bpcc);

                        if (b == true)
                        {
                            //oBP.Series = 1;
                            oBP.CardName = objcus.CustomerName;
                            //oBP.AliasName = objcus.CustTYpe;
                            //oBP.CardType = BoCardTypes.cCustomer;
                            //oBP.GroupCode = 100;
                            //oBP.Phone1 = Convert.ToString(dsCUST.Tables[0].Rows[0]["Tele_O"]);
                            oBP.Phone2 = objcus.AltrphoneNo;
                            //oBP.SubjectToWithholdingTax = BoYesNoEnum.tNO;
                            oBP.EmailAddress = objcus.Emailid;
                            oBP.Cellular = objcus.PhoneNumber;

                            var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");

                            if ((objcus.Anniversery == null) || (objcus.Anniversery.ToString() == ""))
                            {

                            }
                            else
                            {
                                DateTime DT1 = new DateTime();
                                DT1 = DateTime.ParseExact(objcus.Anniversery, "dd/MM/yyyy", dutchCulture);
                                oBP.UserFields.Fields.Item("U_DOA").Value = DT1;
                            }

                            if ((objcus.DBO == null) || (objcus.DBO.ToString() == ""))
                            {

                            }
                            else
                            {
                                DateTime DT2 = new DateTime();
                                DT2 = DateTime.ParseExact(objcus.DBO, "dd/MM/yyyy", dutchCulture);
                                oBP.UserFields.Fields.Item("U_DOB").Value = DT2;
                            }

                            //oBP.Addresses.AddressName = objcus.CustomerName;
                            //oBP.Addresses.Street = objcus.Street;
                            //oBP.Addresses.Block = objcus.Block;
                            //oBP.Addresses.ZipCode = objcus.Pincode;
                            //oBP.Addresses.City = objcus.city;
                            //oBP.Addresses.State = objcus.State;
                            //oBP.Addresses.StreetNo = objcus.Area;
                            //oBP.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                            //oBP.Addresses.AddressName2 = objcus.Landmark;
                            //oBP.Addresses.AddressName3 = objcus.DoorNo;
                            //oBP.Addresses.Add();


                            using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                            {
                                conBase.Open();
                                using (SqlCommand cmdbase = new SqlCommand("select LineNum  from crd1 where AdresType='S' and cardcode='" + bpcc + "' ", conBase))
                                //(select DocEntry from(select DocEntry, ROW_NUMBER() OVER (ORDER BY DocEntry DESC )  AS row_num, DocNum from ordr where DocNum = '" + objact.saleorder.ToString() + "') as bb  where bb.row_num = 1)
                                {

                                    using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                    {
                                        if (rdrbase.HasRows)
                                        {
                                            rdrbase.Read();
                                            LineNum = Convert.ToInt32(rdrbase["LineNum"].ToString());

                                        }

                                    }
                                }
                                conBase.Close();
                            }




                            //oBP.Addresses.StreetNo = objcus.Area;
                            //oBP.Addresses.Block = objcus.Block;
                            //oBP.Addresses.ZipCode = objcus.Pincode;
                            //oBP.Addresses.City = objcus.city;
                            //oBP.Addresses.State = objcus.State;
                            //oBP.Addresses.AddressName3 = objcus.DoorNo;
                            //oBP.Addresses.AddressName2 = objcus.Landmark;
                            //oBP.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                            //if (a == true)
                            //    oBP.CardCode = dsCUST.Tables[0].Rows[0]["CardCode"].ToString();
                            //oBP.Addresses.AddressName = objcus.CustomerName;
                            //oBP.Addresses.Add();
                            oBP.Addresses.SetCurrentLine(LineNum);
                            //if (oBP.Addresses.AddressName == objcus.CustomerName)
                            //{
                            oBP.Addresses.Street = objcus.Street;
                            oBP.Addresses.Block = objcus.Block;
                            oBP.Addresses.ZipCode = objcus.Pincode;
                            oBP.Addresses.City = objcus.city;
                            oBP.Addresses.BuildingFloorRoom = objcus.Area;
                            oBP.Addresses.State = objcus.State;
                            oBP.Addresses.AddressName2 = objcus.Landmark;
                            oBP.Addresses.AddressName3 = objcus.DoorNo;
                            //oBP.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                            //}  


                            //oBP.Addresses.AddressName = objcus.CustomerName;
                            //oBP.Addresses.Street = objcus.Street;
                            //oBP.Addresses.Block = objcus.Block;
                            //oBP.Addresses.ZipCode = objcus.Pincode;
                            //oBP.Addresses.City = objcus.city;
                            //oBP.Addresses.StreetNo = objcus.Area;
                            //oBP.Addresses.State = objcus.State;
                            //oBP.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                            //oBP.CardCode = bpcc;

                            //oBP.Addresses.Add();
                            //lRetCode = oBP.Update();


                            //oBP.Addresses.Street = objcus.Street;
                            //oBP.Addresses.Block = objcus.Block;
                            //oBP.Addresses.ZipCode = objcus.Pincode;
                            //oBP.Addresses.City = objcus.city;
                            //oBP.Addresses.StreetNo = objcus.Area;
                            //oBP.Addresses.State = objcus.State;
                            //oBP.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo;
                            //oBP.CardCode = bpcc;
                            //oBP.Addresses.AddressName = objcus.CustomerName;
                            //oBP.Addresses.Add();
                        }

                    }


                    if (a == false)
                        lRetCode = oBP.Add();
                    else
                        lRetCode = oBP.Update();
                    if (lRetCode == 0)
                        ELupFlg = true;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP);
                    if (lRetCode != 0)
                    {
                        oCompany.GetLastError(out lErrCode, out sErrMsg);

                        // Logger.LogInfo("DML FLAG:" + Dml + ":SAP EXIST FLAG:" + a.ToString() + "$" + lErrCode + ":" + sErrMsg + "Error Rec:" + CardCd);

                        //V_Transaction_No = CardCd;
                        //V_Error_Code = lErrCode.ToString();
                        //V_Error_Msg = sErrMsg.Replace("'", "");
                        //V_DML = Dml;
                        return sErrMsg;
                    }
                    else
                    {
                        return sapmeg = "Success";
                    }
                }
                else
                {
                    return sapmeg;


                }


            }
            catch (Exception e)
            {
                //Logger.LogInfo("Exception 2 Rec" + e.Message.ToString());
                //Logger.LogInfo(e);
                //V_Date_Time = DateTime.Now;
                //V_Transaction_Type = "Customer";
                //V_Transaction_No = " ExcpRec";
                //V_Error_Code = "0";
                //V_Error_Msg = e.Message.Replace("'", "");
                //V_DML = "";
                return e.Message;
                //V_Remarks = "Exception 2 Rec: " + Logger.LogInfoSS(e);              
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            // Logger.LogInfo("Customer Master Updated Succesfully");
            //return lRetCode;
            return sapmeg;
        }

        public string Delivery(deliveryModel objdel)
        {
            //SAPConnect();
            V_Transaction_Type = ""; V_Transaction_No = ""; V_Error_Code = ""; V_DML = ""; V_Error_Msg = ""; V_Remarks = ""; V_Dummy1 = "";
            V_Date_Time = new DateTime();
            bool ELupFlg = false;
            bool a = true;
            string query1 = "";
            String websaleorder = "", Dml = "";
            Int32 BaseEntry = 0;
            string sapmeg = "";
            CultureInfo objDATE = new CultureInfo("en-US");
            DataSet dsDel = new DataSet();
            bool CheckUpdate = false;
            DataSet dsDel1 = new DataSet();
            DataSet dsserial = new DataSet();
            SqlCommand sqlcmd1 = new SqlCommand();
            SqlDataReader sqldr1;
            try
            {
                sapmeg = SAPConnect();
                if (sapmeg == "Connected")
                {
                    BaseEntry = 0;
                    V_Transaction_Type = "Delivery";

                    oDelivery = ((SAPbobsCOM.Documents)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)));
                    if (objdel.delheqad != null)
                    {
                        lRetCode = 0;
                        ELupFlg = false;

                        oDelivery.CardCode = objdel.delheqad.CustomerNo.ToString();
                        //oDelivery.NumAtCard = dsCUST.Tables[0].Rows[j]["u_invno"].ToString();
                        oDelivery.DocCurrency = "INR";
                        V_Date_Time = DateTime.Now;
                        var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");

                        DateTime DT1 = new DateTime();
                        DT1 = DateTime.ParseExact(objdel.delheqad.AltDate, "dd/MM/yyyy", null);
                        //DateTime DT1 = new DateTime();
                        //DT1 = Convert.ToDateTime(objdel.delheqad.DelDate);
                        oDelivery.DocDate = DT1;
                        // oDelivery.DocDate =dsDel.Tables[0].Rows[i]["DATE"].ToString();
                        oDelivery.UserFields.Fields.Item("U_UserId").Value = objdel.delheqad.UserId;
                        //oDelivery.UserFields.Fields.Item("U_TRNO").Value = dsDel.Tables[0].Rows[i]["TRNO"].ToString();
                        //oDelivery.UserFields.Fields.Item("U_GATEPASS").Value = dsDel.Tables[0].Rows[i]["GATEPASS"].ToString();
                        //oDelivery.UserFields.Fields.Item("U_DELTIME").Value = dsDel.Tables[0].Rows[i]["DELTIME"].ToString();
                        //oDelivery.UserFields.Fields.Item("U_RECEIPNO").Value = "";
                        if ((objdel.delheqad.AltDetails == null) || (objdel.delheqad.AltDetails.ToString() == ""))
                        {

                        }
                        else
                        {
                            oDelivery.Comments = objdel.delheqad.AltDetails;
                        }

                        oDelivery.NumAtCard = objdel.delheqad.SaleOrderNo;
                        oDelivery.DiscountPercent = Convert.ToDouble(objdel.delheqad.DiscountP);


                        using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                        {
                            conBase.Open();
                            using (SqlCommand cmdbase = new SqlCommand("select docentry,numatcard from ORDR where DocNum='" + objdel.delheqad.SaleOrderNo.ToString() + "'", conBase))
                            {

                                using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                {
                                    if (rdrbase.HasRows)
                                    {
                                        rdrbase.Read();
                                        BaseEntry = Convert.ToInt32(rdrbase["docentry"].ToString());
                                        websaleorder = rdrbase["numatcard"].ToString();

                                    }

                                }
                            }
                            conBase.Close();
                        }
                        oDelivery.UserFields.Fields.Item("U_WebSaleOrder").Value = websaleorder;






                        //dsDel1.Clear();
                        //string query1 = "select DCCID,DCHID,Model,Whscode,CCCODE,KEYNO,Quantity,RATE,isnull((case when TAXCODE='VAT0' then 'TAX@0' when TAXCODE='VAT0' then 'TAX@0' when TAXCODE='VAT12.5' then 'VAT@12.5' when TAXCODE='VAT4' then 'VAT@4' else 'TAX@0' end),'TAX@0') TAXCODE,TaxAmount,isnull(Discount,0)Discount,Amount,Description,Variant,Color,EngineNo,ChassisNo from dbo.LNS_TRNS_DELCHDET  where Model is not null and DCHID=''";
                        //sqlDA1 = new SqlDataAdapter(query1, conn);
                        //sqlDA1.Fill(dsDel1, "LNS_TRNS_DELCHDET");
                        //if (dsDel1.Tables[0].Rows.Count > 0)
                        //{
                        int j = 0;
                        foreach (SaleOrderDetail objchild in objdel.objsaleorder)
                        {
                           
                            oDelivery.Lines.SetCurrentLine(j);
                            //Itemcode = dsprice.Tables[0].Rows[k]["ITEMCODE"].ToString();
                            oDelivery.Lines.ItemCode = objchild.ItemCode.ToString();
                            //oDelivery.Lines.WarehouseCode = dsDel1.Tables[0].Rows[j]["Whscode"].ToString();
                            oDelivery.Lines.WarehouseCode = objdel.delheqad.WHCODE;
                            oDelivery.Lines.TaxCode = objchild.taxCode.ToString();
                            oDelivery.Lines.Quantity = Convert.ToDouble(objchild.quantity);
                            oDelivery.Lines.UnitPrice = Convert.ToDouble(objchild.UnitPrice);
                            //DateTime DT2 = new DateTime();
                            //DT2 = Convert.ToDateTime(objchild.Deliverydate);
                            //var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");

                            DateTime DT3 = new DateTime();
                            DT3 = DateTime.ParseExact(objchild.Deliverydate, "dd/MM/yyyy", null);
                            oDelivery.Lines.UserFields.Fields.Item("U_DelDate").Value = DT3;
                            oDelivery.Lines.BaseEntry = BaseEntry;
                            oDelivery.Lines.BaseType = 17;


                            using (SqlConnection conBase1 = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                            {
                                conBase1.Open();
                                query1 = "select LineNum  from RDR1 where  Docentry='" + BaseEntry + "  and Itemcode='" + objchild.ItemCode.ToString() + "'";
                                sqlcmd1 = new SqlCommand("select LineNum  from RDR1 where  Docentry='" + BaseEntry + "'  and Itemcode='" + objchild.ItemCode.ToString() + "' and Quantity='" + Convert.ToDouble(objchild.quantity.ToString()) + "'", conBase1);
                                sqldr1 = sqlcmd1.ExecuteReader();

                                while (sqldr1.Read())
                                {
                                    oDelivery.Lines.BaseLine = Convert.ToInt16(sqldr1["LineNum"].ToString());

                                    //using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                                    //{
                                    //    connection.Open();
                                    //    using (SqlCommand command = new SqlCommand("update  R set R.U_Flag ='Y'   from PDN1 As R inner join OPDN As P on R.DocEntry =P.DocEntry     where     P.NumAtCard ='" + OrDs.Tables["APItbl"].Rows[i]["RCT_MRN_NO"].ToString() + "' and R.Itemcode='" + OrDs.Tables["APItbl"].Rows[i]["ITEMCODE"].ToString() + "' and R.Quantity='" + Convert.ToDouble(OrDs.Tables["APItbl"].Rows[i]["RCT_FREE_QTY"].ToString()) + "'  and R.LineNum='" + Convert.ToInt16(sqldr1["LineNum"].ToString()) + "'", connection))
                                    //    {
                                    //        command.ExecuteNonQuery();
                                    //    }
                                    //    connection.Close();
                                    //}

                                }
                                conBase1.Close();
                            }
                            sqldr1.Close();






                            oDelivery.Lines.Add();
                            j++;
                        }
                        //}

                        lRetCode = oDelivery.Add();
                        if (lRetCode == 0)
                            ELupFlg = true;
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oDelivery);
                        CheckUpdate = true;
                        if (lRetCode != 0)
                        {
                            oCompany.GetLastError(out lErrCode, out sErrMsg);
                            return sErrMsg;

                        }
                        else if (lRetCode == 0)
                        {
                            return sapmeg = "Success";

                        }
                        //}

                    }
                    else
                    {
                        return sapmeg = "No Records Exists!";
                    }
                }
                else
                {
                    return sapmeg;
                }


            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return sapmeg;
        }

        public string SalesInvoice(Invoicemodel objinv)
        {
            //SAPConnect();
            V_Transaction_Type = ""; V_Transaction_No = ""; V_Error_Code = ""; V_DML = ""; V_Error_Msg = ""; V_Remarks = ""; V_Dummy1 = "";
            V_Date_Time = new DateTime();
            bool ELupFlg = false;
            bool a = true;
            string query1 = "";
            Double Advanc = 0;
            String websaleorder = "";
            //string Dml = "";
            Int32 BaseEntry = 0, sodoc = 0, cnt = 0; ;
            string sapmeg = "";
            Int32 RecCnt = 0;
            CultureInfo objDATE = new CultureInfo("en-US");
            DataSet dsDel = new DataSet();
            bool CheckUpdate = false;
            DataSet dsDel1 = new DataSet();
            DataSet dsserial = new DataSet();
            SqlCommand sqlcmd1 = new SqlCommand();
            SqlDataReader sqldr1;
            try
            {
                sapmeg = SAPConnect();
                if (sapmeg == "Connected")
                {
                    DateTime dtTmp;
                    V_Transaction_Type = "SaleInvoice";


                    oInvoice = ((SAPbobsCOM.Documents)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)));



                    //testing block for ved
                    //oInvoice = ((SAPbobsCOM.Documents)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)));
                    //oSaleorder = ((SAPbobsCOM.Documents)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)));

                    //sodoc = 68;


                    //a = oSaleorder.GetByKey(sodoc);
                    //if (a == true)
                    //{
                    //    //oSaleorder.DocumentStatus = BoStatus.bost_Open;
                    //    oSaleorder.Close();
                    //    //oSaleorder.Update();
                    //}
                    //    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSaleorder);

                    //










                    //oActivity = ((SAPbobsCOM.Contacts)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oContacts)));
                    if (objinv.invhead != null)
                    {



                        using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                        {
                            conBase.Open();
                            using (SqlCommand cmdbase = new SqlCommand("select Count(*) Count from OINV where NumAtCard='" + objinv.invhead.DeliveryNo.ToString() + "' and  Docstatus='O' and CANCELED='N'", conBase))
                            {

                                using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                {
                                    if (rdrbase.HasRows)
                                    {
                                        rdrbase.Read();
                                        cnt = Convert.ToInt32(rdrbase["Count"].ToString());

                                    }

                                }
                            }
                            conBase.Close();
                        }

                        if (cnt > 0)
                        {
                            return sapmeg = "Sale Order Already Exists";
                        }
                        else
                        {
                            lRetCode = 0;
                            ELupFlg = false;
                            //for (int i = 0; i < dsDel.Tables[0].Rows.Count; i++)
                            //{
                            oInvoice.CardCode = objinv.invhead.CustomerNo.ToString();
                            //oDelivery.NumAtCard = dsCUST.Tables[0].Rows[j]["u_invno"].ToString();
                            oInvoice.DocCurrency = "INR";
                            V_Date_Time = DateTime.Now;

                            var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");

                            //DateTime DT1 = new DateTime();
                            //DT1 = DateTime.ParseExact(objinv.invhead.DeliveryDate.Trim());

                            DateTime DT1 = new DateTime();

                            string deldate = objinv.invhead.Invoicedate.Trim();


                            DT1 = DateTime.ParseExact(deldate, "dd/MM/yyyy", null);
                            oInvoice.DocDate = DT1;
                            oInvoice.UserFields.Fields.Item("U_UserId").Value = objinv.invhead.UserId;
                            //oDelivery.UserFields.Fields.Item("U_TRNO").Value = dsDel.Tables[0].Rows[i]["TRNO"].ToString();
                            //oDelivery.UserFields.Fields.Item("U_GATEPASS").Value = dsDel.Tables[0].Rows[i]["GATEPASS"].ToString();
                            //oDelivery.UserFields.Fields.Item("U_DELTIME").Value = dsDel.Tables[0].Rows[i]["DELTIME"].ToString();
                            //oDelivery.UserFields.Fields.Item("U_RECEIPNO").Value = "";
                            if ((objinv.invhead.invoicedetails == null) || (objinv.invhead.invoicedetails.ToString() == ""))
                            {

                            }
                            else
                            {
                                oInvoice.Comments = objinv.invhead.AltDetails;
                            }

                            oInvoice.NumAtCard = objinv.invhead.DeliveryNo;
                            oInvoice.DiscountPercent = Convert.ToDouble(objinv.invhead.discount);

                            using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                            {
                                conBase.Open();
                                using (SqlCommand cmdbase = new SqlCommand("select docentry,U_WebSaleOrder from ORDR where DocNum='" + objinv.invhead.DeliveryNo.ToString() + "'", conBase))
                                {

                                    using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                    {
                                        if (rdrbase.HasRows)
                                        {
                                            rdrbase.Read();
                                            BaseEntry = Convert.ToInt32(rdrbase["docentry"].ToString());
                                            websaleorder = rdrbase["U_WebSaleOrder"].ToString();
                                        }

                                    }
                                }
                                conBase.Close();
                            }

                            oInvoice.UserFields.Fields.Item("U_WebSaleOrder").Value = websaleorder;
                            //dsDel1.Clear();
                            //string query1 = "select INVDETID,INVID,Model,Whscode,CCCODE,KEYNO,RATE,isnull((case when TAXCODE='VAT0' then 'TAX@0' when TAXCODE='VAT0' then 'TAX@0' when TAXCODE='VAT12.5' then 'VAT@12.5' when TAXCODE='VAT4' then 'VAT@4' else 'TAX@0' end),'TAX@0') TAXCODE,TaxAmount,Discount,Amount,Quantity,Variant,Color,EngineNo,ChassisNo from dbo.LNS_TRNS_INVDET where INVID=" + INVID;
                            //sqlDA1 = new SqlDataAdapter(query1, conn);
                            //sqlDA1.Fill(dsDel1, "LNS_TRNS_INVDET");
                            //foreach (SaleOrderDetails objchild in objdel.objsaleorder)
                            //{
                            int j = 0;
                            foreach (Invoicechild objchild in objinv.objinvlist)
                            {
                                //int j = 0;
                                oInvoice.Lines.SetCurrentLine(j);
                                oInvoice.Lines.ItemCode = objchild.ItemCode.ToString();
                                oInvoice.Lines.WarehouseCode = objinv.invhead.WHCODE;
                                oInvoice.Lines.TaxCode = objchild.Tax.ToString();
                                oInvoice.Lines.Quantity = Convert.ToDouble(objchild.qty.ToString());
                                oInvoice.Lines.UnitPrice = Convert.ToDouble(objchild.Rate.ToString());
                                //oInvoice.Lines.COGSCostingCode = dsDel1.Tables[0].Rows[j]["CCCODE"].ToString();
                                //string serialquery = "select SysNumber,DistNumber,MnfSerial,isnull(ExpDate,'9999-12-31 00:00:00.000')ExpDate from osrn where itemcode='" + dsDel1.Tables[0].Rows[j]["Model"].ToString() + "' and MnfSerial='" + dsDel1.Tables[0].Rows[j]["EngineNo"].ToString() + "' AND DistNumber='" + dsDel1.Tables[0].Rows[j]["ChassisNo"].ToString() + "' AND U_KeyNum='" + dsDel1.Tables[0].Rows[j]["KEYNO"].ToString() + "'";
                                //SAPbobsCOM.Items oItemN;
                                //oItemN = ((SAPbobsCOM.Items)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)));
                                //oItemN.GetByKey(dsDel1.Tables[0].Rows[j]["Model"].ToString());
                                //if (oItemN.ManageSerialNumbers == SAPbobsCOM.BoYesNoEnum.tYES)
                                //{
                                //    sqlDA2 = new SqlDataAdapter(serialquery, conn);
                                //    sqlDA2.Fill(dsserial, "osrn");
                                //    if (dsserial.Tables[0].Rows.Count > 0)
                                //    {
                                //        oInvoice.Lines.SerialNumbers.SystemSerialNumber = Convert.ToInt32(dsserial.Tables[0].Rows[0]["SysNumber"]);
                                //        oInvoice.Lines.SerialNumbers.ManufacturerSerialNumber = dsserial.Tables[0].Rows[0]["MnfSerial"].ToString();
                                //        oInvoice.Lines.SerialNumbers.InternalSerialNumber = dsserial.Tables[0].Rows[0]["DistNumber"].ToString();
                                //        oInvoice.Lines.SerialNumbers.UserFields.Fields.Item("U_Variant").Value = dsDel1.Tables[0].Rows[j]["Variant"].ToString();
                                //        oInvoice.Lines.SerialNumbers.UserFields.Fields.Item("U_Color").Value = dsDel1.Tables[0].Rows[j]["Color"].ToString();
                                //        oInvoice.Lines.SerialNumbers.UserFields.Fields.Item("U_KeyNum").Value = dsDel1.Tables[0].Rows[j]["KEYNO"].ToString();
                                //        oInvoice.Lines.SerialNumbers.SetCurrentLine(0);
                                //        oInvoice.Lines.SerialNumbers.Add();
                                //    }
                                //}


                                //DateTime DT2 = new DateTime();
                                //DT2 = Convert.ToDateTime(objchild.d);
                                //oDelivery.Lines.UserFields.Fields.Item("U_DelDate").Value = DT2;
                                oInvoice.Lines.BaseEntry = BaseEntry;
                                oInvoice.Lines.BaseType = 17;


                                using (SqlConnection conBase1 = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                                {
                                    conBase1.Open();
                                    query1 = "select LineNum  from RDR1 where  Docentry='" + BaseEntry + "  and Itemcode='" + objchild.ItemCode.ToString() + "'";
                                    sqlcmd1 = new SqlCommand("select TOP 1 LineNum  from RDR1 where  Docentry='" + BaseEntry + "'  and Itemcode='" + objchild.ItemCode.ToString() + "' and Quantity='" + Convert.ToDouble(objchild.qty.ToString()) + "' and ((U_MPI='" + objchild.Line.ToString() + "') or (U_ParentItem='" + objchild.Line.ToString() + "' ))   order by LineNum desc", conBase1);
                                    sqldr1 = sqlcmd1.ExecuteReader();

                                    while (sqldr1.Read())
                                    {
                                        oInvoice.Lines.BaseLine = Convert.ToInt16(sqldr1["LineNum"].ToString());

                                        //using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                                        //{
                                        //    connection.Open();
                                        //    using (SqlCommand command = new SqlCommand("update  R set R.U_Flag ='Y'   from PDN1 As R inner join OPDN As P on R.DocEntry =P.DocEntry     where     P.NumAtCard ='" + OrDs.Tables["APItbl"].Rows[i]["RCT_MRN_NO"].ToString() + "' and R.Itemcode='" + OrDs.Tables["APItbl"].Rows[i]["ITEMCODE"].ToString() + "' and R.Quantity='" + Convert.ToDouble(OrDs.Tables["APItbl"].Rows[i]["RCT_FREE_QTY"].ToString()) + "'  and R.LineNum='" + Convert.ToInt16(sqldr1["LineNum"].ToString()) + "'", connection))
                                        //    {
                                        //        command.ExecuteNonQuery();
                                        //    }
                                        //    connection.Close();
                                        //}

                                    }
                                    conBase1.Close();
                                }
                                sqldr1.Close();





                                oInvoice.Lines.Add();
                                j++;
                            }
                            //}
                            lRetCode = oInvoice.Add();
                            if (lRetCode == 0)
                            {
                                try
                                {
                                    if (objinv.invhead != null)
                                    {

                                        if (objinv.invhead.CurrentAdvance == null)
                                        {
                                            Advanc = 0;
                                        }
                                        else
                                        {
                                            Advanc = Convert.ToDouble(objinv.invhead.CurrentAdvance);

                                        }

                                        if (Advanc > 0)
                                        {
                                            DateTime CRDate;
                                            oIncomingPayments = ((SAPbobsCOM.Payments)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)));
                                            oIncomingPayments.CardCode = objinv.invhead.CustomerNo.ToString();
                                            oIncomingPayments.DocTypte = SAPbobsCOM.BoRcptTypes.rCustomer;






                                            //DT1 = DateTime.ParseExact(invDate, "dd/MM/yyyy", null);
                                            //oInvoice.DocDate = DT1;


                                            DateTime DT2 = new DateTime();
                                            string invDate = objinv.invhead.Invoicedate.Trim();
                                            DT2 = DateTime.ParseExact(invDate, "dd/MM/yyyy", null);
                                            oIncomingPayments.DocDate = DT2;
                                            oIncomingPayments.DocCurrency = "INR";
                                            oIncomingPayments.UserFields.Fields.Item("U_SONo").Value = objinv.invhead.DeliveryNo.ToString();
                                            oIncomingPayments.UserFields.Fields.Item("U_Adv").Value = objinv.invhead.CurrentAdvance.ToString();

                                            if ((objinv.invhead.AltDetails == null) || (objinv.invhead.AltDetails.ToString() == ""))
                                            {

                                            }
                                            else
                                            {
                                                oIncomingPayments.Remarks = objinv.invhead.AltDetails;
                                            }


                                            //using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                                            //{
                                            //    conBase.Open();
                                            //    using (SqlCommand cmdbase = new SqlCommand("select docentry,docnum from ORDR where NumAtCard='" + objinv.invhead.DeliveryNo.ToString() + "'", conBase))
                                            //    {

                                            //        using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                            //        {
                                            //            if (rdrbase.HasRows)
                                            //            {
                                            //                rdrbase.Read();
                                            //                BaseEntry = Convert.ToInt32(rdrbase["docnum"].ToString());

                                            //            }

                                            //        }
                                            //    }
                                            //    conBase.Close();
                                            //}
                                            oIncomingPayments.UserFields.Fields.Item("U_SAPSO").Value = objinv.invhead.DeliveryNo.ToString();

                                            if ((objinv.invhead.cash > 0))
                                            {
                                                oIncomingPayments.CashSum = Convert.ToDouble(objinv.invhead.cash);


                                            }
                                            if ((objinv.invhead.card > 0))
                                            {
                                                oIncomingPayments.CreditCards.CreditCard = 1;
                                                oIncomingPayments.CreditCards.CreditCardNumber = "1234";
                                                string CCDate = DateTime.Now.AddYears(2).ToString("yyyy-MM-dd");
                                                CRDate = Convert.ToDateTime(CCDate);
                                                oIncomingPayments.CreditCards.CardValidUntil = CRDate;
                                                oIncomingPayments.CreditCards.CreditSum = Convert.ToDouble(objinv.invhead.card);
                                                oIncomingPayments.CreditCards.PaymentMethodCode = 1;
                                                oIncomingPayments.CreditCards.VoucherNum = "234";
                                                oIncomingPayments.CreditCards.OwnerIdNum = "125";
                                                oIncomingPayments.CreditCards.OwnerPhone = "254";
                                                oIncomingPayments.CreditCards.Add();
                                            }
                                            if ((objinv.invhead.OtherPayments > 0))
                                            {
                                                oIncomingPayments.TransferAccount = "1110006";
                                                oIncomingPayments.TransferDate = DT2;
                                                oIncomingPayments.TransferSum = Convert.ToDouble(objinv.invhead.OtherPayments);
                                            }


                                            lRetCode = oIncomingPayments.Add();
                                            if (lRetCode == 0)
                                            {
                                                ELupFlg = true;
                                                System.Runtime.InteropServices.Marshal.ReleaseComObject(oIncomingPayments);
                                                CheckUpdate = true;
                                                return sapmeg = "Success";


                                            }
                                            else if (lRetCode != 0)
                                            {
                                                oCompany.GetLastError(out lErrCode, out sErrMsg);

                                                return sErrMsg;

                                            }

                                        }
                                        else
                                        {
                                            return sapmeg = "Success";
                                        }



                                    }

                                }
                                catch (Exception e)
                                {

                                    return e.Message;
                                }
                                finally
                                {

                                }

                            }
                            else if (lRetCode != 0)
                            {
                                oCompany.GetLastError(out lErrCode, out sErrMsg);
                                return sErrMsg;

                            }

                        }







                        //}

                    }
                    else
                    {
                        return sapmeg = "No Records!";
                    }

                }
                else
                {
                    return sapmeg;
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return sapmeg;
        }



        public string saleorder(SapSaleorder objsaleorder)
        {
            V_Transaction_Type = ""; V_Transaction_No = ""; V_Error_Code = ""; V_DML = ""; V_Error_Msg = ""; V_Remarks = ""; V_Dummy1 = "";
            V_Date_Time = new DateTime();
            bool ELupFlg = false;
            bool a = true;
            string query1 = "", Name = "";
            String ItmsG = "";
            //string Dml = "";
            Double unitprice, UP, MP, SP;
            Double SPQty, SP1, MPQty, MP1;
            Double Advanc = 0;
            Int32 BaseEntry = 0, sodoc = 0, cnt = 0;
            Int32 RecCnt = 0;
            string sapmeg = "";
            CultureInfo objDATE = new CultureInfo("en-US");
            DataSet dsDel = new DataSet();
            bool CheckUpdate = false;
            DataSet dsDel1 = new DataSet();
            DataSet dsserial = new DataSet();
            SqlCommand sqlcmd1 = new SqlCommand();
            SqlDataReader sqldr1;
            try
            {
                sapmeg = SAPConnect();
                if (sapmeg == "Connected")
                {
                    //oOrder = ((SAPbobsCOM.Documents)(MvcApplication.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)));
                    oOrder = ((SAPbobsCOM.Documents)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)));
                    //bool a = oOrder.GetByKey(Convert.ToInt32(objsaleorder.objsaleorderHeader.DocEntry));

                    if (objsaleorder.objsaleorder != null)
                    {

                        using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                        {
                            conBase.Open();
                            using (SqlCommand cmdbase = new SqlCommand("select Count(*) Count from ORDR where NumAtCard='" + objsaleorder.objsaleorder.DocEntry.ToString() + "' and  Docstatus='O' and CANCELED='N'", conBase))
                            {

                                using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                {
                                    if (rdrbase.HasRows)
                                    {
                                        rdrbase.Read();
                                        cnt = Convert.ToInt32(rdrbase["Count"].ToString());

                                    }

                                }
                            }
                            conBase.Close();
                        }

                        if (cnt > 0)
                        {
                            //return sapmeg = "Sale Order Already Exists";

                            //try
                            //{
                            //    if (objsaleorder.objsaleorder != null)
                            //    {

                            //        if (objsaleorder.objsaleorder.Advance == null)
                            //        {
                            //            Advanc = 0;
                            //        }
                            //        else
                            //        {
                            //            Advanc = Convert.ToDouble(objsaleorder.objsaleorder.Advance);

                            //        }

                            //        if (Advanc > 0)
                            //        {
                            //            DateTime CRDate;
                            //            oIncomingPayments = ((SAPbobsCOM.Payments)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)));
                            //            oIncomingPayments.CardCode = objsaleorder.objsaleorder.CardCode.ToString();
                            //            oIncomingPayments.DocTypte = SAPbobsCOM.BoRcptTypes.rCustomer;
                            //            DateTime DT2 = new DateTime();
                            //            DT2 = DateTime.ParseExact(objsaleorder.objsaleorder.DocDate, "dd/MM/yyyy", null);
                            //            oIncomingPayments.DocDate = DT2;
                            //            oIncomingPayments.DocCurrency = "INR";
                            //            oIncomingPayments.UserFields.Fields.Item("U_SONo").Value = objsaleorder.objsaleorder.DocEntry.ToString();
                            //            oIncomingPayments.UserFields.Fields.Item("U_Adv").Value = objsaleorder.objsaleorder.Advance.ToString();

                            //            if ((objsaleorder.objsaleorder.Remarks == null) || (objsaleorder.objsaleorder.Remarks.ToString() == ""))
                            //            {

                            //            }
                            //            else
                            //            {
                            //                oIncomingPayments.Remarks = objsaleorder.objsaleorder.Remarks;
                            //            }


                            //            using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                            //            {
                            //                conBase.Open();
                            //                using (SqlCommand cmdbase = new SqlCommand("select docentry,docnum from ORDR where NumAtCard='" + objsaleorder.objsaleorder.DocEntry.ToString() + "'", conBase))
                            //                {

                            //                    using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                            //                    {
                            //                        if (rdrbase.HasRows)
                            //                        {
                            //                            rdrbase.Read();
                            //                            BaseEntry = Convert.ToInt32(rdrbase["docnum"].ToString());

                            //                        }

                            //                    }
                            //                }
                            //                conBase.Close();
                            //            }
                            //            oIncomingPayments.UserFields.Fields.Item("U_SAPSO").Value = Convert.ToString(BaseEntry);

                            //            if ((objsaleorder.objsaleorder.cash > 0))
                            //            {
                            //                oIncomingPayments.CashSum = Convert.ToDouble(objsaleorder.objsaleorder.cash);


                            //            }
                            //            if ((objsaleorder.objsaleorder.card > 0))
                            //            {
                            //                oIncomingPayments.CreditCards.CreditCard = 1;
                            //                oIncomingPayments.CreditCards.CreditCardNumber = "1234";
                            //                string CCDate = DateTime.Now.AddYears(2).ToString("yyyy-MM-dd");
                            //                CRDate = Convert.ToDateTime(CCDate);
                            //                oIncomingPayments.CreditCards.CardValidUntil = CRDate;
                            //                oIncomingPayments.CreditCards.CreditSum = Convert.ToDouble(objsaleorder.objsaleorder.card);
                            //                oIncomingPayments.CreditCards.PaymentMethodCode = 1;
                            //                oIncomingPayments.CreditCards.VoucherNum = "234";
                            //                oIncomingPayments.CreditCards.OwnerIdNum = "125";
                            //                oIncomingPayments.CreditCards.OwnerPhone = "254";
                            //                oIncomingPayments.CreditCards.Add();
                            //            }
                            //            if ((objsaleorder.objsaleorder.OtherPayments > 0))
                            //            {
                            //                oIncomingPayments.TransferAccount = "1110006";
                            //                oIncomingPayments.TransferDate = DT2;
                            //                oIncomingPayments.TransferSum = Convert.ToDouble(objsaleorder.objsaleorder.OtherPayments);
                            //            }


                            //            lRetCode = oIncomingPayments.Add();
                            //            if (lRetCode == 0)
                            //            {
                            //                ELupFlg = true;
                            //                System.Runtime.InteropServices.Marshal.ReleaseComObject(oIncomingPayments);
                            //                CheckUpdate = true;
                            //                return sapmeg = "Success";


                            //            }
                            //            else if (lRetCode != 0)
                            //            {
                            //                oCompany.GetLastError(out lErrCode, out sErrMsg);

                            //                return sErrMsg;

                            //            }

                            //        }
                            //        else
                            //        {
                            //            return sapmeg = "Success";
                            //        }



                            //    }

                            //}
                            //catch (Exception e)
                            //{

                            //    return e.Message;
                            //}
                            //finally
                            //{

                            //}

                        }
                        else
                        {
                            lRetCode = 0;
                            ELupFlg = false;

                            oOrder.CardCode = objsaleorder.objsaleorder.CardCode;
                            oOrder.DocCurrency = "INR";
                            oOrder.NumAtCard = objsaleorder.objsaleorder.DocEntry;
                            var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");

                            DateTime DT1 = new DateTime();
                            DT1 = DateTime.ParseExact(objsaleorder.objsaleorder.DocDate, "dd/MM/yyyy", null);
                            oOrder.DocDate = DT1;
                            oOrder.DocDueDate = DT1;
                            oOrder.UserFields.Fields.Item("U_UserId").Value = objsaleorder.objsaleorder.UserName;
                            oOrder.UserFields.Fields.Item("U_WebSaleOrder").Value = objsaleorder.objsaleorder.DocEntry;
                            if ((objsaleorder.objsaleorder.Remarks == null) || (objsaleorder.objsaleorder.Remarks.ToString() == ""))
                            {

                            }
                            else
                            {
                                oOrder.Comments = objsaleorder.objsaleorder.Remarks;
                            }
                            oOrder.DiscountPercent = Convert.ToDouble(objsaleorder.objsaleorder.Discount);
                            oOrder.UserFields.Fields.Item("U_Adv").Value = Convert.ToDouble(objsaleorder.objsaleorder.Advance);
                            oOrder.UserFields.Fields.Item("U_BAmt").Value = Convert.ToDouble(objsaleorder.objsaleorder.BalanceAmount);
                            oOrder.UserFields.Fields.Item("U_WhsCode").Value = objsaleorder.objsaleorder.WHSCODE;
                            if ((objsaleorder.objsaleorder.Pickupuser == null) || (objsaleorder.objsaleorder.Pickupuser.ToString() == ""))
                            {

                            }
                            else
                            {
                                oOrder.UserFields.Fields.Item("U_PUser").Value = objsaleorder.objsaleorder.Pickupuser;
                            }
                            int j = 0;
                            string deleiverydate = "";
                            foreach (SaleOrderDetail objchild in objsaleorder.objsaleorderdetails)
                            {

                                if ((objchild.parentId != "0") && (objchild.quantity == "0"))
                                {

                                }
                                else
                                {
                                    if (objchild.parentId == "0")
                                    {
                                        deleiverydate = objchild.Deliverydate;
                                    }

                                    oOrder.Lines.SetCurrentLine(j);
                                    oOrder.Lines.ItemCode = objchild.ItemCode.ToString();
                                    oOrder.Lines.WarehouseCode = objsaleorder.objsaleorder.WHSCODE; 
                                    oOrder.Lines.TaxCode = objchild.taxCode.ToString();
                                    if (objchild.parentId == "0")
                                    {
                                        if ((objchild.Deliverydate == null) || (objchild.Deliverydate.ToString() == ""))
                                        {
                                            DateTime DT3 = new DateTime();
                                            DT3 = DateTime.ParseExact(deleiverydate, "dd/MM/yyyy", null);
                                            oOrder.Lines.UserFields.Fields.Item("U_DelDate").Value = DT3;
                                        }
                                        else
                                        {
                                            DateTime DT3 = new DateTime();
                                            DT3 = DateTime.ParseExact(objchild.Deliverydate, "dd/MM/yyyy", null);
                                            oOrder.Lines.UserFields.Fields.Item("U_DelDate").Value = DT3;
                                        }
                                        if ((objchild.Intdeldate == null) || (objchild.Intdeldate.ToString() == ""))
                                        {
                                            DateTime DT3 = new DateTime();
                                            DT3 = DateTime.ParseExact(objchild.Intdeldate, "dd/MM/yyyy", null);
                                            oOrder.Lines.UserFields.Fields.Item("U_IDelDate").Value = DT3;
                                        }
                                        else
                                        {
                                            DateTime DT3 = new DateTime();
                                            DT3 = DateTime.ParseExact(objchild.Intdeldate, "dd/MM/yyyy", null);
                                            oOrder.Lines.UserFields.Fields.Item("U_IDelDate").Value = DT3;
                                        }

                                    }
                      

                                    oOrder.Lines.UserFields.Fields.Item("U_ParentItem").Value = objchild.parentId;
                                    if ((objchild.NoFabraic == null) || (objchild.NoFabraic.ToString() == ""))
                                    {

                                    }
                                    else
                                    {
                                        oOrder.Lines.UserFields.Fields.Item("U_Fabric").Value = objchild.NoFabraic;
                                    }

                                    //oOrder.Lines.UserFields.Fields.Item("U_Fabric").Value = objchild.NoFabraic;


                                    if ((objchild.Remarks == null) || (objchild.Remarks.ToString() == ""))
                                    {

                                    }
                                    else
                                    {
                                        oOrder.Lines.UserFields.Fields.Item("U_Remarks").Value = objchild.Remarks;
                                    }

                                    using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                                    {
                                        conBase.Open();
                                        using (SqlCommand cmdbase = new SqlCommand("select ItmsGrpNam  from OITB where ItmsGrpCod=(select ItmsGrpCod  from OITM where itemcode='" + objchild.ItemCode.ToString() + "')", conBase))
                                        {

                                            using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                            {
                                                if (rdrbase.HasRows)
                                                {
                                                    rdrbase.Read();
                                                    ItmsG = rdrbase["ItmsGrpNam"].ToString();

                                                }

                                            }
                                        }
                                        conBase.Close();
                                    }


                                    if (objchild.parentId == "0")
                                    {
                                        oOrder.Lines.UserFields.Fields.Item("U_MPI").Value = objchild.Id.ToString();
                                    }

                                    oOrder.Lines.UserFields.Fields.Item("U_ItemG").Value = ItmsG;
                                    if ((objchild.quantity == null) || (objchild.quantity.ToString() == ""))
                                    {
                                        oOrder.Lines.Quantity = Convert.ToDouble("1");
                                    }
                                    else
                                    {
                                        oOrder.Lines.Quantity = Convert.ToDouble("1");
                                        oOrder.Lines.UserFields.Fields.Item("U_Qty").Value = objchild.quantity;
                                        //oOrder.Lines.Quantity = Convert.ToDouble(objchild.quantity.ToString());
                                    }

                                    if ((objchild.UnitPrice == null) || (objchild.UnitPrice.ToString() == ""))
                                    {
                                        UP = Convert.ToDouble("0");
                                    }
                                    else
                                    {
                                        UP = Convert.ToDouble(objchild.UnitPrice.ToString());
                                    }
                                    if ((objchild.MeterialCost == null) || (objchild.MeterialCost.ToString() == ""))
                                    {
                                        MP = Convert.ToDouble("0");
                                        MP1 = Convert.ToDouble("0");
                                    }
                                    else
                                    {
                                        MPQty = Convert.ToDouble(objchild.quantity);
                                        MP = Convert.ToDouble(objchild.MeterialCost.ToString());
                                        MP1 = ((MPQty) * (MP));
                                    }
                                    if ((objchild.ServiceCost == null) || (objchild.ServiceCost.ToString() == ""))
                                    {
                                        SP = Convert.ToDouble("0");
                                        //SP1= Convert.ToDouble("0");
                                    }
                                    else
                                    {
                                        //SPQty = Convert.ToDouble(objchild.quantity);
                                        SP = Convert.ToDouble(objchild.ServiceCost.ToString());
                                        //SP1 = ((SPQty) * (SP));
                                    }

                                    //if (objchild.IsCustomermeterial == false)
                                    //{
                                        unitprice = ((UP) + (MP1) + (SP));
                                    //}
                                    //else
                                    //{
                                    //    unitprice = Convert.ToDouble(objchild.TotalAmount.ToString());
                                    //}
                                    //unitprice = ((UP) + (MP1) + (SP));
                                    //unitprice = ((UP) + (MP) + (SP));


                                    oOrder.Lines.UnitPrice = unitprice;
                                    oOrder.Lines.Add();

                                    j++;
                                }




                            }

                            lRetCode = oOrder.Add();
                            if (lRetCode == 0)
                                ELupFlg = true;
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(oOrder);
                            CheckUpdate = true;
                            if (lRetCode != 0)
                            {
                                oCompany.GetLastError(out lErrCode, out sErrMsg);

                                return sErrMsg;
                            }
                            else if (lRetCode == 0)
                            {

                                try
                                {
                                    if (objsaleorder.objsaleorder != null)
                                    {

                                        if (objsaleorder.objsaleorder.Advance == null)
                                        {
                                            Advanc = 0;
                                        }
                                        else
                                        {
                                            Advanc = Convert.ToDouble(objsaleorder.objsaleorder.Advance);

                                        }
                                        if (Advanc > 0)
                                        {
                                            DateTime CRDate;
                                            oIncomingPayments = ((SAPbobsCOM.Payments)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)));
                                            oIncomingPayments.CardCode = objsaleorder.objsaleorder.CardCode.ToString();
                                            oIncomingPayments.DocTypte = SAPbobsCOM.BoRcptTypes.rCustomer;
                                            DateTime DT2 = new DateTime();
                                            DT2 = DateTime.ParseExact(objsaleorder.objsaleorder.DocDate, "dd/MM/yyyy", null);
                                            oIncomingPayments.DocDate = DT2;
                                            oIncomingPayments.DocCurrency = "INR";
                                            oIncomingPayments.UserFields.Fields.Item("U_SONo").Value = objsaleorder.objsaleorder.DocEntry.ToString();
                                            oIncomingPayments.UserFields.Fields.Item("U_Adv").Value = objsaleorder.objsaleorder.Advance.ToString();

                                            if ((objsaleorder.objsaleorder.Remarks == null) || (objsaleorder.objsaleorder.Remarks.ToString() == ""))
                                            {

                                            }
                                            else
                                            {
                                                oIncomingPayments.Remarks = objsaleorder.objsaleorder.Remarks;
                                            }


                                            using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                                            {
                                                conBase.Open();
                                                using (SqlCommand cmdbase = new SqlCommand("select docentry,docnum from ORDR where NumAtCard='" + objsaleorder.objsaleorder.DocEntry.ToString() + "'", conBase))
                                                {

                                                    using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                                    {
                                                        if (rdrbase.HasRows)
                                                        {
                                                            rdrbase.Read();
                                                            BaseEntry = Convert.ToInt32(rdrbase["docnum"].ToString());

                                                        }

                                                    }
                                                }
                                                conBase.Close();
                                            }
                                            oIncomingPayments.UserFields.Fields.Item("U_SAPSO").Value = Convert.ToString(BaseEntry);

                                            if ((objsaleorder.objsaleorder.cash > 0))
                                            {
                                                oIncomingPayments.CashSum = Convert.ToDouble(objsaleorder.objsaleorder.cash);


                                            }
                                            if ((objsaleorder.objsaleorder.card > 0))
                                            {
                                                oIncomingPayments.CreditCards.CreditCard = 1;
                                                oIncomingPayments.CreditCards.CreditCardNumber = "1234";
                                                string CCDate = DateTime.Now.AddYears(2).ToString("yyyy-MM-dd");
                                                CRDate = Convert.ToDateTime(CCDate);
                                                oIncomingPayments.CreditCards.CardValidUntil = CRDate;
                                                oIncomingPayments.CreditCards.CreditSum = Convert.ToDouble(objsaleorder.objsaleorder.card);
                                                oIncomingPayments.CreditCards.PaymentMethodCode = 1;
                                                oIncomingPayments.CreditCards.VoucherNum = "234";
                                                oIncomingPayments.CreditCards.OwnerIdNum = "125";
                                                oIncomingPayments.CreditCards.OwnerPhone = "254";
                                                oIncomingPayments.CreditCards.Add();
                                            }
                                            if ((objsaleorder.objsaleorder.OtherPayments > 0))
                                            {
                                                oIncomingPayments.TransferAccount = "1110006";
                                                oIncomingPayments.TransferDate = DT2;
                                                oIncomingPayments.TransferSum = Convert.ToDouble(objsaleorder.objsaleorder.OtherPayments);
                                            }


                                            lRetCode = oIncomingPayments.Add();
                                            if (lRetCode == 0)
                                            {
                                                ELupFlg = true;
                                                System.Runtime.InteropServices.Marshal.ReleaseComObject(oIncomingPayments);
                                                CheckUpdate = true;
                                                return sapmeg = "Success";


                                            }
                                            else if (lRetCode != 0)
                                            {
                                                oCompany.GetLastError(out lErrCode, out sErrMsg);

                                                return sErrMsg;

                                            }

                                        }
                                        else
                                        {
                                            return sapmeg = "Success";
                                        }



                                    }

                                }
                                catch (Exception e)
                                {

                                    return e.Message;
                                }
                                finally
                                {

                                }



                            }

                        }











                    }
                    else
                    {
                        return sapmeg = "No Records!";
                    }
                }
                else
                {
                    return sapmeg;
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return sapmeg;
        }






        public string Activity(ActivityModel objact)
        {
            bool a = true;
            string sapmeg = "", Name = "";
            DataSet dsCUST = new DataSet();
            Int32 cnt = 0, ANo = 0;
            bool b = true;
            //string ANo = "";
            Int32 BaseEntry = 0;
            try
            {
                V_Transaction_Type = "Customer";
                conn.Open();
                BaseEntry = 0;
                sapmeg = SAPConnect();
                if (sapmeg == "Connected")
                {

                    if (objact != null)
                    {

                        using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                        {
                            conBase.Open();
                            using (SqlCommand cmdbase = new SqlCommand("select Count(*) Count from OCLG where clgcode='" + objact.Actvityno.ToString() + "'", conBase))
                            {

                                using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                {
                                    if (rdrbase.HasRows)
                                    {
                                        rdrbase.Read();
                                        cnt = Convert.ToInt32(rdrbase["Count"].ToString());

                                    }

                                }
                            }
                            conBase.Close();
                        }

                        if (cnt > 0)
                        {
                            oActivity = ((SAPbobsCOM.Contacts)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oContacts)));

                            ANo = objact.Actvityno;
                            b = oActivity.GetByKey(ANo);

                            if (b == true)
                            {

                                oActivity.Details = objact.operatorcomments;
                                oActivity.Notes = objact.custcomments;
                                using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                                {
                                    conBase.Open();
                                    using (SqlCommand cmdbase = new SqlCommand("select ISNULL(FirstName,'')+ISNULL(MiddleName,'')+ISNULL(LastName,'') Name from OHEm where Empid='" + objact.Assignempid + "' ", conBase))
                                    //(select DocEntry from(select DocEntry, ROW_NUMBER() OVER (ORDER BY DocEntry DESC )  AS row_num, DocNum from ordr where DocNum = '" + objact.saleorder.ToString() + "') as bb  where bb.row_num = 1)
                                    {

                                        using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                        {
                                            if (rdrbase.HasRows)
                                            {
                                                rdrbase.Read();
                                                Name = rdrbase["Name"].ToString();

                                            }

                                        }
                                    }
                                    conBase.Close();
                                }



                                oActivity.UserFields.Fields.Item("U_AssignBy").Value = Name;
                                lRetCode = oActivity.Update();
                            }
                            else
                            {
                                return sapmeg = "No Records!";

                            }





                            //if (a == false)

                            //else
                            //    lRetCode = oActivity.Update();
                            if (lRetCode == 0)
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(oActivity);
                            if (lRetCode != 0)
                            {
                                oCompany.GetLastError(out lErrCode, out sErrMsg);

                                // Logger.LogInfo("DML FLAG:" + Dml + ":SAP EXIST FLAG:" + a.ToString() + "$" + lErrCode + ":" + sErrMsg + "Error Rec:" + CardCd);

                                return sErrMsg;
                            }
                            else
                            {
                                return sapmeg = "Success";
                            }

                        }
                        else
                        {
                            oActivity = ((SAPbobsCOM.Contacts)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oContacts)));
                            oActivity.CardCode = objact.Customercode;
                            if (objact.Activitytype == "Phone Call")
                            {
                                oActivity.Activity = SAPbobsCOM.BoActivities.cn_Conversation;

                            }
                            else if (objact.Activitytype == "Meeting")
                            {
                                oActivity.Activity = SAPbobsCOM.BoActivities.cn_Meeting;
                            }
                            else if (objact.Activitytype == "Task")
                            {
                                oActivity.Activity = SAPbobsCOM.BoActivities.cn_Task;
                            }
                            else if (objact.Activitytype == "Note")
                            {
                                oActivity.Activity = SAPbobsCOM.BoActivities.cn_Note;
                            }
                            else if (objact.Activitytype == "Campaign")
                            {
                                oActivity.Activity = SAPbobsCOM.BoActivities.cn_Campaign;
                            }
                            else
                            {
                                oActivity.Activity = SAPbobsCOM.BoActivities.cn_Other;
                            }

                            oActivity.ActivityType = -1;
                            oActivity.Phone = objact.mobileno;
                            //oActivity.Add();
                            a = false;
                            oActivity.UserFields.Fields.Item("U_User").Value = objact.Assignby;

                            using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                            {
                                conBase.Open();
                                using (SqlCommand cmdbase = new SqlCommand("select ISNULL(FirstName,'')+ISNULL(MiddleName,'')+ISNULL(LastName,'') Name from OHEm where Empid='" + objact.Assignempid + "' ", conBase))
                                //(select DocEntry from(select DocEntry, ROW_NUMBER() OVER (ORDER BY DocEntry DESC )  AS row_num, DocNum from ordr where DocNum = '" + objact.saleorder.ToString() + "') as bb  where bb.row_num = 1)
                                {

                                    using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                    {
                                        if (rdrbase.HasRows)
                                        {
                                            rdrbase.Read();
                                            Name = rdrbase["Name"].ToString();

                                        }

                                    }
                                }
                                conBase.Close();
                            }



                            oActivity.UserFields.Fields.Item("U_AssignBy").Value = Name;







                            //oActivity.ContactPersonCode  = 85;
                            oActivity.Details = objact.operatorcomments;
                            oActivity.Notes = objact.custcomments;
                            var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");

                            DateTime DT1 = new DateTime();
                            DT1 = DateTime.ParseExact(objact.Startdate, "dd/MM/yyyy", null);
                            oActivity.StartDate = DT1;
                            DateTime DT2 = new DateTime();
                            DT2 = DateTime.ParseExact(objact.enddate, "dd/MM/yyyy", null);
                            oActivity.EndDuedate = DT2;

                            oActivity.StartTime = Convert.ToDateTime(objact.starttimeto);
                            oActivity.EndTime = Convert.ToDateTime(objact.Endtime);

                            if (objact.status == "Close")
                            {
                                oActivity.Closed = BoYesNoEnum.tYES;
                            }
                            else if (objact.status == "Inactive")
                            {
                                oActivity.Inactiveflag = BoYesNoEnum.tYES;

                            }

                            if ((objact.saleorder == null) || (objact.saleorder.ToString() == ""))
                            {

                            }
                            else
                            {
                                oActivity.DocType = 17;


                                using (SqlConnection conBase = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["sqlconn"]))
                                {
                                    conBase.Open();
                                    using (SqlCommand cmdbase = new SqlCommand("(select DocEntry from(select DocEntry, ROW_NUMBER() OVER (ORDER BY DocEntry DESC )  AS row_num, DocNum from ordr where DocNum = '" + objact.saleorder.ToString() + "') as bb  where bb.row_num = 1)", conBase))
                                    //(select DocEntry from(select DocEntry, ROW_NUMBER() OVER (ORDER BY DocEntry DESC )  AS row_num, DocNum from ordr where DocNum = '" + objact.saleorder.ToString() + "') as bb  where bb.row_num = 1)
                                    {

                                        using (SqlDataReader rdrbase = cmdbase.ExecuteReader())
                                        {
                                            if (rdrbase.HasRows)
                                            {
                                                rdrbase.Read();
                                                BaseEntry = Convert.ToInt32(rdrbase["docentry"].ToString());

                                            }

                                        }
                                    }
                                    conBase.Close();
                                }


                                oActivity.DocEntry = BaseEntry.ToString();
                            }









                            //if (a == false)
                            lRetCode = oActivity.Add();
                            //else
                            //    lRetCode = oActivity.Update();
                            if (lRetCode == 0)
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(oActivity);
                            if (lRetCode != 0)
                            {
                                oCompany.GetLastError(out lErrCode, out sErrMsg);

                                // Logger.LogInfo("DML FLAG:" + Dml + ":SAP EXIST FLAG:" + a.ToString() + "$" + lErrCode + ":" + sErrMsg + "Error Rec:" + CardCd);

                                return sErrMsg;
                            }
                            else
                            {
                                return sapmeg = "Success";
                            }

                        }





                    }
                    else
                    {
                        return sapmeg = "No Records!";
                    }





                }
                else
                {
                    return sapmeg;

                }


            }
            catch (Exception e)
            {

                return e.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return sapmeg;
        }

    }
    public static class Log_file
    {
        public static string LogFileName
        {
            get;
            set;
        }
    }

}