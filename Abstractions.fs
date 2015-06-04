namespace InvestoBank.Execution
open InvestoBank.Execution.Domain


module Abstractions =
    type ITradeService =
        abstract member ReceivedClientOrder: clientId:string * qty:uint16  * orderType: OrderType-> Result<ExecutedOrderData,string>

    type IBrokerFacade =
        abstract member QuoteOnOrder: OpenOrderData -> Result<BrokerOrderQuote, string>
        abstract member ExecuteQuote: BrokerOrderQuote -> Result<BrokerExecutedOrder,string>



