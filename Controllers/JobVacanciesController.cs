namespace DevJobs.API.Controllers
{
    using System.Linq;
    using DevJobs.API.Entities;
    using DevJobs.API.Models;
    using DevJobs.API.Persistence;
    using DevJobs.API.Persistence.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class JobVacanciesController : ControllerBase
    {
    
        private readonly IJobVacancyRepository _repository;
        public JobVacanciesController(IJobVacancyRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var jobVacancies = _repository.GetAll();

            return Ok(jobVacancies);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var jobVacancy = _repository.GetById(id);

            if(jobVacancy == null)
            {
                return NotFound();
            }
            return Ok(jobVacancy);
        }

        /// <summary>
        ///  Cadastrar uma vaga de emprego.
        /// </summary>
        /// <remarks>
        /// {
        /// "title":"Dev .NET Jr",
        /// "description": Vaga para sustentação de aplicações .NET Core",
        /// "company":"Dev",
        /// "isRemote": true,
        /// "salaryRange": "3000 a 5000"
        /// }
        /// </remarks>
        /// <param name="model">Dados da vaga.</param>
        /// <returns>Objeto Criado</returns>
        /// <response code="201">Sucesso</response>

        [HttpPost]
        public IActionResult Post(AddJobVacancyInputModel model)
        {
            var jobVacancy = new JobVacancy(
                model.Title,
                model.Description,
                model.Company,
                model.IsRemote,
                model.SalaryRange
            );

            _repository.Add(jobVacancy);

            return CreatedAtAction("GetById", new { id = jobVacancy.Id }, jobVacancy);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateJobVacancyInputModel model)
        {
            var jobVacancy = _repository.GetById(id);
            if(jobVacancy == null)
            {
                return NotFound();
            }

            jobVacancy.Update(model.Title, model.Description);
            _repository.Update(jobVacancy);
            
            return NoContent();
        }
    }
}