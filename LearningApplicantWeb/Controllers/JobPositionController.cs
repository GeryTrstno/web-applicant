using LearningApplicantWeb.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LearningApplicantWeb.Controllers
{
    [Authorize(Roles = "1")]
    public class JobPositionController : Controller
    {
        public IActionResult Index()
        {
            using (var context = DBClass.GetContext())
            {
                var model = new JobPositionVM.Index
                {
                    JobPositions = context.JobPositions.ToList()
                };
                return View(model);
            }
        }

        // Menggunakan JobPositionVM.Add
        public IActionResult Create()
        {
            var model = new JobPositionVM.Add();
            return PartialView("Create", model);
        }

        // Menerima parameter JobPositionVM.Add
        [HttpPost]
        public IActionResult Submit(JobPositionVM.Add model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { status = false, message = string.Join(", ", errors) });
            }
            try
            {
                var username = User.Identity.Name;
                JobPositionVM.Create(model, username);
                return Json(new { status = true, message = "Data Posisi Berhasil Ditambahkan" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        public IActionResult Update(int id)
        {
            using (var context = DBClass.GetContext())
            {
                var position = context.JobPositions.FirstOrDefault(p => p.PositionId == id);
                if (position == null)
                    return Json(new { Status = "Error", Message = "Posisi tidak ditemukan" });

                var model = new JobPositionVM.Update
                {
                    PositionId = position.PositionId,
                    PositionName = position.PositionName
                };
                return PartialView("Update", model);
            }
        }

        [HttpPost]
        public IActionResult SubmitUpdate(JobPositionVM.Update model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { status = false, message = string.Join(", ", errors) });
            }
            try
            {
                var username = User.Identity.Name;
                JobPositionVM.Edit(model, username);
                return Json(new { status = true, message = "Data Posisi Berhasil Diupdate" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        public IActionResult Destroy(int id)
        {
            using (var context = DBClass.GetContext())
            {
                var position = context.JobPositions.FirstOrDefault(p => p.PositionId == id);
                if (position == null)
                    return Json(new { Status = "Error", Message = "Posisi tidak ditemukan" });

                var model = new JobPositionVM.Destroy
                {
                    PositionId = position.PositionId,
                    PositionName = position.PositionName
                };
                return PartialView("Destroy", model);
            }
        }

        [HttpPost]
        public IActionResult SubmitDestroy(int id)
        {
            try
            {
                var username = User.Identity.Name;
                JobPositionVM.Delete(id, username);
                return Json(new { status = true, message = "Data Posisi Berhasil Dihapus" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
