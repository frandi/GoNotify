namespace GoNotify
{
    /// <summary>
    /// The SMTP options
    /// </summary>
    public class SmtpOptions : NotificationProviderOptions
    {
        internal static string Parameter_Server = $"{EmailSmtpConstants.DefaultName}_{nameof(Server)}";
        internal static string Parameter_Port = $"{EmailSmtpConstants.DefaultName}_{nameof(Port)}";
        internal static string Parameter_Username = $"{EmailSmtpConstants.DefaultName}_{nameof(Username)}";
        internal static string Parameter_Password = $"{EmailSmtpConstants.DefaultName}_{nameof(Password)}";
        internal static string Parameter_UseSSL = $"{EmailSmtpConstants.DefaultName}_{nameof(UseSSL)}";

        /// <summary>
        /// Instantiate the <see cref="SmtpOptions"/>
        /// </summary>
        public SmtpOptions()
        {
            Parameters.Add(Parameter_Server, "");
            Parameters.Add(Parameter_Port, 0);
            Parameters.Add(Parameter_Username, "");
            Parameters.Add(Parameter_Password, "");
            Parameters.Add(Parameter_UseSSL, false);
        }

        /// <summary>
        /// The SMTP server
        /// </summary>
        public string Server
        {
            get => Parameters[Parameter_Server].ToString();
            set => Parameters[Parameter_Server] = value;
        }

        /// <summary>
        /// The SMTP port
        /// </summary>
        public int Port
        {
            get => int.Parse(Parameters[Parameter_Port].ToString());
            set => Parameters[Parameter_Port] = value;
        }

        /// <summary>
        /// The SMTP username
        /// </summary>
        public string Username
        {
            get => Parameters[Parameter_Username].ToString();
            set => Parameters[Parameter_Username] = value;
        }

        /// <summary>
        /// The SMTP password
        /// </summary>
        public string Password
        {
            get => Parameters[Parameter_Password].ToString();
            set => Parameters[Parameter_Password] = value;
        }

        /// <summary>
        /// The SMTP SSL status
        /// </summary>
        public bool UseSSL
        {
            get => bool.Parse(Parameters[Parameter_UseSSL].ToString());
            set => Parameters[Parameter_UseSSL] = value;
        }
    }
}
