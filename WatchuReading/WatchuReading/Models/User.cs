using System;

namespace WatchuReading.Models
{
    public class User : IBusinessEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime AddDate { get; set; }
        public bool ShowMenu { get; set; } = false;
    }
}
