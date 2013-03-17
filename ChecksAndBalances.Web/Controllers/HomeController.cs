using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChecksAndBalances.Data.Models.Enum;
using ChecksAndBalances.Web.Models;
using ChecksAndBalances.Extensions;
using ChecksAndBalances.Data.Models;
using ChecksAndBalances.Data.Storage.Context;
using ChecksAndBalances.Service.Services;

namespace ChecksAndBalances.Web.Controllers
{
    public class HomeController : ChecksAndBalancesControllerBase
    {
        private IArticleService _service;
        private ICategoryTagService _tagService;

        public HomeController(IArticleService service, ICategoryTagService tagService)
        {
            _service = service;
            _tagService = tagService;
        }

        public ActionResult Index()
        {
            return View(_service.GetArticles().ToList());
        }

        [Authorize]
        public ActionResult ChooseState()
        {
            var viewModel = new HomeViewModel
            {
                States = StateSelectList
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChooseState(HomeViewModel viewModel)
        {
            return this.RedirectToRoute("Default", new { controller = "PlayersPage", action = "Index", state = viewModel.SelectedState });
        }

        public ActionResult SideBar(State state)
        {
            var articles = _service.ArticlesByState(state, size: 5).ToList();

            var viewModel = new SideBarViewModel
            {
                CurrentState = state,
                SpotLightArticles = articles.Select(x => x.Title),
                RecentArticles = articles,
                PopularArticles = _service.GetPopularArticles(),
                RecentlyCommentArticles = Enumerable.Empty<Article>(), // _service.GetRecentlyCommentedArticles(),
                PopularTags = _tagService.GetTagsByState(state).Select(x => x.Name),
                LatestPhotos = articles
                    .Select(x => x.ImageUrl)
            };

            return PartialView("~/Views/Shared/SideBar.cshtml", viewModel);
        }
    }
}
