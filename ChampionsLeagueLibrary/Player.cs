namespace ChampionsLeagueLibrary;

// TODO: Vytvořte třídu Player
public class Player
{
    // TODO: Vytvořte vlastnosti
    // - string Name
    // - FootballClub Club
    // - int GoalCount
    public string Name { get; set; }
    public FootballClub Club { get; set; }
    public int GoalCount { get; set; }

    // TODO: Vytvořte parametrický konstruktor (name, club, goalCount)
    public Player(string name, FootballClub club, int goalCount)
    {
        Name = name;
        Club = club;
        GoalCount = goalCount;
    }

    public override bool Equals(Object? player2)
    {
        if (player2 == null) return false;
        Player pl2 = (Player)player2;
        if (this.Name == pl2.Name && this.Club == pl2.Club && this.GoalCount == pl2.GoalCount)
        {
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode() + Club.GetHashCode() + GoalCount.GetHashCode();
    }

    //  Konec třídy Player
}
