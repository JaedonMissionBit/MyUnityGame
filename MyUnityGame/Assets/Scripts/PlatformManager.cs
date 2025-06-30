using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> platforms;

    [ContextMenu("ResetPlatforms")]
    public void RespawnPlatforms()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            platforms[i].GetComponent<FallingPlatform>().ResetPlatform();
        }
    }
    [ContextMenu("Test all platforms")]
    public void TestFallAll()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            platforms[i].GetComponent<FallingPlatform>().TestFall();
        }
    }
}
