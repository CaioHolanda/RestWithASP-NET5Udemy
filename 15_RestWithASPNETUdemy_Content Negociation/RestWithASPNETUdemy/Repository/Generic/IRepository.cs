using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Base;

namespace RestWithASPNETUdemy.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Create(T item);
        T FindById(long id);
        T Update(T item);
        void Delete(long id);
        List<T> FindAll();
        bool Exist(long id);
    }
}
