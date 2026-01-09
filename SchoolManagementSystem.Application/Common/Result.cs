using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace SchoolManagementSystem.Application.Common;
public static class Result
{
    public static IResult<T> Success<T>(T data, int code = StatusCodes.Status200OK)
    {
        return new Result<T>(code, data, Array.Empty<string>(), "");
    }

    public static IResult<T> Success<T>(T data, string notificationMessage, int code = StatusCodes.Status200OK)
    {
        return new Result<T>(code, data, Array.Empty<string>(), notificationMessage);
    }

    public static IResult<T> Fail<T>(int code)
        where T : class
    {
        return new Result<T>(code, default!, Array.Empty<string>(), "");
    }

    public static IResult<T> Fail<T>(int code, IEnumerable<ValidationFailure> failures)
    {
        return new Result<T>(code, default, failures.Select(s => s.ErrorMessage).ToArray(), "");
    }


    public static IResult<object> Fail(int code, IEnumerable<ValidationFailure> failures)
    {
        return new Result<object>(code, default, failures.Select(s => s.ErrorMessage).ToArray(), "");
    }

    public static IResult<object> Fail(int code, params string[] errors)
    {
        return new Result<object>(code, default!, errors, "");
    }

    public static IResult<T> Fail<T>(int code, params string[] errors)
    {
        return new Result<T>(code, default!, errors, "");
    }

    public static IResult<T> Fail<T>(T data = default!)
    {
        return new Result<T>(StatusCodes.Status400BadRequest, data, Array.Empty<string>(), "");
    }

    public static IResult<T> Fail<T>(IEnumerable<ValidationFailure> failures)
    {
        return new Result<T>(StatusCodes.Status400BadRequest, default!, failures.Select(s => s.ErrorMessage).ToArray(), "");
    }

    public static IResult<object> Fail(int code)
    {
        return new Result<object>(code, default!, Array.Empty<string>(), "");
    }

    public static IResult<object> Fail(int code, string notificationMessage)
    {
        return new Result<object>(code, default!, Array.Empty<string>(), notificationMessage);
    }

    public static IResult<object> Fail(int code, string notificationMessage, params string[] errors)
    {
        return new Result<object>(code, default!, errors, notificationMessage);
    }

    public static IResult<object> Fail()
    {
        return new Result<object>(StatusCodes.Status400BadRequest, default!, Array.Empty<string>(), "");
    }

    public static IResult<object> Fail(IEnumerable<ValidationFailure> failures)
    {
        return new Result<object>(StatusCodes.Status400BadRequest, default!, failures.Select(s => s.ErrorMessage).ToArray(), "");
    }

    public static IResult<object> Success()
    {
        return new Result<object>(StatusCodes.Status200OK, default!, Array.Empty<string>(), "");
    }

    public static IResult<object> Success(int code)
    {
        return new Result<object>(code, default!, Array.Empty<string>(), "");
    }
}

public class Result<T> : IResult<T>
{
    public Result(int statusCode, T? data, string[] errors, string notificationMessage)
    {
        StatusCode = statusCode;
        IsSuccess = StatusCode is >= 200 and <= 299;
        Errors = errors;
        Data = data;
        NotificationMessage = notificationMessage;
    }

    public bool IsSuccess { get; }
    public int StatusCode { get; }
    public T? Data { get; }
    public string[] Errors { get; }
    public string NotificationMessage { get; }
}

public interface IResult<out T> : IResult
{
    T? Data { get; }
}

public interface IResult
{
    bool IsSuccess { get; }

    int StatusCode { get; }

    string[] Errors { get; }

    public string NotificationMessage { get; }
}