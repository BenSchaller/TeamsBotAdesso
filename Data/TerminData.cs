using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Data
{
    public class TerminData
    {
        public int ID { get; set; }
        public string DatumUndZeit { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan Startzeit { get; set; }
        public TimeSpan Endzeit { get; set; }

    }
}
