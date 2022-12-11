using System;
using System.Collections.Generic;
using Loadson;
using LoadsonAPI;
using UnityEngine;
using UnityEngine.Rendering;

namespace InputDisplay
{
    public class Main : Mod
    {
        private bool init = false;
        private Texture2D tex;
        private GUIStyle style;

        private int configWindowId;
        private Rect configWindow = new Rect((Screen.width - 200) / 2, (Screen.height - 105) / 2, 200, 105);
        private bool configWindowShow = false;

        private Dictionary<string, string> prefs;

        public override void OnEnable()
        {
            configWindowId = ImGUI_WID.GetWindowId();
            prefs = Preferences.GetPreferences();
            if(!prefs.ContainsKey("enable"))
            {
                prefs.Add("enable", true.ToString());
                prefs.Add("offX", 100f.ToString());
                prefs.Add("offY", (-100f).ToString());
            }

            MenuEntry.AddMenuEntry(new List<(string, Action)>()
            {
                ("Settings", ()=> configWindowShow = true)
            }, "<size=35>Input Display</size>");
        }

        public override void OnGUI()
        {
            if(!init)
            {
                tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                tex.SetPixel(1, 1, new Color(1f, 1f, 1f, 0.2f));
                tex.Apply();
                style = new GUIStyle();
                style.normal.background = tex;
                style.alignment = TextAnchor.MiddleCenter;
                style.fontSize = 20;
                style.normal.textColor = new Color(1f, 1f, 1f, 0.5f);
                init = true;
            }

            if (configWindowShow) configWindow = GUI.Window(configWindowId, configWindow, (_) =>
            {
                if (GUI.Button(new Rect(150, 0, 50, 20), "Close")) configWindowShow = false;
                if (GUI.Button(new Rect(150, 20, 50, 20), "Reset"))
                {
                    prefs["offX"] = 100f.ToString();
                    prefs["offY"] = (-100f).ToString();
                }

                GUI.Label(new Rect(5, 25, 50, 20), "offX");
                string trailDot = "";
                if (Math.Floor(float.Parse(prefs["offX"])) == float.Parse(prefs["offX"])) trailDot = ",";
                prefs["offX"] = float.Parse(GUI.TextField(new Rect(35, 25, 100, 20), prefs["offX"] + trailDot)).ToString();
                prefs["offX"] = GUI.HorizontalSlider(new Rect(5, 50, 190, 20), float.Parse(prefs["offX"]), 0f, 200f).ToString();

                GUI.Label(new Rect(5, 65, 50, 20), "offY");
                trailDot = "";
                if (Math.Floor(float.Parse(prefs["offY"])) == float.Parse(prefs["offY"])) trailDot = ",";
                prefs["offY"] = float.Parse(GUI.TextField(new Rect(35, 65, 100, 20), prefs["offY"] + trailDot)).ToString();
                prefs["offY"] = GUI.HorizontalSlider(new Rect(5, 90, 190, 20), float.Parse(prefs["offY"]), -200f, 0f).ToString();

                GUI.DragWindow();
            }, "Input Display Settings <i> </i> <i> </i> <i> </i> <i> </i> <i> </i> <i> </i>");

            float xOffset = float.Parse(prefs["offX"]), yOffset = float.Parse(prefs["offY"]);

            GUI.Box(new Rect(120 + xOffset, Screen.height - 100 + yOffset, 40, 40), "W", style);
            GUI.Box(new Rect(80 + xOffset, Screen.height - 100 + yOffset, 40, 40), "Q", style);
            GUI.Box(new Rect(160 + xOffset, Screen.height - 100 + yOffset, 40, 40), "E", style);
            GUI.Box(new Rect(80 + xOffset, Screen.height - 60 + yOffset, 40, 40), "A", style);
            GUI.Box(new Rect(120 + xOffset, Screen.height - 60 + yOffset, 40, 40), "S", style);
            GUI.Box(new Rect(160 + xOffset, Screen.height - 60 + yOffset, 40, 40), "D", style);
            GUI.Box(new Rect(80 + xOffset, Screen.height - 20 + yOffset, 120, 40), "Jump", style);
            GUI.Box(new Rect(0 + xOffset, Screen.height - 100 + yOffset, 80, 40), "Caps", style);
            GUI.Box(new Rect(0 + xOffset, Screen.height - 60 + yOffset, 80, 40), "Shift", style);
            GUI.Box(new Rect(0 + xOffset, Screen.height - 20 + yOffset, 80, 40), "Ctrl", style);
            GUI.Box(new Rect(200 + xOffset, Screen.height - 100 + yOffset, 40, 120), "LM", style);
            GUI.Box(new Rect(240 + xOffset, Screen.height - 100 + yOffset, 40, 120), "RM", style);

            if (Input.GetKey(KeyCode.W))
                GUI.Box(new Rect(120 + xOffset, Screen.height - 100 + yOffset, 40, 40), "W", style);
            if (Input.GetKey(KeyCode.Q))
                GUI.Box(new Rect(80 + xOffset, Screen.height - 100 + yOffset, 40, 40), "Q", style);
            if (Input.GetKey(KeyCode.E))
                GUI.Box(new Rect(160 + xOffset, Screen.height - 100 + yOffset, 40, 40), "E", style);
            if (Input.GetKey(KeyCode.A))
                GUI.Box(new Rect(80 + xOffset, Screen.height - 60 + yOffset, 40, 40), "A", style);
            if (Input.GetKey(KeyCode.S))
                GUI.Box(new Rect(120 + xOffset, Screen.height - 60 + yOffset, 40, 40), "S", style);
            if (Input.GetKey(KeyCode.D))
                GUI.Box(new Rect(160 + xOffset, Screen.height - 60 + yOffset, 40, 40), "D", style);
            if (Input.GetKey(KeyCode.Space))
                GUI.Box(new Rect(80 + xOffset, Screen.height - 20 + yOffset, 120, 40), "Jump", style);
            if (Input.GetKey(KeyCode.CapsLock))
                GUI.Box(new Rect(0 + xOffset, Screen.height - 100 + yOffset, 80, 40), "Caps", style);
            if (Input.GetKey(KeyCode.LeftShift))
                GUI.Box(new Rect(0 + xOffset, Screen.height - 60 + yOffset, 80, 40), "Shift", style);
            if (Input.GetKey(KeyCode.LeftControl))
                GUI.Box(new Rect(0 + xOffset, Screen.height - 20 + yOffset, 80, 40), "Ctrl", style);
            if (Input.GetKey(KeyCode.Mouse0))
                GUI.Box(new Rect(200 + xOffset, Screen.height - 100 + yOffset, 40, 120), "LM", style);
            if (Input.GetKey(KeyCode.Mouse1))
                GUI.Box(new Rect(240 + xOffset, Screen.height - 100 + yOffset, 40, 120), "RM", style);
        }
    }
}