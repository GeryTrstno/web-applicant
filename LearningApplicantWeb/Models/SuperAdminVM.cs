using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using LearningApplicantWeb.Models.EF; // Pastikan namespace ini benar

namespace LearningApplicantWeb.Models
{
    public class SuperAdminVM
    {
        // KELAS VIEWMODEL DIJADIKAN WADAH DATA MURNI
        public class Index
        {
            public List<Role> Roles { get; set; } = new List<Role>();
        }

        // --- PERBAIKAN 1: Nama kelas diubah dari 'Create' menjadi 'Add' ---
        public class Add
        {
            [Required(ErrorMessage = "ID wajib diisi")]
            [Range(1, int.MaxValue, ErrorMessage = "ID harus berupa angka positif")]
            public int RoleId { get; set; }

            [Required(ErrorMessage = "Nama wajib diisi")]
            [StringLength(100, ErrorMessage = "Nama maksimal 100 karakter")]
            public string Name { get; set; }
        }

        public class Update
        {
            [Required]
            public int RoleId { get; set; }

            [Required(ErrorMessage = "Nama wajib diisi")]
            public string Name { get; set; }
        }

        public class Destroy
        {
            public int RoleId { get; set; }
            public string Name { get; set; }
        }

        // METODE STATIS DIPERBAIKI
        // --- PERBAIKAN 2: Metode Create sekarang menerima input dari kelas 'Add' ---
        public static void Create(Add input)
        {
            using (var context = DBClass.GetContext())
            {
                if (context.Roles.Any(r => r.RoleId == input.RoleId))
                {
                    throw new Exception("ID Role sudah digunakan.");
                }
                if (context.Roles.Any(r => r.RoleName == input.Name))
                {
                    throw new Exception("Nama Role sudah ada.");
                }

                var newRow = new Role
                {
                    RoleId = input.RoleId,
                    RoleName = input.Name
                };
                context.Roles.Add(newRow);
                context.SaveChanges();
            }
        }

        public static void Edit(Update input)
        {
            using (var context = DBClass.GetContext())
            {
                var existingRole = context.Roles.FirstOrDefault(r => r.RoleId == input.RoleId);
                if (existingRole == null)
                {
                    throw new Exception("Role tidak ditemukan");
                }

                if (context.Roles.Any(r => r.RoleName == input.Name && r.RoleId != input.RoleId))
                {
                    throw new Exception("Nama role sudah ada");
                }
                existingRole.RoleName = input.Name;
                context.SaveChanges();
            }
        }

        public static void Delete(int id)
        {
            using (var context = DBClass.GetContext())
            {
                var role = context.Roles.FirstOrDefault(r => r.RoleId == id);
                if (role == null)
                {
                    throw new Exception("Role tidak ditemukan");
                }
                if (context.Users.Any(u => u.RoleId == id))
                {
                    throw new Exception("Role tidak dapat dihapus karena masih digunakan oleh user");
                }
                context.Roles.Remove(role);
                context.SaveChanges();
            }
        }
    }
}
