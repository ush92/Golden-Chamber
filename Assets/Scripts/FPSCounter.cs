using UnityEngine;
using System.Collections;

public class Fps : MonoBehaviour
{
    private float count;

    //private IEnumerator Start()
    //{
    //    GUI.depth = 2;
    //    while (true)
    //    {
    //        count = 1f / Time.unscaledDeltaTime;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    //private void OnGUI()
    //{
    //    GUIStyle myStyle = new()
    //    {
    //        fontSize = 72,       
    //    };

    //    GUI.Label(new Rect(10, 80, 200, 50), "FPS: " + Mathf.Round(count), myStyle);
    //}
}