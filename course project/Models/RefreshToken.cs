namespace course_project.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
