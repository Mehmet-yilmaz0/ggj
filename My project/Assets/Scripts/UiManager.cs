using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Player player;

    [Header("UI Objects")]
    [SerializeField] GameObject redMask;
    [SerializeField] GameObject blueMask;
    [SerializeField] GameObject greenMask;

    [SerializeField] GameObject redbar;
    [SerializeField] GameObject bluebar;
    [SerializeField] GameObject greenbar;

    [Header("Sprites")]
    [SerializeField] Sprite[] RedLevels; 
    [SerializeField] Sprite[] BlueLevels;
    [SerializeField] Sprite[] GreenLevels;

    [SerializeField] Sprite[] RedBars;     
    [SerializeField] Sprite[] BlueBars;
    [SerializeField] Sprite[] GreenBars;
    [SerializeField] Image PauseScreen;


    private void Start()
    {
        Control();
    }

    private void Update()
    {
        Control();
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            PauseScreen.gameObject.SetActive(!PauseScreen.gameObject.activeSelf);
            Time.timeScale=Time.timeScale == 0? 1:0;
        }

    }

    void Control()
    {
        UpdateMaskUI(player.masks[0], redMask, redbar, RedLevels, RedBars);

        UpdateMaskUI(player.masks[1], blueMask, bluebar, BlueLevels, BlueBars);

        UpdateMaskUI(player.masks[2], greenMask, greenbar, GreenLevels, GreenBars);
    }

    void UpdateMaskUI(Mask maskData, GameObject maskObj, GameObject barObj, Sprite[] levels, Sprite[] bars)
    {
        if (!maskData.isGot)
        {
            if (maskObj.activeSelf) maskObj.SetActive(false);
            return;
        }

        if (!maskObj.activeSelf) maskObj.SetActive(true);

        float timer = maskData.timer;
        Image maskImage = maskObj.GetComponent<Image>();
        Image barImage = barObj.GetComponent<Image>();

        if (timer >= 30f)
        {
            if (levels.Length > 2) maskImage.sprite = levels[2];
        }
        else if (timer >= 15f)
        {
            if (levels.Length > 1) maskImage.sprite = levels[1];
        }
        else
        {
            if (levels.Length > 0) maskImage.sprite = levels[0];
        }

        int barIndex = (int)(timer / 3f);

        barIndex = Mathf.Clamp(barIndex, 0, bars.Length - 1);

        barImage.sprite = bars[barIndex];
    }
    public void Continue()
    {
        PauseScreen.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}