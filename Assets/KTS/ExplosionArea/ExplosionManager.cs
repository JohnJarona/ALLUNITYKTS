using UnityEngine;
using UnityEngine.InputSystem;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField] private ExplosionClick _explosionPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ExplosionClick exp = GameObject.Instantiate(_explosionPrefab);
            Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            pos.z = -1;
            exp.transform.position = pos;
        }
    }
}
