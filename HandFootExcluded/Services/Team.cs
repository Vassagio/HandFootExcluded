using System.Collections;
using System.Diagnostics;
using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.Common;
using HandFootExcluded.Events;

namespace HandFootExcluded.Services;

public interface ITeam : ISet<IPlayer>
{
    int TopScore { get; }
    int BottomScore { get; }
    int Score { get; }
}

internal sealed partial class GameService
{
    [DebuggerDisplay("{Display,nq}")]
    private sealed class Team : BindableItem, ITeam
    {
        private readonly ISet<IPlayer> _players = new HashSet<IPlayer>(2);

        private int _topScore;
        private int _bottomScore;
        private int _score;

        public int Count => _players.Count;
        public bool IsReadOnly => _players.IsReadOnly;
        public int TopScore { get => _topScore; set => SetProperty(ref _topScore, value, OnScoreChanged); }
        public int BottomScore { get => _bottomScore; set => SetProperty(ref _bottomScore, value, OnScoreChanged); }
        public int Score { get => _score; set => SetProperty(ref _score, value); }

        private string Display => string.Join(" / ", _players);

        public Team(IPlayer player, IPlayer partner)
        {
            _players.Add(player);
            _players.Add(partner);
        }

        public IEnumerator<IPlayer> GetEnumerator() => _players.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<IPlayer>.Add(IPlayer player)
        {
            if (player is null or UnknownPlayer)
                return;

            _players.Add(player);
        }

        bool ISet<IPlayer>.Add(IPlayer player) => player is not (null or UnknownPlayer) && _players.Add(player);
        public void ExceptWith(IEnumerable<IPlayer> players) => _players.ExceptWith(players);
        public void IntersectWith(IEnumerable<IPlayer> players) => _players.IntersectWith(players);
        public bool IsProperSubsetOf(IEnumerable<IPlayer> players) => _players.IsProperSubsetOf(players);
        public bool IsProperSupersetOf(IEnumerable<IPlayer> players) => _players.IsProperSupersetOf(players);
        public bool IsSubsetOf(IEnumerable<IPlayer> players) => _players.IsSubsetOf(players);
        public bool IsSupersetOf(IEnumerable<IPlayer> players) => _players.IsSupersetOf(players);
        public bool Overlaps(IEnumerable<IPlayer> players) => _players.Overlaps(players);
        public bool SetEquals(IEnumerable<IPlayer> players) => _players.SetEquals(players);
        public void SymmetricExceptWith(IEnumerable<IPlayer> players) => _players.SymmetricExceptWith(players);
        public void UnionWith(IEnumerable<IPlayer> players) => _players.UnionWith(players);
        public void Clear() => _players.Clear();
        public bool Contains(IPlayer player) => _players.Contains(player);
        public void CopyTo(IPlayer[] array, int arrayIndex) => _players.CopyTo(array, arrayIndex);
        public bool Remove(IPlayer player) => _players.Remove(player);

        private void OnScoreChanged()
        {
            Score = _topScore + _bottomScore;
            EventAggregator.Instance.SendMessage(PlayerScoreEvent.Yes);
        }

        public override string ToString() => Display;
    }
}