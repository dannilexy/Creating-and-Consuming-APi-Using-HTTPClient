using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using System.Collections.Generic;

namespace ParkyApi.Repository.IRepository
{
    public interface INationalPark
    {
        ICollection<NationalPark> GetNamtionalParks();
        NationalPark GetNationalPark(int id);
        bool NationalParkExist(string name);    
        bool NationalParkExist(int id);  
        bool CreateNationalPark(NationalPark nationalPark);
        bool UpdateNationalPark(NationalPark nationalPark);
        bool DeleteNationalPark(int id);
        bool Save();

    }
}
