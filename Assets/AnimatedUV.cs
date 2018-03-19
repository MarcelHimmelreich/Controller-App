using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedUV : MonoBehaviour {

    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);

    Vector2 uvOffset = Vector2.zero;

    void LateUpdate()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
        if (GetComponent<Renderer>().enabled)
        {
            GetComponent<Renderer>().materials[materialIndex].SetTextureOffset("_MainTex", uvOffset);
        }
    }
}
