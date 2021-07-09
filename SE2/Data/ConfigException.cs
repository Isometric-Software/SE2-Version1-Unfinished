using System;

namespace SE2.Data
{
    public class ConfigException : Exception
    {
        public ConfigException() { }

        public ConfigException(string message) : base("A required configuration entry was not found or was null.\n" + message) { }

        public ConfigException(string message, Exception inner) : base("A required configuration entry was not found or was null.\n" + message, inner) { }
    }
}