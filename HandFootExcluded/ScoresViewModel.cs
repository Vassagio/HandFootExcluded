﻿using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.Common;

namespace HandFootExcluded;

public interface IScoresViewModel
{
    IEnumerable<IPlayerScore> PlayerScores { get; }
}

internal sealed class ScoresViewModel : BindableItem, IScoresViewModel
{
    private IEnumerable<IPlayerScore> _playerScores = Enumerable.Empty<IPlayerScore>();

    public IEnumerable<IPlayerScore> PlayerScores { get => _playerScores; set => SetProperty(ref _playerScores, value); }

    public ScoresViewModel() { EventAggregator.Instance.RegisterHandler<IEnumerable<IPlayerScore>>(Score); }

    private void Score(IEnumerable<IPlayerScore> playerScores) => PlayerScores = playerScores.OrderByDescending(ps => ps.Score);
}