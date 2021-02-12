using Conrtacts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositorys;
using Entities.Dto_DataTransferObjects__;
using AutoMapper;
using Entities.Models;
using CompanyEmployees.ModelBinders;

namespace CompanyEmployees.Controllers
{


    public class CompaniesController : CustomController
    {
        public ILoggerManager _logger { get; }
        public IRepositoryWrapper _wrapper { get; }
        public IMapper _mapper { get; }

        public CompaniesController(ILoggerManager logger, IRepositoryWrapper wrapper, IMapper mapper)
        {
            _logger = logger;
            _wrapper = wrapper;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public IActionResult GetCompaniesAll()
        {
            try
            {
                var companies = _wrapper.Company.FindAll(trackChanges: false).ToList();

                #region Альтернатива для автомапера
                /* var companiesDto = companies.Select(x=> new CompanyDto 
                 { 
                     Id = x.Id,
                     Name = x.Name,
                     FullAddress = string.Join(',', x.Address, x.Country) 

                 }).ToList();*/
                #endregion
                var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

                var count = companiesDto.Count();
                if (count <= 0)
                {
                    _logger.LogError("Компаний в базе нет");
                    return BadRequest("Компаний в базе нет");
                }
                _logger.LogInfo($"Компании были успешно получены. Количество: {count}");
                return Ok(companiesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"При попытке получения Компаний произошла ошибка. Подробности " +
                    $"{ex.Message},\n {ex.Source}, \n {ex.HelpLink}");
                return BadRequest($"При попытке получения Компаний произошла ошибка. Подробности " +
                    $"{ex.Message},\n {ex.Source}, \n {ex.HelpLink}");
            }
        }

        [HttpGet("allSort")]
        public async Task<IActionResult> GetCompanisSotrByNameAsync()
        {
            try
            {
                var companies = await _wrapper.Company.GetAllCompaniesSortByNameAsunc(trackChanges: false);

                #region Альтернатива автомаперу
                /* var companiesDto = companies.Select(x => new CompanyDto
                 {
                     Id = x.Id,
                     Name = x.Name,
                     FullAddress = string.Join(',', x.Address, x.Country)
                 }) ;*/
                #endregion
                var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

                var count = companiesDto.Count();
                if (count <= 0)
                {
                    _logger.LogError("Компаний в базе нет");
                    return BadRequest("Компаний в базе нет");
                }
                _logger.LogInfo($"Компании были успешно получены. Количество: {count}");
                return Ok(companiesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"При попытке получения Компаний произошла ошибка. Подробности " +
                    $"{ex.Message},\n {ex.Source}, \n {ex.HelpLink}");
                return BadRequest($"При попытке получения Компаний произошла ошибка. Подробности " +
                    $"{ex.Message},\n {ex.Source}, \n {ex.HelpLink}");
            }
        }

        [HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompaniesByID(Guid Id)
        {
            try
            {
                var companyById = await _wrapper.Company.FindCompanyByIdAsync(Id, trackChanges: false);
                if (companyById == null)
                {
                    _logger.LogError($"Пользователь с Id {Id} отсутсвует в базе данных");
                    return NotFound($"Пользователь с Id {Id} отсутсвует в базе данных");
                }
                _logger.LogInfo($"Из базы была извлечена компания {companyById.Name}");
                return Ok(_mapper.Map<CompanyDto>(companyById));
            }
            catch (Exception ex)
            {
                _logger.LogError($"При попвтке получения пользователя с Id{Id} произошла ошибка. Подробности" +
                    $"{ex.Message}, {ex.Data}, {ex.TargetSite}");
                return BadRequest($"При попвтке получения пользователя с Id{Id} произошла ошибка. Подробности" +
                    $"{ex.Message}, {ex.Data}, {ex.TargetSite}");
            }
        }

        [HttpGet]
        public async Task<IEnumerable<CompanyDto>> GetCompaniesWhithEmployee()
        {
            try
            {
                var companyWithEmployee = (await _wrapper.Company.FindCompaniesWhithEmployeeAsync()).ToList();
                if (companyWithEmployee == null || companyWithEmployee.Count <= 0)
                {
                    _logger.LogInfo("Список компаний получит не удалось");
                }
                return _mapper.Map<IEnumerable<CompanyDto>>(companyWithEmployee); ;
            }
            catch (Exception ex)
            {
                _logger.LogError($"При попвтке получения компании с пользователем произошла ошибка. Подробности" +
                  $"{ex.Message}, {ex.Data}, {ex.TargetSite}");
                return null;
            }
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
        {

            #region использование валидации

            /*Чтобы вернуть 422 вместо 400, первое, что нам нужно сделать, это подавить 
             BadRequest ошибка, когда ModelState является недействительным. 
             Мы собираемся сделать это, добавив этот код в Startup класс в ConfigureServices метод:
            services.Configure<ApiBehaviorOptions>(options =>
            {
            options.SuppressModelStateInvalidFilter = true;
            });
            */

            #endregion
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the EmployeeForCreationDto object");
                return UnprocessableEntity(ModelState);
            }


            var checkCompany = _wrapper.Company.FindByCondition(x => x.Name.Equals(company.Name), false).ToList();
            if (checkCompany.Count != 0)
            {
                _logger.LogError("Добавить компанию не удалось. Компания с таким именем уже существует");
                return BadRequest("Добавить компанию не удалось. Компания с таким именем уже существует");
            }

            if (company == null)
            {
                _logger.LogError("CompanyForCreationDto object sent from client is null.");
                return BadRequest("CompanyForCreationDto object is null");
            }
            try
            {
                var companyEntity = _mapper.Map<Company>(company);
                _wrapper.Company.CreateCompany(companyEntity);
                _wrapper.SaveAsync();
                var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
                return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);//CreatedAtRoute Метод предназначен для возвращения URI для вновь созданного ресурса при вызове метода POST для хранения какой - то новый объект.
            }
            catch (Exception ex)
            {
                _logger.LogError($"При попвтке создания организации с с названием {company.Name} произошла ошибка. Подробности" +
                     $"{ex.Message}, {ex.Data}, {ex.TargetSite}");
                return BadRequest($"При попвтке создания организации с с названием {company.Name} произошла ошибка. Подробности" +
                     $"{ex.Message}, {ex.Data}, {ex.TargetSite}");
            }
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetListCompany([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)// описание ArrayModelBinder смотри в классе
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var companyEntities = await _wrapper.Company.FindByIdsAsync(ids, trackChanges: false);
            if (ids.Count() != companyEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return Ok(companiesToReturn);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection == null)
            {
                _logger.LogError("Company collection sent from client is null.");
                return BadRequest("Company collection is null");
            }

            try
            {
                var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);

                foreach (var item in companyEntities)
                {
                    _wrapper.Company.CreateCompany(item);
                }
                _wrapper.SaveAsync();

                var companyCollectionToReturn = _mapper.Map<ICollection<CompanyDto>>(companyEntities);
                var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));//мы не можем просто передать список идентификаторов в CreatedAtRoute потому что нет поддержки для создания Header Location со списком
                return CreatedAtRoute("CompanyCollection", new { ids }, companyCollectionToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($" Произошла ошибка(описание и бла бла бла). Подробности {ex.Message}, {ex.Data}, {ex.TargetSite}");
                return BadRequest($" Произошла ошибка(описание и бла бла бла). Подробности {ex.Message}, {ex.Data}, {ex.TargetSite}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var companyWithEmployee = await _wrapper.Company.FindCompaniesWhithEmployeeAsync();
            var company = companyWithEmployee.Where(x => x.Id.Equals(id)).SingleOrDefault();
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return NotFound($"Company with id: {id} doesn't exist in the database.");
            }
            var emploeers = (company.Employees)?.ToList();
            var count = emploeers.Count();
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _wrapper.Company.DeleteCompany(company);
            await _wrapper.SaveAsync();
            _logger.LogInfo($"Удалена компания {company.Name} в которой было {count} сотрудников.");

            return Ok($"Удалена компания {company.Name} в которой было {count} сотрудников");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            #region использование валидации

            /*Чтобы вернуть 422 вместо 400, первое, что нам нужно сделать, это подавить 
             BadRequest ошибка, когда ModelState является недействительным. 
             Мы собираемся сделать это, добавив этот код в Startup класс в ConfigureServices метод:
            services.Configure<ApiBehaviorOptions>(options =>
            {
            options.SuppressModelStateInvalidFilter = true;
            });
            */

            #endregion
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the EmployeeForCreationDto object");
                return UnprocessableEntity(ModelState);
            }
            if (company == null)
            {
                _logger.LogError("CompanyForUpdateDto object sent from client is null.");
                return BadRequest("CompanyForUpdateDto object is null");
            }

            var companyEntity = _wrapper.Company.GetCompanyAsync(id, trackChanges: true);
            if (companyEntity == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(company, companyEntity);
            _wrapper.SaveAsync();

            return NoContent();
        }

    }
}
