using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stopwatch.Model;
using Windows.UI.Xaml;

namespace Stopwatch.ViewModel
{
    class StopwatchViewModel : INotifyPropertyChanged
    {
        private StopwatchModel _stopwatchModel = new StopwatchModel();
        private DispatcherTimer _timer = new DispatcherTimer();

        public bool Running // 동작되는지 아닌지
        {
            get { return _stopwatchModel.Running; }
        }

        public StopwatchViewModel() //StackOverflow에서 질문함... 해석 불가능 ㅠㅠ
        {
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Tick += TimerTick;
            _timer.Start();
            Start();

            _stopwatchModel.LapTimeUpdated += LapTimeUpdatedEventHandler;
        }

        public void Start()
        {
            _stopwatchModel.Start();
        }

        public void Stop()
        {
            _stopwatchModel.Stop();
        }

        public void Lap()
        {
            _stopwatchModel.Lap();
        }

        public void Reset()
        {
            bool running = Running;
            _stopwatchModel.Reset();
            if (running) _stopwatchModel.Start();
        }

        int _lastHours, _lastMinutes;
        decimal _lastSeconds; // 소수점 이하를 구현하기 위해

        void TimerTick(object sender, object e) //시간이 지날때마다, 시, 분, 초가 변화했는지 확인!
        {
            if (_lastHours != Hours)
            {
                _lastHours = Hours;
                OnPropertyChanged("Hours");
            }

            if (_lastMinutes != Minutes)
            {
                _lastMinutes = Minutes;
                OnPropertyChanged("Minutes");
            }

            if (_lastSeconds != Seconds)
            {
                _lastSeconds = Seconds;
                OnPropertyChanged("Seconds");
            }
        }

        public int Hours
        {
            get { return _stopwatchModel.Elapsed.HasValue ? _stopwatchModel.Elapsed.Value.Hours : 0; }
        }

        public int Minutes
        {
            get { return _stopwatchModel.Elapsed.HasValue ? _stopwatchModel.Elapsed.Value.Minutes : 0; }
        }

        public decimal Seconds
        {
            get
            {
                if (_stopwatchModel.Elapsed.HasValue)
                {
                    return (decimal)_stopwatchModel.Elapsed.Value.Seconds + (_stopwatchModel.Elapsed.Value.Milliseconds * .001M);
                }

                else return 0.0M;
            }
        }

        public int LapHours
        {
            get { return _stopwatchModel.LapTime.HasValue ? _stopwatchModel.LapTime.Value.Hours : 0; }
        }

        public int LapMinutes
        {
            get { return _stopwatchModel.LapTime.HasValue ? _stopwatchModel.LapTime.Value.Minutes : 0; }
        }

        public decimal LapSeconds
        {
            get
            {
                if (_stopwatchModel.LapTime.HasValue)
                {
                    return (decimal)_stopwatchModel.LapTime.Value.Seconds + (_stopwatchModel.LapTime.Value.Milliseconds * .001M);
                }

                else return 0.0M;
            }
        }

        int _lastLapHours; // Lap 시간 변수 구현
        int _lastLapMinutes;
        decimal _lastLapSeconds;

        private void LapTimeUpdatedEventHandler(object sender, LapEventArgs e) 
        {
            if (_lastLapHours != LapHours)
            {
                _lastLapHours = LapHours;
                OnPropertyChanged("LapHours");
            }

            if (_lastMinutes != LapMinutes)
            {
                _lastLapMinutes = LapMinutes;
                OnPropertyChanged("LapMinutes");
            }

            if (_lastLapSeconds != LapSeconds)
            {
                _lastLapSeconds = LapSeconds;
                OnPropertyChanged("LapSeconds");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged; // 일반적인 이벤트 핸들러 양식
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null) propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
