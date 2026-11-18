using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ExplosionClick : MonoBehaviour
{
    [SerializeField] private PointEffector2D _effector;
    [SerializeField] private CircleCollider2D _collider;
    public IEnumerator Start()
    {
        DOTween.To(() => _collider.radius, x => _collider.radius = x, 2, 0.1f);
        yield return new WaitForSeconds(0.1f);
        DOTween.To(() => _collider.radius, x => _collider.radius = x, 0, 0.1f);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
