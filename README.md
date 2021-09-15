# SIMPLE_METHOD_REPLACER
.NET SIMPLE METHOD REPLACER

How to Patch / Detour / Hook .NET managed code using DLL injection

this code includes CLR wrapper in order to be able to inject our DLL into other managed process using common Injectors Like Extreme Injector 3

Just build and place TestCLR.DLL and Test.DLL in the same folder , Lunch Extreme Injector 3 , type in the name of the process , add TestCLR.DLL and inject it .

Trick is to include the file that includes the original Types/Classes as a "Resource" in your managed DLL file you intend to inject

Please star this repository if you found it useful

HOOKME.EXE is the target process which loads the target DLL in which the target method/function "Add" is defined

HOOKMEDLL.dll is the target DLL in which the target method/function "Add" is defined

TestCLR.DLL is the DLL we use to attack the target process through injection , i.e. load it into Extreme Injector

TestDLL.DLL is the assembly in which we define our modified method/function

Enjoy
