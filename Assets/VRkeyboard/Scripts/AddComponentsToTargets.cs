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
using System.Reflection;

/// <summary>
/// Simple script that allows you to add components to a series
/// of targets. Useful for if you need to modify a prefab at
/// startup without breaking things.
/// </summary>
public class AddComponentsToTargets : MonoBehaviour 
{
    public GameObject[] Targets;
    public string ComponentTypeToAdd;

	// Use this for initialization
	void Awake () 
    {
        string assemblyName = Assembly.GetExecutingAssembly().FullName;
        System.Type componentType = Types.GetType(ComponentTypeToAdd, assemblyName);
	    for(int i=0; i < Targets.Length; i++)
        {
            GameObject target = Targets[i];
            target.AddComponent(componentType);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
