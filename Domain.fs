namespace InvestoBank.Execution

module Domain =
    open System
    open System.Runtime.Serialization
  
   
    type ClientId = ClientId of string

   
    //[<CustomEquality;CustomComparison>]
   // [<StructuralEquality;StructuralComparison>]
    type BrokerId = BrokerId of string 
   (*
             with
        interface System.IEquatable<BrokerId> with
            member x.Equals(y) = 
                let (BrokerId right) = y
                let (BrokerId left) = x
                left.Equals(right, StringComparison.InvariantCultureIgnoreCase)

        interface System.IComparable with
            member x.CompareTo yobj =
                 match yobj with
                  | :? BrokerId as y -> compare x y
                  | _ -> invalidArg "yobj" "cannot compare values of different types"
                  *)

                  (* with
        override x.Equals (y: obj ) =
            match  y with
            | :? BrokerId as y' -> 
                    let (BrokerId left) = y'
                    let (BrokerId right) = x
                    left.Equals( right, StringComparison.InvariantCultureIgnoreCase)
            | _ -> false
         override x.GetHashCode() =
            let (BrokerId x') = x
            x'.ToUpper().GetHashCode()

         interface System.IComparable with
            member x.CompareTo yobj =
                  match yobj with
                  | :? BrokerId as y -> compare x y
                  | _ -> invalidArg "yobj" "cannot compare values of different types"
                  *)
 
    type OrderType = 
        | BUY | SELL | SUM

    type OpenOrderData = {
        OrderType : OrderType;
        ClientId: ClientId;
        Qty:  uint16;
        WhenReceived: DateTime;
    } 
   
    type BrokerOrderQuote= {
        Id: BrokerId;
        OrderType: OrderType;
        WhenQuoted: DateTime;
        Qty: uint16;
        Quote: float;
    }    

    type BrokerExecutedOrder ={
        Id: BrokerId;
        OrderType: OrderType;
        WhenExecuted: DateTime;
        Qty: uint16
        Amount: float;
    }

    type QuotedOrderData = { 
        Order: OpenOrderData;
        Quotes: BrokerOrderQuote seq
    }
    
    type ExecutedOrderData = {
        Execution: BrokerExecutedOrder seq
    }

    type ClientOrder = 
    | OpenOrder of OpenOrderData
    | QuotedOrder of QuotedOrderData
    | ExecutedOrder of OpenOrderData * ExecutedOrderData

    /// Type Extensions
    type OpenOrderData with
        static member CreateOpenOrderData (id:string, qty:uint16, orderType:OrderType) =
           { OpenOrderData.ClientId = ClientId id; OrderType = orderType; Qty = qty; WhenReceived = DateTime.UtcNow }

   