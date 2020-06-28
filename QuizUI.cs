using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class QuizUI : MonoBehaviour

{   [SerializeField] private QuizManager quizManager;
    [SerializeField] private Text questionText, scoreText, timerText;
    [SerializeField] private GameObject gameOverPanel, mainMenuPanel, gameMenuPanel;
    [SerializeField] private Image questionImage;
    [SerializeField] private UnityEngine.Video.VideoPlayer questionVideo;
    [SerializeField] private AudioSource questionAudio;
    [SerializeField] private List<Button> options,uiButtons;
    [SerializeField] private Color correctColor, wrongColor, normalColor;
    [SerializeField] public Text totalScore;

  

  
    private bool answered;
    private QuizManager.Question question;

    public float audioLength;

    public Text ScoreText { get { return scoreText; } }

    public Text TimerText { get { return timerText; } }

    public GameObject  GameOverPanel { get { return gameOverPanel; } }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        for(int i = 0; i< options.Count; i++)
        {
            Button localBtn = options[i];
            localBtn.onClick.AddListener(() => OnClick(localBtn));
        }

        for (int i = 0; i < uiButtons.Count; i++)
        {
            Button localBtn = uiButtons[i];
            localBtn.onClick.AddListener(() => OnClick(localBtn));
        }
    }


    public void SetQuestion(QuizManager.Question question)
    {
        this.question = question;
        switch (question.questionType)
        {
            case QuizManager.QuestionType.TEXT:
                questionImage.transform.parent.gameObject.SetActive(false);
                    break;
            case QuizManager.QuestionType.IMAGE:
                ImageHolder();
                questionImage.transform.gameObject.SetActive(true);
                questionImage.sprite = question.questionImg;
                break;
            case QuizManager.QuestionType.VIDEO:
                ImageHolder();
                questionVideo.transform.gameObject.SetActive(true);
                questionVideo.clip = question.questionVideo;
                questionVideo.Play();
                break;
            case QuizManager.QuestionType.AUDIO:
                ImageHolder();
                questionAudio.transform.gameObject.SetActive(true);
                break;
        }

        questionText.text = question.questionInfo;
        List<string> answerList = ShuffleList.ShuffleListItems<string>(question.options);


        for(int i = 0; i<options.Count; i++)
        {
            options[i].GetComponentInChildren<Text>().text = answerList[i];
            options[i].name = answerList[i];
            options[i].image.color = normalColor;
        }

        answered = false;
       
    }

 


    void ImageHolder()
    {
        questionImage.transform.parent.gameObject.SetActive(true);
        questionImage.transform.gameObject.SetActive(false);
        questionAudio.transform.gameObject.SetActive(false);
        questionVideo.transform.gameObject.SetActive(false); 
    }

   public void OnClick(Button btn)
    {
        if (quizManager.gameStatus == QuizManager.GameStatus.Playing)
        { 
            if (!answered)
            {
                answered = true;
                bool val = quizManager.Answer(btn.name);
                if (val)
                {
                    btn.image.color = correctColor;

                }

                else
                {
                    btn.image.color = wrongColor;
                }
            }
        }

        switch (btn.name)
        {
            case "START QUIZ 1":
                quizManager.StartGame(0);
                mainMenuPanel.SetActive(false);
                gameMenuPanel.SetActive(true);
                break;
            case "START QUIZ 2":
                quizManager.StartGame(1);
                mainMenuPanel.SetActive(false);
                gameMenuPanel.SetActive(true);
                break;
            case "START QUIZ 3":
                quizManager.StartGame(2);
                mainMenuPanel.SetActive(false);
                gameMenuPanel.SetActive(true);
                break;
            case "Quit":
                break;
        }


    }


    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

  
}
