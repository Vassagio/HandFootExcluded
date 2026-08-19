using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.UI.Eventing;
using HandFootExcluded.UI.Services.ScoringServices;
using HandFootExcluded.UI.Views;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace HandFootExcluded.UI.ViewModels;

public interface ITotalScoreViewModel : IViewModel
{
    ICommand SummaryCommand { get; }
    IScoreLines ScoreLines { get; }
}

internal sealed class TotalScoreViewModel : ViewModelBase, ITotalScoreViewModel
{
    private IScoreLines _scoreLines;
    private ICommand _summaryCommand;
    private readonly IScoreLines _allScores;

    [JsonIgnore]
    public ICommand SummaryCommand => _summaryCommand ?? new Command(Summary);

    public IScoreLines ScoreLines { get => _scoreLines; set => SetProperty(ref _scoreLines, value); }

    public TotalScoreViewModel() { } 
    public TotalScoreViewModel(IScoreLines scoreLines)
    {
        _allScores = scoreLines;
        ScoreLines = scoreLines.GetGrandTotalLines();
    }

    private void Summary()
    {
        EventAggregator.Instance.PostMessage(new ScoreLinesChangedEvent(_allScores));
        Navigate<ISummaryPage>();
    }
}

public class TotalScoreViewModelConverter : JsonConverter<ITotalScoreViewModel>
{
    public override ITotalScoreViewModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<TotalScoreViewModel>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, ITotalScoreViewModel value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (TotalScoreViewModel)value, options);
    }
}