using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratz.Web.Data
{
    public class InMemoryVoteRoomRepository : IVoteRoomRepository
    {
        private static Dictionary<string, Room> inMemoryCache = new Dictionary<string, Room>();

        public async Task CreateRoom(Room room)
        {
            inMemoryCache.Add(room.Id, room);
        }

        public async Task<Room> GetRoom(string id)
        {
            return inMemoryCache[id];
        }

        public async Task DeleteRoom(string id)
        {
            inMemoryCache.Remove(id);
        }

        public async Task AddMember(string id, RoomMember member)
        {
            var room = inMemoryCache[id];
            room.Members.Add(member);
        }

        public async Task RemoveMember(string id, string connectionId)
        {
            var room = inMemoryCache[id];
            room.Members.Remove(room.Members.FirstOrDefault(x => x.ConnectionId == connectionId));
        }
    }
}
