module App.Questions.Q3

open App.Types
module Q = App.Types.Question


let chance() =
    Q.choice "If I have a box with three balls in it - blue, red, and green, - and I pull a ball from the box without looking..."
        [ Q.options ["1/3"] ["1/4"; "1/6"; "50%"] |> Q.withTitle "What is the chance that I pull a red ball?"
          Q.options ["1/3"] ["1/4"; "1/6"; "50%"] |> Q.withTitle "What is the chance that I pull a green ball?"
          Q.options ["2/3"] ["3/4"; "1/3"; "1/6"] |> Q.withTitle "What is the chance that I pull a red OR a green ball?" ]

let all : Test list =
    [ chance ]
