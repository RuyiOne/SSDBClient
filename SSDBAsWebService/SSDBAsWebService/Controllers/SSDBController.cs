using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Profiling;

namespace SSDBAsWebService.Controllers
{
    public class SSDBController : AsyncController
    {
        //
        // GET: /SSDB/
        public string ipinfo { get; set; }
        public int portinfo { get; set; }
        public async Task<ActionResult> Index()
        {
            MiniProfiler.Start();
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Async Welcome"))
            {
            }
            MiniProfiler.Stop();
           return View();
        }

        [HttpGet]
        public async Task<JsonResult> Info()
        {
            ServerInfo S = new ServerInfo();
            SSDBConsoleHost C = new SSDBConsoleHost(ipinfo, portinfo);
                     

            
            MiniProfiler.Start();
            var profiler = MiniProfiler.Current;
            string[] T = {""};
            using (profiler.Step("Async Get Information relating to default SSDB server in this application."))
            {
                try
                {
                    T = await S.Info(C);
                }
                catch(Exception ex)
                {
                    
                }

            }
            MiniProfiler.Stop();

            return Json(T, JsonRequestBehavior.AllowGet);
           
        }
        [HttpPost]
        public async Task<JsonResult> GetDetails(SSDBInfo ipandport)
        {
            ServerInfo S = new ServerInfo();
            MiniProfiler.Start();
            var profiler = MiniProfiler.Current;
            KeyInfo T = new KeyInfo();


            using (profiler.Step("Async Get the names of all Hashes, SortedSets, List and Keyvaluepairs."))
            {

                try
                {
                    T = await S.All(ipandport);
                }
                catch (Exception ex)
                {

                }
                
                
            }
            MiniProfiler.Stop();
            return Json(T, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GetInfo(SSDBInfo ipandport)
        {
            ServerInfo S = new ServerInfo();
            SSDBConsoleHost C = new SSDBConsoleHost(ipandport.ipadd, int.Parse(ipandport.portno));
            try
            {
            using (C)
            {

            


                ipinfo = ipandport.ipadd;
                portinfo = int.Parse(ipandport.portno);
                //SSDBConsoleHost C = new SSDBConsoleHost() { ipinfo = ipandport.ipadd, portinfo = 0} ;



                MiniProfiler.Start();
                var profiler = MiniProfiler.Current;
                string[] T = { "" };
                using (profiler.Step("Async Get Information relating to added SSDB server in this application."))
                {



                    try
                    {
                        T = await S.Info(C);
                    }
                    catch (Exception ex)
                    {

                    }

                }
                MiniProfiler.Stop();
                return Json(T);
            }
            }
            catch(Exception ex)
            {
                return null;
            }
            
                
            
            
        }
        [HttpPost]
        public async Task<JsonResult> GetKey(GetKeyInfo typeandname)
        {
            ServerInfo S = new ServerInfo();
            KeyDetails T = new KeyDetails();
            MiniProfiler.Start();
            var profiler = MiniProfiler.Current;

            using (profiler.Step("Async Get Details of Hashes, SortedSets, List and Keyvaluepairs."))
            {

                try
                {
                    T = await S.GetKeyDetails(typeandname, new SSDBInfo() { ipadd = ipinfo, portno = portinfo.ToString(), tablelength = 0 }).ConfigureAwait(false); 
                }
                catch (Exception ex)
                {

                }
                 
            }
            MiniProfiler.Stop();
            return Json(T);

        }

        public async Task<JsonResult> GetRequest(GetRequestArgs args)
        {
             MiniProfiler.Start();
             SSDBConsoleHost C = new SSDBConsoleHost(ipinfo, portinfo);
            
            var profiler = MiniProfiler.Current;
            var T = new List<byte[]>();
            string[] S ;
            int i = 0;
            using (profiler.Step("Async Get Custom Request '" + args.parameters[0] + "' " + args.parameters[1] + "."))
            {

                try
                {
                    T = C._request(args.cmd, args.parameters, new SSDBInfo() { ipadd = ipinfo, portno = portinfo.ToString(), tablelength = 0 });
                    S = new string[T.Count];
                    foreach (byte[] bb in T){                        
                        S[i]= Encoding.UTF8.GetString(bb);
                        i++;
                    }

                }
                catch (Exception ex)
                {
                    S = new string[1];
                    S[0] = null;
                }

            }
            MiniProfiler.Stop();

            return Json(S);
        }
    }
}
