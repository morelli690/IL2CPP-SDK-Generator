using IL2CPPSDKGen.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IL2CPPSDKGen.IL2CPP
{
    public class IL2CppClass
    {
        private IL2CppClassProperties Properties { get; set; }

        public IL2CppClass(IL2CppClassProperties properties)
        {
            Properties = properties;
        }

        private string RemoveInvalidChars(string filename) {
            return string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
        }

        public void Save()
        {
            GlobalUtils.namespaceSaveCount++;
            GlobalUtils.PropertiesSaveCount++;

            string Name = Properties.Namespace.ToString();
            string PropertyName = Properties.Name.ToString();

            if (!Regex.IsMatch(Name, "(?i)^[a-z]+$")) 
                Name = $"namespace_{GlobalUtils.namespaceSaveCount}";

            if (!Regex.IsMatch(PropertyName, "(?i)^[a-z]+$")) 
                PropertyName = $"class_{GlobalUtils.PropertiesSaveCount}";

            Directory.CreateDirectory($"Output\\{Properties.Namespace.ToString()}\\{Name.ToString()}");

            string classPayload = GenerateClassPayload();

            Console.WriteLine($"Output\\{Properties.Namespace.ToString()}\\{Name.ToString()}\\{PropertyName}.h");
            File.WriteAllText($"Output\\{Properties.Namespace.ToString()}\\{Name.ToString()}\\{PropertyName}.h", classPayload);
        }
        private string BuildFields()
        {
            string MegaString = null;
            foreach(var field in Properties.Fields)
            {
                MegaString += field.ToString() + "\n";
            }

            return MegaString;
        }

        private string BuildMethods()
        {
            string MegaString = null;
            foreach(var method in Properties.Methods)
            {
                MegaString += method.ToString() + "\n";
            }

            return MegaString;
        }

        private string GenerateClassPayload()
        {
            StringBuilder builder = new StringBuilder();
            if (Properties.Definition.IsEnum)
            {
                //Support for enum classes
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(Properties.Definition.Fields[0].Name + " -- eNUM");
            }
            return null;
        }
    }
}
