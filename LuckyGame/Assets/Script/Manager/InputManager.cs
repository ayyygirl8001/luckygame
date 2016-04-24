using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public GameObject gameManager;
    private int layerMask = -1;
    
	void Awake ()
    {
        layerMask = LayerMask.NameToLayer("Target");
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RayCasting(ray, "TouchingObject");
            Debug.Log("GetMouseButtonDown");
        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RayCasting(ray, "TouchedObject");
            Debug.Log("GetMouseButtonUp");
        }
	}

    void RayCasting(Ray ray, string method)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("Raycast: Layer("+layerMask.ToString()+") ObjectLayer("+ hit.transform.gameObject.layer.ToString()+")");
            if (hit.transform.gameObject.layer == layerMask)
            {
                Debug.Log("LayerMask");
                gameManager.SendMessage(method, hit.transform.gameObject);
            }
        }
    }
}
