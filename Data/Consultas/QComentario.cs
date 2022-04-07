using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Midas.Data.Consultas
{
    public class QComentario : QUsuario
    {
        public int? ComentarioID { get; set; }
        public string ComentarioUsuarioID { get; set; }
        public string Mensagem { get; set; }
        public int Avaliacao { get; set; }
        public bool FoiRecomendado { get; set; }
        public DateTime ComentarioPublicadoEm { get; set; }
        public bool ComentarioFoiAlterado { get; set; }
        public DateTime ComentarioAlteradoEm { get; set; }
        public bool ComentarioFoiRecomendado { get; set; }

    }
}
