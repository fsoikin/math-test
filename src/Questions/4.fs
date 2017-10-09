module App.Questions.Q4

open App.Types
module Q = App.Types.Question


let rectangle() =
    Q.choice "What's the area of a rectangle that is 8 meters wide and 4 meters high?"
        [ Q.options ["32 square meters"] ["12 square meters"; "2 square meters"; "24 square meters"; "4 square meters"] ]

let all : Test list =
    [ rectangle ]
