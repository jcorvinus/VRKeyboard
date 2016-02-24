using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FingerButton))]
public class FingerButtonTest : MonoBehaviour 
{
    private FingerButton button;

    [Header("Variables")]
    public float ThrowValue;
    public bool ApplyThrow=false;

    [Header("Commands")]
    public bool Activate = false;
    public bool Hover = false;

	// Use this for initialization
	void Awake () 
    {
        button = GetComponent<FingerButton>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(ApplyThrow)
        {
            button.CurrentThrowValue = ThrowValue;
        }

        if(Activate)
        {
            button.Activate();
            Activate = false;
        }

        if(Hover)
        {
            button.Hover();
            Hover = false;
        }
	}
}
