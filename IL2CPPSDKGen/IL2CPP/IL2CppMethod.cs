using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IL2CPPSDKGen.IL2CPP
{
    public class IL2CppMethod
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string RVA { get; set; }

        public List<Dictionary<string, string>> Parameters = new List<Dictionary<string, string>>();

        private string BuildParameters()
        {
            string MegaString = null;
            for(int i = 0; i < Parameters.Count(); i++)
            {
                if (i == Parameters.Count() - 1)
                {
                    foreach (Dictionary<string, string> dictionary in Parameters)
                    {
                        foreach (var type in dictionary.Keys)
                        {
                            string name = null;
                            if (dictionary.TryGetValue(type, out name))
                            {
                                MegaString += $"{type}* {name}";
                            }
                        }
                    }
                }
                else
                {
                    foreach (Dictionary<string, string> dictionary in Parameters)
                    {
                        foreach (var type in dictionary.Keys)
                        {
                            string name = null;
                            if (dictionary.TryGetValue(type, out name))
                            {
                                MegaString += $"{type}* {name}, ";
                            }
                        }
                    }
                }
            }

            return MegaString;
        }

        public override string ToString()
        {
            var Parameters = BuildParameters();
            return $"typedef {Type} __stdcall {Name}_Method({Parameters});\n\nstatic {Type}* {Name}({Parameters})\n" + "{" + $"\n{Name}_Method* method = IL2Helper::FindFunction<{Name}_Method>({RVA});\nmethod();";
        }

        public IL2CppMethod(string type, string name, string rva, MethodDef def)
        {
            Type = type;
            Name = name;
            RVA = rva;

            var parameterCount = 0;
            foreach(var parameter in def.Parameters)
            {
                parameterCount++;
                string ParameterTypeName = parameter.Type.FullName.ToString();
                string ParameterName = parameter.Name.ToString();

                if (!Regex.IsMatch(ParameterTypeName, "(?i)^[a-z]+$")) ParameterTypeName = $"type_{parameterCount}";
                if (!Regex.IsMatch(ParameterName, "(?i)^[a-z]+$")) ParameterName = $"parameter_{parameterCount}";

                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add(ParameterTypeName.ToString(), ParameterName.ToString());

                Parameters.Add(dict);
            }
        }
    }
}
