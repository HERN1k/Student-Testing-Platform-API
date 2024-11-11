using System.Globalization;

using Microsoft.AspNetCore.Http;

namespace Domain.DTOs
{
    public sealed class Response
    {
        public abstract class ResponseBase<TObject> where TObject : class
        {
            public string Status { get; init; }

            public string? Message { get; init; }

            public TObject? Data { get; init; }

            private static readonly string _statusCodeOK = StatusCodes.Status200OK.ToString();
            private static readonly string _statusCodeInternalServerError = StatusCodes.Status500InternalServerError.ToString();

            public ResponseBase()
            {
                Status = _statusCodeOK;
            }

            public ResponseBase(TObject data)
            {
                Status = _statusCodeOK;
                Data = data ?? default;
            }

            public ResponseBase(string status, string? message = null)
            {
                Status = status ?? _statusCodeInternalServerError;
                Message = message;
            }
        }

        public sealed class Error(string status, string message) : ResponseBase<object>(status, message) { }

        public sealed class Success : ResponseBase<object> { }

        public sealed class Time(DTO.TimeResponse data) : ResponseBase<DTO.TimeResponse>(data) { }
    }

    public sealed class Request
    {
        public sealed record Authentication(
            string? Id,
            string? DisplayName,
            string? Name,
            string? Surname,
            string? Mail
        );
    }

    public sealed class DTO
    {
        public sealed class TimeResponse
        {
            public string Time { get; init; }

            public TimeResponse()
            {
                Time = DateTime.UnixEpoch.ToString(null, CultureInfo.InvariantCulture);
            }

            public TimeResponse(string time)
            {
                if (string.IsNullOrEmpty(time) || string.IsNullOrWhiteSpace(time))
                {
                    Time = time ?? DateTime.UnixEpoch.ToString(null, CultureInfo.InvariantCulture);
                }
                else
                {
                    Time = time;
                }
            }

            public TimeResponse(TimeSpan time)
            {
                Time = time.ToString(null, CultureInfo.InvariantCulture);
            }

            public TimeResponse(DateTime time)
            {
                Time = time.ToString(null, CultureInfo.InvariantCulture);
            }
        }

        public sealed record AddOrUpdateUserInDB(
                string? Id,
                string? DisplayName,
                string? Name,
                string? Surname,
                string? Mail
            );
    }
}