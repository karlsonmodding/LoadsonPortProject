using System;
using System.Collections.Generic;
using System.Reflection;
using Loadson;
using LoadsonAPI;
using UnityEngine;

namespace BoomerMod
{
    public class Main : Mod
    {
        public override void OnEnable()
        {
            prefs = Preferences.GetPreferences();

            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("BoomerMod");
            harmony.PatchAll(Assembly.GetAssembly(typeof(Main)));
        }

        public static Dictionary<string, string> prefs;
    }
}
