using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using xNet;
using Newtonsoft.Json;

namespace VK_API
{
    public class ConnectAPI
    {
        public string access_token = "";
        public string user_id = "";
        private string _tokenPath = "C:\\Users\\dinar\\Desktop\\Token.txt";
        private const string _vkAPIURL = "https://api.vk.com/method/";

        public bool available { get; set; } = false;

        public ConnectAPI()
        {
        }

        public ConnectAPI(string access_token, string user_id)
        {
            this.access_token = access_token;
            this.user_id = user_id;
            available = true;
        }

        public void SaveToken ()
        {
            FileStream file = new FileStream(_tokenPath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine(access_token);
            writer.WriteLine(user_id);
            writer.Close();
            file.Close();
            available = true;
        }

        public void ReadToken ()
        {
            if (!File.Exists(_tokenPath))
            {
                available = false;
                return;
            }
            FileStream file = new FileStream(_tokenPath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            access_token = reader.ReadLine();
            user_id = reader.ReadLine();

            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("v", "5.52");
            GetInformation.AddUrlParam("access_token", access_token);
            string result = GetInformation.Get(_vkAPIURL + "friends.getOnline").ToString();
            available = true;
            try
            {
                Dictionary<string, string[]> dict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(result);
            }
            catch
            {
                available = false;
            }
            reader.Close();
            file.Close();
        }
        
        public string[] GetListPosts ()
        {
            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("v", "5.52");
            GetInformation.AddUrlParam("access_token", access_token);
            GetInformation.AddUrlParam("owner_id", user_id);
            string result = GetInformation.Get(_vkAPIURL + "wall.get").ToString();
            Dictionary<string, Dictionary<string, object>> dict;
            Post[] posts;
            try
            {
                dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(result);
                posts = JsonConvert.DeserializeObject<Post[]>(dict["response"]["items"].ToString());
            }
            catch
            {
                return new string[0];
            }

            string[] id_posts = new string[posts.Length];
            for (int i = 0; i < posts.Length; i++)
            {
                id_posts[i] = posts[i].id.ToString();
            }
            return id_posts;
        }

        public string CreatePost (string text)
        {
            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("v", "5.52");
            GetInformation.AddUrlParam("access_token", access_token);
            GetInformation.AddUrlParam("owner_id", user_id);
            GetInformation.AddUrlParam("message", text);
            string result = GetInformation.Get(_vkAPIURL + "wall.post").ToString();
            Dictionary<string, Dictionary<string, int>> dict;
            try
            {
                dict = JsonConvert.DeserializeObject< Dictionary<string, Dictionary<string, int>>>(result);
            }
            catch
            {
                return "";
            }
            return dict["response"]["post_id"].ToString();
        }

        public string GetPost(string id_post)
        {
            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("v", "5.52");
            GetInformation.AddUrlParam("access_token", access_token);
            GetInformation.AddUrlParam("posts", user_id + "_" + id_post);
            string result = GetInformation.Get(_vkAPIURL + "wall.getById").ToString();
            Dictionary<string, Post[]> dict;
            try
            {
                dict = JsonConvert.DeserializeObject<Dictionary<string, Post[]>>(result);
            }
            catch
            {
                return "not found";
            }

            return dict["response"][0].text;
        }

        public bool UpdatePost (string id_post, string text)
        {
            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("v", "5.52");
            GetInformation.AddUrlParam("access_token", access_token);
            GetInformation.AddUrlParam("owner_id", user_id);
            GetInformation.AddUrlParam("post_id", id_post);
            GetInformation.AddUrlParam("message", text);
            string result = GetInformation.Get(_vkAPIURL + "wall.edit").ToString();
            Dictionary<string, int> dict;
            try
            {
                dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(result);
            }
            catch
            {
                return false;
            }
            return dict["response"] == 1;
        }

        public bool DeletePost(string id_post)
        {
            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("v", "5.52");
            GetInformation.AddUrlParam("access_token", access_token);
            GetInformation.AddUrlParam("owner_id", user_id);
            GetInformation.AddUrlParam("post_id", id_post);
            string result = GetInformation.Get(_vkAPIURL + "wall.delete").ToString();
            Dictionary<string, int> dict;
            try
            {
                dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(result);
            }
            catch
            {
                return false;
            }
            return dict["response"] == 1;
        }
    }
}