using Hangfire.Dashboard;

namespace Izumi.Framework.Hangfire
{
    public class AllowAllAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
