using System;

namespace VCRSharp
{
    public class PlaybackException : Exception
    {
        public PlaybackException(string message) : base(message)
        {
        }
    }
}
