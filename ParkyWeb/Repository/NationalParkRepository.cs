using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System.Net.Http;

namespace ParkyWeb.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public NationalParkRepository(IHttpClientFactory _clientFactory) : base(_clientFactory)
        {
            this._clientFactory = _clientFactory;
        }
    }
}
