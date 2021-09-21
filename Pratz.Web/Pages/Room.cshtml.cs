using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IActionResult> OnGet(string id)
        {
            var room = await voteRoomRepository.GetRoom(id);
            if (room is null)
            {
                return NotFound();
            }

            Room = room;
            return Page();
        }
    }
}