using BattleshipGame.Data;
using BattleshipGame.Entities;
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
        public ActionResult<Grid> GetGrid()
        {
            var results = JsonConvert.SerializeObject(_repository.GetGrid());
            return Ok(results);
        }

        /*[HttpGet("clear")]
        public ActionResult<Grid> ClearGrid()
        {
            _repository.ClearGrid();

            var results = JsonConvert.SerializeObject(_repository.GetGrid());
            return Ok(results);
        }*/
    }
}
