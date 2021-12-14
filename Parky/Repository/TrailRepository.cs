using Microsoft.EntityFrameworkCore;
using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyApi.Repository
{
    public class TrailRepo : ITrailRepository
    {
        private readonly ApplicationDbContext _db;
        public TrailRepo(ApplicationDbContext _db)
        {
            this._db = _db;
        }
        public bool CreateTrail(Trail trail)
        {
            _db.Trail.Add(trail);
            return Save();
        }

        public  bool DeleteTrail(int id)
        {
           var trail = _db.Trail.Find(id);
            _db.Trail.Remove(trail);
            return Save();
        }

        public ICollection<Trail> GetTrails()
        {
            var trail = _db.Trail.Include(a => a.NationalPark).OrderBy(q=>q.Name).ToList();
            return trail;
        }

        public Trail GetTrail(int id)
        {
           var trail = _db.Trail.Include(a => a.NationalPark).FirstOrDefault(a=>a.Id==id);
            return trail;
        }

        public bool TrailExist(string name)
        {
            bool value = _db.Trail.Any(q => q.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExist(int id)
        {
            bool value = _db.Trail.Any(a => a.Id == id);
            return value;   
        }

        public bool Save()
        {
            var value = _db.SaveChanges() >= 0 ? true : false;
            return value;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trail.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _db.Trail.Include(a=>a.NationalPark).Where(i=>i.NationalParkId==npId).ToList();
        }
    }
}
