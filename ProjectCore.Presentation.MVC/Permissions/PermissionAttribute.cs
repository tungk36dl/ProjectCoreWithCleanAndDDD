namespace ProjectCore.Presentation.MVC.Permissions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class PermissionAttribute : Attribute
    {
        public string Module { get; }
        public string Action { get; }

        public PermissionAttribute(string module, string action)
        {
            Module = module;
            Action = action;
        }
    }

}
