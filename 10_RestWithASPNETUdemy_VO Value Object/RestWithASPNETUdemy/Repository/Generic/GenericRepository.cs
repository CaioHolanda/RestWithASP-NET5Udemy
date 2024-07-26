﻿using Microsoft.EntityFrameworkCore;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using System;
namespace RestWithASPNETUdemy.Repository.Implementations
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private MySQLContext _context;
        private DbSet<T> dataset;
        public GenericRepository(MySQLContext context)
        {
            _context = context;
            dataset=_context.Set<T>(); //--> neste ponto ocorre a associacao generica
        }
        public T CreateR(T item)
        {
            try{
                _context.Add(item);
                _context.SaveChanges();
                return item;
            }
            catch(Exception) {
                throw;
            }
        }
        public void Delete(long id)
        {
            var result = dataset.SingleOrDefault(p => p.Id.Equals(id));
            if (result != null)
            {
                try
                {
                    dataset.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public List<T> FindAll()
        {
            return dataset.ToList();
        }
        public T FindById(long id)
        {
            return dataset.SingleOrDefault(p=>p.Id.Equals(id));
        }
        public T Update(T item)
        {
            //if (!Exists(item.Id)) return null;
            var result =dataset.SingleOrDefault(p=>p.Id.Equals(item.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }
        public bool Exists(long id)
        {
            return dataset.Any(p=>p.Id.Equals(id));
        }
    }
}
