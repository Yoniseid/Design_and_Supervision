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
    public class RequestController : Controller
    {
        Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider issuedb = new Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider();
        string Error = "";
        public ActionResult Index(int Dept=0, int Eid=0)
        {
            try
            {
                DataTable dt = new DataTable();
                //TempData["tenderid"] = Tid;

              
                if ((Dept != 0) && (Eid !=0))
                {
                    var param = new ArrayList()
                    {
                  "@ToDepId" ,"@EmpId"
                     };
                    var values = new ArrayList() {
                      Dept,Eid
                    };
                    dt = issuedb.ExecuteDataTable("dbo", "GetTenderByDepHead", param, values, ref Error);
                    if(dt.Rows.Count<1)
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
        public ActionResult RequestList(int Tid, int res_status = 3)
        {
            try
            {
                DataTable dt = new DataTable();
                TempData["tenderid"] = Tid;


                if (res_status == 3)
                {
                    var param = new ArrayList()
            {
                "@tender_id"
            };
                    var values = new ArrayList() {
                Tid
            };
                    dt = issuedb.ExecuteDataTable("dbo", "GetRespquestByTenderId", param, values, ref Error);
                }
                else
                {
                    var param = new ArrayList()
                     {
                       "@tender_id","@Res_status"
                     };
                    var values = new ArrayList() {
                      Tid,res_status
                     };
                    dt = issuedb.ExecuteDataTable("dbo", "GetNoRespquestByTenderId", param, values, ref Error);
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
                return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ClientId = issuedb.combofill("dbo", "getClient", "ClientName", "ClientId", ref Error);
            ViewBag.ToDepId = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
            ViewBag.DeprtmentHead = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
            // ViewBag.FromDept = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);

            DataTable dt = new DataTable();

            var param = new ArrayList()
            {
                "@id"
            };
            var values = new ArrayList() {
                id
            };
            dt = issuedb.ExecuteDataTable("dbo", "GetRequestById", param, values, ref Error);

            if (dt.Rows.Count == 1)
            {



                ModelRequest issuem = new ModelRequest()
                {
                    id = Convert.ToInt32(dt.Rows[0][0].ToString()),
                    RequestMode = Convert.ToString(dt.Rows[0][2].ToString()),
                    RequestType = Convert.ToString(dt.Rows[0][3].ToString()),
                    ToDepId = Convert.ToString(dt.Rows[0][4].ToString()),
                    Sentto = Convert.ToString(dt.Rows[0][5].ToString()),
                    DeprtmentHead= Convert.ToString(dt.Rows[0][6].ToString()),
                    RequestDate = Convert.ToDateTime(dt.Rows[0][8].ToString()),
                    RequestRemark = Convert.ToString(dt.Rows[0][9].ToString()),
                    RequestAttachement = Convert.ToString(dt.Rows[0][10].ToString())
                    
                };
                //string a = Convert.ToString(dt.Rows[0][2].ToString());
                //string b = Convert.ToString(dt.Rows[0][5].ToString());
                Session["attachement1"] = Convert.ToString(dt.Rows[0][10].ToString());

                //ViewBag.SelectedFromDep = Convert.ToString(dt.Rows[0][3].ToString());
                ViewBag.SelectedToDep = Convert.ToString(dt.Rows[0][5].ToString());
                ViewBag.SelectedType = Convert.ToString(dt.Rows[0][3].ToString());
                ViewBag.SelectedHead = Convert.ToString(dt.Rows[0][7].ToString());

                return View(issuem);
            }
            else
            {
                return RedirectToAction("Index");
            }



        }
        [HttpPost]
        public ActionResult Edit(ModelRequest obj)
        {
            HttpFileCollectionBase file = Request.Files;
            bool a = false;
            //obj.Project_Id= issuedb.combofill("dbo", "getProject", "ProjectName", "Project_Id", ref Error)
            if (file[0].FileName.ToString() != "")
            {
                string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));

                file[0].SaveAs(path);
                obj.RequestAttachement = "~/img/" + Path.GetFileName(file[0].FileName);

            }
            if (obj.DeprtmentHead == "" || obj.DeprtmentHead == null)
            {
                obj.DeprtmentHead = Convert.ToString("0");

            }


            var errors = ModelState
              .Where(x => x.Value.Errors.Count > 0)
              .Select(x => new { x.Key, x.Value.Errors })
              .ToArray();
            if (ModelState.IsValid)
            {
                var param = new ArrayList()
            {
              "@id","@tender_id","@RequestMode","@RequestType","@ToDepId","@Sentto","@EmpId","@DeprtmentHead","@RequestRemark","@RequestAttachement"


            };
                var values = new ArrayList()
            {

                    obj.id,TempData["tenderid"],obj.RequestMode,Convert.ToString(Request["Type"]),obj.ToDepId,obj.Sentto,obj.DeprtmentHead,Convert.ToString(Request["head"]),obj.RequestRemark,obj.RequestAttachement
            };


                a = issuedb.ExecuteNonQuery("dbo", "UpdateRequest", param, values, ref Error);
            }
            if (a)
            {
                return RedirectToAction("RequestList", new { Tid = TempData["tenderid"] });
               // return RedirectToAction("RequestList");
            }
            else
            {
                ViewBag.msg = "<script>alert('Tender update failed') </script>";
                ModelState.Clear();
                ViewBag.ClientId = issuedb.combofill("dbo", "getClient", "ClientName", "ClientId", ref Error);
                ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
                ViewBag.AssignedTeam = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
            }
            return View();
        }
        
        public ActionResult Create()
        {
            try
            {
                //ViewBag.FromDept = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
                ViewBag.ToDepId = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
                ViewBag.ClientId = issuedb.combofill("dbo", "getClient", "ClientName", "ClientId", ref Error);
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Admin", "Error"));
            }
        }
        [HttpPost]
        public ActionResult Create(ModelRequest obj)
        {
            try
            {
                HttpFileCollectionBase file = Request.Files;
                bool a = false;

                //int tender = Convert.ToInt32(TempData["tenderid"]);
                if (obj.DeprtmentHead =="" || obj.DeprtmentHead== null)
                {
                    obj.DeprtmentHead =Convert.ToString("0");
                   
                }

                    if (file[0].FileName != "")
                {
                    string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));

                    file[0].SaveAs(path);
                    obj.RequestAttachement = "~/img/" + Path.GetFileName(file[0].FileName);
                }
                else
                {
                    obj.RequestAttachement = "-";
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
                "@RequestMode",
                "@RequestType",
                "@ToDepId",
                "@Sentto",
                "@EmpId" ,
                "@DeprtmentHead",
                "@RequestDate",
                "@RequestRemark",
                "@RequestAttachement",
                "@Res_status",
                 "@LastEditedBy",
                "@isdeleted"


                  };


                    DateTime pc = DateTime.Now;
                    //string c = Convert.ToString(Request["subidto"]);


                    var values = new ArrayList()
                    {
                    Convert.ToInt32(TempData["tenderid"]),
                    obj.RequestMode,
                    Convert.ToString(Request["type"]),
                    obj.ToDepId,
                    obj.Sentto,
                    obj.DeprtmentHead,
                    Convert.ToString(Request["head"]),
                    DateTime.Now,
                    obj.RequestRemark,
                    obj.RequestAttachement,0,
                    Convert.ToString(Session["Fullname"]),
                    0  

                     };


                    
                    a = issuedb.ExecuteNonQuery("dbo", "AddRequest", param, values, ref Error);


                }

                if (a)
                {
                    return RedirectToAction("RequestList", new { Tid = TempData["tenderid"] });
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
    }
}