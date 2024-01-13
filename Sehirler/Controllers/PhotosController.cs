using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sehirler.Data;
using Sehirler.Dtos;
using Sehirler.Helpers;
using Sehirler.Models;
using System.Security.Claims;

namespace Sehirler.Controllers
{
	[ApiController]
	[Route("api/cities/{id}/photos")]
	public class PhotosController : Controller
	{
		private IAppRepository _appRepository;
		private IMapper _mapper;
		private IOptions<CloudinarySettings> _cloundinaryConfig;

		private Cloudinary _cloudinary;

		public PhotosController(IAppRepository appRepository, IMapper mapper, IOptions<CloudinarySettings> cloundinaryConfig = null)
		{
			_appRepository = appRepository;
			_mapper = mapper;
			_cloundinaryConfig = cloundinaryConfig;

			Account account = new Account(
				_cloundinaryConfig.Value.CloudName,
				_cloundinaryConfig.Value.ApiKey,
				_cloundinaryConfig.Value.ApiSecret);

			_cloudinary = new Cloudinary(account);
		}

		[HttpPost]
		public ActionResult AddPhotoForCtiy(int ctiyid,[FromBody]PhotoForCreationDto photoForCreationDto)
		{
			var ctiy = _appRepository.GetCityByid(ctiyid);
			if (ctiy == null)
			{
			 return	BadRequest("Could Not find the ctiy");

			}

			var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

			if (currentUserId !=ctiy.UserId)
			{
				return Unauthorized();
			}

			var file = photoForCreationDto.File;

			var uploudResult = new ImageUploadResult();

			if (file.Length >0)
			{
				using (var stream = file.OpenReadStream())
				{
					var uploadParams = new ImageUploadParams
					{
						File = new FileDescription(file.Name, stream)
					};

					uploudResult = _cloudinary.Upload(uploadParams);
				}
			}
			photoForCreationDto.Url = uploudResult.Url.ToString();
			photoForCreationDto.PublicId = uploudResult.PublicId;

			var photo = _mapper.Map<Photo>(photoForCreationDto);
			photo.City = ctiy;

			if (!ctiy.Photos.Any(p=>p.IsMain))
			{
				photo.IsMain = true;
			}
			ctiy.Photos.Add(photo);

			if (_appRepository.SaveAll())
			{
				var photoToReturn = _mapper.Map<PhotoForCreationDto>(photo);
				return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
			}
			return BadRequest("Could Not Add The Photo");

		}
		[HttpGet]
		public ActionResult GetPhoto(int id)
		{
			var photoForm = _appRepository.GetPhoto(id);
			var photo = _mapper.Map<PhotoForReturnDto>(photoForm);
			return Ok(photo);
		}




		
	}
}
