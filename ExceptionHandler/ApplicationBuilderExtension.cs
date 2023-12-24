namespace shoppingcart.ExceptionHandler;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder) => applicationBuilder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
}