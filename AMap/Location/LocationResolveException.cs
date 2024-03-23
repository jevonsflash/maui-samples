using System;
using System.Runtime.Serialization;

namespace AMap.Location
{
    public class LocationResolveException : Exception
    {
        public LocationResolveException()
        {
        }

        public LocationResolveException(string message)
            : base(message)
        {
        }

        public LocationResolveException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public LocationResolveException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
