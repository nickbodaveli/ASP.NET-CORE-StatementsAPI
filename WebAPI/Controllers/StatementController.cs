using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.IRepository;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatementController : ControllerBase
    {
        private readonly IStatementRepository _repository;
 
        public StatementController(IStatementRepository repository)
        {
            _repository = repository;
           
        }

        [HttpPost(Name = "Post")]
        [ActionName("Post")]
        public async Task<IActionResult> Post([FromForm] Statement statement)
        {
            var addStatement = await _repository.AddStatementAsync(statement);
            return Ok(addStatement);
        }

        [HttpGet(Name = "GetBId")]
        [ActionName("GetById")]
        public async Task<IActionResult> GetStatementById(int id)
        {
            var _statement = await _repository.GetStatementAsync(id);
           
            if (_statement != null)
                return Ok(_statement);
            else
                return NotFound();
        }
        [HttpDelete("{id}", Name = "Delete")]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var _statement = await _repository.DeletePersonAsync(id);

            if (_statement != null)
                return Ok(_statement);
            else
                return NotFound();
        }

        [HttpPut("{id}", Name = "Update")]
        [ActionName("Update")]
        public async Task<IActionResult> Update(int id, [FromForm] Statement statement)
        {
            var _statement = await _repository.UpdatePersonAsync(id, statement);

            if (_statement != null)
                return Ok(_statement);
            else
                return NotFound();
        }

        [HttpGet(Name = "GetAllStatement")]
        [ActionName("GetAllStatement")]
        public async Task<IActionResult> GetAll()
        {
            var _statement = await _repository.GetAllStatementAsync();

            if (_statement != null)
                return Ok(_statement);
            else
                return NotFound();
        }



    }
}
