using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Midas.Data.Consultas
{
    public class QEstoque: QPromocao
    {
        public int EstoqueID { get; set; }
        public int EstoqueQuantidade { get; set; }
        public DateTime EstoqueAlteradoEm { get; set; }
        public DateTime EstoquePrevisaoChegada { get; set; }

    }
}
