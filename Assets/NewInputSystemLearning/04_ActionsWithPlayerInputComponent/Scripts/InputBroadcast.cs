using UnityEngine;

public class InputBroadcast : MonoBehaviour
{
    

    private void OnAttack()
    {
        Debug.Log("Attack triggered! (Broadcast)", gameObject);
    }
}
