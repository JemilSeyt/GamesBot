using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GamesBot.Model;


namespace GamesBot.SomeLists
{
    public class Lists
    {
        public static List<GamesInform> GameBaseList = new List<GamesInform>();
        public static List<GamesInform> Comments = new List<GamesInform>();
    }
}
