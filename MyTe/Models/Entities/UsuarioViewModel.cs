using System.ComponentModel.DataAnnotations;

namespace MyTe.Models.Entities
{
    public class UsuarioViewModel
    {
        [Required]
        [Display(Name = "Nome Completo")]
        public string? Nome { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Senha")]
        public string? ConfirmarSenha { get; set; }

        [Display(Name = "Selecione um perfil")]
        public string? Perfil { get; set; }

    }
}
