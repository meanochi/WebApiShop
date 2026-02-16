using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {

        IShowService _showService;
        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        // GET: api/<ShowsController>
        [HttpGet]
        public async Task<ActionResult<List<ShowReadDTO>>> Get()
        {
            List<ShowReadDTO> shows = await _showService.getAllShows();
            if(shows==null)
                return NoContent();
            return Ok(shows);
        }

        // GET api/<ShowsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowReadDTO>> Get(int id)
        {
            ShowReadDTO show = await _showService.getShowById(id);
            if (show == null)
                return NoContent();
            return Ok(show);
        }

        // POST api/<ShowsController>
        [HttpPost]
        public async Task<ActionResult<ShowReadDTO>> Post([FromBody] ShowCreateDTO show)
        {
            ShowReadDTO createdShow = await _showService.addShow(show);
            if(createdShow == null)
                return BadRequest();
            return CreatedAtAction(nameof(Get), new { createdShow.Title}, createdShow); ;
        }

        // PUT api/<ShowsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ShowReadDTO>> Put(int id, [FromBody] ShowUpdateDTO show)
        {
            ShowReadDTO updatedShow = await _showService.updateShow(show, id);
            if (updatedShow == null)
                return BadRequest();
            return Ok(updatedShow);
        }

        // DELETE api/<ShowsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        // GET: api/<ShowsController>
        [HttpGet("filters")]
        public async Task<ActionResult<(IEnumerable<ShowReadDTO> shows, int total)>> GetAll(string? description, int? minPrice, int? maxPrice, int skip, int position, int categoryId)
        {
            int[] categorys = new int[0];
            (IEnumerable<ShowReadDTO> shows, int total) shows = await _showService.getAllShows(description, minPrice, maxPrice, skip, position, categorys);
            if (shows.shows == null)
                return NoContent();
            return Ok(shows.shows);
        }

    }
}
