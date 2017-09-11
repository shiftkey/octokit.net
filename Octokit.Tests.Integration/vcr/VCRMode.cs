namespace VCRSharp
{
    public enum VCRMode
    {
        /// <summary>
        /// Only use the local fixtures when executing network requests
        /// </summary>
        Playback,
        /// <summary>
        /// Use the cached response if found, otherwise fetch and store the result
        /// </summary>
        Cache,
        /// <summary>
        /// Avoid cached responses - use the network and store the result
        /// </summary>
        Record
    }
}
