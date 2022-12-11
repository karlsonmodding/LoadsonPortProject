using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Loadson;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace BoomerMod
{
    [HarmonyPatch(typeof(PlayerMovement), "Start")]
    public class PlayerMovement_Start
    {
        public static bool Prefix(PlayerMovement __instance)
        {
            __instance.spawnWeapon = LoadsonAPI.PrefabManager.NewBoomer();
            return true;
        }
    }

    [HarmonyPatch(typeof(Game), "Win")]
    public class Game_Win
    {
        public static bool Prefix(Game __instance)
        {
            __instance.playing = false;
            Timer.Instance.Stop();
            Time.timeScale = 0.05f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManger.Instance.WinUI(true);
            float timer = Timer.Instance.GetTimer();
            int num = int.Parse(SceneManager.GetActiveScene().name[0].ToString() ?? "");
            int num2;
            if (int.TryParse(SceneManager.GetActiveScene().name.Substring(0, 2) ?? "", out num2))
            {
                num = num2;
            }
            float num3 = CustomSave.times[num];
            if (timer < num3 || num3 == 0f)
            {
                CustomSave.times[num] = timer;
                CustomSave.Save();
            }
            MonoBehaviour.print("time has been saved as: " + Timer.Instance.GetFormattedTime(timer) + " on BoomerModSave");
            __instance.done = true;
            return false;
        }
    }



    [HarmonyPatch(typeof(Lobby), "Start")]
    public class Lobby_Start
    {
        public static void Postfix()
        {
            CustomSave.Load();
            LoadsonAPI.Coroutines.StartCoroutine(MainMenu());
        }

        public static readonly string[] levelNames = new string[] { "Tutorial", "Sandbox0", "Sandbox1", "Sandbox2", "Escape0", "Escape1", "Escape2", "Escape3", "Sky0", "Sky1", "Sky2" };

        public static IEnumerator MainMenu()
        {
            yield return new WaitForEndOfFrame();
            GameObject playSection = GameObject.Find("/UI").transform.Find("Play").gameObject;
            playSection.SetActive(true);
            int i = 0;
            foreach (var s in levelNames)
            {
                var parent = playSection.transform.Find(s);
                var text = UnityEngine.Object.Instantiate(parent.Find("Text (TMP) (1)").gameObject, parent);
                text.transform.position = new Vector3(7.829f, 16.419f, 191.500f);
                text.transform.localPosition = new Vector3(0f, -42.949f, 0.017f);
                text.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                text.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                var tmp = text.GetComponent<TMPro.TextMeshProUGUI>();
                tmp.text = $"<size=20>[BM] " + (CustomSave.times[i] == 0 ? "[NO RECORD]" : Timer.Instance.GetFormattedTime(CustomSave.times[i]));
                tmp.fontStyle = TMPro.FontStyles.Normal;
                i++;
            }
            playSection.SetActive(false);
        }
    }
}
