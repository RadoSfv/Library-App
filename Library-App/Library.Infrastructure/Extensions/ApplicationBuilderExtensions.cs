using Microsoft.AspNetCore.Builder;
using Library.Infrastructure.CustomMiddlewares;

namespace Library.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedRoles(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedRolesMiddleware>();
        }
    }
}
