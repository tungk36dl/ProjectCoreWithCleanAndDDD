namespace ProjectCore.Presentation.MVC.Models.Users
{
    public class UserQueryViewModel
    {
        public string? Keyword { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public Guid? RoleId { get; set; }

        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int Page { get; set; } = 1;
    }

}
