using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Data;
using Design_and_Supervion_Issue_Tracking.Models;
using System.IO;

namespace Design_and_Supervion_Issue_Tracking.Controllers
{
    public class AcceptanceController : Controller
    {
        Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider issuedb = new Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider();
        string Error = "";
        public ActionResult Index()
        {
            try
            {
                DataTable dt = new DataTable();

                dt = issuedb.ExecuteDataTable("dbo", "GetAcceptance", ref Error);

                ViewData.Model = dt.AsEnumerable();

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }

        }
        public ActionResult Edit(int id)
        {


            DataTable dt = new DataTable();
            ViewBag.tenderdocnumber = issuedb.combofill("dbo", "GetTender", "TenderDocNumber", "id", ref Error);
            var param = new ArrayList()
            {
                "@id"
            };
            var values = new ArrayList() {
                id
            };
            dt = issuedb.ExecuteDataTable("dbo", "GetAcceptancebyId", param, values, ref Error);

            if (dt.Rows.Count == 1)
            {

                // tblSLA.id, tblSLA.SLAnumber,tblSLA.attachement,tblSLA.dateofsigned,tblSLA.SentTo,PMSProject.ProjectTitle,,PMSProject.Id as projectid

                ModelAcceptance issuem = new ModelAcceptance()
                {
                    id = Convert.ToInt32(dt.Rows[0][0].ToString()),
                   // tenderno = Convert.ToInt32(dt.Rows[0][1].ToString()),
                    tenderdocnumber = Convert.ToString(dt.Rows[0][1].ToString()),
                    AcceptanceLetterno = Convert.ToString(dt.Rows[0][3].ToString()),
                    attachement = Convert.ToString(dt.Rows[0][5].ToString())

                };
                Session["attachement"] = Convert.ToString(dt.Rows[0][5].ToString());

                ViewBag.Selectedtender = Convert.ToString(dt.Rows[0][2].ToString());
                //  ViewBag.SelResponsible_Personnel = Convert.ToString(dt.Rows[0][6].ToString());

                return View(issuem);
            }
            else
            {
                return RedirectToAction("Index", TempData["tenderid"]);
            }

        }
        [HttpPost]
        public ActionResult Edit(ModelAcceptance obj)
        {
            HttpFileCollectionBase file = Request.Files;
            bool a = false;
            //obj.Project_Id= issuedb.combofill("dbo", "getProject", "ProjectName", "Project_Id", ref Error)
            if (file[0].FileName != "")
            {
                string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));
                file[0].SaveAs(path);
                obj.attachement = "~/img/" + Path.GetFileName(file[0].FileName);


            }



            var errors = ModelState
              .Where(x => x.Value.Errors.Count > 0)
              .Select(x => new { x.Key, x.Value.Errors })
              .ToArray();
            if (ModelState.IsValid)
            {
                var param = new ArrayList()
            {
           "@id","@tenderno","@tenderdocnumber","@AcceptanceLetterno","@attachement"


                };
                var values = new ArrayList()
            {
                  obj.id,obj.tenderdocnumber,Convert.ToString(Request["tenderno"].ToString()),obj.AcceptanceLetterno,obj.attachement

            };


                a = issuedb.ExecuteNonQuery("dbo", "UpdateAcceptance", param, values, ref Error);
            }
            if (a)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.msg = "<script>alert('update failed') </script>";
                ModelState.Clear();

            }
            return View();
        }


        public ActionResult Create()
        {
            ViewBag.tenderdocnumber = issuedb.combofill("dbo", "GetTender", "TenderDocNumber", "id", ref Error);
            return View();

        }
        [HttpPost]
        public ActionResult Create(ModelAcceptance obj)
        {
            HttpFileCollectionBase file = Request.Files;
            bool a = false;

            int tender = Convert.ToInt32(TempData["tenderid"]);


            if (file[0].FileName != "")
            {
                string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));

                file[0].SaveAs(path);
                obj.attachement = "~/img/" + Path.GetFileName(file[0].FileName);
            }
            else
            {
                obj.attachement = "-";
            }
            var errors = ModelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => new { x.Key, x.Value.Errors })
               .ToArray();
            if (ModelState.IsValid)
            {
                var param = new ArrayList()
            {
            "@tenderno",
            "@tenderdocnumber",
            "@AcceptanceLetterno",
            "@date",
            "@attachement"

            };

                var values = new ArrayList()
            {

                    obj.tenderdocnumber,
                    Convert.ToString(Request["tenderno"].ToString()),
                    obj.AcceptanceLetterno,
                    DateTime.Now,
                    obj.attachement


            };
                a = issuedb.ExecuteNonQuery("dbo", "AddAcceptanceLetter", param, values, ref Error);
               
            }
            return View();
        }
    }
}