using HandFootExcluded.Core.GameServices;
using HandFootExcluded.UI.Services.ConfigurationServices;
using HandFootExcluded.UI.Services.ScoringServices;
using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Services.GameHistoryServices
{
    public interface IGameHistoryService
    {
        void Save(IGame game, IEnumerable<IRoundViewModel> rounds);
        IEnumerable<IGameHistory> Load();
    }

    internal sealed class GameHistoryService : IGameHistoryService
    {
        private readonly IScoringService _scoringService;
        private readonly IConfigurationManager _configurationManager;
        public GameHistoryService(IScoringService scoringService, IConfigurationManager configurationManager)
        {
            _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
        }

        public async void Save(IGame game, IEnumerable<IRoundViewModel> rounds)
        {
            var settings = await _configurationManager.Load();
            var score = await _scoringService.Score(game, rounds, settings.BonusAmount);
            var settingsPageViewModel = new SettingsPageViewModel(settings);

            var gameHistory = new GameHistory(DateTime.Now, score, settingsPageViewModel);
        }

        public IEnumerable<IGameHistory> Load()
        {
            return Enumerable.Empty<IGameHistory>();
        }
    }
}
