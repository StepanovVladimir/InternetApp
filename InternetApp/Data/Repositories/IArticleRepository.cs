using InternetApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetApp.Data.Repositories
{
    public interface IArticleRepository
    {
        Article GetArticle(int id);
        List<Article> GetArticles();
        Task<bool> CreateArticle(Article article);
        Task<bool> UpdateArticle(Article article);
        Task<bool> DeleteArticle(int id);
    }
}
