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
    public class ResponseController : Controller
    {
        // GET: Response
        Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider issuedb = new Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider();
        string Error = "";
        int RequestNo;
        public ActionResult ReplayList(int id)
        {


            try
            {
               TempData["RequestNo"] = id;
                DataTable dt = new DataTable();

                var param = new ArrayList()
            {
                "@RequestNo"
            };
                var values = new ArrayList() {
                id
            };
                dt = issuedb.ExecuteDataTable("dbo", "GetResponseByRequestId", param, values, ref Error);


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
        public ActionResult Edit(int id)
        {
            ViewBag.SubDepartment = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
            ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
            //ViewBag.FromDept = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);

            DataTable dt = new DataTable();

            var param = new ArrayList()
            {
                "@id"
            };
            var values = new ArrayList() {
                id
            };
            dt = issuedb.ExecuteDataTable("dbo", "GetResponseById", param, values, ref Error);

            if (dt.Rows.Count == 1)
            {



                ModelResponse issuem = new ModelResponse()
                {


                    id = Convert.ToInt32(dt.Rows[0][0].ToString()),
                    SubDepartment = Convert.ToString(dt.Rows[0][3].ToString()),
                    Responsible_Personnel = Convert.ToString(dt.Rows[0][5].ToString()),                    
                    TechDocProposal = Convert.ToString(dt.Rows[0][7].ToString()),                    
                    FinDocProposal = Convert.ToString(dt.Rows[0][8].ToString()),
                    FinalProposal = Convert.ToString(dt.Rows[0][9].ToString()),
                    FinalAgreement = Convert.ToString(dt.Rows[0][10].ToString()),
                    OtherAttachement = Convert.ToString(dt.Rows[0][11].ToString()),
                     ResponseRemark = Convert.ToString(dt.Rows[0][13].ToString()),
                    Status = Convert.ToString(dt.Rows[0][14].ToString()),
                    Approved = Convert.ToString(dt.Rows[0][15].ToString())



                };
                Session["TechDocProposal"] = Convert.ToString(dt.Rows[0][7].ToString());
                Session["FinDocProposal"] = Convert.ToString(dt.Rows[0][8].ToString());
                Session["FinalProposal"] = Convert.ToString(dt.Rows[0][9].ToString());
                Session["FinalAgreement"] = Convert.ToString(dt.Rows[0][10].ToString());
                Session["OtherAttachement"] = Convert.ToString(dt.Rows[0][11].ToString());
                ViewBag.SelResponsible_Personnel = Convert.ToString(dt.Rows[0][6].ToString());
                ViewBag.SelSubDepartment = Convert.ToString(dt.Rows[0][4].ToString());
                ViewBag.EmpId = Convert.ToInt32(dt.Rows[0][16].ToString());
                ViewBag.Status = Convert.ToString(dt.Rows[0][14].ToString());
                ViewBag.Approved = Convert.ToString(dt.Rows[0][15].ToString());

                return View(issuem);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public ActionResult Edit(ModelResponse obj)
        {
            HttpFileCollectionBase file = Request.Files;
            bool a = false;

              
            if (file[0].FileName != "")
            {
                string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));

                file[0].SaveAs(path);
                obj.TechDocProposal = "~/img/" + Path.GetFileName(file[0].FileName);
            }
            else
            {
                obj.TechDocProposal = "-";
            }
            if (file[1].FileName != "")
            {
                string path1 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[1].FileName));

                file[1].SaveAs(path1);
                obj.FinDocProposal = "~/img/" + Path.GetFileName(file[1].FileName);
            }
            else
            {
                obj.FinDocProposal = "-";
            }
            if (file[2].FileName != "")
            {
                string path2 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[2].FileName));

                file[2].SaveAs(path2);
                obj.FinalProposal = "~/img/" + Path.GetFileName(file[2].FileName);
            }
            else
            {
                obj.FinalProposal = "-";
            }
            if (file[3].FileName != "")
            {
                string path3 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[3].FileName));

                file[3].SaveAs(path3);
                obj.FinalAgreement = "~/img/" + Path.GetFileName(file[3].FileName);
            }
            else
            {
                obj.FinalAgreement = "-";
            }
            if (file[4].FileName != "")
            {
                string path4 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[4].FileName));

                file[4].SaveAs(path4);
                obj.OtherAttachement = "~/img/" + Path.GetFileName(file[4].FileName);
            }
            else
            {
                obj.OtherAttachement = "-";
            }


            var errors = ModelState
              .Where(x => x.Value.Errors.Count > 0)
              .Select(x => new { x.Key, x.Value.Errors })
              .ToArray();
            if (ModelState.IsValid)
            {
                var param = new ArrayList()
            {
               "@id","@SubDeprtId","@Sub_Department","@EmployeeId","@Responsible_Personnel","@TechDocProposal","@FinDocProposal","@FinalProposal","@FinalAgreement","@Otherattachement","@ResponseRemark","@Status","@Approved"


                };
                var values = new ArrayList()
            {
                   obj.id,obj.SubDepartment,Convert.ToString(Request["subdeprt"].ToString()), obj.Responsible_Personnel,Convert.ToString(Request["empid"].ToString()),obj.TechDocProposal,obj.FinDocProposal,obj.FinalProposal,obj.FinalAgreement,obj.OtherAttachement,obj.ResponseRemark,obj.Status,obj.Approved

            };
                var param1 = new ArrayList()
                {
                    "@id" ,

                    "@Res_status"
                };
               
                if (obj.Approved == "Approved")
                {
                    var values1 = new ArrayList()
                {
                    TempData["RequestNo"],1
                };
                    a = issuedb.ExecuteNonQuery("dbo", "UpdateRequestStatus", param1, values1, ref Error);
                }
                else if ((obj.Approved != "Approved") && (obj.Status== "Complete Send For Approval"))
                {
                    var values1 = new ArrayList()
                {
                    TempData["RequestNo"],2
                };
                    a = issuedb.ExecuteNonQuery("dbo", "UpdateRequestStatus", param1, values1, ref Error);
                }
                else {
                    var values1 = new ArrayList()
                {
                    TempData["RequestNo"],0
                };
                    a = issuedb.ExecuteNonQuery("dbo", "UpdateRequestStatus", param1, values1, ref Error);
                }
                        a = issuedb.ExecuteNonQuery("dbo", "UpdateResponse", param, values, ref Error);
               

            }
            if (a)
            {
               // return RedirectToAction("");
                return RedirectToAction("Index", new { id = TempData["RequestNo"] });
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
            ViewBag.SubDepartment = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
            return View();
        }

        [HttpPost]
        public ActionResult Create(ModelResponse obj)
        {
            try
            { 
                HttpFileCollectionBase file = Request.Files;
                bool a = false;

               


                if (file[0].FileName != "")
                {
                    string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));

                    file[0].SaveAs(path);
                    obj.TechDocProposal = "~/img/" + Path.GetFileName(file[0].FileName);
                }
                else
                {
                    obj.TechDocProposal = "-";
                }
                if (file[1].FileName != "")
                {
                    string path1 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[1].FileName));

                    file[1].SaveAs(path1);
                    obj.FinDocProposal = "~/img/" + Path.GetFileName(file[1].FileName);
                }
                else
                {
                    obj.FinDocProposal = "-";
                }
                if (file[2].FileName != "")
                {
                    string path2 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[2].FileName));

                    file[2].SaveAs(path2);
                    obj.FinalProposal = "~/img/" + Path.GetFileName(file[2].FileName);
                }
                else
                {
                    obj.FinalProposal = "-";
                }
                if (file[3].FileName != "")
                {
                    string path3 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[3].FileName));

                    file[3].SaveAs(path3);
                    obj.FinalAgreement = "~/img/" + Path.GetFileName(file[3].FileName);
                }
                else
                {
                    obj.FinalAgreement = "-";
                }
                if (file[4].FileName != "")
                {
                    string path4 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[4].FileName));

                    file[4].SaveAs(path4);
                    obj.OtherAttachement = "~/img/" + Path.GetFileName(file[4].FileName);
                }
                else
                {
                    obj.OtherAttachement = "-";
                }


                var errors = ModelState
                   .Where(x => x.Value.Errors.Count > 0)
                   .Select(x => new { x.Key, x.Value.Errors })
                   .ToArray();
                if (ModelState.IsValid)
                {
                    var param = new ArrayList()
                  {
                    "@tender_id",
                    "@RequestNo",
                    "@SubDeprtId",
                    "@Sub_Department",
                    "@EmployeeId",
                    "@Responsible_Personnel",
                    "@TechDocProposal",
                    "@FinDocProposal",
                    "@FinalProposal",
                    "@FinalAgreement",
                    "@Otherattachement",
                    "@ResponseDate",
                    "@ResponseRemark",
                    "@Status",
                    "@Approved",
                    "@LastEditedBy",
                    "@isdeleted"

                  };

                    var values = new ArrayList()
                    {
                     TempData["tenderid"],
                    TempData["RequestNo"],
                    obj.SubDepartment,
                    Convert.ToString(Request["subdeprt"].ToString()),
                     obj.Responsible_Personnel,
                     Convert.ToString(Request["empid"].ToString()),
                     obj.TechDocProposal,
                     obj.FinDocProposal,
                     obj.FinalProposal,
                     obj.FinalAgreement,
                     obj.OtherAttachement,
                    DateTime.Now,
                    obj.ResponseRemark,
                    "New", "New",
                     Convert.ToString(Session["Fullname"]),0

                     };



                    a = issuedb.ExecuteNonQuery("dbo", "AddResponse", param, values, ref Error);
                  
                }
                    if (a)
                    {
                       // return RedirectToAction("ReplayList", );
                    return RedirectToAction("ReplayList", new { id = TempData["RequestNo"] });
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
    }
}
