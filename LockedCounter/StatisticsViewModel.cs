using LockedCounter.Model;
using LockedCounter.Storage;
using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockedCounter
{
    public class StatisticsViewModel: VMBase
    {
        public BaseRepository<StateDuration> Repository { get; set; }

        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged();
                if(ShouldBeTrimmed(value))
                {
                    StartTime = TrimToHour(value);
                }
            }
        }

        private DateTime _endTime;

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnPropertyChanged();
                if(ShouldBeTrimmed(value))
                {
                    EndTime = TrimToHour(value);
                }
            }
        }
        
        private IList<DataPoint> _plotPoints = new List<DataPoint>();

        public IList<DataPoint> PlotPoints
        {
            get { return _plotPoints; }
            set
            {
                _plotPoints = value;
                OnPropertyChanged();
            }
        }

        public StatisticsViewModel()
        {
            var now = DateTime.Now;
            StartTime =  TrimToHour(now - TimeSpan.FromDays(1));
            EndTime = TrimToHour(now);
            DrawEmpty();
        }

        private void DrawEmpty()
        {
            PlotPoints = new List<DataPoint>()
            {
                new DataPoint(DateTimeAxis.ToDouble(StartTime), 0),
                new DataPoint(DateTimeAxis.ToDouble(EndTime), 0)
            };
        }

        public void Calculate()
        {
            var dataPoints = new List<DataPoint>();
            for(DateTime current = StartTime, end = StartTime.AddHours(1); current < EndTime; current = current.AddHours(1), end = end.AddHours(1))
            {
                IEnumerable<StateDuration> inRange = Repository
                .Elements
                .Where(x => (x.StartTime >= current && x.EndTime <= end)
                || (x.StartTime <= current && x.EndTime >= end)
                || (x.StartTime <= current && x.EndTime >= current && x.EndTime <= end)
                || (x.StartTime >= current && x.StartTime <= end && x.EndTime >= end));
                if(inRange.Count() == 0)
                {
                    dataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(current.AddMinutes(30)), 0));
                    continue;
                }
                double minutesLocked = 0, minutesUnlocked = 0;
                foreach(var duration in inRange)
                {
                    double sum = (duration.EndTime - duration.StartTime).TotalMinutes;
                    if(duration.StartTime < current)
                    {
                        sum -= (current - duration.StartTime).TotalMinutes;
                    }
                    if(duration.EndTime > end)
                    {
                        sum -= (duration.EndTime - end).TotalMinutes;
                    }
                    switch(duration.State)
                    {
                        case ScreenState.Locked: minutesLocked += sum; break;
                        case ScreenState.Unlocked: minutesUnlocked += sum; break;
                    }
                }
                var percent = minutesUnlocked / (minutesLocked + minutesUnlocked) * 100d;
                dataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(current.AddMinutes(30)), percent));
            }
            PlotPoints = dataPoints;
        }

        private bool ShouldBeTrimmed(DateTime dateTime)
        {
            return dateTime.Millisecond != 0
                || dateTime.Second != 0
                || dateTime.Minute != 0;
        }

        private DateTime TrimToHour(DateTime dateTime)
        {
            return dateTime
                .AddMilliseconds(-dateTime.Millisecond)
                .AddSeconds(-dateTime.Second)
                .AddMinutes(-dateTime.Minute);
        } 
    }
}
