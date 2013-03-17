using ChecksAndBalances.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChecksAndBalances.Extensions;

namespace ChecksAndBalances.Web.Controllers
{
    public abstract class ChecksAndBalancesControllerBase : Controller
    {
        protected static IEnumerable<SelectListItem> StateSelectList { get; private set; }

        static ChecksAndBalancesControllerBase()
        {
            StateSelectList = StateService.States.Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToDescription()
            });
        }
    }
}
