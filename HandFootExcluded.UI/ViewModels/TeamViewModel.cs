using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.UI.Eventing;

namespace HandFootExcluded.UI.ViewModels;

public interface ITeamViewModel : IViewModel
{
    string TeamName { get; }
    string PlayerInitials { get; }
    bool PlayerBonus { get; }
    string PartnerInitials { get; }
    bool PartnerBonus { get; }
    int TopScore { get; }
    int BottomScore { get; }
    int Score { get; }
}

internal sealed class TeamViewModel : ViewModelBase, ITeamViewModel
{
    private int _bottomScore;
    private bool _partnerBonus;
    private string _partnerInitials;
    private bool _playerBonus;
    private string _playerInitials;
    private int _score;
    private string _teamName;
    private int _topScore;

    public string TeamName { get => _teamName; set => SetProperty(ref _teamName, value); }
    public string PlayerInitials { get => _playerInitials; set => SetProperty(ref _playerInitials, value); }
    public bool PlayerBonus { get => _playerBonus; set => SetProperty(ref _playerBonus, value, OnScoreChanged); }
    public string PartnerInitials { get => _partnerInitials; set => SetProperty(ref _partnerInitials, value); }
    public bool PartnerBonus { get => _partnerBonus; set => SetProperty(ref _partnerBonus, value, OnScoreChanged); }
    public int TopScore { get => _topScore; set => SetProperty(ref _topScore, value, OnScoreChanged); }
    public int BottomScore { get => _bottomScore; set => SetProperty(ref _bottomScore, value, OnScoreChanged); }
    public int Score { get => _score; set => SetProperty(ref _score, value); }

    private void OnScoreChanged()
    {
        Score = TopScore + BottomScore;
        EventAggregator.Instance.SendMessage(new ScoreChangedEvent());
    }
}