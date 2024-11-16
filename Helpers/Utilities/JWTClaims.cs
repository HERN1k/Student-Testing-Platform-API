namespace Helpers.Utilities
{
    public sealed class JWTClaims
    {
        public const string DisplayName = "name";
        public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
        public const string Surname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
        public const string Id = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        public const string Mail = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
    }
}