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

/// <summary>
/// Connects to a button and relays those events to
/// a virtual keyboard.
/// </summary>
[RequireComponent(typeof(FingerButton))]
public class KeyButton : MonoBehaviour 
{
    public delegate void KeyButtonEventHandler(KeyButton sender);
    public event KeyButtonEventHandler Activated;
    public event KeyButtonEventHandler HoverGained;
    public event KeyButtonEventHandler HoverLost;

    private Keyboard keyboard;
    private FingerButton button;
    private bool isAltMode = false;

    /// <summary>Describes what they key will do when in "Alternate" mode.</summary>
    public enum AlternateMode 
    { 
        /// <summary>In Alternate mode, character will change case.</summary>
        Case, 
        Symbol
    }
    public AlternateMode Mode = AlternateMode.Case;
    public KeyCode Key;
    public KeyCode AlternateKey;
    private UnityEngine.UI.Text characterText;
    public string Text
    {
        get { return characterText.text; }
    }
    
    void Awake()
    {
        button = GetComponent<FingerButton>();
        keyboard = GetComponentInParent<Keyboard>();
        keyboard.ModeChange += keyboard_ModeChange;
        characterText = GetComponentInChildren<UnityEngine.UI.Text>();

        if(characterText != null)
        {
            characterText.transform.SetParent(button.ButtonFace);
        }
    }

    void keyboard_ModeChange(Keyboard sender, bool normalMode)
    {
        isAltMode = !normalMode;
        SetText();
    }

    void OnEnable()
    {
        SetText();

        keyboard.AddKey(this);

        button.ButtonActivated += OnButtonActivated;
        button.ButtonHovered += OnButtonHoverGained;
        button.ButtonHoverEnded += OnButtonHoverLost;
    }

    private void SetText()
    {
        if (characterText != null)
        {
            if (isAltMode)
            {
                if (Mode == AlternateMode.Case)
                {
                    characterText.text = Keyboard.ConvertCodeToChar(Key.ToString());
                }
                else
                {
                    characterText.text = Keyboard.ConvertCodeToChar(AlternateKey.ToString());
                }
            }
            else
            {
                if (Mode == AlternateMode.Case)
                {
                    characterText.text = Keyboard.ConvertCodeToChar(Key.ToString().ToLower());
                }
                else
                {
                    characterText.text = Keyboard.ConvertCodeToChar(Key.ToString());
                }
            }
        }
    }

    void OnDisable()
    {
        keyboard.RemoveKey(this);
        button.ButtonActivated -= OnButtonActivated;
        button.ButtonHovered -= OnButtonHoverGained;
        button.ButtonHoverEnded -= OnButtonHoverLost;
    }

	void Start () 
    {

	}

    #region Event Methods
    private void OnButtonActivated(FingerButton sender)
    {
        if (Activated != null) Activated(this);
    }

    private void OnButtonHoverGained(FingerButton sender)
    {
        if (HoverGained != null) HoverGained(this);
    }

    private void OnButtonHoverLost(FingerButton sender)
    {
        if (HoverLost != null) HoverLost(this);
    }
    #endregion
}
