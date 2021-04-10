using System;

namespace BowlingScore.Api.Infrastructure.Exceptions
{
    public class BowlingScoreApiExceptions : Exception
    {
        public BowlingScoreApiExceptions()
        { }

        public BowlingScoreApiExceptions(string message)
            : base(message)
        { }

        public BowlingScoreApiExceptions(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}