using Crystal.TestWebApp.Interfaces;
using Crystal.TestWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crystal.TestWebApp.Controllers
{
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly IGenericRepository<Entity> _entityRepository;

        public EntityController(IGenericRepository<Entity> entityRepository)
        {
            _entityRepository = entityRepository;
        }
        
        [HttpGet]
        [Route("api/entities")]
        public IActionResult GetAll()
        {
            return Ok(_entityRepository.GetAll());
        }

        [HttpPost]
        [Route("api/entities")]
        public IActionResult Create(Entity entity)
        {
            _entityRepository.Insert(entity);
            return Ok();
        }
    }
}