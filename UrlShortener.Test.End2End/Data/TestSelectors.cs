namespace UrlShortener.Test.End2End.Data
{
    internal static class TestSelectors
    {
        /// <summary>
        /// Login form on login page
        /// </summary>
        public const string LoginFormSelector = "#login-form";

        /// <summary>
        /// Register form on register page
        /// </summary>
        public const string RegisterFormSelector = "#register-form";

        /// <summary>
        /// Login button on login or register page
        /// </summary>
        public const string LoginBtnSelector = "#login-btn";

        /// <summary>
        /// Register button on login or register page
        /// </summary>
        public const string RegisterBtnSelector = "#register-btn";

        /// <summary>
        /// Loading indicator wrapping everything on home/mappings page
        /// </summary>
        public const string MappingsLoadingIndicatorSelector = "#mappings-loading-indicator";

        /// <summary>
        /// Input for name to create a mapping
        /// </summary>
        public const string CreateMappingNameInputSelector = "#mapping-name-input";

        /// <summary>
        /// Input for long url to create a mapping
        /// </summary>
        public const string CreateMappingUrlInputSelector = "#mapping-url-input";

        /// <summary>
        /// Wrapper containing all active mappings
        /// </summary>
        public const string MappingsWrapperSelector = "#mappings-wrapper";

        /// <summary>
        /// Cards containing information about active mappings
        /// </summary>
        public const string MappingCardSelector = ".mapping-card";

        /// <summary>
        /// Modal displaying details about a mapping
        /// </summary>
        public const string MappingDetailsModalSelector = "#mapping-details-modal";

        /// <summary>
        /// Button inside mapping card to show details
        /// </summary>
        public const string MappingDetailsBtnSelector = ".mapping-details-btn";

        /// <summary>
        /// Button inside mapping card to delete mapping
        /// </summary>
        public const string MappingDeleteBtnSelector = ".mapping-delete-btn";

        /// <summary>
        /// Button to create new mapping
        /// </summary>
        public const string MappingCreateBtnSelector = "#create-mapping-btn";

        /// <summary>
        /// Button inside top menu to log out
        /// </summary>
        public const string TopMenuLogoutBtnSelector = "#logout-btn";

        /// <summary>
        /// Expected title on login page
        /// </summary>
        public const string LoginExpectedTitle = "Login";

        /// <summary>
        /// Login email input
        /// </summary>
        public const string LoginEmailInput = "#login-email-input";

        /// <summary>
        /// Login password input
        /// </summary>
        public const string LoginPasswordInput = "#login-password-input";

        /// <summary>
        /// Register email input
        /// </summary>
        public const string RegisterEmailInput = "#register-email-input";

        /// <summary>
        /// Register first password input
        /// </summary>
        public const string RegisterPassword1Input = "#register-password-input-1";

        /// <summary>
        /// Register second password input
        /// </summary>
        public const string RegisterPassword2Input = "#register-password-input-2";

        /// <summary>
        /// Expected title on register page
        /// </summary>
        public const string RegisterExpectedTitle = "Register";

        /// <summary>
        /// Expected title on home/mappings page
        /// </summary>
        public const string MappingsExpectedTitle = "UrlShortener";
    }
}
