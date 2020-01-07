using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Cl.Blog.Web.Areas.Api.Controllers
{
    public class FilesController : ApiController
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object UploadImage()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                HttpRequest currentRequest = HttpContext.Current.Request;
                HttpFileCollection files = currentRequest.Files;
                if (files.Count > 0)
                {   //上传的文件
                    HttpPostedFile file = currentRequest.Files[files.AllKeys[0]];
                    string initPath = "/upload/";
                    string path = currentRequest.MapPath(initPath);
                    string fileName = file.FileName;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    file.SaveAs(path + fileName);
                    var CKEditorFuncNum = System.Web.HttpContext.Current.Request["CKEditorFuncNum"];

                    //上传成功后，我们还需要通过以下的一个脚本把图片返回到第一个tab选项
                    //webapi需要通过这种方式返回执行才会执行js
                    HttpResponseMessage responseMessage = new HttpResponseMessage();
                    responseMessage.Content = new StringContent("<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + initPath + fileName + "\", '');</script>");
                    responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                    return responseMessage;
                }
                return null;
            }
            catch (Exception ex)
            {
                return Json(new { result_code = 0, msg = ex.Message });
            }
        }
    }
}
