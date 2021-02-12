using Entities.Models;
using Entities.PagingParametrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Conrtacts
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        IEnumerable<Employee> GetEmployeeSortByAge(bool trackChanges);
        IEnumerable<Employee> GetEmployeeByCondition(int age);
        IEnumerable<Employee> GetEmployeeByCondition(Guid id);
        IEnumerable<Employee> GetEmployeeByCondition(Guid id, bool trackChanges);
        PagedList<Employee> FindAllEmployeere(bool trackChanges, EmployeeParameters employeeParameters);
        IEnumerable<Employee> FindeEmployeesCompany(Guid companyId, bool trackChanges);
        IEnumerable<Employee> FindAllCompanyFithEmloyee();
        void CreateEmployeeForCompany(Guid companyId, Employee employee);
        void DeleteEmployee(Employee employee);
        Employee GetEmployee(Guid companyId, Guid id, bool trackChanges);
    }
}
