
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RBPlayerControl : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform _swordTr;
    [SerializeField] private RBProjectile _swordProjectile;


    private float _inputPos;
    private bool _isJumped;
    public void Start()
    {
        _healthBar.maxValue = _health;
        _healthBar.value = _health;
    }
    public void TakeDamage(float damage)
    {
        _health = Mathf.Max(_health - damage, 0);
        _healthBar.value = _health;
        if (_health == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void Update()
    {
        _inputPos = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _isJumped = true;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RBProjectile projectile = GameObject.Instantiate(_swordProjectile);
            projectile.transform.position = _swordTr.position;
            projectile.Shoot(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position);
        }
    }
    public void FixedUpdate()
    {
        RaycastHit2D  ray = Physics2D.Raycast(transform.position, Vector2.down, 10, _groundMask);
        
        if (_isJumped && ray.collider != null && ray.distance < 0.6f)
        {
            _isJumped = false;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.y, 15);
        }
        _rb.linearVelocity = new Vector2(_inputPos * 10, _rb.linearVelocity.y);
    }
}