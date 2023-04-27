using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityComponent : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private Tile m_newParent;

    private void OnEnable()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_newParent = GridManager.GetTile(GetComponent<Candy>());
        m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
        m_rigidbody.gravityScale = 1;
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.y - m_newParent.transform.position.y) <= 0.1f)
        {
            m_rigidbody.gravityScale = 0;
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
            transform.position = m_newParent.transform.position;
            this.enabled = false;
        }
    }

}
