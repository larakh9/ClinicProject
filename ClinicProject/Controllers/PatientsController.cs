using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicProject.Data;
using ClinicProject.Models;
using ClinicProject.NewFolder;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ClinicProject.Paging;

namespace ClinicProject.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CountryModel>> Countries()
        {
            string Baseurl = "https://restcountries.eu/rest/v2/all";
            List<CountryModel> country = new List<CountryModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync(Baseurl);

                if (Res.IsSuccessStatusCode)
                {
                    var CountryResponse = Res.Content.ReadAsStringAsync().Result;
                    country = JsonConvert.DeserializeObject<List<CountryModel>>(CountryResponse);
                }
            }
                return country;
            }


        /*public IActionResult Index(string sortField, string currentSortField, string currentSortOrder, string currentFilter, int? pageNo)
        {
            //var pats = _context.Patients;
            
            ViewData["CurrentSort"] = sortField;
            int pageSize = 10;
            return View(PagingList<Patient>.CreateAsync(pats.AsQueryable<Patient>(), pageNo ? ? 1, pageSize));
        }*/

        // GET: Patients
        public IActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "PatientName_desc" : "";

            var pats = from d in _context.Patients
                       select d;
            switch (sortOrder)
            {
                case "PatientName_desc":
                    pats = pats.OrderByDescending(d => d.PatientName);
                    break;
                default:  // Name ascending 
                    pats = pats.OrderBy(d => d.PatientName);
                    break;
            }

            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(PagingList<Patient>.CreateAsync(pats.AsQueryable<Patient>(), pageNumber, pageSize));
            //return View(pats.ToPagedList(pageNumber, pageSize));
           // return View(await pats.ToListAsync());
            //return View(await _context.Patients.ToListAsync());
        } 

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public async Task<IActionResult> Create()
        {
                ViewData["Country"] = new SelectList(await this.Countries(), "Name", "Name");

                return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Country,DOB,Gender,PhoneNum,Email,Address,RegDate,SSN")] Patient patient, CountryModel country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Country"] = new SelectList(await this.Countries(), "Name", "Name", country.Name);
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["Country"] = new SelectList(await this.Countries(), "Name", "Name");
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,FirstName,LastName,Country,DOB,Gender,PhoneNum,Email,Address,RegDate,SSN")] Patient patient, CountryModel country)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Country"] = new SelectList(await this.Countries(), "Name", "Name", country.Name);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(long id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }

        //Here I added the search 
        public async Task<IActionResult> Search(string searchString)
        {
            var pats = from p in _context.Patients
                        select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                pats = pats.Where(s  => s.PatientName.Contains(searchString) || s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
            }

            return View(await pats.ToListAsync());
        }
    }
}
