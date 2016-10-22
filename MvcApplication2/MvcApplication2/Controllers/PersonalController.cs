using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Antlr.Runtime;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class PersonalController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();

        }
        [HttpPost]
        public JsonResult SaveFiles(string description)
        {
            string fileName = string.Empty;
            bool flag = true;
            bool fileAlreadyPresent = false;
            if (Request.Files != null)
            {
                var myFile = Request.Files[0];
                if (myFile != null)
                {
                    //actualFileName = file.FileName;
                    //fileName = Guid.NewGuid() + Path.GetExtension(myFile.FileName);

                    try
                    {
                        fileName = Path.GetFileName(myFile.FileName);
                        //int size = file.ContentLength;
                        if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images"), fileName)))
                        {
                            //System.IO.File.Delete(Path.Combine(Server.MapPath("~/Images"), fileName));
                            fileAlreadyPresent = true;
                        }
                        else
                        {
                            myFile.SaveAs(Path.Combine(Server.MapPath("~/Images"), fileName));
                            flag = true;
                        }

                        //UploadedFile f = new UploadedFile
                        //{
                        //    FileName = actualFileName,
                        //    FilePath = fileName,
                        //    Description = description,
                        //    FileSize = size
                        //};
                        //using (MyDatabaseEntitiesEntities dc = new MyDatabaseEntitiesEntities())
                        //{
                        //    dc.UploadedFiles.Add(f);
                        //    dc.SaveChanges();
                        //    Message = "File uploaded successfully";
                        //    flag = true;
                        //}
                    }
                    catch (Exception)
                    {
                        //message = "File upload failed! Please try again";
                        flag = false;
                    }
                }
            }
            return new JsonResult
            {
                Data =
                    new
                    {
                        FileAlreadyPresent = fileAlreadyPresent,
                        Flag = flag,
                        FileName = description,
                        FilePath = "Images/" + fileName
                    }
            };
        }

        [HttpPost]
        public JsonResult DeleteFiles(string imageName)
        {
            bool flag = true;
            if (Request.Files != null)
            {

                //actualFileName = file.FileName;
                //fileName = Guid.NewGuid() + Path.GetExtension(myFile.FileName);

                try
                {
                    var fileName = Path.GetFileName(imageName);
                    //int size = file.ContentLength;
                    if (fileName != null && System.IO.File.Exists(Path.Combine(Server.MapPath("~/Images"), fileName)))
                    {
                        System.IO.File.Delete(Path.Combine(Server.MapPath("~/Images"), fileName));
                    }
                }
                catch (Exception)
                {
                    flag = false;
                }

            }
            return new JsonResult{Data =new{Flag = flag}};
        }
    }
}
