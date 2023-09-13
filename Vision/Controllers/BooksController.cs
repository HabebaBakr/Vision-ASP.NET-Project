using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vision.Data;
using Vision.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Vision.Controllers
{
	public class BooksController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _environment;

		public BooksController(ApplicationDbContext context, IWebHostEnvironment environment)
		{
			_context=context;
			_environment=environment;
		}

		[HttpGet]
		public IActionResult GetIndexView(int catId,string? search, string sortType, string sortOrder, int pageSize = 20, int pageNumber = 1)
		{
			ViewBag.AllCategories=_context.categories.ToList();
			ViewBag.CurrentSearch=search;
			ViewBag.SelectedCategoryId=catId;

			IQueryable<Book> books = _context.books.AsQueryable();

			if (catId!=0)
			{
				books=books.Where(b=>b.CategoryId==catId);

			}
			if (string.IsNullOrEmpty(search)==false)
			{
				books=books.Where(b=>b.Title.Contains(search));
			}
            if (sortType=="Title"&&sortOrder=="asc")
            {
                books = books.OrderBy(b=> b.Title);
            }

            if (sortType == "Title" && sortOrder == "desc")
            {
                books = books.OrderByDescending(b => b.Title);
            }
            if (sortType == "NumberOfSales" && sortOrder == "asc")
            {
                books = books.OrderBy(b => b.NumberOfSales);
            }

            if (sortType == "NumberOfSales" && sortOrder == "desc")
            {
                books = books.OrderByDescending(b => b.NumberOfSales);
            }

            if (pageSize > 50) pageSize = 50;

            if (pageSize <1) pageSize = 1;


            if (pageNumber < 1) pageNumber = 1;

            books=books.Skip(pageSize*(pageNumber-1)).Take(pageSize);
            return View("Index",books);
		}

		[HttpGet]
		public IActionResult GetDetailsView(int id)
		{
			Book book= _context.books.Include(b=>b.Category).FirstOrDefault(b=>b.Id==id);
			ViewBag.CurrentBook=book;

			if (book==null)
			{
				return NotFound();
			}
			else
			{
                return View("Details",book);
			}
			

		}

		[HttpGet]
		public IActionResult GetCreateView() 
		{
			ViewBag.AllCategories=_context.categories.ToList();
			return View("Create");
		
		}

		[HttpGet]
		public IActionResult GetDeleteView(int id)
		{
			Book book=_context.books.Include(b=>b.Category).FirstOrDefault(b=>b.Id==id);
			ViewBag.CurrentBook=book;

			if (book==null)
			{
				return NotFound() ;
			}
			else { 
			    return View("Delete",book);
			
			}
		
		}

		[HttpGet]
		public IActionResult GetEditView(int id)
		{
			Book book = _context.books.Find(id);
			if(book==null)
			{
				return NotFound();
			}
			else 
			{
				ViewBag.AllCategories=_context.categories.ToList();
			     return View("Edit",book);
		    }
		}

		[HttpPost]
		public IActionResult AddNew(Book book,IFormFile? imageFormFile)
		{

			if (book.ReleaseDate>DateTime.Now)
			{
				ModelState.AddModelError(string.Empty,"Release Date must be before current date");
			}

			if (ModelState.IsValid==true)
			{

				if (imageFormFile==null)
				{

					book.ImagePath="\\images\\not Available.jpeg";
				}
				else
				{
					Guid imgGuid = Guid.NewGuid();
					string imgExtension = Path.GetExtension(imageFormFile.FileName);
					string imgName = imgGuid + imgExtension;
					book.ImagePath="\\images\\books\\"+imgName;

					string imgFullPath = _environment.WebRootPath+book.ImagePath;

					FileStream fileStream = new FileStream(imgFullPath, FileMode.Create);
					imageFormFile.CopyTo(fileStream);
					fileStream.Dispose();
				}
				_context.books.Add(book);
				_context.SaveChanges();

				return RedirectToAction("GetIndexView");


			}
			else
			{
				ViewBag.AllCategories=_context.categories.ToList();

				return View("Create",book);
			
			}

		}

		[HttpPost]
		public IActionResult DeleteCurrent(int id) {
			Book book=_context.books.Find(id);

			if(book.ImagePath!="\\images\\not Available.jpeg")
			{
				string imgPath = _environment.WebRootPath+book.ImagePath;

				System.IO.File.Delete(imgPath);
			}

			_context.books.Remove(book);
			_context.SaveChanges();
			return RedirectToAction("GetIndexView");
		}

		[HttpPost]
		public IActionResult EditCurrent(Book book, IFormFile? imageFormFile)
		{
			if (book.ReleaseDate>DateTime.Now)
			{
				ModelState.AddModelError(string.Empty, "Release Date must be before current date");
			}

			if (ModelState.IsValid==true)
			{

				if (imageFormFile!=null)
				{

                    if (book.ImagePath != "\\images\\not Available.jepg")
                    {
                        string imgPath = _environment.WebRootPath + book.ImagePath;
                        if (System.IO.File.Exists(imgPath))
                        {
                            System.IO.File.Delete(imgPath);
                        }
                    }
                
				
					Guid imgGuid = Guid.NewGuid();
					string imgExtension = Path.GetExtension(imageFormFile.FileName);
					string imgName = imgGuid + imgExtension;
					book.ImagePath="\\images\\books\\"+imgName;

					string imgFullPath = _environment.WebRootPath+book.ImagePath;

					FileStream fileStream = new FileStream(imgFullPath, FileMode.Create);
					imageFormFile.CopyTo(fileStream);
					fileStream.Dispose();
				}
				_context.books.Update(book);
				_context.SaveChanges();

				return RedirectToAction("GetIndexView");
			}
			else
			{
				ViewBag.AllCategories=_context.categories.ToList();

				return View("Edit", book);
			}
		}


	}
}
