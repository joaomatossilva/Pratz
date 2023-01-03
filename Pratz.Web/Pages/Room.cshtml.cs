using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pratz.Web.Data;

namespace Pratz.Web.Pages
{
    public class RoomModel : PageModel
    {
        private readonly IVoteRoomRepository voteRoomRepository;

        public RoomModel(IVoteRoomRepository voteRoomRepository)
        {
            this.voteRoomRepository = voteRoomRepository;
        }

        [BindProperty]
        public Room Room { get; set; }

        public bool IsOwner { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            //If first access is unauthenticated, ask for a name first
            if(User?.Identity.IsAuthenticated != true)
            {
                return RedirectToPage("Profile", new { room = id });
            }

            var room = await voteRoomRepository.GetRoom(id);
            if (room is null)
            {
                return NotFound();
            }

            IsOwner = User.FindFirstValue(ClaimTypes.NameIdentifier) == room.OwnerUserId;
            Room = room;

            return Page();
        }
    }
}