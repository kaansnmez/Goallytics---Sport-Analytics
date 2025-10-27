namespace GoallyticsApp.UI.Models.Auth
{
    public class RegisterDtoModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? DateOfBirth { get; set; } = DateTime.Now;
        public int AppRoleId { get; set; }
        public int GenderId { get; set; }
    }
}
