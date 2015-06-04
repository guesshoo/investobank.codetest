#load "Common.fs"
#load "Domain.fs"
open InvestoBank.Execution.Common
#load "Abstractions.fs"
#load "Broker.fs"

open System;

open InvestoBank.Execution.Broker
open InvestoBank.Execution.Domain;;
let order = { OpenOrderData.ClientId = ClientId "NH04058"; 
    OpenOrderData.OrderType = BUY; 
    OpenOrderData.Qty =50us; 
    OpenOrderData.WhenReceived = DateTime.UtcNow
    }


let broker = new Broker2() :> InvestoBank.Execution.Abstractions.IBrokerFacade
let quote = broker.QuoteOnOrder order

printfn "q:%O" quote