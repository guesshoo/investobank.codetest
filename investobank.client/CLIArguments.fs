namespace InvestoBank.Execution.Service

module CmdLine = 
    open Nessos.UnionArgParser

    type CLIArguments =
        | Order of string * uint16 * string
        | Report
        
    with
        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | Order _ -> "submit a client order: clientId qty orderType"
                | Report _ -> "report current day activity so far"

 