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
    //[ApiExplorerSettings(GroupName = "v1Np")]

    public class NationalParkController : ControllerBase
    {
        private readonly INationalPark _np;
        private readonly IMapper mapper;
        public NationalParkController(INationalPark _np, IMapper mapper)
        {
            this._np = _np;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a List of National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type =typeof(List<NationalParkDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetNationalPacks()
        {
            var nationalParks = _np.GetNamtionalParks();
            var objDto = new List<NationalParkDto>();
            foreach (var obj in nationalParks)
            {
                objDto.Add(mapper.Map<NationalParkDto>(obj));
            }
            return Ok(objDto);
        }


        /// <summary>
        /// Get Individual National Park
        /// </summary>
        /// <param name="nationalParkId">The Id Of the National Park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type =typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _np.GetNationalPark(nationalParkId);
            if (nationalPark == null)
            {
                return NotFound();
            }
            var objDto = mapper.Map<NationalParkDto>(nationalPark);
            return Ok(objDto);
        }


        /// <summary>
        /// Create National Pack
        /// </summary>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPost("CreateNationalPark")]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        public IActionResult CreateNationalPark(NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
                return BadRequest(ModelState);
            bool exists = _np.NationalParkExist(nationalParkDto.Name);
            if (exists)
                return BadRequest("National Park Already Exists");
            if (ModelState.IsValid)
            {
                var nationalPark = mapper.Map<NationalPark>(nationalParkDto);
                bool created = _np.CreateNationalPark(nationalPark);
                if (created)
                {
                    return CreatedAtRoute("GetNationalPark", new { nationalParkId  = nationalPark.Id}, nationalPark);
                }
                return StatusCode(500, "An error Occured while performing Operation!");
            }
            return BadRequest(ModelState);
            
        }

        /// <summary>
        /// Update National park 
        /// </summary>
        /// <param name="nationalParkDto"></param>
        /// <param name="nationalParkId"></param>
        /// <returns></returns>
        [HttpPut("UpdateNationalPark/{nationalParkId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateNationalPark(NationalParkDto nationalParkDto,int nationalParkId)
        {
            var obj = _np.NationalParkExist(nationalParkId);
            if (obj == false)
            {
                return NotFound("National Park Not found!");
            }
            if (ModelState.IsValid)
            {
                var nationalPark = mapper.Map<NationalPark>(nationalParkDto);
                bool updated = _np.UpdateNationalPark(nationalPark);
                if (updated)
                {
                    return StatusCode(204, "National Park Updated Successfully");
                }
                return StatusCode(500, "Something went wrong while performing operation");
            }
            return BadRequest();
          
            
        }

        /// <summary>
        /// Delete National Parks
        /// </summary>
        /// <param name="nationalParkId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteNationalPark/{nationalParkId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            var exist = _np.NationalParkExist(nationalParkId);
            if (exist)
            {
                _np.DeleteNationalPark(nationalParkId);
                return Ok("National Park deleted");
            }
            return NotFound();
        }
    }
}
