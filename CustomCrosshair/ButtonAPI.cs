using TMPro;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

namespace CustomCrosshair
{
    // Token: 0x02000002 RID: 2
    internal class ButtonAPI
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static GameObject CreateNewButton(string text, string objName, Transform parent, float x, float y, Vector3 scale, UnityAction action)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("Managers (1)").transform.Find("UI/Game/DeadUI/MenuBtn").gameObject, parent);
            TextMeshProUGUI component = gameObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            Button component2 = gameObject.GetComponent<Button>();
            gameObject.name = objName;
            gameObject.transform.localScale = scale;
            gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            gameObject.transform.localPosition = new Vector3(0f + x * 185.102f, -125.965f + y * 83.976f, 0f);
            component.text = text;
            component.enableWordWrapping = false;
            component.enableAutoSizing = true;
            component2.onClick = new Button.ButtonClickedEvent();
            component2.onClick.AddListener(action);
            return gameObject;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002140 File Offset: 0x00000340
        public static GameObject MakeSlider(string text, string objectName, float minValue, float maxValue, Color textColor, float x, float y, Vector3 scale, Transform parent)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("UI/Menu").transform.parent.Find("Settings/Slider").gameObject, parent);
            Slider slider = gameObject.GetComponent<Slider>();
            TextMeshProUGUI sliderText = gameObject.transform.Find("Sens").GetComponent<TextMeshProUGUI>();
            sliderText.text = text + " " + slider.value.ToString("F1");
            sliderText.color = textColor;
            sliderText.enableAutoSizing = true;
            sliderText.enableWordWrapping = true;
            gameObject.name = objectName;
            gameObject.transform.localPosition = new Vector3(0f + x * 247f, 0f + y * 40f, 0f);
            gameObject.transform.localScale = scale;
            slider.onValueChanged.RemoveAllListeners();
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.onValueChanged.AddListener((_) =>
            {
                sliderText.text = text + " " + slider.value.ToString("F1");
            });
            return gameObject;
        }
    }
}
