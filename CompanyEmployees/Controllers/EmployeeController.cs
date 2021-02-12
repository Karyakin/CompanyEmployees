using AutoMapper;
using CompanyEmployees.Filters;
using Conrtacts;
using Entities.Dto_DataTransferObjects__;
using Entities.Models;
using Entities.PagingParametrs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    // [Route("api/[Controller]")]

    [Route("api/Employee")]
    [ApiController]
    [TypeFilter(typeof(ValidateModelAttribute))]
   // [ValidateModel]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeController(IRepositoryWrapper repositoryWrapper, ILoggerManager loggerManager, IMapper mapper)
        {
            _wrapper = repositoryWrapper;
            _logger = loggerManager;
            _mapper = mapper;
        }
        
        [HttpGet("all")]//Мы используем [FromQuery] чтобы указать, что мы будем использовать параметры запроса, чтобы определить, какую страницу и сколько сотрудников мы запрашиваем.
        public IActionResult GetAllEmployees([FromQuery] EmployeeParameters employeeParameters)
        {

          

            if (!employeeParameters.ValidAgeRange)
                return BadRequest("Max age can't be less than min age.");
            try
            {
                var emloeers = _wrapper.Employee.FindAllEmployeere(trackChanges: false, employeeParameters);
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(emloeers.MetaData));

                var count = emloeers.Count();
                if (count <= 0)
                {
                    _logger.LogError("Сотрудников в базе нет");
                    return BadRequest("Сотрудников в базе нет");
                }
                _logger.LogInfo($"Сотрудники были успешно получены. Количество: {count}");
                _mapper.Map<IEnumerable<EmployeeDto>>(emloeers);
                return Ok(emloeers);
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
        public IActionResult GetAllEmployeeSortByAge()
        {
            try
            {
                var employee = _wrapper.Employee.GetEmployeeSortByAge(trackChanges: false);
                var count = employee.Count();
                if (count <= 0)
                {
                    _logger.LogError("Сотрудников в базе нет");
                    return BadRequest("Сотрудников в базе нет");
                }
                _logger.LogInfo($"Сотрудники были успешно получены и отсортированны по возрасту. Количество: {count}");
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"При попытке получения Компаний произошла ошибка. Подробности " +
                   $"{ex.Message},\n {ex.Source}, \n {ex.HelpLink}");
                return BadRequest($"При попытке получения Компаний произошла ошибка. Подробности " +
                    $"{ex.Message},\n {ex.Source}, \n {ex.HelpLink}");
            }
        }

        [HttpGet("{age}")]
        public IActionResult GetEmployeeByCompany(int age)
        {
            try
            {
                var employeeByAge = _wrapper.Employee.GetEmployeeByCondition(age);
                var count = employeeByAge.Count();
                if (count <= 0)
                {
                    _logger.LogError($"Сотрудников в возрасте {age} не существует");
                    return BadRequest($"Сотрудников в возрасте {age} не существует");
                }
                _logger.LogInfo($"Сотрудники были успешно получены и отсортированны по возрасту. Количество: {count}");
                return Ok(employeeByAge);
            }
            catch (Exception ex)
            {
                _logger.LogError($"При попытке получения Компаний произошла ошибка. Подробности " +
                   $"{ex.Message},\n {ex.Source}, \n {ex.HelpLink}");
                return BadRequest($"При попытке получения Компаний произошла ошибка. Подробности " +
                    $"{ex.Message},\n {ex.Source}, \n {ex.HelpLink}");
            }
        }

        [HttpGet("empcomp/{companyId}")]
        public IActionResult GetEmployeesCompany(Guid companyId)
        {
            try
            {
                var company = _wrapper.Company.FindCompanyByIdAsync(companyId, false);
                if (company == null)
                {
                    _logger.LogError($"Компании с Id {companyId} не существует в базе данных");
                    return NotFound($"Компании с Id {companyId} не существует в базе данных");
                }
                var employeesWithCompany = _wrapper.Employee.FindeEmployeesCompany(companyId, false);
                var employeesWithCompanyDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithCompany);
                return Ok(employeesWithCompanyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"При попытке получения Компаний произошла ошибка. Подробности " +
                     $"{ex.Message},\n {ex.Source}, \n {ex.HelpLink}");
                return BadRequest($"При попытке получения Компаний произошла ошибка. Подробности " +
                    $"{ex.Message},\n {ex.Source}, \n {ex.HelpLink}");
            }
        }

        [HttpGet]
        public IEnumerable<Employee> GetAllCompanyFithEmloyee()
        {
            var allComp = _wrapper.Employee.FindAllCompanyFithEmloyee().ToList();
            return allComp;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany([FromBody] EmployeeForCreationDto employee)
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

            #endregion// проверяется в атрибуте
            // перенанесно в ValidateModelAttribute
            /* if (!ModelState.IsValid)
             {
                 ModelState.AddModelError("Age", "Хуй");// необязательный метод. Можно к определенным полям добавлять пользовательское сообщение об ошибке
                 ModelState.AddModelError("Name", "Хуй");// необязательный метод. Можно к определенным полям добавлять пользовательское сообщение об ошибке
                 _logger.LogError("Invalid model state for the EmployeeForCreationDto object");
                 return UnprocessableEntity(ModelState);
             }*/


            try
            {
                // перенанесно в ValidateModelAttribute
                /*if (employee == null)
                {
                    _logger.LogError("EmployeeForCreationDto object sent from client is null.");
                    return BadRequest("EmployeeForCreationDto object is null");
                }*/

                var companyall = (await _wrapper.Company?.GetAllCompaniesSortByNameAsunc(false)).ToList();
                var companyByName = companyall?.Where(x => x.Name.Equals(employee.CompanyName))?.SingleOrDefault();

                if (companyByName == null)
                {
                    _logger.LogError("EmployeeForCreationDto object sent from client is null.");
                    return BadRequest("EmployeeForCreationDto object is null");
                }

                var company = await _wrapper.Company.GetCompanyAsync(companyByName.Id, false);
                if (company == null)
                {
                    _logger.LogInfo($"Company with id: {companyByName.Id} doesn't exist in the database.");
                    return NotFound();
                }
                var employeeEntity = _mapper.Map<Employee>(employee);
                _wrapper.Employee.CreateEmployeeForCompany(companyByName.Id, employeeEntity);
               await _wrapper.SaveAsync();
                var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

                return CreatedAtRoute(routeValues: new { id = employeeToReturn.Id }, value: employeeToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"При при попытке добавить пользователя {employee.Name} произошла ошибка. Подробности" +
                   $"{ex.Message}, {ex.Data}, {ex.TargetSite}");
                return BadRequest($"При попвтке создания организации с с названием {employee.Name} произошла ошибка. Подробности" +
                     $"{ex.Message}, {ex.Data}, {ex.TargetSite}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            try
            {
                var allEmployeerFoDelete = _wrapper.Employee.GetEmployeeByCondition(id).ToList();
                if (allEmployeerFoDelete.Count > 1)
                {
                    _logger.LogInfo("Несколько пользователей");
                    return BadRequest("Несколько пользователей");
                }

                if (allEmployeerFoDelete == null || allEmployeerFoDelete.Count < 1)
                {
                    _logger.LogInfo($"Нет таких {id}");
                    return BadRequest($"Нет таких {id}");
                }
                var employeerFoDelete = allEmployeerFoDelete.SingleOrDefault();

                _wrapper.Employee.DeleteEmployee(employeerFoDelete);
                _wrapper.SaveAsync();
                _logger.LogInfo($"Пользователь {employeerFoDelete.Name} удален");
                return Ok($"Пользователь {employeerFoDelete.Name} удален");
            }
            catch (Exception ex)
            {
                _logger.LogError($"При удалении произошла ошибка. произошла ошибка. Подробности {ex.Message}, {ex.Data}, {ex.TargetSite}");
                return BadRequest($"При удалении произошла ошибка. произошла ошибка. Подробности {ex.Message}, {ex.Data}, {ex.TargetSite}");
            }
        }
        #region Описание GetEmployeeByCondition
        /*В trackChanges параметр установлен на true для employeeEntity. 
          Это потому, что мы хотим, чтобы EF Core отслеживал изменения в этой сущности. 
          Это означает, что как только мы изменим какое-либо свойство в этой сущности, 
          EF Core установит состояние этой сущности на Modified.
          Как видите, мы отображаем employee объект (мы изменим в запросе только свойство age) на employeeEntity - таким образом 
          изменяя состояние employeeEntity объект к измененному.
          Поскольку наша сущность имеет измененное состояние, достаточно вызвать Save без дополнительных действий по обновлению. 
          Как только мы позвоним Save, наша сущность будет обновлена в базе данных.
        */
        #endregion
        [HttpPut("{id}")]
        public IActionResult UpdateEmployeeForCompany(Guid id, [FromBody] EmployeeForUpdateDto employee)
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

            if (employee == null)
            {
                _logger.LogError("EmployeeForUpdateDto object sent from client is null.");
                return BadRequest("EmployeeForUpdateDto object is null");
            }

            var companiesId = _wrapper.Company.
                FindCompaniesWhithEmployeeQuer().
                FirstOrDefault(x => x.Employees.Any(e => e.Id == id)).Id;

            var employeeEntity = _wrapper.Employee.GetEmployee(companiesId, id, trackChanges: true);

            if (employeeEntity == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(employee, employeeEntity);
            _wrapper.SaveAsync();

            return NoContent();
        }


        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateEmployeeForCompany(Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            
            
           


            if (patchDoc.Equals(null))
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var companyId = _wrapper.Company.
              FindCompaniesWhithEmployeeQuer().
              FirstOrDefault(x => x.Employees.Any(e => e.Id == id)).Id;

            var company = _wrapper.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company.Equals(null))
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var employeeEntity = _wrapper.Employee.GetEmployee(companyId, id, trackChanges: true);
            if (employeeEntity == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return NotFound();
            }




            //1. Принимаем patchDoc из тела запроса
            //2. Затем из полученного работника делаем EmployeeForUpdateDto
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            //3. Выполняем применение изменений
            patchDoc.ApplyTo(employeeToPatch, ModelState);// для валидации данных Patch запроса
            TryValidateModel(employeeToPatch);//проверка на сохранение недействительных данных в базу, если удаилить это
                                              //то при удалении данные в контексте будут Null 
                                              //и при сохранеии изменений мы получим исключение
                                              // а так перед сохранением мы проведем валидацию и сообщим об ошибке

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }


            _mapper.Map(employeeToPatch, employeeEntity);
            _wrapper.SaveAsync();
            return CreatedAtRoute(routeValues: new { employeeToPatch }, value: employeeToPatch);
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");

            return Ok();
        }


    }
}
