using InternetApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetApp.Data.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private AppDbContext _context;

        public ArticleRepository(AppDbContext context)
        {
            _context = context;
        }

        public Article GetArticle(int id)
        {
            return _context.Articles.Find(id);
        }

        public List<Article> GetArticles()
        {
            return _context.Articles.ToList();
        }

        public async Task<bool> CreateArticle(Article article)
        {
            _context.Add(article);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteArticle(int id)
        {
            _context.Articles.Remove(GetArticle(id));
            return await SaveChangesAsync();
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
