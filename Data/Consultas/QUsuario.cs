using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Midas.Data.Consultas
{
    public class QUsuario 
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UsuarioCodigoUnico { get; set; }
       public DateTime DataNascimento { get; set; }
       public byte Sexo { get; set; }
    }
}
