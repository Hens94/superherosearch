namespace SuperHeroSearch_Common.Configurations
{
    public class ApiConfig
    {
        public string BaseUrl { get; set; }
        public string AccessToken { get; set; }
        public bool WriteInLogs { get; set; } = false;
        public Endpoints Endpoints { get; set; }
    }

    public class Endpoints
    {
        public string Search { get; set; }
        public string Info { get; set; }
    }
}