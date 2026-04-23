using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _service;

    public MembersController(IMemberService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var members = await _service.GetAllAsync();
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var member = await _service.GetByIdAsync(id);
        if (member == null) return NotFound(new { error = "Member not found" });

        return Ok(member);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMemberRequest request)
    {
        try
        {
            var member = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateMemberRequest request)
    {
        try
        {
            var success = await _service.UpdateAsync(id, request);
            if (!success) return NotFound(new { error = "Member not found" });

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound(new { error = "Member not found" });

        return Ok();
    }
}