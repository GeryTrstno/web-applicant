using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using LearningApplicantWeb.Models.EF;
using System;
using System.Linq;

namespace LearningApplicantWeb.Models
{
    public class JobPositionVM
    {
        public class Index
        {
            public List<JobPosition> JobPositions { get; set; } = new List<JobPosition>();
        }

        // --- PERBAIKAN 1: Nama kelas diubah dari 'Create' menjadi 'Add' ---
        public class Add
        {
            [Required(ErrorMessage = "ID Posisi wajib diisi")]
            [Range(1, int.MaxValue, ErrorMessage = "ID harus berupa angka positif")]
            public int PositionId { get; set; }

            [Required(ErrorMessage = "Nama Posisi wajib diisi")]
            [StringLength(255, ErrorMessage = "Nama Posisi maksimal 255 karakter")]
            public string PositionName { get; set; }
        }

        public class Update
        {
            [Required]
            public int PositionId { get; set; }

            [Required(ErrorMessage = "Nama Posisi wajib diisi")]
            [StringLength(255, ErrorMessage = "Nama Posisi maksimal 255 karakter")]
            public string PositionName { get; set; }
        }

        public class Destroy
        {
            public int PositionId { get; set; }
            public string PositionName { get; set; }
        }

        // --- METODE LOGIKA ---

        // --- PERBAIKAN 2: Metode Create sekarang menerima input dari kelas 'Add' ---
        public static void Create(Add input, string createdBy)
        {
            using (var context = DBClass.GetContext())
            {
                if (context.JobPositions.Any(p => p.PositionId == input.PositionId))
                    throw new Exception("ID Posisi sudah digunakan.");

                if (context.JobPositions.Any(p => p.PositionName == input.PositionName))
                    throw new Exception("Nama Posisi sudah ada.");

                var newPosition = new JobPosition
                {
                    PositionId = input.PositionId,
                    PositionName = input.PositionName,
                    CreatedBy = createdBy,
                    CreatedAt = DateTime.Now
                };
                context.JobPositions.Add(newPosition);
                context.SaveChanges();
            }
        }

        public static void Edit(Update input, string updatedBy)
        {
            using (var context = DBClass.GetContext())
            {
                var position = context.JobPositions.FirstOrDefault(p => p.PositionId == input.PositionId);
                if (position == null)
                    throw new Exception("Posisi tidak ditemukan.");

                if (context.JobPositions.Any(p => p.PositionName == input.PositionName && p.PositionId != input.PositionId))
                    throw new Exception("Nama Posisi sudah ada.");

                position.PositionName = input.PositionName;
                position.UpdatedBy = updatedBy;
                position.UpdatedAt = DateTime.Now;
                context.SaveChanges();
            }
        }

        public static void Delete(int id, string deletedBy)
        {
            using (var context = DBClass.GetContext())
            {
                var position = context.JobPositions.FirstOrDefault(p => p.PositionId == id);
                if (position == null)
                    throw new Exception("Posisi tidak ditemukan.");

                if (context.Applicants.Any(a => a.PositionId == id))
                    throw new Exception("Posisi tidak dapat dihapus karena sudah ada pelamar.");

                context.JobPositions.Remove(position);
                context.SaveChanges();
            }
        }
    }
}
