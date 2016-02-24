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

using Hands;

public class FingerButton : MonoBehaviour
{
    #region Events
    public delegate void ButtonEventHandler(FingerButton sender);
    public event ButtonEventHandler ButtonActivated;
    public event ButtonEventHandler ButtonHovered;
    public event ButtonEventHandler ButtonHoverEnded;
    #endregion

    #region Fingertip Tracking Variables
    [Header("Fingertip Tracking Variables")]
    public List<GameObject> FingertipsInCollisionBounds;
    public List<FingerFilter> FingerTipFilters; // we might want to find a way to filter thumbs and pinkies?
    [SerializeField]
    private float furthestPushPoint;
    public float FurthestPushPoint { get { return furthestPushPoint; } }
    public List<float> fingerDots;

    public Chirality RespondToHands = Chirality.Either;
    public bool RespondToIndex = true;
    public bool RespondToMiddle = true;
    public bool RespondToThumb = false;
    public bool WaitingForReactivation = false;

    public HandModel HandModel;
    #endregion

    #region Button Variables
    [Header("Button Variables")]
    public float ButtonFaceDistance;
    public float ButtonThrowDistance;
    public float CurrentThrowValue;

    public bool CanHighlight = true;
    #endregion

    #region Visual Variables
    private Animator buttonAnimatorComponent;
    public TweenMaterialColor HighlightTweener;
    public Transform ButtonFace;
    #endregion

    #region Audio Variables
    [Header("Audio Variables")]
    public AudioClip HoverClip;
    public AudioClip ActivateClip;
    public AudioSource ThrowSource;
    public float PitchMin, PitchMax;
    #endregion

    #region Debug Variables
    public bool EnableDebugLogging = false;
    #endregion

    // Use this for initialization
	void Awake ()
    {
        fingerDots = new List<float>();
        FingertipsInCollisionBounds = new List<GameObject>();

        buttonAnimatorComponent = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // determine the furthest finger and see how 'far' we've pushed things.
        if (FingertipsInCollisionBounds.Count > 0)
        {
            furthestPushPoint = ButtonFaceDistance;
            for (int i = 0; i < FingertipsInCollisionBounds.Count; i++)
            {
                float fingerDot = Vector3.Dot(FingertipsInCollisionBounds[i].transform.forward,
                    transform.forward); // 1 is orthogonal, 0 is perpendicular, -1 is inverse orthogonal. -1 is what we want.
                fingerDots[i] = fingerDot;

                if (fingerDot > -0.7f) continue; // finger is probably facing down. Ignore it.
                Vector3 fingertipPosition = HandModel.fingers[(int)HandProperties.FingerTypeFromFingerFilter(FingerTipFilters[i])].GetTipPosition();
                fingertipPosition = transform.InverseTransformPoint(fingertipPosition);

                if (furthestPushPoint > fingertipPosition.z)
                {
                    furthestPushPoint = fingertipPosition.z;
                }
            }

            if (!WaitingForReactivation)
            {
                CurrentThrowValue = Mathf.InverseLerp(ButtonThrowDistance, ButtonFaceDistance, furthestPushPoint);
                //ThrowSource.volume = Mathf.Lerp(ThrowSource.volume, 1f, Time.deltaTime * 2);
                ThrowSource.volume = Mathf.Lerp(0f, 1f, MathSupplement.UnitReciprocal(CurrentThrowValue));
                ThrowSource.pitch = Mathf.Lerp(1f, 1.84f, MathSupplement.UnitReciprocal(CurrentThrowValue));
                if (furthestPushPoint < ButtonThrowDistance)
                {
                    Activate();
                }
            }
            else
            {
                if(furthestPushPoint >= ButtonThrowDistance)
                {
                    WaitingForReactivation = false;
                    if(EnableDebugLogging) Debug.Log("Re-activation allowed.");
                }
            }
        }
        else
        {
            ThrowSource.volume = 0;
            ThrowSource.pitch = 1;
        }
	}

    void LateUpdate()
    {
        if (FingertipsInCollisionBounds.Count > 0)
        {
            ButtonFace.transform.localPosition = new Vector3(0, furthestPushPoint, 0);
        }
        else
        {
            ButtonFace.transform.localPosition = Vector3.Lerp(ButtonFace.transform.localPosition, Vector3.up * ButtonFaceDistance, Time.deltaTime * 5);
        }
    }

    void OnFingertipTriggerEnter(FingertipData fingertipData)
    {
        bool acceptFingertip = false;
        if ((fingertipData.finger == FingerFilter.index) && (RespondToIndex)) acceptFingertip = true;
        if ((fingertipData.finger == FingerFilter.middle) && (RespondToMiddle)) acceptFingertip = true;
        if ((fingertipData.finger == FingerFilter.thumb) && (RespondToThumb)) acceptFingertip = true;

        if(acceptFingertip)
        {
            if (FingertipsInCollisionBounds.Count == 0)
            {
                HandModel = fingertipData.HandModel;
            }
            else
            {
                // check incoming hand model to make sure it matches the existing one.
                int oldHandModelID = HandModel.GetInstanceID();
                int newHandModelID = fingertipData.HandModel.GetInstanceID();
                if (oldHandModelID != newHandModelID)
                {
                    Debug.Log("Tried using two different hands at once to push a button. Ignoring second hand.");
                    return;
                }
                else
                {
                    int candidateID = fingertipData.Owner.GetInstanceID();                    
                    FingerFilter candidateFilter = fingertipData.finger;

                    for(int i=0; i < FingertipsInCollisionBounds.Count; i++)
                    {
                        FingerFilter knownFilter = FingerTipFilters[i];
                        int knownID = FingertipsInCollisionBounds[i].GetInstanceID();

                        if((knownID == candidateID) || (knownFilter == candidateFilter))
                        {
                            Debug.Log("Fingertip exists already, why wasn't it removed?");
                            return;
                        }
                    }
                }
            }

            FingertipsInCollisionBounds.Add(fingertipData.Owner);
            FingerTipFilters.Add(fingertipData.finger);
            fingerDots.Add(0);

            if (FingertipsInCollisionBounds.Count == 1)
            {
                Hover();
            }
        }
    }

    void OnFingertipTriggerStay(FingertipData fingertipData)
    {

    }

    void OnFingertipTriggerExit(FingertipData fingertipData)
    {
        for (int i = 0; i < FingertipsInCollisionBounds.Count; i++)
        {
            int knownID = FingertipsInCollisionBounds[i].GetInstanceID();
            int candidateID  = fingertipData.Owner.GetInstanceID();

            FingerFilter knownFilter = FingerTipFilters[i];
            FingerFilter candidateFilter = fingertipData.finger;

            if (knownID == candidateID)
            {
                FingertipsInCollisionBounds.RemoveAt(i);
                FingerTipFilters.RemoveAt(i);
                fingerDots.RemoveAt(i);

                if (FingertipsInCollisionBounds.Count == 0) 
                {
                    CancelHover();
                }
            }
        }
    }

    #region Functions
    public void Activate()
    {
        AudioSource.PlayClipAtPoint(ActivateClip, transform.position);
        WaitingForReactivation = true;
        if (ButtonActivated != null) ButtonActivated(this);
        if (EnableDebugLogging) Debug.Log("FingerButton: " + name + " activated.");
    }

    public void Hover()
    {
        buttonAnimatorComponent.SetTrigger("Highlight");
        AudioSource.PlayClipAtPoint(HoverClip, transform.position);
        HighlightTweener.enabled = true;
        if (ButtonHovered != null) ButtonHovered(this);
        if (EnableDebugLogging)
        {
            Debug.Log("FingerButton: " + name + " hovered.");
            Debug.Log("Re-activation disallowed.");
        }
    }

    public void CancelHover()
    {
        HighlightTweener.enabled = false;
        if (ButtonHoverEnded != null) ButtonHoverEnded(this);
        if (EnableDebugLogging) Debug.Log("FingerButton: " + name + " hover ended.");
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + (transform.forward * ButtonFaceDistance), 0.008f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + (transform.forward * ButtonThrowDistance), 0.008f);
    }
}
