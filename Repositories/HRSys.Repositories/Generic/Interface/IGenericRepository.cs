using HRSys.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Repositories.Generic.Interface
{
    public interface IGenericRepository<TEntity>
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> All(Expression<Func<TEntity, bool>> predicate = null);
        Task<IEnumerable<TEntity>> AllWithTraking(Expression<Func<TEntity, bool>> predicate = null,bool tracking = false);

        Task<IEnumerable<TEntity>> All(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> All(Expression<Func<TEntity, bool>> predicate = null, params string[] includeProperties);
        IQueryable<TEntity> AllAsIQueryable(Expression<Func<TEntity, bool>> predicate = null, params string[] includeProperties);
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        void ClearLocal();
        void Delete(IEnumerable<TEntity> entities);
        void Delete(int id);
        Task<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate, bool WithTracking = false, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, bool WithTracking = false, params Expression<Func<TEntity, object>>[] includeProperties);
       // Task<IEnumerable<BusinessPermissions>> All(Expression<Func<BusinessPermissions, bool>> expression);
        TEntity GetById(int id, bool withTracking = false, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetById(int id, bool withTracking = false,  string[] includeProperties = null);
        TEntity GetById(Guid id, bool withTracking = false, string[] includeProperties = null);
        Task<TEntity> GetByIdAsync(int id, bool withTracking = false, string[] includeProperties = null);
        Task<IPagedList<TEntity>> GetPagedList(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool withTracking = false, int page = 1, int pageSize = 10, params Expression<Func<TEntity, object>>[] includeProperties);
        void Update(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Update(TEntity entity, List<Object> otherEntities = null);
        Task<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate, string[] includeProperties, bool WithTracking = false);
        void Attach(TEntity entity);
        void Attach(List<TEntity> entity);
        Task LoadEntities(Expression<Func<TEntity, bool>> predicate = null, params string[] includeProperties);


    }
}
