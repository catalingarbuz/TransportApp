using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Endpoints
{
    public static class UserFileEndpoints
    {
        private const long MaxFileSize = 128 * 1024 * 1024; // Set the maximum size for file requests to 128MB.
        public static IEndpointRouteBuilder MapUserFileEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/UserFile")
                           .WithTags("UserFile");

            group.MapGet("GetPage", GetPage)
                .WithName("GetUserFilesPage")
                .Produces<RequestResponse<PagedResponse<UserFileDTO>>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<PagedResponse<UserFileDTO>>>(StatusCodes.Status400BadRequest);

            group.MapPost("Add", Add)
                 .WithName("AddUserFile")
                 .WithMetadata(new RequestFormLimitsAttribute { MultipartBodyLengthLimit = MaxFileSize })
                 .WithMetadata(new RequestSizeLimitAttribute(MaxFileSize))
                 .Produces<RequestResponse>(StatusCodes.Status200OK)
                 .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapGet("Download/{id:guid}", Download)
                 .WithName("DownloadUserFile")
                 .Produces(
                     StatusCodes.Status200OK,
                     contentType: MediaTypeNames.Application.Octet,
                     additionalContentTypes: new[] { MediaTypeNames.Application.Json })
                 .Produces(
                     StatusCodes.Status400BadRequest,
                     contentType: MediaTypeNames.Application.Octet,
                     additionalContentTypes: new[] { MediaTypeNames.Application.Json });

            return app;
        }

        private static async Task<IResult> GetPage([AsParameters] PaginationSearchQueryParams pagination,
            IUserFileService userFileService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await userFileService.GetUserFiles(pagination)) :
                ControllerExtensions.ErrorMessageResult<PagedResponse<UserFileDTO>>(currentUser.Error);
        }

        private static async Task<IResult> Add(UserFileAddDTO form,
            IUserFileService userFileService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await userFileService.SaveFile(form, currentUser.Result)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> Download(Guid id,
            IUserFileService userFileService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            if (currentUser.Result == null)
            {
                return ControllerExtensions.ErrorMessageResult(currentUser.Error);
            }

            var file = await userFileService.GetFileDownload(id);

            return file.Result != null ?
                Results.File(file.Result.Stream, MediaTypeNames.Application.Octet, file.Result.Name) : // The File method of the controller base returns a response from a stream with the given media type and filename.
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }
    }
}
