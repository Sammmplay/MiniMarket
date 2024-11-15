using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



public class CreateObjects : MonoBehaviour
{
    Transform _piso;
    [SerializeField] Plane _planoMovimiento;
    Vector3 offset;
    bool active;
    [SerializeField] bool _selected;
    [SerializeField] public bool _activeMoveBooton;
    bool isDragging = false;
    [SerializeField] float smootSpeed = 8;
    public GameObject seleccionObjt;
    private void Start()
    {
        _piso = GameObject.Find("Piso").transform;
        _planoMovimiento = new Plane(Vector3.up, Vector3.zero);
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        SelecionarObject();
        MoverObjeto();
    }
    public void SelecionarObject() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) {
            if (!EventSystem.current.IsPointerOverGameObject()) {//para verificar si el mouse esta sobre un elemento de la UI
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Selected")) {
                    _selected = true;
                    seleccionObjt = hit.collider.gameObject;
                } else if (hit.collider != null || !hit.collider.CompareTag("Selected")) {
                    _selected = false;
                    seleccionObjt = null;
                    _activeMoveBooton = false;
                }
            }
        }
    }
    public void MoverObjeto() {
        if (!_activeMoveBooton) return;
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                //si el objeto clickeado es el que queremos mover
                if (hit.collider.gameObject == seleccionObjt) {
                    isDragging = true;
                    offset = seleccionObjt.transform.position - hit.point;
                }
            }
        }
        //si estamos arrastrando, mover el objeto
        if (isDragging) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Vector3 targetPosition = hit.point + offset; // mover el objeto basado en la posicion del mouse
                targetPosition.y =seleccionObjt.transform.position.y; // para no cambiar la posicion en y 
                //usamos lerp para mover suavemente el objeto
                seleccionObjt.transform.position = Vector3.Lerp(seleccionObjt.transform.position,
                    targetPosition, smootSpeed * Time.deltaTime);
            }
            if (Input.GetMouseButtonUp(0)) {
                isDragging = false;
            }
        }
    }
    public void MoverPanelInicioUp(RectTransform obj  ) {
        LeanTween.move(obj, new Vector3(0,50,0), 1.2f).setEase(LeanTweenType.easeInBounce);
    }
    public void DowPanelInicio(RectTransform obj) {
        LeanTween.move(obj, new Vector3(0, -150, 0), 1.2f).setEase(LeanTweenType.easeInBounce); ;
    }
    public void BotonCrear(RectTransform obj) {
        active = !active;
        if (!active) {
            LeanTween.move(obj, new Vector3(490, -20, 0), 1.2f).setEase(LeanTweenType.easeInBounce); ;
        } else {
            LeanTween.move(obj, new Vector3(-86, -20, 0), 1.2f).setEase(LeanTweenType.easeInBounce); ;

        }
        _activeMoveBooton = false;
    }

    public void CreateObjetos(GameObject obj)
    {
        GameObject Instancia = Instantiate(obj,_piso);
        _activeMoveBooton = false;
    }
    public void MoverObject() {
        if (seleccionObjt == null && !_selected) return;
        
        _activeMoveBooton = !_activeMoveBooton;
    }
    public void RotarObject() {
        if (seleccionObjt == null) return;
        LeanTween.rotate(seleccionObjt, seleccionObjt.transform.eulerAngles+new Vector3(0,90,0),0.8f );
        _activeMoveBooton = false;
    }
    public void DeleteObject() {
        if (seleccionObjt == null) return;
        Destroy(seleccionObjt.gameObject);
        seleccionObjt = null;
        _selected = false;
        _activeMoveBooton = false;
    }
}
