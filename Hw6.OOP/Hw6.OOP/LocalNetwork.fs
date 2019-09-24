﻿module Virus

/// Абстрактный класс с данными о сопротивляемости вирусам разных ОС.
[<AbstractClass>]
type Resistance() =       
    abstract member LinuxResistance: int
    abstract member MacOSResistance: int
    abstract member WindowsResistance: int
    abstract member OtherOSResistance: int

/// Стандартная сопротивляемость вирусам.
type DefaultResistance() =
    inherit Resistance()
    override this.LinuxResistance = 70
    override this.MacOSResistance = 60
    override this.WindowsResistance = 30
    override this.OtherOSResistance = 75

/// Моделирует работу локальной сети. OS: linux, windows, macos, other.
type LocalNetwork(computers : (string * string * bool)[], connections : int[,], resistance : Resistance) =         
        
    /// Получить первый элемент кортежа.
    let first (a, _, _) = a

    /// Получить второй элемент кортежа.
    let second (_, b, _) = b

    /// Получить третий элемент кортежа.
    let third (_, _, c) = c 
    
    /// Список операционных систем в локальной сети.
    let mutable _computers = 
        let mutable inner = computers
        
        Seq.iteri (fun index PC ->     
            let lowerCaseOSName = ((second PC) : string).ToLower()
            inner.[index] <- (first PC, lowerCaseOSName, third PC)) inner
        
        inner

    /// Вспомогательный массив для избежания заражения через вновь заражённых.
    let mutable isNewbie = Array.create _computers.Length false

    /// Матрица смежности соединения компьютеров в локальной сети.
    let _connections = connections

    /// Сопротивление операционных систем.
    let _resistance = resistance

    /// Длина первого измерения (отвечает за соединение с другими ПК) двумерного массива соединений.   
    let lengthOfConnections = Array2D.length1 _connections

    /// Количество компьютеров в сети.
    let lengthOfComputers = _computers.Length  

    // match dosn't see this names :/ 
    // Good refactoring practice :)
    // Linux ОС.
    let linux = "linux"

    // Windows ОС.
    let windows = "windows"

    // MacOS ОС.
    let macos = "macos"
    
    /// Попытаться заразить компьютер.
    let tryInfect indexOfPrey preyInfo = 
        // костыль. Здесь должна быть случайность!
        // if _resistance.LinuxSusceptibility = 0.0 then

        match second preyInfo with
        | "linux" -> 
            let random = System.Random()
            let bigIsDangerous = random.Next(0,100)
            if bigIsDangerous > _resistance.LinuxResistance then
                _computers.[indexOfPrey] <- (first preyInfo, second preyInfo, true)
                isNewbie.[indexOfPrey] <- true
        | "windows" ->
            let random = System.Random()
            let bigIsDangerous = random.Next(0,100)
            if bigIsDangerous > _resistance.WindowsResistance then
                _computers.[indexOfPrey] <- (first preyInfo, second preyInfo, true)
                isNewbie.[indexOfPrey] <- true
        | "macos" ->
            let random = System.Random()
            let bigIsDangerous = random.Next(0,100)
            if bigIsDangerous > _resistance.MacOSResistance then
                _computers.[indexOfPrey] <- (first preyInfo, second preyInfo, true)
                isNewbie.[indexOfPrey] <- true
        | _ ->
            let random = System.Random()
            let bigIsDangerous = random.Next(0,100)
            if bigIsDangerous > _resistance.OtherOSResistance then
                _computers.[indexOfPrey] <- (first preyInfo, second preyInfo, true)
                isNewbie.[indexOfPrey] <- true

        //_computers.[indexOfPrey] <- (first preyInfo, second preyInfo, true)
        //isNewbie.[indexOfPrey] <- true
            
    /// Найти компьютеры, заражённые с прошлой эпохи, либо с самого начала.
    let noOneIsNewbieNow () = 
        let rec loop step =
            if step = lengthOfComputers then ()
            else 
                isNewbie.[step] <- false
                loop (step + 1)

        loop 0

    /// Найти компьютеры, соединенные с заражённым.
    let checkConnections fromThisComputer =                
        let rec loop index =
            if index = lengthOfConnections then ()
            elif _connections.[fromThisComputer, index] = 1 then                
                //if isNewbie.[fromThisComputer] then loop (index + 1)
                //else                     

                // Попытаться заразить жертву:
                let preyInfo = _computers.[index]
                if third preyInfo then loop (index + 1)
                else 
                    tryInfect index preyInfo
                    
                    //_computers.[index] <- (first computerInfo, second computerInfo, true)
                    loop (index + 1)
            else loop (index + 1)
        
        loop 0        

    /// Новый этап жизни вируса в локальной сети.
    let newEpoch () = 
        //let illComputers = findOldVirus _computers
        
        let rec findPCWithVirus index = 
            if index = lengthOfComputers then ()
            else
                if third _computers.[index] && isNewbie.[index] <> true then
                    checkConnections index
                    findPCWithVirus (index + 1)
                else findPCWithVirus (index + 1)
                
        findPCWithVirus 0

        noOneIsNewbieNow ()
    
    /// Новый этап жизни вируса в локальной сети.
    member this.NewEpoch() = newEpoch ()

    /// Информация о компьютерах: имя, OS, заражён ли.
    member this.Computers =         
        _computers
    
    /// Инициализирует новый экземпляр класса LocalNetwork с сопротивлением к вирусам по умолчанию.
    new(computers : (string * string * bool)[], connections : int[,]) = LocalNetwork(computers, connections, DefaultResistance())        
        
[<EntryPoint>]
let main argv =    
    0