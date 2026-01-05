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
    public class SLAController : Controller
    {
        Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider issuedb = new Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider();
        string Error = "";
        public ActionResult Index()
        {
            try
            {
                DataTable dt = new DataTable();

                dt = issuedb.ExecuteDataTable("dbo", "GetSLA", ref Error);

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
           // ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
            ViewBag.Projectno = issuedb.combofill("dbo", "getProject", "ProjectName", "Project_Id", ref Error);
            ViewBag.SentTo = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
            //ViewBag.FromDept = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);

            DataTable dt = new DataTable();

            var param = new ArrayList()
            {
                "@id"
            };
            var values = new ArrayList() {
                id
            };
            dt = issuedb.ExecuteDataTable("dbo", "GetSLAbyId", param, values, ref Error);

            if (dt.Rows.Count == 1)
            {

               // tblSLA.id, tblSLA.SLAnumber,tblSLA.attachement,tblSLA.dateofsigned,tblSLA.SentTo,PMSProject.ProjectTitle,,PMSProject.Id as projectid

                ModelSLA issuem = new ModelSLA()
                {
                    id = Convert.ToInt32(dt.Rows[0][0].ToString()),
                    Projectno = Convert.ToInt32(dt.Rows[0][7].ToString()),
                    SLAnumber = Convert.ToString(dt.Rows[0][1].ToString()),
                    SentTo = Convert.ToString(dt.Rows[0][4].ToString()),
                    attachement = Convert.ToString(dt.Rows[0][2].ToString())

                };
                Session["attachement"] = Convert.ToString(dt.Rows[0][2].ToString());

                ViewBag.SelectedProject = Convert.ToString(dt.Rows[0][3].ToString());
                ViewBag.SelResponsible_Personnel = Convert.ToString(dt.Rows[0][6].ToString());

                return View(issuem);
            }
            else
            {
                return RedirectToAction("Index", TempData["tenderid"]);
            }

        }
        [HttpPost]
        public ActionResult Edit(ModelLegal obj)
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
               "@id","@RequestType","@attachement","@EmployeeId","@Responsible_Personnel","@Remark","@status"

                };
                var values = new ArrayList()
            {
                   obj.id, obj.RequestType,obj.attachement,obj.Responsible_Personnel,Convert.ToString(Request["Empno"].ToString()),obj.Remark,obj.status

            };


                a = issuedb.ExecuteNonQuery("dbo", "UpdateLegal", param, values, ref Error);
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
            ViewBag.Projectno = issuedb.combofill("dbo", "getProject", "ProjectName", "Project_Id", ref Error);
            ViewBag.SentTo = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
            return View();

        }

        [HttpPost]
        public ActionResult Create(ModelSLA obj)
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
           "@Projectno",
           "@InternalId",
           "@SentTo",
           "@SLAnumber",
           "@dateofsigned",
           "@attachement"

            };

                var values = new ArrayList()
            {

                obj.Projectno,
                obj.InternalId,
                obj.SentTo,
                obj.SLAnumber,
                DateTime.Now,
                obj.attachement



            };
                a = issuedb.ExecuteNonQuery("dbo", "AddSLA", param, values, ref Error);

            }
            return View();
        }
    }
}