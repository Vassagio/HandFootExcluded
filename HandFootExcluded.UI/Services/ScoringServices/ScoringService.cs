using HandFootExcluded.Core.GameServices;
using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IScoringService
{
    Task<IScoreLines> Score(IGame game, IEnumerable<IRoundViewModel> rounds, IDictionary<int, int> individualBonusAmounts, int teamBonusAmount);
}

internal sealed class ScoringService : IScoringService
{
    private readonly IScoreLineFactory _scoreLineFactory;

    public ScoringService(IScoreLineFactory scoreLineFactory) => _scoreLineFactory = scoreLineFactory ?? throw new ArgumentNullException(nameof(scoreLineFactory));

    public async Task<IScoreLines> Score(IGame game, IEnumerable<IRoundViewModel> rounds, IDictionary<int, int> individualBonusAmounts, int teamBonusAmount) => _scoreLineFactory.Create(game, rounds, individualBonusAmounts, teamBonusAmount);
}