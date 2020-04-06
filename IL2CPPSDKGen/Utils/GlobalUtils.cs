using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL2CPPSDKGen.Utils
{
    public static class GlobalUtils
    {
        public static int namespaceSaveCount = 0;

        public static int PropertiesSaveCount = 0;

        public static List<string> BlacklistedMethodNames = new List<string>()
        {
            "invoke",
            ".ctor",
            ".cctor",
            "begininvoke",
            "endinvoke",
            "system.idisposable.dispose",
            "movenext",
            "system.collections.generic.ienumerator<system.object>.get_current",
            "system.collections.ienumerator.reset",
            "system.collections.ienumerator.get_current"
        };
    }
}
