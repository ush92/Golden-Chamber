using UnityEngine;

public class StoneBossBehaviour : MonoBehaviour
{
    public GameObject stoneForm1;
    public GameObject stoneForm2;
    private bool isStoneForm1 = false;
    private bool isStoneForm2 = true;
    public float shiftTime;

    private enum Side { Top, Right, Bottom, Left };
    private Side currentSide = Side.Top;

    void Start()
    {
        InvokeRepeating("SwitchForms", 0f, shiftTime);
    }

    void Update()
    {
        if (isStoneForm1)
        {
            Form1Behaviour();
        }
        else
        {
            Form2Behaviour();
        }

        CheckActivity();
    }

    private void Form1Behaviour()
    {

    }

    private void Form2Behaviour()
    {

    }

    private void CheckActivity()
    {
        if (stoneForm1 == null || stoneForm2 == null)
        {
            CancelInvoke("SwitchForms");
        }

        if (stoneForm1 == null && stoneForm2 == null)
        {
            isStoneForm1 = false;
            isStoneForm2 = false;
        }
        else if (stoneForm1 == null && stoneForm2)
        {
            stoneForm2.gameObject.SetActive(true);
            isStoneForm1 = false;
            isStoneForm2 = true;
        }
        else if (stoneForm1 && stoneForm2 == null)
        {
            stoneForm1.gameObject.SetActive(true);
            isStoneForm1 = true;
            isStoneForm2 = false;
        }

    }

    private void SwitchForms()
    {
        if(isStoneForm1)
        {
            stoneForm1.gameObject.SetActive(false);
            stoneForm2.gameObject.SetActive(true);
        }
        else
        {
            stoneForm1.gameObject.SetActive(true);
            stoneForm2.gameObject.SetActive(false);
        }

        isStoneForm1 = !isStoneForm1;
        isStoneForm2 = !isStoneForm2;
    }
}
