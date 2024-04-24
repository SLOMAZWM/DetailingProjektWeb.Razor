public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userRole = context.Session.GetString("UserType");

        if (context.Request.Path.StartsWithSegments("/ClientUser") && userRole != "Client")
        {
            context.Response.Redirect("/LoginRegister");
            return;
        }

        if (context.Request.Path.StartsWithSegments("/EmployeeUser") && userRole != "Employee")
        {
            context.Response.Redirect("/LoginRegister");
            return;
        }

        await _next(context);
    }
}
