using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VISUALNOVEL;

public class MainMenu : MonoBehaviour
{
    public const string MAIN_MENU_SCENE = "MenuScene";

    public static MainMenu instance { get; private set; }

    public AudioClip menuMusic;
    public CanvasGroup mainPanel;
    private CanvasGroupController mainCG;
    [SerializeField] private GalleryMenu gallery;

    private UIConfirmationMenu uiChoiceMenu => UIConfirmationMenu.instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCG = new CanvasGroupController(this, mainPanel);

        AudioManager.instance.StopAllSoundEffects();
        AudioManager.instance.StopAllTracks();

        AudioManager.instance.PlayTrack(menuMusic, channel: 0, startingVolume: 1);
    }

    public void Click_StartNewGame()
    {
        uiChoiceMenu.Show("创建一个新游戏？", new UIConfirmationMenu.ConfirmationButton("是", StartNewGame), new UIConfirmationMenu.ConfirmationButton("否", null));
    }

    public void LoadGame(VNGameSave file)
    {
        VNGameSave.activeFile = file;
        StartCoroutine(StartingGame());
    }

    private void StartNewGame()
    {
        VNGameSave.activeFile = new VNGameSave();
        StartCoroutine(StartingGame());
    }

    private IEnumerator StartingGame()
    {
        mainCG.Hide(speed: 0.3f);
        AudioManager.instance.StopTrack(0);

        while (mainCG.isVisible)
            yield return null;

        VN_Configuration.activeConfig.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene("AVGScene");
    }
}
