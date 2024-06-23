using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

namespace DIALOGUE
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject root;
        public NameContainer nameContainer;
        public TextMeshProUGUI dialogueText;

        private CanvasGroupController cgController;

        public void SetDialogueColor(Color color) => dialogueText.color = color;
        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;
        public void SetDialogueFontSize(float size) => dialogueText.fontSize = size;

        private bool initialized = false;
        public void Initialize()
        {
            if (initialized)
                return;

            cgController = new CanvasGroupController(DialogueSystem.instance, root.GetComponent<CanvasGroup>());
        }

        public bool isVisible => cgController.isVisible;
        public Coroutine Show(float speed = 1f, bool immediate = false) => cgController.Show(speed, immediate);
        public Coroutine Hide(float speed = 1f, bool immediate = false) => cgController.Hide(speed, immediate);
        
        // 新方法，用于调整对话框文本的位置和尺寸
        public void SetDialogueTextTransform_Past()
        {
            RectTransform rt = dialogueText.GetComponent<RectTransform>();
            Image img = root.GetComponent<Image>();
            img.color = new Color(255, 255, 255, 0);
            rt.anchoredPosition = new Vector2(0, 0);
            rt.sizeDelta = new Vector2(1400, 600);
        }

        public void SetDialogueTextTransform_Default()
        {
            RectTransform rt = dialogueText.GetComponent<RectTransform>();
            Image img = root.GetComponent<Image>();
            img.color = new Color(255, 255, 255, 200);
            rt.anchoredPosition = new Vector2(0, -380);
            rt.sizeDelta = new Vector2(1000, 160);
        }
    }
}