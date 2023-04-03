namespace TestCsv
{
    using System.Resources;
    using System.Reflection;
    using System.Globalization;
    public static class LanguageHelper
    {
        private static ResourceManager rm;

        //Using the resource manager to access the resources
        //that are used in the application in the form of string massages
        static LanguageHelper( )
        {
            rm = new ResourceManager("TestCsv.Language.strings", Assembly.GetExecutingAssembly());
        }

        //A method to call the strings from the resources by their given name
        public static string? GetString(string name)
        {
            return rm.GetString(name);
        }

        //And a method to change the recource that is used and therefore the language
        public static void ChangeLanguage (string language)
        {
            var cultureInfo = new CultureInfo(language);

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo; 
        }
    }
}
