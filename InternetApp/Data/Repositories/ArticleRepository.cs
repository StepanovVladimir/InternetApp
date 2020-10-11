using InternetApp.Data.FileManagers;
using InternetApp.Models;
using InternetApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InternetApp.Data.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private AppDbContext _context;
        private IImagesFileManager _fileManager;

        public ArticleRepository(AppDbContext context, IImagesFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public Article GetArticle(int? id)
        {
            return _context.Articles.Find(id);
        }

        public List<Article> GetArticles()
        {
            return _context.Articles.OrderByDescending(a => a.CreatedAt).ToList();
        }

        public async Task<int> StoreArticle(ArticleViewModel viewModel)
        {
            var article = new Article
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                Content = viewModel.Content,
                CreatedAt = DateTime.Now
            };
            try
            {
                article.Image = await _fileManager.SaveImage(viewModel.Image);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }

            _context.Add(article);
            await SaveChangesAsync();

            return article.Id;
        }

        public async Task<bool> UpdateArticle(ArticleViewModel viewModel)
        {
            var article = GetArticle(viewModel.Id);
            article.Title = viewModel.Title;
            article.Description = viewModel.Description;
            article.Content = viewModel.Content;
            if (viewModel.Image != null)
            {
                string previousImage = article.Image;
                try
                {
                    article.Image = await _fileManager.SaveImage(viewModel.Image);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                _fileManager.DeleteImage(previousImage);
            }
            
            _context.Update(article);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteArticle(int id)
        {
            var article = GetArticle(id);
            _fileManager.DeleteImage(article.Image);
            _context.Remove(article);
            return await SaveChangesAsync();
        }

        public FileStream GetImageStream(string image)
        {
            return _fileManager.GetImageStream(image);
        }

        private async Task<bool> SaveChangesAsync()
        {
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
