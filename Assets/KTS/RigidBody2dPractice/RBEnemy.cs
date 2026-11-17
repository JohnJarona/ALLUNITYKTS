using UnityEngine;
using UnityEngine.UI;

public class RBEnemy : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _damage = 20;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private BoxCollider2D _collision;
    private float damageCd;
    public void TakeDamage(float damage)
    {
        _health = Mathf.Max(_health - damage, 0);
        _healthBar.value = _health;
        if (_health == 0)
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        _healthBar.maxValue = _health;
        _healthBar.value = _health;
    }
    public void FixedUpdate()
    {
        if (damageCd > 0)
        {
            damageCd += -Time.fixedDeltaTime;
            return;
        }
        RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position + _collision.offset, _collision.size * 1.25f, 0, Vector2.zero, 0, _playerMask);
        if (hit.collider != null)
        {
            hit.collider.GetComponent<RBPlayerControl>().TakeDamage(_damage);
            damageCd = 1;
        }
    }
}