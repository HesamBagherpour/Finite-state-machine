using System;

namespace HB.Packages.StateMachine
{
    public abstract class Condition
    {
        #region Public Methods
        public abstract bool CheckCondition();
        #endregion
    }

    public class DelegatedCondition : Condition
    {
        private readonly Func<bool> _func;

        public DelegatedCondition(Func<bool> func)
        {
            _func = func;
        }
        public override bool CheckCondition()
        {
            return _func.Invoke();
        }
    }
}