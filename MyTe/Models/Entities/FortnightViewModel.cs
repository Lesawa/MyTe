using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MyTe.Models.Entities
{
    public class FortnightViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Title { get; set; }
    }
}