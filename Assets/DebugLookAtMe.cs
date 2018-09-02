using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLookAtMe : MonoBehaviour {

    private void Update() {
        transform.rotation = Quaternion.LookRotation((transform.position - Camera.main.transform.position).normalized, new Vector3(0.0f, 1.0f, 0.0f));
    }
}
