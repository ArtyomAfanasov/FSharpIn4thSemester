﻿namespace Hw5.Generics.Tests

open NUnit.Framework
open FsUnit
open PhonebookErrors
open PhonebookLogic

[<TestFixture>]
type PhonebookTestClass () =        
    [<Test>]
    member this.``Test should add records (name and phone).`` () =        
        let phones = addRecord "name" "phone" Map.empty
        let updatedPhones = addRecord "name_2" "phone_2" phones

        Map.find "name" phones |> should equal "phone"
        Map.find "name_2" updatedPhones |> should equal "phone_2"
    
    [<Test>]
    member this.``Test should find phone by name.`` () =
        let phones = addRecord "name" "phone" Map.empty
        let updatedPhones = addRecord "name_2" "phone_2" phones
        
        Option.get (findPhoneByName "name" updatedPhones) |> should equal "phone"
        Option.get (findPhoneByName "name_2" updatedPhones) |> should equal "phone_2"        
    
    [<Test>]
    member this.``Test should not find phone by not existed name.`` () =
        let phones = addRecord "name" "phone" Map.empty
        
        findPhoneByName "notExistedName" phones |> should equal None //keyNotFoundError    
    
    [<Test>]
    member this.``Test should not find phone in empty phonebook.`` () =
        findPhoneByName "notExistedName" Map.empty |> should equal  None // keyNotFoundError    
    
    [<Test>]
    member this.``Test should not find name by phone.`` () =
        let phones = addRecord "name" "phone" Map.empty
        let updatedPhones = addRecord "name_2" "phone_2" phones

        Option.get (findNameByPhone "phone" updatedPhones) |> should equal "name"
        Option.get (findNameByPhone "phone_2" updatedPhones) |> should equal "name_2"
    
    [<Test>]
    member this.``Test should not find name by not existed phone.`` () =
        let phones = addRecord "name" "phone" Map.empty
        findNameByPhone "notExistedPhone" phones |> should equal None // keyNotFoundError
    
    [<Test>]
    member this.``Test should not find name in empty phonebook.`` () =
        findNameByPhone "notExistedName" Map.empty |> should equal None //keyNotFoundError    
    
    [<Test>]
    member this.``Test should correct normalize phone.`` () =
        normalizePhone "+7(999)-888-77-66" |> should equal "79998887766"    
    
    [<Test>]
    member this.``Test should fail on incorrect phone name while normalize.`` () =
        (fun () -> normalizePhone "987incorrectPhone123" |> ignore) |> should throw typeof<System.Exception>     