module Client

open Elmish
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch
open Shared
open Fable.Import
open Fable.Import.React
open Fulma
open System
open Elmish.Bridge

type Model = { Now: DateTime option }

let init(): Model * Cmd<ClientMsg> =
    { Now = None }, Cmd.none

let update (msg: ClientMsg) (model: Model): Model * Cmd<ClientMsg> =
    match msg with
    | ClientMsg.GotNow now ->
        { model with Now = Some now }, Cmd.none

let view (model: Model) (dispatch: ClientMsg -> unit) =
    div []
        [ div [ ClassName "block"; HTMLAttr.MarginHeight 20.0 ]
              [ Box.box' []
                  [ match model.Now with
                    | None -> yield str "No now!"
                    | Some x -> yield str (string x) ]] 
        ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
|> Program.withBridge Shared.Bridge.endpoint
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
