using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ConsumerFriendModel
    {

        public long Id { get; set; }
        public ConsumerModel? Friend { get; set; }
        public int FriendshipLevel { get; set; }
        public DateOnly EstablishedDate { get; set; }
    }
}
