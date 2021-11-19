using Domain.EmployeeObjects;
using Domain.DataProtection.Implementations;
using Domain.DataProtection.Interfaces;
using Domain.EmployeeObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClientApp.RequestSending
{
	public class HttpRequester 
    {
        private readonly HttpClient _client;
        private readonly ITextCrypter _textCrypter = null;

		public HttpRequester(string apiUrl)
		{
            _client = new HttpClient();
            _client.BaseAddress = new Uri(apiUrl);

            _textCrypter = new EmptyCrypter();
        }

        public HttpRequester(string apiUrl, ITextCrypter textCrypter)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(apiUrl);

            _textCrypter = textCrypter;
        }

        public WebServiceResponse SendDeleteRequest(Employee employee)
        {
            string queryStringParameter = _textCrypter.Crypt(employee.Id.ToString());
            return SendQueryStringRequestWithMethod(System.Net.Http.HttpMethod.Delete, $"employeeId={queryStringParameter}");
        }

        public WebServiceResponse SendUpdateRequest(Employee employee) 
        {
            string queryStringParameter = _textCrypter.Crypt(JsonConvert.SerializeObject(employee));
            return SendQueryStringRequestWithMethod(System.Net.Http.HttpMethod.Put, $"employeeJson={queryStringParameter}");
        }

        public WebServiceResponse SendInsertRequest(Employee employee)
        {
            string queryStringParameter = _textCrypter.Crypt(JsonConvert.SerializeObject(employee));
            return SendQueryStringRequestWithMethod(System.Net.Http.HttpMethod.Post, $"employeeJson={queryStringParameter}");
        }

        public WebServiceResponse SendGetAllRequest()
        {
            return SendQueryStringRequestWithMethod(System.Net.Http.HttpMethod.Get, $"");
        }

        public WebServiceResponse SendGetByConditionsRequest(EmployeeFilter employeeFilter)
        {
            string queryStringParameter = _textCrypter.Crypt(JsonConvert.SerializeObject(employeeFilter));
            return SendQueryStringRequestWithMethod(System.Net.Http.HttpMethod.Get, $"employeeFilterJson={queryStringParameter.Replace(" ", "")}");
        }

        private WebServiceResponse SendQueryStringRequestWithMethod(HttpMethod method, string queryString)
        {
            string requestUri = !String.IsNullOrEmpty(queryString) ? _client.BaseAddress + "?" + queryString : _client.BaseAddress.ToString();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, requestUri);

            Task<HttpResponseMessage> sendingTask = _client.SendAsync(httpRequestMessage);
            sendingTask.Wait();

            return new WebServiceResponse(sendingTask.Result, _textCrypter);
        }
    }
}
