using UnityEngine;

public class RainbowEffect : MonoBehaviour
{
    public float Speed = 1.0f;
    public Color rainbowColor;
    public GameObject gameObject;

    void Update()
    {
        rainbowColor = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * Speed, 1), 1, 1));
        gameObject.GetComponent<MeshRenderer>().material.color = rainbowColor;
    }
}