using Loadson;

namespace BoomerMod
{
    class CustomSave
    {
        public static float[] times;
        public static void Load()
        {
            if (!Main.prefs.ContainsKey("times"))
            {
                times = new float[11] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
                Main.prefs.Add("times", SaveManager.Instance.Serialize(times));
            }
            times = SaveManager.Instance.Deserialize<float[]>(Main.prefs["times"]);
        }
        public static void Save()
        {
            Main.prefs["times"] = SaveManager.Instance.Serialize(times);
        }
    }
}
