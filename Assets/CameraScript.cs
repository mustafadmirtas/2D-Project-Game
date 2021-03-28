using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform _target;
    public Vector3 offset;
    public float smooth;
    // Update is called once per frame
    void FixedUpdate()
    {
        Follow();
    }
    private void Follow()
    {
        Vector3 targetPos = _target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, smooth * Time.fixedDeltaTime);
        transform.position = smoothPos;
    }
}
