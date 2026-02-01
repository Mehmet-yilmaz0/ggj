using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    [Header("Sprites (0 = full, last = empty)")]
    [SerializeField] private List<Sprite> versions = new List<Sprite>();

    public Entity player;

    private Image sr;
    private int _currentIndex = -1;

    void Awake()
    {
        sr = GetComponent<Image>();
    }

    void Update()
    {
        if (player == null) return;
        if (versions == null || versions.Count == 0) return;
        if (sr == null) return;

        // 0..100 kabul ediyoruz
        float hp = Mathf.Clamp(player.health, 0f, 100f);
        float percent = hp / 100f; // 0..1

        int count = versions.Count;        // 21
        int lastIndex = count - 1;         // 20

        // yüzdeyi indexe çevir
        int index = Mathf.FloorToInt((1f - percent) * count);
        index = Mathf.Clamp(index, 0, lastIndex);

        // %95 ve üstü her zaman full bar
        if (percent >= 0.95f)
            index = 0;

        // Sprite deðiþmediyse dokunma
        if (index == _currentIndex) return;

        sr.sprite = versions[index];
        _currentIndex = index;
    }
}
