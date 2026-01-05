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
    public class LegalResponseController : Controller
    {
        Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider issuedb = new Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider();
        string Error = "";
        int RequestNo;

        public ActionResult Index(int Dept = 0, int Eid = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                //TempData["tenderid"] = Tid;


                if ((Dept != 0) && (Eid != 0))
                {
                    var param = new ArrayList()
                    {
                  "@ToDepId" ,"@EmpId"
                     };
                    var values = new ArrayList() {
                      Dept,Eid
                    };
                    dt = issuedb.ExecuteDataTable("dbo", "GetTenderByDepHead", param, values, ref Error);
                    if (dt.Rows.Count < 1)
                    {

                        dt = issuedb.ExecuteDataTable("dbo", "GetResqByEmployee", param, values, ref Error);

                    }

                }

                if (dt.Rows.Count >= 1)
                {
                    TempData["tenderid"] = dt.Rows[0][1].ToString();
                }
                ViewData.Model = dt.AsEnumerable();
                return View();
            }
            catch (NullReferenceException ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
        }






        public ActionResult LegalReplayList(int id)
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
                dt = issuedb.ExecuteDataTable("dbo", "GetLegalResponseByRequestId", param, values, ref Error);


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
            // ViewBag.ClientName = issuedb.combofill("dbo", "getClient", "ClientName", "ClientId", ref Error);
            //ViewBag.SubDepartment = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
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
            dt = issuedb.ExecuteDataTable("dbo", "GetLegalResponseById", param, values, ref Error);

            if (dt.Rows.Count == 1)
            {



                ModelLegalResponse issuem = new ModelLegalResponse()
                {

                                                                          
                    id = Convert.ToInt32(dt.Rows[0][0].ToString()),
                    FinalProposal = Convert.ToString(dt.Rows[0][5].ToString()),
                    Responsible_Personnel= Convert.ToString(dt.Rows[0][3].ToString()),
                    FinalAgreement = Convert.ToString(dt.Rows[0][6].ToString()),
                    OtherAttachement = Convert.ToString(dt.Rows[0][7].ToString()),
                    ResponseRemark = Convert.ToString(dt.Rows[0][9].ToString()),
                    Status = Convert.ToString(dt.Rows[0][11].ToString()),
                    Approved = Convert.ToString(dt.Rows[0][10].ToString())



                };
                Session["FinalProposal"] = Convert.ToString(dt.Rows[0][5].ToString());
                Session["FinalAgreement"] = Convert.ToString(dt.Rows[0][6].ToString());
                Session["OtherAttachement"] = Convert.ToString(dt.Rows[0][7].ToString());
                ViewBag.EmpId= Convert.ToInt32(dt.Rows[0][15].ToString());
                ViewBag.emp= Convert.ToString(dt.Rows[0][4].ToString());
                ViewBag.Status = Convert.ToString(dt.Rows[0][11].ToString());
                ViewBag.Approve = Convert.ToString(dt.Rows[0][10].ToString());


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
                obj.FinalProposal = "~/img/" + Path.GetFileName(file[0].FileName);
            }
            else
            {
                obj.FinalProposal = "-";
            }
            if (file[1].FileName != "")
            {
                string path1 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[1].FileName));

                file[1].SaveAs(path1);
                obj.FinalAgreement = "~/img/" + Path.GetFileName(file[1].FileName);
            }
            else
            {
                obj.FinalAgreement = "-";
            }
            if (file[2].FileName != "")
            {
                string path2 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[2].FileName));

                file[2].SaveAs(path2);
                obj.OtherAttachement = "~/img/" + Path.GetFileName(file[2].FileName);
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
               "@id","@EmpId","@Responsible_Personnel","@FinalProposal","@FinalAgreement","@OtherAttachement","@ResponseDate","@ResponseRemark","@Approved","@Status","@LastEditedBy"


                };
                var values = new ArrayList()
            {
                   obj.id,obj.Responsible_Personnel,Request["emp"],obj.FinalProposal,obj.FinalAgreement,obj.OtherAttachement,DateTime.Now,obj.ResponseRemark,obj.Approved,obj.Status,Convert.ToString(Session["Fullname"])

            };

                var param1 = new ArrayList()
                {
                    "@id" ,

                    "@status"
                };

                if (obj.Approved == "Approved")
                {
                    var values1 = new ArrayList()
                {
                    TempData["RequestNo"],1
                };
                    a = issuedb.ExecuteNonQuery("dbo", "UpdateRequestStatus", param1, values1, ref Error);
                }
                else if ((obj.Approved != "Approved") && (obj.Status == "Complete Send For Approval"))
                {
                    var values1 = new ArrayList()
                {
                    TempData["RequestNo"],2
                };
                    int b =Convert.ToInt32(TempData["RequestNo"]);
                    a = issuedb.ExecuteNonQuery("dbo", "UpdateLegalRequestStatus", param1, values1, ref Error);
                }
                else
                {
                    var values1 = new ArrayList()
                {
                    TempData["RequestNo"],0
                };
                    a = issuedb.ExecuteNonQuery("dbo", "UpdateLegalRequestStatus", param1, values1, ref Error);
                }

                a = issuedb.ExecuteNonQuery("dbo", "UpdateLegalResponse", param, values, ref Error);

            }
            if (a)
            {
                // return RedirectToAction("Index");
                return RedirectToAction("LegalReplayList", new { id = TempData["RequestNo"] });
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
            //ViewBag.SubDepartment = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
            return View();
        }

        [HttpPost]
        public ActionResult Create(ModelLegalResponse obj)
        {
            try
            {
                HttpFileCollectionBase file = Request.Files;
                bool a = false;
/*
                var param1 = new ArrayList()
            {
               "@id","@status"


                };
                var values1 = new ArrayList()
            {
                   TempData["RequestNo"],Convert.ToInt32("1")

            };

    */
                if (file[0].FileName != "")
                {
                    string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));

                    file[0].SaveAs(path);
                    obj.FinalProposal = "~/img/" + Path.GetFileName(file[0].FileName);
                }
                else
                {
                    obj.FinalProposal = "-";
                }
                if (file[1].FileName != "")
                {
                    string path1 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[1].FileName));

                    file[1].SaveAs(path1);
                    obj.FinalAgreement = "~/img/" + Path.GetFileName(file[1].FileName);
                }
                else
                {
                    obj.FinalAgreement = "-";
                }
                
                if (file[2].FileName != "")
                {
                    string path4 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[4].FileName));

                    file[2].SaveAs(path4);
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
                    "@EmpId",
                    "@Responsible_Personnel",
                    "@FinalProposal",
                    "@FinalAgreement",
                     "@OtherAttachement",
                     "@ResponseDate",
                     "@ResponseRemark",
                    "@Approved",
                    "@Status",
                    "@LastEditedBy",
                    "@isdeleted"

                  };

                    var values = new ArrayList()
                    {
                     TempData["tenderid"],
                    TempData["RequestNo"],
                    obj.Responsible_Personnel,
                    Request["emp"],
                     obj.FinalProposal,
                     obj.FinalAgreement,
                     obj.OtherAttachement,
                    DateTime.Now,
                    obj.ResponseRemark,
                    "New", "New",
                     Convert.ToString(Session["Fullname"]),0

                     };



                    a = issuedb.ExecuteNonQuery("dbo", "AddLegalResponse", param, values, ref Error);
                   // a = issuedb.ExecuteNonQuery("dbo", "UpdateLegalRequestStatus", param1, values1, ref Error);
                }
                if (a)
                {
                    // return RedirectToAction("ReplayList", );
                    return RedirectToAction("LegalReplayList", new { id = TempData["RequestNo"] });
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