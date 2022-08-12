namespace Feliz.TanStack.Table

open Fable.Core.JsInterop

[<AutoOpen>]
module Pagination =
    
    type Table =
        static member hasPreviousPage (table : Table<'T>) : bool =
            table?_obj?getCanPreviousPage()
        
        static member hasNextPage (table : Table<'T>) : bool =
            table?_obj?getCanNextPage()
            
        static member setPageIndex (index : int) (table : Table<'T>) =
            table?_obj?setPageIndex(index)
            table
            
        static member setPageSize (size : int) (table : Table<'T>) : Table<'T> =
            table?_obj?setPageSize(size)
            table
            
        static member getPageCount (table : Table<'T>) : int =
            table?_obj?getPageCount() 

        
        static member setPageCount (count : int) (table : Table<'T>) : Table<'T> =
            let o = createObj [ "pageCount" ==> count ]
            Table.setOption o table
            table
        
        static member nextPage (table : Table<'T>) : Table<'T> =
            table?_obj?nextPage()
            table
        
        static member previousPage (table : Table<'T>) : Table<'T> =
            table?_obj?previousPage()
            table