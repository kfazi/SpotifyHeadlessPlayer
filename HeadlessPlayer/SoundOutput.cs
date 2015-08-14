namespace HeadlessPlayer
{
    using System;
    using System.Runtime.InteropServices;

    using NAudio.Wave;

    public class PositionTracker
    {
        private int _samplesInBuffer;

        public int Position { get; private set; }

        public void AddSamplesCount(int count)
        {
            _samplesInBuffer += count;
        }

        public void UpdateSamplesInBufferCount(int count)
        {
            Position += _samplesInBuffer - count;
            _samplesInBuffer = count;
        }

        public void Reset()
        {
            _samplesInBuffer = 0;
            Position = 0;
        }
    }

    public class SoundOutput : ISoundOutput, IDisposable
    {
        private readonly IWavePlayer _waveOutDevice;

        private byte[] _copiedSamples;

        private BufferedWaveProvider _bufferedWaveProvider;

        private VolumeWaveProvider16 _volumeWaveProvider;

        private WaveFormat _waveFormat;

        public SoundOutput()
        {
            _waveOutDevice = new WaveOut(WaveCallbackInfo.FunctionCallback());
            _bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat())
            {
                BufferDuration = TimeSpan.FromSeconds(120),
                DiscardOnBufferOverflow = true
            };

            PositionTracker = new PositionTracker();

            SetAudioParameters(44100, 2);
        }

        ~SoundOutput()
        {
            Dispose(false);
        }

        public PositionTracker PositionTracker { get; private set; }

        public float Volume
        {
            get
            {
                return _volumeWaveProvider.Volume;
            }

            set
            {
                _volumeWaveProvider.Volume = value;
            }
        }

        public int SampleRate
        {
            get
            {
                return _waveFormat.SampleRate;
            }
        }

        public int Channels
        {
            get
            {
                return _waveFormat.Channels;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetAudioParameters(int sampleRate, int channels)
        {
            _waveFormat = new WaveFormat(sampleRate, channels);
            _bufferedWaveProvider = new BufferedWaveProvider(_waveFormat)
            {
                BufferDuration = TimeSpan.FromSeconds(120),
                DiscardOnBufferOverflow = false
            };
            _volumeWaveProvider = new VolumeWaveProvider16(_bufferedWaveProvider);
            _waveOutDevice.Init(_volumeWaveProvider);
        }

        public void Play()
        {
            _waveOutDevice.Play();
        }

        public void Pause()
        {
            _waveOutDevice.Pause();
        }

        public void Stop()
        {
            _waveOutDevice.Stop();
        }

        public void AddSamples(IntPtr samples, int count)
        {
            var size = count * Channels * 2;

            if (_copiedSamples == null || _copiedSamples.Length < size)
            {
                _copiedSamples = new byte[size];
            }

            Marshal.Copy(samples, _copiedSamples, 0, size);

            _bufferedWaveProvider.AddSamples(_copiedSamples, 0, size);

            PositionTracker.AddSamplesCount(count);
        }

        public int GetSamplesInBufferCount()
        {
            PositionTracker.AddSamplesCount(_bufferedWaveProvider.BufferedBytes / Channels / 2);

            return _bufferedWaveProvider.BufferedBytes / 2;
        }

        private void Dispose(bool isDisposing)
        {
            if (!isDisposing)
            {
                return;
            }

            _waveOutDevice.Dispose();
        }
    }
}