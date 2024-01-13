using Sehirler.Models;

namespace Sehirler.Dtos
{
	public class CtiyForDetailDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public List<Photo> Photos { get; set; }
	}
}
