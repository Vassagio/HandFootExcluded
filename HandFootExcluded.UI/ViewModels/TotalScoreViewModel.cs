using HandFootExcluded.UI.Services.ScoringServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HandFootExcluded.UI.ViewModels;

public interface ITotalScoreViewModel : IViewModel
{
    IScoreLines ScoreLines { get; }
}

internal sealed class TotalScoreViewModel : ViewModelBase, ITotalScoreViewModel
{
    private IScoreLines _scoreLines;

    public IScoreLines ScoreLines { get => _scoreLines; set => SetProperty(ref _scoreLines, value); }

    public TotalScoreViewModel() { } 
    public TotalScoreViewModel(IScoreLines scoreLines) => ScoreLines = scoreLines;
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