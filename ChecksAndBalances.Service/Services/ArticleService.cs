using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ChecksAndBalances.Data.Models;
using ChecksAndBalances.Data.Models.Enum;
using ChecksAndBalances.Data.Storage.Context;
using Newtonsoft.Json;

namespace ChecksAndBalances.Service.Services
{
    public interface IArticleService 
    {
        Article Get(int id);
        Article Get(string name);
        IEnumerable<Article> GetArticles(int page = 1, int size = 10);
        IEnumerable<Article> ArticlesByState(State state, int page = 1, int size = 10);

        Article GetArticleInProgress(int id);
        IEnumerable<Article> GetArticlesInProgress(int page = 1, int size = 10);

        ArticleInProgress SaveArticle(ArticleInProgress article);
        Article PublishArticle(Article article);

        void Delete(int id);
        void Revoke(int id);

        IEnumerable<Article> GetPopularArticles(int size = 5);
        IEnumerable<Article> GetRecentlyCommentedArticles(int size = 5);
    }

    class ArticleService : IArticleService
    {
        private IChecksAndBalancesSession _session;

        public ArticleService(IChecksAndBalancesSession session)
        {
            _session = session;
        }

        public Article Get(int id)
        {
            return _session.Single<Article>(x => x.Id == id);
        }

        public Article Get(string name)
        {
            return _session.Single<Article>(x => x.Title.ToLower() == name.ToLower());
        }

        public IEnumerable<Article> GetArticles(int page = 1, int size = 10)
        {
            var skip = (page - 1) * size;

            return _session.All<Article>()
                .Include("States")
                .Include("Advertisement")
                .OrderByDescending(x => x.DatePublished)
                .Skip(skip)
                .Take(size)
                .ToList();
        }

        public Article GetArticleInProgress(int id)
        {
            var articleInProgress = _session.Single<ArticleInProgress>(x => x.Id == id);
            if (articleInProgress == null)
                return null;

            var article = JsonConvert.DeserializeObject<Article>(articleInProgress.SavedContent, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });

            return article;
        }

        public IEnumerable<Article> GetArticlesInProgress(int page = 1, int size = 10)
        {
            var skip = (page - 1) * size;

            var articlesInProgress = _session.All<ArticleInProgress>()
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(size);

            return articlesInProgress.ToList().Select(x => {
                var article = JsonConvert.DeserializeObject<Article>(x.SavedContent, new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                article.Id = x.Id;

                return article;
            });
        }

        public IEnumerable<Article> ArticlesByState(State state, int page = 1, int size = 10)
        {
            var skip = (page - 1) * size;

            return GetByState(state)
                .OrderByDescending(x => x.DatePublished)
                .Skip(skip)
                .Take(size)
                .ToList();
        }

        public ArticleInProgress SaveArticle(ArticleInProgress article)
        {
            var saveArticle = _session.Single<ArticleInProgress>(x => x.Id == article.Id) ?? article;
            saveArticle.SavedContent = article.SavedContent;

            _session.AddOrUpdate<ArticleInProgress>(article);

            _session.CommitChanges();
            return article;
        }

        public Article PublishArticle(Article article)
        {
            _session.Delete<ArticleInProgress>(x => x.Id == article.Id);

            article.Id = 0;
            article.DatePublished = DateTime.Now;
            article.Published = true;

            article.States.ToList().ForEach(x => x.Article = article);
            article.Tags = article.Tags.ToList().Select(x =>
            {
                x = _session.Single<CategoryTag>(y => y.Name == x.Name) ?? x;
                if (!x.Articles.Any(y => y.Id == article.Id))
                    x.Articles.Add(article);

                return x;
            }).ToList();

            _session.Add(article);
            _session.CommitChanges();

            return article;
        }

        private IQueryable<Article> GetByState(State state)
        {
            return _session.All<Article>()
                .Where(x => x.States.Any(y => y.StateId == (int)state));
        }

        public void Delete(int id)
        {
            _session.Delete<ArticleInProgress>(x => x.Id == id);
            _session.CommitChanges();
        }

        public void Revoke(int id)
        {
            var article = _session.Single<Article>(x => x.Id == id);

            var articleInProgress = new ArticleInProgress
            {
                SavedContent = JsonConvert.SerializeObject(article, Formatting.None, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                })
            };

            _session.Add<ArticleInProgress>(articleInProgress);
            _session.Delete<Article>(article);
            _session.CommitChanges();
        }

        public IEnumerable<Article> GetPopularArticles(int size = 5)
        {
            return _session.All<Article>().OrderByDescending(x => x.Views).Take(size);
        }

        public IEnumerable<Article> GetRecentlyCommentedArticles(int size = 5)
        {
            var articles = from article in _session.All<Article>()
                           let latestComment = article.Comments.OrderByDescending(x => x.DateCreated).FirstOrDefault()
                           orderby latestComment.DateCreated descending
                           select article;

            return articles.Take(size);
        }
    }
}
