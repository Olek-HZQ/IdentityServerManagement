namespace IdentityServer.Admin.Models
{
    public class DeleteUserClaimModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}
