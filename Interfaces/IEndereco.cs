using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Midas.Data;
using Midas.Models;
namespace Midas.Interfaces
{
    interface IEndereco
    {
       public Task <List<Endereco>> Get(string id, MidasContext midasContext);
    }
}
