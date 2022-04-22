using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class Repository<T> : IRepository<T> where T : Entity
  {
        private readonly RestFullApiContext _context; 
        protected readonly DbSet<T> entity;
        public Repository(RestFullApiContext _context)
        {
            this._context = _context;
            this.entity = this._context.Set<T>();
        }
        public async Task Delete(int id)
        {
            T entity = await this.Show(id);
            this.entity.Remove(entity);
        }
        public IEnumerable<T> Get()
        {
            return this.entity.AsEnumerable();
        }

        public async Task<T> Show(int id)
        {
            return await this.entity.FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task Store(T entity)
        {
            await this.entity.AddAsync(entity);
        }

        public void Update(T entity)
        {
            this.entity.Update(entity);
        }
    }
}