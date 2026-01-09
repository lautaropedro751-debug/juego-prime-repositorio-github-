using System;
using System.Diagnostics;
using UnityEditor.Rendering;
using UnityEditor.Tilemaps;
using UnityEngine;

[DebuggerDisplay("{" + nameof(DebuggerDisplayAttribute) + "(),nq}")]
public class Player : MonoBehaviour
{
    //PLAYER COMPONENTS
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Transform m_transform;
    private Animator m_animator;

	[Header("Move And Jump settings")]
	[SerializeField] private float speed;
    private int direction = 1;
    [SerializeField] private float jumpForce;
    [SerializeField] private int extraJumps;
    [SerializeField] private int counterExtraJumps;
	private int idspeed;

	[Header("Ground Settings")]
	[SerializeField] private Transform lFoot;
	[SerializeField] private Transform rFoot;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask groundLayer;
	private int idIsGrounded;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        m_gatherInput = GetComponent<GatherInput>();
        m_transform = GetComponent<Transform>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        idspeed = Animator.StringToHash("speed");
        idIsGrounded = Animator.StringToHash("IsGrounded");
        lFoot = GameObject.Find("LFoot").GetComponent<Transform>();
        rFoot = GameObject.Find("RFoot").GetComponent<Transform>();
        counterExtraJumps = extraJumps;
    }

    private void Update()
    {
        SetAnimatorValues();
    }

    private void SetAnimatorValues()
    {
		m_animator.SetFloat(idspeed, Mathf.Abs(m_rigidbody2D.linearVelocityX));
        m_animator.SetBool(idIsGrounded, isGrounded);
	}

    // Update is called once per frame
    void FixedUpdate()  
    {
        Move();
        jump();
        CheckGround();
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
	private void jump()
	{
		if (m_gatherInput.IsJumping)
        {
            if (isGrounded)
               m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce);
            if (counterExtraJumps > 0)
            {
				m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce);
                counterExtraJumps--;
            }
        }
        m_gatherInput.IsJumping = false;
	 
	}
	private void CheckGround()
	{
        RaycastHit2D lFootRay = Physics2D.Raycast(lFoot.position, Vector2.down, rayLength, groundLayer);
		RaycastHit2D rFootRay = Physics2D.Raycast(rFoot.position, Vector2.down, rayLength, groundLayer);
        if (rFootRay || rFootRay) 
        { 
            isGrounded = true;
            counterExtraJumps = extraJumps;
        }
        else
		{
            isGrounded = false;
        }
	}

}
