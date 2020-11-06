namespace JobWebApi.Repository
{
    public class MySqlRepository
    {
        public JobContext JobContext { get; set; }

        public MySqlRepository(JobContext jobContext)
        {
            JobContext = jobContext;
            jobContext.Database.EnsureCreated();
        }
    }
}
