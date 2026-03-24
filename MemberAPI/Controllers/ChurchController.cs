using Microsoft.AspNetCore.Mvc;
using MemberAPI.Models;
using MemberAPI.Repositories;

namespace MemberAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChurchController : ControllerBase
{
    private readonly ChurchRepository _repo;

    public ChurchController(ChurchRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _repo.GetAllAsync());
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var church = await _repo.GetByIdAsync(id);

        return church is null ? NotFound() : Ok(church);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Church church)
    {
        var created = await _repo.CreateAsync(church);
        return CreatedAtAction(nameof(GetById), new{id = created.Id}, created);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Church church)
    {
        var updated = await _repo.UpdateAsync(church);
        return updated ? NoContent() : NotFound();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repo.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

}