using Conrtacts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repositorys
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext _repositoryContext;
        public RepositoryBase(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll(bool trackChanges)=>
            !trackChanges ? _repositoryContext.Set<T>().AsNoTracking() : _repositoryContext.Set<T>();

        /// <summary>
        /// trackChanges параметр. Мы собираемся использовать его для повышения производительности наших запросов только для чтения. Если установлено значение false, мы присоединяемAsNoTracking в наш запрос, чтобы сообщить EF Core, что ему не нужно отслеживать изменения для требуемых сущностей. Это значительно увеличивает скорость запроса.
        /// </summary>
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ?_repositoryContext.Set<T>().Where(expression)
            .AsNoTracking() : _repositoryContext.Set<T>().Where(expression);

        public void Update(T entity) => _repositoryContext.Set<T>().Update(entity);
        public void Create(T entity) => _repositoryContext.Set<T>().Add(entity);
        public void Delete(T entity) => _repositoryContext.Set<T>().Remove(entity);
    }
}
