using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StroyOnline_API.Data;
using StroyOnline_API.Models;

namespace StroyOnline_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly StroyOnlineDBContext _stroyOnlineDBContext;

        public EmployeesController(StroyOnlineDBContext stroyOnlineDBContext)
        {
            _stroyOnlineDBContext = stroyOnlineDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees() // метод для возврата списка сотрудников
        {
            var employees = await _stroyOnlineDBContext.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employeeRequest)
        {
            employeeRequest.Id = Guid.NewGuid();

            await _stroyOnlineDBContext.Employees.AddAsync(employeeRequest);
            await _stroyOnlineDBContext.SaveChangesAsync();

            return Ok(employeeRequest);
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id) // метод для возврата модели сотрудника для редактирования
        {
            var employee = await _stroyOnlineDBContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> PutEmployee([FromRoute] Guid id, Employee updateEmployeeRequest) // метод для редактирования существующей записи
        {
            var employee = await _stroyOnlineDBContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee.Firstname = updateEmployeeRequest.Firstname;
            employee.Surname = updateEmployeeRequest.Surname;
            employee.Lastname = updateEmployeeRequest.Lastname;
            employee.Birthday = updateEmployeeRequest.Birthday;
            employee.PositionId = updateEmployeeRequest.PositionId;
            employee.Salary = updateEmployeeRequest.Salary;
            employee.IsActive = updateEmployeeRequest.IsActive;


            await _stroyOnlineDBContext.SaveChangesAsync();

            return Ok(employee); // или updateEmployeeRequest
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id) // метод для удаления записи
        {
            var employee = await _stroyOnlineDBContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            _stroyOnlineDBContext.Employees.Remove(employee);
            await _stroyOnlineDBContext.SaveChangesAsync();

            return Ok(employee);
        }
    }
}
