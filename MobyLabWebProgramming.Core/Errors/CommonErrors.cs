using System.Net;

namespace MobyLabWebProgramming.Core.Errors;

/// <summary>
/// Common error messages that may be reused in various places in the code.
/// </summary>
public static class CommonErrors
{
    public static ErrorMessage UserNotFound => new(HttpStatusCode.NotFound, "User doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage DriverNotFound => new(HttpStatusCode.NotFound, "Driver doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage DriverInfoNotFound => new(HttpStatusCode.NotFound, "Driver Info doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage CarNotFound => new(HttpStatusCode.NotFound, "Car doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage RouteNotFound => new(HttpStatusCode.NotFound, "Route doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage FileNotFound => new(HttpStatusCode.NotFound, "File not found on disk!", ErrorCodes.PhysicalFileNotFound);
    public static ErrorMessage TechnicalSupport => new(HttpStatusCode.InternalServerError, "An unknown error occurred, contact the technical support!", ErrorCodes.TechnicalError);
}
