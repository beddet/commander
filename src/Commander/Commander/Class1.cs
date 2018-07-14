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

    public class FullCharCommander : Command<char, char, char?>
    {
        protected List<FullCharHistory> History = new List<FullCharHistory>();
        private int _undoCount;

        public override char Execute(char arg)
        {
            History.Add(new FullCharHistory(arg, CommandMethod.Execute));
            _undoCount = 0;
            return arg;
        }

        public override char? Undo()
        {
            if (!History.Any()) return null;
            if (_undoCount == 0) _undoCount = 1;
            var charHistory = History[History.Count - _undoCount];
            var item = charHistory.Item;
            History.Add(new FullCharHistory(item, CommandMethod.Undo));
            _undoCount += 2;
            return item;
        }

        public override char? Redo()
        {
            if (!History.Any()) return null;
            var charHistory = History.Last();
            var item = charHistory.Item;
            History.Add(new FullCharHistory(item, CommandMethod.Redo));
            _undoCount = 0;
            return item;
        }
    }

    public enum CommandMethod
    {
        Execute,
        Undo,
        Redo
    }

    public class FullCharHistory : FullHistory<char>
    {
        public FullCharHistory(char item, CommandMethod method) : base(item, method) { }
    }

    public class FullHistory<T>
    {
        public readonly T Item;
        public CommandMethod Method;

        public FullHistory(T item, CommandMethod method)
        {
            Item = item;
            Method = method;
        }
    }
}
