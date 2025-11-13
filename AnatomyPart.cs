using UnityEngine;

public class AnatomyPart : MonoBehaviour
{
    [Header("Metadata")]
    public string partName = "Part";
    [TextArea] public string description = "Description of this part.";
    public Sprite image; // small reference image shown in annotation

    public Color highlightColor = Color.yellow;
    private Color originalColor;
    private Renderer[] renderers;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
            originalColor = renderers[0].material.color;
    }

    public void Highlight(bool on)
    {
        foreach (var r in renderers)
        {
            if (on)
            {
                if (r.material.HasProperty("_Color")) r.material.color = highlightColor;
            }
            else
            {
                if (r.material.HasProperty("_Color")) r.material.color = originalColor;
            }
        }
    }
}
