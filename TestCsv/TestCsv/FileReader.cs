namespace TestCsv
{
    using CsvHelper;
    using System.Globalization;
    using System.Text;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Mime;
    using MathNet.Numerics.Statistics;

    class FileReader
    {
        //Constructor that takes a file path parameter
        public FileReader(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    //Reading the file and storing it's contents in a variable
                    var records = csvReader.GetRecords<Days>().ToList();

                    Console.WriteLine($"{LanguageHelper.GetString("MinTemp")}");

                    //Geting user input for the launch parameters
                    string minTempInput = Console.ReadLine();
                    int inputMinTemp = 0;
                    inputMinTemp = Convert.ToInt32(minTempInput);

                    Console.WriteLine($"{LanguageHelper.GetString("MaxTemp")}");

                    string maxTempInput = Console.ReadLine();
                    int inputMaxTemp = 0;
                    inputMaxTemp = Convert.ToInt32(maxTempInput);

                    Console.WriteLine($"{LanguageHelper.GetString("Wind")}");

                    string windInput = Console.ReadLine();
                    int wind = 0;
                    wind = Convert.ToInt32(windInput);

                    Console.WriteLine($"{LanguageHelper.GetString("Humidity")}");

                    string humidityInput = Console.ReadLine();
                    int humidity = 0;
                    humidity = Convert.ToInt32(humidityInput);

                    Console.WriteLine($"{LanguageHelper.GetString("Lightning")}");

                    string lightning = Console.ReadLine();

                    //Checking if the input is correct
                    while (lightning != "Yes" && lightning != "No")
                    {
                        Console.WriteLine($"{LanguageHelper.GetString("InvalidInput")}");

                        lightning = Console.ReadLine();
                    }

                    //Getting the days in which launch is possible 
                    var days = records.Where(r => r.Temperature >= inputMinTemp
                                               && r.Temperature <= inputMaxTemp
                                               && r.Wind <= wind
                                               && r.Humidity <= humidity
                                               && r.Percipitation == 0
                                               && r.Lightning == lightning
                                               && r.Clouds != "Cumulus"
                                               && r.Clouds != "Nimbus").ToList();

                    StringBuilder csvContent = new StringBuilder();

                    //Here we start creating the Weather Report file
                    csvContent.AppendLine("Day,Temperature,Wind Speed,Humidity,Percipitation,Lightning,Clouds");

                    if (days.Any())
                    {
                        //We get all the average, max, min and median values
                        double avgTemp = days.Any() ? days.Average(d => d.Temperature) : 0;
                        double avgWind = days.Any() ? days.Average(d => d.Wind) : 0;
                        double avgHumidity = days.Any() ? days.Average(d => d.Humidity) : 0;

                        double maxTemp = days.Any() ? days.Max(d => d.Temperature) : 0;
                        double maxWind = days.Any() ? days.Max(d => d.Wind) : 0;
                        double maxHumidity = days.Any() ? days.Max(d => d.Humidity) : 0;

                        double minTemp = days.Any() ? days.Min(d => d.Temperature) : 0;
                        double minWind = days.Any() ? days.Min(d => d.Wind) : 0;
                        double minHumidity = days.Any() ? days.Min(d => d.Humidity) : 0;

                        int bestWindDay = 0;
                        int bestHumidityDay = 0;

                        var tempMedian = GetMedian(days.Select(d => (double)d.Temperature).ToArray());
                        var windMedian = GetMedian(days.Select(d => (double)d.Wind).ToArray());
                        var humidityMedian = GetMedian(days.Select(d => (double)d.Humidity).ToArray());

                        Console.WriteLine($"{LanguageHelper.GetString("GoodDays")}");

                        foreach (var day in days)
                        {
                            Console.WriteLine($" {LanguageHelper.GetString("Day")} #{day.Day}");

                            //The values are added to the file
                            csvContent.AppendLine($"{day.Day},{day.Temperature},{day.Wind},{day.Humidity},{day.Percipitation},{day.Lightning},{day.Clouds}");

                            if (day.Wind == minWind)
                            {
                                bestWindDay = day.Day;
                            }
                            if (day.Humidity == minHumidity)
                            {
                                bestHumidityDay = day.Day;
                            }

                        }

                        csvContent.AppendLine(" ,Avg Temperature, Avg Wind Speed, Avg Humidity");

                        csvContent.AppendLine($" ,{avgTemp},{avgWind},{avgHumidity}");

                        csvContent.AppendLine(" ,Max Temperature, Max Wind Speed, Max Humidity");

                        csvContent.AppendLine($" ,{maxTemp},{maxWind},{maxHumidity}");

                        csvContent.AppendLine(" ,Min Temperature, Min Wind Speed, Min Humidity");

                        csvContent.AppendLine($" ,{minTemp},{minWind},{minHumidity}");

                        csvContent.AppendLine(" ,Med Temperature, Med Wind Speed, Med Humidity");

                        csvContent.AppendLine($" ,{tempMedian},{windMedian},{humidityMedian}");

                        csvContent.AppendLine("Day, , Best Wind Speed");

                        csvContent.AppendLine($"{bestWindDay} , ,{minWind}, ");

                        csvContent.AppendLine("Day, , ,Best Humidity ");

                        csvContent.AppendLine($"{bestHumidityDay} , , ,{minHumidity}");

                        string csvPath = "D:\\WeatherReport.csv";

                        //The file is created
                        File.AppendAllText(csvPath, csvContent.ToString());
                        
                        Console.WriteLine($"{LanguageHelper.GetString("FileCreated")}");
                        
                        //This is the attached file that is later sent with the email
                        Attachment data = new Attachment(csvPath, MediaTypeNames.Application.Octet); ;

                        //Getting the user credentials
                        Console.WriteLine($"{LanguageHelper.GetString("Sender")}");

                        string sender = Console.ReadLine();

                        //Validating the user email input
                        try
                        {
                            var emailAddress = new MailAddress(sender);
                        }
                        catch
                        {
                            Console.WriteLine($"{LanguageHelper.GetString("InvalidEmail")}");
                        }

                        Console.WriteLine($"{LanguageHelper.GetString("Password")}");

                        string password = Console.ReadLine();

                        Console.WriteLine($"{LanguageHelper.GetString("Recipient")}");

                        string recipient = Console.ReadLine();

                        try
                        {
                            var emailAddress = new MailAddress(recipient);
                        }
                        catch
                        {
                            Console.WriteLine($"{LanguageHelper.GetString("InvalidEmail")}");
                        }

                        //Creating the message to be sent as an email
                        MailMessage message = new MailMessage();

                        message.From = new MailAddress(sender);

                        message.Subject = "Test Subject";

                        message.To.Add(new MailAddress(recipient));

                        message.Body = "<html><body> TEST </body></html>";

                        message.IsBodyHtml = true;

                        message.Attachments.Add(data);

                        //Initializing the connection
                        var smtpClient = new SmtpClient("smtp.gmail.com")
                        {
                            Port = 587,
                            Credentials = new NetworkCredential(sender, password),
                            EnableSsl = true,
                        };

                        //Checking if the message has been sent properly if not we dispose of the resources
                        try
                        {
                            smtpClient.Send(message);
                        }
                        catch
                        {
                            smtpClient.Dispose();

                            Console.WriteLine($"{LanguageHelper.GetString("InvalidPassword")}");
                        }
                        

                        Console.WriteLine($"{LanguageHelper.GetString("Sent")}");

                    }
                    else
                    {

                        Console.WriteLine($"{LanguageHelper.GetString("NoGoodDays")}");
                    }
                }

            }

        }

        //A method to calculate the median values
        public static double GetMedian(double[] arr)
        {
            return arr.Median();
        }
    }
}
