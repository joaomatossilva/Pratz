using System.Threading.Tasks;

namespace Pratz.Web.Data
{
    public interface IVoteRoomRepository
    {
        Task CreateRoom(Room room);
        Task<Room> GetRoom(string id);
        Task DeleteRoom(string id);
        Task AddMember(string id, RoomMember member);
        Task RemoveMember(string id, string connectionId);
    }
}