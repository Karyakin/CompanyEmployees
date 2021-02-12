using Conrtacts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorys
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesSortByNameAsunc(bool trackChanges) => await
                FindAll(trackChanges)
                .OrderByDescending(x => x.Name)
                .ToListAsync();
        public async Task<Company> FindCompanyByIdAsync(Guid companyId, bool trackChanges)
        {
            //Две разных реализации, возвращают одно итоже
            var a = await FindByCondition(x => x.Id.Equals(companyId), trackChanges).FirstOrDefaultAsync();
            var b = await FindAll(trackChanges).Where(x => x.Id.Equals(companyId)).FirstOrDefaultAsync();
            return b;
        }
        public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) => await
            FindByCondition(x => x.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();
        public void CreateCompany(Company company) => Create(company);
        public async Task<IEnumerable<Company>> FindByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => await
            FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        public void DeleteCompany(Company company)
        {
            Delete(company);
        }
        public IQueryable<Company> FindCompaniesWhithEmployeeQuer()=>
            FindAll(false).Include(x => x.Employees);
        public async Task<IEnumerable<Company>> FindCompaniesWhithEmployeeAsync() => await
            FindAll(false).Include(x => x.Employees).ToListAsync();
    }
}
