using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace TownDefense
{
    public class PhysicsBouncerBehaviour : MonoBehaviour{
        [SerializeField]private float mult = 1;
        [SerializeField]private float frq = 1;
        [SerializeField]private Rigidbody2D _rb;
        public IEnumerator Start()
        {
            _rb.AddForce(new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f))*10000);
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1f / frq,5f / frq));
                _rb.AddForce(new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f))*10000 * mult);
            }
        }
    }
}