using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class BuildAnimClipFromNumberedPngs
{
    // Seçili klasör(ler) için anim clip üretir
    [MenuItem("Tools/Animations/Build Clip From 1..N PNGs (Selected Folder)")]
    public static void Build()
    {
        var guids = Selection.assetGUIDs;

        foreach (var guid in guids)
        {
            string folderPath = AssetDatabase.GUIDToAssetPath(guid);
            if (!AssetDatabase.IsValidFolder(folderPath))
                continue;

            CreateClipFromFolder(folderPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Animation clip build complete.");
    }

    private static void CreateClipFromFolder(string folderPath)
    {
        // 1.png, 2.png, ... þeklinde numaralý PNG'leri bul
        var pngs = Directory.GetFiles(folderPath, "*.png", SearchOption.TopDirectoryOnly)
            .Select(p => p.Replace("\\", "/"))
            .Select(p => new { Path = p, Num = ExtractNumber(Path.GetFileNameWithoutExtension(p)) })
            .Where(x => x.Num.HasValue)                 // sadece numaralýlar
            .OrderBy(x => x.Num.Value)                  // sayýsal sýrala
            .ToArray();

        if (pngs.Length == 0)
        {
            Debug.LogWarning($"No numbered PNGs found in: {folderPath}");
            return;
        }

        // Her PNG'nin sprite'ýný yükle (importer sprite olmalý)
        var sprites = pngs
            .Select(x => AssetDatabase.LoadAssetAtPath<Sprite>(x.Path))
            .Where(s => s != null)
            .ToArray();

        if (sprites.Length == 0)
        {
            Debug.LogWarning($"No Sprites loaded (are these PNGs imported as Sprite?) Folder: {folderPath}");
            return;
        }

        // Clip oluþtur
        var clip = new AnimationClip();

        // FPS ayarý (isteðe göre deðiþtir)
        // 12 FPS genelde pixel-art için iyi; istersen 4, 8, 10 yap
        float fps = 12f;
        clip.frameRate = fps;

        // SpriteRenderer.m_Sprite binding
        var binding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        // Keyframe'leri oluþtur: her sprite bir frame
        var keyframes = new ObjectReferenceKeyframe[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                // i / fps saniyede deðiþsin
                time = i / fps,
                value = sprites[i]
            };
        }

        AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);

        // Loop ayarý
        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        // Clip'i ayný klasöre kaydet
        string clipPath = $"{folderPath}/{Path.GetFileName(folderPath)}_Anim.anim";
        clipPath = AssetDatabase.GenerateUniqueAssetPath(clipPath);

        AssetDatabase.CreateAsset(clip, clipPath);

        Debug.Log($"Created: {clipPath}  (frames: {sprites.Length})");
    }

    // Dosya adýndan sayý çýkarýr: "1" -> 1, "001" -> 1, "1_something" -> 1
    private static int? ExtractNumber(string name)
    {
        // Baþtaki sayýyý al: 1, 001, 12, vs.
        var m = Regex.Match(name, @"^\d+");
        if (!m.Success) return null;
        if (int.TryParse(m.Value, out int n)) return n;
        return null;
    }
}
