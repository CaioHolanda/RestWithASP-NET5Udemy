using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Filters;

namespace RestWithASPNETUdemy.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class PersonController : ControllerBase
    {
        public readonly ILogger<PersonController> _logger;
        private IPersonBusiness _personService;
        public PersonController(ILogger<PersonController> logger, IPersonBusiness personService)
        {
            _logger = logger;
            _personService = personService;
        }
        [HttpGet]
        [ProducesResponseType((200),Type=typeof(List<PersonVO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter (typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_personService.FindAll());
        }
        [HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            var person=_personService.FindById(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }
        [HttpPost]
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] PersonVO person)
        {
            
            if (person == null)
            {
                return BadRequest();
            }
            return Ok(_personService.Create(person));
        }   
        [HttpPut]
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] PersonVO person)
        {
            
            if (person == null)
            {
                return BadRequest();
            }
            return Ok(_personService.Update(person));
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Delete(long id)
        {
            _personService.Delete(id);
            return NoContent();
        }

    }
}
