using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickListener : MonoBehaviour
{
    // Start is called before the first frame update
    private RaycastHit hit;
    private Camera cam;
    public bool editor;

    

    void Start()
    {
        cam = GetComponent<Camera>();
#if UNITY_EDITOR
        editor = true;
#else
        editor = false;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (editor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit);
                Debug.Log(hit.collider.name.ToString());
                if (hit.collider.CompareTag("Collectable"))
                {
                    hit.collider.GetComponent<Collectable>().Collect();
                }

            }
        }
        if (!editor)
        {
            if (Input.touchCount > 0)
            {
                Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
                Physics.Raycast(ray, out hit);
                Debug.Log(hit.collider.name.ToString());
                if (hit.collider.CompareTag("Collectable"))
                {
                    hit.collider.GetComponent<Collectable>().Collect();
                }

            }
        }

    }
}
