namespace HeadlessPlayer
{
    using System;

    public interface IPlayerThreadSynchronization
    {
        void Wait(TimeSpan timeout);

        void Set();
    }
}