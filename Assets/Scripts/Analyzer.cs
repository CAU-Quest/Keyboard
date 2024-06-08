using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Analyzer : MonoBehaviour
{
    [Header("Prefab Settings")] 
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI errorRateText;

    private int correctCharacterCount = 0;
    private int inCorrectCharacterCount = 0;
    private int inCorrectFixedCharacterCount = 0;

    private float currentErrorRate = 0f;

    public List<float> errorRates = new List<float>();

    private string correctAnswer;
    private int lastCorrectIndex = 0;

    private float startTime = 0f;
    private bool timerStarted = false;

    void Start()
    {
        correctAnswer = answerText.text;
        inputField.onValueChanged.AddListener(EvaluateErrorRate);
    }

    public void EvaluateErrorRate(string input)
    {
        if (!timerStarted)
        {
            startTime = Time.time;
            timerStarted = true;
        }

        if (input.Length > correctAnswer.Length)
        {
            inputField.text = input.Substring(0, correctAnswer.Length);
            return;
        }

        for (int i = lastCorrectIndex; i < input.Length; i++)
        {
            if (input[i] == correctAnswer[i])
            {
                correctCharacterCount++;
                lastCorrectIndex++;
            }
            else
            {
                inCorrectCharacterCount++;
                if (i < correctAnswer.Length && input[i] != correctAnswer[i])
                {
                    inCorrectFixedCharacterCount++;
                }
                break;
            }
        }

        currentErrorRate = (float)(inCorrectCharacterCount + inCorrectFixedCharacterCount) / (inCorrectCharacterCount + inCorrectFixedCharacterCount + correctCharacterCount);

        errorRates.Add(currentErrorRate);

        float elapsedTime = Time.time - startTime;
        float wpm = (correctCharacterCount / 5f) / (elapsedTime / 60f); 

        float errorRatePercentage = currentErrorRate * 100f;
        errorRateText.text = $"Word Per Minute: {wpm:F2}, Error Rate: {errorRatePercentage:F2}%";
    }
}