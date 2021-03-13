using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBusSystem;

public class NotePedalsRaycaster : MonoBehaviour
{

    Camera _camera;
    [SerializeField]
    LayerMask _mask;
    int _raycastDistance = 9999; //fair long cast

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, _raycastDistance, _mask.value))
            {
                var noteButton = hit.transform.gameObject.GetComponent<NoteButton>();
                if (noteButton != null)
                {
                    EventBus.RaiseEvent<IUserInputHandler>(x => x.UpdatePlayerDestination(noteButton.note));
                }
            }
        }

    }
}
