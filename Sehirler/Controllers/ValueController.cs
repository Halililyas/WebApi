using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sehirler.Data;

namespace Sehirler.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ValueController : ControllerBase
	{

		private DataContext _context;
		public ValueController(DataContext context)
		{
			_context = context;
		}
		[HttpGet]
		public async Task<ActionResult> Get()
		{
			//Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
			var values = await _context.Values.ToListAsync();
			return Ok(values);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult> GetById(int id)
		{
			var value = await _context.Values.FirstOrDefaultAsync(v=>v.Id==id);
			return Ok(value);
		}

    }

		
		
	
}