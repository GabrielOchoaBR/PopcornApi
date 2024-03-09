namespace Infrastructure.Context
{
    public class AppDbContextConfiguration
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
    }
}
