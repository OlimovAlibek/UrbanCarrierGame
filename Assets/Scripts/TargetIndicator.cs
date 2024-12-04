using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    // The target object the indicator will point to
    [SerializeField]
    public GameObject Target = null;

    // Update is called once per frame
    void Update()
    {
        // Check if a target is assigned
        if (Target != null)
        {
            // Calculate the direction vector from the indicator to the target
            var dir = Target.transform.position - transform.position;

            // Calculate the angle (in degrees) of the direction vector
            // Mathf.Atan2 gives the angle in radians; convert it to degrees
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // Rotate the indicator to point at the target using the calculated angle
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
