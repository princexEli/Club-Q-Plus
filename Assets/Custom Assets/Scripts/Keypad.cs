
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class Keypad : UdonSharpBehaviour
{
    public string CODE = "7772";
    public Text input;

    public Button[] buttons;

    public GameObject urlInput;
    
    private string inputString;
    private int FAILS_ALLOWED = 3;
    private int failsTotal;
    private bool lockInput;
    private bool isReset;
    private bool isLocked;
    private bool isUnlocked;

    string[] stars = { "****", "***", "**", "*",""};

    public void Start()
    {
        isLocked = false;
        isUnlocked = false;
        failsTotal = 0;
        updateDisplay("");
        isReset = false;
    }

    private void checkValid()
    {
        if (input.text == CODE)
        {
            showSuccess();
            isUnlocked = true;
            urlInput.SetActive(true);
        }
        else
        {
            failsTotal++;
            if (failsTotal == FAILS_ALLOWED)
            {
                isLocked = true;
            }
            showError();
        }
        if (isLocked || isUnlocked)
        {
            disableButtons();
        }
    }

    private void disableButtons()
    {
        foreach(Button b in buttons)
        {
            b.interactable = false;
        }
    }

    private void showError()
    {
        input.text = "FAIL";
        inputString = "";
        input.color = Color.red;
        isReset = true;
    }

    private void showSuccess()
    {
        input.color = Color.green;
    }

    private void buttonPressed(string val)
    {
        if (!lockInput)
        {
            if (isReset)
            {
                input.color = Color.black;
                isReset = false;
            }
            input.color = Color.black;
            updateDisplay(val);
            if (inputString.Length == 4)
            {
                checkValid();
            }
        }
    }

    public void updateDisplay(string val)
    {
        inputString = inputString + val;
        input.text = inputString + stars[inputString.Length];
    }

    #region buttons
    public void press0()
    {
        buttonPressed("0");
    }
    public void press1()
    {
        buttonPressed("1");
    }
    public void press2()
    {
        buttonPressed("2");
    }
    public void press3()
    {
        buttonPressed("3");
    }
    public void press4()
    {
        buttonPressed("4");
    }
    public void press5()
    {
        buttonPressed("5");
    }
    public void press6()
    {
        buttonPressed("6");
    }
    public void press7()
    {
        buttonPressed("7");
    }
    public void press8()
    {
        buttonPressed("8");
    }
    public void press9()
    {
        buttonPressed("9");
    }
    #endregion
}
