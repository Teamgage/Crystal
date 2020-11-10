using System.Linq;
using Crystal.TestWebApp.Interfaces;
using Crystal.TestWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Crystal.TestWebApp.DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : Entity
    {
        private readonly AppContext _context;
        private readonly DbSet<T> _dbSet;
        
        public GenericRepository(AppContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }
    }
}