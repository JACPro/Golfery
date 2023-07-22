using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnSelectEvents : MonoBehaviour, ISelectHandler, IPointerClickHandler, ISubmitHandler
{
    [SerializeField] private Selectable _selectable;
    public static bool _initialSelect = true;

    public UnityEvent _onSelect, _onClick, _onSubmit;

    private void Awake()
    {
        if (_selectable == null)
        {
            if (!TryGetComponent<Selectable>(out _selectable))
            {
                Debug.Log($"<color=#FF0000>MISSING SELECTABLE REFERENCE</color>", gameObject);
                return;
            }    
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (_initialSelect)
        {
            _initialSelect = false;
            return;
        }

        if (!(eventData is PointerEventData)) //ignore select events for mouse as we use PointerClick instead
        {
            _onSelect.Invoke();
        }

    }

    public static void ResetInitialSelect()
    {
        _initialSelect = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onClick.Invoke();        
    }

    public void OnSubmit(BaseEventData eventData)
    {
        _onSubmit.Invoke();
    }
}
