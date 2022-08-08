using BattleshipGame.Data;
using BattleshipGame.Data.Entities;
using BattleshipGame.Data.Models;
using BattleshipGame.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace BattleshipGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ShipController : Controller
    {
        private readonly IShipRepository _repository;
        private readonly ILogger<ShipController> _logger;

        public ShipController(IShipRepository repository, ILogger<ShipController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ship>> Get()
        {
            var results = _repository.GetAllShips();

            if (results == null)
            {
                return NotFound();
            }

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Ship> GetById(int id)
        {
            var results = _repository.GetShipById(id);

            if (results == null)
            {
                return NotFound();
            }

            return Ok(results);
        }

        [HttpGet("spawn")]
        public ActionResult<Board> GenerateBoardWithShips() 
        {
            var results = _repository.GenerateBoard();
            var grid = JsonConvert.SerializeObject(results.Grid, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            
            return Ok(grid);
        }

        [HttpPost("shoot")]
        public ActionResult<Ship> GetShipWithHit([FromBody] ShotDataModel data)
        {
            try
            {
                bool isDuplicate = _repository.IsCellDuplicate(data.Cells, data.Cell);
                List<Cell> cellsList = data.Cells.ToList();

                if (!isDuplicate)
                {
                    cellsList.Add(data.Cell);

                    if (data.Ship.Id != 0) 
                    {
                        Ship updatedShip = _repository.MarkShipCellWithHit(data.Ship, data.Cell);
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
                var cells = _repository.GenerateShot(data.Cells);
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
                        updatedShip = _repository.MarkShipCellWithHit(oldShip, cell);
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
