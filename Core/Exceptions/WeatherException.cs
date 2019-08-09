using System;

namespace Core.Exceptions
{
    public class WeatherException : Exception
    {
        public WeatherException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
