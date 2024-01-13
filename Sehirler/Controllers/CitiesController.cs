using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sehirler.Data;
using Sehirler.Dtos;
using Sehirler.Models;

namespace Sehirler.Controllers
{
	[ApiController]
	[Route("api/Cities")]
	public class CitiesController : Controller
	{
		private IAppRepository _appReporsitory;
        private IMapper _mapper;
        public CitiesController(IAppRepository appRepository,IMapper mapper)
        {
            _appReporsitory = appRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult GetCities()
        {
           var cities = _appReporsitory.GetCity();
            var citiesToReturn = _mapper.Map<List<CtiryForListDTO>>(cities);
            return Ok(citiesToReturn);
        }
		
	   [HttpPost]
        
		public IActionResult Add( [FromBody] City city)
        {
            _appReporsitory.Add(city);
            _appReporsitory.SaveAll();
            return Ok(city);
        }
		[HttpGet]
        [Route("detail")]

		public IActionResult GetCitiesById(int id)
		{
			var city = _appReporsitory.GetCityByid(id);
			var cityToReturn = _mapper.Map<CtiyForDetailDto>(city);
			return Ok(cityToReturn);
			
		}

		[HttpGet]
		[Route("Photos")]

		public IActionResult GetPhotosByCtiy(int ctiyİd)
		{
			var Photos = _appReporsitory.GetPhotosByCtiy(ctiyİd);
			
			return Ok(Photos);

		}
	}
}
