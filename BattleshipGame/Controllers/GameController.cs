using BattleshipGame.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BattleshipGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly IGameRepository _repository;
        private readonly ILogger<GameController> _logger;

        public GameController(IGameRepository repository, ILogger<GameController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<int[]>> GetGrid()
        {
            var results = JsonConvert.SerializeObject(_repository.GetGrid());
            return Ok(results);
        }
    }
}
