using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Core_Layer.IRepository
{
    public interface IRepository<T>
    {
        T GetById(int id);
        public T GetById(string id);
        List<T> GetAll();
        public List<T> GetAll(int page, int pageSize, Func<T, bool> search = null);
        void Add(T entity);
        public void AddList(List<T> entities);
        void Update(T entity);
        void Delete(T entity);
        public void DeleteMany(List<T> entities);
    }
}
