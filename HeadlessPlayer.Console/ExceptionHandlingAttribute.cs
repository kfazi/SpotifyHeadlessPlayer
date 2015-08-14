namespace HeadlessPlayer.Console
{
    using System.Web.Http.Filters;

    using NLog;

    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            Log.Error("Unhandled Exception", exception);

            base.OnException(actionExecutedContext);
        }
    }
}