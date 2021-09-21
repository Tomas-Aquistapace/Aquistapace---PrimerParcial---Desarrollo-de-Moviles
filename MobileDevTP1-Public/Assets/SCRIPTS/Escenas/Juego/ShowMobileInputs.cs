using System.Collections.Generic;
using UnityEngine;

public class ShowMobileInputs : MonoBehaviour
{
    [SerializeField] List<GameObject> playersInput = new List<GameObject>();

    private void Awake()
    {
        foreach (var input in playersInput)
        {
            input.SetActive(false);
        }

#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
        foreach (var input in playersInput)
        {
            input.SetActive(true);
        }
#endif
    }


}
