namespace ProjectCore.Presentation.MVC.Models.Roles
{
    public class RoleQueryViewModel
    {
        public string? Keyword { get; set; }
        public string? Name { get; set; }

        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int Page { get; set; } = 1;
    }
}

