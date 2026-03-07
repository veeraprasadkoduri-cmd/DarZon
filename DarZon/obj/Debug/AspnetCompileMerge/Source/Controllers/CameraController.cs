using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using context = System.Web.HttpContext;
using DarZon.Models;
using System.Web;

namespace DarZon.Controllers
{
    public class CameraController : Controller
    {
        DARZANTESTEntities db = new DARZANTESTEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Capture()
        {
            string username = (string)Session["UserName"];
            var stream = Request.InputStream;
            string dump;
            var saleheader = (from a in db.SaleOrderHeaders.AsEnumerable() where (a.status == "O" || a.status == "P" || a.status == "R") && a.UserName == username select a).LastOrDefault();
         
            int picno = saleheader.Series??0 ;
            picno = picno + 1;

            saleheader.Series = picno;
            db.Entry(saleheader).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            using (var reader = new StreamReader(stream))
            {
                dump = reader.ReadToEnd();

                DateTime nm = DateTime.Now;

                string date = nm.ToString("yyyymmddMMss");
                string subpath= saleheader.DocEntry.ToString() + "_" + picno.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyhhmmsstt")+".jpg";
                var path = Server.MapPath("~/App_Data/") + subpath; 
                System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));

                ViewData["path"] = subpath;

                Session["pathval"] = subpath;
            }
           

            return View("Index");
        }


        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;

            byte[] bytes = new byte[numBytes];

            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }

            return bytes;
        }

        private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;

        public static void SendErrorToText(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;

            ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            Errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();
            exurl = context.Current.Request.Url.ToString();
            ErrorLocation = ex.Message.ToString();

            try
            {
                string filepath = context.Current.Server.MapPath("~/ExceptionDetailsFile/");  //Text File Path

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);

                }
                filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                if (!System.IO.File.Exists(filepath))
                {


                    System.IO.File.Create(filepath).Dispose();

                }
                using (StreamWriter sw = System.IO.File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                e.ToString();

            }
        }

        public JsonResult Rebind()
        {
            string path = Session["pathval"].ToString();
            return Json(path, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            string username = (string)Session["UserName"];
            var stream = Request.InputStream;
            string dump;
            var saleheader = (from a in db.SaleOrderHeaders.AsEnumerable() where (a.status == "O" || a.status == "P" || a.status == "R") && a.UserName == username select a).LastOrDefault();

            int picno = saleheader.Series ?? 0;
            picno = picno + 1;

            saleheader.Series = picno;
            db.Entry(saleheader).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        DateTime nm = DateTime.Now;

                        string date = nm.ToString("yyyymmddMMss");
                        string subpath = saleheader.DocEntry.ToString() + "_" + picno.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyhhmmsstt")+"_" + fname;
                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/App_Data/"), subpath);
                        file.SaveAs(fname);

                        ViewData["path"] = subpath;

                        Session["pathval"] = subpath;
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }


    }
}