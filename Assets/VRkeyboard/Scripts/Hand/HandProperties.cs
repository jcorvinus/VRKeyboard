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
using Leap;

namespace Hands
{
	// Just storing some hand-relevant definitions common to all sorts of UI stuff.
    // note: fingers point 'forward' on the z axis, the red axis points inwards on the hand (from index to thumb) and the green axis points away from the palm)

	public enum HandFilter { Left = 0, Right = 1, Either = 2, Both = 3, None = 4 }
	[System.Flags]
	public enum FingerFilter { none = 0, thumb = 0x1, index = 0x2, middle = 0x4, ring = 0x8, pinky = 0x0f }
	public enum HandLocation { finger, wrist, forearm, palm } 

    /// <summary>
    /// Stores relevant information about a fingertip.
    /// </summary>
    public struct FingertipData
    {
        public GameObject Owner;
        public bool HandLeft { get { return HandModel.GetLeapHand().IsLeft; } }
        public FingerFilter finger;
        public Vector3 TipPosition { get {return (FingerModel != null) ? FingerModel.GetTipPosition() : Vector3.zero; } }

        public HandModel HandModel;
        public FingerModel FingerModel;
    }

    public static class HandProperties
    {
        /// <summary>
        /// Converts a Lucidigital Leap.Finger.FingerType enum type to a FingerFilter enum.
        /// 
        /// There are two possible failure states: being fed an enum as integer that is out of range (you have to try hard to do this), and 
        /// trying to convert a FingerFilter.none to Leap.Finger.FingerType, as FingerType has no equivalent.
        /// 
        /// Upon failure, the method will throw a System.NotSupported exception with details about the failure.
        /// </summary>
        /// <param name="finger">Finger filter to convert.</param>
        /// <returns></returns>
        public static Finger.FingerType FingerTypeFromFingerFilter(FingerFilter finger)
        {
            switch (finger)
            {
                case FingerFilter.none:
                    throw new System.NotSupportedException("Error in HandProperties.cs: FingerFilter.none cannot map to Leap.Finger.FingerType. No equivalent exists.");
                    return Finger.FingerType.TYPE_THUMB; // all thumbs, lol

                case FingerFilter.thumb:
                    return Finger.FingerType.TYPE_THUMB;

                case FingerFilter.index:
                    return Finger.FingerType.TYPE_INDEX;                    

                case FingerFilter.middle:
                    return Finger.FingerType.TYPE_MIDDLE;

                case FingerFilter.ring:
                    return Finger.FingerType.TYPE_RING;

                case FingerFilter.pinky:
                    return Finger.FingerType.TYPE_PINKY;

                default:
                    throw new System.NotSupportedException("Error in HandProperties.cs: An out of bounds enum value " + ((int)finger).ToString() + " was supplied.");
                    return Finger.FingerType.TYPE_THUMB; // all thumbs, lol
            }
        }

        /// <summary>
        /// Converts a FingerFilter enum type to a Leap.Finger.FingerType enum.
        /// 
        /// There is only one possible failure state: being fed an enum as an integer that is out of range.
        /// 
        /// Upon failure, the method will throw a System.NotSupported exception with details about the failure.
        /// </summary>
        /// <param name="fingerType">FingerType to convert.</param>
        /// <returns></returns>
        public static FingerFilter FingerFilterFromFingerType(Leap.Finger.FingerType fingerType)
        {
            switch (fingerType)
            {
                case Finger.FingerType.TYPE_INDEX:
                    return FingerFilter.index;

                case Finger.FingerType.TYPE_MIDDLE:
                    return FingerFilter.middle;

                case Finger.FingerType.TYPE_PINKY:
                    return FingerFilter.pinky;

                case Finger.FingerType.TYPE_RING:
                    return FingerFilter.ring;

                case Finger.FingerType.TYPE_THUMB:
                    return FingerFilter.thumb;

                default:
                    throw new System.NotSupportedException("Error in HandProperties.cs: An out of bounds enum value " + ((int)fingerType).ToString() + " was supplied.");
                    return FingerFilter.thumb;
            }
        }

        /// <summary>
        /// Check a HandFilter against a IsLeft boolean value.
        /// </summary>
        /// <param name="isLeft"></param>
        /// <param name="hand"></param>
        /// <returns>hand filter is valid given the supplied isLeft value.</returns>
        public static bool HandFilterMatchesSide(bool isLeft, HandFilter hand)
        {
            if (hand == HandFilter.Either) return true;
            else if (hand == HandFilter.Both) return true;
            else if (hand == HandFilter.Left) return isLeft;
            else if (hand == HandFilter.Right) return !isLeft;

            return false;
        }

        /// <summary>
        /// Check a HandFilter against a Leap Chirality.
        /// </summary>
        /// <param name="handedness"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        public static bool HandFilterMatchesChirality(Chirality handedness, HandFilter hand)
        {
            switch (handedness)
            {
                case Chirality.Left:
                    if ((hand == HandFilter.Left) || (hand == HandFilter.Either)) return true;
                    break;

                case Chirality.Right:
                    if ((hand == HandFilter.Right) || (hand == HandFilter.Either)) return true;
                    break;

                case Chirality.Either:
                    if (hand != HandFilter.None) return true;
                    break;

                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Returns a 
        /// </summary>
        /// <param name="isLeft"></param>
        /// <returns>Either HandFilter.Left or HandFilter.Right depending on the input value.</returns>
        public static HandFilter HandFilterFromSide(bool isLeft)
        {
            if (isLeft) return HandFilter.Left;
            else return HandFilter.Right;
        }
    }
}