using dnlib.DotNet;
using IL2CPPSDKGen.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IL2CPPSDKGen.IL2CPP
{
    public class IL2CppClassProperties
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public List<IL2CppField> Fields = new List<IL2CppField>();

        public List<IL2CppMethod> Methods = new List<IL2CppMethod>();

        public IL2CppClassProperties(TypeDef type, string ilnamespace)
        {
            Name = type.Name.ToString();
            Namespace = ilnamespace.ToString();

            int methodNumber = 0;
            int fieldNumber = 0;
            int Type1Number = 0;
            int Type2Number = 0;
            foreach (var method in type.Methods)
            {
                methodNumber++;
                Type1Number++;
                if (!GlobalUtils.BlacklistedMethodNames.Contains(method.Name.ToLower()))
                {
                    if (method.CustomAttributes.FindAll("Il2CppDummyDll.AddressAttribute").Count() > 0)
                    {
                        string MethodName = method.Name.ToString();
                        string TypeName = method.ReturnType.FullName.ToString();

                        if (!Regex.IsMatch(MethodName, "(?i)^[a-z]+$")) MethodName = $"method_{methodNumber}";
                        if (!Regex.IsMatch(TypeName, "(?i)^[a-z]+$")) TypeName = $"type_{Type1Number}";

                        var attribute = method.CustomAttributes.Find("Il2CppDummyDll.AddressAttribute");
                        var RVA = attribute.GetField("RVA").Value;
                        Methods.Add(new IL2CppMethod(TypeName.ToString(), MethodName.ToString(), RVA.ToString(), method));
                    }
                }
            }

            foreach(var field in type.Fields)
            {
                fieldNumber++;
                Type2Number++;
                if (field.CustomAttributes.FindAll("Il2CppDummyDll.FieldOffsetAttribute").Count() > 0)
                {
                    string FieldName = field.Name.ToString();
                    string TypeName = field.FieldType.FullName.ToString();
                    if (!Regex.IsMatch(FieldName, "(?i)^[a-z]+$")) FieldName = $"field_{fieldNumber}";
                    if (!Regex.IsMatch(TypeName, "(?i)^[a-z]+$")) TypeName = $"type_{Type2Number}";

                    var attribute = field.CustomAttributes.Find("Il2CppDummyDll.FieldOffsetAttribute");
                    var Offset = attribute.GetField("Offset").Value;
                    Fields.Add(new IL2CppField(TypeName.ToString(), FieldName.ToString(), Offset.ToString()));
                }
            }
        }
    }
}
