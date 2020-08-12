using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSensor : MonoBehaviour
{
    public Transform Icosphere;
    private int _layermask;
    private Vector3 _cameraPos;

    // Start is called before the first frame update
    void Awake()
    {
        _layermask = LayerMask.GetMask("3DUI");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = Game.Instance.CameraPositioning.Camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layermask))
        {
            Debug.DrawRay(_cameraPos, hit.point - _cameraPos, Color.red);
            Transform objectHit = hit.transform;
        }
    }

    void LateUpdate()
    {
        _cameraPos = Game.Instance.CameraPositioning.transform.position;
        Icosphere.LookAt(_cameraPos);
    }
}
