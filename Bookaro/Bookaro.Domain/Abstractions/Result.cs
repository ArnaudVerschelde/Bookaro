using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Bookaro.Domain.Abstractions;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("A result cannot be successful and contain an error");
        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("A result cannot be failure and not contain an error");
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Sucess<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Sucess(value) : Failure<TValue>(Error.NullValue);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    // change for making it work with caching services
    [JsonConstructor]
    public Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull] // tells tools that Value will never be null (at runtime) if accessed
    public TValue Value => IsSuccess
        ? _value! // suppress null warnings
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
    // e.g when doing : Result<string> result = "hello"; this will do same as Result<string>.Create("hello")
}