using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IL2CPPSDKGen.SDKGen;

namespace IL2CPPSDKGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "IL2CPP SDK Generator";
            Console.WriteLine("[LOG] Looking for files to convert...");
            if (Directory.GetFiles("Input").Length == 0)
            {
                Console.WriteLine("[LOG] Could not find any files to convert. Make sure you put all the dumped dlls inside the Input folder.");
            }
            else
            {
                foreach(var file in Directory.GetFiles("Input"))
                {
                    SDKGenerator.OpenModule(file);
                }

                string helperFile = $"#include <Windows.h>\n\nstruct IL2Helper " + "{\n\n" + $"static DWORD64 GetModuleBaseAddress() " + "{\nreturn (DWORD64)GetModuleHandle(L\"GameAssembly.dll\");\n}\ntemplate<class T>\n" + $"static T* FindFunction(DWORD64 offset) " + "{\nreturn (T*)(GetModuleBaseAddress() + offset);\n}\n\n};";
                File.WriteAllText("Output\\Helper.h", helperFile);
                Console.WriteLine("[LOG] Created Helper.h File");
                Console.WriteLine("You may now close the sdk generator.");
            }
            Console.ReadLine();
        }
    }
}
