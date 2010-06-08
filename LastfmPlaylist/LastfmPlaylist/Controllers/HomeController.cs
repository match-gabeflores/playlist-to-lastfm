using System.IO;
using System.Web;
using System.Web.Mvc;
using LastfmPlaylist.Models;

//todo m3u /m3u8 files only in open dialog

namespace LastfmPlaylist.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        //[OutputCache(Duration = 15, VaryByParam = "yes")]
        public ActionResult Index()
        {
            if (TempData["output"] == null)
                TempData["output"] = string.Empty;
            
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase fileUpload, bool youtube, bool wikipedia, bool myspace)
        {
            // which checkboxes are checked
            // youtube api?
            if (fileUpload != null && fileUpload.ContentType == "audio/mpegurl")
            {
    //            uploadedFile.InputStream
                if (Request.Files[0] != null)
                {
                    var playlistFile = Request.Files[0];
                    //todo : error check only m3u files, only text files
                    string input = new StreamReader(playlistFile.InputStream).ReadToEnd();

                    Playlist.MySpaceLink = myspace;
                    Playlist.YouTubeLink = youtube;
                    Playlist.WikipediaLink = wikipedia;
                    TempData["output"] = Playlist.Convert(input);
                }
             }
                
                return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            return View();
        }

        public ContentResult GetContent()
        {
            if (TempData["output"] == null)
                TempData["output"] = "null";

            string output = TempData["output"] as string;
            return Content(output);
        }
    }
}
