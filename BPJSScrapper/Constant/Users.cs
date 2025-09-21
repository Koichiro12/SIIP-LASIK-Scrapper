using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPJSScrapper.Constant
{
    class Users
    {
        public String Id { get; set; }
        public string hwid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }

        public Users()
        {

        }
        public Users(String id, string hwid, string username, string password, string role)
        {
            Id = id;
            this.hwid = hwid;
            this.username = username;
            this.password = password;
            this.role = role;
        }
    }
}
