using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLoadIn : MonoBehaviour
{
    private bool animating;
    private Queue<Transform> queue;
    public bool loaded;
    public static float LoadTime;
    private void Start()
    {
        animating = false;
        queue = new Queue<Transform>();
        loaded = false;
    }
    public void Deselected()
    {
        loaded = false;
    }
    public void Selected(Transform transfor)
    {
        loaded = true;
        if (transfor.childCount <= 0)
        {
            return;
        }
        transfor.localScale = Vector3.one;
        for (int i = transfor.childCount - 1; i >= 0; --i)
        {
            transfor.GetChild(i).localScale = Vector3.zero;
        }
        /*for (int i = transfor.childCount - 1; i >= 0; --i)
        {
            if(transfor.GetChild(i).childCount > 0 &&
               transfor.GetChild(i).GetComponent<Image>() == null*//* &&
                (transfor.GetChild(i).name.Contains("col") ||
                 transfor.GetChild(i).name.Contains("Button"))*//*)
            {
                Selected(transfor.GetChild(i));
            }

            if (animating)
            {
                queue.Enqueue(transfor.GetChild(i));
            }
            else
            {
                StartCoroutine(ScaleUp(transfor.GetChild(i)));
            }
        }*/
        for (int i = 0; i < transfor.childCount; ++i)
        {
            if (transfor.GetChild(i).childCount > 0 &&
               transfor.GetChild(i).GetComponent<Image>() == null/* &&
                (transfor.GetChild(i).name.Contains("col") ||
                 transfor.GetChild(i).name.Contains("Button"))*/)
            {
                Selected(transfor.GetChild(i));
            }

            if (animating)
            {
                queue.Enqueue(transfor.GetChild(i));
            }
            else
            {
                StartCoroutine(ScaleUp(transfor.GetChild(i)));
            }
        }
    }
    private IEnumerator ScaleUp(Transform t)
    {
        animating = true;
        for (int i = 0; i < 10; ++i)
        {
            if (t.localScale.x < 1)
            {
                t.localScale = new Vector3(t.localScale.x + 0.1f, t.localScale.y + 0.1f, t.localScale.z + 0.1f);
            }
            else
            {
                i = 11;
            }
            yield return new WaitForSeconds(LoadTime);
        }
        t.localScale = Vector3.one;
        StopCoroutine(ScaleUp(t));
        if (queue.Count > 0)
        {
            StartCoroutine(ScaleUp(queue.Dequeue()));
        }
        else
        {
            animating = false;
        }
    }
}
