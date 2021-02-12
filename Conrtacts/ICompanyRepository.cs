using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conrtacts
{
    public interface ICompanyRepository : IRepositoryBase<Company>
    {
        Task<IEnumerable<Company>> GetAllCompaniesSortByNameAsunc(bool trackChanges);
        Task<Company> FindCompanyByIdAsync(Guid companyId ,bool trackChanges);
        Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges);
        Task <IEnumerable<Company>> FindCompaniesWhithEmployeeAsync();
        IQueryable<Company> FindCompaniesWhithEmployeeQuer();
        void CreateCompany(Company company);

        Task<IEnumerable<Company>> FindByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteCompany(Company company);
       
    }
}
