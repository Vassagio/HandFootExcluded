using System.Windows.Input;
using HandFootExcluded.UI.Services.ScoringServices;
using HandFootExcluded.UI.Views;

namespace HandFootExcluded.UI.ViewModels;

public interface ITotalScoreViewModel : IViewModel
{
    IScoreLines ScoreLines { get; }
    ICommand SummaryCommand { get; }
}

internal sealed class TotalScoreViewModel : ViewModelBase, ITotalScoreViewModel
{
    private IScoreLines _scoreLines;

    private ICommand _summaryCommand;

    public IScoreLines ScoreLines { get => _scoreLines; set => SetProperty(ref _scoreLines, value); }
    public ICommand SummaryCommand => _summaryCommand ?? new Command(Summary);

    public TotalScoreViewModel(IScoreLines scoreLines) => ScoreLines = scoreLines;

    private void Summary() { Navigate<ISummaryPage>(); }
}