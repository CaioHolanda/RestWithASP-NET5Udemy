using RestWithASPNETUdemy.Data.Converter.Implementations;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository;
using System;

namespace RestWithASPNETUdemy.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private readonly IRepository<Person> _repository; //--> ponto de criacao para o repositorio especifico Person
        private readonly PersonConverter _converter;
        public PersonBusinessImplementation(IRepository<Person> repository)
        {
            _repository = repository;
            _converter = new PersonConverter();
        }
        public PersonVO Create(PersonVO person)
        {
            var personEntity=_converter.Parse(person);
            personEntity=_repository.CreateR(personEntity);
            return _converter.Parse(personEntity);
        }
        public void Delete(long id)
        {
            _repository.Delete(id);
        }
        public List<PersonVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }
        public PersonVO FindById(long id)
        {
            return _converter.Parse(_repository.FindById(id));
        }
        public PersonVO Update(PersonVO person)
        {
            var personEntity = _converter.Parse(person);
            personEntity = _repository.Update(personEntity);
            return _converter.Parse(personEntity);
        }
    }
}
