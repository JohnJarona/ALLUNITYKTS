using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDropController : MonoBehaviour
{
    [SerializeField] private PointEffector2D _effector;
    [SerializeField] private Transform _effectTr;
    public void Update()
    {
        _effectTr.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _effector.enabled = true;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _effector.enabled = false;
        }
    }
    public void Start()
    {
        _effector.enabled = false;
    }
}
