using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using System.Collections.Generic;

namespace ParkyApi.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();
        ICollection<Trail> GetTrailsInNationalPark(int npId);
        Trail GetTrail(int id);
        bool TrailExist(string name);    
        bool TrailExist(int id);  
        bool CreateTrail(Trail trail);
        bool UpdateTrail(Trail trail);
        bool DeleteTrail(int id);
        bool Save();

    }
}
