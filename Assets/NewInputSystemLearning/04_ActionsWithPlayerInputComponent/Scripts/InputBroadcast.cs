using UnityEngine;

public class InputBroadcast : MonoBehaviour
{
    public void OnAttack()
    {
        Debug.Log("Attack triggered! (Broadcast)", gameObject);
    }
}
