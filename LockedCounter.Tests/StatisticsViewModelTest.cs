using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LockedCounter.Storage;
using LockedCounter.Model;
using System.Collections.Generic;

namespace LockedCounter.Tests
{
    [TestClass]
    public class StatisticsViewModelTest
    {
        static readonly DateTime Start = new DateTime(2016, 1, 1, 1, 0, 0);
        static readonly DateTime End = Start.AddHours(1);

        [TestMethod]
        public void BeforeStartAfterEnd()
        {
            StateDuration duration = new StateDuration() { StartTime = Start.AddMinutes(-5), EndTime = End.AddMinutes(5), State = ScreenState.Unlocked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration });
            StatisticsViewModel vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(100, vm.PlotPoints[0].Y);
        }

        [TestMethod]
        public void BeforeStartBeforeEnd()
        {
            var duration = new StateDuration() { StartTime = Start.AddMinutes(-5), EndTime = Start.AddMinutes(30), State = ScreenState.Unlocked };
            var duration2 = new StateDuration() { StartTime = Start.AddMinutes(30), EndTime = End, State = ScreenState.Locked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration, duration2 });
            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(50d, vm.PlotPoints[0].Y);
        }



        [TestMethod]
        public void AfterStartAfterEnd()
        {
            var duration = new StateDuration() { StartTime = Start, EndTime = Start.AddMinutes(30), State = ScreenState.Locked };
            var duration2 = new StateDuration() { StartTime = Start.AddMinutes(30), EndTime = End.AddMinutes(5), State = ScreenState.Unlocked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration, duration2 });
            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(50d, vm.PlotPoints[0].Y);
        }

        [TestMethod]
        public void AfterStartBeforeEnd()
        {
            var duration = new StateDuration() { StartTime = Start, EndTime = Start.AddMinutes(15), State = ScreenState.Locked };
            var duration2 = new StateDuration() { StartTime = Start.AddMinutes(15), EndTime = Start.AddMinutes(45), State = ScreenState.Unlocked };
            var duration3 = new StateDuration() { StartTime = Start.AddMinutes(45), EndTime = End, State = ScreenState.Locked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration, duration2, duration3 });
            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(50d, vm.PlotPoints[0].Y);
        }

        [TestMethod]
        public void OnStartOnEnd()
        {
            var duration = new StateDuration() { StartTime = Start, EndTime = End, State = ScreenState.Unlocked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration });
            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(100d, vm.PlotPoints[0].Y);
        }

        [TestMethod]
        public void BeforeStartOnEnd()
        {
            var duration = new StateDuration() { StartTime = Start.AddMinutes(-5), EndTime = End, State = ScreenState.Unlocked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration });
            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(100d, vm.PlotPoints[0].Y);
        }

        [TestMethod]
        public void OnStartBeforeEnd()
        {
            var duration = new StateDuration() { StartTime = Start, EndTime = Start.AddMinutes(30), State = ScreenState.Unlocked };
            var duration2 = new StateDuration() { StartTime = Start.AddMinutes(30), EndTime = End, State = ScreenState.Locked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration, duration2 });
            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(50d, vm.PlotPoints[0].Y);
        }

        [TestMethod]
        public void OnStartAfterEnd()
        {
            var duration = new StateDuration() { StartTime = Start, EndTime = End.AddMinutes(5), State = ScreenState.Unlocked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration });
            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(100d, vm.PlotPoints[0].Y);
        }

        [TestMethod]
        public void AfterStartOnEnd()
        {
            var duration = new StateDuration() { StartTime = Start, EndTime = Start.AddMinutes(30), State = ScreenState.Locked };
            var duration2 = new StateDuration() { StartTime = Start.AddMinutes(30), EndTime = End, State = ScreenState.Unlocked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration, duration2 });
            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(50d, vm.PlotPoints[0].Y);
        }

        [TestMethod]
        public void ManyDurations()
        {
            var duration = new StateDuration() { StartTime = Start.AddMinutes(-10), EndTime = Start.AddMinutes(15), State = ScreenState.Locked };
            var duration2 = new StateDuration() { StartTime = Start.AddMinutes(15), EndTime = Start.AddMinutes(25), State = ScreenState.Unlocked };
            var duration3 = new StateDuration() { StartTime = Start.AddMinutes(25), EndTime = Start.AddMinutes(40), State = ScreenState.Locked };
            var duration4 = new StateDuration() { StartTime = Start.AddMinutes(40), EndTime = End.AddMinutes(40), State = ScreenState.Unlocked };

            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration, duration2, duration3, duration4 });

            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(50d, vm.PlotPoints[0].Y);
        }

        [TestMethod]
        public void BreakInTheMiddle()
        {
            var duration = new StateDuration() { StartTime = Start.AddMinutes(-10), EndTime = Start.AddMinutes(15), State = ScreenState.Unlocked };
            var duration2 = new StateDuration() { StartTime = Start.AddMinutes(15), EndTime = End.AddMinutes(25), State = ScreenState.Unlocked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration, duration2 });

            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(100d, vm.PlotPoints[0].Y);
        }

        [TestMethod]
        public void ManyBreaks()
        {
            var duration = new StateDuration() { StartTime = Start.AddMinutes(-10), EndTime = Start.AddMinutes(5), State = ScreenState.Unlocked };
            var duration2 = new StateDuration() { StartTime = Start.AddMinutes(15), EndTime = Start.AddMinutes(20), State = ScreenState.Locked };
            var duration3 = new StateDuration() { StartTime = Start.AddMinutes(30), EndTime = Start.AddMinutes(35), State = ScreenState.Unlocked };
            var duration4 = new StateDuration() { StartTime = Start.AddMinutes(55), EndTime = End.AddMinutes(25), State = ScreenState.Locked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration, duration2 });

            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = End
            };
            vm.Calculate();
            Assert.AreEqual(1, vm.PlotPoints.Count);
            Assert.AreEqual(50d, vm.PlotPoints[0].Y);
        }

        [TestMethod]
        public void ManyPoints()
        {
            var end = Start.AddHours(2);
            var duration = new StateDuration() { StartTime = Start.AddMinutes(-30), EndTime = Start.AddMinutes(30), State = ScreenState.Unlocked };
            var duration2 = new StateDuration() { StartTime = Start.AddMinutes(30), EndTime = Start.AddMinutes(90), State = ScreenState.Locked };
            var duration3 = new StateDuration() { StartTime = Start.AddMinutes(90), EndTime = end, State = ScreenState.Unlocked };
            var repositoryMock = new StateDurationRepository(new List<StateDuration>() { duration, duration2, duration3 });

            var vm = new StatisticsViewModel()
            {
                Repository = repositoryMock,
                StartTime = Start,
                EndTime = end
            };
            vm.Calculate();
            Assert.AreEqual(2, vm.PlotPoints.Count);
            Assert.AreEqual(50d, vm.PlotPoints[0].Y);
            Assert.AreEqual(50d, vm.PlotPoints[1].Y);

        }
    }
}
