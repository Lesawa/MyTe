using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyTe.Models.Entities
{
    public class Funcionario
    {
        [DisplayName("Id do Funcionário")]
        [Required(ErrorMessage = "Campo obrigatório.", AllowEmptyStrings = false)]
        public int Id { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Campo obrigatório.", AllowEmptyStrings = false)]
        public string? Nome { get; set; }

        [DisplayName("E-Mail")]
        [Required(ErrorMessage = "Campo obrigatório.", AllowEmptyStrings = false)]
        public string? Email { get; set; }
        public ICollection<LancamentoDeHora>? Horas { get; set; }
    }
}
