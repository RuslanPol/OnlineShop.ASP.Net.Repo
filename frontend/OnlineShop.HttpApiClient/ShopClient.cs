using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using OnlineShop.Domain.Entities;
using OnlineShop.Models.Requests;
using OnlineShop.Models.Responses;

namespace OnlineShop.HttpClient
{
    public class ShopClient : IShopClient
    {
        private const string DefaultHost = "https://api.mysite.com";
        private readonly string _host;
        private readonly System.Net.Http.HttpClient _httpClient;

        public ShopClient( string host = DefaultHost,System.Net.Http.HttpClient? httpClient=null)
        {
            _host = host;
            _httpClient = httpClient ?? new System.Net.Http.HttpClient();
        }


       

        public async Task<IReadOnlyList<Product>> GetProducts(CancellationToken cancellationToken)
        {
            string uri = $"{_host}/Products/get_all";
            IReadOnlyList<Product>? products =
                await _httpClient.GetFromJsonAsync<IReadOnlyList<Product>>(uri, cancellationToken);
            return products!;
        }

        public  async Task<RegisterResponse> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            string uri = $"{_host}/accounts/register";
            var responseMessage = await _httpClient.PostAsJsonAsync(uri, request, cancellationToken);
            if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
            {
                var json = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception(json);
            }

            responseMessage.EnsureSuccessStatusCode();
            var response = await responseMessage.Content.ReadFromJsonAsync<RegisterResponse>(
                cancellationToken:cancellationToken);
            SetAuthToken(response!.Token);
            return response;
        }

        public async Task<LogInResponse> Login(LogInRequest request, CancellationToken cancellationToken=default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            string uri = $"{_host}/accounts/log_in";
            HttpResponseMessage responseMessage =
                await _httpClient.PostAsJsonAsync(uri, request, cancellationToken);
        
            if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
            {
                var json = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpBadRequestExeption(json);
            }
            responseMessage.EnsureSuccessStatusCode();
            var response = await responseMessage.Content.ReadFromJsonAsync<LogInResponse>(
                cancellationToken:cancellationToken);
          
             
            SetAuthToken(response!.Token);
            return response;
        
        }

        public async Task<Product> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            string uri = $"{_host}/products/get_by_id/{id}";
            Product? product = await _httpClient.GetFromJsonAsync<Product>(uri, cancellationToken);
            return product!;
        }


        public async Task UpdateProduct(Guid id, Product product, CancellationToken cancellationToken)
        {
            string uri = $"{_host}/products/update/{id}";
            var responseMassage = await _httpClient.PutAsJsonAsync(uri, product, cancellationToken);
            responseMassage.EnsureSuccessStatusCode();
        }

        public async Task DeleteProduct(Guid id, CancellationToken cancellationToken)
        {
            string uri = $"{_host}/products/delete/{id}";
            var responseMessage = await _httpClient.DeleteAsync(uri, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
        }

        public async Task AddProduct(Product product, CancellationToken cancellationToken)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            string uri = $"{_host}/products/add";
            var responseMessage = await _httpClient.PostAsJsonAsync(uri, product, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
        }
        public void SetAuthToken(string token)
        {
      
            var header = new AuthenticationHeaderValue("Bearer",token  );
            _httpClient.DefaultRequestHeaders.Authorization = header;

        }
    }
  

    public class HttpBadRequestExeption : Exception
    {
        public HttpBadRequestExeption(string json):base(message:json)
        {
            throw new NotImplementedException();
        }
    }
}