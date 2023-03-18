using HB.Packages.Logger;

namespace HB.Packages.StateMachine
{
    public abstract class State
    {
        #region Public Fields

        public string Name;
        public Transition OutTransition;

        #endregion

        #region Protected Fields

        protected Fsm Fsm;

        #endregion

        #region Public Properties

        public bool IsFinished { get; private set; }

        #endregion

        #region  Constructors

        protected State()
        {
            IsFinished = false;
        }

        protected State(Fsm fsm) : this()
        {
            SetFsm(fsm);
        }

        #endregion

        #region Unity

        public void Update(float deltaTime)
        {
            if (IsFinished) return;
            OnUpdate(deltaTime);
        }

        #endregion

        #region Unity

        protected virtual void OnUpdate(float deltaTime)
        {
        }

        #endregion

        #region Public Methods

        public virtual void SetFsm(Fsm fsm)
        {
            Fsm = fsm;
        }

        public void Enter()
        {
            OnEnter();
        }

        public void Exit()
        {
            OnExit();
        }

        public override string ToString()
        {
            return $"[{Name}]";
        }

        #endregion

        #region Protected Methods

        protected virtual void Finished()
        {
            if (IsFinished)
            {
                Log.Error("FSM", $"State has already Finished: {Name}");
                return;
            }

            IsFinished = true;
            Fsm.ApplyTransition(this);
        }

        protected virtual void OnEnter()
        {
            IsFinished = false;
        }

        protected virtual void OnExit()
        {
        }

        #endregion
    }

    public abstract class State<TAgent> : State
    {
        #region Protected Fields

        protected TAgent Agent;

        #endregion

        #region Public Methods

        public override void SetFsm(Fsm fsm)
        {
            base.SetFsm(fsm);
            SetAgent(fsm);
        }

        #endregion

        #region Private Methods

        private void SetAgent(Fsm fsm)
        {
            Fsm<TAgent> agentFsm = (Fsm<TAgent>) fsm;
            Agent = agentFsm.Agent;
        }

        #endregion
    }
}