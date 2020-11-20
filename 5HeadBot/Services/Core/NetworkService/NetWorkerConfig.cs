using _5HeadBot.Services.Core.NetworkService.Deserializers;
using System;

namespace _5HeadBot.Services.Core.NetworkService
{
    public class NetWorkerConfig
    {
        private IDeserializer defaultDeserializer;

        /// <summary>
        /// Default deserializer to be used by <see cref="NetWorker"/>
        /// </summary>
        public IDeserializer DefaultDeserializer
        {
            get
            {
                if (defaultDeserializer is null)
                    throw new ArgumentNullException(nameof(defaultDeserializer));

                return defaultDeserializer;
            }

            set => defaultDeserializer = value;
        }
    }
}