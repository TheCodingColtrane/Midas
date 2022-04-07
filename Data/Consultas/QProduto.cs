using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Midas.Data.Consultas
{
    public class QProduto : QEndereco
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public Microsoft.AspNetCore.Mvc.FileContentResult Arquivo { get; set; }
        public decimal Valor { get; set; }
        public decimal Porcentagem { get; set; }        
        public string Descricao { get; set; }
        public string Especificacao_Tecnica { get; set; }
        public List<QProduto> Dados { get; set; }
        public string[,] Carrinho { get; set; }
    }
}
