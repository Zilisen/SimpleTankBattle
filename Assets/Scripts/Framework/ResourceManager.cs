using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 资源管理器，它只是对Resources.Load的一层封装。
// 之所以不直接使用Resources.Load，主要是考虑到后续如果实现热更新等功能，会有不同的处理，
// 届时，只需要修改LoadPrefab的具体实现，不需要更改逻辑代码。

public class ResourceManager : MonoBehaviour
{
    // 加载预设
    public static GameObject LoadPrefab(string path)
    {
        return Resources.Load<GameObject>(path);
    }
}
