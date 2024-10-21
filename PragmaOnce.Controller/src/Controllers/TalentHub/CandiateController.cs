using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PragmaOnce.Core.src.Entities;
using PragmaOnce.Service.src.DTOs.Shop;
using PragmaOnce.Service.src.DTOs.TalentHub;
using PragmaOnce.Service.src.Interfaces;
using PragmaOnce.Service.src.Interfaces.TalentHub;
using PragmaOnce.Service.src.Services;

namespace PragmaOnce.Controller.src.Controllers.TalentHub
{


    [ApiController]
    [Route("api/v1/candidate")]
    public class CandiateController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private ICandidateService _candidateService;
        public CandiateController(IUserService userService, IAuthorizationService authorizationService, ICandidateService candidateService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _candidateService = candidateService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ActionName(nameof(GetCandidateByIdAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CandidateReadDto>> GetCandidateByIdAsync([FromRoute] Guid id)
        {
            var category = await _candidateService.GetOneByIdAsync(id);
            return Ok(category);
        }


        [HttpPatch("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryReadDto>> UpdateCandidateAsync([FromRoute] Guid id, [FromForm] CandidateUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedCandidate = await _candidateService.UpdateOneAsync(id, updateDto);
            return Ok(updatedCandidate);
        }

    }
}
