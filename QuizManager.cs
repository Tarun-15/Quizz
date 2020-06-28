using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private QuizUI quizUI;
    [SerializeField] private List<QuizDataScriptable> quizData;

    private List<Question> questions;
    private Question selectedQuestion;

    private int scoreCount = 0;
    private float currentTime;
   
    [SerializeField] private float timeLimit = 30f;

   public GameStatus gameStatus = GameStatus.Next;
    


   
 

    // Start is called before the first frame update
   public void  StartGame(int index)
    {
        scoreCount = 0;
        currentTime = timeLimit;

        questions = new List<Question>();
        for(int i=0; i < quizData[index].questions.Count; i++)
        {
            questions.Add(quizData[index].questions[i]);
        }
        SelectQuestion();
        gameStatus = GameStatus.Playing;

    }
    private void Update()
    {
        if(gameStatus == GameStatus.Playing)
        {
            currentTime -= Time.deltaTime;
            SetTimer(currentTime);
        }
    }
    void SelectQuestion()
    {
        int val = UnityEngine.Random.Range(0, questions.Count);
        selectedQuestion = questions[val];
        quizUI.SetQuestion(selectedQuestion);

        questions.RemoveAt(val);
    }


    private void SetTimer(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(value);
        quizUI.TimerText.text = "Time: " + time.ToString("mm':'ss");

        if (currentTime < 0)
        {
            gameStatus = GameStatus.Next;
            quizUI.GameOverPanel.SetActive(true);
        }
    
    }
   public bool Answer(string answered)
    {
        bool correctAns = false;
        if(answered == selectedQuestion.correctAns)
        {
            //yes
            correctAns = true;
            scoreCount += 1;
            quizUI.ScoreText.text = "Score: " + scoreCount;
            quizUI.totalScore.text = "Your Score: " + scoreCount;

        }
        else
        {
            //no
        }

        if (gameStatus == GameStatus.Playing)
        {
            if (questions.Count > 0)
            {
                Invoke("SelectQuestion", 0.4f);
            }

            else
            {
                gameStatus = GameStatus.Next;
                quizUI.GameOverPanel.SetActive(true);
            }
        }
        return correctAns;

    }

    [System.Serializable]
    public class Question
    {
        public string questionInfo;
        public QuestionType questionType;
        public string correctAns;
        public Sprite questionImg;
        public AudioClip questionClip;
        public UnityEngine.Video.VideoClip questionVideo;
        public List<string> options;
    }

    [System.Serializable]
    public enum QuestionType
    {
        TEXT,
        IMAGE,
        VIDEO,
        AUDIO
    }

    [System.Serializable]
    public enum GameStatus
    {
        Next,
        Playing
    }
}
