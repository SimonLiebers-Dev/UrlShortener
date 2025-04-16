namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents the possible error types that can occur during user registration.
    /// </summary>
    public enum RegisterErrorType
    {
        /// <summary>
        /// No error occurred. Registration was successful.
        /// </summary>
        None,

        /// <summary>
        /// The registration request was missing an email address or password.
        /// </summary>
        MissingEmailOrPassword,

        /// <summary>
        /// The provided email address is already associated with an existing user.
        /// </summary>
        EmailAlreadyExists
    }
}
