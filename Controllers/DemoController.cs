using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using WONDERLUST_PROJECT_ADMINs.Models;
using Microsoft.EntityFrameworkCore;

namespace WONDERLUST_PROJECT_ADMINs.Controllers
{
    public class DemoController : Controller
    {
        //private static List<Country> countries = new List<Country>();

        //private HttpClient httpClient = new HttpClient();

        //private string url = "";
        private readonly DBWONDERLUSTContext _context;

        public DemoController(DBWONDERLUSTContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> CountryAll()
        {
            var city = _context.City.Include(f => f.State);

            return View(await city.ToListAsync());

        }




        //public DemoController()
        //{
        //    url = @"http://localhost:5275/api/Country";
        //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //}

        //// GET: CourseController
        //public async Task<ActionResult> Index()
        //{
        //    var msg = await httpClient.GetAsync(url);
        //    var Response = msg.Content.ReadAsStringAsync();

        //    countries = JsonConvert.DeserializeObject<List<Country>>(Response.Result);

        //    return View(countries);
        //}

    }
}
