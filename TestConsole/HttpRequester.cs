using Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{

	public class HttpRequester 
    {
        private readonly HttpClient _client;
		public HttpRequester(string urlForHttpClient)
		{
            _client = new HttpClient();
            _client.BaseAddress = new Uri(urlForHttpClient);
        }
        public WebServiceResponse SendDeleteRequest(Employee employee)
        {
            return SendQueryStringRequestWithMethod(System.Net.Http.HttpMethod.Delete, $"employeeJson={JsonConvert.SerializeObject(employee)}");
        }

        public WebServiceResponse SendUpdateRequest(Employee employee) 
        {
            return SendQueryStringRequestWithMethod(System.Net.Http.HttpMethod.Put, $"employeeJson={JsonConvert.SerializeObject(employee)}");
        }

        public WebServiceResponse SendInsertRequest(Employee employee)
        {
            return SendQueryStringRequestWithMethod(System.Net.Http.HttpMethod.Post, $"employeeJson={JsonConvert.SerializeObject(employee)}");
        }

        public WebServiceResponse SendGetAllRequest()
        {
            return SendQueryStringRequestWithMethod(System.Net.Http.HttpMethod.Get, $"");
        }

        public WebServiceResponse SendGetByConditionsRequest(EmployeeFilter employeeFilter)
        {
            return SendQueryStringRequestWithMethod(System.Net.Http.HttpMethod.Get, $"employeeFilterJson={JsonConvert.SerializeObject(employeeFilter)}");
        }

        private WebServiceResponse SendQueryStringRequestWithMethod(HttpMethod method, string queryString)
        {
            string requestUri = !String.IsNullOrEmpty(queryString) ? _client.BaseAddress + "?" + queryString : _client.BaseAddress.ToString();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, requestUri);

            Task<HttpResponseMessage> sendingTask = _client.SendAsync(httpRequestMessage);
            sendingTask.Wait();
            return new WebServiceResponse(sendingTask.Result);
        }
    }
}
