using System;
using UnityEngine;

namespace TGL.Learning.NewInputSystem.UsingInputActionAsset
{
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] private PlayerNormalActionMap playerNormalActionMap;
        [SerializeField] private Transform artTransform;
        [SerializeField] private Rigidbody2D _rb;
        
        [Header("Movement"), Range(1, 10), SerializeField] private float speed;
        [SerializeField] private FlipType flipType = FlipType.UsingFlipProperty;
        
        [Header("jump"), Range(1, 10), SerializeField] private float jumpForce; 
        
        [Header("ground Check"), SerializeField] private Transform leftGroundCheckTransform;
#if UNITY_EDITOR
        [SerializeField] private Color leftGroundCheckColor = Color.red;
        [SerializeField] private Color rightGroundCheckColor = Color.blue;
#endif
        [SerializeField] private Transform rightGroundCheckTransform;
        [SerializeField, Range(0.01f, 2f)] private float rayLength;
        [SerializeField] private LayerMask groundLayerMask;
        

        private SpriteRenderer artSpriteRenderer;
        private Animator artAnimator;
        private bool facingRight;
        private bool grounded;
        private float lastFramePositionX;
        private float lastFramePositionY;

        #region MonoBehaviour_Methods
        
        private void Awake()
        {
            if (ValidateSerializedFields())
            {
                IdentifyFacingDirection();
                AddListenersToActions();
            }
            else
            {
                Debug.LogError($"The player controls script on {gameObject.name} has missing serialized field references. Please fix before playing the game.", this);
            }
        }

        private void FixedUpdate()
        {
            GroundCheck();
            SetAnimatorParam();
        }

        private void OnDestroy()
        {
            RemoveListenersFromActions();
        }

        #endregion MonoBehaviour_Methods

        #region Listeners

        private void AddListenersToActions()
        {
            playerNormalActionMap.PlayerNormalActionMapMoveHorizontal += Move;
            playerNormalActionMap.PlayerNormalActionMapPerformJump += Jump;
        }
    
        private void RemoveListenersFromActions()
        {
            playerNormalActionMap.PlayerNormalActionMapMoveHorizontal -= Move;
            playerNormalActionMap.PlayerNormalActionMapPerformJump -= Jump;
        }

        #endregion Listeners

        #region Movement_Methods
        
        private void Move(float axisVal)
        {
            if (grounded)
            {
                _rb.linearVelocity = new Vector2(axisVal * speed, _rb.linearVelocity.y);
                // _rb.AddForce(new Vector2((axisVal * speed) - _rb.linearVelocity.x, 0), ForceMode2D.Impulse);
            }
            FlipX();
        }

        private void FlipX()
        {
            if (facingRight && _rb.linearVelocity.x < 0)
            {
                switch (flipType)
                {
                    case FlipType.UsingFlipProperty:
                        artSpriteRenderer.flipX = true;
                        break;
                    case FlipType.UsingRotation180:
                        artTransform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    case FlipType.UsingScaleNegative:
                        Vector3 localScale = artTransform.localScale;
                        localScale.x = -Mathf.Abs(localScale.x);
                        artTransform.localScale = localScale;
                        break;
                    case FlipType.NONE:
                    default:
                        Debug.LogWarning($"Flip type is set to NONE on {gameObject.name}. Defaulting flipping x direction to inverse of cureent", this);
                        artSpriteRenderer.flipX = !artSpriteRenderer.flipX;
                        break;
                }
                facingRight = false;
            }
            else if (!facingRight && _rb.linearVelocity.x > 0)
            {
                switch (flipType)
                {
                    case FlipType.UsingFlipProperty:
                        artSpriteRenderer.flipX = false;
                        break;
                    case FlipType.UsingRotation180:
                        artTransform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case FlipType.UsingScaleNegative:
                        Vector3 localScale = artTransform.localScale;
                        localScale.x = Mathf.Abs(localScale.x);
                        artTransform.localScale = localScale;
                        break;
                    case FlipType.NONE:
                    default:
                        Debug.LogWarning($"Flip type is set to NONE on {gameObject.name}. Defaulting flipping x direction to inverse of cureent", this);
                        artSpriteRenderer.flipX = !artSpriteRenderer.flipX;
                        break;
                }
                facingRight = true;
            }
        }
        
        private void Jump()
        {
            if (grounded)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
                // _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        #endregion Movement_Methods

        #region AnimationMethods

        private void SetAnimatorParam()
        {
            artAnimator.SetBool(GameConstants.PLAYER_ANIM_PARAM_IS_GROUNDED, grounded);
            artAnimator.SetFloat(GameConstants.PLAYER_ANIM_PARAM_SPEED_X, Mathf.Abs(lastFramePositionX - artTransform.position.x)/Time.fixedDeltaTime);
            artAnimator.SetFloat(GameConstants.PLAYER_ANIM_PARAM_SPEED_y, (artTransform.position.y - lastFramePositionY)/Time.fixedDeltaTime);
            lastFramePositionX = artTransform.position.x;
            lastFramePositionY = artTransform.position.y;
        }

        #endregion AnimationMethods
        
        #region Validators
        
        private bool ValidateSerializedFields()
        {
            bool hasError = false;
            if (playerNormalActionMap == null)
            {
                Debug.LogError($"player normal action map is not assigned in the inspector on {gameObject.name}", this);
                hasError = true;
            }

            if (artTransform == null)
            {
                Debug.LogError($"art transform is not assigned in the inspector on {gameObject.name}", this);
                hasError = true;
            }
            else
            {
                artSpriteRenderer = artTransform.GetComponent<SpriteRenderer>();
                if (artSpriteRenderer == null)
                {
                    Debug.LogError($"sprite renderer component not found on art transform assigned in the inspector on {gameObject.name}", this);
                    hasError = true;
                }
                else
                {
                    artAnimator = artTransform.GetComponent<Animator>();
                    if (artAnimator == null)
                    {
                        Debug.LogError($"animator component not found on art transform assigned in the inspector on {gameObject.name}", this);
                        hasError = true;
                    }
                }
            }

            if (_rb == null)
            {
                Debug.LogError($"rigidbody is not assigned in the inspector on {gameObject.name}", this);
                hasError = true;
            }

            if (Mathf.Approximately(speed, 0))
            {
                Debug.LogError($"speed variable cannot be set to {speed} as it is too close to zero", this);
                hasError = true;
            }

            return !hasError;
        }
        
        private void IdentifyFacingDirection()
        {
            switch (flipType)
            {
                case FlipType.UsingFlipProperty:
                    facingRight = !artSpriteRenderer.flipX;
                    break;
                case FlipType.UsingRotation180:
                    // facingRight = Quaternion.Angle(artTransform.rotation, Quaternion.Euler(0, 0, 0)) < 1;
                    facingRight = artTransform.right.x > 0; // assuming world axis and local axis are aligned
                    break;
                case FlipType.UsingScaleNegative:
                    facingRight = artTransform.localScale.x > 0;
                    break;
                case FlipType.NONE:
                default:
                    Debug.LogWarning($"Flip type is set to NONE on {gameObject.name}. Defaulting facing direction to true (right).", this);
                    facingRight = true;
                    break;
            }
        }

        private void GroundCheck()
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(leftGroundCheckTransform.position, Vector2.down, rayLength, groundLayerMask);
            RaycastHit2D hitRight = Physics2D.Raycast(rightGroundCheckTransform.position, Vector2.down, rayLength, groundLayerMask);
            grounded = hitLeft || hitRight;
        }
        
        #endregion Validators

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (leftGroundCheckTransform != null)
            {
                Gizmos.color = leftGroundCheckColor;
                Gizmos.DrawLine(leftGroundCheckTransform.position, leftGroundCheckTransform.position + Vector3.down * rayLength);
            }
            if (rightGroundCheckTransform != null)
            {
                Gizmos.color = rightGroundCheckColor;
                Gizmos.DrawLine(rightGroundCheckTransform.position, rightGroundCheckTransform.position + Vector3.down * rayLength);
            }
        }
#endif
    }
}