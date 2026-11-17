using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private float _speed;
    public void Update()
    {
        if (_followTarget == null) return;
        Vector3 pos = Vector3.Lerp(transform.position, _followTarget.position, 1 - Mathf.Exp(-_speed * Time.deltaTime));
        pos.z = -10;
        transform.position = pos;
    }
}