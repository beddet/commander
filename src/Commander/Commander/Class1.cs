using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander
{
    public abstract class Command<T, TResult, TNullable>
    {
        public abstract TResult Execute(T arg);
        public abstract TNullable Undo();
        public abstract TNullable Redo();
    }

    public class CharCommander : Command<char, char, char?>
    {
        protected List<CharHistory> History = new List<CharHistory>();
        private int _lastActionIndex = -1;

        public override char Execute(char arg)
        {
            History.Add(new CharHistory(arg));
            _lastActionIndex = History.Count - 1;
            return arg;
        }

        public override char? Undo()
        {
            for (int i = History.Count - 1; i >= 0; i--)
            {
                if (History[i].Active)
                {
                    _lastActionIndex = i;
                    History[i].Active = false;
                    return History[i].Item;
                }
            }

            return null;
        }

        public override char? Redo()
        {
            for (int i = _lastActionIndex; i >= 0; i--)
            {
                _lastActionIndex = i;
                History[i].Active = true;
                return History[i].Item;
            }

            return null;
        }
    }

    

    public class CharHistory : History<char>
    {
        public CharHistory(char item, bool active = true) : base(item, active) { }
    }

    public class History<T>
    {
        public readonly T Item;
        public bool Active;

        public History(T item, bool active)
        {
            Item = item;
            Active = active;
        }
    }

}
