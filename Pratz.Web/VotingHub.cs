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

        private VotingHub(IVoteRoomRepository voteRoomRepository)
        {
            this.voteRoomRepository = voteRoomRepository;
        }

        //TODO: handle injection properly
        public VotingHub(ILogger<VotingHub> logger)
            :this(new InMemoryVoteRoomRepository()) {
            this.logger = logger;
        }

        public async Task Vote(string value)
        {
            
        }

        public async override Task OnConnectedAsync()
        {
            var roomId = Context.GetHttpContext().Request.Query["roomId"];
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, roomId);

            var userName = this.Context.User.Identity.Name;
            await voteRoomRepository.AddMember(roomId, new RoomMember
            {
                ConnectionId = Context.ConnectionId,
                UserId = Context.UserIdentifier,
                UserName = userName
            });

            await this.Clients.AllExcept(this.Context.ConnectionId).SendAsync("UserJoined", userName);
        }
    }
}
