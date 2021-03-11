using FriendsAadPicker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AadAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendsController : ControllerBase
    {
        private FriendsDbContext context;

        public FriendsController(FriendsDbContext context) => this.context = context;

        [HttpGet]
        [RequiredScope("read")]
        public IActionResult GetAllFriends()
        {
            return Ok(new[] { context.Friends });
        }

        [HttpGet]
        [RequiredScope("write")]
        public async Task<IActionResult> AddFriend([FromBody] Friend newFriend)
        {
            await context.AddFriend(newFriend);
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}