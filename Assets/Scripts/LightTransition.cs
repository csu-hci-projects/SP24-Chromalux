using UnityEngine;

public class LightTransition : MonoBehaviour
{
    public GameObject lightGroup;
    public Material lightMaterial;
    public Color[] colors = new Color[] { Color.red, Color.green, Color.blue };
    public float transitionDuration = 1f;

    private Light[] lightsInGroup;
    private int currentIndex = 0;
    private Color startColor;
    private Color targetColor;
    private Color startEmissionColor;
    private Color targetEmissionColor;
    private float startTime;

    void Start()
    {
        if (lightGroup == null)
        {
            Debug.LogError("No light group assigned");
            enabled = false;
            return;
        }

        lightsInGroup = lightGroup.GetComponentsInChildren<Light>();

        if (lightsInGroup.Length == 0 || colors.Length == 0)
        {
            Debug.LogError("No lights in group OR no colors defined");
            enabled = false;
            return;
        }

        foreach (Light light in lightsInGroup)
        {
            light.color = colors[currentIndex];
        }

        if (lightMaterial != null)
        {
            lightMaterial.color = colors[currentIndex];
            lightMaterial.SetColor("_EmissionColor", colors[currentIndex]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartColorTransition();
        }

        if (startColor != targetColor)
        {
            float t = (Time.time - startTime) / transitionDuration;
            foreach (Light light in lightsInGroup)
            {
                light.color = Color.Lerp(startColor, targetColor, t);
            }

            if (lightMaterial != null)
            {
                Color lerpedEmissionColor = Color.Lerp(startEmissionColor, targetEmissionColor, t);
                lightMaterial.SetColor("_EmissionColor", lerpedEmissionColor);
            }

            if (t >= 1f)
            {
                startColor = targetColor;
                startEmissionColor = targetEmissionColor;
            }
        }
    }

    // vvvvv CALL THIS TO CHANGE COLORS vvvvv
    public void StartColorTransition()
    {
        currentIndex = (currentIndex + 1) % colors.Length;
        startColor = lightsInGroup[0].color;
        targetColor = colors[currentIndex];
        startTime = Time.time;

        if (lightMaterial != null)
        {
            startEmissionColor = lightMaterial.GetColor("_EmissionColor");
            targetEmissionColor = colors[currentIndex];
        }
    }
}
