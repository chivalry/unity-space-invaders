using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Editor-only utility that procedurally builds a shield from individual pixel objects.
/// Invoke via the context menu on the component in the Inspector.
/// </summary>
public class ShieldBuilder : MonoBehaviour
{
#if UNITY_EDITOR
    /// <summary>
    /// Destroys any existing child pixels and rebuilds a 6×4 pixel grid with a 2×2 notch
    /// cut from the bottom-center, matching the classic Space Invaders shield shape.
    /// Each pixel gets a <see cref="SpriteRenderer"/>, a trigger <see cref="BoxCollider2D"/>,
    /// and a <see cref="ShieldPixel"/> component.
    /// </summary>
    [ContextMenu("Build Shield Pixels")]
    void BuildShieldPixels()
    {
        foreach (Transform child in transform)
            DestroyImmediate(child.gameObject);

        int cols = 6;
        int rows = 4;
        float pixelSize = 0.5f;

        // Notch: bottom 2 rows, middle 2 columns removed
        bool[,] notch = new bool[rows, cols];
        notch[2, 2] = true; notch[2, 3] = true;
        notch[3, 2] = true; notch[3, 3] = true;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (notch[row, col]) continue;

                float x = (col - (cols - 1) / 2f) * pixelSize;
                float y = ((rows - 1 - row) - (rows - 1) / 2f) * pixelSize;

                GameObject pixel = new GameObject($"Pixel_{row}_{col}");
                pixel.transform.SetParent(transform, false);
                pixel.transform.localPosition = new Vector3(x, y, 0f);
                pixel.layer = LayerMask.NameToLayer("Shield");

                SpriteRenderer sr = pixel.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(
                    "UI/Skin/UISprite.psd");
                sr.color = new Color(0.2f, 0.8f, 0.2f);
                sr.sortingLayerName = "Shield";
                sr.size = new Vector2(pixelSize, pixelSize);
                sr.drawMode = SpriteDrawMode.Sliced;

                BoxCollider2D col2d = pixel.AddComponent<BoxCollider2D>();
                col2d.isTrigger = true;
                col2d.size = new Vector2(pixelSize, pixelSize);

                pixel.AddComponent<ShieldPixel>();
            }
        }

        EditorUtility.SetDirty(gameObject);
        Debug.Log("Shield pixels built.");
    }
#endif
}
