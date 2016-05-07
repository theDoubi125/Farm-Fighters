using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

    [SerializeField]
    private Transform Target;

    [SerializeField]
    private float Height = -5;

	// Update is called once per frame
	void Update () {
        if(transform != null)
            transform.position = new Vector3(Target.position.x, Target.position.y, Height);
	}
}
