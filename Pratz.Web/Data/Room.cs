using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratz.Web.Data
{
    public class Room
    {
        public Room()
        {
            Members = new List<RoomMember>();
        }

        public string Id { get; set; }
        
        public string OwnerConnectionId { get; set; }
        
        public string OwnerUserId { get; set; }
        
        public string OwnerName { get; set; }

        public ICollection<RoomMember> Members { get; }
    }
}
