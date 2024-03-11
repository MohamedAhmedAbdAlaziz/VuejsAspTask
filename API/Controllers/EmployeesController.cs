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
    public class EmployeesController : BaseApiController
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
        // Get ALL Employees 
        [HttpGet]
        public async Task<ActionResult> GetAllEmployees()
        {
            // check if Role type at the first
            // if Role is Admin get AllEmployees 
            // else it is a normal get just just Employees created by cuurent User
            string Role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var employeeSpecification = new EmployeeSpecification();
            if (Role != null &&  Role.ToLower().Trim()!= "admin")
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
            string Role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var employeeSpecification = new EmployeeSpecification(id);
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
                return BadRequest(new FailResponse(400) { Errors = new string[] { "there is an error while updating this Employee" } });
            }
           
            string UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if(UserId != null &&  UserId == null)
            {
                return Unauthorized(new FailResponse(401));
            }
            var employeeSpecification = new EmployeeSpecification(model.Id);
               Employee employee = await _unitOfWork.Repository<Employee>().GetEntityWithSpec(employeeSpecification);

               employee.Name = model.Name;
               employee.Email = model.Email;
               employee.PhoneNumber = model.PhoneNumber;
               employee.GraduationStatus = model.GraduationStatus;
               employee.ImageUrI = model.Photo == null ? employee.ImageUrI : SortImage(model.Photo);          
            try
            {
                await _unitOfWork.Complete();
            }
            catch
            {
                return StatusCode(500, new FailResponse(500) { Errors = new string[] { "there is an error where update this Employee" } });
            }
            return Ok(new SucceededRespone(200) { Data = employee, Message = "updated succeessfully" });
        }

        [HttpPost]
        public async Task<ActionResult> PostEmployee([FromForm] EmployeeDTO model)
        {
            var employeeSpecification = new EmployeeSpecification();
            employeeSpecification.FiltersByEmail(model.Email);
            Employee employee = await _unitOfWork.Repository<Employee>().GetEntityWithSpec(employeeSpecification);
            
            if (employee != null&& employee.Email == model.Email)
            {
                return BadRequest(new FailResponse(400) { Errors = new string[] { "this email is token" } });
            }

            string UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    GraduationStatus= model.GraduationStatus,
                    ImageUrI =  SortImage(model.Photo),
                    CreatedBy= UserId

                };
                _unitOfWork.Repository<Employee>().Add(newEmployee);
            try
            {
                await _unitOfWork.Complete();
            }
            catch
            {
                return StatusCode(500, new FailResponse(500) { Errors = new string[] { "Error while Creating this Employee" } });
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


        #region Sort Image
        private string SortImage(IFormFile Photo)
        {
            string uniqueFileName = "";
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + " " + "_" + Photo.FileName;
            string filePath = Path.Combine(uploadFolder, uniqueFileName);
            Photo.CopyTo(new FileStream(filePath, FileMode.Create));

            return uniqueFileName;
        }

        #endregion



    }
}
