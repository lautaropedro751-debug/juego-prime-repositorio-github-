using System;
using System.Diagnostics;
using UnityEditor.Rendering;
using UnityEditor.Tilemaps;
using UnityEngine;

[DebuggerDisplay("{" + nameof(DebuggerDisplayAttribute) + "(),nq}")]
public class Player : MonoBehaviour
{
    //COMPONENTS
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Transform m_transform;
    private Animator m_animator;

    //VALUES
    [SerializeField] private float speed;
    private int direction = 1;
    private int idspeed;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        m_gatherInput = GetComponent<GatherInput>();
        m_transform = GetComponent<Transform>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        idspeed = Animator.StringToHash("speed");
    }

    private void Update()
    {
        SetAnimatorValues();
    }

    private void SetAnimatorValues()
    {
		m_animator.SetFloat(idspeed, Mathf.Abs(m_rigidbody2D.linearVelocityX));
	}

    // Update is called once per frame
    void FixedUpdate()  
    {
        Move();
    }

    private void Move()
    {
        Flip();
		m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, m_rigidbody2D.linearVelocityY);
	}

    private void Flip()
    {
        if (m_gatherInput.ValueX * direction < 0)
        {
            m_transform.localScale = new Vector3(-m_transform.localScale.x, 1, 1);
            direction *= -1;
        }

    }
}