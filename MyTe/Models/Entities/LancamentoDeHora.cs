using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyTe.Models.Entities
{
    public class LancamentoDeHora
    {
        public int Id { get; set; }

        [DisplayName("Funcionário")]
        public int FuncionarioId { get; set; }
        
        [DisplayName("Código WBS")]
        public int WBSId { get; set; }
       
        [DisplayName("Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Informe a data")]
        public DateTime RegistroData { get; set; }

        [DisplayName("Horas  Trabalhadas")]
        [Required(ErrorMessage = "Informe as horas trabalhadas.", AllowEmptyStrings = false)]
        public double HorasTrabalhadas { get; set; }

        public Funcionario? Funcionario { get; set; }
        public WBS? TipoWBS { get; set; }

    }
}
