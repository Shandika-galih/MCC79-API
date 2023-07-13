using API.Utilities.Enums;
using Client.Contracts;
using Newtonsoft.Json;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Client.Repositories
{
    public class GeneralRepository<Entity, TId> : IRepository<Entity, TId>
        where Entity : class
    {
        private readonly string request;
        private readonly HttpClient httpClient;

        public GeneralRepository(string request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7186/api/")
                
            };
            this.request = request;
        }

        public async Task<ResponseHandler<Entity>> Delete(TId id)
        {
            ResponseHandler<Entity> entityVM = null;
            using (var response = await httpClient.DeleteAsync($"{request}/{id}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<Entity>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseHandler<IEnumerable<Entity>>> Get()
        {
            ResponseHandler<IEnumerable<Entity>> entityVM = null;
            using (var response = await httpClient.GetAsync(request))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<IEnumerable<Entity>>>(apiResponse);
            }
            return entityVM;
        }

        public Task<ResponseHandler<Entity>> Get(TId id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseHandler<Entity>> Post(Entity entity)
        {
            ResponseHandler<Entity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<Entity>>(apiResponse);
            }
            return entityVM;
        }

        public Task<ResponseHandler<Entity>> Put(TId id, Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
