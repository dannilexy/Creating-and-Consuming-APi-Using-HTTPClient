using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using ParkyApi.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyApi.Controllers
{
    [Route("api/v{version:int}/[controller]")]
    [ApiController]
    [ProducesResponseType(400)]
    //[ApiExplorerSettings(GroupName = "v1Trails")]

    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository _trRepo;
        private readonly IMapper mapper;
        public TrailsController(ITrailRepository _trRepo, IMapper mapper)
        {
            this._trRepo = _trRepo;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a List of Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type =typeof(List<TrailDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetTrails()
        {
            var trails = _trRepo.GetTrails();
            var objDto = new List<TrailDto>();
            foreach (var obj in trails)
            {
                objDto.Add(mapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get a List of Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTrailsByNationalParkId/{npId:int}")]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetTrailsByNationalParkId(int npId)
        {
            var trails = _trRepo.GetTrailsInNationalPark(npId);
            var objDto = new List<TrailDto>();
            foreach (var obj in trails)
            {
                objDto.Add(mapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);
        }


        /// <summary>
        /// Get Individual Trail
        /// </summary>
        /// <param name="trailId">The Id Of the Trail</param>
        /// <returns></returns>
        [HttpGet("GetTrail/{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type =typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var trail = _trRepo.GetTrail(trailId);
            if (trail == null)
            {
                return NotFound();
            }
            var objDto = mapper.Map<TrailDto>(trail);
            return Ok(objDto);
        }


        /// <summary>
        /// Create National Pack
        /// </summary>
        /// <param name="trailDto"></param>
        /// <returns></returns>
        [HttpPost("CreateTrail")]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail(TrailCreateDto trailDto)
        {
            if (trailDto == null)
                return BadRequest(ModelState);
            bool exists = _trRepo.TrailExist(trailDto.Name);
            if (exists)
                return BadRequest("Trail Already Exists");
            if (ModelState.IsValid)
            {
                var trail = mapper.Map<Trail>(trailDto);
                bool created = _trRepo.CreateTrail(trail);
                if (created)
                {
                    return CreatedAtRoute("GetTrail", new { trailId  = trail.Id}, trail);
                }
                return StatusCode(500, "An error Occured while performing Operation!");
            }
            return BadRequest(ModelState);
            
        }

        /// <summary>
        /// Update Trail 
        /// </summary>
        /// <param name="TrailUpdateDto"></param>
        /// <param name="trailId"></param>
        /// <returns></returns>
        [HttpPut("UpdateTrail/{trailId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateTrail(TrailUpdateDto trailDto,int trailId)
        {
            if (trailDto.Id != trailId)
            {
                return BadRequest();
            }
            var obj = _trRepo.TrailExist(trailId);
            if (obj == false)
            {
                return NotFound("Trail Not found!");
            }
            if (ModelState.IsValid)
            {
                var trail = mapper.Map<Trail>(trailDto);
                bool updated = _trRepo.UpdateTrail(trail);
                if (updated)
                {
                    return StatusCode(204, "Trail Updated Successfully");
                }
                return StatusCode(500, "Something went wrong while performing operation");
            }
            return BadRequest();
          
            
        }

        /// <summary>
        /// Delete Trails
        /// </summary>
        /// <param name="trailId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteTrail/{trailId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteTrail(int trailId)
        {
            var exist = _trRepo.TrailExist(trailId);
            if (exist)
            {
                _trRepo.DeleteTrail(trailId);
                return Ok("Trail deleted");
            }
            return NotFound();
        }
    }
}
