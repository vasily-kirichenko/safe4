namespace Shared

open System

type Counter = int

[<RequireQualifiedAccess>]
type ServerMsg = Tick

[<RequireQualifiedAccess>]
type ClientMsg =
    | GotNow of DateTime

module Bridge =
    let [<Literal>] endpoint = "/socket"