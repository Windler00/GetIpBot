using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;

namespace GetIpBot
{
    public class Init
    {
        public static Config config = new Config();

        static string dir = Directory.GetCurrentDirectory();
        static string configFile = dir + "\\config.json";

        public static void InitCfg()
        {
            CreateConfig();
            GetConfig();
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
                        JsonSerializer.Serialize(fs, startConfig);
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
    }
}
