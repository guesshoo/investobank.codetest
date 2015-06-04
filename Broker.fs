namespace InvestoBank.Execution

open System
open System.Linq
open InvestoBank.Execution.Domain
open InvestoBank.Execution.Abstractions

module Broker = 
    
    let inline (|MULTIPLE10|NON10|) qty =
        if (qty%10us = 0us) then MULTIPLE10 else NON10

//    let (|ValidateQty|_|) qty =
//        match qty with
//        | NON10 -> Some (sprintf "Multiples of 10 allowed. Quantity specified: %O" qty)
//        | (qty >= 100us) -> Some (sprintf "Maximum Quantity allowed is 100. You've submitted %O" qty)
//        | _ -> None

    let contains x =  Seq.exists((=)x)
    let CreateQuote (openOrder:OpenOrderData, brokerId:string, quote:float) ={ 
        BrokerOrderQuote.Id = BrokerId brokerId; 
        BrokerOrderQuote.OrderType = openOrder.OrderType;
        BrokerOrderQuote.Qty = openOrder.Qty;
        BrokerOrderQuote.WhenQuoted = DateTime.UtcNow
        BrokerOrderQuote.Quote = quote
        }

    type Broker1 () =
        let PRICE = 1.49
        let COMMISION = 1.05

        interface IBrokerFacade with 
            member x.QuoteOnOrder order = 
                let { OpenOrderData.Qty = qty} = order
                match qty with
                | NON10 -> Failure ( sprintf "Multiples of 10 allowed. You've order :%O" qty)
                | MULTIPLE10 -> 
                    let quote = PRICE * COMMISION * (float qty)
                    Success (CreateQuote(order, "BROKER1", quote ))

            member x.ExecuteQuote order = Failure "Oh Yes"

        member x.QuoteOnOrder = (x:> IBrokerFacade).QuoteOnOrder
        member x.ExecuteQuote = (x:> IBrokerFacade).ExecuteQuote


    type Broker2 () =
        let PRICE = 1.52
        let COMMISSIONS = [ 
            [10us..10us..40us] , 1.03; 
            [50us..10us..80us] , 1.025
            [90us..10us..100us] , 1.02;
           ]

        let calculate_quote order =
            let { OpenOrderData.Qty = qty} = order
            let commission = COMMISSIONS |> Seq.find(fun (r, c)-> contains qty r) |> fun (r, c)-> c
            let quote = PRICE * commission * (float qty)
            Success (CreateQuote(order, "BROKER2", quote ))

        interface IBrokerFacade with 
            member x.QuoteOnOrder order = 
                let { OpenOrderData.Qty = qty} = order
                match qty with
                | NON10 -> Failure ( sprintf "Multiples of 10 allowed. You've order :%O" qty)
                | MULTIPLE10 -> calculate_quote order

            member x.ExecuteQuote order = Failure "Oh Yes"
        
        member x.QuoteOnOrder = (x:> IBrokerFacade).QuoteOnOrder
        member x.ExecuteQuote = (x:> IBrokerFacade).ExecuteQuote
    