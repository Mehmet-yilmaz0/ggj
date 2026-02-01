using System.Collections;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [Header("Scene Objects")]
    public GameObject environment;
    public GameObject bossArea;
    public GameObject player;
    public Vector3 startpos;

    [Header("Fade UI (CanvasGroup on a fullscreen black Image)")]
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 0.4f;

    private bool _isTransitioning;

    private void Awake()
    {
        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.blocksRaycasts = false;
            fadeGroup.interactable = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isTransitioning) return;
        if (!collision.gameObject.CompareTag("Player") && collision.gameObject.name != "Player") return;

        StartCoroutine(GoDarkAndTeleport());
    }

    private IEnumerator GoDarkAndTeleport()
    {
        _isTransitioning = true;

        // Fade to black
        yield return Fade(1f);

        // Switch area + teleport
        if (environment != null) environment.SetActive(false);
        if (bossArea != null) bossArea.SetActive(true);
        GoToAnotherPlace();

        // Fade back
        yield return Fade(0f);

        _isTransitioning = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeGroup == null) yield break;

        float startAlpha = fadeGroup.alpha;
        float t = 0f;

        // Ýstersen bu sýrada input bloklansýn:
        fadeGroup.blocksRaycasts = true;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeDuration);
            fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, k);
            yield return null;
        }

        fadeGroup.alpha = targetAlpha;

        // ekran açýldýysa input'u geri býrak
        fadeGroup.blocksRaycasts = targetAlpha > 0f;
    }

    public void GoToAnotherPlace()
    {
        if (player != null)
            player.transform.position = startpos;
    }
}
