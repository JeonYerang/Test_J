using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerAnimatorController : MonoBehaviour
{
    public SpriteResolver[] resolvers;
    public float animationSpeed = 1.0f;
    void Awake()
    {
        resolvers = GetComponentsInChildren<SpriteResolver>();
    }

    private void Start()
    {
        
            string category = resolvers[0].GetCategory();

        StartCoroutine(AnimationEnumerator(category, animationSpeed));

    }
    // Update is called once per frame
    void Update()
    {
    }
    SpriteLibrary a;
    IEnumerator AnimationEnumerator(string category, float animationSpeed)
    {
        a = new SpriteLibrary();    

        while (true)
        {
            for (int i = 0; i < resolvers.Length; i++)
            {
                resolvers[i].SetCategoryAndLabel(category, "0");
                print("Change Resolver Label to 0");
            }
            yield return new WaitForSeconds(animationSpeed);
            for (int i = 0; i < resolvers.Length; i++)
            {
                resolvers[i].SetCategoryAndLabel(category, "1");

                print("Change Resolver Label to 1");
            }
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
