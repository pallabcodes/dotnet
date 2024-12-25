> c# interface is to be used to hold contracts i.e. methods
>
> LINQ and THREADS are skipped for now

```c#
// C# Struct Example
public struct Person
{
    public string Name;
    public int Age;

    public void Introduce()
    {
        Console.WriteLine($"Hello, my name is {Name}, and I'm {Age} years old.");
    }
}

// C# Interface Example
public interface ISpeaker
{
    void Speak();
}


```
> c# struct is to be used to contain fields/properties only
> 
> 
## .NET confusion: .NET framework (4.8) vs .Net core(3.4) vs .Net standard vs .Net vs C#

1. .NET Framework (4.8) is a platform first and foremost to run .NET environment supported languages (c#, f#, vb.net, xamarin etc.), 
2. [ISSUE]: However .NET Framework is designed specifically for Windows and cannot run natively on other operating systems.
   So,
   this framework itself not cross-platform
   (so to work, deploy and use it must be a Windows server or machine off course)   
3. [SOLUTION]: So, to bring in cross-platform, `.NET core (3.4)` is introduced
4. [Successor]: .NET core (3.4) -> next version named .NET 5 to reflect that it is the successor and defacto standard (and since .NET framework and .NET code 3.4 by skipping version 4 it made sense to refer that .NET 5 is new standard/successor for both and to make this version more readable and easy to differentiate, `Microsoft decided to name it .NET 5 dropping the core word` -> `so once again .NET 5 just successor i.e. newer version .NET core 3.4`) so off course after .NET 5 and it will .NET 6 (not .NET core 6)
5. [.NET STANDARD]: 

## What is .NET Framework and what it does?

1. **Code Writing**: Developers write code in a .NET-supported language such as **VB.NET**, **C#**, or **F#**.

2. **Compilation to Intermediate Language (IL)**:
    - The source code is compiled into **Common Intermediate Language (CIL)** (formerly known as Microsoft Intermediate Language or MSIL) by the language-specific compiler (e.g., `csc.exe` for C#).
    - The output of this compilation is an **assembly**, typically a `.dll` or `.exe` file.

3. **Execution Using Common Language Runtime (CLR)**:
    - The CLR is the runtime environment in the .NET Framework that handles execution. It provides services such as memory management, garbage collection, and exception handling.

4. **Just-In-Time (JIT) Compilation**:
    - When the application runs, the CLR uses the **Just-In-Time (JIT)** compiler to convert the CIL code into **native machine code** specific to the host operating system and processor architecture.
    - The CPU then executes this machine code.

5. **Execution and Optimization**:
    - During execution, the CLR performs optimizations like inlining and caching frequently used methods.
    - Managed code is run within the CLR, ensuring security and robustness.



## From source code to machine code

1. Code Written in a .NET Language
   Developers write code in a .NET-supported language (e.g., C#, VB.NET, F#).

2. Compilation:
   The source code is compiled by a language-specific compiler (e.g., csc for C#) into Intermediate Language (IL).
   The compiled IL is stored in an assembly, which can be either:
   .exe: A standalone executable file.
   .dll: A library file, meant to be used by other applications.
   This assembly is not machine code yet; it is platform-independent IL code.

3. Execution by the Common Language Runtime (CLR):
   When the program is executed:
   The CLR loads the assembly (.dll or .exe) containing the IL code.
   The CLR uses the Just-In-Time (JIT) Compiler to convert the IL code into native machine code specific to the operating system and CPU.
   The CPU executes the native machine code.