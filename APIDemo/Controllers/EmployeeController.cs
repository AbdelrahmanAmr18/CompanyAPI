using APIDemo.DTO;
using APIDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public EmployeeController(ApplicationContext context)
        {
            _context = context;
        }

        // getall 
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var employees = _context.Employees.ToList();
            return Ok(employees);
        }
        [HttpGet]
        [Route("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }
        [HttpGet]
        [Route("GetByIdAsync/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {

            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
            {
                return BadRequest($"no Employee has this id : {id}");
            }
            return Ok(emp);
        }


        [HttpDelete]
        [Route("DeleteEmployee/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
            {
                return BadRequest($"Employee not found has id = {id}");
            }
            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }

        [HttpPost]
        [Route("PostEmployee")]
        public async Task<IActionResult> PostEmployee(Employee employee)
        {
            if (employee != null)
            {
                if (ModelState.IsValid)
                {
                    await _context.Employees.AddAsync(employee);
                    await _context.SaveChangesAsync();
                    return Created("http://localhost:5131/swagger/index.html", employee);
                }

            }

            return BadRequest("invalied data");
        }

        [HttpPut]
        [Route("EditEmployee/{id}")]
        public async Task<IActionResult> EditEmployee(int id, Employee employeeNewData)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
            {
                return BadRequest();
            }
            emp.Name = employeeNewData.Name;
            emp.Age = employeeNewData.Age;
            emp.Salary = employeeNewData.Salary;
            emp.Address = employeeNewData.Address;
            emp.Phone = employeeNewData.Phone;
            emp.DepartmentId = employeeNewData.DepartmentId;
            // context.Entry(blog).State = EntityState.Added;
            //_context.Entry(employeeNewData).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet]
        [Route("GetEmployeeDataByIdAsync/{id}")]
        public async Task<IActionResult> GetEmployeeDataByIdAsync(int id )
        {
            var emp = await _context.Employees.Include(ww => ww.Department).Where(ww => ww.Id == id).FirstOrDefaultAsync();

            if (emp == null)
                return NotFound();

            EmployeeDTO employeeDTO = new EmployeeDTO();
            //manual mapping 
            employeeDTO.Name=emp.Name;
            employeeDTO.Age = emp.Age;
            employeeDTO.Salary = emp.Salary;
            employeeDTO.DepartmentName = emp.Department?.DepartmentName;

            return Ok(employeeDTO);
        }


        [HttpGet]
        [Route("GetAllEmployeesDtoAsync")]
        public async Task <IActionResult> GetAllEmployeesDtoAsync()
        {
          var emps =  await _context.Employees.Include(ww => ww.Department).ToListAsync();

            List<EmployeeDTO> employees = new List<EmployeeDTO>();

             foreach (var employee in emps)
            {
                EmployeeDTO obj = new EmployeeDTO();
                obj.Name = employee.Name;
                obj.Age= employee.Age;
                obj.Salary = employee.Salary;
                obj.DepartmentName=employee.Department?.DepartmentName;
                employees.Add(obj);
            }
           

            return Ok(employees);
        }

    }
}
