using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarZon.Models;
using DarZon;


namespace DarZon.Controllers
{
    public class TailorassignmentSaveController : Controller
    {
        // GET: Tailorassignment
        DARZANTESTEntities db = new DARZANTESTEntities();
        [UserAuthenticationFilters]
        public ActionResult Tailorassignment()
        {
            Tailorassignment obj = new Tailorassignment();
            obj.Objdate = new Tailorassignmentdates();
            obj.objchilddata = new List<Tailorchilddata>();
            if (TempData["fromdate"] != null&& TempData["todate"]!=null)
            {
                obj.Objdate.fromdate = TempData["fromdate"].ToString();
                obj.Objdate.todate = TempData["todate"].ToString();
            }
            ViewBag.uname = Session["UserName"].ToString();
            var user = (from a in db.OHEMs join b in db.OHPS on a.position equals b.posID where b.name == "Production designer" select new { Value = a.firstName + a.middleName + a.lastName, Text = a.firstName + a.middleName + a.lastName }).ToList();
            ViewBag.userlist = user;
            if (TempData["datatlist"] == null)
            {

               

                ViewBag.ordelist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where b.U_mastertailor == null && b.U_ParentItem=="0" &&(((from f in db.Transportaionchilds where f.orderno==a.DocEntry select f.orderno).Contains(a.DocEntry) && (from x in db.Transportaionheads where ((from f in db.Transportaionchilds where f.orderno == a.DocEntry select f.Tno).Contains(x.Tno)) && x.Tstatus == "Close" select x.Tno).Count() >= 1) ||(from w in db.OWHS where w.WhsCode == a.U_WhsCode select w.U_main??"").ToList().FirstOrDefault()=="YES"  ) select new Tailorchilddata { saleorder = a.DocNum.ToString(), status = (b.U_status ?? "Notassigned"), lineno = b.LineNum, LabelNo = b.DocEntry.ToString() + "-" + a.DocNum.ToString() + "-" + b.LineNum.ToString(), itemcode = b.ItemCode, Description = b.Dscription, custname = a.CardName, schedulefordelivey = ((a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy")), Docentry = a.DocEntry, internaldeliveydate =(b.U_IDelDate ?? DateTime.Now).ToString("dd/MM/yyyy"), prdesigner = b.U_prdesigner, mastertailor = b.U_mastertailor, MPI = b.U_MPI, websaleoredr=a.U_WebSaleOrder }).ToList();
                obj.objchilddata = ViewBag.ordelist;
                return View(obj);

            }
            else
            {
                obj.objchilddata = (ICollection<Tailorchilddata>)TempData["datatlist"];
                return View(obj);
            }



        }
        //public ActionResult AllocationList(string fromdate, string todate)
        //{

        //    var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");
        //    DateTime frmdate = DateTime.ParseExact(todate, "dd/MM/yyyy", dutchCulture);
        //    DateTime tdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", dutchCulture);

        //    //var ordrlist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where a.DocDate >= frmdate && a.DocDate <= tdate && b.U_mastertailor == null && b.U_prdesigner == null select new Tailorchilddata { saleorder = a.DocNum.ToString(), Docentry = a.DocEntry, lineno = b.LineNum, schedulefordelivey = ((b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy")), internaldeliveydate = ((b.U_DelDate ?? DateTime.Now).ToString("dd/MM/yyyy")), custname = a.CardName }).ToList();
        //    var ordrlist = (from a in db.ORDRs.AsEnumerable() where a.DocDate >= frmdate && a.DocDate <= tdate && (from b in db.RDR1 where b.U_mastertailor == null && b.U_prdesigner == null select b.DocEntry).Contains(a.DocEntry) select new Tailorchilddata { saleorder = a.DocNum.ToString(), Docentry = a.DocEntry, custname = a.CardName }).ToList();
        //    return PartialView(ordrlist);


        //}
        public ActionResult tailorAddItem(string fromdate, string todate)
        {
            TempData["fromdate"] = fromdate;
            TempData["todate"] = todate;
            List<Tailorchilddata> objinvlist = new List<Tailorchilddata>();

            //ViewBag.Itemcategory = category;
            var user = (from a in db.OHEMs join b in db.OHPS on a.position equals b.posID where b.name == "Production designer" select new { Value = a.firstName + a.middleName + a.lastName, Text = a.firstName + a.middleName + a.lastName }).ToList();
            ViewBag.userlist = user;
            var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");
            DateTime frmdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", dutchCulture);
            DateTime tdate = DateTime.ParseExact(todate, "dd/MM/yyyy", dutchCulture);

            objinvlist = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where a.DocDate >= frmdate && a.DocDate <= tdate && b.U_ParentItem == "0" && b.U_mastertailor == null && ((from f in db.Transportaionchilds where f.orderno == a.DocEntry select f.orderno).Contains(a.DocEntry) || (from w in db.OWHS where w.WhsCode == a.U_WhsCode select w.U_main ?? "").ToList().FirstOrDefault() == "Yes") select new Tailorchilddata { saleorder = a.DocNum.ToString(), LabelNo = b.DocEntry.ToString() + "-" + a.DocNum.ToString() + "-" + b.LineNum.ToString(), status =(b.U_status ?? "Notassigned"), lineno = b.LineNum, itemcode = b.ItemCode, Description = b.Dscription, custname = a.CardName, schedulefordelivey = ((a.DocDate ?? DateTime.Now).ToString("dd/MM/yyyy")), Docentry = a.DocEntry, internaldeliveydate = (b.U_IDelDate ?? DateTime.Now).ToString("dd/MM/yyyy"), prdesigner = b.U_prdesigner, mastertailor = b.U_mastertailor, MPI = b.U_MPI,websaleoredr=a.U_WebSaleOrder }).ToList();
            TempData["datatlist"] = objinvlist;
            return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult ReAllocationList(string fromdate, string todate)
        //{
        //    var dutchCulture = System.Globalization.CultureInfo.CreateSpecificCulture("nl-NL");
        //    DateTime frmdate = DateTime.ParseExact(todate, "dd/MM/yyyy", dutchCulture);
        //    DateTime tdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", dutchCulture);
        //    var ordrlist = (from a in db.ORDRs.AsEnumerable() where a.DocDate >= frmdate && a.DocDate <= tdate && (from b in db.RDR1 where b.U_mastertailor != null && b.U_prdesigner != null select b.DocEntry).Contains(a.DocEntry) select new Tailorchilddata { saleorder = a.DocNum.ToString(), Docentry = a.DocEntry, custname = a.CardName }).ToList();
        //    //b.U_mastertailor!=null && b.U_prdesigner!=null
        //    return PartialView(ordrlist);
        //}
        public ActionResult ReallAddItem(string Docnum, string fromdate, string todate)
        {
            TempData["fromdate"] = fromdate;
            TempData["todate"] = todate;
            List<Tailorchilddata> objinvlist = new List<Tailorchilddata>();

           
            return Json(new { value = "Success" }, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("Tailorassignment")]
        [onAction(ButtonName = "Save")]
        public ActionResult Save(Tailorassignment objinvlist)
        {
            foreach (var items in  objinvlist.objchilddata)
            {
                RDR1 objrdr = (from a in db.ORDRs.AsEnumerable() join b in db.RDR1.AsEnumerable() on a.DocEntry equals b.DocEntry where a.DocEntry == items.Docentry && b.LineNum == items.lineno select b).FirstOrDefault();
                //  objrdr.U_status = items.status;
                // objrdr.LineNum = items.lineno;
                objrdr.U_mastertailor = items.mastertailor;
                objrdr.U_prdesigner = items.prdesigner;
             

                if (objrdr.U_mastertailor == null)
                {
                    objrdr.U_status = "Notassigned";
                }
                else
                {
                    objrdr.U_status = "Assigned";
                }
                // db.RDR1.(a => a.ItemCode == objrdr.ItemCode);
                db.Entry(objrdr).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();



            }


            return RedirectToAction("Tailorassignment", "TailorassignmentSave");
        }


        public JsonResult masterdata(string prname)
        {

            int managerid = (from e in db.OHEMs where (e.firstName + e.middleName + e.lastName) == prname select e.empID).FirstOrDefault();
            var user = (from a in db.OHEMs join b in db.OHPS on a.position equals b.posID where a.manager == managerid select new { Value = a.firstName + a.middleName + a.lastName, Text = a.firstName + a.middleName + a.lastName }).ToList();
            
            return Json(new { value = user }, JsonRequestBehavior.AllowGet);



        }
    }
}