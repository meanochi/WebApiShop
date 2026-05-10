using Microsoft.AspNetCore.Authorization;

public class ManagerOnlyAttribute : AuthorizeAttribute
{
    public ManagerOnlyAttribute() : base()
    {
        Roles = "Manager";
    }
}