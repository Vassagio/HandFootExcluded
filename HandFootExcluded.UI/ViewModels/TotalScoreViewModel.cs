using HandFootExcluded.UI.Services.ScoringServices;

namespace HandFootExcluded.UI.ViewModels;

public interface ITotalScoreViewModel : IViewModel
{
    IScoreLines ScoreLines { get; }
}

internal sealed class TotalScoreViewModel : ViewModelBase, ITotalScoreViewModel
{
    private IScoreLines _scoreLines;

    public IScoreLines ScoreLines { get => _scoreLines; set => SetProperty(ref _scoreLines, value); }

    public TotalScoreViewModel(IScoreLines scoreLines) => ScoreLines = scoreLines;
}