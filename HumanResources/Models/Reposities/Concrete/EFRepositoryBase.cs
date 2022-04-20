using HumanResources.Models.Entities.Abstract;
using HumanResources.Models.Entities.Context;
using HumanResources.Models.Reposities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HumanResources.Models.Reposities.Concrete
{
    public class EFRepositoryBase<TEntity> : IEntityRepository<TEntity> where TEntity : BaseEntity
    {
        private ProjectContext _context;
        public EFRepositoryBase(ProjectContext context)
        {
            _context = context;
        }
        public IQueryable<TEntity> Entities => _context.Set<TEntity>().AsQueryable();

        public bool Delete(TEntity entity)
        {
            var deletedEntity = _context.Entry(entity);
            deletedEntity.Entity.DeletedDate = DateTime.Now;
            deletedEntity.Entity.IsActive = false;
            deletedEntity.State = EntityState.Modified;
            int result = _context.SaveChanges();
            return result > 0;
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().Where(filter).AsNoTracking().FirstOrDefault();
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            if(filter == null)
            {
                return _context.Set<TEntity>().ToList();
            }
            else
            {
                return _context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public bool Insert(TEntity entity)
        {
            var addedEntity = _context.Entry(entity);
            addedEntity.Entity.CreatedDate = DateTime.Now;
            addedEntity.Entity.IsActive = true;
            addedEntity.State = EntityState.Added;
            int result = _context.SaveChanges();
            return result > 0;
        }

        public bool Update(TEntity entity)
        {
            var updatedEntity = _context.Entry(entity);
            updatedEntity.Entity.ModifiedDate = DateTime.Now;
            updatedEntity.State = EntityState.Modified;
            int result = _context.SaveChanges();
            return result > 0;
        }
    }
}
