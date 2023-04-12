using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EntityFramework_Slider.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //HttpContext.Session.SetString("name", "Pervin");   

            //Response.Cookies.Append("surname", "Rehimli", new CookieOptions { MaxAge = TimeSpan.FromMinutes(30)});  


            //Book book = new Book
            //{
            //    Id = 1,
            //    Name = "Xoshrov ve Shirin"
            //};

           
            //Response.Cookies.Append("book",JsonConvert.SerializeObject(book));



            
            List<Slider> sliders = await _context.Sliders.ToListAsync();

            //IQueryable<Slider> slide = _context.Sliders.AsQueryable();
            //List<Slider> query = slide.Where(m=> m.Id > 5).ToList();
           
            

            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();

            
            IEnumerable<Blog> blogs = await _context.Blogs.Where(m=>m.SoftDelete == false).ToListAsync();

           
            IEnumerable<Category> categories = await _context.Categories.Where(m =>m.SoftDelete == false).ToListAsync();


            
            IEnumerable<Product> products = await _context.Products.Include(m=>m.Images).Where(m => !m.SoftDelete).ToListAsync();

            About abouts = await _context.Abouts.Include(m => m.Adventages).FirstOrDefaultAsync();

            IEnumerable<Experts>  experts = await _context.Experts.Where(m=>m.SoftDelete == false).ToListAsync();

            ExpertsHeader expertsHeader = await _context.ExpertsHeaders.FirstOrDefaultAsync();

            Subscribe subscribe = await _context.Subscribes.FirstOrDefaultAsync();

            OurBlog ourBlog = await _context.OurBlogs.FirstOrDefaultAsync();

            IEnumerable<Say> says = await _context.Says.Where(m => m.SoftDelete == false).ToListAsync();

            IEnumerable<Instagram> instagrams = await _context.Instagrams.Where(m => m.SoftDelete == false).ToListAsync();





            HomeVM model = new()
            {
                Sliders = sliders,
                SliderInfo = sliderInfo,
                Blogs = blogs,
                Categories = categories,
                Products = products,
                Abouts = abouts,
                Experts = experts,
                ExpertsHeader = expertsHeader,
                Subscribe = subscribe,
                OurBlog = ourBlog,
                Says = says,
                Instagrams = instagrams,
            };

            return View(model);
        }


        //public IActionResult Test() 
        //{
        //    var sessionData = HttpContext.Session.GetString("name"); 

        //    var cookieData = Request.Cookies["surname"];    

        //    var objectData = JsonConvert.DeserializeObject<Book>(Request.Cookies["book"]);   
        //    

        //    return Json(sessionData);
        //}


        [HttpPost] 
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> AddBasket(int? id)
        {
            if(id == null) return BadRequest(); 

            Product dbproduct = await _context.Products.FindAsync(id);   

            if (dbproduct == null) return NotFound();


            List<BasketVM> basket;  

            if (Request.Cookies["basket"] != null) 
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);    
              
            }
            else
            {
                basket = new List<BasketVM>();
                
            }


            BasketVM? existProduct = basket?.FirstOrDefault(m => m.Id == dbproduct.Id);
            


            if (existProduct == null)
            {
                basket?.Add(new BasketVM   
                {
                    Id = dbproduct.Id,
                    Count = 1
                });
            }
            else
            {
                existProduct.Count++;
            }

           

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            


            return RedirectToAction(nameof(Index)); 
        }

    }


    //class Book 
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }

    //}
}