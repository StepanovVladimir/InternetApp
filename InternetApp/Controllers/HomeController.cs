using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InternetApp.Models;
using InternetApp.Data;
using InternetApp.Data.Repositories;

namespace InternetApp.Controllers
{
    public class HomeController : Controller
    {
        private IArticleRepository _repository;

        public HomeController(IArticleRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var articles = _repository.GetArticles();
            return View(articles);
        }

        public IActionResult Article(int? id)
        {
            var article = _repository.GetArticle(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Store(Article article)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.StoreArticle(article))
                {
                    return RedirectToAction("Article", new { id = article.Id });
                }
            }
            return View("Create", article);
        }

        public IActionResult Edit(int? id)
        {
            var article = _repository.GetArticle(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Article article)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.UpdateArticle(article))
                {
                    return RedirectToAction("Article", new { id = article.Id });
                }
            }
            return View("Edit", article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteArticle(id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
