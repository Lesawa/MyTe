using MyTe.Models.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace MyTe.Models.DTO
{
    public class LancamentoDeHora2DTO
    {
        public int Id { get; set; }

        public int WBSId { get; set; }

        [DisplayName("Código WBS")]
        public string? CodigoWBSId { get; set; }

        [DisplayName("Descrição")]
        public string? DescricaoWBS { get; set; }

        [Range(1, 2)]
        [DisplayName("Situação")]
        public int WBSTipo { get; set; }

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
