using System;
using System.Linq;
using LearningApplicantWeb.Models.EF;

namespace LearningApplicantWeb.Models
{
    /// <summary>
    /// Kelas statis yang berisi logika bisnis untuk User.
    /// Beroperasi menggunakan ViewModel dari AccountVM.
    /// </summary>
    public static class UserVM
    {
        /// <summary>
        /// Memverifikasi kredensial user dari database.
        /// </summary>
        /// <param name="input">Data dari form login (LoginViewModel).</param>
        /// <returns>Objek User jika berhasil, atau null jika gagal.</returns>
        public static User Login(AccountVM.LoginViewModel input)
        {
            using (var context = DBClass.GetContext())
            {
                // 1. Cari user berdasarkan username
                var userFromDb = context.Users.FirstOrDefault(u => u.Username == input.Username);
                if (userFromDb == null)
                {
                    return null; // User tidak ditemukan
                }

                // 2. Verifikasi password yang di-hash
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(input.Password, userFromDb.Password);
                if (!isPasswordValid)
                {
                    return null; // Password salah
                }

                // 3. Jika berhasil, kembalikan data user
                return userFromDb;
            }
        }

        /// <summary>
        /// Membuat user baru di database.
        /// </summary>
        /// <param name="input">Data dari form registrasi (RegisterViewModel).</param>
        public static void Create(AccountVM.RegisterViewModel input)
        {
            using (var context = DBClass.GetContext())
            {
                // Cek apakah username sudah ada
                if (context.Users.Any(u => u.Username == input.Username))
                {
                    throw new Exception("Username sudah terdaftar.");
                }

                var newUser = new User
                {
                    Username = input.Username,
                    // Selalu HASH password sebelum disimpan!
                    Password = BCrypt.Net.BCrypt.HashPassword(input.Password),
                    // Atur RoleId default, misal: 2 untuk 'User' atau 'Applicant'.
                    // Pastikan ID ini ada di tabel 'roles' Anda.
                    RoleId = 3
                };

                context.Users.Add(newUser);
                context.SaveChanges();
            }
        }
    }
}
