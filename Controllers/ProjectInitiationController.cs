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
    public class ProjectInitiationController : Controller
    {
        Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider issuedb = new Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider();
        string Error = "";
        public ActionResult Index()
        {
            try
            {
                DataTable dt = new DataTable();

                dt = issuedb.ExecuteDataTable("dbo", "GetProjectInitiation1", ref Error);

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
            ViewBag.projectno = issuedb.combofill("dbo", "getProject", "ProjectName", "Project_Id", ref Error);
            ViewBag.clientno = issuedb.combofill("dbo", "getClient", "ClientName", "ClientId", ref Error);
            ViewBag.ResponsibleDeptId = issuedb.combofill("dbo", "getDep", "DepartmentName", "DepartmentID", ref Error);
            ViewBag.ProjectManagerid = issuedb.combofill("dbo", "GetEmployee", "EmployeeName", "EmployeeId", ref Error);
            ViewBag.AcceptanceLetterno = issuedb.combofill("dbo", "GetAcceptanceByProject", "AcceptanceLetterno", "id", ref Error);
            ViewBag.kickoffmeetingLetterno = issuedb.combofill("dbo", "GetKickoff", "KickoffmeetingLetterno", "id", ref Error);
            ViewBag.BranchId = issuedb.combofill("dbo", "getDep", "DepartmentName", "DepartmentID", ref Error);


            DataTable dt = new DataTable();

            var param = new ArrayList()
            {
                "@id"
            };
            var values = new ArrayList() {
                id
            };
            dt = issuedb.ExecuteDataTable("dbo", "GetProjectInitiationbyId", param, values, ref Error);

            if (dt.Rows.Count == 1)
            {
       


                ModelProjectInitiation issuem = new ModelProjectInitiation()
                {


                    id = Convert.ToInt32(dt.Rows[0][9].ToString()),
                    projectno = Convert.ToInt32(dt.Rows[0][10].ToString()),
                    clientno = Convert.ToInt32(dt.Rows[0][11].ToString()),
                    ProjectManagerid = Convert.ToInt32(dt.Rows[0][12].ToString()),
                    ResponsibleDeptId = Convert.ToInt32(dt.Rows[0][13].ToString()),
                    BranchId = Convert.ToInt32(dt.Rows[0][13].ToString()),
                    AcceptanceLetterno = Convert.ToInt32(dt.Rows[0][14].ToString()),
                    kickoffmeetingLetterno = Convert.ToInt32(dt.Rows[0][15].ToString()),
                    kickoffmeetingdate = Convert.ToDateTime(dt.Rows[0][6].ToString()),
                    kickoffmeetingminute = Convert.ToString(dt.Rows[0][8].ToString()),
                    remark = Convert.ToString(dt.Rows[0][7].ToString())


                };
                Session["kickoffmeetingminute"] = Convert.ToString(dt.Rows[0][8].ToString());

               // ViewBag.SelRequestType = Convert.ToString(dt.Rows[0][3].ToString());
               // ViewBag.SelResponsible_Personnel = Convert.ToString(dt.Rows[0][6].ToString());

                return View(issuem);
            }
            else
            {
                return RedirectToAction("Index", TempData["tenderid"]);
            }

        }
        [HttpPost]
        public ActionResult Edit(ModelProjectInitiation obj)
        {
            HttpFileCollectionBase file = Request.Files;
            bool a = false;
            //obj.Project_Id= issuedb.combofill("dbo", "getProject", "ProjectName", "Project_Id", ref Error)
            if (file[0].FileName.ToString() != "")
            {
                string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));

                file[0].SaveAs(path);
                obj.kickoffmeetingminute = "~/img/" + Path.GetFileName(file[0].FileName);


            }



            var errors = ModelState
              .Where(x => x.Value.Errors.Count > 0)
              .Select(x => new { x.Key, x.Value.Errors })
              .ToArray();
            if (ModelState.IsValid)
            {
                var param = new ArrayList()
            {
              "@id","@projectno","@clientno","@ProjectManagerid","@ResponsibleDeptId","@BranchId","@AcceptanceLetterno","@kickoffmeetingLetterno","@kickoffmeetingminute","@remark"

                };
                var values = new ArrayList()
            {
                   obj.id, obj.projectno,obj.clientno,obj.ProjectManagerid,obj.ResponsibleDeptId,obj.BranchId,obj.AcceptanceLetterno,obj.kickoffmeetingLetterno,obj.kickoffmeetingminute,obj.remark
            };


                a = issuedb.ExecuteNonQuery("dbo", "UpdateProjectInitiation", param, values, ref Error);
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
            ViewBag.projectno = issuedb.combofill("dbo", "getProject", "ProjectName", "Project_Id", ref Error);
            ViewBag.clientno = issuedb.combofill("dbo", "getClient", "ClientName", "ClientId", ref Error);
            ViewBag.ResponsibleDeptId = issuedb.combofill("dbo", "getDep", "DepartmentName", "DepartmentID", ref Error);
            ViewBag.ProjectManagerid = issuedb.combofill("dbo", "GetEmployee", "EmployeeName", "EmployeeId", ref Error);
            ViewBag.AcceptanceLetterno = issuedb.combofill("dbo", "GetAcceptanceByProject", "AcceptanceLetterno", "id", ref Error);
            ViewBag.kickoffmeetingLetterno= issuedb.combofill("dbo", "GetKickoff", "KickoffmeetingLetterno", "id", ref Error);
            ViewBag.BranchId = issuedb.combofill("dbo", "getDep", "DepartmentName", "DepartmentID", ref Error);
            return View();

        }
        [HttpPost]
        public ActionResult Create(ModelProjectInitiation obj)
        {
            HttpFileCollectionBase file = Request.Files;
            bool a = false;

            int tender = Convert.ToInt32(TempData["tenderid"]);


            if (file[0].FileName != "")
            {
                string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));

                file[0].SaveAs(path);
                obj.kickoffmeetingminute = "~/img/" + Path.GetFileName(file[0].FileName);
            }
            else
            {
                obj.kickoffmeetingminute = "-";
            }
            var errors = ModelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => new { x.Key, x.Value.Errors })
               .ToArray();
            if (ModelState.IsValid)
            {
                var param = new ArrayList()
            {
            "@projectno",
            "@clientno",
            "@ProjectManagerid",
            "@ResponsibleDeptId",
            "@BranchId",
            "@AcceptanceLetterno",
            "@kickoffmeetingLetterno",
            "@kickoffmeetingdate",
            "@kickoffmeetingminute",
            "@remark",
            "@Isdeleted",
            "@lasteditedby"



            };

                var values = new ArrayList()
            {

                    obj.projectno,
                    obj.clientno,
                    obj.ProjectManagerid,
                    obj.ResponsibleDeptId,
                    obj.BranchId,
                    obj.AcceptanceLetterno,
                    obj.kickoffmeetingLetterno,
                    obj.kickoffmeetingdate,
                    obj.kickoffmeetingminute,
                    obj.remark,
                    0,Convert.ToString(Session["Fullname"])



            };
                a = issuedb.ExecuteNonQuery("dbo", "AddProjectInitiation", param, values, ref Error);

            }
            return View();
        }
    }
}