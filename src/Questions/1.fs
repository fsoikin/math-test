module App.Questions.Q1

open App.Types
module Q = App.Types.Question

let product() =
    Q.choice "What is a 'Product'?"
        [ Q.options ["Result of multiplication"] ["Result of addition"; "Result of division"; "A kind of function"]
          Q.withTitle "А по-русски?" <| Question.options ["Произведение"] ["Сумма"; "Вычитание"; "График"; "Умножение"] ]

let sum() =
    Q.choice "What is a 'Sum'?"
        [ Q.options ["Result of addition"] ["Result of multiplication"; "Result of division"; "A kind of function"]
          Q.withTitle "А по-русски?" <| Question.options ["Сумма"] ["Произведение"; "Вычитание"; "График"; "Умножение"] ]

let fraction() =
    Q.choice "In a fraction, what are the top and bottom numbers called?"
        [ Q.options ["Numerator and Denominator"] ["Sum and Difference"; "Multiply and Divide"; "Duck and Hen"]
          Q.withTitle "А по-русски?" <| Question.options ["Числитель и Знаменатель"] ["Произведение и Деление"; "Дивиденд и Адденд"; "Старик и Старуха"] ]

let all : Test list = [product; sum; fraction]
