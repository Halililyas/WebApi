using Microsoft.EntityFrameworkCore;
using Sehirler.Models;

namespace Sehirler.Data
{
	public class AppRepository : IAppRepository
	{
		private DataContext _context;
        public AppRepository(DataContext context)
        {
			_context = context;
        }
        public void Add<T>(T entity)
		{
			_context.Add(entity);
		}

		public void Delete<T>(T entity)
		{
			_context.Remove(entity);
		}

		public List<City> GetCity()
		{
			var cityies = _context.Cities.Include(c => c.Photos).ToList();
			return cityies;
		}

		public City GetCityByid(int ctiyId)
		{
			var ctiy = _context.Cities.Include(p => p.Photos).FirstOrDefault(c => c.Id == ctiyId);
			return ctiy;
		}

		public Photo GetPhoto(int id)
		{
			var photo = _context.Photos.FirstOrDefault(p => p.Id == id);
			return photo;
		}

		public List<Photo> GetPhotosByCtiy(int id)
		{
			var photos = _context.Photos.Where(c => c.CityId == id).ToList();
			return photos;
		}

		public bool SaveAll()
		{
			return _context.SaveChanges() > 0;
		}
	}
}
