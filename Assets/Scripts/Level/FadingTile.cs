using UnityEngine;

public class FadingTile : MonoBehaviour
{
    public Sprite sprite;
    public bool isEnabled = false;
    private float currentAlpha;

    public float appearedTime;
    private float currentAppearedTime;
    public float speedOfDisappearing;

    public float disappearedTime;
    private float currentDisappearedTime;
    public float speedOfAppearing;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        currentAlpha = isEnabled ? 1.0f : 0f;
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, currentAlpha);
        GetComponent<BoxCollider2D>().enabled = isEnabled;

        currentAppearedTime = appearedTime;
        currentDisappearedTime = disappearedTime;
    }

    void Update()
    {
        if(isEnabled)
        {
            currentAppearedTime -= Time.deltaTime;
            if(currentAppearedTime <=0f)
            {
                currentAlpha -= Time.deltaTime * speedOfDisappearing;
                GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, currentAlpha);

                if(currentAlpha <= 0.5f && currentAlpha > 0f)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                }
                else if (currentAlpha <= 0f)
                {
                    currentAlpha = 0f;
                    isEnabled = false;
                    currentAppearedTime = appearedTime;
                }
            }
        }
        else
        {
            currentDisappearedTime -= Time.deltaTime;
            if (currentDisappearedTime <= 0f)
            {
                currentAlpha += Time.deltaTime * speedOfAppearing;
                GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, currentAlpha);

                if (currentAlpha >= 0.5f && currentAlpha < 1f)
                {
                    GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (currentAlpha >= 1f)
                {
                    currentAlpha = 1f;
                    isEnabled = true;
                    currentDisappearedTime = disappearedTime;
                }
            }
        }
    }
}
