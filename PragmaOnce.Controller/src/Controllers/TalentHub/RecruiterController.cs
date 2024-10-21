using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PragmaOnce.Service.src.DTOs.Shop;
using PragmaOnce.Service.src.DTOs.TalentHub;
using PragmaOnce.Service.src.Interfaces.TalentHub;

namespace PragmaOnce.Controller.src.Controllers.TalentHub
{
    [ApiController]
    [Route("api/v1/recruiter")]
    public class RecruiterController : ControllerBase
    {
        private readonly IRecruiterService _recruiterService;

        public RecruiterController(IRecruiterService recruiterService)
        {
            _recruiterService = recruiterService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ActionName(nameof(GetRecruiterByIdAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecruiterReadDto>> GetRecruiterByIdAsync([FromRoute] Guid id)
        {
            var category = await _recruiterService.GetOneByIdAsync(id);
            return Ok(category);
        }


        [HttpPatch("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryReadDto>> UpdateCandidateAsync([FromRoute] Guid id, [FromForm] RecruiterUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _recruiterService.UpdateOneAsync(id, updateDto);
            return Ok(updated);
        }
    }
}
