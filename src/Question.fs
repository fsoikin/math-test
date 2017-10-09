module App.Question

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

type OptsListState = {
    prototype: Types.OptionsList
    options: string list
    selected: string list
    showIncorrect: bool
}

type QState = ChoiceState of OptsListState list | TextState of answer: string * showIncorrect: bool
type State = {
    question: Types.Question
    qState: QState
}

type Msg =
    | ToggleOption of OptsListState * string
    | EditAnswer of string

let private isSingleElement = function | [_] -> true | _ -> false

let optsList l dispatch =
    let optType = if isSingleElement l.prototype.correct then "radio" else "checkbox"
    div [ClassName (if l.showIncorrect then "incorrect option-list" else "option-list")]
        [
            match l.prototype.title with Some t -> yield div [ClassName "title"] [str t] | None -> ()

            for o in l.options do
                let isSelected = l.selected |> List.contains o
                let input = input [ Type optType
                                    Checked isSelected
                                    OnChange (fun _ -> dispatch <| ToggleOption (l, o)) ]
                yield div [] [ label [ClassName "radio"] [input; str " "; str o]]
        ]

let answer state dispatch =
    match state.qState with
    | ChoiceState opts ->
        div [ClassName "option-lists"] [for ol in opts -> optsList ol dispatch]
    | TextState (answer, showIncorrect) ->
        input
            [OnChange (fun e -> !!e.target?value |> EditAnswer |> dispatch)
             ClassName (if showIncorrect then "incorrect" else "")
             AutoFocus true
             Value !^answer]

let view state dispatch =
    div []
        [
            div [ClassName "question"] [str state.question.text]
            answer state dispatch
        ]

let init q =
    {
        question = q
        qState = match q.kind with
                 | Choice ols -> ChoiceState [for ol in ols -> { prototype = ol; options = shuffle ol.options; selected = []; showIncorrect = false }]
                 | Text _ -> TextState ("", false)
    }

let update msg state =
    let s =
        match state.qState, msg with
        | ChoiceState ols, ToggleOption (l, o) ->
            let newSelected =
                if isSingleElement l.prototype.correct then [o]
                elif l.selected |> List.contains o then l.selected |> List.except [o]
                else o :: l.selected
            let ols = ols |> List.map (fun ol ->
                if ol.prototype = l.prototype
                then { ol with selected = newSelected; showIncorrect = false }
                else ol )
            { state with qState = ChoiceState ols }

        | TextState (_, showIncorrect), EditAnswer a ->
            { state with qState = TextState (a, false) }

        | _ -> state

    s, Cmd.Empty

let isAnswered s =
    match s.qState with
    | ChoiceState ols -> ols |> List.forall (fun ol -> ol.selected <> [])
    | TextState (a, _) -> a <> ""

let showIncorrect s =
    { s with
        qState = match s.qState, s.question.kind with
                 | ChoiceState ols, _ -> ChoiceState [for ol in ols -> { ol with showIncorrect = List.sort ol.selected <> List.sort ol.prototype.correct }]
                 | TextState (a, _), Text check -> TextState (a, not <| check a)
                 | _ -> s.qState }

let isAnyIncorrectShown s =
    match s.qState with
    | ChoiceState ols -> ols |> List.exists (fun ol -> ol.showIncorrect)
    | TextState (_, showIncorrect) -> showIncorrect
