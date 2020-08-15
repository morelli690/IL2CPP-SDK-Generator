using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL2CPPSDKGen.IL2CPP
{
    public class IL2CppField
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Offset { get; set; }

        public FieldDef Definition { get; set; }

        public override string ToString()
        {
            return $"{Type}* {Name}; //Field Offset: {Offset}";    
        }

        public IL2CppField(string type, string name, string offset, FieldDef def)
        {
            Type = type;
            Name = name;
            Offset = offset;
            Definition = def;
        }
    }
}
