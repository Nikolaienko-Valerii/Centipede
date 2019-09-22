using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControllerScript : MonoBehaviour
{
    public Button[] Buttons;
    public Color BaseColor;
    public Color SelectedColor;
    int selectedButton = 0;
    public AudioClip SelectSound;
    AudioSource audioSource;

    private void Start()
    {
        Buttons[selectedButton].GetComponentInChildren<Text>().color = SelectedColor;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            audioSource.PlayOneShot(SelectSound, 4f);
            int old = selectedButton;
            selectedButton++;
            if (selectedButton == Buttons.Length)
            {
                selectedButton = 0;
            }
            ReAplyColor(old, selectedButton);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            audioSource.PlayOneShot(SelectSound, 4f);
            int old = selectedButton;
            selectedButton--;
            if (selectedButton < 0)
            {
                selectedButton = Buttons.Length - 1;
            }
            ReAplyColor(old, selectedButton);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(SelectSound, 4f);
            new WaitForSeconds(0.5f);
            StopAllCoroutines();
            StartCoroutine(MakeSelectionAfterDelay());
        }
    }

    IEnumerator MakeSelectionAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        Buttons[selectedButton].onClick.Invoke();
    }

    void ReAplyColor(int oldIndx, int newIndx)
    {
        Buttons[oldIndx].GetComponentInChildren<Text>().color = BaseColor;
        Buttons[newIndx].GetComponentInChildren<Text>().color = SelectedColor;
    }

}
