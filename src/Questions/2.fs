module App.Questions.Q2

open App.Types
module Q = App.Types.Question

let arith op opName (min, max) () =
    let x, y = rnd min max, rnd min max
    Q.text (sprintf "%d %s %d" x opName y) (tryInt >> (=) (Some (op x y)))

let sum = arith (+) "+"
let mult = arith (*) "*"

let div (min, max) () =
    let x, y = rnd 2 10, rnd min max
    let product = x*y
    Q.text (sprintf "%d / %d" product x) (tryInt >> (=) (Some y))

let all : Test list =
    [ sum (21, 999)
      sum (1, 99)
      sum (1, 19)
      mult (1, 19)
      div (1, 10)
      div (10, 30) ]
    |> List.replicate 5
    |> List.concat
