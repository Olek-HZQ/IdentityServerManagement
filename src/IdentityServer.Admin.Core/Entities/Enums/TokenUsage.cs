namespace IdentityServer.Admin.Core.Entities.Enums
{
    /// <summary>Token usage types.</summary>
    public enum TokenUsage
    {
        /// <summary>Re-use the refresh token handle</summary>
        ReUse,
        /// <summary>Issue a new refresh token handle every time</summary>
        OneTimeOnly,
    }
}
