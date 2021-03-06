namespace Hw2.Tests

open NUnit.Framework
open FsUnit
open FirstEntry

[<TestFixture>]
type FirstEntryTestClass () =
    
    [<Test>]
    member this.``FirstEntry should complete with 4 for data: 5 in [ 1; 2; 3; 4; 5; 6 ]`` () =
        (firstEntry [ 1; 2; 3; 4; 5; 6 ] 5).Value |> should equal 4
    
    [<Test>]
    member this.``FirstEntry should complete with 0 for data: 5 in [ 5; 4; 3 ]`` () =
        (firstEntry [ 5; 4; 3 ] 5).Value |> should equal 0
    
    [<Test>]
    member this.``FirstEntry should complete with 5 for data: 5 in [ 0; 1; 2; 3; 4; 5 ]`` () = 
        (firstEntry [ 0; 1; 2; 3; 4; 5 ] 5).Value |> should equal 5
    
    [<Test>]
    member this.``FirstEntry should complete with exception for data: 5 in []`` () =
        firstEntry [] 5 |> should equal None        
    
    [<Test>]
    member this.``FirstEntry should complete with exception for data: 5 in [ 1; 2; 3 ]`` () =
        firstEntry [ 1; 2; 3 ] 5 |> should equal None