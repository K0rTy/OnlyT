﻿namespace OnlyT.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Moq;
    using Services.Bell;
    using Services.Options;
    using Services.TalkSchedule;
    using Services.Timer;
    using Utils;
    using ViewModel;

    [TestClass]
    public class TestViewModels
    {
        [TestMethod]
        public void TestOperatorViewStartStop()
        {
            const int TalkIdStart = 500;
            const int NumTalks = 3;

            Mock<IOptionsService> optionsService = new Mock<IOptionsService>();
            optionsService.Setup(o => o.Options).Returns(MockOptions.Create());

            Mock<ITalkTimerService> timerService = new Mock<ITalkTimerService>();
            Mock<IAdaptiveTimerService> adaptiveTimerService = new Mock<IAdaptiveTimerService>();
            ITalkScheduleService scheduleService = new MockTalksScheduleService(TalkIdStart, NumTalks);
            Mock<IBellService> bellService = new Mock<IBellService>();

            var vm = new OperatorPageViewModel(
                timerService.Object, 
                scheduleService,
                adaptiveTimerService.Object, 
                optionsService.Object,
                bellService.Object);

            Assert.IsFalse(vm.IsRunning);
            Assert.IsFalse(vm.IsManualMode);

            for (int n = 0; n < NumTalks; ++n)
            {
                int talkId = TalkIdStart + n;
                Assert.IsTrue(vm.TalkId == talkId);

                var talk = scheduleService.GetTalkScheduleItem(talkId);
                Assert.IsNotNull(talk);
                Assert.AreEqual(vm.CurrentTimerValueString, TimeFormatter.FormatTimerDisplayString(talk.GetDurationSeconds()));

                vm.StartCommand.Execute(null);
                Assert.IsTrue(vm.IsRunning);

                Assert.IsTrue(vm.TalkId == TalkIdStart + n);

                vm.StopCommand.Execute(null);
                Assert.IsFalse(vm.IsRunning);

                Assert.IsTrue(vm.TalkId == (n == NumTalks - 1 ? 0 : TalkIdStart + n + 1));
            }
        }
    }
}
