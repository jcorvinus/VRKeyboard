using UnityEngine;
using System.Collections;

public class TweenMaterialColor : MonoBehaviour 
{
    public enum WrapType { Repeat, PingPong, None }

    public WrapType Wrap;
    private float totalElapsedTime;
    private float wrappedTime;
    [SerializeField]
    private float tValue;
    public float TValue { get { return tValue; } }
    public float Duration;

    public bool UseCustomStartColor=true;
    public Color StartColor;
    public Color GoalColor;

    public MeshRenderer RendererToTween;
    public int MaterialIndex;
    public string ColorName;

    [SerializeField]
    private Color currentColor;
    public Color CurrentColor { get { return currentColor; } }

    void Awake()
    {
        if (RendererToTween == null)
        {
            Debug.Log("Cannot operate without a renderer to operate on.");
            enabled = false;
        }
        else
        {
            StartColor = RendererToTween.materials[MaterialIndex].GetColor(ColorName);
        }
    }

	// Use this for initialization
	void Start () 
    {
	    if(!UseCustomStartColor)
        {
            
        }
	}

    void OnDisable()
    {
        RendererToTween.materials[MaterialIndex].SetColor(ColorName, StartColor);
    }
	
	// Update is called once per frame
	void Update () 
    {
        totalElapsedTime += Time.deltaTime;

        switch (Wrap)
        {
            case WrapType.Repeat:
                wrappedTime = Mathf.Repeat(totalElapsedTime, Duration);
                break;

            case WrapType.PingPong:
                wrappedTime = Mathf.PingPong(totalElapsedTime, Duration);
                break;

            case WrapType.None:
                wrappedTime = Mathf.Clamp(totalElapsedTime, 0, Duration);
                break;
            default:
                break;
        }

        tValue = Mathf.InverseLerp(0, Duration, wrappedTime);
        currentColor = Color.Lerp(StartColor, GoalColor, tValue);

        RendererToTween.materials[MaterialIndex].SetColor(ColorName, currentColor);
	}
}
