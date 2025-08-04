using LearningApplicantWeb.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LearningApplicantWeb.Controllers
{
    
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            using (var context = DBClass.GetContext())
            {
                var model = new RoleVM.Index
                {
                    Roles = context.Roles.ToList()
                };
                return View(model);
            }
        }

        // Action untuk menampilkan form
        [Authorize(Roles = "1")]
        public IActionResult Create()
        {
            var model = new RoleVM.Add();
            // Pastikan nama PartialView-nya adalah "Create"
            return PartialView("Create", model);
        }

        [Authorize(Roles = "1")]
        public IActionResult Update(int idx)
        {
            using (var context = DBClass.GetContext())
            {
                var role = context.Roles.FirstOrDefault(r => r.RoleId == idx);
                if (role == null)
                {
                    return Json(new { Status = "Error", Message = "Role tidak ditemukan" });
                }

                var model = new RoleVM.Update
                {
                    RoleId = role.RoleId,
                    Name = role.RoleName
                };
                return PartialView("Update", model);
            }
        }
        [Authorize(Roles = "1")]
        public IActionResult Destroy(int id)
        {
            using (var context = DBClass.GetContext())
            {
                var role = context.Roles.FirstOrDefault(r => r.RoleId == id);
                if (role == null)
                {
                    return Json(new { Status = "Error", Message = "Role tidak ditemukan" });
                }

                var model = new RoleVM.Destroy
                {
                    RoleId = role.RoleId,
                    Name = role.RoleName
                };
                return PartialView("Destroy", model);
            }
        }

        [Authorize(Roles = "1")]
        // Action untuk memproses data dari form Add/Create
        [HttpPost]
        // --- PERBAIKAN: Parameter diubah dari RoleVM.Create menjadi RoleVM.Add ---
        public IActionResult Submit(RoleVM.Add model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { status = false, message = string.Join(", ", errors) });
            }

            try
            {
                RoleVM.Create(model);
                return Json(new { status = true, message = "Data Berhasil Ditambahkan" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult SubmitUpdate(RoleVM.Update model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { status = false, message = string.Join(", ", errors) });
            }
            try
            {
                RoleVM.Edit(model);
                return Json(new { status = true, message = "Data Berhasil Diupdate" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult SubmitDestroy(int id)
        {
            try
            {
                RoleVM.Delete(id);
                return Json(new { status = true, message = "Data Berhasil Dihapus" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
