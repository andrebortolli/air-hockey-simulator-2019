using UnityEngine;
using TMPro;

public class RainbowEffect : MonoBehaviour
{
    public float Speed = 1.0f;
    public Color inverseRainbowColor;
    public Color rainbowColor;
    public GameObject gO;
    public ParticleSystem pS;
    public TextMeshProUGUI textMeshUGUI;

    Color InvertColor(Color color)
    {
        float maxComponent = color.maxColorComponent;
        return new Color(maxComponent - color.r, maxComponent - color.g, maxComponent - color.b);
    }

    void Update()
    {
        float timeSpeed = Time.time * Speed;
        rainbowColor = HSBColor.ToColor(new HSBColor(Mathf.PingPong(timeSpeed, 1), 1, 1));
        inverseRainbowColor = InvertColor(rainbowColor);
        VertexGradient colorGrad = new VertexGradient(rainbowColor, inverseRainbowColor, rainbowColor, inverseRainbowColor);
        if (gO)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = rainbowColor;
        }
        if (pS)
        {
            pS.startColor = rainbowColor;
        }
        if (textMeshUGUI)
        {
            textMeshUGUI.colorGradient = colorGrad;
        }
    }
    private void LateUpdate()
    {
        
    }
}