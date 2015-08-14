namespace HeadlessPlayer
{
    using System;

    public interface ISoundOutput
    {
        float Volume { get; set; }

        void Play();

        void Pause();

        void Stop();

        void AddSamples(IntPtr samples, int count);

        int GetSamplesInBufferCount();
    }
}