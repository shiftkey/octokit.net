using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VCRSharp
{
    public class Body
    {
        [JsonProperty("encoding")]
        public string Encoding { get; set; }
        [JsonProperty("Base64_string")]
        public string Base64String { get; set; }
    }

    public class CachedRequest
    {
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("uri")]
        public Uri Uri { get; set; }
        [JsonProperty("body")]
        public Body Body { get; set; }
        [JsonProperty("headers")]
        public Dictionary<string, string[]> Headers { get; set; }
    }

    public class Status
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class CachedResponse
    {
        [JsonProperty("status")]
        public Status Status { get; set; }
        [JsonProperty("headers")]
        public Dictionary<string, string[]> Headers { get; set; }
        [JsonProperty("body")]
        public Body Body { get; set; }
    }

    public class CachedRequestResponse
    {
        [JsonProperty("request")]
        public CachedRequest Request { get; set; }
        [JsonProperty("response")]
        public CachedResponse Response { get; set; }
    }

    public class CachedRequestResponseArray
    {
        [JsonProperty("http_interactions")]
        public CachedRequestResponse[] HttpInteractions { get; set; }
    }
}
