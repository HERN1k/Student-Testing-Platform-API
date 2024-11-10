#pragma warning disable CS8618

namespace Domain.Settings
{
    public record AzureAd
    {
        public AzureAd() { }

        public AzureAd(
                string authorityInstance,
                string issuerInstance,
                string client,
                string tenantId
            )
        {
            ArgumentException.ThrowIfNullOrEmpty(authorityInstance);
            ArgumentException.ThrowIfNullOrWhiteSpace(authorityInstance);

            ArgumentException.ThrowIfNullOrEmpty(issuerInstance);
            ArgumentException.ThrowIfNullOrWhiteSpace(issuerInstance);

            ArgumentException.ThrowIfNullOrEmpty(client);
            ArgumentException.ThrowIfNullOrWhiteSpace(client);

            ArgumentException.ThrowIfNullOrEmpty(tenantId);
            ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);

            AuthorityInstance = authorityInstance;
            IssuerInstance = issuerInstance;
            Client = client;
            TenantId = tenantId;
        }

        public string AuthorityInstance { get; set; }

        public string IssuerInstance { get; set; }

        public string Client { get; set; }

        public string TenantId { get; set; }
    }
}