using UnityEngine;

public class Player : MonoBehaviour
{
	// COMPONENTES
	private Rigidbody2D rb;
	private GatherInput input;
	private Transform tr;
	private Animator anim;

	[Header("Move And Jump Settings")]
	[SerializeField] private float speed = 5f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private int extraJumps = 1;

	private int counterExtraJumps;
	private int direction = 1;

	[Header("Ground Settings")]
	[SerializeField] private Transform lFoot;
	[SerializeField] private Transform rFoot;
	[SerializeField] private float rayLength = 0.2f;
	[SerializeField] private LayerMask groundLayer;

	private bool isGrounded;
	private bool wasGrounded;

	// Animator IDs
	private int idSpeed;
	private int idIsGrounded;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		input = GetComponent<GatherInput>();
		tr = transform;
		anim = GetComponent<Animator>();

		idSpeed = Animator.StringToHash("speed");
		idIsGrounded = Animator.StringToHash("IsGrounded");

		counterExtraJumps = extraJumps;
	}

	void Update()
	{
		UpdateAnimator();
	}

	void FixedUpdate()
	{
		CheckGround();
		Move();
		Jump();
	}

	// ───────── MOVIMIENTO ─────────
	private void Move()
	{
		Flip();
		rb.linearVelocity = new Vector2(speed * input.ValueX, rb.linearVelocityY);
	}

	private void Flip()
	{
		if (input.ValueX * direction < 0)
		{
			direction *= -1;
			tr.localScale = new Vector3(direction, 1, 1);
		}
	}

	// ───────── SALTO (FIX DEFINITIVO) ─────────
	private void Jump()
	{
		if (!input.IsJumping)
			return;

		// Puede saltar si está en suelo o tiene saltos disponibles
		if (isGrounded || counterExtraJumps > 0)
		{
			rb.linearVelocity =
				new Vector2(rb.linearVelocityX, 0);

			rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

			// Si saltó en el aire, consume salto extra
			if (!isGrounded)
				counterExtraJumps--;
		}

		// Consumir input
		input.IsJumping = false;
	}

	// ───────── GROUND CHECK ─────────
	private void CheckGround()
	{
		RaycastHit2D leftRay =
			Physics2D.Raycast(lFoot.position, Vector2.down, rayLength, groundLayer);
		RaycastHit2D rightRay =
			Physics2D.Raycast(rFoot.position, Vector2.down, rayLength, groundLayer);

		isGrounded = leftRay || rightRay;

		// Acaba de tocar el suelo
		if (isGrounded && !wasGrounded)
		{
			counterExtraJumps = extraJumps;
		}

		// Acaba de dejar el suelo (cayó sin saltar o saltó)
		if (!isGrounded && wasGrounded)
		{
			counterExtraJumps = extraJumps;
		}

		wasGrounded = isGrounded;
	}

	// ───────── ANIMATOR ─────────
	private void UpdateAnimator()
	{
		anim.SetFloat(idSpeed, Mathf.Abs(rb.linearVelocityX));
		anim.SetBool(idIsGrounded, isGrounded);
	}
}