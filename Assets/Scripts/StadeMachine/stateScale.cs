using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class stateScale : State
{
    Vector3 offset;
    float smootSpeed = 8;
    bool isDragging;

    public override void EnterState(ManagerState manager)
    {
        Debug.Log("StateScale");
        RectTransform _objPanelInstancias = GameObject.Find("PanelInstancias").GetComponent<RectTransform>();
        for (int i = 0; i < manager._panelText.Length; i++) {
            manager._panelText[i].SetActive(false);
        }
        manager._panelText[3].SetActive(true);
        if (manager._panelInstancias)
        {
            manager.AbrirPanelCreate(_objPanelInstancias);
        }
    }

    public override void ExitState(ManagerState manager)
    {
        manager._panelText[3].SetActive(false);
        manager._currentState = null;
    }
    public override void Update(ManagerState manager)
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("presiono boton desde Update StateScale");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //si el objeto clickeado es el que queremos mover
                if (hit.collider.gameObject == manager._seleccionado)
                {
                    isDragging = true;
                    //offset = manager._seleccionado.transform.position - hit.point;
                }
            }
        }
        //si estamos arrastrando, mover el objeto
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Vector3 targetPosition = hit.point.normalized; // mover el objeto basado en la posicion del mouse
                Vector3 targetPosition = Input.mousePosition.normalized;
                offset = targetPosition - hit.point;
                Debug.Log("targetposition "+targetPosition.y);
                Debug.Log("offset " + offset.y);
                if (targetPosition.y == 0) return;
                if (targetPosition.y < offset.y)
                {
                    Debug.Log("newposition es menor q positionantes ");
                    if (manager._seleccionado.transform.localScale.x <= 1.5f &&
                        manager._seleccionado.transform.localScale.y <= 1.5f &&
                        manager._seleccionado.transform.localScale.z <= 1.5f)
                    {
                        LeanTween.scale(manager._seleccionado, manager._seleccionado.transform.localScale + new Vector3(0.2f, 0.2f, 0.2f), 0.8f);
                    }
                       
                }
                else if (targetPosition.y > offset.y)
                {
                    Debug.Log("positionantes es mayor targetposition");
                    if(manager._seleccionado.transform.localScale.x > 0.5f &&
                        manager._seleccionado.transform.localScale.y > 0.5f &&
                        manager._seleccionado.transform.localScale.z > 0.5f)
                    {
                        LeanTween.scale(manager._seleccionado,manager._seleccionado.transform.localScale - new Vector3(0.2f, 0.2f, 0.2f), 0.8f);
                    }
                    
                }
                
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }

    }

}
