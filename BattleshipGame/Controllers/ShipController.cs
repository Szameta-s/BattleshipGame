using BattleshipGame.Data;
using BattleshipGame.Data.Entities;
using BattleshipGame.Data.Models;
using BattleshipGame.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ShipController : Controller
    {
        private readonly IShipRepository _shipRepository;
        private readonly ILogger<ShipController> _logger;

        public ShipController(IShipRepository shipRepository, ILogger<ShipController> logger)
        {
            _shipRepository = shipRepository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ship>> Get()
        {
            var results = _shipRepository.GetAllShips();

            if (results == null)
            {
                return NotFound();
            }

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Ship> GetById(int id)
        {
            var results = _shipRepository.GetShipById(id);

            if (results == null)
            {
                return NotFound();
            }

            return Ok(results);
        }


        [HttpPost("shoot")]
        public ActionResult<Ship> GetShipWithHit([FromBody] ShotDataModel data)
        {
            try
            {
                bool isDuplicate =  GameUtilities.IsCellDuplicate(data.Cells, data.Cell);
                List<Cell> cellsList = data.Cells.ToList();

                if (!isDuplicate)
                {
                    cellsList.Add(data.Cell);

                    if (data.Ship.Id != 0) 
                    {
                        Ship updatedShip = GameUtilities.MarkShipCellWithHit(data.Ship, data.Cell);
                        return Ok(new ShotDataModel() { Ship = updatedShip, Cells = cellsList });
                    }

                    return Ok(new ShotDataModel() { Cells = cellsList });
                }

                return Ok(new ShotDataModel() { Ship = new Ship { Id = 0 }, Cells = cellsList });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("computer")]
        public ActionResult<IEnumerable<Cell>> GetAIMove([FromBody] AIShotDataModel data)
        {
            try
            {
                var cells = GameUtilities.GenerateShot(data.Cells);
                int lastIdx = cells.Count() - 1;
                var cell = cells.ToList()[lastIdx];
                Board board = data.Board;
                int[,] grid = board.Grid;
                Ship updatedShip = new Ship() { Id = 0 };

                if (grid[cell.Position[1], cell.Position[0]] != 0)
                {
                    int shipId = grid[cell.Position[1], cell.Position[0]];
                    Ship oldShip = data.Board.Ships.Where(ship => ship.Id == shipId).FirstOrDefault();

                    if (oldShip != null)
                    {
                        updatedShip = GameUtilities.MarkShipCellWithHit(oldShip, cell);
                    }
                }

                return Ok(new { Cells = cells, Ship = updatedShip } );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
