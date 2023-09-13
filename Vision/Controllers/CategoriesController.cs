using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vision.Data;
using Vision.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Vision.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _environment;

		public CategoriesController(ApplicationDbContext context, IWebHostEnvironment environment)
		{
			_context=context;
			_environment=environment;
		}

		[HttpGet]
		public IActionResult GetIndexView(string? search, string sortType, string sortOrder, int pageSize = 20, int pageNumber = 1)
		{
			ViewBag.CurrentSearch=search;

			IQueryable<Category>cat=_context.categories.AsQueryable();

			if (string.IsNullOrEmpty(search)==false)
			{
				cat=cat.Where(c=>c.Name.Contains(search));
			}

            if (sortType=="Name"&&sortOrder=="asc")
            {
                cat = cat.OrderBy(c=> c.Name);
            }

            if (sortType == "Title" && sortOrder == "desc")
            {
                cat = cat.OrderByDescending(c => c.Name);
            }

            if (pageSize > 50) pageSize = 50;

            if (pageSize <1) pageSize = 1;


            if (pageNumber < 1) pageNumber = 1;

            cat=cat.Skip(pageSize*(pageNumber-1)).Take(pageSize);
            return View("Index",cat);
		}


		[HttpGet]
		public IActionResult GetDetailsView(int id)
		{
			Category cat=_context.categories.Include(c=>c.books).FirstOrDefault(c=>c.Id==id);

			ViewBag.CurrentCat=cat;

			if(cat==null)
			{
				return NotFound();

			}
			else
			{
                return View("Details",cat);
			}

			
		}

		[HttpGet]
		public IActionResult GetEditView(int id)
		{

			Category cat = _context.categories.Find(id);
			if (cat==null)
			{
				return NotFound();

			}
			else
			{
				return View("Edit", cat);
			}
		}

		[HttpGet]
		public IActionResult GetDeleteView(int id)
		{
			Category cat = _context.categories.Include(b => b.books).FirstOrDefault(b => b.Id==id);
			ViewBag.CurrentCat=cat;

			if (cat==null)
			{
				return NotFound();
			}
			else
			{
				return View("Delete", cat);

			}

		}

		[HttpGet]
		public IActionResult GetCreateView()
		{
			return View("Create");

		}

		[HttpPost]
		public IActionResult AddNew(Category cat, IFormFile? imageFormFile)
		{

			if (ModelState.IsValid==true)
			{

				if (imageFormFile==null)
				{
					cat.ImagePath="\\images\\not Available.jpeg";
				}

				else
				{
					Guid imgGuid = Guid.NewGuid();
					string imgExtension = Path.GetExtension(imageFormFile.FileName);
					string imgName = imgGuid + imgExtension;
					cat.ImagePath="\\images\\categories\\"+imgName;

					string imgFullPath = _environment.WebRootPath+cat.ImagePath;

					FileStream fileStream = new FileStream(imgFullPath, FileMode.Create);
					imageFormFile.CopyTo(fileStream);
					fileStream.Dispose();
				}

				_context.categories.Add(cat);
				_context.SaveChanges();

				return RedirectToAction("GetIndexView");
			}
			else
			{
				return View("Create", cat);

			}
		}

		[HttpPost]
		public IActionResult DeleteCurrent(int id)
		{
			Category cat = _context.categories.Find(id);

			if (cat.ImagePath!="\\images\\not Available.jpeg")
			{
				string imgPath = _environment.WebRootPath+cat.ImagePath;

				System.IO.File.Delete(imgPath);
			}

			_context.categories.Remove(cat);
			_context.SaveChanges();
		
			return RedirectToAction("GetIndexView");
		}

		[HttpPost]
		public IActionResult EditCurrent(Category cat, IFormFile? imageFormFile)
		{

			if (ModelState.IsValid==true)
			{

				if (imageFormFile!=null)
				{

                    if (cat.ImagePath != "\\images\\not Available.jepg")
                    {
                        string imgPath = _environment.WebRootPath + cat.ImagePath;
                        if (System.IO.File.Exists(imgPath))
                        {
                            System.IO.File.Delete(imgPath);
                        }
                    }
               
					Guid imgGuid = Guid.NewGuid();
					string imgExtension = Path.GetExtension(imageFormFile.FileName);
					string imgName = imgGuid + imgExtension;
					cat.ImagePath="\\images\\categories\\"+imgName;

					string imgFullPath = _environment.WebRootPath+cat.ImagePath;

					FileStream fileStream = new FileStream(imgFullPath, FileMode.Create);
					imageFormFile.CopyTo(fileStream);
					fileStream.Dispose();
				}

				_context.categories.Update(cat);
				_context.SaveChanges();

				return RedirectToAction("GetIndexView");
				
			}
			else
			{
				return View("Edit", cat);
			}
		}
	}
}
