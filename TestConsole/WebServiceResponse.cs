using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestConsole
{
	/// <summary>
	/// Класс предназначен для хранения информации о результате HTTP запроса.
	/// </summary>
	public class WebServiceResponse
    {
        /// <summary>
        /// HTTP код ответа
        /// </summary>
        public HttpStatusCode StatusCode => this._httpResponseMessage.StatusCode;

        /// <summary>
        /// Результат выполнения HTTP запроса
        /// </summary>
        private HttpResponseMessage _httpResponseMessage;

        public WebServiceResponse(HttpResponseMessage httpResponseMessage) 
        {
            _httpResponseMessage = httpResponseMessage;
        }

        public string ReadResponseMessageContent() 
        {
            Task<string> resultWainting = _httpResponseMessage.Content.ReadAsStringAsync();
            resultWainting.Wait();
            return resultWainting.Result;
        }
    }
}
