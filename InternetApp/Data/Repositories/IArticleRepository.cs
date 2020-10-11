using InternetApp.Models;
using InternetApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InternetApp.Data.Repositories
{
    public interface IArticleRepository
    {
        Article GetArticle(int? id);
        List<Article> GetArticles();
        Task<int> StoreArticle(ArticleViewModel article);
        Task<bool> UpdateArticle(ArticleViewModel article);
        Task<bool> DeleteArticle(int id);
        FileStream GetImageStream(string image);
    }
}
