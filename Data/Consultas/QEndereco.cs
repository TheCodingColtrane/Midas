using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Midas.Data.Consultas
{
    public class QEndereco : QEstoque
    {
        public int EnderecoID { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public int CodigoUnico { get; set; }
    }
}
