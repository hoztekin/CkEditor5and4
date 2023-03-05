using CkEditor.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CkEditor.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDb db;
		private readonly Image image;
		private readonly IWebHostEnvironment environment;

		public HomeController(ILogger<HomeController> logger, AppDb db, Image image, IWebHostEnvironment environment )
		{
			_logger = logger;
			this.db = db;
			this.image = image;
			this.environment = environment;
			
		}


		#region CK Editör 5 ile uygulama bölümü
		public IActionResult CkEditor()
		{
			return View();
		}


		/// <summary>
		/// Image olmadan metinsel ifadeler için 
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>

		//[HttpPost]
		//public IActionResult CkEditor(Image image)
		//{
		//	Image images = new Image();
		//	images.Address= image.Address;
		//	images.PhoneNumber= image.PhoneNumber;
		//	images.Description= image.Description;
		//	db.Add(images);
		//	db.SaveChanges();
		//	return View();
		//}




		[HttpPost]
		public ActionResult CkEditor(List<IFormFile> files)
		{
			var filepath = "";
			foreach (IFormFile photo in Request.Form.Files)
			{
				string serverMapPath = Path.Combine(environment.WebRootPath, "Image", photo.FileName);
				using (var stream = new FileStream(serverMapPath, FileMode.Create))
				{
					photo.CopyTo(stream);
				}
				filepath = "https://localhost:7055/" + "Image/" + photo.FileName;
			}
			return Json(new { url = filepath });
		}

		#endregion







		#region CK Editor4 ile uygulama bölümü

		[HttpPost]
		public IActionResult Create(Image model)
		{

			
			return View();
		}





		public IActionResult Create()
		{

			Image image = new Image();
			return View(image);
		}



		[HttpPost]
		public IActionResult UploadImage(IFormFile upload)
		{

			if (upload != null && upload.Length>0)
			{

				var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
				var path = Path.Combine(Directory.GetCurrentDirectory(), environment.WebRootPath, filename);
				var stream = new FileStream(path, FileMode.Create);
				upload.CopyToAsync(stream);

				return new JsonResult(new { path = "/uploads/" + filename});
			}

			return RedirectToAction(nameof(Create));
		}


		[HttpGet]
		public IActionResult UploadExplorer() 
		{

			var dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), environment.WebRootPath, "uploads"));
			ViewBag.fileInfo = dir.GetFiles();
			return View("FileExplorer");
		
		}







		#endregion




		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}