using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChecksAndBalances.Data.Storage
{
    public interface ISession : IDisposable
    {
        void CommitChanges();
        void Delete<T>(Expression<Func<T, bool>> expression) where T : class, IEntity, new();
        void Delete<T>(T item) where T : class, IEntity, new();
        void DeleteAll<T>() where T : class, IEntity, new();
        T Single<T>(Expression<Func<T, bool>> expression) where T : class, IEntity, new();
        DbSet<T> All<T>() where T : class, IEntity, new();
        void Add<T>(T item) where T : class, IEntity, new();
        void Add<T>(IEnumerable<T> items) where T : class, IEntity, new();
        void Update<T>(T item) where T : class, IEntity, new();
        void Update<T>(IEnumerable<T> items) where T : class, IEntity, new();
        void AddOrUpdate<T>(T item) where T : class, IEntity, new();
        void AddOrUpdate<T>(IEnumerable<T> items) where T : class, IEntity, new();
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
    }
}
