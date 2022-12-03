namespace HandFootExcluded;

public interface IPlayerScore
{
    IPlayer Player { get; }
    int Score { get; }
}

internal sealed class PlayerScore : BindableItem, IPlayerScore
{
    private IPlayer _player;
    private int _score;

    public IPlayer Player { get => _player; set => SetProperty(ref _player, value); }
    public int Score { get => _score; set => SetProperty(ref _score, value); }

    public PlayerScore(IPlayer player, int score)
    {
        Player = player;
        Score = score;
    }

    public override string ToString() => $"{_player.FirstName}: {_score}";
}