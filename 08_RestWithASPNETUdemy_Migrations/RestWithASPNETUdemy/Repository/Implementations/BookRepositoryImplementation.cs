using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using System;

namespace RestWithASPNETUdemy.Repository.Implementations
{
    public class BookRepositoryImplementation : IBookRepository
    {

        
        private MySQLContext _context;
        public BookRepositoryImplementation(MySQLContext context)
        {
            _context = context;
        }

        public Book Create(Book book)
        {
            try{
                _context.Add(book);
                _context.SaveChanges();

            }
            catch(Exception) {
                throw;
            }
            return book;
        }

        public void Delete(long id)
        {
            var result = _context.Books.SingleOrDefault(p => p.Id.Equals(id));
            if (result != null)
            {
                try
                {
                    _context.Books.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<Book> FindAll()
        {
            return _context.Books.ToList();
        }

        public Book FindById(long id)
        {
            return _context.Books.SingleOrDefault(p=>p.Id.Equals(id));
        }
        public Book Update(Book person)
        {
            if (!Exists(person.Id)) return null;
            var result =_context.Books.SingleOrDefault(p=>p.Id.Equals(person.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(person);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return person;
        }
        public bool Exists(long id)
        {
            return _context.Books.Any(p=>p.Id.Equals(id));
        }
    }
}
