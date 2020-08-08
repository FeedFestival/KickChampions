using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPoint : MonoBehaviour
{
    public Transform ShootDirection;
    private Camera _mainCamera;

    // Use this for initialization
    void Start()
    {
        _mainCamera = Camera.main;
        var cPos = _mainCamera.transform.position;
        transform.LookAt(cPos);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

    }

    void OnMouseDown()
    {
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.name == gameObject.name)
            {
                ShootDirection.LookAt(hit.point);
                ShootDirection.eulerAngles = new Vector3(0, ShootDirection.eulerAngles.y, 0);
            }
        }
    }
}
