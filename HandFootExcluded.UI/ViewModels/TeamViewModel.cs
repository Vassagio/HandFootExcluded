using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.UI.Eventing;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    private string _teamName;
    private string _playerInitials;
    private bool _playerBonus = false;
    private string _partnerInitials;
    private bool _partnerBonus = false;
    private int _topScore;
    private int _bottomScore;
    private int _score;

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

public class TeamViewModelConverter : JsonConverter<ITeamViewModel>
{
    public override ITeamViewModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<TeamViewModel>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, ITeamViewModel value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (TeamViewModel)value, options);
    }
}