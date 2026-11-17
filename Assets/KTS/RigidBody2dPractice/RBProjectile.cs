using UnityEngine;

public class RBProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _sprTr;
    [SerializeField] private LayerMask _enemyMask;
    private bool _isStopped;
    private bool _isHitted;
    private float _rotSpd = 10f;
    public void Start()
    {
        Destroy(gameObject, 10f);
    }
    public void Shoot(Vector2 dir)
    {
        _rb.linearVelocity = dir.normalized  * _speed;
    }
    public void SetStopped()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 10f);
        _isStopped = true;
        _rb.gravityScale = 4;
        _rotSpd += 0.5f;
    }
    public void FixedUpdate()
    {
        if (!_isHitted)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 10, _enemyMask);
            if (hit.collider != null)
            {
                hit.collider.GetComponent<RBEnemy>().TakeDamage(15);
                SetStopped();
                _isHitted = true;
            }
        }
        if (!_isStopped)
        {

            _rb.linearVelocity *= 0.9f;
            _rotSpd *= 0.9f;
            
            if (_rb.linearVelocity.sqrMagnitude < 0.01f)
            {
                SetStopped();
            }
        }
        _sprTr.eulerAngles += new Vector3(0,0,_rotSpd * 10f);
    }
}