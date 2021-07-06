using System;

namespace EADragDropMVVMTest.Commands.Common
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

    }
}
