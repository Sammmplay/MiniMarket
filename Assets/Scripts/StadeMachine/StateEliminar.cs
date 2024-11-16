using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateEliminar : State
{
    public override void EnterState(ManagerState manager) {
        //primero preguntamos si desea eliminar o no 
        RectTransform panelDeOpciones = GameObject.Find("PanelCreacion").GetComponent<RectTransform>();
        if (manager._panelInstancias) {
            manager.EnterButoonCreacion();
        }
        if (manager._panelText[1].activeSelf) {
            manager._panelText[1].SetActive(false);
            LeanTween.move(panelDeOpciones, manager._positionPanelCentroCreacion, 0.2f);
        }
        manager._panelText[2].SetActive(true);
    }
    public override void ExitState(ManagerState manager) {
        Object.Destroy(manager._seleccionado);
        manager._panelText[2].SetActive(false);
        manager._seleccionado = null;
        manager._selected = false;
        manager.ChangeState(null);
    }
    public override void Update(ManagerState manager) {
        
    }

}
