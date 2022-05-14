using HRSys.Constants;
using HRSys.Model;
using HRSys.Repositories.Generic.Interface;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Repositories.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        #region Fields
        protected readonly HRSysContext context;
        private readonly DbSet<TEntity> dbSet;
        #endregion
        #region CTOR
        public GenericRepository(HRSysContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        #endregion

        #region Methods

        public async Task<IEnumerable<TEntity>> All(Expression<Func<TEntity, bool>> predicate = null)
        {
          
            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable();

            if (predicate != null)
                results = results.Where(predicate);

            return await results.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> All(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable();

            if (predicate != null)
                results = results.Where(predicate);

            if (includeProperties != null && includeProperties.Count() > 0)
                results = includeProperties.Aggregate(results, (current, includeProperty) => current.Include(includeProperty));

            return await results.ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> All(Expression<Func<TEntity, bool>> predicate = null, params string[] includeProperties)
        {
            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable();
            if (predicate != null)
                results = results.Where(predicate);
            if (includeProperties != null && includeProperties.Count() > 0)
                results = includeProperties.Aggregate(results, (current, includeProperty) => current.Include(includeProperty));

            return await results.ToListAsync();

        }

        public async Task<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate,
            bool WithTracking = false,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;

            if (!WithTracking)
                query = query.AsNoTracking();
            else
                query = query.AsQueryable();

            if (includeProperties != null && includeProperties.Count() > 0)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            query = query.Where(predicate);


            return await query.FirstOrDefaultAsync();
        }
        public void ClearLocal()
        {
            dbSet.Local.Clear();
        }
        public async Task<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            bool WithTracking = false,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;

            if (!WithTracking)
                query = query.AsNoTracking();
            else
                query = query.AsQueryable();

            query = query.Where(predicate);

            if (includeProperties != null && includeProperties.Count() > 0)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            query = orderBy.Invoke(query);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate,
            string[] includeProperties,
            bool WithTracking = false)

        {
            IQueryable<TEntity> query = dbSet;

            if (!WithTracking)
                query = query.AsNoTracking();
            else
                query = query.AsQueryable();

            if (includeProperties != null && includeProperties.Count() > 0)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<IPagedList<TEntity>> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool withTracking = false,
            int page = 1,
            int pageSize = Paging.DefaultSize,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;

            if (!withTracking)
                query = query.AsNoTracking();
            else
                query = query.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            if (includeProperties != null && includeProperties.Count() > 0)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
                query = orderBy.Invoke(query);

            return new PagedList<TEntity>(await query.ToListAsync(), page, pageSize);
        }

        public TEntity GetById(int id,
     bool withTrack = true,
     params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(withTrack, includeProperties);
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            if (withTrack)
                return query.FirstOrDefault(lambda);

            return query.AsNoTracking().SingleOrDefaultAsync(lambda).Result;
        }
        public TEntity GetById(int id,
      bool withTrack = true, string[] includeProperties = null)
        {
            var query = GetAllIncluding(withTrack, includeProperties);
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            if (withTrack)
                return query.SingleOrDefaultAsync(lambda).Result;

            return query.AsNoTracking().SingleOrDefaultAsync(lambda).Result;
        }
        public async Task<TEntity> GetByIdAsync(int id,
     bool withTrack = true, string[] includeProperties = null)
        {
            var query = GetAllIncluding(withTrack, includeProperties);
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            if (withTrack)
                return await query.SingleOrDefaultAsync(lambda);

            return await query.AsNoTracking().SingleOrDefaultAsync(lambda);
        }
        public TEntity GetById(Guid id,
     bool withTrack = true, string[] includeProperties = null)
        {
            var query = GetAllIncluding(withTrack, includeProperties);
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            if (withTrack)
                return query.SingleOrDefaultAsync(lambda).Result;

            return query.AsNoTracking().SingleOrDefaultAsync(lambda).Result;
        }
        private IQueryable<TEntity> GetAllIncluding
           (bool withTrack, string[] includeProperties = null)
        {
            IQueryable<TEntity> queryable = null;

            if (withTrack)
                queryable = dbSet.AsQueryable();
            else
                queryable = dbSet.AsNoTracking();
            if (includeProperties == null)
            {
                return queryable;
            }
            return includeProperties.Aggregate
                (queryable, (current, includeProperty) => current.Include(includeProperty).AsNoTracking());
        }
        private IQueryable<TEntity> GetAllIncluding
           (bool withTrack, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = null;

            if (withTrack)
                queryable = dbSet.AsQueryable();
            else
                queryable = dbSet.AsNoTracking();
            if (includeProperties == null)
            {
                return queryable;
            }
            return includeProperties.Aggregate
                (queryable, (current, includeProperty) => current.Include(includeProperty));
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await dbSet.AsQueryable().AnyAsync(predicate);
            return result;
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }
        public void Update(TEntity entity, List<Object> otherEntities = null)
        {
            var isLocal = context.Set<TEntity>().Local.Contains(entity);

            if (entity.GetType().Namespace != "System.Data.Entity.DynamicProxies" && !isLocal)
                throw new Exception($"Are you trying to update an untrackable entity???? {nameof(entity)}");
            //if (!isLocal)
            //    dbSet.Attach(entity);

            if (otherEntities != null)
            {
                foreach (var e in otherEntities)
                {
                    //isLocal = context.Entry(e).State 
                    //if (!isLocal)
                    //    dbSet.Attach(e);
                    context.Entry(e).State = EntityState.Modified;
                }
            }
            context.Entry(entity).State = EntityState.Modified;
        }
        public void Update(TEntity entity)
        {
            var isLocal = context.Set<TEntity>().Local.Contains(entity);

            if (entity.GetType().Namespace != "System.Data.Entity.DynamicProxies" && !isLocal)
                throw new Exception($"Are you trying to update an untrackable entity???? {nameof(entity)}");

            context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var isLocal = context.Set<TEntity>().Local.Contains(entity);
                //if (!isLocal)
                //    dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
        }
        public void Delete(int id)
        {
            var entity = GetById(id);
            context.Set<TEntity>().Remove(entity);
        }

        //public void Delete(int id)
        //{
        //    var entity = GetById(id, withTracking: true);
        //    dbSet.Remove(entity.Result);
        //}
        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var isLocal = context.Set<TEntity>().Local.Contains(entity);
                if (!isLocal)
                    dbSet.Attach(entity);
                //  context.Set<TEntity>().Remove(entity);
                //if (entity.GetType().Namespace != "System.Data.Entity.DynamicProxies" && !isLocal)
                //    throw new Exception($"Are you trying to delete an untrackable entity???? in {nameof(entities)}");
            }
            dbSet.RemoveRange(entities);
            //context.Set<TEntity>().RemoveRange(entities);
        }



        public async Task<IList<TEntity>> List(
      Expression<Func<TEntity, bool>> filter = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
      string includeProperties = "")
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();

            }
        }

        public void Attach(TEntity entity)
        {
            context.Attach(entity);

        }
   
        public void Attach(List<TEntity> entity)
        {
            context.AttachRange(entity);

        }
        public IQueryable<TEntity> AllAsIQueryable(Expression<Func<TEntity, bool>> predicate = null, params string[] includeProperties)
        {
            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable();
            if (predicate != null)
                results = results.Where(predicate);
            if (includeProperties != null && includeProperties.Count() > 0)
                results = includeProperties.Aggregate(results, (current, includeProperty) => current.Include(includeProperty));

            return results.AsNoTracking();
        }
        public async Task LoadEntities(Expression<Func<TEntity, bool>> predicate = null, params string[] includeProperties)
        {
            IQueryable<TEntity> results = dbSet.AsNoTracking().AsQueryable();
            if (predicate != null)
                results = results.Where(predicate);
            if (includeProperties != null && includeProperties.Count() > 0)
                results = includeProperties.Aggregate(results, (current, includeProperty) => current.Include(includeProperty));

            await dbSet.LoadAsync();
        }

        public  async Task<IEnumerable<TEntity>> AllWithTraking(Expression<Func<TEntity, bool>> predicate = null, bool tracking = false)
        {
            IQueryable<TEntity> results= dbSet.AsNoTracking().AsQueryable(); ;
            if(tracking)
               results = dbSet.AsTracking().AsQueryable();
            

      

            if (predicate != null)
                results = results.Where(predicate);

            return await results.ToListAsync();
        }
        #endregion
    }
}
