using Microsoft.AspNetCore.SignalR;
using Pratz.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratz.Web
{
    public class VotingHub : Hub
    {
        private readonly IVoteRoomRepository voteRoomRepository;

        public VotingHub(IVoteRoomRepository voteRoomRepository)
        {
            this.voteRoomRepository = voteRoomRepository;
        }

        //TODO: handle injection properly
        public VotingHub()
            :this(new InMemoryVoteRoomRepository()) { }

        public async Task Vote(string value)
        {
            
        }

        public async override Task OnConnectedAsync()
        {
            var groupName = "a";
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);

            var userName = this.Context.User.Identity.Name;
            //TODO: alert owner client connected
        }
    }
}
