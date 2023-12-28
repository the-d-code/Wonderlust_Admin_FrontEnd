using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WONDERLUST_PROJECT_ADMINs.Models;

namespace WONDERLUST_PROJECT_ADMINs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //private string baseurl = "http://localhost:5275/api/Country";
        private string baseurl = "http://localhost:5275/Auth/Login";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

      
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: BooksController/Create
        public ActionResult Login()
        {
            return View();
        }

        // POST: BooksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Users user)
        {
            try
            {
                StringContent content
                    = new StringContent(JsonConvert.SerializeObject(user),
                    Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var Response = await httpClient.PostAsync(baseurl, content);

                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return Index();
            }
        }


        




    }
}
