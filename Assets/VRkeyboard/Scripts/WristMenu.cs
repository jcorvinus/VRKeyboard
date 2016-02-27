/* The MIT License (MIT)

Copyright (c) 2016 Joshua Corvinus

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */

using UnityEngine;
using System.Collections;

public class WristMenu : MonoBehaviour
{
    #region Keyboard Variables
    [Header("Keyboard Variables")]
    public Keyboard Keyboard;
    public FingerButton ResetButton;
    public FingerButton LettersButton;
    public FingerButton NumbersButton;
    #endregion

    #region Hand Variables
    [Header("Hand Variables")]
    public Transform Forearm;
    public Vector3 LocalOffset;
    #endregion

    [Header("Visibility Variables")]
    public bool Visible=false;

    void OnEnable()
    {
        ResetButton.ButtonActivated += ResetButton_ButtonActivated;
        LettersButton.ButtonActivated += LettersButton_ButtonActivated;
        NumbersButton.ButtonActivated += NumbersButton_ButtonActivated;
    }

    void OnDisable()
    {
        ResetButton.ButtonActivated -= ResetButton_ButtonActivated;
        LettersButton.ButtonActivated -= LettersButton_ButtonActivated;
        NumbersButton.ButtonActivated -= NumbersButton_ButtonActivated;
    }

    void Start () 
    {
        transform.SetParent(Forearm);
        transform.localPosition = LocalOffset;
	}

    void NumbersButton_ButtonActivated(FingerButton sender)
    {
        Keyboard.ShowCharSet(1);
    }

    void LettersButton_ButtonActivated(FingerButton sender)
    {
        Keyboard.ShowCharSet(0);
    }

    void ResetButton_ButtonActivated(FingerButton sender)
    {
        UnityEngine.VR.InputTracking.Recenter();
    }
	
	void Update () 
    {
        transform.localScale = Vector3.Lerp(transform.localScale, (Visible) ? Vector3.one : Vector3.zero, Time.deltaTime * 4.5f);
	}

    public void Show()
    {
        Visible = true;
        SetButtonVisibility(Visible);
    }

    public void Hide()
    {
        Visible = false;
        SetButtonVisibility(Visible);
    }

    public void SetButtonVisibility(bool visible)
    {
        ResetButton.gameObject.SetActive(Visible);
        LettersButton.gameObject.SetActive(Visible);
        NumbersButton.gameObject.SetActive(Visible);
    }
}
