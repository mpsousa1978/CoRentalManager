using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSWPFDesktopUI.Library.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        public string Toekn { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }


        public void LogOffUser()
        {
            Toekn = null;
            Id = null;
            FirstName   = null;
            LastName = null;
            EmailAddress = null;
            CreatedDate = DateTime.MinValue;
        }
    }
}
