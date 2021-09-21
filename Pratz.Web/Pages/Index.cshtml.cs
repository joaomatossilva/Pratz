using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pratz.Web.Data;

namespace Pratz.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IVoteRoomRepository voteRoomRepository;

        public IndexModel(ILogger<IndexModel> logger, IVoteRoomRepository voteRoomRepository)
        {
            _logger = logger;
            this.voteRoomRepository = voteRoomRepository;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostCreate()
        {
            var room = new Room { Id = "teste" };
            await voteRoomRepository.CreateRoom(room);
            return RedirectToPage("Room", new { id = room.Id });
        }

        public IActionResult OnPostJoin(string roomId)
        {
            return RedirectToPage("Room", new { id = roomId });
        }
    }
}
