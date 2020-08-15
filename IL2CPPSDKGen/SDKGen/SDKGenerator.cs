using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IL2CPPSDKGen.IL2CPP;
using IL2CPPSDKGen.Utils;
using System.IO;
using System.Text.RegularExpressions;

namespace IL2CPPSDKGen.SDKGen
{
    public static class SDKGenerator
    {

        private static List<IL2CppClass> Classes = new List<IL2CppClass>();

        public static void OpenModule(string path)
        {
            try
            {
                var module = ModuleDefMD.Load(path);
                Console.WriteLine($"[LOG] Opened file: {path} for module usage.");
                if (HandleRVA(module))
                {
                    Console.WriteLine("[LOG] Handled RVA and Field Offsets.");
                    if (HandleClasses(Classes))
                    {
                        Console.WriteLine($"[LOG] Saved Classes in {path} || Clearing RVA Cache");

                        if (ClearRVA())
                            Console.WriteLine("[LOG] Cleared RVA Cache on " + path);
                    }
                }
            }
            catch(Exception)
            {
                Console.WriteLine($"[LOG] There was an error opening a module for usage. Please make sure you're actually passing a path to a valid file to use. See file: {path}");
            }
        }

        public static bool HandleRVA(ModuleDefMD module)
        {
            if (module.GetTypes().Count() == 0) return false;
            int NamespaceCount = 0;
            foreach(var type in module.GetTypes())
            {
                NamespaceCount++;
                string NamespaceName = type.Namespace.ToString();
                if (type.HasMethods)
                {
                    if (!Regex.IsMatch(type.Namespace, "(?i)^[a-z]+$"))
                        NamespaceName = $"namespace_{NamespaceCount}";

                    Classes.Add(new IL2CppClass(new IL2CppClassProperties(type, NamespaceName)));
                }
            }

            return true;
        }

        public static bool ClearRVA()
        {
            Classes.Clear();
            return Classes.Count() == 0;
        }

        public static bool HandleClasses(List<IL2CppClass> Classes)
        {
            try
            {
                foreach (var meme in Classes)
                    meme.Save();

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("[LOG] Failed to Save Classes.");
                return false;
            }
        }
    }
}
