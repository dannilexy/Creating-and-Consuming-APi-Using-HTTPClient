using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using ParkyApi.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyApi.Controllers
{
    [Route("api/v{version:int}/NationalParks")]
    [ApiController]
    [ProducesResponseType(400)]
    //[ApiExplorerSettings(GroupName = "v1Np")]

    public class NationalParkV2Controller : ControllerBase
    {
        private readonly INationalPark _np;
        private readonly IMapper mapper;
        public NationalParkV2Controller(INationalPark _np, IMapper mapper)
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


       
    }
}
