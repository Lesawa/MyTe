using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace MyTe.Models.Entities
{
    public class HorasViewModel
    {
        public int Id { get; set; }

        public string? CodigoWBSId { get; set; }

        [DisplayName("Descrição")]
        public string? DescricaoWBS { get; set; }

        [DisplayName("Funcionário")]
        public int FuncionarioId { get; set; }

        [DisplayName("Funcionário")]
        [Required(ErrorMessage = "Informe a data", AllowEmptyStrings = false)]
        public string? NomeFuncionario { get; set; }

        [DisplayName("Data")]
        [Required(ErrorMessage = "Informe a data", AllowEmptyStrings = false)]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime RegistroData { get; set; }

        [DisplayName("E-mail")]
        public string? EmailFuncionario { get; set; }

        [DisplayName("Horas Trabalhadas")]
        public double HorasTrabalhadas { get; set; }
    }
}
