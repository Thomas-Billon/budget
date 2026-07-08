namespace Budget.Server.Middleware.Configuration
{
    public static class ConfigurationManagerExtension
    {
        public static T GetRequiredValue<T>(this ConfigurationManager configuration, string key)
        {
            var value = configuration.GetValue<T>(key);

            if (value == null)
            {
                throw new Exception($"Error: Missing {key} configuration");
            }

            return value;
        }

        public static T GetRequiredSection<T>(this ConfigurationManager configuration, string sectionKey)
            where T : class
        {
            var section = configuration.GetSection(sectionKey).Get<T>();

            if (section == null)
            {
                throw new Exception($"Error: Missing {sectionKey} configuration");
            }

            return section;
        }
    }
}
