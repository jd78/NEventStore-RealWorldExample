namespace FootballMatchStatEventStore.Domain.FSharp

open System
open FootballMatchStatEventStore.Contracts
open FootballMatchStatEventStore.Contracts
open CommonDomain.Core

[<AbstractClass>]
type DomainBase() =
   inherit AggregateBase()

   member this.RaiseDomainEvent (evt:IEvent) =
    evt.Id <- this.Id
    evt.Version <- this.Version
    this.RaiseEvent evt

type MatchStatus =
    |Declared
    |FirstHalf
    |HalfTime
    |SecondHalf
    |Ended

type Match = 
    inherit DomainBase
    val Id:Guid
    val mutable private _homeTeam:string 
    val mutable private _awayTeam:string 
    val mutable private _homeTeamScorers: List<string>
    val mutable private _awayTeamScorers: List<string>
    val mutable private _matchStatus:MatchStatus

    private new (homeTeam, awayTeam) = { 
        inherit DomainBase();
        Id = Guid.NewGuid(); 
        _homeTeam=homeTeam;
        _awayTeam=awayTeam;
        _homeTeamScorers=[];
        _awayTeamScorers=[];
        _matchStatus=MatchStatus.Declared;
    }

    static member CreateMatch homeTeam awayTeam =
        Match(homeTeam, awayTeam)

    member this.Result = sprintf "%d - %d" this._homeTeamScorers.Length this._awayTeamScorers.Length
    member this.GetHomeTeam = this._homeTeam
    member this.GetAwayTeam = this._awayTeam
    member this.GetHomeTeamScorers = this._homeTeamScorers |> List.toSeq
    member this.GetAwayTeamScorers = this._awayTeamScorers |> List.toSeq
    member this.GetMatchStatus = this._matchStatus
    member this.GoalsPerHomePlayer name = this._homeTeamScorers |> List.filter (fun f -> f = name) |> List.length
    member this.GoalsPerAwayPlayer name = this._awayTeamScorers |> List.filter (fun f -> f = name) |> List.length

module test =
    let m = Match.CreateMatch "" ""
    let aa = m.GetHomeTeamScorers
    
    
    