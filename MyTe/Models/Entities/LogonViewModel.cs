using System.ComponentModel.DataAnnotations;

namespace MyTe.Models.Entities
{
    public class LogonViewModel
    {
        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }

        [Display(Name = "Lembrar-me")]

        public bool RememberMe { get; set; }
        public string? SuccessMessage { get; set; }
    }
}
