module App.Questions.Q4

open App.Types
module Q = App.Types.Question


let rectangle() =
    let w, h = rnd 3 12, rnd 3 12
    let s = sprintf "%d square meters"
    Q.choice (sprintf "What's the area of a rectangle that is %d meters wide and %d meters high?" w h)
        [ Q.options [s (w*h)] (List.map s [h; w; w+h; (w+h)*2; (w*h)/2; (w*h)*2]) ]

let circumference() =
    Q.choice "Что такое окружность?"
        [ Q.options ["Линия состоящая из всех точек равно удалённых от центра"] ["То же самое, что круг"; "Форма пончика"; "Линия состоящая из точек находящихся в прямой пропорции"; "Красная точка"]
          Q.withTitle "And in English?" <| Question.options ["Circle"] ["Square"; "Surface"; "Cylinder"; "Parabola"] ]

let all : Test list =
    [ rectangle; circumference ]
