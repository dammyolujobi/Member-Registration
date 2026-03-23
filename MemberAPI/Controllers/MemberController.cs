using Microsoft.AspNetCore.Mvc;
using MemberAPI.Models;
using MemberAPI.Repositories;

namespace MemberAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly MemberRepository _repo;

    public MembersController(MemberRepository repository)
    {
        _repo = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => 
        Ok(await _repo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var member = await _repo.GetByIdAsync(id);
        return member is null ? NotFound() : Ok(member);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var result = await _repo.FindByName(name);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Member member)
    {
        var created = await _repo.CreateAsync(member);
        return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repo.DeleteAsync(id);
        return deleted? NoContent() : NotFound();
    }
    
}

