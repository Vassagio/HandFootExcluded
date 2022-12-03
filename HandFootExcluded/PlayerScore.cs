using System.Diagnostics;

namespace HandFootExcluded;

public interface IPlayerScore
{
    string Bonus {get;}
    IPlayer Player { get; }
    int Score { get; }
}

[DebuggerDisplay("{Display,nq}")]
internal sealed class PlayerScore : BindableItem, IPlayerScore
{    
    private string _bonus;
    private IPlayer _player;
    private int _score;

    public string Bonus { get => _bonus; set => SetProperty(ref _bonus, value); }
    public IPlayer Player { get => _player; set => SetProperty(ref _player, value); }
    public int Score { get => _score; set => SetProperty(ref _score, value); }

    public PlayerScore(IPlayer player, int score, int bonusCount)
    {
        Bonus = new string('*', bonusCount);
        Player = player;
        Score = score;
    }
    
    private string Display => $"{_bonus}{_player.FirstName}: {_score}";

    public override string ToString() => Display;
}