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
    public class TenderDetailController : Controller
    {
        // GET: Assign
        Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider issuedb = new Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider();
        string Error = "";
        public ActionResult AssignedList(int Tid)
        {
            try
            {
                DataTable dt = new DataTable();
                TempData["tenderid"] = Tid;

                var param = new ArrayList()
            {
                "@Tenderid"
            };
                var values = new ArrayList() {
                Tid
            };
                dt = issuedb.ExecuteDataTable("dbo", "GetTenderDetailByTenderId", param, values, ref Error);


                ViewData.Model = dt.AsEnumerable();
                return View();
            }
            catch (NullReferenceException ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
        }
        public ActionResult Create()
        {
            try
            {
                ViewBag.AssignedTeam = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
               // ViewBag.Responsible_Position = issuedb.combofill("dbo", "GetProffesionType", "ProfessionType", "id", ref Error);
                ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Admin", "Error"));
            }
        }
        [HttpPost]
        public ActionResult Create(ModelTenderDetail obj)
        {
            try
            {
                HttpFileCollectionBase file = Request.Files;
                bool a = false;

                int tender = Convert.ToInt32(TempData["tenderid"]);


                if (file[0].FileName != "")
                {
                    string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));

                    file[0].SaveAs(path);
                    obj.Attachment = "~/img/" + Path.GetFileName(file[0].FileName);
                }
                else
                {
                    obj.Attachment = "-";
                }

                    
                   
                
                var errors = ModelState
                   .Where(x => x.Value.Errors.Count > 0)
                   .Select(x => new { x.Key, x.Value.Errors })
                   .ToArray();
                if (ModelState.IsValid)
                {
                    var param = new ArrayList()
            {
                 "@Tenderid",
                 "@EmployeeId",
                 "@Responsible_Personnel",
                 "@AssignedTeamId",
                 "@AssignedTeam",
                "@DateOfAssign",
                "@initialdeadlinedate",
                "@Attachment",
                "@Remark",
                "@FinalizedDate",
                "@ApprovedorRejected",
                "@Status",
                "@LastEditedBy",
                "@isdeleted",
                "@ResponseAttachement"

            };
                    var values = new ArrayList()
            {
                    tender,
                    obj.Responsible_Personnel,
                    Convert.ToString(Request["personnelid"].ToString()),
                    obj.AssignedTeam,
                    Convert.ToString(Request["ssubid"].ToString()),
                    obj.DateOfAssign.ToShortDateString(),
                    obj.initialdeadlinedate.ToShortDateString(),
                    obj.Attachment,obj.Remark,DateTime.Now.ToShortDateString(),"New","New",Convert.ToString(Session["Fullname"]),0,"-"

            };


                    a = issuedb.ExecuteNonQuery("dbo", "AddTenderDetail", param, values, ref Error);


                }

                if (a)
                {
                    return RedirectToAction("AssignedList", new { Tid = TempData["tenderid"] });
                }
                else
                {
                    ViewBag.msg = "<script>alert('Personnel Assigned is not saved') </script>";
                    ModelState.Clear();
                    ViewBag.SubDepartment = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
                   // ViewBag.Responsible_Position = issuedb.combofill("dbo", "GetProffesionType", "ProfessionType", "id", ref Error);
                    ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
                }
                return View();
            }
            catch (NullReferenceException ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                //ModelTenderDetail issue = new ModelTenderDetail();
                DataTable dt = new DataTable();
                ViewBag.AssignedTeam = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
               // ViewBag.Responsible_Position = issuedb.combofill("dbo", "GetProffesionType", "ProfessionType", "id", ref Error);
                ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);

                var param = new ArrayList()
            {
                "@id"
            };
                var values = new ArrayList() {
                id
            };

                dt = issuedb.ExecuteDataTable("dbo", "GetTenderDetailsById", param, values, ref Error);

                if (dt.Rows.Count == 1)
                {
                    ModelTenderDetail issue = new ModelTenderDetail()
                    {
                        id = Convert.ToInt32(dt.Rows[0][0].ToString()),
                        Responsible_Personnel = Convert.ToString(dt.Rows[0][2].ToString()),
                        AssignedTeam = Convert.ToString(dt.Rows[0][4].ToString()),
                        initialdeadlinedate = Convert.ToDateTime(dt.Rows[0][6].ToString()),
                        Attachment = Convert.ToString(dt.Rows[0][7].ToString()),
                        Remark = Convert.ToString(dt.Rows[0][8].ToString()),
                        ApprovedorRejected = Convert.ToString(dt.Rows[0][10].ToString()),
                        Status = Convert.ToString(dt.Rows[0][11].ToString()),
                         ResponseAttachement= Convert.ToString(dt.Rows[0][14].ToString()),

                    };

                    ViewBag.EmpId = Convert.ToString(dt.Rows[0][13].ToString());

                    ViewBag.team = Convert.ToString(dt.Rows[0][5].ToString());
                    ViewBag.Personnel = Convert.ToString(dt.Rows[0][3].ToString());
                    Session["imgpath"] = Convert.ToString(dt.Rows[0][7].ToString());
                    Session["Response"] = Convert.ToString(dt.Rows[0][14].ToString());
                    return View(issue);
                }
                else
                {
                    return View();
                }
            }
            catch (NullReferenceException ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }

        }
        [HttpPost]
        public ActionResult Edit(ModelTenderDetail obj)
        {
            try
            {
                bool a = false;
                bool b = false;
                HttpFileCollectionBase file = Request.Files;
                string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));
                /*
                if (Convert.ToString(Request["Selstatus"]) == "Closed" && file[0].FileName.ToString() == "")
                {
                    ViewBag.msg1 = "<script>alert('If Issue Cause is Closed Attachement is needed') </script>";
                    goto last;
                }
                */
                if (file[0].FileName.ToString() != "")
                {
                    file[0].SaveAs(path);
                    obj.Attachment = "~/img/" + Path.GetFileName(file[0].FileName);
                }
                else
                    {
                    obj.Attachment = "-";
                }

                if (file[1].FileName.ToString() != "")
                {
                    file[1].SaveAs(path);
                    obj.ResponseAttachement = "~/img/" + Path.GetFileName(file[1].FileName);
                }
                else
                {
           
                        obj.ResponseAttachement = "-";
                    
                }

                if (ModelState.IsValid)
                {

                    var param = new ArrayList()
                {
                "id","@EmployeeId","@Responsible_Personnel","@AssignedTeamId","@AssignedTeam","@initialdeadlinedate","@Attachment","@Remark","ApprovedorRejected","@Status","@LastEditedBy", "@ResponseAttachement"
                };



                    var values = new ArrayList()
                 {
                    obj.id,
                    obj.Responsible_Personnel ,
                    Convert.ToString(Request["EmpId"].ToString()),
                    obj.AssignedTeam,
                    Convert.ToString(Request["SubDID"].ToString()),
                    obj.initialdeadlinedate,
                    obj.Attachment,
                    obj.Remark,
                    obj.ApprovedorRejected,
                    obj.Status,
                     Session["Fullname"],
                     obj.ResponseAttachement

            };

                    var param1 = new ArrayList()
                {
                   "@id",
                   "@Status",
                   "@LastEditedBy"
                };
                    if (obj.ApprovedorRejected == "Approved")
                    {
                        var values1 = new ArrayList()
                {
                   TempData["tenderid"],
                   "Finalized",
                    Session["Fullname"]
                };
                        a = issuedb.ExecuteNonQuery("dbo", "UpdateTenderStatus", param1, values1, ref Error);

                    }
                    else if ((obj.ApprovedorRejected != "Approved") && (obj.Status == "Complete Send For Approval"))
                    {
                        var values1 = new ArrayList()
                  {
                   TempData["tenderid"],"Complete Send For Approval", Session["Fullname"]
                   };
                        a = issuedb.ExecuteNonQuery("dbo", "UpdateTenderStatus", param1, values1, ref Error);
                    }

                    a = issuedb.ExecuteNonQuery("dbo", "UpdateTenderDetails", param, values, ref Error);
                }

            last:
                if (a || b)
                {

                    return RedirectToAction("AssignedList", new { Tid = TempData["tenderid"] });
                }
                else
                {
                    if (Request["Selstatus"] != "Closed")
                    {
                        ViewBag.msg = "<script>alert('Initiation Details update failed') </script>";
                        ModelState.Clear();
                        ViewBag.AssignedTeam = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
                        // ViewBag.Responsible_Position = issuedb.combofill("dbo", "GetProffesionType", "ProfessionType", "id", ref Error);
                        ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
                    }
                    else
                    {
                        return RedirectToAction("List", new { Isid = TempData["issueid"] });
                    }
                }
                return View();
            }
            catch (NullReferenceException ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Admin", "Error"));
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Admin", "Error"));
            }


        }
        public ActionResult SelectPersonnel(int Id = 0, int SubDepartment = 0)
        {

            try
            {
                DataTable dt = new DataTable();
                var param = new ArrayList()
            {
                "Id",
                "@UnitId"
            };
                var values = new ArrayList() {
                Id,
                SubDepartment
            };
                dt = issuedb.ExecuteDataTable("dbo", "GetEmployeefullInfopercriteria", param, values, ref Error);

                List<ModelEmployee> Employee = issuedb.ConvertDataTable<ModelEmployee>(dt);
                if (Id == 0)
                {
                    ViewBag.Selectedemployee = new SelectList(Employee, "Id", "FullName");
                }
                else
                {
                    ViewBag.Selectedemployee = new SelectList(Employee, "Id", "PositionClassName");
                }
                return PartialView("DisplayEmployee");
            }
            catch (NullReferenceException ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
        }

    }
}