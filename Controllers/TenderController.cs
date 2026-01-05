using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using Design_and_Supervion_Issue_Tracking.Models;


namespace Design_and_Supervion_Issue_Tracking.Controllers
{
    public class TenderController : Controller
    {

        Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider issuedb = new Design_and_Supervion_Issue_Tracking.Repository_Classes.DataAccessprovider();
        Repository_Classes.Encryption Encryption = new Repository_Classes.Encryption();
        string Error = "";
        public ActionResult Index(int DepId=0,int EmpId=0)
        {
            try
            {
                DataTable dt = new DataTable();
                List<ModelTender> tenderdetails = new List<ModelTender>();

                if (DepId != 0 && EmpId != 0)
                {
                    var param = new ArrayList()
                     {
                        "@EmpId",
                        "@ToDepId"
                     };
                    var values = new ArrayList()
                    {
                       DepId, EmpId
                    };

                    //dt = issuedb.ExecuteDataTable("dbo", "GetTenderByDepartment", param, values, ref Error);
                    dt = issuedb.ExecuteDataTable("dbo", "GetTenderByDepHead", param, values, ref Error);



                    if (dt.Rows.Count < 1)
                    {
                        var param1 = new ArrayList()
                        {
                        "@EmpId","@Department"
                        };
                        var values1 = new ArrayList()
                        {

                             EmpId,
                              DepId
                        };
                        dt = issuedb.ExecuteDataTable("dbo", "GetTenderByEmployeeId", param1, values1, ref Error);


                    }
                }
                else
                {

                    dt = issuedb.ExecuteDataTable("dbo", "GetTender", ref Error);
                }

                    tenderdetails = issuedb.ConvertDataTable<ModelTender>(dt);
                 return View(tenderdetails.ToList());
                //return View();

            }
            catch (Exception e)
            {
                //strErrMsg = e.Message;
                var dir = System.Web.HttpContext.Current.Server.MapPath("~\\count");
                var file = Path.Combine(dir, "count.txt");

                Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(file, e.Message);
                return View("Error", new HandleErrorInfo(e, "Tender", "Error"));


            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {


            ViewBag.ClientName = issuedb.combofill("dbo", "getClient", "ClientName", "ClientId", ref Error);
            ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
            ViewBag.AssignedTeam = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);

            DataTable dt = new DataTable();

            var param = new ArrayList()
            {
                "@id"
            };
            var values = new ArrayList() {
                id
            };
            dt = issuedb.ExecuteDataTable("dbo", "GetTenderbyId", param, values, ref Error);

            if (dt.Rows.Count == 1)
            {
               // int k = Convert.ToInt32(dt.Rows[0][2].ToString());
               // string h = Convert.ToString(dt.Rows[0][3].ToString());
               // int p = Convert.ToInt32(dt.Rows[0][5].ToString());
               // string d = Convert.ToString(dt.Rows[0][6].ToString());
               // int AssignedTeamId = Convert.ToInt32(dt.Rows[0][7].ToString());
               // string AssignedTeam = Convert.ToString(dt.Rows[0][8].ToString());
               // int EmployeeId = Convert.ToInt32(dt.Rows[0][9].ToString());
              //  string Responsible_Personnel = Convert.ToString(dt.Rows[0][10].ToString());


                ModelTender issuem = new ModelTender()
                {
                    id = Convert.ToInt32(dt.Rows[0][0].ToString()),
                    InitiationType= Convert.ToString(dt.Rows[0][1].ToString()),
                    TenderDocNumber = Convert.ToString(dt.Rows[0][2].ToString()),                    
                   // ClientNo = Convert.ToInt32(dt.Rows[0][2].ToString()),                   
                    ClientName = Convert.ToString(dt.Rows[0][3].ToString()),
                    ProjectName = Convert.ToString(dt.Rows[0][5].ToString()),                  
                   // ReceiverId = Convert.ToInt32(dt.Rows[0][5].ToString()),                  
                    ReceivedBy = Convert.ToString(dt.Rows[0][6].ToString()),                 
                   // AssignedTeamId = Convert.ToInt32(dt.Rows[0][7].ToString()),               
            
                    LetterDate = Convert.ToDateTime(dt.Rows[0][8].ToString()),
                    RegisterationDate = Convert.ToDateTime(dt.Rows[0][9].ToString()),
                    TenderClosingDate = Convert.ToDateTime(dt.Rows[0][10].ToString()),
                    TypeOfWork = Convert.ToString(dt.Rows[0][11].ToString()),
                    Main_Attachement = Convert.ToString(dt.Rows[0][12].ToString()),
                    Other_Attachment = Convert.ToString(dt.Rows[0][13].ToString()),
                    Remark = Convert.ToString(dt.Rows[0][14].ToString()),
                    Status = Convert.ToString(dt.Rows[0][15].ToString()),
                    LastEditedBy = Convert.ToString(dt.Rows[0][16].ToString()),
                    IsDelete= 0

                };
                string a = Convert.ToString(dt.Rows[0][12].ToString());
                string b= Convert.ToString(dt.Rows[0][13].ToString());
                Session["torpath"] = Convert.ToString(dt.Rows[0][12].ToString());
                Session["Otherpath"] = Convert.ToString(dt.Rows[0][13].ToString());
               // ViewBag.Selectedproject = Convert.ToString(dt.Rows[0][4].ToString());

                ViewBag.SelectedClient = Convert.ToString(dt.Rows[0][4].ToString());

                ViewBag.SelectedTypeOfWork = Convert.ToString(dt.Rows[0][11].ToString());
                ViewBag.SelectedStatus = Convert.ToString(dt.Rows[0][15].ToString());
                return View(issuem);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public ActionResult Edit(ModelTender obj)
        {

            HttpFileCollectionBase file = Request.Files;
            bool a = false;
            //obj.Project_Id= issuedb.combofill("dbo", "getProject", "ProjectName", "Project_Id", ref Error)
            if (file[0].FileName.ToString() != "")
            {
                string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));

                file[0].SaveAs(path);
                obj.Main_Attachement = "~/img/" + Path.GetFileName(file[0].FileName);

            }
           
            if (file[1].FileName.ToString() != "")
            {
                string path1 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[1].FileName));

                file[1].SaveAs(path1);
                obj.Other_Attachment = "~/img/" + Path.GetFileName(file[1].FileName);

            }
            else
            {
                obj.Other_Attachment = "-";
            }
            if(obj.TenderClosingDate == null)
            {
                obj.TenderClosingDate = Convert.ToDateTime("1/1/2099");
            }
            string r = Convert.ToString(Request["Type"].ToString());
            //Convert.ToString(Request["ResponsibleDepartmentid"].ToString()), Convert.ToString(Request["DepheadId"].ToString()),  obj.DepartmentHead,Convert.ToString(Request["BranchId"].ToString()),obj.Branch,obj.FollowerName,Convert.ToString(Request["FollowerId"].ToString()),Convert.ToString(Request["Sevirity"].ToString()),Convert.ToDateTime(Request["IssueLetterDa

            var errors = ModelState
              .Where(x => x.Value.Errors.Count > 0)
              .Select(x => new { x.Key, x.Value.Errors })
              .ToArray();
            if (ModelState.IsValid)
            {
                var param = new ArrayList()
            {
               "@id","@InitiationType","@TenderDocNumber","@ClientNo","@ClientName","@ProjectName","@LetterDate","@TenderClosingDate","@TypeOfWork","@Main_Attachement","@Other_Attachment","@Status","@Remark","@LastEditedBy"
            };
                var values = new ArrayList()
            {
               obj.id,obj.InitiationType,obj.TenderDocNumber ,obj.ClientName,Convert.ToString(Request["cNo"].ToString()),obj.ProjectName,obj.LetterDate,obj.TenderClosingDate,Convert.ToString(Request["Type"].ToString()),obj.Main_Attachement,obj.Other_Attachment,Convert.ToString(Request["Selstatus"].ToString()),obj.Remark,Session["Fullname"]



                };


                a = issuedb.ExecuteNonQuery("dbo", "UpdateTender", param, values, ref Error);
            }
            if (a)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.msg = "<script>alert('Tender update failed') </script>";
                ModelState.Clear();
                ViewBag.ClientName = issuedb.combofill("dbo", "getClient", "ClientName", "ClientId", ref Error);
                ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
                ViewBag.AssignedTeam = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
            }
            return View();

        }
        public ActionResult GetSelectedEmployee(int Id = 0, int SubDepartment = 0)
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
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(ModelAdmin obj)
        {
            DataTable dt = new DataTable();
            try
            {
                //SHA256,SHA384,SHA512,
                //DataTable dt = new DataTable();
                var param = new ArrayList()
                 {
                "@username",
                //"@password"
                "@EmployeeId"
                 };
                var values = new ArrayList() {
                obj.Username,
                obj.Password
                };
                //string l = issuedb.encryption(obj.Password);



                dt = issuedb.ExecuteDataTable("dbo", "Getuserlogin1", param, values, ref Error);
                if (dt.Rows.Count >= 1)
                {
                    Session["EmpID"] = Convert.ToString(dt.Rows[0][0].ToString());
                    //string j = Convert.ToString(Session["EmpID"]);
                    Session["Fullname"] = Convert.ToString(dt.Rows[0][1].ToString());
                    //j = Convert.ToString(Session["Fullname"]);
                    Session["DepartmentID"] = Convert.ToString(dt.Rows[0][2].ToString());
                    Session["DepartmentName"] = Convert.ToString(dt.Rows[0][3].ToString());
                    Session["SubDepartmentID"] = Convert.ToString(dt.Rows[0][4].ToString());
                    Session["SubDepartmentName"] = Convert.ToString(dt.Rows[0][5].ToString());
                    Session["PositionId"] = Convert.ToString(dt.Rows[0][6].ToString());
                    string a = Convert.ToString(Session["EmpID"]);
                    if (Convert.ToInt32(Session["SubDepartmentID"]) == 187)
                    {
                        Session["role"] = "Admin";
                        return RedirectToAction("Index", new   {
                            
                            
                           // id = Convert.ToInt32(Session["EmpID"]),
                          //  subid = Convert.ToInt32(Session["SubDepartmentID"])
                        });

                    }
                    else if(Convert.ToInt32(Session["SubDepartmentID"]) == 198)
                    {
                        if (Convert.ToInt32(Session["PositionId"]) == 6697)
                        {
                            Session["role"] = "AdminUser";
                            return RedirectToAction("Index", new  {

                               
                                // id = Convert.ToInt32(Session["EmpID"]),
                                DepId = Convert.ToInt32(Session["SubDepartmentID"]),
                                Empid = Convert.ToInt32(Session["EmpId"])



                            });

                           // return View("Error", new HandleErrorInfo(e, "Tender", "Error"));
                        }
                        else
                        {
                        
                                Session["role"] = "User";
                                return RedirectToAction("Index", new
                                {

                                                               
                                    DepId = Convert.ToInt32(Session["SubDepartmentID"]),
                                    Empid = Convert.ToInt32(Session["EmpId"])
                                });




                        }
                    }
                 
                    else if (Convert.ToInt32(Session["SubDepartmentID"]) == 199 )
                    {
                        if (Convert.ToInt32(Session["PositionId"]) == 4233)
                        {
                            Session["role"] = "AdminUser";
                            return RedirectToAction("Index", new
                            {

                               
                                // id = Convert.ToInt32(Session["EmpID"]),
                                DepId = Convert.ToInt32(Session["SubDepartmentID"]),
                                Empid = Convert.ToInt32(Session["EmpId"])
                            });
                        }
                        else
                        {
                            Session["role"] = "User";
                            return RedirectToAction("Index", new
                            {

                               
                                // id = Convert.ToInt32(Session["EmpID"]),
                                DepId = Convert.ToInt32(Session["SubDepartmentID"]),
                                Empid = Convert.ToInt32(Session["EmpId"])
                            });
                        }
                    }

                    else if (Convert.ToInt32(Session["SubDepartmentID"]) == 200)
                    {
                        Session["role"] = "Legal";
                        return RedirectToAction("Index","Legal", new
                        {

                            Tid = 0,

                        }); 
                    }
                    else
                    {
                        Session["role"] = "Others";
                        return RedirectToAction("Index", "Request", new
                        {
                           
                            Eid = Convert.ToInt32(Session["EmpID"]),
                            Dept = Convert.ToInt32(Session["SubDepartmentID"])

                        }); ;
                    }
                }
            }

            catch (Exception e)
            {
                var dir = System.Web.HttpContext.Current.Server.MapPath("~\\count");
                var file = Path.Combine(dir, "count.txt");

                Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(file, "Tender" + e.Message);
            }

            return View();

        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.ClientName = issuedb.combofill("dbo", "getClient", "ClientName", "ClientId", ref Error);
                ViewBag.Responsible_Personnel = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);
                ViewBag.AssignedTeam = issuedb.combofill("dbo", "GetSubDep", "SubDepartmentName", "SubDepartmentId", ref Error);
            }
            catch (Exception e)
            {
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(ModelTender obj)
        {

            List<SelectListItem> list = new List<SelectListItem>();
            bool a = false;
            HttpFileCollectionBase file = Request.Files;
            try {


                if (file[0].FileName.ToString() != "")
                {
                    string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[0].FileName));

                    file[0].SaveAs(path);
                    obj.Main_Attachement = "~/img/" + Path.GetFileName(file[0].FileName);

                }

                if (file[1].FileName.ToString() != "")
                {
                    string path1 = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file[1].FileName));

                    file[1].SaveAs(path1);
                    obj.Other_Attachment = "~/img/" + Path.GetFileName(file[1].FileName);

                }
                else
                {
                    obj.Other_Attachment = "-";
                }
                if (obj.TenderClosingDate ==Convert.ToDateTime(" 1 / 1 / 0001 12:00:00 AM"))
                {
                    obj.TenderClosingDate = Convert.ToDateTime("1/1/2099");
                }

                                                                                                                                        
               var errors = ModelState
             .Where(x => x.Value.Errors.Count > 0)
             .Select(x => new { x.Key, x.Value.Errors })
             .ToArray();
                if (ModelState.IsValid)
                {
                    var param = new ArrayList()
                    {
                                               
                        "@InitiationType","@TenderDocNumber","@ClientNo","@ClientName","@ProjectName","@ReceiverId","@RecivedBy","@LetterDate","@RegisterationDate","@TenderClosingDate","@TypeOfWork","@Main_Attachement","@Other_Attachment","@Remark","@Status","@LastEditedBy","@IsDelete"
                    
                    };
                var values = new ArrayList()
                    {
                        obj.InitiationType, obj.TenderDocNumber,obj.ClientName,Convert.ToString(Request["clientno"].ToString()),obj.ProjectName,Convert.ToInt32(Session["EmpId"]),Convert.ToString(Session["Fullname"]), obj.LetterDate,DateTime.Now.ToShortDateString(),obj.TenderClosingDate,Convert.ToString(Request["worktype"].ToString()),obj.Main_Attachement,obj.Other_Attachment,obj.Remark,"Active",Convert.ToString(Session["Fullname"]),0
                    };

                    a = issuedb.ExecuteNonQuery("dbo", "AddTender", param, values, ref Error);
                   
                } 


                if (a)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                     ViewBag.msg = "<script>alert('Issue not saved') </script>";
                   
                    ViewBag.ClientName = issuedb.combofill("dbo", "getClient", "ClientName", "ClientId", ref Error);
                    ViewBag.ReceivedBy = issuedb.combofill("dbo", "getEmployee", "EmployeeName", "EmployeeId", ref Error);

                }
                return View();



            }
            catch(Exception ex) {
                 return View("Error", new HandleErrorInfo(ex, "Tender", "Error"));
            }
            finally
            {
                
            }

        }

        public ActionResult DownloadFile(string filePath)
        {

            try
            {
                //string Error = "";
                string fullName = Server.MapPath(filePath);
                byte[] fileBytes = issuedb.GetFile(fullName);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
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
