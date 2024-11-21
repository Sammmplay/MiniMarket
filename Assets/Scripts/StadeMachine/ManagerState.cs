using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class ManagerState : MonoBehaviour
{
    public  State _currentState;
    StateMove _stateMove = new StateMove();
    StateRotar _stateRotar = new StateRotar();
    StateEliminar _stateEliminar = new StateEliminar();
    stateScale _stateScale = new stateScale();
    [SerializeField] public Button _abajo;
    [SerializeField] public GameObject[] _panelText;
    [SerializeField] GameObject[] _esferaObject;
    public Vector2 _positionPanelInstanceFuera = new Vector3(490, -20, 0);
    public Vector2 _positionPanelinstanceDentro = new Vector3(-90,-20,0);
    public Vector2 _positionPanelCentroCreacion = new Vector3(0, 50, 0);
    public Vector2 _positionPanelDowCreacion = new Vector3(0, -150, 0);
    public GameObject _seleccionado;
    public GameObject _seleccionadoAntes;
    public Transform _piso;
    [SerializeField] public bool _selected;
    [SerializeField] public bool _panelUpDownCreacion;
    [SerializeField] public bool _panelInstancias;
    [SerializeField] public bool _esferaInstanciada;
    private void Update() {
        if(_currentState != null) {
            _currentState.Update(this);
        }
        SelectionObject();
    }
    //metodo para cambiar estado
    public void ChangeState(State newState) {

        _currentState = newState;
        if (_currentState == null) Debug.Log("no existe estado actual");
        if (_currentState == null) return;
        _currentState.EnterState(this);
    }
    public void UPDowPanelCreacion(RectTransform obj) {
        _panelUpDownCreacion = !_panelUpDownCreacion;
        if (_panelUpDownCreacion) {
            LeanTween.move(obj, _positionPanelCentroCreacion, 0.8f).setEase(LeanTweenType.easeInBounce);
        } else {
            LeanTween.move(obj, _positionPanelDowCreacion, 0.2f);
        }
    }
    public void AbrirPanelCreate(RectTransform obj) {
        _panelInstancias = !_panelInstancias;
        if (_panelInstancias){
            LeanTween.move(obj, _positionPanelinstanceDentro, 0.8f).setEase(LeanTweenType.easeInBounce);
        } else {
            LeanTween.move(obj, _positionPanelInstanceFuera, 0.2f);
        }
    }
    public void EnterCreate(GameObject prefab) {
        GameObject obj = Instantiate(prefab, _piso);
    }
    public void EnterMove() {
        if (!_selected && !_seleccionado) return;
        _currentState = null;
        //EnterButoonCreacion();
        ChangeState(_stateMove);
    }
    public void EnterButoonCreacion() {
        RectTransform _panelCreate = GameObject.Find("PanelInstancias").GetComponent<RectTransform>();
        AbrirPanelCreate(_panelCreate);
        //_abajo.onClick.Invoke();
    }

    public void SelectionObject() {
        if (_currentState != null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0)) {
            if (!EventSystem.current.IsPointerOverGameObject()) {//para verificar si el mouse esta sobre un elemento de la UI
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Selected")) {
                    //si hay un objeto previamente seleccionado, apagar su luz
                    if(_seleccionadoAntes != null && _seleccionadoAntes != hit.collider.gameObject) {
                        Light ligth = _seleccionadoAntes.GetComponentInChildren<Light>();
                        if(ligth != null) {
                            ligth.enabled = false;
                        }
                    }
                    //actualizar el objeto seleccionado actual y encender su luz
                    _seleccionado = hit.collider.gameObject;
                    if (!_esferaInstanciada)
                    {
                        _esferaObject[1] = Instantiate(_esferaObject[0], _seleccionado.transform);
                        _esferaObject[1].transform.SetPositionAndRotation(hit.transform.position, Quaternion.identity);
                        Vector3 _esferaScale = new Vector3(_esferaObject[1].transform.localScale.x - 0.5f, _esferaObject[1].transform.localScale.y, _esferaObject[1].transform.localScale.z - 0.5f);
                        LeanTween.scale(_esferaObject[1], _esferaScale, 1f).setLoopPingPong().setEase(LeanTweenType.easeInOutSine);
                        _esferaInstanciada = true;
                    }
                    
                    _selected = true;
                    Light selectedLigth = _seleccionado.GetComponentInChildren<Light>(); 
                    if(selectedLigth != null) {
                        selectedLigth.enabled = true;
                    }
                    //guardar el objeto actual como el objeto anterior para futuras referencias
                    _seleccionadoAntes = _seleccionado;

                } else if (!hit.collider.CompareTag("Selected")) {
                    //deselecionar el objeto si se hace click fuera de un objeto con el tag "Selected"
                    _selected = false;
                    if(_seleccionado != null) {
                        Light ligth = _seleccionadoAntes.GetComponentInChildren< Light>();
                        if (ligth != null) {
                            ligth.enabled = false;
                        }
                    }
                    if (_esferaObject != null)
                    {
                        Destroy(_esferaObject[1]);
                        _esferaInstanciada = false;
                    }
                    _seleccionado = null;
                    _seleccionadoAntes = null;
                }
            }
        }
    }
    public void AgregarOQuitarLuces() {
        if (_seleccionado == null) return;
        _seleccionadoAntes = _seleccionado;
        Light ligt = _seleccionadoAntes.GetComponentInChildren<Light>();
        ligt.enabled = _seleccionado;
    }
    public void ButtonCancelarMove() {
        _currentState.ExitState(this);
        _currentState = null;
    }
    public void ButtonAceptarEliminar() {
        _currentState.ExitState(this);
    }
    public void ButtonCancelarRotar() {
        if (!_selected) {
            _seleccionadoAntes.GetComponentInChildren<Light>().enabled = false;
        }
        _currentState.ExitState(this);
    }
    public void EnterStateRotar() {
        if (_seleccionado == null) return;
        ChangeState(_stateRotar);
    }
    public void EnterStateEliminar() {
        if (_seleccionado == null) return;
        ChangeState(_stateEliminar);
    }
   public void stateScale()
    {
        if (!_selected && !_seleccionado) return;
        ChangeState(_stateScale);
    }
    public void ButtonCancelScale()
    {
        _currentState.ExitState(this);
    }
    
}
