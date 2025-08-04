using LearningApplicantWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LearningApplicantWeb.Controllers
{
    [Authorize(Roles = "1")]
    public class SuperAdminController : Controller
    {
        public IActionResult Index()
        {
            using (var context = DBClass.GetContext())
            {
                var model = new SuperAdminVM.Index
                {
                    Roles = context.Roles.ToList()
                };
                return View(model);
            }
        }

        // Action untuk menampilkan form
        public IActionResult Create()
        {
            var model = new SuperAdminVM.Add();
            // Pastikan nama PartialView-nya adalah "Create"
            return PartialView("Create", model);
        }

        public IActionResult Update(int idx)
        {
            using (var context = DBClass.GetContext())
            {
                var role = context.Roles.FirstOrDefault(r => r.RoleId == idx);
                if (role == null)
                {
                    return Json(new { Status = "Error", Message = "Role tidak ditemukan" });
                }

                var model = new SuperAdminVM.Update
                {
                    RoleId = role.RoleId,
                    Name = role.RoleName
                };
                return PartialView("Update", model);
            }
        }

        public IActionResult Destroy(int id)
        {
            using (var context = DBClass.GetContext())
            {
                var role = context.Roles.FirstOrDefault(r => r.RoleId == id);
                if (role == null)
                {
                    return Json(new { Status = "Error", Message = "Role tidak ditemukan" });
                }

                var model = new SuperAdminVM.Destroy
                {
                    RoleId = role.RoleId,
                    Name = role.RoleName
                };
                return PartialView("Destroy", model);
            }
        }

        // Action untuk memproses data dari form Add/Create
        [HttpPost]
        // --- PERBAIKAN: Parameter diubah dari RoleVM.Create menjadi RoleVM.Add ---
        public IActionResult Submit(SuperAdminVM.Add model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { status = false, message = string.Join(", ", errors) });
            }

            try
            {
                SuperAdminVM.Create(model);
                return Json(new { status = true, message = "Data Berhasil Ditambahkan" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult SubmitUpdate(SuperAdminVM.Update model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { status = false, message = string.Join(", ", errors) });
            }
            try
            {
                SuperAdminVM.Edit(model);
                return Json(new { status = true, message = "Data Berhasil Diupdate" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult SubmitDestroy(int id)
        {
            try
            {
                SuperAdminVM.Delete(id);
                return Json(new { status = true, message = "Data Berhasil Dihapus" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
