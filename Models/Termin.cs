using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramwork.Models
{
    public class Termin
    {
        public int ID { get; set; }
        public string WebinarTitel { get; set; }
        public string WebinarBeschreibung { get; set; }
        public DateTime Datum { get; set; }
        public DateTime StartZeit { get; set; }
        public DateTime EndZeit { get; set; }
        public int Dauer { get; set; }
    }
}
