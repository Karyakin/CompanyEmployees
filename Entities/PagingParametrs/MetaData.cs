using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.PagingParametrs
{
    public class MetaData
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1; //если текущая страница больше 1 значит true
        public bool HasNext => CurrentPage < TotalPages;
    }
}
