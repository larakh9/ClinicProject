using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicProject.Data;
using ClinicProject.Models;
using PagedList;
using ClinicProject.NewFolder;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using ClinicProject.Paging;

namespace ClinicProject.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
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
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var applicationDbContext = _context.Doctors.Include(d => d.Specialization);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "DoctorName_desc" : "";


            var docs = from d in applicationDbContext
                       select d;
            switch (sortOrder)
            {
                case "DoctorName_desc":
                    docs = docs.OrderByDescending(d => d.DoctorName);
                    break;
                default:  // Name ascending 
                    docs = docs.OrderBy(d => d.DoctorName);
                    break;
            }

            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(PagingList<Doctor>.CreateAsync(docs.AsQueryable<Doctor>(), pageNumber, pageSize));
            //return View(docs.ToPagedList(pageNumber, pageSize));

            //return View(await docs.ToListAsync());

        }

        // GET: Doctors
        /*public async Task<IActionResult> Index(string sortOrder)
        {
            var applicationDbContext = from s in _context.Doctors
                                       select s;
            //_context.Doctors.Include(d => d.Specialization);
            //return View(await applicationDbContext.ToListAsync());
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "DoctorName_desc" : "";
            var docs = from d in _context.Doctors
                       select d;
            switch (sortOrder)
            {
                case "DoctorName_desc":
                    docs = docs.OrderByDescending(d => d.DoctorName);
                    applicationDbContext = _context.Doctors.Include(d => d.Specialization);
                    break;
                default:
                    docs = docs.OrderBy(d => d.DoctorName);
                    applicationDbContext = _context.Doctors.Include(d => d.Specialization);
                    break;
            }
            return View(await docs.ToListAsync());
        }*/

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public async Task<IActionResult> Create()
        {
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "SpecializationName");
            ViewData["Country"] = new SelectList( await this.Countries(), "Name", "Name");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Country,Address,Notes,PhoneNum,MonthlySalary,Email,IBAN,SpecializationId")] Doctor doctor, CountryModel country
            )
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "Id", doctor.SpecializationId);
            ViewData["Country"] = new SelectList(await this.Countries(), "Name", "Name", country.Name);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "SpecializationName", doctor.SpecializationId);
            ViewData["Country"] = new SelectList(await this.Countries(), "Name", "Name");
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,FirstName,LastName,Country,Address,Notes,PhoneNum,MonthlySalary,Email,IBAN,SpecializationId")] Doctor doctor, CountryModel country)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
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
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "Id", doctor.SpecializationId);
            ViewData["Country"] = new SelectList(await this.Countries(), "Name", "Name", country.Name);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(long id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }

        // This is an attempt to add a search bar in the Doctors tab
        public async Task<IActionResult> Search(string searchString)
        {
            var docs = from d in _context.Doctors
                         select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                docs = docs.Where(s => s.DoctorName.Contains(searchString) || s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
            }

            return View(await docs.ToListAsync());
        }

        // This is an attemp to add the sorting method
       /* public async Task<IActionResult> Sort(string sortOrder)
        {
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "DoctorName_desc" : "";
            //ViewBag.IDSort = sortOrder == "Id" ? "Id_desc" : "Id";
            var docs = from d in _context.Doctors
                       select d;
            switch (sortOrder)
            {
                case "DoctorName_desc":
                    docs = docs.OrderByDescending(d => d.DoctorName);
                    break;
               case "Id":
                    docs = docs.OrderBy(d => d.Id);
                    break;
                case "Id_desc":
                    docs = docs.OrderByDescending(d => d.Id);
                    break; 
                default:
                    docs = docs.OrderBy(d => d.DoctorName);
                    break;
            }
            return View(await docs.ToListAsync());
        }*/
    }
}
