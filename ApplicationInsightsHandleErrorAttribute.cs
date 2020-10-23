using System;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;

namespace DatelAPI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ApplicationInsightsHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (ShouldTrackException(filterContext))
            {
                var telemetryClient = new TelemetryClient();
                telemetryClient.TrackException(filterContext.Exception);
            }

            base.OnException(filterContext);
        }

        private static bool ShouldTrackException(ExceptionContext filterContext)
        {
            return filterContext?.HttpContext.IsCustomErrorEnabled == true && filterContext.Exception != null;
        }
    }
}