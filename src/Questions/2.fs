module App.Questions.Q2

open App.Types
module Q = App.Types.Question

let arith op opName (min, max) () =
    let rnd = System.Random()
    let x, y = rnd.Next(min, max), rnd.Next(min, max)
    Q.text (sprintf "%d %s %d" x opName y) (tryInt >> (=) (Some (op x y)))

let sum = arith (+) "+"
let mult = arith (*) "*"

let all : Test list =
    [ sum (21, 999)
      sum (1, 99)
      sum (1, 19)
      mult (1, 19) ]
    |> List.replicate 5
    |> List.concat
