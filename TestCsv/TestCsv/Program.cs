namespace TestCsv
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose a language!");

            Console.WriteLine("For English type E, for German type G :");

            //Waits for user input
            string lang = Console.ReadLine();

            //Validates the user input
            while(lang != "E" && lang != "e" && lang != "G" && lang != "g")
            {
                Console.WriteLine("Type the correct character !");

                lang = Console.ReadLine();
            }

            //Checks the input and switches the language
            if (lang == "G" || lang == "g")
            {
                LanguageHelper.ChangeLanguage("de");
            }

            //Calls a string response
            Console.WriteLine($"{LanguageHelper.GetString("File")}");

            //Catches exceptions on user input for the file location and returns a response
            try
            {
                string path = Console.ReadLine();

                FileReader fileReader = new FileReader(path);

            }
            //Exception for wrong path or missing file
            catch(FileNotFoundException e)
            {
                Console.WriteLine($"{LanguageHelper.GetString("FileNotFound")}");

                return;
            }
            //Exception for empty input
            catch (ArgumentException e)
            {

                Console.WriteLine($"{LanguageHelper.GetString("EmptyInput")}");

                return;
            }

            Console.ReadKey();
        }

        

    }
}
