using BattleshipGame.Data;
using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;
using Microsoft.AspNetCore.Mvc;


namespace BattleshipGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GameController : Controller
    {
        private readonly IGameRepository _gameRepository;
        private readonly IShipRepository _shipRepository;
        private readonly ILogger<GameController> _logger;

        public GameController(IGameRepository repository, IShipRepository shipRepository, ILogger<GameController> logger)
        {
            _gameRepository = repository;
            _shipRepository = shipRepository;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public ActionResult<Player> GetPlayers(int id)
        {
            Player player = _gameRepository.GetPlayerById(id);
            Board board = _shipRepository.GenerateBoard();
            player.Board = board;

            return Ok(player);
        }
    }
}
