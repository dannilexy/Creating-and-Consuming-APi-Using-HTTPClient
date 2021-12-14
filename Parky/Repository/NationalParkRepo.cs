using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyApi.Repository
{
    public class NationalParkRepo : INationalPark
    {
        private readonly ApplicationDbContext _db;
        public NationalParkRepo(ApplicationDbContext _db)
        {
            this._db = _db;
        }
        public  bool CreateNationalPark(NationalPark nationalPark)
        {
            _db.NationalPark.AddAsync(nationalPark);
            return Save();
        }

        public  bool DeleteNationalPark(int id)
        {
           var nationPark = _db.NationalPark.Find(id);
            _db.NationalPark.Remove(nationPark);
            return Save();
        }

        public ICollection<NationalPark> GetNamtionalParks()
        {
            var nationalParks = _db.NationalPark.OrderBy(q=>q.Name).ToList();
            return nationalParks;
        }

        public NationalPark GetNationalPark(int id)
        {
           var nationalPark = _db.NationalPark.Find(id);
            return nationalPark;
        }

        public bool NationalParkExist(string name)
        {
            bool value = _db.NationalPark.Any(q => q.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExist(int id)
        {
            bool value = _db.NationalPark.Any(a => a.Id == id);
            return value;   
        }

        public bool Save()
        {
            var value = _db.SaveChanges() >= 0 ? true : false;
            return value;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _db.NationalPark.Update(nationalPark);
            return Save();
        }
    }
}
