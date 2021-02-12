using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.Dto_DataTransferObjects__
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// В этом свойстве объеденены адрес и страна.
        /// </summary>
        public string FullAddress { get; set; }

        /// <summary>
        /// специально обращаемся к EmployeeDto, т.к нам не нужна ссылка на компанию, чтобы не зациклится.
        /// В EmployeeDto просто нет ссылки на Company
        /// </summary>
        public IEnumerable<EmployeeDto> Employees{ get; set; }

}
}
