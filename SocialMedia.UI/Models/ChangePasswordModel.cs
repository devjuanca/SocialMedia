namespace SocialMedia.UI.Models
{
    public class ChangePasswordModel
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPasword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
