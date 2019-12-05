using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_API
{
    public class ConnectAPI
    {
        public string access_token = "";
        public string user_id = "";

        public ConnectAPI()
        {
        }

        public ConnectAPI(string access_token, string user_id)
        {
            this.access_token = access_token;
            this.user_id = user_id;
        }
    }
}
