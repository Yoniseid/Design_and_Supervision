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
    public class LegalController : Controller
    {
        // GET: Legal
        Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider issuedb = new Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider();
        string Error = "";

        public ActionResult Index(int Tid)
        {
           

            try
            {
                DataTable dt = new DataTable();
                TempData["tenderid"] = Tid;
                List<ModelLegal> legaldetails = new List<ModelLegal>();
                var param1 = new ArrayList()
                {
                "@tender_id","@Res_status"
                };
                var values1 = new ArrayList() {
                 Tid,0
               };

                               
                dt = issuedb.ExecuteDataTable("dbo", "GetNoRespquestByTenderId", param1, values1, ref Error);

                if (dt.Rows.Count != 0)
                {
                    TempData["alertMessage"] = "There are unclosed requests in this record.";


                    return RedirectToAction("RequestList", "Request", new
                    {
                        Tid = Tid,
                        res_status = 0
                    });
                }
                else
                {
                    var param = new ArrayList()
            {
                "@Tenderid"
            };
                    var values = new ArrayList() {
                Tid
            };

                    if (Tid != 0)
                    {

                        dt = new DataTable();
                        dt = issuedb.ExecuteDataTable("dbo", "GetLegalByTender", param, values, ref Error);

                        legaldetails = issuedb.ConvertDataTable<ModelLegal>(dt);
                        //return View(legaldetails.ToList());


                        if (dt.Rows.Count > 0)
                        {


                            //ViewData.Model = dt.AsEnumerable();
                            return View(legaldetails.ToList());
                        }

                        else
                        {
                            return RedirectToAction("Create");
                        }

                    }
                    else
                    {
                        dt = issuedb.ExecuteDataTable("dbo", "GetLegal", ref Error);
                        legaldetails = issuedb.ConvertDataTable<ModelLegal>(dt);
                        return View(legaldetails.ToList());

                    }
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

        public ActionResult ViewTenderDetails(int Tid)
        {
            try
            {
                DataTable dt = new DataTable();
                var param = new ArrayList()
            {
                "@id"
            };
                var values = new ArrayList() {
                Tid
            };

                //dt = issuedb.ExecuteDataTable("dbo", "GetLegalByTender", param, values, ref Error);
                dt = issuedb.ExecuteDataTable("dbo", "GetTenderDocDetails", param, values, ref Error);

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
            ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
            //ViewBag.ToDept = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
            //ViewBag.FromDept = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);

            DataTable dt = new DataTable();

            var param = new ArrayList()
            {
                "@id"
            };
            var values = new ArrayList() {
                id
            };
            dt = issuedb.ExecuteDataTable("dbo", "GetLegalById", param, values, ref Error);

            if (dt.Rows.Count == 1)
            {



                ModelLegal issuem = new ModelLegal()
                {


                    id = Convert.ToInt32(dt.Rows[0][0].ToString()),
                    RequestType = Convert.ToString(dt.Rows[0][3].ToString()),
                    attachement = Convert.ToString(dt.Rows[0][4].ToString()),
                    // ClientNo = Convert.ToInt32(dt.Rows[0][2].ToString()),                   
                    //EmployeeId = Convert.ToInt32(dt.Rows[0][5].ToString()),
                    Responsible_Personnel  = Convert.ToString(dt.Rows[0][5].ToString()),
                    Remark = Convert.ToString(dt.Rows[0][7].ToString()),
                    status= Convert.ToString(dt.Rows[0][8].ToString())

                };
                string a= Convert.ToString(dt.Rows[0][4].ToString());
                Session["attachement"] = Convert.ToString(dt.Rows[0][4].ToString());
                string Remark = Convert.ToString(dt.Rows[0][7].ToString());
                ViewBag.SelRequestType = Convert.ToString(dt.Rows[0][3].ToString());
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
            if (file[0].FileName.ToString() != "")
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
            try
            {
                ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
                // ViewBag.Responsible_Position = issuedb.combofill("dbo", "GetProffesionType", "ProfessionType", "id", ref Error);
                //ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Admin", "Error"));
            }
        }
        [HttpPost]
        public ActionResult Create(ModelLegal obj)
        {
            try
            {
                HttpFileCollectionBase file = Request.Files;
                bool a = false;

                int tender = Convert.ToInt32(TempData["tenderid"]);


                if (file[0].FileName.ToString() != "")
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
                "@tenderid",
                "@date",
                "@RequestType",
                "@attachement",
                "@EmployeeId",
                "@Responsible_Personnel",
                "@Remark",
                 "@status",
                "@isdeleted",
                "@lasteditedby"

            };

                    var values = new ArrayList()
            {
                    tender,
                    DateTime.Now,
                    obj.RequestType,
                    obj.attachement,
                    obj.Responsible_Personnel,
                    Convert.ToString(Request["Responsibleid"].ToString()),"","",
                    obj.Remark,
                    obj.status,
                    0,Convert.ToString(Session["Fullname"])


            };
                    
                    var param1 = new ArrayList()
                    {
                         "@id",
                         "@Status",
                         "@LastEditedBy"
                    };
                    var values1 = new ArrayList()
                    {
                          tender,
                          "Sent To Legal",
                          Convert.ToString(Session["Fullname"])
                    };

                    a = issuedb.ExecuteNonQuery("dbo", "AddLegal", param, values, ref Error);
                    bool b = issuedb.ExecuteNonQuery("dbo", "UpdateTenderStatus", param1, values1, ref Error);
                     
                } 

                if (a)
                {
                    return RedirectToAction("Index", new { Tid = TempData["tenderid"] });
                }
                else
                {
                    ViewBag.msg = "<script>alert('Record is not saved') </script>";
                    ModelState.Clear();
                    
                    ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
                }
                return View();
            }
            catch (NullReferenceException ex)
            {
                return View("Error", new HandleErrorInfo(ex.InnerException, "Tender", "Error"));
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
        }

        public ActionResult ListTenderDetails(int Tid)
        {

            try
            {
                DataTable dt = new DataTable();
                var param = new ArrayList()
                 {
                "@tender_id",

                 };
                var values = new ArrayList() {
               Tid

                };

                dt = issuedb.ExecuteDataTable("dbo", "GetReqResponseByTenderId", param, values, ref Error);

                ViewData.Model = dt.AsEnumerable();

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }




        }
    }
}