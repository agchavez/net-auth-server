using Newtonsoft.Json;

namespace AUTHSERVER.utils
{
    public interface IErrorService
    {
        ApplicationException GetBadRequestException(string message, int errorCode);
        ApplicationException GetNotFoundException(string message, int errorCode);
        ApplicationException GetConflictException(string message, int errorCode);

        ApplicationException GetInternalServerException (string message, int errorCode);
    }



    public class ErrorService : IErrorService
    {
        public ApplicationException GetBadRequestException(string message, int errorCode)
        {
            return new ApplicationException(JsonConvert.SerializeObject(new
            {
                message = message,
                status_code = StatusCodes.Status400BadRequest,
                error_code = errorCode
            }));
        }

        public ApplicationException GetNotFoundException(string message, int errorCode)
        {
            return new ApplicationException(JsonConvert.SerializeObject(new
            {
                message = message,
                status_code = StatusCodes.Status404NotFound,
                error_code = errorCode
            }));
        }

        public ApplicationException GetConflictException(string message, int errorCode)
        {
            return new ApplicationException(JsonConvert.SerializeObject(new
            {
                message = message,
                status_code = StatusCodes.Status409Conflict,
                error_code = errorCode
            }));
        }

        public ApplicationException GetInternalServerException(string message, int errorCode)
        {
            return new ApplicationException(JsonConvert.SerializeObject(new
            {
                message = message,
                status_code = StatusCodes.Status500InternalServerError,
                error_code = errorCode
            }));
        }
    }
}
