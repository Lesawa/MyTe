using System.ComponentModel.DataAnnotations;

namespace MyTe.Models.Entities
{
    public class UsuarioViewModel
    {
        [Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
        [Display(Name = "Nome Completo")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?!.*(.)\1{2})(?=.*[A-Z]).{8,}$", ErrorMessage = "A senha deve ter 8 caracteres, sendo uma letra maíuscula e 3 caracteres diferentes.")]
        public string? Senha { get; set; }

        [Required(ErrorMessage = "O campo Confirmar a Senha é obrigatório.")]
        [Display(Name = "Confirmar Senha")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não são iguais, verifique.")]
        public string? ConfirmarSenha { get; set; }

        [Required(ErrorMessage = "O campo Perfil é obrigatório.")]
        [Display(Name = "Selecione um perfil")]
        public string? Perfil { get; set; }

    }
}
