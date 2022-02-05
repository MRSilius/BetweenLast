using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Vector3 _groundCheckOffset;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _characterWidth;
    [SerializeField] private float _characterHeight;

    [SerializeField] private Vector3 _wallRightCheckOffset;
    [SerializeField] private Vector3 _wallLeftCheckOffset;
    [SerializeField] private float _wallCheckDistance;

    [SerializeField] private LayerMask groundMask;

    private Vector3 _input;
    [SerializeField] private bool _canMove;
    private bool _isMoving;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _walljump;
    [SerializeField] private bool _onWall;
    [SerializeField] private bool _onWallRight;
    [SerializeField] private bool _onWallLeft;
    [SerializeField] private float _currentTimeAtFly;
    [SerializeField] private float _timeAtFly;

    private Rigidbody2D _rigidbody;
    private CharacterAnimations _animations;
    [SerializeField] private AudioSource _walkAudioSource;
    [SerializeField] private AudioSource _soundAudioSource;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private PhysicsMaterial2D _physMaterial;
    [SerializeField] private Transform[] _characterSprites;
    private bool _gameStarted;
    //private List<SpriteRenderer> _characterSprites;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animations = GetComponent<CharacterAnimations>();

        //_characterSprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }

    private void Update()
    {
        Move();
        CheckGround();
        CheckWall();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        _animations.IsMoving = _isMoving;
        _animations.IsGrounded = _isGrounded;
        _animations.OnWall = _onWall;
        _animations.IsFlying = IsFlying();

        //Âûםוסעט מעהוכüםמ
        if (_rigidbody.velocity.x != 0 && _isGrounded)
        {
            _walkAudioSource.gameObject.SetActive(true);
        }
        else
        {
            _walkAudioSource.gameObject.SetActive(false);
        }
    }

    private bool IsFlying()
    {
        if (_rigidbody.velocity.y < -.1f && !_isGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckGround()
    {
        Vector3 rayStartPosition = transform.position + _groundCheckOffset;
        RaycastHit2D[] hits = new RaycastHit2D[3];
        float step = _characterWidth / hits.Length;

        /*RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down * 10, 10, groundMask);

        if (hit.collider != null)
        {
            Debug.DrawLine(transform.position, hit.normal, Color.yellow, 10.0f);
        }*/

        for (int i = 0; i < hits.Length; i++)
        {
            Vector2 rPos = new Vector2(rayStartPosition.x + (step * i), rayStartPosition.y);
            hits[i] = Physics2D.Raycast(rPos, Vector3.down * _groundCheckDistance, _groundCheckDistance, groundMask);

            if (hits[i].collider != null)
            {
                _isGrounded = hits[i].collider.CompareTag("Ground") || hits[i].collider.CompareTag("Interactions") ? true : false;
                break;
            }
            else
            {
                _isGrounded = false;
            }
        }

        if (_isGrounded)
        {
            _physMaterial.friction = .7f;
        }
        else
        {
            _physMaterial.friction = .1f;
        }

        //RaycastHit2D hit = Physics2D.Raycast(rayStartPosition, Vector3.down * _groundCheckDistance, _groundCheckDistance, groundMask);

    }

    private void CheckWall()
    {
        Vector3 rayRightStartPosition = transform.position + _wallRightCheckOffset;
        Vector3 rayLeftStartPosition = transform.position + _wallLeftCheckOffset;

        RaycastHit2D[] wallRightHits = new RaycastHit2D[2];
        RaycastHit2D[] wallLeftHits = new RaycastHit2D[2];

        float hStep = _characterHeight / wallRightHits.Length;
        for (int i = 0; i < wallRightHits.Length; i++)
        {
            Vector2 rPosR = new Vector2(rayRightStartPosition.x, rayRightStartPosition.y + (hStep * i));
            wallRightHits[i] = Physics2D.Raycast(rPosR, Vector3.right * _wallCheckDistance, _wallCheckDistance, groundMask);
            //Gizmos.DrawRay(rPosR, Vector3.right * _wallCheckDistance);

            Vector2 rPosL = new Vector2(rayLeftStartPosition.x, rayLeftStartPosition.y + (hStep * i));
            wallLeftHits[i] = Physics2D.Raycast(rPosL, Vector3.left * _wallCheckDistance, _wallCheckDistance, groundMask);
            //Gizmos.DrawRay(rPosL, Vector3.left * _wallCheckDistance);


            if (wallRightHits[i].collider != null || wallLeftHits[i].collider != null)
            {
                if (_isGrounded) return;
                if (!_walljump)
                {
                    if((wallRightHits[i].collider != null && wallRightHits[i].collider.CompareTag("ClimbingWall")) || (wallLeftHits[i].collider != null && wallLeftHits[i].collider.CompareTag("ClimbingWall")))
                    {
                        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
                        print("ZERO");
                    }
                    if (!_onWall)
                    {
                        if (wallRightHits[i].collider != null)
                        {
                            FlipSprite(true);
                        }
                        if (wallRightHits[i].collider != null)
                        {
                            FlipSprite(false);
                        }
                    }
                }

                if (wallRightHits[i].collider != null && wallRightHits[i].collider.CompareTag("ClimbingWall"))
                {
                    _onWall = true;
                    _onWallRight = true;
                    break;
                }

                if (wallLeftHits[i].collider != null && wallLeftHits[i].collider.CompareTag("ClimbingWall"))
                {
                    _onWall = true;
                    _onWallLeft = true;
                    break;
                }

            }
            else
            {
                _onWall = false;
                _onWallRight = false;
                _onWallLeft = false;
            }
        }

        if (_onWall)
        {
            float clampedY = Mathf.Clamp(_rigidbody.velocity.y, -1, 6);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, clampedY);
        }
    }

    private void Move()
    {
        if (!_gameStarted) return;

        _input = new Vector2(Input.GetAxis("Horizontal"), 0);
        _isMoving = _input.x != 0 ? true : false;

        if (!_canMove) return;

        float moveBy = _input.x * _speed;
        //_rigidbody.AddForce(Vector2.right * moveBy);
        //_rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, 5);

        if (_isGrounded || !_walljump && _currentTimeAtFly > _timeAtFly)
        {
            if (moveBy != 0)
            {
                _rigidbody.velocity = new Vector2(Mathf.Lerp(_rigidbody.velocity.x, moveBy, 10 * Time.deltaTime), _rigidbody.velocity.y);
            }
        }

        if (_onWallRight)
        {
            _rigidbody.velocity = new Vector2(Mathf.Clamp(_rigidbody.velocity.x, -7, 0), _rigidbody.velocity.y);
        }
        if (_onWallLeft)
        {
            _rigidbody.velocity = new Vector2(Mathf.Clamp(_rigidbody.velocity.x, 0, 7), _rigidbody.velocity.y);
        }
        if (_walljump)
        {
            _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, 7);
        }


        if (_isMoving && !_onWall)
        {
            FlipSprite(_input.x > 0 ? false : true);
            //_wallCheckDirection = _input.x > 0 ? transform.right : -transform.right;
        }

        if (!_isGrounded || !_onWall)
        {
            if (_currentTimeAtFly < _timeAtFly)
            {
                _currentTimeAtFly += Time.deltaTime;
            }
        }
        if (_walljump && _currentTimeAtFly >= _timeAtFly)
        {
            _walljump = false;
            _currentTimeAtFly = 0;
        }

        if (_isGrounded || _onWall)
        {
            if (!_walljump)
            {
                _currentTimeAtFly = 0;
            }
        }
    }

    private void Jump()
    {
        if (!_canMove) return;

        if (_isGrounded)
        {
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
            _animations.Jump();
            _soundAudioSource.PlayOneShot(_jumpClip);
        }
        else
        {
            if (_onWall)
            {
                _walljump = true;
                _soundAudioSource.PlayOneShot(_jumpClip);

                if (_input.x < 0.2f && _input.x > -0.2f)
                {
                    if (_onWallRight)
                    {
                        _rigidbody.AddForce(transform.up * _jumpForce * 1.75f + Vector3.left * _jumpForce, ForceMode2D.Impulse);
                        FlipSprite(true);
                    }
                    if (_onWallLeft)
                    {
                        _rigidbody.AddForce(transform.up * _jumpForce * 1.75f + Vector3.right * _jumpForce, ForceMode2D.Impulse);
                        FlipSprite(false);
                    }
                }
                else
                {
                    if (_input.x > 0.2f)
                    {
                        if (_onWallRight)
                        {
                            _rigidbody.AddForce(transform.up * _jumpForce * 1.75f + Vector3.left * _jumpForce, ForceMode2D.Impulse);
                            FlipSprite(true);
                        }
                        if (_onWallLeft)
                        {
                            print(transform.up * _jumpForce + Vector3.right * _jumpForce * 1.5f);
                            _rigidbody.AddForce(transform.up * _jumpForce + Vector3.right * _jumpForce * 1.5f, ForceMode2D.Impulse);
                        }
                    }
                    if (_input.x < -0.2f)
                    {
                        if (_onWallRight)
                        {
                            _rigidbody.AddForce(transform.up * _jumpForce + Vector3.left * _jumpForce * 1.5f, ForceMode2D.Impulse);
                        }
                        if (_onWallLeft)
                        {
                            _rigidbody.AddForce(transform.up * _jumpForce * 1.75f + Vector3.right * _jumpForce, ForceMode2D.Impulse);
                            FlipSprite(false);
                        }
                    }

                }

                _animations.Jump();
            }
        }
    }

    public void Knockback(Vector2 vector)
    {
        _rigidbody.velocity = Vector2.zero;
        float knockbackForce = 3f;
        _rigidbody.AddForce(vector * knockbackForce, ForceMode2D.Impulse);
    }

    private void FlipSprite(bool value)
    {
        /*foreach (SpriteRenderer sprite in _characterSprites)
        {
            sprite.flipX = value;
        }*/

        float v = value ? 1 : -1;
        for (int i = 0; i < _characterSprites.Length; i++)
        {
            _characterSprites[i].localScale = new Vector3(0.2f * v, 0.2f, 0.2f);
        }
    }

    public void CanMove()
    {
        _canMove = true;
    }

    public void StopMove()
    {
        _canMove = false;
    }

    public void StartGame()
    {
        _gameStarted = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayStartPosition = transform.position + _groundCheckOffset;
        //Gizmos.DrawRay(rayStartPosition, Vector3.down * _groundCheckDistance);
        float step = _characterWidth / 3;
        for (int i = 0; i < 3; i++)
        {
            Vector2 rPos = new Vector2(rayStartPosition.x + (step * i), rayStartPosition.y);
            Gizmos.DrawRay(rPos, Vector3.down * _groundCheckDistance);
        }

        Gizmos.color = Color.green;
        Vector3 rayRightWallStartPosition = transform.position + _wallRightCheckOffset;
        Vector3 rayLeftWallStartPosition = transform.position + _wallLeftCheckOffset;
        float hStep = _characterHeight / 2;
        for (int i = 0; i < 2; i++)
        {
            Vector2 rPosR = new Vector2(rayRightWallStartPosition.x, rayRightWallStartPosition.y + (hStep * i));
            Gizmos.DrawRay(rPosR, Vector3.right * _wallCheckDistance);

            Vector2 rPosL = new Vector2(rayLeftWallStartPosition.x, rayLeftWallStartPosition.y + (hStep * i));
            Gizmos.DrawRay(rPosL, Vector3.left * _wallCheckDistance);
        }

        Gizmos.color = Color.blue;
        Vector3 startPos = transform.position;
        if (!_rigidbody) return;
        Gizmos.DrawRay(rayLeftWallStartPosition, _rigidbody.velocity);
    }
}