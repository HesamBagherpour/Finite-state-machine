using System.Collections.Generic;
using System.Threading.Tasks;
using HB.Packages.Controllers;
using HB.Packages.StateMachine;
using UnityEngine;

public class PreloadController : Controller
{
    public abstract class PreloadState : State<PreloadController>
    {
    }

    public class InitializeApp : PreloadState
    {
            
        private bool _finished;
        protected override async void OnEnter()
        {
            Debug.Log("on initialize state enter");
            base.OnEnter();

        }

  
        private void OnSplashScreenVideoFinished()
        {
            //  Debug.Log("Video finne");

            Temp();
        }

        private async void Temp()
        {
            await Task.Delay(1000);
            Agent.splashScreenVideoPassed = true;
        }

 



        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (_finished)
                Finished();
        }

        protected override void OnExit()
        {
            base.OnExit();
               
        }
    }

    public class NextSceneState : PreloadState
    {
        private bool _finished;

        protected override void OnEnter()
        {
            Debug.Log("on next scene enter ");


            base.OnEnter();
            LoadNextScene();
            _finished = true;
        }

        private void LoadNextScene()
        {
           
            _loadNextSceneOperation.allowSceneActivation = true;
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (_finished)
                Finished();
        }

        protected override void OnExit()
        {
              

            base.OnExit();
        }
    }

    public class ConnectState : PreloadState
    {

        private int _attempt;
        private float _elapsed;

        protected override void OnEnter()
        {
                

            base.OnEnter();

        }

        private void OnSuccess()
        {
      
            Finished();
        }



        private void OnError()
        {
       
            //Debug.Log(error.Message);
      
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            _elapsed = 0;

        }
    }

    public class OfflineModeState : PreloadState
    {
          

        protected override void OnEnter()
        {
            
            base.OnEnter();
     
            Finished();
        }

        protected override void OnExit()
        {
            base.OnExit();
           
        }
    }

    public class GoToMatch3Debug : PreloadState
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            LoadNextScene();
        }

        private async void LoadNextScene()
        {
            // SceneManager.LoadScene(Strings.Match3Debug); 
            // await Agent._assetLoader.LoadScene(Strings.Match3Debug);
        }
    }

    #region Private Fields

    private Fsm<PreloadController> _fsm;

    private float _progressTimer;

    private bool splashScreenVideoPassed = false;
    private static AsyncOperation _loadNextSceneOperation;
    #endregion

    #region Unity

    protected override void Awake()
    {


        PlayerPrefs.SetFloat("StartTime", Time.time);
        PlayerPrefs.Save();
        _progressTimer = 3;
            
        splashScreenVideoPassed = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        InitializeApp initialState = new InitializeApp { Name = "Init App state" };
        NextSceneState nextSceneState = new NextSceneState { Name = "Next Scene state" };
        OfflineModeState offlineState = new OfflineModeState { Name = "Offline State" };
        GoToMatch3Debug gotoMatch3 = new GoToMatch3Debug() { Name = "Goto Match3 State" };
        Transition.CreateAndAssign(initialState, offlineState);
        Transition.CreateAndAssign(offlineState, nextSceneState);

        _fsm = new Fsm<PreloadController>(this, initialState) { Name = "Preload FSM" };

        RunFsm();
          
    }


    private void RunFsm()
    {
         
        _fsm.Start();
    }

    private void ShowSettingMenu()
    {
  
        List<string> options = new List<string>();



        // selectButton.onClick.AddListener(() =>
        // {
        //     dropdown.gameObject.SetActive(false);
        //     selectButton.gameObject.SetActive(false);
        //     if (gameMode == GameMode.None) gameMode = (GameMode)1;
        //     _fsm.Start();
        // });
    }


    private void Update()
    {
        //_fsm?.UnityEngine.PlayerLoop.Update(Time.deltaTime);
        _progressTimer += Time.deltaTime;
        if (_loadNextSceneOperation != null)
        {
            _progressTimer = Mathf.Clamp(_progressTimer, _loadNextSceneOperation.allowSceneActivation ? 8 : 0, _loadNextSceneOperation.allowSceneActivation ? 10 : 9);
        }
        var progress = _progressTimer / 10f;
           
    }

    #endregion
}