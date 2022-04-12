using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChampionsLeagueLibrary
{
    public class PlayersFileSerializerDeserializer
    {
        readonly IPlayersSaveableLoadable players;
        readonly string file;
        public PlayersFileSerializerDeserializer(IPlayersSaveableLoadable players, string file)
        {
            this.players = players;
            this.file = file;
        }
        public void Save()
        {
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                using(StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {
                    foreach (Player player in players.Save())
                    {
                        sw.WriteLine(Serialize(player));
                    }
                    sw.Close();
                }
            }
        }
        public void Load()
        {
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    Player[] loadedPlayers = new Player[0];
                    while (true)
                    {
                        string? s = sr.ReadLine();
                        if (String.IsNullOrEmpty(s))
                        {
                            break;
                        }                        
                        Array.Resize(ref loadedPlayers, loadedPlayers.Length+1);
                        loadedPlayers[loadedPlayers.Length - 1] = Deserialize(s);
                    }
                    players.Load(loadedPlayers);
                    sr.Close();
                }
            }
        }
        private static string Serialize(Player p)
        {
            return $"{p.Name};*;{p.Club};*;{p.GoalCount}";
        }
        private static Player Deserialize(string s)
        {
            string[] parts = s.Split(";*;");

            FootballClub fbKlub = FootballClub.None;
            switch (parts[1])
            {
                case "FCPorto":
                    fbKlub = FootballClub.FCPorto;
                    break;
                case "Arsenal":
                    fbKlub = FootballClub.Arsenal;
                    break;
                case "RealMadrid":
                    fbKlub = FootballClub.RealMadrid;
                    break;
                case "Chelsea":
                    fbKlub = FootballClub.Chelsea;
                    break;
                case "Barcelona":
                    fbKlub = FootballClub.Barcelona;
                    break;
            }

            return new Player(parts[0],fbKlub,int.Parse(parts[2]));
        }

    }
}
