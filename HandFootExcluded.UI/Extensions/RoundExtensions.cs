using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;
using HandFootExcluded.Core.TeamServices;
using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Extensions;

public static class RoundExtensions
{
    //public static readonly ITotalScoreViewModel _totalScore = new TotalScoreViewModel();

    //public static IRoundViewModel ToViewModel(this IRound round) =>
    //    new RoundViewModel
    //    {
    //        Order = round.Order,
    //        OpenAmount = round.OpenAmount,
    //        StartingTeam = round.Teams.Find<IStartingTeam>().ToViewModel(),
    //        OpposingTeam = round.Teams.Find<IOpposingTeam>().ToViewModel(),
    //        ExcludedPlayerInitials = round.Players?.Find<IExcludedPlayer>()?.Initials ?? string.Empty,
    //        TotalScore = _totalScore
    //    };

    //public static IEnumerable<IRoundViewModel> ToViewModel(this IRounds rounds) => rounds.Select(ToViewModel);
}