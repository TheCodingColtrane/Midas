using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Midas.Data.Consultas
{
    public class QPromocao : QComentario
    {
        public int PromocaoID { get; set; }
        public string PromocaoNome { get; set; }
        public string PromocaoDescricao { get; set; }
        public decimal PromocaoPorcentagem { get; set; }
        public DateTime PromocaoInicio { get; set; }
        public DateTime PromocaoFim { get; set; }
        public bool PromocaoEstaAtiva { get; set; }
    }
}
