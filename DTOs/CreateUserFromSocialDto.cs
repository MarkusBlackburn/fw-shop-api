namespace fw_shop_api.DTOs
{
    public class CreateUserFromSocialDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string LoginProviderSubject { get; set; } = string.Empty;
    }
}