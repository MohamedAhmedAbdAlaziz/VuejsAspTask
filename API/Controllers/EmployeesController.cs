using API.DTOs.Entities;
using API.Error;
using API.Errors;
using API.Services;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IMapper _mapper;
        public EmployeesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _mapper= mapper;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllEmployees()
        {
            string Role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var employeeSpecification = new EmployeeSpecification();
               

            if (Role.ToLower().Trim()!= "admin")
            { 
                string UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                 employeeSpecification.FiltersByCreatedBy(UserId);
            }
            List<Employee> employees = await _unitOfWork.Repository<Employee>().ListAsync(employeeSpecification);
            var employeesDtos = _mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<EmployeeDTO>>(employees);

            return Ok(new SucceededRespone(200)
            {
                Data = employeesDtos
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetEmployeeByID(int id)
        {
            string? Role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var employeeSpecification = new EmployeeSpecification(id);
            if (Role.ToLower().Trim() != "admin")
            {
                string UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                employeeSpecification.FiltersByCreatedBy(UserId);
            }
            Employee employee = await _unitOfWork.Repository<Employee>().GetEntityWithSpec(employeeSpecification);
            var employeeDTO = _mapper.Map<Employee, EmployeeDTO>(employee);
            return Ok(new SucceededRespone(200)
            {
                Data = employeeDTO
            });
        }
 
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateEmployee(int id,[FromForm] EmployeeDTO model)
        {
           if(id!= model.Id)
            {
                return BadRequest(new FailResponse(400) { Errors = new string[] { "there is an error where update this Employee" } });
            }
            string? UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if(UserId == null)
            {
                return Unauthorized(new FailResponse(401));
            }
            Employee updatedEmployee = new Employee
            {
                Id=model.Id,
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                GraduationStatus = model.GraduationStatus,
                ImageUrI = SortImage(model.Photo),
                CreatedBy = UserId

            };
            _unitOfWork.Repository<Employee>().Update(updatedEmployee);
            try
            {
                await _unitOfWork.Complete();
            }
            catch
            {
                return StatusCode(500, new FailResponse(500) { Errors = new string[] { "there is an error where update this Employee" } });
            }
            return Ok(new SucceededRespone(200) { Data = model, Message = "updated succeessfully" });
        }

        [HttpPost]
        public async Task<ActionResult> PostEmployee([FromForm] EmployeeDTO model)
        {
             string UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
              

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    GraduationStatus= model.GraduationStatus,
                    ImageUrI = SortImage(model.Photo),
                    CreatedBy= UserId

                };
                _unitOfWork.Repository<Employee>().Add(newEmployee);
            try
            {
                await _unitOfWork.Complete();
            }
            catch
            {
                return StatusCode(500, new FailResponse(500) { Errors = new string[] { "error while Creating this Employee" } });
            }


            return CreatedAtAction("GetEmployeeByID", new { id = newEmployee.Id }, new SucceededRespone(201) { Data = newEmployee });

        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var Employees = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);
            if (Employees == null)
            {
                return BadRequest(new FailResponse(401) { Errors = new string[] { "UserName not Found" } });
            }
            _unitOfWork.Repository<Employee>().Delete(Employees);
            await _unitOfWork.Complete();


            return Ok(new SucceededRespone(200, "deleted succeessfully"));
        }

        private string SortImage(IFormFile Photo)
        {
            string uniqueFileName = "";
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + " " + "_" + Photo.FileName;
            string filePath = Path.Combine(uploadFolder, uniqueFileName);
            Photo.CopyTo(new FileStream(filePath, FileMode.Create));

            return uniqueFileName;
        }

       
    }
}
