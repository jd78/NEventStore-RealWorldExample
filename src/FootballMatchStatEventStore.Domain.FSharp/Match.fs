namespace FootballMatchStatEventStore.Domain.FSharp

open System
open FootballMatchStatEventStore.Contracts
open FootballMatchStatEventStore.Common
open CommonDomain.Core


[<AbstractClass>]
type DomainBase() =
   inherit AggregateBase()

   member this.RaiseDomainEvent (evt:IEvent) =
    evt.Id <- this.Id
    evt.Version <- this.Version
    this.RaiseEvent evt

type MatchStatus =
    |Declared = 0
    |FirstHalf = 1
    |HalfTime = 2
    |SecondHalf = 3
    |Ended = 4

type Match = 
    inherit DomainBase
    val Id:Guid
    val mutable private _homeTeam:string 
    val mutable private _awayTeam:string 
    val mutable private _homeTeamScorers: seq<string>
    val mutable private _awayTeamScorers: seq<string>
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
    
    //Queries
    member this.Result = sprintf "%d - %d" (this._homeTeamScorers |> Seq.length) (this._awayTeamScorers |> Seq.length)
    member this.GetHomeTeam = this._homeTeam
    member this.GetAwayTeam = this._awayTeam
    member this.GetHomeTeamScorers = this._homeTeamScorers
    member this.GetAwayTeamScorers = this._awayTeamScorers
    member this.GetMatchStatus = this._matchStatus
    member this.GoalsPerHomePlayer name = this._homeTeamScorers |> Seq.filter (fun f -> f = name) |> Seq.length
    member this.GoalsPerAwayPlayer name = this._awayTeamScorers |> Seq.filter (fun f -> f = name) |> Seq.length

    //Commands
    member this.updateMatchStatus matchStatus = 
        MatchStatusUpdated(Status = EnumEx.MapByStringValue<MatchStatus, FootballMatchStatEventStore.Contracts.MatchStatus>(matchStatus)) |> this.RaiseDomainEvent
    member this.updateHomeScore scorer = HomeGoalScored(Scorer = scorer) |> this.RaiseDomainEvent
    member this.updateAwayScore scorer = AwayGoalScored(Scorer = scorer) |> this.RaiseDomainEvent

    //Apply/Mutators
    member private this.Apply (m:MatchDeclared) = 
        this._homeTeam <- m.HomeTeam
        this._awayTeam <- m.AwayTeam

    member private this.Apply (status:MatchStatusUpdated) = 
        this._matchStatus <- EnumEx.MapByStringValue<FootballMatchStatEventStore.Contracts.MatchStatus, MatchStatus>(status.Status)

    member private this.Apply(goal:HomeGoalScored) =
        this._homeTeamScorers <- Seq.append this._homeTeamScorers (Seq.singleton goal.Scorer)

    member private this.Apply(goal:AwayGoalScored) =
        this._awayTeamScorers <- Seq.append this._awayTeamScorers (Seq.singleton goal.Scorer)
        

module test =
    let m = Match.CreateMatch "" ""
    let aa = m.GetHomeTeamScorers
    
    
    