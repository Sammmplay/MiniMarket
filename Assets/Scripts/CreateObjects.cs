using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreateObjects : MonoBehaviour
{
    Transform _piso;
    public int distance;
    public Ray ray;
    public RaycastHit hit;
    public GameObject seleccionObjt;
    public bool selected;
    private void Start()
    {
        _piso = GameObject.Find("Piso").transform;
    }
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                seleccionObjt = hit.collider.gameObject;
                selected = true;

            }
        }
    }
    public void CreateObjetos(GameObject obj)
    {
        GameObject Instancia = Instantiate(obj,_piso);
    }
    public void DeleteObject()
    {
        Destroy(seleccionObjt.gameObject);
        
    }
}
