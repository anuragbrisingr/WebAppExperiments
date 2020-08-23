using ReactDemo.Models;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;

namespace ReactDemo.Controllers
{
    public class HomeController : Controller
    {
        private static readonly IList<CommentModel> _comments;

        static HomeController()
        {
            _comments = new List<CommentModel>
            {
                new CommentModel
                {
                    Id = 1,
                    Author = "Rama Krishna",
                    Text = "Namaste! React world, this is Rama Krishna."
                },
                new CommentModel
                {
                    Id = 2,
                    Author = "Raghu Ram",
                    Text = "Comment by Raghu."
                },
                new CommentModel
                {
                    Id = 3,
                    Author = "Arjuna Veera",
                    Text = "Comment."
                }
            };
        }

        public ActionResult Index()
        {
            return View();
        }

        // OutputCache attribute is used to prevent browsers from Caching the response.
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Comments()
        {
            return Json(_comments, JsonRequestBehavior.AllowGet);
        }
    }
}