using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Pratz.Web.Data;
using System.Threading.Tasks;

namespace Pratz.Web
{
    public class VotingHub : Hub
    {
        private readonly IVoteRoomRepository voteRoomRepository;
        private readonly ILogger<VotingHub> logger;

        public VotingHub(IVoteRoomRepository voteRoomRepository, ILogger<VotingHub> logger)
        {
            this.voteRoomRepository = voteRoomRepository;
            this.logger = logger;
        }

        public async Task Vote(string value)
        {
            
        }

        public async override Task OnConnectedAsync()
        {
            var roomId = Context.GetHttpContext().Request.Query["roomId"];
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, roomId);

            var room = await voteRoomRepository.GetRoom(roomId);
            var userName = this.Context.User.Identity.Name;
            var userId = Context.UserIdentifier;

            if (room.OwnerUserId != userId)
            {
                await voteRoomRepository.AddMember(roomId, new RoomMember
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = userId,
                    UserName = userName
                });

                //TODO: with other Repository implementations `room` might not have the member we just added, maybe retrieve Room again?
            }

            logger.LogDebug("User Connected", roomId, userName, userId);
            await this.Clients.All.SendAsync("UserJoined", room, userName);
        }
    }
}
