using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectInteractionTests
{
    private ObjectInteractionUtilityFunctions _utilityFunctions = new(null, null, null, null, null);
    
    [Test]
    public void ObjectInteractionTests_IsLeftCloser_True()
    {
        Vector3 pointOfContact = new Vector3(0, 1, 0);
        Vector3 leftPos = new Vector3(0, 0, 0);
        Vector3 rightPos = new Vector3(0, 10, 0);
        
        Assert.IsTrue(_utilityFunctions.IsLeftCloser(pointOfContact, leftPos, rightPos));
    }
    
    [Test]
    public void ObjectInteractionTests_IsLeftCloser_False()
    {
        Vector3 pointOfContact = new Vector3(0, 1, 0);
        Vector3 leftPos = new Vector3(0, 5, 0);
        Vector3 rightPos = new Vector3(0, 4, 0);
        
        Assert.IsFalse(_utilityFunctions.IsLeftCloser(pointOfContact, leftPos, rightPos));
    }
    
    [Test]
    public void ObjectInteractionTests_IsLeftCloser_Equal()
    {
        Vector3 pointOfContact = new Vector3(0, 1, 0);
        Vector3 leftPos = new Vector3(0, 5, 0);
        Vector3 rightPos = new Vector3(0, 5, 0);
        
        Assert.IsFalse(_utilityFunctions.IsLeftCloser(pointOfContact, leftPos, rightPos));
    }
}
