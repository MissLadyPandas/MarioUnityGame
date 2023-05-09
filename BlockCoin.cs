using System.Collections;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayCoin();
        GameManager.Instance.AddCoin();

        StartCoroutine(Animate());
    }

        private IEnumerator Animate()
    {        
        Vector3 restingPostion = transform.localPosition;
        Vector3 animatedPosition = restingPostion + Vector3.up * 2f;

        yield return Move(restingPostion, animatedPosition);
        yield return Move(animatedPosition, restingPostion);

        Destroy(gameObject);
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.25f;

        while (elapsed < duration)
        {
            float  t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }

}
