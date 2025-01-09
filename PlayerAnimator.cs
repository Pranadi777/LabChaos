using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Need to reference player to get if walking
    // if player is walking - set bool to true for animation parameter

    private Animator animator;
    private const string IS_WALKING = "IsWalking";

    [SerializeField] Player player;

    private void Awake() {
        // Need to reference the animator
        animator = this.GetComponent<Animator>();
    }

    private void Update() {
        // have the IsWalking function return a bool if the player is walking
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
