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

namespace Hands
{
    /// <summary>
    /// Sends messages to objects in the world that are touched by the fingertip.
    /// 
    /// Legend:
    /// OnFingertipTriggerEnter - sent when this fingertip enters a trigger volume.
    /// OnFingertipTriggerStay - sent when this fingertip stays in a trigger volume.
    /// OnFingertipTriggerExit - sent when this fingertip exits a trigger volume (deletion is handled, you won't have to check against the fingertip suddenly going null).
    /// </summary>
    public class Fingertip : MonoBehaviour
    {
        private FingertipData fingertipData;

        private List<GameObject> otherObjectList;

        void Awake()
        {
            fingertipData.Owner = this.gameObject;
            fingertipData.HandModel = GetComponentInParent<HandModel>();
            otherObjectList = new List<GameObject>();

            fingertipData.finger = (FingerFilter)System.Enum.Parse(typeof(FingerFilter), transform.parent.name);
            fingertipData.FingerModel = fingertipData.HandModel.fingers[(int)HandProperties.FingerTypeFromFingerFilter(fingertipData.finger)];
        }

        void OnTriggerEnter(Collider other)
        {
            other.gameObject.SendMessage("OnFingertipTriggerEnter", fingertipData, SendMessageOptions.DontRequireReceiver);
            otherObjectList.Add(other.gameObject);
        }

        void OnTriggerStay(Collider other)
        {
            other.gameObject.SendMessage("OnFingertipTriggerStay", fingertipData, SendMessageOptions.DontRequireReceiver);
        }

        void OnTriggerExit(Collider other)
        {
            SendExitEvent(other.gameObject);
        }

        private void SendExitEvent(GameObject other)
        {
            try
            {
                other.SendMessage("OnFingertipTriggerExit", fingertipData, SendMessageOptions.DontRequireReceiver);
                otherObjectList.Remove(other.gameObject);
            }
            catch(MissingReferenceException missingRef)
            {
                Debug.LogError("Missing Reference in Fingertip.cs");
            }
        }

        void OnDestroy()
        {
            GameObject[] objectList = otherObjectList.ToArray();

            foreach(GameObject otherObject in objectList)
            {
                SendExitEvent(otherObject);
            }
        }
    }
}