using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
using DarZon.DAL;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace DarZon.Controllers
{
    public class TransportationController : Controller
    {
        // GET: Transportation
        DARZANTESTEntities db = new DARZANTESTEntities();
        [UserAuthenticationFilters]
        public ActionResult Index(Orderlistmaster objorder)
        {
            ViewBag.print = "";
            if (TempData["Transdata"] != null)
            {
                objorder = (Orderlistmaster)TempData["Transdata"];
            }
           
            if(TempData["optverification"]!=null)
            {
                ViewBag.optverify = "NO";
            }
            else
            {
                ViewBag.optverify = "YES";
            }


            if (objorder.objsaleorderHeader == null)
            {
                objorder = new Orderlistmaster();
                objorder.objsaleorderHeader = new Orderheader();
                int MaxID = 0;
                if ((from emp in db.Transportaionheads select emp).Count() > 0)
                {
                   MaxID = (from emp in db.Transportaionheads select emp.Tno).Max();
                }
                    string date = DateTime.Now.ToString("dd/MM/yyyy");
                objorder.objsaleorderHeader.Tdate = date;
        
                objorder.objsaleorderHeader.Tno = MaxID +1;
                objorder.objsaleorderHeader.Tstatus = "Open-1";

            }
            string uname = Session["UserName"].ToString();
            var userlist = (from a in db.OHEMs.AsEnumerable() join b in db.OHPS on a.position equals b.posID where b.name== "Pick up" select new { Value = a.firstName + a.middleName + a.lastName, Text =a.empID.ToString()+"-"+ a.firstName + a.middleName + a.lastName }).ToList();
            var whscode = (from a in db.OHEMs where a.U_EMPCODE == uname select  a.U_WareH ).FirstOrDefault();
            var whsto = (from a in db.OWHS where a.WhsCode!= whscode select new { Value = a.WhsName, Text = a.WhsName }).ToList();
            // xs
            var whsfrom = (from a in db.OWHS where a.WhsCode == whscode select new { name = a.WhsName, main = a.U_main }).FirstOrDefault();

            ViewBag.userid = userlist;
            ViewBag.whsfrom = whsfrom.name;
           ViewBag.main= whsfrom.main;
            if (whsfrom.main == "YES")
            {
                ViewBag.whsto = whsto;
            }
            else
            {
               var outlet= (from a in db.OWHS where a.U_main=="Yes" select a.WhsName ).FirstOrDefault();

               if((string)TempData["optverification"] != "No")
                objorder.objsaleorderHeader.outletto = outlet;

            }

            ViewBag.whhscode = whscode;
            // ViewBag.usercode= Session["Usercode"].ToString();
            if (TempData["optverification"] == null)
                objorder.objsaleorderHeader.Outletfrom = whsfrom.name;
          // objsaleorder.objsaleorderHeader.DocEntry = sequenceno.NumberPreix + "-" + sequenceno.Number.ToString();
          TempData["wharehouse"] = whscode;
            objorder.objsaleorderHeader.UserID = Session["UserName"].ToString();
            ViewBag.status = (new List<SelectListItem>
{

  
     new SelectListItem { Selected = false, Text = "Close", Value = "Close"},

});

            string whcode = (string)Session["WhareHouse"];
            string whername = (from a in db.OWHS where a.WhsCode == whcode select a.WhsName).FirstOrDefault();
            var transhead = (from a in db.Transportaionheads.AsEnumerable() where a.Tstatus == "Open" && a.userid!= Session["UserName"].ToString() && a.outletto== whername select a).ToList();
            ViewBag.transhead = transhead;

            return View(objorder);

        }
        public PartialViewResult AddItem(string Orderno, int Qty, string Schedulefordelivery,decimal AddQty,string websaleorder)
        {

            Orderlistchild objsaleorder = new Orderlistchild();
            objsaleorder.Orderno = Orderno;
            objsaleorder.Qty = Qty;
            objsaleorder.Addonqty = AddQty;
            // string date= Schedulefordelivery.ToString("dd/MM/yyyy");
            //ViewBag.Itemcategory = category;
            objsaleorder.websaleorder = websaleorder;
            objsaleorder.Schedulefordelivery = Schedulefordelivery;
            return PartialView("AddItem", objsaleorder);
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("Index")]
        [onAction(ButtonName = "Save")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Save(Orderlistmaster orderuserdeatails)
        {

            if (orderuserdeatails.objchild != null)
            {
                Transportaionhead objhead = new Transportaionhead();

                int cnthead = (from a in db.Transportaionheads.AsEnumerable()
                               where a.Tno == orderuserdeatails.objsaleorderHeader.Tno
                               select a.Tno).Count();
              
                if (cnthead != 0)
                {
                    Transportaionhead objrdr = (from a in db.Transportaionheads.AsEnumerable() where a.Tno == orderuserdeatails.objsaleorderHeader.Tno select a).FirstOrDefault();
                  //  TempData["tno"] = objrdr.Tno;
                    objrdr.Tno = orderuserdeatails.objsaleorderHeader.Tno;
                    objrdr.Tstatus = orderuserdeatails.objsaleorderHeader.Tstatus;
                    db.Entry(objrdr).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    foreach (var items in orderuserdeatails.objchild)
                    {
                      var docentry = (from a in db.ORDRs.AsEnumerable() where a.DocNum == int.Parse(items.Orderno) select a.DocEntry).FirstOrDefault();
                   
                    var tranordr = (from a in db.RDR1.AsEnumerable() where a.U_TrailStatus == "Yes" && a.DocEntry == docentry && (a.U_TStatus == "No") && a.U_TNo == orderuserdeatails.objsaleorderHeader.Tno.ToString() select a).ToList();

                    
                        foreach (RDR1 item in tranordr)
                        {
                            item.U_TNo = orderuserdeatails.objsaleorderHeader.Tno.ToString();
                            item.U_TStatus = "Yes";
                            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                }
                else
                {

                    objhead.Tno = orderuserdeatails.objsaleorderHeader.Tno;
                   // TempData["tno"] = objhead.Tno;
                    // objhead.userid =orderuserdeatails.objsaleorderHeader.UserID;



                    var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");
                    DateTime tdate = DateTime.ParseExact(orderuserdeatails.objsaleorderHeader.Tdate, "dd/MM/yyyy", null);
                    objhead.userid= orderuserdeatails.objsaleorderHeader.UserID;
                    var username = (from z in db.OHEMs where z.U_EMPCODE == objhead.userid select z).FirstOrDefault();
                    objhead.Username = username.firstName+ username.middleName + username.lastName;
                    objhead.Tdate = tdate;
                    objhead.outletfrom = orderuserdeatails.objsaleorderHeader.Outletfrom;
                    objhead.outletto = orderuserdeatails.objsaleorderHeader.outletto;
                    objhead.MeasurmentsDetails = orderuserdeatails.objsaleorderHeader.MeasurmentDeatails;
                    objhead.Tstatus = orderuserdeatails.objsaleorderHeader.Tstatus;
                    objhead.Pickupuser = orderuserdeatails.objsaleorderHeader.pickupuser;
                    db.Transportaionheads.Add(objhead);
                    db.SaveChanges();
                    foreach (var items in orderuserdeatails.objchild)
                    {
                        Transportaionchild objchild = new Transportaionchild();
                        string orno = items.Orderno;
                        //int cnt = (from a in db.Transportaionchilds.AsEnumerable()
                        //           where a.orderno == int.Parse(orno)
                        //           select a.orderno).Count();
                        
                            objchild.orderno = Convert.ToInt32(orno);
                            objchild.Remarks = items.Remarks;
                       
                            //objchild.Reschedule = items.Reschedule;
                            //objchild.Schedulefordelivery = items.Schedulefordelivery.ToString();
                            objchild.Qty = items.Qty;
                            objchild.Addonqty = int.Parse(items.Addonqty.ToString());

                            objchild.Tno = orderuserdeatails.objsaleorderHeader.Tno;
                            db.Transportaionchilds.Add(objchild);
                            db.SaveChanges();
                        var docentry = (from a in db.ORDRs.AsEnumerable() where a.DocNum == int.Parse(items.Orderno) select a.DocEntry).FirstOrDefault();
                        var orderlist = (from a in db.RDR1 where a.U_TrailStatus == "Yes" && a.DocEntry == docentry && a.U_TStatus==null select a).ToList();
                       // var tranordr = (from a in db.RDR1 where a.U_TrailStatus == "Yes" && a.DocEntry == docentry && a.U_TStatus == "No" select a).ToList();

                      //  objchild.Reschedule = "Print";
                        foreach (RDR1 item in orderlist)
                        {
                            item.U_TNo = orderuserdeatails.objsaleorderHeader.Tno.ToString();
                            item.U_TStatus = "No";
                            db.Entry(item).State= System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                      

                        

                    }

                }
                //return Content("<script language='javascript' type='text/javascript'>alert('Trail Update Successfully');this.load();window.open('../Transportation/PublicAreaReport','_blank');</script>");
                // return RedirectToAction("PublicAreaReport", "Transportation");


                string uname = Session["UserName"].ToString();
                var userlist = (from a in db.OHEMs.AsEnumerable() select new { Value = a.firstName + a.middleName + a.lastName, Text =a.empID.ToString()+"-"+ a.firstName + a.middleName + a.lastName }).ToList();
                var whscode = (from a in db.OHEMs where a.U_EMPCODE == uname select a.U_WareH).FirstOrDefault();
                var whsto = (from a in db.OWHS where a.WhsCode != whscode select new { Value = a.WhsName, Text = a.WhsName }).ToList();
                // xs
                var whsfrom = (from a in db.OWHS where a.WhsCode == whscode select new { Value = a.WhsName, Text = a.WhsName }).ToList();
                ViewBag.userid = userlist;
                ViewBag.whsfrom = whsfrom;
                ViewBag.whsto = whsto;
                ViewBag.whhscode = whscode;
                // ViewBag.usercode= Session["Usercode"].ToString();

                // objsaleorder.objsaleorderHeader.DocEntry = sequenceno.NumberPreix + "-" + sequenceno.Number.ToString();
                TempData["wharehouse"] = whscode;
               // objorder.objsaleorderHeader.UserID = Session["UserName"].ToString();
                ViewBag.status = (new List<SelectListItem>
{

    new SelectListItem { Selected = false, Text = "InTrasit", Value = "InTrasit"},
     new SelectListItem { Selected = false, Text = "Close", Value = "Close"},

});
                //  return Content("<script language='javascript' type='text/javascript'>alert('Data Update Successfully');$('#Savedetails').modal('show');window.location ='../Transportation/Index'</script>");
                //  orderuserdeatails.objsaleorderHeader.orderno = "Print";
                //TempData["Transdata"] = orderuserdeatails;

                return Content("<script language='javascript' type='text/javascript'>alert('Logistics details has been submitted');window.location ='../Transportation/Index'</script>");


            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('No Records Exists');window.location ='../Transportation/Index'</script>");
               
            }
                //  return View();

            //  return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult PublicAreaReport()
        //{
        //    int tno = int.Parse(TempData["tno"].ToString());
        //    LocalReport lr = new LocalReport();
        //    string path = Path.Combine(Server.MapPath("~/DarzanReport/Transpotation.rdlc"));
        //    if (System.IO.File.Exists(path))
        //    {
        //        lr.ReportPath = path;
        //    }
        //    else
        //    {
        //        lr.ReportPath = path;
        //    }


        //    using (db = new DARZANTESTEntities())
        //    {
        //        //   int dno =Convert.ToInt32(TempData["delno"]);
        //        var orderlist = (from a in db.Transportaionheads.AsEnumerable()
        //                         join b in db.Transportaionchilds.AsEnumerable() on a.Tno equals b.Tno
        //                         where a.Tno == tno


        //                         select new
        //                         {
        //                             b.orderno,
        //                             Pickupuser = a.Pickupuser,
        //                             a.outletfrom,
        //                             outletto = a.outletto,
        //                             Qty = b.Qty,
        //                             addonqty=b.Addonqty,
        //                             Tdate = a.Tdate,
        //                             Tno = a.Tno,a.Username,
        //                             Tstatus = a.Tstatus


        //                         }).ToList();
        //        ReportDataSource rd = new ReportDataSource("salehead", orderlist);
        //        lr.DataSources.Add(rd);
        //        var orderlist1 = (from a in db.Transportaionheads.AsEnumerable()
        //                          join b in db.Transportaionchilds.AsEnumerable() on a.Tno equals b.Tno
        //                          where a.Tno == tno
        //                          select new
        //                          {
        //                              b.orderno,
        //                              a.Pickupuser,
        //                              a.outletfrom,
        //                              a.outletto,
        //                              b.Qty,
        //                              Tdate = a.Tdate,
        //                              Tno = a.Tno,a.Username,a.Tstatus,
        //                              ItemDescription = (from c in db.SaleOrderDetails select c.ItemDescription)

        //                          }).ToList();
        //        ReportDataSource rd1 = new ReportDataSource("salechild", orderlist1);
        //        lr.DataSources.Add(rd1);

        //        var orderlist2 = (from a in db.Transportaionheads
        //                          join b in db.Transportaionchilds on a.Tno equals b.Tno
        //                          where a.Tno == tno

        //                          select new
        //                          {
        //                              b.orderno,
        //                              a.Pickupuser,
        //                              a.outletfrom,
        //                              a.outletto,
        //                              b.Qty,
        //                              Tdate = a.Tdate,
        //                              Tno = a.Tno,a.Username,
        //                              a.Tstatus
        //                          }).ToList();
        //        ReportDataSource rd2 = new ReportDataSource("Transchild", orderlist2);
        //        lr.DataSources.Add(rd2);
        //        var orderlist3 = (from a in db.Transportaionheads
        //                          join b in db.Transportaionchilds on a.Tno equals b.Tno
        //                          where a.Tno == tno

        //                          select new
        //                          {
        //                              b.orderno,
        //                              a.Pickupuser,
        //                              a.outletfrom,
        //                              a.outletto,
        //                              b.Qty,
        //                              Tdate = a.Tdate,
        //                              Tno = a.Tno,
        //                              a.Username,
        //                              a.Tstatus

        //                          }).ToList();
        //        ReportDataSource rd3 = new ReportDataSource("Transhead", orderlist3);
        //        lr.DataSources.Add(rd3);


        //        string reporType = "PDF";
        //        string mimetypr;
        //        string encodeing;
        //        string filenameexns;
        //        string deviceinfo = "<DeviceInfo>" + "<OutputFormat>PDF</OutputFormat>" +
        //            "<PageWidth>8.5in</PageWidth>" + "<PageHeight>8.5in</PageHeight>" +
        //            "<MarginTop>0.5in</MarginTop>" + "<MarginLeft>lin</MarginLeft>" + "<MarginRight>lin</MarginRight>"
        //            + "<MarginButtom>0.5in</MarginButtom>" + "</DeviceInfo>";
        //        Warning[] warnings;
        //        string[] streams;
        //        byte[] renderedbytes;
        //        renderedbytes = lr.Render(reporType, deviceinfo, out mimetypr, out encodeing, out filenameexns, out streams, out warnings);

        //        return File(renderedbytes, mimetypr);

        //    }

        //}

        [HttpPost]
        [AllowAnonymous]
        [ActionName("Index")]
        [onAction(ButtonName = "Print")]

        public ActionResult PublicAreaReport(Orderlistmaster orderuserdeatails)
        {
            using (db = new DARZANTESTEntities())
            {
                List<Logisticprint> orderlist = new List<Logisticprint>();
                LogisticPrintHead headprint = new LogisticPrintHead();
                headprint.outletfrom = orderuserdeatails.objsaleorderHeader.Outletfrom;
                headprint.outletto = orderuserdeatails.objsaleorderHeader.outletto;
                headprint.Username = orderuserdeatails.objsaleorderHeader.UserID;
                // headprint.Outletfrom = orderuserdeatails.objsaleorderHeader.Outletfrom;
                headprint.Pickupuser = orderuserdeatails.objsaleorderHeader.pickupuser;
                headprint.Tno = orderuserdeatails.objsaleorderHeader.Tno;
                headprint.Tstatus = orderuserdeatails.objsaleorderHeader.Tstatus;
                List<LogisticPrintHead> objheaderlist = new List<LogisticPrintHead>();
                objheaderlist.Add(headprint);

                LocalReport lr = new LocalReport();
                string path;

                foreach (var item in orderuserdeatails.objchild)
                {
                    int tstatus = (from or in db.ORDRs.AsEnumerable() join t in db.RDR1 on or.DocEntry equals t.DocEntry where or.DocNum == int.Parse(item.Orderno) && t.U_TrailStatus == "Yes" select t.U_TrailStatus).ToList().Count();
                    //var tnos = (from or in db.Transportaionheads.AsEnumerable() join t in db.Transportaionchilds on or.Tno equals t.Tno where or.Tno == int.Parse() &&  == "Yes" select t.LineNum).ToList();

                    if (tstatus >=1)
                    {
                        string wherecode = (string)Session["WhareHouse"];
                        //from head office to Store and strore recive
                        var checkmain = (from a in db.OWHS where a.WhsCode == wherecode select a.U_main).FirstOrDefault();

                        var readylist = new List<int>();
                        if (checkmain == "YES")
                        {
                             readylist = (from or in db.ORDRs.AsEnumerable() join t in db.RDR1.AsEnumerable() on or.DocEntry equals t.DocEntry where or.DocNum == int.Parse(item.Orderno) && t.U_TrailStatus == "Yes" && t.U_TStatus == null && (t.U_TNo??"")=="" select t.LineNum).ToList();
                        }
                        else
                        {
                            readylist = (from or in db.ORDRs.AsEnumerable() join t in db.RDR1.AsEnumerable() on or.DocEntry equals t.DocEntry where or.DocNum == int.Parse(item.Orderno) && t.U_TrailStatus == "Yes" && t.U_TStatus == "No" && t.U_TNo== headprint.Tno.ToString() select t.LineNum).ToList();
                        }
                        // LocalReport lr = new LocalReport();
                        path = Path.Combine(Server.MapPath("~/DarzanReport/Transheadtostore.rdlc"));
                        if (System.IO.File.Exists(path))
                        {
                            lr.ReportPath = path;
                        }
                        else
                        {
                            lr.ReportPath = path;
                        }
                         List<Logisticprint> objchild = (from a in db.LabelDetails.AsEnumerable()    where a.DocNum==item.Orderno && a.MainlabelNo!="" && readylist.Contains(int.Parse(a.Lbno)) select new Logisticprint { DocNum = a.DocNum, MainQty = a.MainQty, AddonQty = a.AddonQty, Fabric = a.Fabric, MainlabelNo = a.MainlabelNo,Lbno=a.Lbno, WebSO = a.WebSO,Id=a.Id}).ToList();
                        orderlist.AddRange(objchild);
                    }
                    else
                    {
                        var readylist = (from or in db.ORDRs.AsEnumerable() join t in db.RDR1.AsEnumerable() on or.DocEntry equals t.DocEntry where or.DocNum == int.Parse(item.Orderno)  select t.LineNum).ToList();
                        path = Path.Combine(Server.MapPath("~/DarzanReport/Transpotation.rdlc"));
                        if (System.IO.File.Exists(path))
                        {
                            lr.ReportPath = path;
                        }
                        else
                        {
                            lr.ReportPath = path;
                        }
                        List<Logisticprint> objchild = (from a in db.LabelDetails.AsEnumerable()  where a.DocNum == item.Orderno select new Logisticprint { DocNum = a.DocNum, MainQty = a.MainQty, AddonQty = a.AddonQty, Fabric = a.Fabric, LabelNo = a.LabelNo, Lbno=a.Lbno, WebSO = a.WebSO,Id=a.Id }).ToList();

                        orderlist.AddRange(objchild);
                    }
                   
                }
                List<Logisticprint> finalorderlist = orderlist.OrderBy(a => a.Id).ToList();

                ReportDataSource rd = new ReportDataSource("salehead", objheaderlist);
                lr.DataSources.Add(rd);
                ReportDataSource rd1 = new ReportDataSource("salechild", objheaderlist);
                lr.DataSources.Add(rd1);
                ReportDataSource rd2 = new ReportDataSource("Transchild", objheaderlist);
                lr.DataSources.Add(rd2);
                ReportDataSource rd3 = new ReportDataSource("Transhead", objheaderlist);
                lr.DataSources.Add(rd3);
                ReportDataSource rd4 = new ReportDataSource("LabelDetails", finalorderlist);
                lr.DataSources.Add(rd4);
                string reporType = "PDF";
                string mimetypr;
                string encodeing;
                string filenameexns;
                string deviceinfo = "<DeviceInfo>" + "<OutputFormat>PDF</OutputFormat>" +
                    "<PageWidth>8.5in</PageWidth>" + "<PageHeight>8.5in</PageHeight>" +
                    "<MarginTop>0.5in</MarginTop>" + "<MarginLeft>lin</MarginLeft>" + "<MarginRight>lin</MarginRight>"
                    + "<MarginButtom>0.5in</MarginButtom>" + "</DeviceInfo>";
                Warning[] warnings;
                string[] streams;
                byte[] renderedbytes;
                renderedbytes = lr.Render(reporType, deviceinfo, out mimetypr, out encodeing, out filenameexns, out streams, out warnings);

                return File(renderedbytes, mimetypr);

            }




        }

        public ActionResult findtransportation(int? id)
        {

            Orderlistmaster orderuserdeatails = new Orderlistmaster();
            orderuserdeatails.objsaleorderHeader = new Orderheader();
            orderuserdeatails.objchild = new List<Orderlistchild>();
            Transportaionhead objhead = (from a in db.Transportaionheads where a.Tno == id select a).FirstOrDefault();
            orderuserdeatails.objsaleorderHeader.Tno = objhead.Tno;
            // objhead.userid =orderuserdeatails.objsaleorderHeader.UserID;
            orderuserdeatails.objsaleorderHeader.Tdate = (objhead.Tdate ?? DateTime.Now).ToString("dd/MM/yyyy");
            orderuserdeatails.objsaleorderHeader.Outletfrom = objhead.outletfrom;
            orderuserdeatails.objsaleorderHeader.outletto = objhead.outletto;
            orderuserdeatails.objsaleorderHeader.MeasurmentDeatails = objhead.MeasurmentsDetails;
            orderuserdeatails.objsaleorderHeader.Tstatus = objhead.Tstatus;
            orderuserdeatails.objsaleorderHeader.pickupuser = objhead.Pickupuser;

            List<Transportaionchild> objchlist = (from a in db.Transportaionchilds where a.Tno == id select a).ToList();
            foreach (var items in objchlist)
            {
                Orderlistchild objchild = new Orderlistchild();
                string orno = items.orderno.ToString();


                objchild.Orderno = items.orderno.ToString();
                objchild.Remarks = items.Remarks;
                objchild.Reschedule = items.Reschedule;
                objchild.websaleorder = (from d in db.ORDRs where d.DocNum == (items.orderno ?? 0) select d.U_WebSaleOrder).FirstOrDefault();
                //  objchild.Schedulefordelivery = items.Schedulefordelivery.ToString();
                objchild.Qty = items.Qty ?? 0;
                objchild.Addonqty = decimal.Parse(items.Addonqty.ToString());
                orderuserdeatails.objchild.Add(objchild);



            }
            TempData["Transdata"] = orderuserdeatails;
            TempData["optverification"] = "No";

            return RedirectToAction("Index", "Transportation");



        }

        public JsonResult findphonenumber(int empid)
        {

            var phonenumber = (from a in db.OHEMs where a.empID == empid select a.mobile).FirstOrDefault();

            return Json(new { value = phonenumber }, JsonRequestBehavior.AllowGet);
        }

       




    }
}