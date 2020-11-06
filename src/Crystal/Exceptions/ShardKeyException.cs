using System;

namespace Crystal.Exceptions
{
    public class ShardKeyException : ArgumentException
    {
        public ShardKeyException(string message)
            : base(message) { }
    }
}
