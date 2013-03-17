using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChecksAndBalances.Data.Models;
using ChecksAndBalances.Service.Services;
using ChecksAndBalances.Web.Areas.Admin.Models;
using ChecksAndBalances.Extensions;
using ChecksAndBalances.Web.Attributes;
using ChecksAndBalances.Web.Controllers;
using Newtonsoft.Json;

namespace ChecksAndBalances.Web.Areas.Admin.Controllers
{
    [Authorize(Roles="Administrator")]
    public class ArticleController : ChecksAndBalancesControllerBase
    {
       IArticleService _service;
       ICategoryTagService _tagService;

        public ArticleController(IArticleService service, ICategoryTagService tagService)
        {
            _service = service;
            _tagService = tagService;
        }

        public ActionResult Index(int? page)
        {
            var viewModel = new ArticleViewModel
            {
                UnPublishedArticles = _service.GetArticlesInProgress(),
                PublishedArticles = _service.GetArticles()
                    .OrderBy(x => x.DatePublished)
                    .Skip(page.GetValueOrDefault())
                    .Take(ArticleViewModel.ItemsPerPage)
            };

            return View(viewModel);
        }

        public ActionResult Edit(int? id)
        {
            var article = _service.GetArticleInProgress(id.GetValueOrDefault()) ?? new Article();

            article.Id = id.GetValueOrDefault();

            ViewBag.States = StateSelectList;

            ViewBag.Tags = _tagService.GetAllTags();

            return View(article);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([FromJson]Article article)
        {
            article.Content = Server.UrlDecode(article.Content);
            var id = _service.SaveArticle(new ArticleInProgress
            {
                Id = article.Id,
                SavedContent = JsonConvert.SerializeObject(article, Formatting.None, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                })
            });

            return RedirectToAction("Edit", new { id = id.Id });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Publish([FromJson]Article article)
        {
            article.Content = Server.UrlDecode(article.Content);
            _service.PublishArticle(article);

            var url = Url.RouteUrl("Default", new { controller = "Article", action = "Get", state = article.States.First().State, resource = article.Title.ToUrlSafeString() });
            return Redirect(url);
        }

        public ActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Revoke(int id)
        {
            _service.Revoke(id);
            return RedirectToAction("Index");
        }

    }
}
