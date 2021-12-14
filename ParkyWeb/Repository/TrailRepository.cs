using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System.Net.Http;

namespace ParkyWeb.Repository
{
    
    
        public class TrailRepository : Repository<Trail>, ITrailRepository
        {
            private readonly IHttpClientFactory _clientFactory;
            public TrailRepository(IHttpClientFactory _clientFactory) : base(_clientFactory)
            {
                this._clientFactory = _clientFactory;
            }
        }
    
}
