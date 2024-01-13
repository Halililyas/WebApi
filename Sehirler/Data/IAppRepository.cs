using Sehirler.Models;

namespace Sehirler.Data
{
	public interface IAppRepository
	{
		void Add<T>(T entity);
		void Delete<T>(T entity);

		bool SaveAll();


		List<City> GetCity();
		List<Photo> GetPhotosByCtiy(int id);

		City GetCityByid(int ctiyId);

		Photo GetPhoto(int id);

	}
}
