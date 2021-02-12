using AutoMapper;
using CompanyEmployees.Filters;
using CompanyEmployees.Filters.TestsFlters;
using Conrtacts;
using Entities.Dto_DataTransferObjects__;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{

    public class UserController : CustomController
    {
        public ILoggerManager _logger { get; }
        public IRepositoryWrapper _wrapper { get; }
        public IMapper _mapper { get; }
        public UserController(ILoggerManager logger, IRepositoryWrapper wrapper, IMapper mapper)
        {
            _logger = logger;
            _wrapper = wrapper;
            _mapper = mapper;
        }


        [HttpPut("new")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [CustomExceptionFilter]
        public IActionResult GreateNewUser(UserForCreationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            _wrapper.User.Create(user);
            _wrapper.Save();
            userDto = _mapper.Map<UserForCreationDto>(user);
            return CreatedAtRoute(new { userDto }, userDto);
        }


        [HttpDelete("{id}")]
        [ServiceFilter(typeof(UserExistsAttribute))]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            // var user = HttpContext.Items["user"] as User;
            var user = await _wrapper.User.GetUserByCondition(id);
            _wrapper.User.DeleteUser(user);
            await _wrapper.SaveAsync();

            return CreatedAtRoute(new { user }, user);
        }


        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(UserExistsAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] UserForCreationDto creationDto)
        {
            var userEntity = await _wrapper.User.GetUserByCondition(id);


            _mapper.Map(creationDto, userEntity);


            await _wrapper.SaveAsync();

            return NoContent();
        }
    }
}
