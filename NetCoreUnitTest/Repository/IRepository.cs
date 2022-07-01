using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreUnitTest.Repository
{
    public interface IRepository<TEntity> where TEntity : class //Bir Entity alacak ve bu entity class olacak.
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById (int id);
        Task Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}
