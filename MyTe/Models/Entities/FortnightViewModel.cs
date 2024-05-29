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
//public DateTime FirstFortnightStart { get; set; }
//        public DateTime FirstFortnightEnd { get; set; }
//        public DateTime SecondFortnightStart { get; set; }
//        public DateTime SecondFortnightEnd { get; set; }
        //public DateTime FirstFortnight { get; set; }
//        //public DateTime SecondFortnight { get; set; }

//    }
//}
