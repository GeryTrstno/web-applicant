using System.ComponentModel.DataAnnotations;

namespace LearningApplicantWeb.Models
{
    // Kelas utama untuk mengelompokkan ViewModel terkait akun
    public class AccountVM
    {
        // ViewModel khusus untuk halaman Login
        public class LoginViewModel
        {
            [Required(ErrorMessage = "Username wajib diisi")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Password wajib diisi")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        // ViewModel khusus untuk halaman Register
        public class RegisterViewModel
        {
            [Required(ErrorMessage = "Username wajib diisi")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Password wajib diisi")]
            [StringLength(100, ErrorMessage = "{0} minimal harus {2} karakter.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Konfirmasi Password")]
            [Compare("Password", ErrorMessage = "Password dan konfirmasi password tidak cocok.")]
            public string ConfirmPassword { get; set; }
        }
    }
}
