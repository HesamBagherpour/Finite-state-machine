namespace HB.Packages.StateMachine
{
    public class Transition
    {
        #region Public Properties

        public State FromState { get; protected set; }
        public State ToState { get; protected set; }

        #endregion

        #region Protected Properties

        protected Condition Condition { get; set; }

        #endregion

        #region Public Methods

        public virtual bool CheckCondition()
        {
            if (Condition == null) return true;
            return Condition.CheckCondition();
        }

        public static Transition CreateAndAssign(State from, State to)
        {
            Transition transition = new Transition
            {
                FromState = from,
                ToState = to
            };

            from.OutTransition = transition;
            return transition;
        }


        public static Transition CreateAndAssign(State from, State to, Condition condition)
        {
            Transition transition = CreateAndAssign(from, to);
            transition.Condition = condition;
            return transition;
        }


        public static SwitchTransition CreateAndAssign(State from, State toSuccess, State toFail, Condition condition)
        {
            SwitchTransition transition = new SwitchTransition(from, toSuccess, toFail, condition);
            from.OutTransition = transition;

            return transition;
        }

        #endregion
    }

    public class SwitchTransition : Transition
    {
        #region Private Fields

        private readonly State _toSuccess;
        private readonly State _toFailed;

        #endregion

        #region  Constructors

        public SwitchTransition(State from, State toSuccess, State toFail, Condition condition)
        {
            FromState = from;
            _toSuccess = toSuccess;
            _toFailed = toFail;
            Condition = condition;
        }

        #endregion

        #region Public Methods

        public override bool CheckCondition()
        {
            bool success = base.CheckCondition();
            ToState = success ? _toSuccess : _toFailed;

            return true;
        }

        #endregion
    }
}