using System;
using System.Collections.Generic;
using Loadson;
using LoadsonAPI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CustomCrosshair
{
    public class Main : Mod
    {
        private Dictionary<string, string> prefs;

        public override void OnEnable()
        {
            prefs = Preferences.GetPreferences();
            if (!prefs.ContainsKey("enable"))
            {
                prefs.Add("enable", true.ToString());
                prefs.Add("Dot Toggle", false.ToString());
                prefs.Add("Outer Toggle", true.ToString());
                prefs.Add("Dot Color R", 1f.ToString());
                prefs.Add("Dot Color G", 1f.ToString());
                prefs.Add("Dot Color B", 1f.ToString());
                prefs.Add("Outer Color R", 1f.ToString());
                prefs.Add("Outer Color G", 1f.ToString());
                prefs.Add("Outer Color B", 1f.ToString());
            }

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            currentLevel = arg0.buildIndex;
            bool flag = currentLevel == 1;
            if (flag)
            {
                Managers = GameObject.Find("Managers (1)");
                deadUI = Managers.transform.Find("UI/Game/DeadUI");
                crosshair = Managers.transform.Find("UI/Game/Crosshair").gameObject;
                LoadButtons();
                LoadCrosshairs();
            }
        }

        public void LoadCrosshairs()
        {
            crosshairDot.SetActive(bool.Parse(prefs["Dot Toggle"]));
            foreach (object obj in crosshair.transform)
            {
                Transform transform = (Transform)obj;
                if (transform.name != "Crosshair Dot")
                {
                    transform.gameObject.SetActive(bool.Parse(prefs["Outer Toggle"]));
                }
            }
            dotColorROBJ.GetComponent<Slider>().value = float.Parse(prefs["Dot Color R"]);
            dotColorGOBJ.GetComponent<Slider>().value = float.Parse(prefs["Dot Color G"]);
            dotColorBOBJ.GetComponent<Slider>().value = float.Parse(prefs["Dot Color B"]);
            outerColorROBJ.GetComponent<Slider>().value = float.Parse(prefs["Outer Color R"]);
            outerColorGOBJ.GetComponent<Slider>().value = float.Parse(prefs["Outer Color G"]);
            outerColorBOBJ.GetComponent<Slider>().value = float.Parse(prefs["Outer Color B"]);
        }

        public void LoadButtons()
        {
            bool flag = !deadUI.transform.Find("CROSSHAIRPAGE");
            if (flag)
            {
                CreatePages();
                crosshairMenuButton = ButtonAPI.CreateNewButton("Crosshair\nSettings", "OpenCrosshairSettings", deadUI, -1.9f, -0.9f, Vector3.one, delegate
                {
                    deadUI.transform.Find("MenuBtn").gameObject.SetActive(false);
                    deadUI.transform.Find("Retrybtn").gameObject.SetActive(false);
                    if (deadUI.transform.Find("MODS"))
                    {
                        deadUI.transform.Find("MODS").gameObject.SetActive(false);
                    }
                    crosshairPage.SetActive(true);
                    crosshairMenuButton.SetActive(false);
                });
                ButtonAPI.CreateNewButton("BACK", "CROSSHAIRBACKBUTTON", crosshairPage.transform, 0f, -1f, new Vector3(1.25f, 1.25f, 1.25f), delegate
                {
                    deadUI.transform.Find("MenuBtn").gameObject.SetActive(true);
                    deadUI.transform.Find("Retrybtn").gameObject.SetActive(true);
                    crosshairPage.gameObject.SetActive(false);
                    crosshairMenuButton.SetActive(true);
                    if (deadUI.transform.Find("MODS"))
                    {
                        deadUI.transform.Find("MODS").gameObject.SetActive(true);
                    }
                    prefs["Outer Color R"] = outerColorR.ToString();
                    prefs["Outer Color G"] = outerColorG.ToString();
                    prefs["Outer Color B"] = outerColorB.ToString();
                    prefs["Dot Color R"] = dotColorR.ToString();
                    prefs["Dot Color G"] = dotColorG.ToString();
                    prefs["Dot Color B"] = dotColorB.ToString();
                });
                crosshairDotToggle = ButtonAPI.CreateNewButton(bool.Parse(prefs["Dot Toggle"]) ? "Disable Dot" : "Enable Dot", "DOTTOGGLE", crosshairPage.transform, -0.5f, 2f, new Vector3(1.25f, 1.25f, 1.25f), delegate
                {
                    crosshairDot.SetActive(!crosshairDot.activeInHierarchy);
                    crosshairDotToggle.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = (crosshairDot.activeInHierarchy ? "Disable Dot" : "Enable Dot");
                    prefs["Dot Toggle"] = crosshairDot.activeInHierarchy.ToString();
                });
                crosshairOuterToggle = ButtonAPI.CreateNewButton(bool.Parse(prefs["Outer Toggle"]) ? "Disable Outer" : "Enable Outer", "OUTERTOGGLE", crosshairPage.transform, 0.5f, 2f, new Vector3(1.35f, 1.35f, 1.35f), delegate
                {
                    foreach (object obj in crosshair.transform)
                    {
                        Transform transform = (Transform)obj;
                        bool flag2 = transform.name != "Crosshair Dot";
                        if (flag2)
                        {
                            transform.gameObject.SetActive(!transform.gameObject.activeInHierarchy);
                            crosshairOuterToggle.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = (transform.gameObject.activeInHierarchy ? "Disable Outer" : "Enable Outer");
                        }
                    }
                    prefs["Outer Toggle"] = crosshair.transform.Find("Crosshair (5)").gameObject.activeInHierarchy.ToString();
                });
                dotColorROBJ = ButtonAPI.MakeSlider("Dot R: ", "DOTREDR", 0f, 1f, Color.red, -0.5f, 0f, new Vector3(0.7f, 0.7f, 0f), crosshairPage.transform);
                dotColorGOBJ = ButtonAPI.MakeSlider("Dot G: ", "DOTREDG", 0f, 1f, Color.green, -0.5f, -1f, new Vector3(0.7f, 0.7f, 0f), crosshairPage.transform);
                dotColorBOBJ = ButtonAPI.MakeSlider("Dot B: ", "DOTREDB", 0f, 1f, Color.cyan, -0.5f, -2f, new Vector3(0.7f, 0.7f, 0f), crosshairPage.transform);
                outerColorROBJ = ButtonAPI.MakeSlider("Outer R: ", "OuterREDR", 0f, 1f, Color.red, 0.3f, 0f, new Vector3(0.7f, 0.7f, 0f), crosshairPage.transform);
                outerColorGOBJ = ButtonAPI.MakeSlider("Outer G: ", "OuterTREDG", 0f, 1f, Color.green, 0.3f, -1f, new Vector3(0.7f, 0.7f, 0f), crosshairPage.transform);
                outerColorBOBJ = ButtonAPI.MakeSlider("Outer B: ", "OuterREDB", 0f, 1f, Color.cyan, 0.3f, -2f, new Vector3(0.7f, 0.7f, 0f), crosshairPage.transform);
            }
        }

        public void CreatePages()
        {
            bool flag = !deadUI.transform.Find("CROSSHAIRPAGE");
            if (flag)
            {
                crosshairPage = new GameObject("CROSSHAIRPAGE");
                crosshairPage.transform.parent = deadUI;
                crosshairPage.transform.localPosition = new Vector3(0f, 40f, 0f);
                crosshairPage.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                crosshairPage.SetActive(false);
                crosshairDot = UnityEngine.Object.Instantiate<GameObject>(crosshair.transform.Find("Crosshair (4)").gameObject, crosshair.transform);
                crosshairDot.name = "Crosshair Dot";
                crosshairDot.transform.localPosition = Vector3.zero;
                crosshairDot.transform.localScale = new Vector3(0.006f, 0.006f, 0f);
                crosshairDot.SetActive(bool.Parse(prefs["Dot Toggle"]));
            }
        }

        public override void Update(float deltaTime)
        {
            bool flag = currentLevel > 1;
            if (flag)
            {
                dotColorR = dotColorROBJ.GetComponent<Slider>().value;
                dotColorG = dotColorGOBJ.GetComponent<Slider>().value;
                dotColorB = dotColorBOBJ.GetComponent<Slider>().value;
                outerColorR = outerColorROBJ.GetComponent<Slider>().value;
                outerColorG = outerColorGOBJ.GetComponent<Slider>().value;
                outerColorB = outerColorBOBJ.GetComponent<Slider>().value;
                Image image = crosshairDot.GetComponent<Image>();
                image.color = new Color(dotColorR, dotColorG, dotColorB, 1f);
                foreach (object obj in crosshair.transform)
                {
                    Transform transform = (Transform)obj;
                    if (transform.name != "Crosshair Dot")
                        transform.GetComponent<Image>().color = new Color(outerColorR, outerColorG, outerColorB, 1f);
                }
            }
        }

        private int currentLevel = 0;
        private GameObject Managers;
        private Transform deadUI;
        private GameObject crosshairPage;
        private GameObject crosshair;
        private GameObject crosshairMenuButton;
        private GameObject crosshairDot;
        private GameObject crosshairDotToggle;
        private GameObject crosshairOuterToggle;
        private float dotColorR;
        private float dotColorG;
        private float dotColorB;
        private GameObject dotColorROBJ;
        private GameObject dotColorGOBJ;
        private GameObject dotColorBOBJ;
        private float outerColorR;
        private float outerColorG;
        private float outerColorB;
        private GameObject outerColorROBJ;
        private GameObject outerColorGOBJ;
        private GameObject outerColorBOBJ;
    }
}
