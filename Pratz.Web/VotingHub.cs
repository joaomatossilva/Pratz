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
            logger.LogInformation("Value submitted", value);
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

                //TODO: with other Repository implementations `room` might not have the member we just added, maybe retrieve Room again? or handle this as business
            }

            logger.LogDebug("User Connected", roomId, userName, userId);
            await this.Clients.Group(roomId).SendAsync("UserJoined", room, userName);
        }

        public async Task StartNewVote(string name)
        {
            var roomId = Context.GetHttpContext().Request.Query["roomId"];
            var room = await voteRoomRepository.GetRoom(roomId);
            var userId = Context.UserIdentifier;
            if(room.OwnerUserId != userId)
            {
                //only owners can start votes
                return;
            }

            await this.Clients.GroupExcept(roomId, Context.ConnectionId).SendAsync("StartNewVote", name);
        }
    }
}
