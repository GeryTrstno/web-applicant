using LearningApplicantWeb.Models.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningApplicantWeb.Controllers
{
    [Authorize]
    public class ApplicantController : Controller
    {
        private readonly ModelContext _context;

        public ApplicantController(ModelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Mengambil semua data pelamar dari database
            var applicants = await _context.Applicants.Include(a => a.Position).ToListAsync();
            return View(applicants);
        }

        public IActionResult Create()
        {
            // Cukup tampilkan halaman form pembuatan data
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Applicant applicant)
        {
            // 'Bind' digunakan untuk keamanan, hanya menerima properti yang disebutkan
            if (ModelState.IsValid)
            {

                // Logika tambahan seperti generate RegisterCode, set CreatedBy, dll.
                applicant.RegisterCode = $"REG-{System.Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
                applicant.CreatedBy = User.Identity.Name; // Mengambil username dari user yang login
                applicant.CreatedAt = DateTime.Now;

                _context.Add(applicant);
                await _context.SaveChangesAsync();
                // Arahkan ke halaman sukses atau dashboard pelamar
                return RedirectToAction("Home", "Index"); // Atau halaman lain yang sesuai
            }
            // Jika model tidak valid, kembali ke form dengan data yang sudah diisi
            return View(applicant);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant == null) return NotFound();
            return View(applicant);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Applicant applicant)
        {
            if (id != applicant.ApplicantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicant.UpdatedBy = User.Identity.Name;
                    applicant.UpdatedAt = DateTime.UtcNow;
                    _context.Update(applicant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Applicants.Any(e => e.ApplicantId == applicant.ApplicantId))
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
            return View(applicant);
        }

        [HttpPost]
        public async Task<IActionResult> Destroy(int id)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant == null) return NotFound();

            _context.Applicants.Remove(applicant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
