using System;

namespace Core.Exceptions
{
    public class LocationException : Exception
    {
        public LocationException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}