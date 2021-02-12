using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.PagingParametrs
{
    public class EmployeeParameters : RequestParameters
    {
       
        public uint MinAge { get; set; }//Поскольку значение uint по умолчанию равно 0 
        public uint MaxAge { get; set; } = int.MaxValue;

        /// <summary>
        /// Мы также добавили простое свойство проверки - ValidAgeRange. 
        /// Его цель - сказать нам, действительно ли max-age больше min-age. 
        /// Если это не так, мы хотим, чтобы пользователь API знал, что он / она делает что-то не так.
        /// </summary>
        public bool ValidAgeRange => MaxAge > MinAge;
        public string SearchTerm { get; set; }// сюда будем передавать строку для поиска 
    }
}
