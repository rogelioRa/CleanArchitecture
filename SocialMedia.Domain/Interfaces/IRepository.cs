using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Domain.Interfaces
{
    public interface IRepository<T> where T: Entity
    {
        Task<T> Show(int id);
        IEnumerable<T> Get();
        Task Store(T entity);
        void Update(T entity);
        Task Delete(int id);
    }
}