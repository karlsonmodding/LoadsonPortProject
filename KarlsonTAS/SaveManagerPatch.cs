using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarlsonTAS
{
    [HarmonyPatch(typeof(SaveManager), "Load")]
    class LoadPatch
    {
        static bool Prefix(SaveManager __instance)
        {
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "karlsondata.xml");
            if (!File.Exists(filepath))
                __instance.NewSave();
            else
                __instance.state = __instance.Deserialize<PlayerSave>(File.ReadAllText(filepath));
            return false;
        }
    }
    [HarmonyPatch(typeof(SaveManager), "Save")]
    class SavePatch
    {
        static bool Prefix(SaveManager __instance)
        {
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "karlsondata.xml");
            File.WriteAllText(filepath, __instance.Serialize(__instance.state));
            return false;
        }
    }
}
