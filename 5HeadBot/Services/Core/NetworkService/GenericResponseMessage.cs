using System;
using System.Net.Http;

namespace _5HeadBot.Services.Core.NetworkService
{
    /// <summary>
    /// Contains default <see cref="HttpResponseMessage"/> and specific <typeparamref name="T"/> <see cref="DesirializedContent"/>
    /// </summary>
    /// <typeparam name="T">Type of desirialized object</typeparam>
    public class GenericResponseMessage<T>
    {
        public HttpResponseMessage HttpResponseMessage { get; }
        public T DesirializedContent { get; }
        public Exception Exception { get; }

        public GenericResponseMessage(HttpResponseMessage resp, T desirializedContent, Exception ex)
        {
            HttpResponseMessage = resp;
            DesirializedContent = desirializedContent;
            Exception = ex;
        }
    }
}
