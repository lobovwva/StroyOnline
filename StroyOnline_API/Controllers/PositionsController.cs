using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StroyOnline_API.Data;
using StroyOnline_API.Models;

namespace StroyOnline_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController : Controller
    {
        private readonly StroyOnlineDBContext _stroyOnlineDBContext;

        public PositionController(StroyOnlineDBContext stroyOnlineDBContext)
        {
            _stroyOnlineDBContext = stroyOnlineDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPositions() // метод для возврата списка должностей
        {
            var positions = await _stroyOnlineDBContext.Positions.ToListAsync();
            return Ok(positions);
        }
    }
}