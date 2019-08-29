﻿/// Содержит однопоточную реализацию lazy-объекта.
module SingleThreadedLazy
    
    open ILazy

    /// Однопоточная реализация для lazy-объекта.
    type SingleThreadedLazy<'a> (supplier : unit -> 'a) =       
        /// Результат вычисления.
        [<DefaultValue>] val mutable result : 'a
        
        /// Выполнено ли вычисление.
        let mutable isValueCreated = false

        interface ILazy<'a> with
            member this.Get () =
                if isValueCreated then this.result
                else                 
                    this.result <- (supplier ())    
                    isValueCreated <- true
                    this.result

        /// Выполнено ли вычисление.
        member this.IsValueCreated = isValueCreated