using System.Linq;
using Crystal.TestWebApp.Models;

namespace Crystal.TestWebApp.Interfaces
{
    public interface IGenericRepository<T> where T : Entity
    {
        IQueryable<T> GetAll();
        void Insert(T entity);
    }
}