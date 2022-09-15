using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NomesIbge.Classes
{
    public class Nome
    {
        public string periodo { get; set; }
        public int frequencia { get; set; }
    }

    public class Nomes
    {
        public string nome { get; set; }
        public object sexo { get; set; }
        public string localidade { get; set; }
        public List<Nome> res { get; set; }
    }
}
