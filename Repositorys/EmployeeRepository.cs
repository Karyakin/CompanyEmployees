using Conrtacts;
using Entities.Models;
using Entities.PagingParametrs;
using Microsoft.EntityFrameworkCore;
using Repositorys.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositorys
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<Employee> GetEmployeeSortByAge(bool trackChanges) =>
                FindAll(trackChanges)
                .OrderByDescending(x => x.Age)
                .ToList();
        public IEnumerable<Employee> GetEmployeeByCondition(int age) =>
            FindByCondition(x => x.Age.Equals(age), false).ToList();

        public IEnumerable<Employee> GetEmployeeByCondition(Guid id) =>
            FindByCondition(x => x.Id.Equals(id), false);


        /// <summary>
        /// В данном методе применяется пойджинг
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <param name="employeeParameters"></param>
        /// <returns></returns>
        public PagedList<Employee> FindAllEmployeere(bool trackChanges, EmployeeParameters employeeParameters)
        {
            var employees = FindAll(trackChanges)
                .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
                .Search(employeeParameters.SearchTerm)
                .OrderBy(x => x.Name)
               // .Where(x => x.Age >= employeeParameters.MinAge && x.Age <= employeeParameters.MaxAge)
                .ToList();
            var employeesPage = PagedList<Employee>.ToPagedList(employees, employeeParameters.PageNumber, employeeParameters.PageSize);
            return employeesPage;
        }




        public IEnumerable<Employee> FindeEmployeesCompany(Guid companyId, bool trackChanges) =>
            FindByCondition(x => x.CompanyId.Equals(companyId), trackChanges = false).OrderBy(x => x.Name);

        public IEnumerable<Employee> FindAllCompanyFithEmloyee()
        {
            var allEmployeers = FindAll(false);
            var allEmployeersWhithCompany = allEmployeers.Include(x => x.Company).ToList();
            return allEmployeersWhithCompany;
        }

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            Delete(employee);
        }

        public IEnumerable<Employee> GetEmployeeByCondition(Guid id, bool trackChanges) =>
            FindByCondition(x => x.Id.Equals(id), trackChanges);

        public Employee GetEmployee(Guid companyId, Guid id, bool trackChanges) =>
            FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges).SingleOrDefault();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee, EmployeeParameters employeeParameters)
        {
            throw new NotImplementedException();
        }
    }
}
