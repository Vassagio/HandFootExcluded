﻿using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.Common;
using HandFootExcluded.UI.Eventing;
using HandFootExcluded.UI.Services.ScoringServices;

namespace HandFootExcluded.UI.ViewModels;

public interface IRoundViewModel : IViewModel
{
    int Order {get;}
    int OpenAmount {get;}
    ITeamViewModel StartingTeam {get;}
    ITeamViewModel OpposingTeam {get;}
    string ExcludedPlayerInitials {get;}
}

internal sealed class RoundViewModel : ViewModelBase, IRoundViewModel
{


    private int _order;
    private int _openAmount;
    private ITeamViewModel _startingTeam;
    private ITeamViewModel _opposingTeam;
    private string _excludedPlayerInitials;
    

    public int Order { get => _order; set => SetProperty(ref _order, value); }
    public int OpenAmount { get => _openAmount; set => SetProperty( ref _openAmount, value); }
    public ITeamViewModel StartingTeam { get => _startingTeam; set => SetProperty(ref _startingTeam, value); }
    public ITeamViewModel OpposingTeam { get => _opposingTeam; set => SetProperty(ref _opposingTeam, value); }
    public string ExcludedPlayerInitials { get => _excludedPlayerInitials; set => SetProperty(ref _excludedPlayerInitials, value); }
}