using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccess
{
    public interface IConsumerRepository
    {
        public Task<List<ConsumerModel>> GetAllBySelect();
        public Task<List<ConsumerModel>> GetAllBySelectWithAtributes();
    }
}
