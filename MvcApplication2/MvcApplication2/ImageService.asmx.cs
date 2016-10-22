using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace MvcApplication2
{
    /// <summary>
    /// Summary description for ImageService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ImageService : System.Web.Services.WebService
    {

        [WebMethod]
        public void GetImages()
        {
            
            //var cs = ConfigurationManager.ConnectionStrings["StevenCorreia_dbEntities"].ConnectionString;
            //using (var con = new SqlConnection(cs))
            //{
            //    var cmd = new SqlCommand("select * from Users", con);
            //    var da = new SqlDataAdapter(cmd);
            //    var ds = new DataSet();
            //    da.Fill(ds);

            //    images.AddRange(from DataRow imageRow in ds.Tables[0].Rows
            //                    select new image
            //                    {
            //                        imageID = Convert.ToInt32(imageRow["imageID"]),
            //                        imageName = imageRow["imageName"].ToString(),
            //                        cover = imageRow["cover"].ToString(),
            //                        createdDate = Convert.ToDateTime(imageRow["createdDate"]),
            //                        dislikes = Convert.ToInt32(imageRow["dislikes"]),
            //                        likes = Convert.ToInt32(imageRow["likes"])
            //                    });
            //}
            List<image> images;
            using (var context = new StevenCorreia_dbEntities())
            {
                images = (from image in context.images
                          select image).ToList();
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(images));
        }
    }
}
