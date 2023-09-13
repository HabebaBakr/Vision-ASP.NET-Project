using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vision.Models
{
	public class Category
	{
		public int Id { get; set; }

		[DisplayName("Category Name")]
		[Required(ErrorMessage = "You have to provide a valid name.")]
		public string Name { get; set; }

		public string Description { get; set; }

		[DisplayName("Image")]
		[ValidateNever]
		public string? ImagePath { get; set; }

		[ValidateNever]
		public List<Book> books{ get; set; }
	}
}
