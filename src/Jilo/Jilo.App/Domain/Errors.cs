using ErrorOr;

namespace Jilo.App.Domain;

public static class Errors
{
    public static class Invitation
    {
        public static Error AlreadyProcessed => Error.Failure(
            code: "Invitation.AlreadyProcessed",
            description: "Invitaion is already processed");

        public static Error Expired => Error.Failure(
            code: "Invitation.Expired",
            description: "Invitation expired");

        public static Error NotAReciever => Error.Forbidden(
            code: "Invitation.NotAReciever",
            description: "User it not the reciever of the invitation");

        public static Error CannotCancel => Error.Failure(
            code: "Invitation.CannotCancel",
            description: "Invitation cannot be canceled");

        public static Error NotAnOwner => Error.Forbidden(
            code: "Invitation.NotAnOwner",
            description: "User is not an owner of the invitation");

        public static Error NotFound => Error.NotFound(
            code: "Invitation.NotFound",
            description: "Invitation not found");
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

    public static class Profile
    {
        public static Error NotFound => Error.NotFound(
            code: "Profile.NotFound",
            description: "Profile not found");
    }

    public static class Lobby
    {
        public static Error PendingInvitation => Error.Conflict(
            code: "Lobby.PendingInvitation",
            description: "User already invited to this lobby");

        public static Error Full => Error.Failure(
            code: "Lobby.Full",
            description: "Lobby is already full");

        public static Error AlreadyInLobby => Error.Conflict(
            code: "Lobby.AlreadyInLobby",
            description: "User is already in lobby");

        public static Error NotFound => Error.NotFound(
            code: "Lobby.NotFound",
            description: "Lobby not found");

        public static Error NotAMemberOfLobby => Error.Failure(
            code: "Lobby.NotAPartOfLobby",
            description: "User is not a member of the lobby");
    }

    public static class Game
    {
        public static Error NotFound => Error.NotFound(
            code: "Game.NotFound",
            description: "Game not found");
    }

    public static class LobbyMember
    {
        public static Error NotFound => Error.NotFound(
            code: "LobbyMember.NotFound",
            description: "Lobby member not found");
    }
}
