using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Vision.Models
{
	public class Book
	{
        public int Id { get; set; }

		[DisplayName("Title")]
		[Required(ErrorMessage = "You have to provide a valid Title.")]
		[MinLength(2, ErrorMessage = "Title mustn't be less than 2 characters.")]
		[MaxLength(50, ErrorMessage = "Title mustn't exceed 50 characters.")]
		public string Title { get; set; }

		[Required(ErrorMessage = "You have to provide a valid Description.")]
		[MinLength(10, ErrorMessage = "Title mustn't be less than 10 characters.")]
		[MaxLength(100, ErrorMessage = "Title mustn't exceed 100 characters.")]
		public string Description { get; set; }

		[Range(20, 2000, ErrorMessage = "Price must be between 20 EGP and 2000 EGP.")]
		public decimal Price { get; set; }

        [DisplayName("Release Date")]
        [DataType(DataType.Date)]
		public DateTime ReleaseDate { get; set; }

		[DisplayName("Number of Sales")]
		[Required(ErrorMessage = "You have to provide a valid Number of Sales.")]
		public decimal NumberOfSales { get; set; }

		[Required(ErrorMessage = "You have to provide a valid stock.")]
		public decimal Stock { get; set; }

		
		[DisplayName("Author Name")]
		[Required(ErrorMessage = "You have to provide a valid name.")]
		[MinLength(10, ErrorMessage = " Author Name mustn't be less than 10 characters.")]
		[MaxLength(50, ErrorMessage = "Author Name mustn't exceed 50 characters.")]
		public string AuthorName { get; set; }


        [DisplayName("Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Choose a valid category.")]
        public int CategoryId { get; set; }


        [ValidateNever]
		public Category Category { get; set; }

		[DisplayName("Image")]
		[ValidateNever]
		public string? ImagePath { get; set; }
	}
}
