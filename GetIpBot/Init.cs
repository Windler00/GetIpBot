using System.Text.Json;

namespace GetIpBot
{
    public class Init
    {
        public static Config config = new Config();
        public static List<UserList> UsersList = new List<UserList>();

        static string dir = Directory.GetCurrentDirectory();
        static string configFile = dir + "\\config.json";
        static string userListFile = dir + "\\userlist.json";

        public static void InitConfig()
        {
            CreateConfig();
            GetConfig();
        }
        public static void InitUsers()
        {
            CreateUsers();
            GetUsers();
        }
        public static void InitBotClient()
        {
            BotClient client = new BotClient();
            client.StartPooling();
        }
        static void CreateConfig()
        {
            Config startConfig = new Config
            {
                ApiToken = "",
            };

            try
            {
                string[] files = Directory.GetFiles(dir);
                if (!files.Contains(configFile))
                {
                    using (var fs = new FileStream(configFile, FileMode.Create))
                    {
                        var options = new JsonSerializerOptions { WriteIndented = true };
                        JsonSerializer.Serialize(fs, startConfig, options);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void GetConfig()
        {
            using (StreamReader r = new StreamReader(configFile))
            {
                string json = r.ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    config = JsonSerializer.Deserialize<Config>(json);
                }
            }
        }
        static void CreateUsers()
        {
            try
            {
                string[] files = Directory.GetFiles(dir);
                if (!files.Contains(userListFile))
                {
                    UserList userList = new UserList();
                    userList.Id = "";
                    UsersList.Add(userList);



                    using (var fs = new FileStream(userListFile, FileMode.Create))
                    {
                        var options = new JsonSerializerOptions { WriteIndented = true };
                        JsonSerializer.Serialize(fs, UsersList, options);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void GetUsers()
        {
            using (StreamReader r = new StreamReader(userListFile))
            {
                string json = r.ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    UsersList = JsonSerializer.Deserialize<List<UserList>>(json);
                }
            }
        }
    }
}