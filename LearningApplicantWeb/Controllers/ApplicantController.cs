using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LearningApplicantWeb.Models.EF; // Ganti dengan namespace DbContext Anda

namespace LearningApplicantWeb.Controllers
{
    public class ApplicantController : Controller
    {
        // Ganti 'YourDbContext' dengan nama kelas DbContext Anda
        private readonly ModelContext _context;

        // Injeksi DbContext melalui constructor
        public ApplicantController(ModelContext context)
        {
            _context = context;
        }

        // GET: Applicant/Index
        // Menampilkan daftar semua pelamar yang belum dihapus (DeletedAt == null)
        public async Task<IActionResult> Index()
        {
            var applicants = await _context.Applicants
                                           .Include(a => a.Position) // Mengambil data relasi JobPosition
                                           .Where(a => a.DeletedAt == null) // Filter untuk soft delete
                                           .ToListAsync();
            return View(applicants);
        }

        // GET: Applicant/Create
        // Menampilkan form untuk membuat lamaran baru
        public IActionResult Create()
        {
            // Mengirim daftar posisi ke view agar bisa dipilih
            ViewData["PositionId"] = new SelectList(_context.JobPositions, "PositionId", "PositionName");
            return View();
        }

        // POST: Applicant/Create
        // Menyimpan data dari form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PositionId,FirstName,LastName,Nik,BirthPlace,BirthDate,Gender,Address,Phone,Email,Education")] Applicant applicant)
        {
            // Hapus validasi untuk properti yang tidak ada di form jika perlu
            ModelState.Remove("Position");
            ModelState.Remove("RegisterCode");
            ModelState.Remove("CreatedBy");

            if (ModelState.IsValid)
            {
                // Mengisi data yang di-generate oleh sistem
                applicant.RegisterCode = $"APP-{DateTime.Now:yyyyMMddHHmmssfff}"; // Contoh kode registrasi unik
                applicant.CreatedAt = DateTime.UtcNow;
                applicant.CreatedBy = User.Identity?.Name ?? "system"; // Mengambil nama user yang login, atau "system" jika tidak ada

                _context.Add(applicant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Jika model tidak valid, kirim kembali daftar posisi dan tampilkan form lagi dengan pesan error
            ViewData["PositionId"] = new SelectList(_context.JobPositions, "PositionId", "PositionName", applicant.PositionId);
            return View(applicant);
        }

        // GET: Applicant/Update/5
        // Menampilkan form untuk mengubah data berdasarkan ID
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant == null || applicant.DeletedAt != null) // Cek juga jika sudah di-soft delete
            {
                return NotFound();
            }

            ViewData["PositionId"] = new SelectList(_context.JobPositions, "PositionId", "PositionName", applicant.PositionId);
            return View(applicant);
        }

        // POST: Applicant/Update/5
        // Menyimpan perubahan data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("ApplicantId,PositionId,RegisterCode,FirstName,LastName,Nik,BirthPlace,BirthDate,Gender,Address,Phone,Email,Education,CreatedBy,CreatedAt")] Applicant applicant)
        {
            if (id != applicant.ApplicantId)
            {
                return NotFound();
            }

            ModelState.Remove("Position");

            if (ModelState.IsValid)
            {
                try
                {
                    // Mengisi data update
                    applicant.UpdatedAt = DateTime.UtcNow;
                    applicant.UpdatedBy = User.Identity?.Name ?? "system";

                    _context.Update(applicant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantExists(applicant.ApplicantId))
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

            ViewData["PositionId"] = new SelectList(_context.JobPositions, "PositionId", "PositionName", applicant.PositionId);
            return View(applicant);
        }

        // POST: Applicant/Destroy/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Destroy(int id)
        {
            try
            {
                var applicant = await _context.Applicants.FindAsync(id);
                if (applicant == null)
                {
                    return Json(new { status = false, message = "Data pelamar tidak ditemukan." });
                }

                if (applicant.DeletedAt != null)
                {
                    return Json(new { status = false, message = "Data ini sudah dihapus sebelumnya." });
                }

                // Mengisi properti untuk soft delete
                applicant.DeletedAt = DateTime.UtcNow;
                applicant.DeletedBy = User.Identity.Name ?? "system";

                _context.Update(applicant);
                await _context.SaveChangesAsync();

                return Json(new { status = true, message = "Data pelamar berhasil dihapus." });
            }
            catch (Exception ex)
            {
                // Log the exception (optional but recommended)
                // _logger.LogError(ex, "Error occurred while deleting applicant with ID {id}", id);
                return Json(new { status = false, message = "Terjadi kesalahan pada server." });
            }
        }

        // Fungsi helper untuk mengecek apakah applicant ada di database
        private bool ApplicantExists(int id)
        {
            return _context.Applicants.Any(e => e.ApplicantId == id && e.DeletedAt == null);
        }
    }
}