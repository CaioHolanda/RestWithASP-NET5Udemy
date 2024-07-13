using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO book);
        BookVO FindById(long id);
        BookVO Update(BookVO book);
        void Delete(long id);
        List<BookVO> FindAll();
    }
}
