using Domain;
using Domain.DataProtection.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmployeeClientApp.RequestSending
{
	/// <summary>
	/// Класс предназначен для хранения информации о результате HTTP запроса.
	/// </summary>
	public class WebServiceResponse
    {
        private readonly HttpResponseMessage _httpResponseMessage;
        private readonly ITextCrypter _textCrypter;

        public readonly CommunicationMessage CommunicationMessage;

        public bool IsSuccessful
        {
            get
            {
                return this._httpResponseMessage.StatusCode == HttpStatusCode.OK && CommunicationMessage.ResponseStatus == ResponseStatus.Success;
            }
        }

        public string ExceptionString 
        {
            get
            {
                if (this._httpResponseMessage.StatusCode == HttpStatusCode.OK)
                { 
                    if (this.CommunicationMessage.ResponseStatus == ResponseStatus.Exception)
                    {
                        return CommunicationMessage.ExceptionMessage;
                    }
                    else if (this.CommunicationMessage.ResponseStatus == ResponseStatus.Fail)
                    {
                        return CommunicationMessage.Content;
                    }
                }
                else if (this._httpResponseMessage.StatusCode != HttpStatusCode.OK)
                    return $"HTTP code: {this._httpResponseMessage.StatusCode}";
                return null;
            }
        }

        public string Content 
        {
            get 
            {
                if (this.CommunicationMessage.ResponseStatus == ResponseStatus.Success)
                    return this.CommunicationMessage.Content;
                else
                    return null;
            }
        }
        public T CastRecievedContentToObject<T>() 
        {
            return JsonConvert.DeserializeObject<T>(Content);
        }

        public ResponseStatus ResponseStatus
        {
            get { return this.CommunicationMessage.ResponseStatus; }
        }

        public WebServiceResponse(HttpResponseMessage httpResponseMessage, ITextCrypter textCrypter) 
        {
            _httpResponseMessage = httpResponseMessage;
            _textCrypter = textCrypter;

            try
			{
                CommunicationMessage = JsonConvert.DeserializeObject<CommunicationMessage>(ReadResponseMessageContent());
            }
			catch (System.Exception ex)
			{
                CommunicationMessage = new CommunicationMessage();
                CommunicationMessage.ResponseStatus = ResponseStatus.Exception;
                CommunicationMessage.ExceptionMessage = ex.Message;
            }
        }

        public string ReadResponseMessageContent() 
        {
            Task<string> resultWainting = _httpResponseMessage.Content.ReadAsStringAsync();
            resultWainting.Wait();
            return _textCrypter == null ? resultWainting.Result : _textCrypter.Decrypt(resultWainting.Result);
        }
    }
}
