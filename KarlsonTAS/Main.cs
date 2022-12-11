using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Loadson;
using LoadsonAPI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KarlsonTAS
{
    public class Main : Mod
    {
        public override void OnEnable()
        {
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("KarlsonTAS");
            try
            {
                harmony.PatchAll();
            }
            catch(System.Exception e)
            {
                Console.Log(e.ToString());
            }

            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "savestates"));
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.buildIndex > 1)
            {
                Coroutines.StartCoroutine(InitDebug());
                if (bypassSetObjArray) bypassSetObjArray = false;
                else SetObjArray();
                player = UnityEngine.Object.FindObjectOfType<PlayerMovement>().transform;
                UnityEngine.Object.Destroy(SlowmoEffect.Instance);
            }
        }

        public override void Update(float _)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                gameSpeed += 1;
                if (gameSpeed > 254) gameSpeed = 254;
                if (Time.timeScale != 0f)
                {
                    Time.timeScale = gameSpeed / 100f;
                }
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                gameSpeed -= 1;
                if (gameSpeed < 1) gameSpeed = 1;
                if (Time.timeScale != 0f)
                {
                    Time.timeScale = gameSpeed / 100f;
                }
            }
        }

        static void SetObjArray()
        {
            allMovables = new GameObject[UnityEngine.Object.FindObjectsOfType<Rigidbody>().Length];
            int counter = 0;
            foreach (Rigidbody r in UnityEngine.Object.FindObjectsOfType<Rigidbody>())
            {
                if (r.gameObject.CompareTag("Gun"))
                {
                    continue;
                }
                allMovables[counter] = r.gameObject;
                counter++;
            }
            allGuns = GameObject.FindGameObjectsWithTag("Gun");
            allEnemies = UnityEngine.Object.FindObjectsOfType<Enemy>();
        }

        private static Transform[] temp;
        public static void Reload()
        { // weirdass shenanigan that fixes the glitch where keys are not registered in scene transition for savestates
            temp = UnityEngine.Object.FindObjectsOfType(typeof(Transform)) as Transform[];
            foreach (Transform t in temp)
            {
                if (t.gameObject.scene.buildIndex == SceneManager.GetActiveScene().buildIndex) UnityEngine.Object.Destroy(t.gameObject);
            }
            bypassSetObjArray = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Additive);
        }

        IEnumerator InitDebug()
        {
            yield return null;
            Debug d = UnityEngine.Object.FindObjectOfType<Debug>();
            typeof(Debug).GetField("speedOn", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(d, true);
            typeof(Debug).GetField("fpsOn", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(d, true);
            GameState.Instance.SetSlowmo(false);
        }

        public static string VersionToString(float version)
        {
            string foo = (version % 10).ToString("f1");
            string bar = Mathf.Floor(version / 10f).ToString();
            return bar + "." + foo;
        }

        static int PatchToInt(float ver)
        {
            while (ver % 1 > Mathf.Floor(ver % 1))
            {
                ver *= 10;
            }
            return (int)ver;
        }

        static bool VersionCheck(float ver)
        {
            if (Mathf.RoundToInt(ver) == Mathf.RoundToInt(version) & (PatchToInt(ver) == PatchToInt(version) | PatchToInt(ver) == PatchToInt(version) - 1)) return false;
            else return true;
        }

        public static void SetSaveState()
        {
            backup = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "savestates", "savestate" + stateSlot + ".ini"));
            IniFile ini = new IniFile(Path.Combine(Directory.GetCurrentDirectory(), "savestates", "savestate" + stateSlot + ".ini"));
            PlayerMovement plr = UnityEngine.Object.FindObjectOfType<PlayerMovement>();
            Vector3 position = plr.gameObject.transform.position;
            Vector3 vel = plr.gameObject.GetComponent<Rigidbody>().velocity;
            string section = "player";
            ini.SetFloat(section, "Version", version);
            ini.SetString(section, "Level", SceneManager.GetActiveScene().name);
            ini.SetFloat(section, "Time", Timer.Instance.GetTimer());
            ini.SetFloat(section, "PlayerX", position.x);
            ini.SetFloat(section, "PlayerY", position.y);
            ini.SetFloat(section, "PlayerZ", position.z);
            ini.SetFloat(section, "VelocityX", vel.x);
            ini.SetFloat(section, "VelocityY", vel.y);
            ini.SetFloat(section, "VelocityZ", vel.z);
            ini.SetFloat(section, "CameraX", plr.playerCam.rotation.eulerAngles.x);
            ini.SetFloat(section, "CameraY", plr.playerCam.rotation.eulerAngles.y);
            ini.SetFloat(section, "CameraZ", plr.playerCam.rotation.eulerAngles.z);
            // xrotation might have some slight difference from the camera x which i'm not fully aware of
            FieldInfo internalX = typeof(PlayerMovement).GetField("xRotation", BindingFlags.NonPublic | BindingFlags.Instance);
            ini.SetFloat(section, "InternalCameraX", (float)internalX.GetValue(plr));
            FieldInfo gun = typeof(DetectWeapons).GetField("gun", BindingFlags.NonPublic | BindingFlags.Instance);
            int gunIndex = -1;
            DetectWeapons DW = UnityEngine.Object.FindObjectOfType<DetectWeapons>();
            for (int i = 0; i < allGuns.Length; i++)
            {
                if (allGuns[i] == (GameObject)gun.GetValue(DW)) gunIndex = i;
            }
            if (SceneManager.GetActiveScene().name == "10Sky2" || SceneManager.GetActiveScene().name == "8Sky0") gunIndex = -1;
            ini.SetInt("Player", "WeaponId", gunIndex);
            for (int i = 0; i < allMovables.Length; i++)
            {
                if (allMovables[i] == plr.gameObject) continue;
                section = "obj_" + i.ToString();
                ini.SetBool(section, "isDestroyed", allMovables[i] == null);
                if (allMovables[i] == null) continue;
                ini.SetFloat(section, "PositionX", allMovables[i].transform.position.x);
                ini.SetFloat(section, "PositionY", allMovables[i].transform.position.y);
                ini.SetFloat(section, "PositionZ", allMovables[i].transform.position.z);
                ini.SetFloat(section, "RotationX", allMovables[i].transform.eulerAngles.x);
                ini.SetFloat(section, "RotationY", allMovables[i].transform.eulerAngles.y);
                ini.SetFloat(section, "RotationZ", allMovables[i].transform.eulerAngles.z);
                if (allMovables[i] == (GameObject)gun.GetValue(UnityEngine.Object.FindObjectOfType<DetectWeapons>())) gunIndex = i;
                Rigidbody rb = allMovables[i].GetComponent<Rigidbody>();
                if (rb == null) continue;
                ini.SetFloat(section, "VelocityX", rb.velocity.x);
                ini.SetFloat(section, "VelocityY", rb.velocity.y);
                ini.SetFloat(section, "VelocityZ", rb.velocity.z);
            }
            for (int i = 0; i < allEnemies.Length; i++) //TODO: complete enemy saving
            {
                if (allEnemies[i].gameObject == null) continue;
                section = "enemy_" + i.ToString();
                ini.SetFloat(section, "PositionX", allEnemies[i].gameObject.transform.position.x);
                ini.SetFloat(section, "PositionY", allEnemies[i].gameObject.transform.position.y);
                ini.SetFloat(section, "PositionZ", allEnemies[i].gameObject.transform.position.z);
                ini.SetFloat(section, "Rotation", allEnemies[i].gameObject.transform.eulerAngles.y);
                ini.SetBool(section, "isDead", allEnemies[i].IsDead());
            }
            if (SceneManager.GetActiveScene().buildIndex == 12)
            {
                section = "Sky2";
                Transform washingMachine = UnityEngine.Object.FindObjectOfType<RotateObject>().gameObject.transform;
                ini.SetFloat(section, "RotationX", washingMachine.eulerAngles.x);
                ini.SetFloat(section, "RotationYZ", washingMachine.eulerAngles.y);
            }
            Console.Log("State saved");
        }

        public static IEnumerator GetSaveState()
        {
            //Time.timeScale = 0f;
            IniFile ini = new IniFile(Path.Combine(Directory.GetCurrentDirectory(), "savestates", "savestate" + stateSlot + ".ini"));
            string section = "player";
            if (ini.GetString(section, "Level") != SceneManager.GetActiveScene().name)
            {
                Console.Log("<color=red>This savestate is not for this level!</color>");
                yield break;
            }
            else if (VersionCheck(ini.GetFloat(section, "Version")))
            {
                Console.Log("<color=red>This savestate is for version " + VersionToString(ini.GetFloat(section, "Version")) + " of Karlson TAS!\nYou're running version " + VersionToString(version) + "</color>");
                yield break;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            yield return null;
            SetObjArray();
            yield return null;
            PlayerMovement plr = UnityEngine.Object.FindObjectOfType<PlayerMovement>();
            Transform transform = plr.gameObject.transform;
            float X = ini.GetFloat(section, "PlayerX");
            float Y = ini.GetFloat(section, "PlayerY");
            float Z = ini.GetFloat(section, "PlayerZ");
            transform.position = new Vector3(X, Y, Z);
            X = ini.GetFloat(section, "CameraX");
            Y = ini.GetFloat(section, "CameraY");
            Z = ini.GetFloat(section, "CameraZ");
            plr.playerCam.eulerAngles = new Vector3(X, Y, Z);
            FieldInfo internalX = typeof(PlayerMovement).GetField("xRotation", BindingFlags.NonPublic | BindingFlags.Instance);
            internalX.SetValue(plr, ini.GetFloat(section, "InternalCameraX"));
            plr.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(ini.GetFloat(section, "VelocityX"), ini.GetFloat(section, "VelocityY"), ini.GetFloat(section, "VelocityZ"));
            X = ini.GetInt(section, "WeaponId");
            if (X >= 0)
            {
                DetectWeapons dw = UnityEngine.Object.FindObjectOfType<DetectWeapons>();
                dw.ForcePickup(allGuns[(int)X]);
                GameObject gun = allGuns[(int)X];
                //gun.transform.SetParent(null);
            }
            if (ini.GetString(section, "Level").Equals("10Sky2")) // washing machine
            {
                UnityEngine.Object.FindObjectOfType<RotateObject>().gameObject.transform.eulerAngles = new Vector3(
                    ini.GetFloat("Sky2", "RotationX"),
                    ini.GetFloat("Sky2", "RotationYZ"),
                    ini.GetFloat("Sky2", "RotationYZ"));
            }
            for (int i = 0; i < allMovables.Length; i++)
            {
                if (allMovables[i] == plr.gameObject) continue;
                section = "obj_" + i.ToString();
                if (ini.GetBool(section, "isDestroyed"))
                {
                    UnityEngine.Object.Destroy(allMovables[i]);
                    continue;
                }
                X = ini.GetFloat(section, "PositionX");
                Y = ini.GetFloat(section, "PositionY");
                Z = ini.GetFloat(section, "PositionZ");
                allMovables[i].transform.position = new Vector3(X, Y, Z);
                X = ini.GetFloat(section, "RotationX");
                Y = ini.GetFloat(section, "RotationY");
                Z = ini.GetFloat(section, "RotationZ");
                allMovables[i].transform.eulerAngles = new Vector3(X, Y, Z);
                X = ini.GetFloat(section, "VelocityX");
                Y = ini.GetFloat(section, "VelocityY");
                Z = ini.GetFloat(section, "VelocityZ");
                allMovables[i].GetComponent<Rigidbody>().velocity = new Vector3(X, Y, Z);
            }
            FieldInfo time = typeof(Timer).GetField("timer", BindingFlags.NonPublic | BindingFlags.Instance);
            time.SetValue(Timer.Instance, ini.GetFloat("player", "Time"));
            for (int i = 0; i < allEnemies.Length; i++)
            {
                section = "enemy_" + i;
                X = ini.GetFloat(section, "PositionX");
                Y = ini.GetFloat(section, "PositionY");
                Z = ini.GetFloat(section, "PositionZ");
                allEnemies[i].transform.position = new Vector3(X, Y, Z);
                allEnemies[i].transform.eulerAngles = new Vector3(
                    allEnemies[i].transform.eulerAngles.x,
                    ini.GetFloat(section, "Rotation"),
                    allEnemies[i].transform.eulerAngles.z);
                if (ini.GetBool(section, "isDead"))
                {
                    allEnemies[i].GetComponent<RagdollController>().MakeRagdoll(Vector3.zero);
                    allEnemies[i].DropGun(Vector3.zero);
                }
            }
            Console.Log("State loaded");
        }

        public static byte gameSpeed = 100;
        public const float version = 2.0f;
        public static Transform player;
        public static byte stateSlot = 0;
        public static string backup;
        static GameObject[] allMovables;
        static Enemy[] allEnemies;
        static GameObject[] allGuns;
        static bool bypassSetObjArray;
        public static bool displayInput = false;
        public static Vector3 velocity, cameraRotation;
    }

    public struct KeyBind
    {
        public char key;
        public string command;
    }
}
