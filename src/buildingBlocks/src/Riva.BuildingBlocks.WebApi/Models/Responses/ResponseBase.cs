using System;
using Newtonsoft.Json;

namespace Riva.BuildingBlocks.WebApi.Models.Responses
{
    public abstract class ResponseBase
    {
        [JsonProperty(Order = -2)]
        public Guid Id { get; }

        protected ResponseBase(Guid id)
        {
            Id = id;
        }
    }
}