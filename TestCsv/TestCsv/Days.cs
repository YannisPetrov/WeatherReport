namespace TestCsv
{
    using CsvHelper.Configuration.Attributes;
    class Days
    {
        [Name("Day/Parameter")]
        public int Day { get; set; }

        [Name("Temperature")]
        public int Temperature { get; set; }
        [Name("Wind")]
        public int Wind { get; set; }
        [Name("Humidity")]
        public int Humidity { get; set; }
        [Name("Percipitation")]
        public int Percipitation { get; set; }
        [Name("Lightning")]
        public string Lightning { get; set; }
        [Name("Clouds")]
        public string Clouds { get; set; }
    }
}
