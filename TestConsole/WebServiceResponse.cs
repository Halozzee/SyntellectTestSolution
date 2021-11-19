using Domain.DataProtection;
using Newtonsoft.Json;
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
        private readonly HttpResponseMessage _httpResponseMessage;

        private readonly ITextCrypter _textCrypter;

        public WebServiceResponse(HttpResponseMessage httpResponseMessage, ITextCrypter textCrypter) 
        {
            _httpResponseMessage = httpResponseMessage;
            _textCrypter = textCrypter;
        }

        public string ReadResponseMessageContent() 
        {
            Task<string> resultWainting = _httpResponseMessage.Content.ReadAsStringAsync();
            resultWainting.Wait();
            return _textCrypter == null ? resultWainting.Result : _textCrypter.Decrypt(resultWainting.Result);
        }

        public T ParseResponseMessageContentToObject<T>() 
        {
            return JsonConvert.DeserializeObject<T>(ReadResponseMessageContent());
        }
    }
}
