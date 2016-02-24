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
using System.Collections.Generic;
using System.Text;

/// <summary>
/// A virtual keyboard for providing text input.
/// </summary>
public class Keyboard : MonoBehaviour 
{
    public delegate void KeyboardModeHandler(Keyboard sender, bool normalMode);
    public event KeyboardModeHandler ModeChange;

    private StringBuilder stringBuilder;
    private List<KeyButton> keyButtons;
    private bool normalMode = true;

    public UnityEngine.UI.Text TextOut;

    public GameObject[] CharacterSets;

    [Header("Debug Variables")]
    public string Contents = "";

    /// <summary> List of keys that cannot be converted straight to a character.</summary>
    private static KeyCode[] NonCharKeys = 
    {
        #region KeyArray
        KeyCode.Backspace,
        KeyCode.Break,
        KeyCode.CapsLock,
        KeyCode.Delete,
        KeyCode.DownArrow,
        KeyCode.End,
        KeyCode.Escape,
        KeyCode.F1,
        KeyCode.F2,
        KeyCode.F3,
        KeyCode.F4,
        KeyCode.F5,
        KeyCode.F6,
        KeyCode.F7,
        KeyCode.F8,
        KeyCode.F9,
        KeyCode.F10,
        KeyCode.F11,
        KeyCode.F12,
        KeyCode.Home,
        KeyCode.Insert,
        KeyCode.JoystickButton0,
        KeyCode.JoystickButton1,
        KeyCode.JoystickButton2,
        KeyCode.JoystickButton3,
        KeyCode.JoystickButton4,
        KeyCode.JoystickButton5,
        KeyCode.JoystickButton6,
        KeyCode.JoystickButton7,
        KeyCode.JoystickButton8,
        KeyCode.JoystickButton9,
        KeyCode.JoystickButton10,
        KeyCode.JoystickButton11,
        KeyCode.JoystickButton12,
        KeyCode.JoystickButton13,
        KeyCode.JoystickButton14,
        KeyCode.JoystickButton15,
        KeyCode.JoystickButton16,
        KeyCode.JoystickButton17,
        KeyCode.JoystickButton18,
        KeyCode.JoystickButton19,
        KeyCode.Joystick1Button0,
        KeyCode.Joystick1Button1,
        KeyCode.Joystick1Button2,
        KeyCode.Joystick1Button3,
        KeyCode.Joystick1Button4,
        KeyCode.Joystick1Button5,
        KeyCode.Joystick1Button6,
        KeyCode.Joystick1Button7,
        KeyCode.Joystick1Button8,
        KeyCode.Joystick1Button9,
        KeyCode.Joystick1Button10,
        KeyCode.Joystick1Button11,
        KeyCode.Joystick1Button12,
        KeyCode.Joystick1Button13,
        KeyCode.Joystick1Button14,
        KeyCode.Joystick1Button15,
        KeyCode.Joystick1Button16,
        KeyCode.Joystick1Button17,
        KeyCode.Joystick1Button18,
        KeyCode.Joystick1Button19,
        KeyCode.Joystick2Button0,
        KeyCode.Joystick2Button1,
        KeyCode.Joystick2Button2,
        KeyCode.Joystick2Button3,
        KeyCode.Joystick2Button4,
        KeyCode.Joystick2Button5,
        KeyCode.Joystick2Button6,
        KeyCode.Joystick2Button7,
        KeyCode.Joystick2Button8,
        KeyCode.Joystick2Button9,
        KeyCode.Joystick2Button10,
        KeyCode.Joystick2Button11,
        KeyCode.Joystick2Button12,
        KeyCode.Joystick2Button13,
        KeyCode.Joystick2Button14,
        KeyCode.Joystick2Button15,
        KeyCode.Joystick2Button16,
        KeyCode.Joystick2Button17,
        KeyCode.Joystick2Button18,
        KeyCode.Joystick2Button19,
        KeyCode.Joystick3Button0,
        KeyCode.Joystick3Button1,
        KeyCode.Joystick3Button2,
        KeyCode.Joystick3Button3,
        KeyCode.Joystick3Button4,
        KeyCode.Joystick3Button5,
        KeyCode.Joystick3Button6,
        KeyCode.Joystick3Button7,
        KeyCode.Joystick3Button8,
        KeyCode.Joystick3Button9,
        KeyCode.Joystick3Button10,
        KeyCode.Joystick3Button11,
        KeyCode.Joystick3Button12,
        KeyCode.Joystick3Button13,
        KeyCode.Joystick3Button14,
        KeyCode.Joystick3Button15,
        KeyCode.Joystick3Button16,
        KeyCode.Joystick3Button17,
        KeyCode.Joystick3Button18,
        KeyCode.Joystick3Button19,
        KeyCode.Joystick4Button0,
        KeyCode.Joystick4Button1,
        KeyCode.Joystick4Button2,
        KeyCode.Joystick4Button3,
        KeyCode.Joystick4Button4,
        KeyCode.Joystick4Button5,
        KeyCode.Joystick4Button6,
        KeyCode.Joystick4Button7,
        KeyCode.Joystick4Button8,
        KeyCode.Joystick4Button9,
        KeyCode.Joystick4Button10,
        KeyCode.Joystick4Button11,
        KeyCode.Joystick4Button12,
        KeyCode.Joystick4Button13,
        KeyCode.Joystick4Button14,
        KeyCode.Joystick4Button15,
        KeyCode.Joystick4Button16,
        KeyCode.Joystick4Button17,
        KeyCode.Joystick4Button18,
        KeyCode.Joystick4Button19,
        KeyCode.Joystick5Button0,
        KeyCode.Joystick5Button1,
        KeyCode.Joystick5Button2,
        KeyCode.Joystick5Button3,
        KeyCode.Joystick5Button4,
        KeyCode.Joystick5Button5,
        KeyCode.Joystick5Button6,
        KeyCode.Joystick5Button7,
        KeyCode.Joystick5Button8,
        KeyCode.Joystick5Button9,
        KeyCode.Joystick5Button10,
        KeyCode.Joystick5Button11,
        KeyCode.Joystick5Button12,
        KeyCode.Joystick5Button13,
        KeyCode.Joystick5Button14,
        KeyCode.Joystick5Button15,
        KeyCode.Joystick5Button16,
        KeyCode.Joystick5Button17,
        KeyCode.Joystick5Button18,
        KeyCode.Joystick5Button19,
        KeyCode.Joystick6Button0,
        KeyCode.Joystick6Button1,
        KeyCode.Joystick6Button2,
        KeyCode.Joystick6Button3,
        KeyCode.Joystick6Button4,
        KeyCode.Joystick6Button5,
        KeyCode.Joystick6Button6,
        KeyCode.Joystick6Button7,
        KeyCode.Joystick6Button8,
        KeyCode.Joystick6Button9,
        KeyCode.Joystick6Button10,
        KeyCode.Joystick6Button11,
        KeyCode.Joystick6Button12,
        KeyCode.Joystick6Button13,
        KeyCode.Joystick6Button14,
        KeyCode.Joystick6Button15,
        KeyCode.Joystick6Button16,
        KeyCode.Joystick6Button17,
        KeyCode.Joystick6Button18,
        KeyCode.Joystick6Button19,
        KeyCode.Joystick7Button0,
        KeyCode.Joystick7Button1,
        KeyCode.Joystick7Button2,
        KeyCode.Joystick7Button3,
        KeyCode.Joystick7Button4,
        KeyCode.Joystick7Button5,
        KeyCode.Joystick7Button6,
        KeyCode.Joystick7Button7,
        KeyCode.Joystick7Button8,
        KeyCode.Joystick7Button9,
        KeyCode.Joystick7Button10,
        KeyCode.Joystick7Button11,
        KeyCode.Joystick7Button12,
        KeyCode.Joystick7Button13,
        KeyCode.Joystick7Button14,
        KeyCode.Joystick7Button15,
        KeyCode.Joystick7Button16,
        KeyCode.Joystick7Button17,
        KeyCode.Joystick7Button18,
        KeyCode.Joystick7Button19,
        KeyCode.Joystick8Button0,
        KeyCode.Joystick8Button1,
        KeyCode.Joystick8Button2,
        KeyCode.Joystick8Button3,
        KeyCode.Joystick8Button4,
        KeyCode.Joystick8Button5,
        KeyCode.Joystick8Button6,
        KeyCode.Joystick8Button7,
        KeyCode.Joystick8Button8,
        KeyCode.Joystick8Button9,
        KeyCode.Joystick8Button10,
        KeyCode.Joystick8Button11,
        KeyCode.Joystick8Button12,
        KeyCode.Joystick8Button13,
        KeyCode.Joystick8Button14,
        KeyCode.Joystick8Button15,
        KeyCode.Joystick8Button16,
        KeyCode.Joystick8Button17,
        KeyCode.Joystick8Button18,
        KeyCode.Joystick8Button19,
        KeyCode.KeypadEnter,
        KeyCode.LeftAlt,
        KeyCode.LeftApple,
        KeyCode.LeftArrow,
        KeyCode.LeftCommand,
        KeyCode.LeftControl,
        KeyCode.LeftShift,
        KeyCode.LeftWindows,
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Mouse2,
        KeyCode.Mouse3,
        KeyCode.Mouse4,
        KeyCode.Mouse5,
        KeyCode.Mouse6,
        KeyCode.None,
        KeyCode.Numlock,
        KeyCode.PageDown,
        KeyCode.PageUp,
        KeyCode.Pause,
        KeyCode.Print,
        KeyCode.Return,
        KeyCode.RightAlt,
        KeyCode.RightApple,
        KeyCode.RightArrow,
        KeyCode.RightCommand,
        KeyCode.RightControl,
        KeyCode.RightShift,
        KeyCode.RightWindows,
        KeyCode.ScrollLock,
        KeyCode.Space,
        KeyCode.SysReq,
        KeyCode.Tab,
        KeyCode.UpArrow
        #endregion
    };
    
    void Awake()
    {
        stringBuilder = new StringBuilder();
        keyButtons = new List<KeyButton>();
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void AddKey(KeyButton button)
    {
        keyButtons.Add(button);
        button.Activated += button_Activated;
        button.HoverLost += button_HoverLost;
        button.HoverGained += button_HoverGained;
    }

    public void RemoveKey(KeyButton button)
    {
        keyButtons.Remove(button);
        button.Activated -= button_Activated;
        button.HoverLost -= button_HoverLost;
        button.HoverGained -= button_HoverGained;
    }

    public void ShowCharSet(int set)
    {
        for (int i = 0; i < CharacterSets.Length; i++) CharacterSets[i].SetActive(false);

        CharacterSets[set].SetActive(true);
    }

    #region Key Button Event Methods
    void button_HoverGained(KeyButton sender)
    {
        Debug.Log("Button: " + sender.name + " started hovering.");
    }

    void button_HoverLost(KeyButton sender)
    {
        Debug.Log("Button: " + sender.name + " stopped hovering.");
    }

    private bool KeyIsChar(KeyCode suspect)
    {
        for(int i=0; i < NonCharKeys.Length; i++)
        {
            if(suspect == NonCharKeys[i])
            {
                return false;               
            }
        }

        return true;
    }

    private void ChangeMode()
    {
        normalMode = !normalMode;
        if (ModeChange != null) ModeChange(this, normalMode);
    }

    void button_Activated(KeyButton sender)
    {
        if(KeyIsChar(sender.Key))
        {
            //stringBuilder.Append(ConvertCodeToChar(sender.Key.ToString()));
            stringBuilder.Append(sender.Text);
        }
        else
        {
            // look for our special case strings!
            if(sender.Key == KeyCode.Backspace)
            {
                if(stringBuilder.Length > 0) stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            else if (sender.Key == KeyCode.Space)
            {
                stringBuilder.Append(" ");
            }
            else if ((sender.Key == KeyCode.LeftShift) || (sender.Key == KeyCode.RightShift))
            {
                ChangeMode();
            }
        }

        Contents = stringBuilder.ToString();
        if(TextOut != null) TextOut.text = Contents;
    }
    #endregion

    public static string ConvertCodeToChar(string input)
    {
        string retString = input;

        int alphaIndx = retString.IndexOf("Alpha");

        retString = (alphaIndx < 0) ? retString : retString.Remove(alphaIndx, "Alpha".Length);

        if (retString == KeyCode.Exclaim.ToString()) retString = "!";
        else if (retString == KeyCode.At.ToString()) retString = "@";
        else if (retString == KeyCode.Hash.ToString()) retString = "#";
        else if (retString == KeyCode.Dollar.ToString()) retString = "$";
        else if (retString == KeyCode.Caret.ToString()) retString = "^";
        else if (retString == KeyCode.Ampersand.ToString()) retString = "&";
        else if (retString == KeyCode.Asterisk.ToString()) retString = "*";
        else if (retString == KeyCode.LeftParen.ToString()) retString = "(";
        else if (retString == KeyCode.RightParen.ToString()) retString = ")";
        else if (retString == KeyCode.Minus.ToString()) retString = "-";
        else if (retString == KeyCode.Plus.ToString()) retString = "+";
        else if (retString == KeyCode.LeftBracket.ToString()) retString = "[";
        else if (retString == KeyCode.RightBracket.ToString()) retString = "]";
        else if (retString == KeyCode.Equals.ToString()) retString = "=";
        else if (retString == KeyCode.Semicolon.ToString()) retString = ";";
        else if (retString == KeyCode.Colon.ToString()) retString = ":";
        else if (retString == KeyCode.Quote.ToString()) retString = "'";
        else if (retString == KeyCode.DoubleQuote.ToString()) retString = "\"";
        else if (retString == KeyCode.Comma.ToString()) retString = ",";
        else if (retString == KeyCode.Less.ToString()) retString = "<";
        else if (retString == KeyCode.Greater.ToString()) retString = ">";
        else if (retString == KeyCode.Slash.ToString()) retString = "/";
        else if (retString == KeyCode.Question.ToString()) retString = "?";
        else if (retString == KeyCode.Backslash.ToString()) retString = "\\";
        else if (retString == KeyCode.BackQuote.ToString()) retString = "`";
        else if (retString == KeyCode.Underscore.ToString()) retString = "_";

        return retString;
    }
}
