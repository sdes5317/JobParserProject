using System;

namespace JobWebApi.Repository.Entity
{
    public class JobDetail
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string Company { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
    }
}
