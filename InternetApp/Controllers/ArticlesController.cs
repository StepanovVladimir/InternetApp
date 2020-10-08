using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InternetApp.Models;
using InternetApp.Data.Repositories;
using InternetApp.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace InternetApp.Controllers
{
    public class ArticlesController : Controller
    {
        private IArticleRepository _repository;

        public ArticlesController(IArticleRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var articles = _repository.GetArticles();
            return View(articles);
        }

        public IActionResult Show(int? id)
        {
            var article = _repository.GetArticle(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Store(ArticleViewModel article)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.StoreArticle(article))
                {
                    return RedirectToAction("Show", new { id = article.Id });
                }
            }
            return View("Create", article);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            var article = _repository.GetArticle(id);
            if (article == null)
            {
                return NotFound();
            }

            return View(new ArticleViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Description = article.Description,
                Content = article.Content
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ArticleViewModel article)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.UpdateArticle(article))
                {
                    return RedirectToAction("Show", new { id = article.Id });
                }
            }
            return View("Edit", article);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteArticle(id);
            return RedirectToAction("Index");
        }

        [HttpGet("[controller]/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_repository.GetImageStream(image), $"image/{mime}");
        }
    }
}
