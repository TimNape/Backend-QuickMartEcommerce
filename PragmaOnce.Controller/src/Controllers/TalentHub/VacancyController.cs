using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PragmaOnce.Service.src.DTOs.Shop;
using PragmaOnce.Service.src.DTOs.TalentHub;
using PragmaOnce.Service.src.Interfaces.TalentHub;
using PragmaOnce.Service.src.Services.TalentHub;

namespace PragmaOnce.Controller.src.Controllers.TalentHub
{
    [ApiController]
    [Route("/api/v1/vacancy")]
    public class VacancyController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;

        public VacancyController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ActionName(nameof(GetRecruiterByIdAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VacancyReadDto>> GetRecruiterByIdAsync([FromRoute] Guid id)
        {
            var vacancy = await _vacancyService.GetOneByIdAsync(id);
            return Ok(vacancy);
        }

        [HttpPatch("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryReadDto>> UpdateCandidateAsync([FromRoute] Guid id, [FromForm] VacancyUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _vacancyService.UpdateOneAsync(id, updateDto);
            return Ok(updated);
        }

    }
}
