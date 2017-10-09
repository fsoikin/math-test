module App.Root

open Elmish
open Elmish.Browser.Navigation
open Elmish.Browser.UrlParser
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Types

importAll "../sass/main.sass"

open Fable.Helpers.React
open Fable.Helpers.React.Props

type State = {
    current: Question.State option
    next: Question.State list
}

type Msg =
    | CurrentMsg of Question.Msg
    | Next

let init() =
    let qs =
        shuffle Questions.All.all
        |> List.truncate 10
        |> List.map (fun t -> Question.init (t()))

    let s =
        match qs with
        | cur::next -> { current = Some cur; next = next }
        | _ -> { current = None; next = [] }

    s, Cmd.Empty

let update msg state =
    match msg, state.current with
    | CurrentMsg m, Some cur ->
        let c, cmd = Question.update m cur
        { state with current = Some c }, Cmd.map CurrentMsg cmd
    | Next, Some cur when Question.isAnswered cur ->
        let newCur = Question.showIncorrect cur
        let s =
            match state.next, Question.isAnyIncorrectShown newCur with
            | [], false -> { current = None; next = [] }
            | [], true -> { current = Some newCur; next = [] }
            | n::rest, false -> { current = Some n; next = rest }
            | n::rest, true -> { current = Some n; next = rest @ [newCur] }
        s, Cmd.Empty
    | _ ->
        state, Cmd.Empty

let private isNone = function None -> true | _ -> false

let root state dispatch =

  let pageHtml =
    match state.current with
    | Some cur -> Question.view cur (dispatch << CurrentMsg)
    | None -> div [ClassName "done"] [str "Good job! Now go play."]

  let canGoNext =
    match state.current with
    | Some c -> Question.isAnswered c
    | None -> false

  let goNext =
    match state.current with
    | Some c ->
        [button
            [ Disabled (not canGoNext)
              Type "submit"
              ClassName "button is-primary"] [str "Next"] ]
    | _ ->
        []

  let progress =
    match state.current, state.next with
    | None, _ -> ""
    | Some _, [] -> "last question!"
    | Some _, l -> sprintf "%d more questions to go" (List.length l)

  div
    []
    [ div
        [ ClassName "section" ]
        [ form
            [ ClassName "container"
              OnSubmit (fun e -> if canGoNext then dispatch Next; e.preventDefault())]
            [ div [ClassName "columns"]
                  [ div [ClassName "column is-one-quarter"] []
                    div [ClassName "column"] [h3 [] [str progress] ]
                    div [ClassName "column is-one-quarter"] [] ]
              div [ ClassName "columns" ]
                  [ div [ClassName "column is-one-quarter"] []
                    div [ClassName "column"]
                        [ pageHtml
                          div [ClassName "control"] goNext ]
                    div [ClassName "column is-one-quarter"] [] ]
            ]
        ]
    ]

open Elmish.React
open Elmish.Debug

// App
Program.mkProgram init update root
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
