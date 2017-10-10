module App.Types

type OptionsList = {
    title: string option
    options: string list
    correct: string list
}

type QuestionKind = Choice of options: OptionsList list | Text of checkAnswer: (string -> bool)

type Question = {
    text: string
    kind: QuestionKind
}

type Test = unit -> Question

module Question =
    let options correct incorrect = { title = None; options = correct @ incorrect; correct = correct }
    let withTitle title opts = { opts with title = Some title }
    let choice text opts = { text = text; kind = Choice opts }
    let text text checkAnswer = { text = text; kind = Text checkAnswer }

[<AutoOpen>]
module Utils =
    let private _rnd = System.Random()
    let shuffle list =
        list
        |> List.map (fun x -> x, _rnd.Next())
        |> List.sortBy snd
        |> List.map fst

    let tryInt s = match System.Int32.TryParse s with | true, x -> Some x | _ -> None

    let rnd min max = _rnd.Next(min, max)
