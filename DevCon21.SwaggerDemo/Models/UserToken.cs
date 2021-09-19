using System;
using System.Collections.Generic;

namespace DevCon21.SwaggerDemo.Models
{
    public class UserToken
    {
        public string Token { get; set; }
        public string Login { get; set; }
        public DateTime ExpiresOn { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
