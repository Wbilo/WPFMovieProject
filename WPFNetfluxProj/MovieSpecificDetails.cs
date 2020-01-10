using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFNetfluxProj
{
    // Følgende klasse indeholder properties fra både Film og FilmDetails klassen så vi kan
    // lave objekter med mere specifikke informationer om en given film 
    public class MovieSpecificDetails
    {
        public string Titel { get; set; }
        public string Genre { get; set; }
        public int Id { get; set; }
        public int? Playtime { get; set; }
        public DateTime? Release { get; set; }
        public double? Rating { get; set; }
    }
}
