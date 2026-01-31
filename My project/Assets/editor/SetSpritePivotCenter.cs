using UnityEditor;
using UnityEngine;
using System.IO;

public class SetSpritePivotCenterFromFolder
{
    [MenuItem("Tools/Sprites/Set Pivot Center (Selected Folders)")]
    static void SetPivotForFolders()
    {
        var guids = Selection.assetGUIDs;
        int changed = 0;

        foreach (var guid in guids)
        {
            string rootPath = AssetDatabase.GUIDToAssetPath(guid);

            // Sadece klasörleri iþle
            if (!AssetDatabase.IsValidFolder(rootPath))
                continue;

            // Alt klasörler dahil tüm asset path'leri al
            string[] assetPaths = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);

            foreach (var path in assetPaths)
            {
                if (!path.EndsWith(".png") && !path.EndsWith(".jpg"))
                    continue;

                var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null)
                    continue;

                if (importer.textureType != TextureImporterType.Sprite)
                    continue;

                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePivot = new Vector2(0.5f, 0.5f);

                importer.SaveAndReimport();
                changed++;
            }
        }

        Debug.Log($"Pivot Center applied to {changed} sprite(s).");
    }
}
