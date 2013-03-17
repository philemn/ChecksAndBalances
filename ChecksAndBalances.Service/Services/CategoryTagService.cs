using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecksAndBalances.Data.Models;
using ChecksAndBalances.Data.Models.Enum;
using ChecksAndBalances.Data.Storage.Context;

namespace ChecksAndBalances.Service.Services
{
    public interface ICategoryTagService
    {
        CategoryTag Get(int id);
        IEnumerable<CategoryTag> GetAllTags();
        IEnumerable<CategoryTag> GetTagsByState(State state, int size = 10);
    }

    public class CategoryTagService : ICategoryTagService
    {
        private IChecksAndBalancesSession _session;

        public CategoryTagService(IChecksAndBalancesSession session)
        {
            _session = session;
        }

        public CategoryTag Get(int id)
        {
            return _session.Single<CategoryTag>(x => x.Id == id);
        }

        public IEnumerable<CategoryTag> GetAllTags()
        {
            return _session.All<CategoryTag>()
                .OrderBy(x => x.Name)
                .ToList();
        }

        public IEnumerable<CategoryTag> GetTagsByState(State state, int size = 10)
        {
            var articleTags = from tag in _session.All<CategoryTag>()
                              where tag.Articles.Any(x => x.States.Any(y => y.StateId == (int)state))
                              let count = tag.Articles.Count()
                              orderby count descending
                              select tag;

            return articleTags.Take(size);
        }
    }
}
