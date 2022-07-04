using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesBot.Model;
using Newtonsoft.Json.Linq;
using GamesBot.SomeLists;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types;

namespace GamesBot.Client
{

    public class GameCLient
    {
     
        static string _host = "https://localhost:7204";


        public static List<GamesInform> Result = new List<GamesInform>();
        public static GamesInform result = new GamesInform();
    

        public List<GamesInform> GameList()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:7204/GamesList"),
            };
            var response = client.SendAsync(request);
            var body = response.Result.Content.ReadAsStringAsync();

            Console.WriteLine(body + "one");

            var array = JArray.Parse(body.Result);
            Console.WriteLine(array + "two");

            Result = array.ToObject<List<GamesInform>>();


            Console.WriteLine(Result + "three");
            return Result;
        }

        public GamesInform GameInformationById()
        {
            StreamReader reader = new StreamReader("D:\\KEY.txt");
            string key = reader.ReadToEnd();

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:7204/GameInformationByID"),
                };
            var response = client.SendAsync(request);
            var body = response.Result.Content.ReadAsStringAsync();
            Console.WriteLine(body + "one");

            var details = JObject.Parse(body.Result);
            Console.WriteLine(details + "two");

            result = details.ToObject<GamesInform>();

            Console.WriteLine(result + "three");

                return result;
        }

        public async Task<bool> Postcomment()
        {

            StreamReader _reader = new StreamReader("D:\\Comm.txt");
            string comm = _reader.ReadToEnd();

            using (StreamWriter writer = new StreamWriter("D:\\Comments.txt", true))
            {
                writer.WriteLineAsync(comm);
            }

            return true;

        }
        public string Putcomment()
        {

            StreamReader _reader = new StreamReader("D:\\Comm.txt");
            string comm = _reader.ReadToEnd();

            using (StreamWriter writer = new StreamWriter("D:\\Comments.txt", false))
            {
                writer.WriteLineAsync(comm);
            }

            return comm;
        }

        public async Task<bool> Deletecomments()
        {

            using (StreamWriter writer = new StreamWriter("D:\\Comments.txt", false))
            {
                await writer.WriteLineAsync();
            }
            return true;
        }
    }
}
