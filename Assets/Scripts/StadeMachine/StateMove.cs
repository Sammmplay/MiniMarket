using UnityEngine;

public class StateMove : State {
    Vector3 offset;
    float smootSpeed = 8;
    bool isDragging;
    public override void EnterState(ManagerState manager) {
        Debug.Log("Enter State Move");
        RectTransform _objPanelInstancias = GameObject.Find("PanelInstancias").GetComponent<RectTransform>();
        for (int i = 0; i < manager._panelText.Length; i++) {
            manager._panelText[i].SetActive(false);
        }
        if (manager._panelInstancias) {
            manager.AbrirPanelCreate(_objPanelInstancias);
        }
        if (manager._panelUpDownCreacion) {
            manager._abajo.onClick.Invoke();
        }
        for (int i = 0; i < manager._panelText.Length; i++) {
            manager._panelText[i].SetActive(false);
        }
        manager._panelText[0].SetActive(true);

    }
    public override void ExitState(ManagerState manager) {
        manager._seleccionado = null;
        manager._selected = false;
        manager._panelText[0].SetActive(false);
        if (!manager._selected) {
            manager._seleccionadoAntes.GetComponentInChildren<Light>().enabled = false;
        }
        Debug.Log("Saliendo");
    }
    public override void Update(ManagerState manager) {
        if (Input.GetMouseButton(0)) {
            Debug.Log("presiono boton desde Update StateMove");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                //si el objeto clickeado es el que queremos mover
                if (hit.collider.gameObject == manager._seleccionado) {
                    isDragging = true;
                    offset = manager._seleccionado.transform.position - hit.point;
                }
            }
        }
        //si estamos arrastrando, mover el objeto
        if (isDragging) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Vector3 targetPosition = hit.point + offset; // mover el objeto basado en la posicion del mouse
                targetPosition.y = manager._seleccionado.transform.position.y; // para no cambiar la posicion en y 
                //usamos lerp para mover suavemente el objeto
                manager._seleccionado.transform.position = Vector3.Lerp(manager._seleccionado.transform.position,
                    targetPosition, smootSpeed * Time.deltaTime);
            }
            if (Input.GetMouseButtonUp(0)) {
                isDragging = false;
            }
        }
    }
}
