namespace InvestoBank.Execution

open InvestoBank.Execution
open InvestoBank.Execution.Abstractions
open InvestoBank.Execution.Domain

module TradingPlatform = 
    let get_quotes (brokers: IBrokerFacade list) order =
        let quotes_results = brokers |> Seq.map( fun b -> b.QuoteOnOrder order)
        let successfully_quotes = onlySuccess quotes_results
        successfully_quotes   


    type TradingService(brokers: IBrokerFacade list) =
        let GetQuoteFromBrokers (order:ClientOrder) : Result<seq<BrokerOrderQuote>, string> = 
            match order with
            | ExecutedOrder executed -> sprintf "Cannot quote on executed order %O" executed |> Failure
            | QuotedOrder quoted -> get_quotes brokers quoted.Order |> Success
            | OpenOrder openOrder -> get_quotes brokers openOrder  |> Success


        let ExecuteOn

        interface ITradeService with
            member x.ReceivedClientOrder(clientId:string, qty:uint16, orderType:OrderType) = 
                let openOrder = OpenOrderData.CreateOpenOrder(clientId, qty, orderType)
                let quotes = GetQuoteFromBrokers openOrder
                match quotes with
                | Success good_quotes -> 
                | Failure msg -> Failure msg
                