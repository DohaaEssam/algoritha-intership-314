using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using testvezeeta.Core_Layer.Domain;
using testvezeeta.Core_Layer.IRepository;
using testvezeeta.Infrastructure_Layer.DbContext;

namespace testvezeeta.Infrastructure_Layer.Repoitory
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbEntities _context;

        public Repository(DbEntities context)
        {
            _context = context;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public T GetById(string id)
        {
            return _context.Set<T>().Find(id);
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public List<T> GetAll(int page, int pageSize, Func<T, bool> search = null)
        {
            var userRepository = new Repository<T>(_context);
            List<T> allItems = userRepository.GetAll().ToList();

            if (search != null)
            {
                allItems = allItems.Where(search).ToList();
            }

            int skipCount = (page - 1) * pageSize;

            if (pageSize > 0)
            {
                allItems = allItems.Skip(skipCount).Take(pageSize).ToList();
            }

            return allItems;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public void DeleteMany(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            _context.SaveChanges();
        }

        public void AddList(List<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            _context.SaveChanges();
        }
    }
}
