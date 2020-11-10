using Crystal.TestWebApp.Interfaces;
using Crystal.TestWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crystal.TestWebApp.Controllers
{
    [ApiController]
    [Route("api/entities")]
    public class EntityController : ControllerBase
    {
        private readonly IGenericRepository<Entity> _entityRepository;

        public EntityController(IGenericRepository<Entity> entityRepository)
        {
            _entityRepository = entityRepository;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_entityRepository.GetAll());
        }

        [HttpPost]
        public IActionResult Create(Entity entity)
        {
            _entityRepository.Insert(entity);
            return Ok();
        }
    }
}