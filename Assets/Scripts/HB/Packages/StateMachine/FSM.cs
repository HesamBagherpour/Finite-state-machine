using System.Collections.Generic;
using HB.Packages.Logger;

namespace HB.Packages.StateMachine
{
    public class Fsm : State
    {
        #region Private Fields

        private readonly State _initialState;
        private readonly List<State> _states;
        private Transition _currentTransition;
        private bool _isStarted;
        private bool _autoStart;

        #endregion

        #region Public Properties

        public State CurrentState { get; private set; }

        #endregion

        #region  Constructors

        public Fsm(State initialState, bool autoStart = false)
        {
            _states = new List<State>();
            _initialState = initialState;
            AddState(_initialState);
            _autoStart = autoStart;
        }

        #endregion

        #region Unity

        public void Start()
        {
            ChangeState(_initialState);
            _isStarted = true;
        }

        #endregion

        #region Unity

        protected override void OnUpdate(float deltaTime)
        {
            if (_autoStart && _isStarted == false) Start();
            if (_isStarted == false) return;
            CurrentState?.Update(deltaTime);
            CheckCurrentTransition();
        }

        #endregion

        #region Public Methods

        public void AddState(State state)
        {
            if (state == null) return;
            if (_states.Contains(state)) return;

            state.SetFsm(this);
            _states.Add(state);
        }

        public void ApplyTransition(State state)
        {
            if (state.OutTransition == null)
            {
                ChangeState(null);
                return;
            }

            _currentTransition = state.OutTransition;
            CheckCurrentTransition();
        }

        #endregion

        #region Protected Methods

        protected override void OnEnter()
        {
            base.OnEnter();
            Start();
        }

        protected void ChangeState(State state)
        {
            Log.Debug("FSM",
                $"{(string.IsNullOrEmpty(Name) ? string.Empty : $"[{Name}]")}" +
                $"{CurrentState} -> {state}");

            if (CurrentState == state)
            {
                Log.Warn("FSM", $"Transition to Self {state}");
                return;
            }

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState?.Enter();
        }

        #endregion

        #region Private Methods

        private void CheckCurrentTransition()
        {
            if (_currentTransition != null && _currentTransition.CheckCondition())
            {
                AddState(_currentTransition.ToState);
                ChangeState(_currentTransition.ToState);
                _currentTransition = null;
            }
        }

        #endregion
    }

    public class Fsm<TAgent> : Fsm
    {
        #region Public Properties

        public TAgent Agent { get; }

        #endregion

        #region  Constructors

        public Fsm(TAgent agent, State<TAgent> initialState, bool autoStart = false) : base(initialState, autoStart)
        {
            Agent = agent;
            initialState.SetFsm(this);
        }

        #endregion
    }
}