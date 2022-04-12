﻿using System.Collections;

namespace ChampionsLeagueLibrary;

// TODO: Vytvořte třídu PlayersCountChangedEventArgs (dědící z EventArgs)
// - vlastnost int OldCount
// - vlastnost int NewCount
public class PlayersCountChangedEventArgs : EventArgs
{
    public int OldCount { get; init; }
    public int NewCount { get; init; }
}
// TODO: Vytvořte delegát pro událost PlayersCountChangedEventHandler (využijte výše definované event args)
public delegate void PlayersCountChangedEventHandler(object sender, PlayersCountChangedEventArgs e);

// TODO: Vytvořte třídu PlayersRecords
public class PlayersRecords : IPlayersSaveableLoadable, IEnumerable
{
    // TODO: Vytvořte atribut players typu Player[]
    public ObjectLinkedList players;
    // TODO: Vytvořte vlastnost Count (pouze pro čtení), která vždy vrací aktuální velikost pole players
    public int Count { get { return players.Count; } }
    // TODO: Vytvořte událost PlayersCountChanged (PlayersCountChangedEventHandler)
    public event PlayersCountChangedEventHandler? PlayersCountChanged;
    // TODO: vytvořte indexer [int index], který umožňuje získat Player? z pole (pouze operace get)
    // - get - pokud je index neplatný, je vráceno null; jinak je vrácen objekt z pole
    public Player? this[int index]
    {
        get
        {
            if (index < 0 || index >= players.Count)
            {
                return null;
            }
            return (Player)players[index];
        }
    }

    // TODO: Vytvořte bezparametrický konstruktor
    // - inicializujte pole players na pole délky 0
    public PlayersRecords()
    {
        players = new ObjectLinkedList();
    }

    // TODO: Vytvořte metodu void Add(Player player)
    // - zvětšete velikost pole o jeden prvek
    // - na poslední index v poli je vložen nový hráč (player)
    // - vyvolejte událost PlayersCountChanged s příslušnými argumenty
    public void Add(Player player)
    {
        players.Add(player);
        PlayersCountChanged?.Invoke(this, new PlayersCountChangedEventArgs() { NewCount = Count, OldCount = Count - 1 });
    }
    // TODO: Vytvořte metodu void Delete(int index)
    // - pokud je index neplatný - metoda nedělá nic
    // - odeberte vybraného hráče z pole, pole setřeste (posuňte hráče z vyšších indexů, tak aby byla zaplněna (null) mezera; zachovejte pořadí hráčů)
    // - zmenšete velikost pole o 1 prvek
    // - vyvolejte událost PlayersCountChanged s příslušnými argumenty
    public void Delete(int index)
    {
        players.RemoveAt(index);
        PlayersCountChanged?.Invoke(this, new PlayersCountChangedEventArgs() { NewCount = Count, OldCount = Count + 1 });
    }

    // TODO: Vytvořte metodu bool FindBestClubs(...)
    // - výstupní parametr FootballClub[] clubs
    // - výstupní parametr int goalCount
    // - pokud je pole players prázdné - pak - clubs: prázdné pole, goalCount: 0, metoda vrací false
    // - sečtěte počty gólů podle klubů
    // - najděte maximální počet gólů a uložte jej do goalCount
    // - najděte všechny kluby, které mají celkově goalCount gólů a uložte je do clubs
    // - vraťte true
    public bool FindBestClubs(out FootballClub[] clubs, out int goalCount)
    {
        if (Count == 0)
        {
            clubs = new FootballClub[0];
            goalCount = 0;
            return false; 
        }
        clubs = new FootballClub[0];
        int maxGoal = 0;
        goalCount = 0;

        foreach (FootballClub klub in FootballClubInfo.Items)
        {
            int goals = 0;
            for (int j = 0; j < players.Count; j++)
            {
                Player player = (Player)players[j];
                if (player.Club == klub)
                {
                    goals += player.GoalCount;
                }
            }
            if (goals > maxGoal)
            {
                maxGoal = goals;
            }
        }
        int index = 0;
        goalCount = maxGoal;

        foreach (FootballClub klub in FootballClubInfo.Items)
        {
            int goals = 0;
            for (int j = 0; j < players.Count; j++)
            {
                Player player = (Player)players[j];
                if (player.Club == klub)
                {
                    goals += player.GoalCount;

                }
            }
            if (goals == maxGoal)
            {
                Array.Resize(ref clubs, clubs.Length + 1);
                clubs[index++] = klub;            
            }
        }
        return true;
    }

    public Player[] Save()
    {
        Player[] poleHracu = new Player[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            poleHracu[i] = (Player)players[i];
        }
        return poleHracu;
    }

    public void Load(Player[] loadedPlayers)
    {
        players.Clear();
        foreach (Player player in loadedPlayers)
        {
            players.Add(player);
        }
    }

    public IEnumerator GetEnumerator()
    {
        return players.GetEnumerator();
    }
    // Konec třídy PlayerRecords
}