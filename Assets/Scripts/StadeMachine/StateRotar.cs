using UnityEngine;

public class StateRotar : State
{
    public override void EnterState(ManagerState manager) {
        RectTransform panelInstancia = GameObject.Find("PanelInstancias").GetComponent<RectTransform>();
        RectTransform panel_creacion = GameObject.Find("PanelCreacion").GetComponent<RectTransform>();
        for (int i = 0; i < manager._panelText.Length; i++) {
            manager._panelText[i].SetActive(false);
        }
        if (manager._panelInstancias) {
            manager.AbrirPanelCreate(panelInstancia);
        }
        LeanTween.move(panel_creacion, new Vector3(0, 125, 0), 0.4f);
        
        manager._panelText[1].SetActive(true);
        if (manager._seleccionado == null && !manager._selected) return;
        LeanTween.rotate(manager._seleccionado, manager._seleccionado.transform.eulerAngles+
            new Vector3(0,90,0),0.4f).setEase(LeanTweenType.easeInOutCirc);
    }
    public override void ExitState(ManagerState manager) {
        RectTransform _panelCreate = GameObject.Find("PanelCreacion").GetComponent<RectTransform>();
        manager._panelInstancias = false;
        LeanTween.move(_panelCreate, new Vector3(0, 50, 0), 0.5f);
        manager._panelText[1].SetActive(false);
        manager.ChangeState(null);
    }
    public override void Update(ManagerState manager) {
        
    }
}
