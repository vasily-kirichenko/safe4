open System
open System.Diagnostics
open System.IO
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Saturn
open Shared
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Elmish
open Elmish.Bridge

let publicPath = Path.GetFullPath "../Client/public"
let port = 8085us

type State = unit

let init (_clientDispatch: Dispatch<ClientMsg>) () =
    (), Cmd.ofMsg ServerMsg.Tick

let update (clientDispatch: Dispatch<ClientMsg>) msg state =
    match msg with 
    | ServerMsg.Tick ->
        clientDispatch (ClientMsg.GotNow DateTime.Now)
        state,
        Cmd.ofAsync
            (fun () -> Async.Sleep 100)
            ()
            (fun _ -> ServerMsg.Tick)
            (fun _ -> ServerMsg.Tick)

let webApp =
    let server =
        Bridge.mkServer Shared.Bridge.endpoint init update
        |> Bridge.withConsoleTrace
        |> Bridge.run Giraffe.server 
          
    choose [
        server
        route "/" >=> htmlFile "/pages/index.html"
    ]

let app = 
    application {
        url (sprintf "http://0.0.0.0:%d/" port)
        use_router webApp
        app_config Giraffe.useWebSockets
        memory_cache
        use_static publicPath
        disable_diagnostics
        use_gzip
    }

run app
