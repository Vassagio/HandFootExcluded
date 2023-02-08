using HandFootExcluded.Core.GameServices;

namespace HandFootExcluded.UI.Services.GameHistoryServices
{
    public interface IGameHistoryService
    {
        void Save(IGame game);
        IEnumerable<IGameHistory> Load();
    }

    internal sealed class GameHistoryService : IGameHistoryService
    {
        public void Save(IGame game) { }

        public IEnumerable<IGameHistory> Load()
        {
            return Enumerable.Empty<IGameHistory>();
        }
    }
}
