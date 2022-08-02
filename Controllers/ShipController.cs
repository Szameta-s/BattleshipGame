﻿using BattleshipGame.Data;
using BattleshipGame.Entities;
using Microsoft.AspNetCore.Mvc;

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
    }
}