using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.PagingParametrs
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }
        /// <summary>
        /// Как видите, мы передали логику пропуска / взятия статическому методу внутри PagedList класс. 
        /// И в MetaData class, мы добавили еще несколько свойств, которые пригодятся в качестве метаданных для нашего ответа.
        /// HasPrevious верно, если CurrentPage больше 1, и HasNext рассчитывается, если CurrentPage(Текущая страница) меньше, 
        /// чем общее количество страниц.TotalPages рассчитывается путем деления количества элементов на размер страницы и 
        /// последующего округления до большего числа, поскольку страница должна существовать, даже если на ней есть только один элемент.
        /// <returns></returns>
        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count(); 
            var items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
