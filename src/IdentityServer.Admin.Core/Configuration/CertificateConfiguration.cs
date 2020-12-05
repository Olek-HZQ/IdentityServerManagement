namespace IdentityServer.Admin.Core.Configuration
{
    public class CertificateConfiguration
    {
        public bool UseTemporarySigningKeyForDevelopment { get; set; }

        public string TemporaryCertificateFileName { get; set; }

        public bool UseSigningCertificatePfxFile { get; set; }

        public string SigningCertificatePfxFilePath { get; set; }

        public string SigningCertificatePfxFilePassword { get; set; }
    }
}
