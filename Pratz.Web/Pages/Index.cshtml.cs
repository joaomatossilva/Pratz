using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pratz.Web.Data;
using Pratz.Web.Services;

namespace Pratz.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IVoteRoomRepository voteRoomRepository;
        private readonly IIdGenerator idGenerator;

        public IndexModel(ILogger<IndexModel> logger, IVoteRoomRepository voteRoomRepository, IIdGenerator idGenerator)
        {
            _logger = logger;
            this.voteRoomRepository = voteRoomRepository;
            this.idGenerator = idGenerator;
        }

        public IActionResult OnGet()
        {
            //If first access is unauthenticated, ask for a name first
            if (User?.Identity.IsAuthenticated != true)
            {
                return RedirectToPage("Profile");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreate()
        {
            var id = idGenerator.GenerateId();
            var room = new Room { 
                Id = id,
                OwnerName = User.Identity.Name,
                OwnerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            await voteRoomRepository.CreateRoom(room);
            _logger.LogDebug("Room Created", room);

            return RedirectToPage("Room", new { id = room.Id });
        }

        public IActionResult OnPostJoin(string roomId)
        {
            return RedirectToPage("Room", new { id = roomId });
        }
    }
}
