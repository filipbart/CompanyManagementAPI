using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyManagementApi.Models;
using Newtonsoft.Json;

namespace CompanyManagementAPI.Controllers
{
    [Route("/company")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly CompanyContext _context;

        public CompaniesController(CompanyContext context)
        {
            _context = context;
        }

        // GET: /company
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> Getcompany()
        {
            return await _context.Companies.Include(e => e.Employees).ToListAsync();
        }

        [HttpGet("/companies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies(IEnumerable<Company> companies)
        {
            return await _context.Companies.Where(c => c == companies).ToListAsync();
        }

        // GET: /company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return await _context.Companies.Include(e => e.Employees).FirstAsync(c => c.Id == id);
        }

        // PUT: /company/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: /company
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<ActionResult<Company>> PostCompany(int id, Company company)
        {
            //Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj.ToString());
            //var employees = JsonConvert.DeserializeObject<ICollection<Employee>>(dict["Employees"].ToString());
            //company.Name = dict["Name"].ToString();
            company.Id = id;
            foreach(var emp in company.Employees)
            {   
                _context.Employees.Add(emp);
            }
            _context.Companies.Add(company);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            

            return CreatedAtAction(nameof(GetCompany), new { id = company.Id });
        }

        // POST: /company/search
        [HttpPost("/company/search")]
        public async Task<ActionResult<Company>> Search(Object obj)
        {

            Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj.ToString());
            var companies = await _context.Companies.Where(c => c.Name.ToLower().Contains(dict["Keyword"].ToString().ToLower()))
                                                    .Include(e => e.Employees.Where(e => e.salary >= int.Parse(dict["EmployeeSalaryFrom"].ToString()) 
                                                                                    && e.salary <= int.Parse(dict["EmployeeSalaryTo"].ToString())))
                                                    .ToListAsync();
            return CreatedAtAction(nameof(GetCompanies), companies);
        }

        // DELETE: /company/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            
            company = await _context.Companies.Include(e => e.Employees).FirstAsync(c => c.Id == id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
