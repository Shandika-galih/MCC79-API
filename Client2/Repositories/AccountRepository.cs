using API.DTOs.Account;
using API.Utilities.Enums;
using Client.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace Client.Repositories
{
    public class AccountRepository : GeneralRepository<RegisterAccount, string>, IAccountRepository
    {
        private readonly string request;
        private readonly HttpClient httpClient;

        public AccountRepository(string request = "accounts/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7186/api/")
            };
            this.request = request;
        }

        public async Task<ResponseHandler<AccountRepository>> Register(RegisterAccount entity)
        {
            ResponseHandler<AccountRepository> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "register", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<AccountRepository>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseHandler<string>> Login(LoginDto login)
        {
            ResponseHandler<string> responseHandler = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            using (var response = await httpClient.PostAsync(request + "login", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseHandler = JsonConvert.DeserializeObject<ResponseHandler<string>>(apiResponse);
            }
            return responseHandler;
        }
    }
}