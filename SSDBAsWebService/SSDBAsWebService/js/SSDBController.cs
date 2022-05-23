using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.Profiling;


    public class SSDBController : Controller
    {
        //
        // GET: /SSDB/

        public  ActionResult Index()
        {
            MiniProfiler.Start();
            var profiler = MiniProfiler.Current;
            using (profiler.Step(" Welcome"))
            {
            }
            MiniProfiler.Stop();
            return View();
        }

        [HttpGet]
        public  JsonResult Info()
        {
            ServerInfo S = new ServerInfo();
            SSDBConsoleHost C = new SSDBConsoleHost();
            MiniProfiler.Start();
            var profiler = MiniProfiler.Current;
            string[] T = { "" };
            using (profiler.Step(" Get Information relating to default SSDB server in this application."))
            {
                T =  S.Info(C);

            }
            MiniProfiler.Stop();

            return Json(T, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public  JsonResult GetDetails(SSDBInfo ipandport)
        {
            ServerInfo S = new ServerInfo();
            MiniProfiler.Start();
            var profiler = MiniProfiler.Current;
            KeyInfo T = new KeyInfo();


            using (profiler.Step(" Get the names of all Hashes, SortedSets, List and Keyvaluepairs."))
            {
                T =  S.All(ipandport);

            }
            MiniProfiler.Stop();
            return Json(T, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetInfo(SSDBInfo ipandport)
        {
            ServerInfo S = new ServerInfo();
            SSDBConsoleHost C = new SSDBConsoleHost();
            C.ipinfo = ipandport.ipadd;
            C.portinfo = int.Parse(ipandport.portno);
            MiniProfiler.Start();
            var profiler = MiniProfiler.Current;
            string[] T = { "" };
            using (profiler.Step(" Get Information relating to added SSDB server in this application."))
            {
                T =  S.Info(C);

            }
            MiniProfiler.Stop();
            return Json(T);
        }
        [HttpPost]
        public JsonResult GetKey(GetKeyInfo typeandname)
        {
            ServerInfo S = new ServerInfo();
            KeyDetails T = new KeyDetails();
            MiniProfiler.Start();
            var profiler = MiniProfiler.Current;

            using (profiler.Step(" Get Details of Hashes, SortedSets, List and Keyvaluepairs."))
            {
                T = S.GetKeyDetails(typeandname);
            }
            MiniProfiler.Stop();
            return Json(T);

        }

    }

