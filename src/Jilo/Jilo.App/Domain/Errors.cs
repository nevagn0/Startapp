using ErrorOr;

namespace Jilo.App.Domain;

public static class Errors
{
    public static class Email
    {
        public static Error AddressIsNullOrEmpty => Error.Validation(
            code: "Email.AddressNullOrEmpty",
            description: "Provided address is null or empty");

        public static Error InvalidLength(int minLength, int maxLength) => Error.Validation(
            code: "Email.InvalidLength",
            description: $"Address length must be from {minLength} to {maxLength}");

        public static Error InvalidFormat => Error.Validation(
            code: "Email.InvalidFormat",
            description: "Invalid email address format");
    }

    public static class Username
    {
        public static Error NameIsNullOrEmpty => Error.Validation(
            code: "Username.NameIsNullOrEmpty",
            description: "Provided name is null or empty");

        public static Error InvalidLength(int minLength, int maxLength) => Error.Validation(
            code: "Username.InvalidLength",
            description: $"Name length must be from {minLength} to {maxLength}");

        public static Error ContainsNotAllowedSymbols(string notAllowedSymbols) => Error.Validation(
            code: "Username.ContainsNotAllowedSymbols",
            description: $"Provided name contains not allowed symbols. Not allowed symbols are: {notAllowedSymbols}");
    }

    public static class User
    {
        public static Error MissingPassword => Error.Validation(
            code: "User.MissingPassword",
            description: "Missing password for user");

        public static Error EmailAlreadyExists => Error.Conflict(
            code: "User.EmailAlreadyExists",
            description: "Email already exists");

        public static Error UsernameAlreadyExists => Error.Conflict(
            code: "User.UsernameAlreadyExists",
            description: "Username already exists");

        public static Error EmailAndUsernameAlreadyExist => Error.Conflict(
            code: "User.EmailAndUsernameAlreadyExists",
            description: "Email and username already exist");

        public static Error NotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User not found");
    }

    public static class Auth
    {
        public static Error Unauthorized => Error.Unauthorized(
            code: "Auth.Unauthorized",
            description: "Unauthorized");
    }

    public static class RefreshToken
    {
        public static Error TokenIsNullOrEmpty => Error.Validation(
            code: "RefreshToken.TokenIsNullOrEmpty",
            description: "Token value is null or empty");

        public static Error NotFound => Error.NotFound(
            code: "RefreshToken.NotFound",
            description: "Refresh token not found");
    }
}
