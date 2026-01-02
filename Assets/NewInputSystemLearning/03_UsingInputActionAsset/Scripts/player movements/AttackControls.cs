using System;
using UnityEngine;

namespace TGL.Learning.NewInputSystem.UsingInputActionAsset
{
    public class AttackControls : MonoBehaviour
    {
        [SerializeField] private PlayerNormalActionMap playerNormalActionMap;
        [SerializeField] private Transform artTransform;
        private SpriteRenderer artSpriteRenderer;
        private Animator artAnimator;
        
        #region MonoBehaviour_Methods
        
        private void Awake()
        {
            if (ValidateSerializedFields())
            {
                AddListenersToActions();
            }
            else
            {
                Debug.LogError($"The player controls script on {gameObject.name} has missing serialized field references. Please fix before playing the game.", this);
            }
        }

        private void OnDestroy()
        {
            RemoveListenersFromActions();
        }

        #endregion MonoBehaviour_Methods
        
        #region Listeners

        private void AddListenersToActions()
        {
            playerNormalActionMap.PlayerNormalActionMapPerformAttack += Attack;
        }
    
        private void RemoveListenersFromActions()
        {
            playerNormalActionMap.PlayerNormalActionMapPerformAttack -= Attack;
        }

        #endregion Listeners
        
        #region ActionMethods
        
        private void Attack()
        {
            Debug.Log("Attack performed", this);
            artAnimator.SetTrigger(GameConstants.PLAYER_ANIM_PARAM_ATTACK);
        }
        
        #endregion ActionMethods
        
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

            return !hasError;
        }
        
        #endregion Validators
    }
}