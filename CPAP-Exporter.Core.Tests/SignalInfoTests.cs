using cpaplib;

namespace CascadePass.CPAPExporter.Core.Tests
{
    [TestClass]
    public class SignalInfoTests
    {
        [TestMethod]
        public void ExamineReport_EmptyReport_ReturnsEmptyList()
        {
            DailyReport report = new();
            List<SignalInfo> expected = [];

            List<SignalInfo> actual = SignalInfo.ExamineReport(report);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExamineReport_SingleSession()
        {
            DailyReport report = new();
            Session session = new();
            Signal signal = new()
            {
                Name = "TestSignal",
                FrequencyInHz = 1.0,
                UnitOfMeasurement = "TestUnit",
            };

            signal.Samples.Add(1.0);
            session.Signals.Add(signal);
            report.Sessions.Add(session);

            List<SignalInfo> actual = SignalInfo.ExamineReport(report);

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("TestSignal", actual[0].Name);
            Assert.AreEqual(1.0, actual[0].FrequencyInHz);
            Assert.AreEqual("TestUnit", actual[0].UnitOfMeasurement);
            Assert.AreEqual(1, actual[0].SampleCount);
            Assert.AreEqual(1.0, actual[0].Sample);
        }

        [TestMethod]
        public void ExamineReport_MultipleSessions()
        {
            DailyReport report = new();
            Session session1 = new();
            Signal signal1 = new()
            {
                Name = "TestSignal1",
                FrequencyInHz = 1.0,
                UnitOfMeasurement = "TestUnit1",
            };

            signal1.Samples.Add(1.0);
            session1.Signals.Add(signal1);
            report.Sessions.Add(session1);
            Session session2 = new();
            Signal signal2 = new()
            {
                Name = "TestSignal2",
                FrequencyInHz = 2.0,
                UnitOfMeasurement = "TestUnit2",
            };

            signal2.Samples.Add(2.0);
            session2.Signals.Add(signal2);
            report.Sessions.Add(session2);

            List<SignalInfo> actual = SignalInfo.ExamineReport(report);

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("TestSignal1", actual[0].Name);
            Assert.AreEqual(1.0, actual[0].FrequencyInHz);
            Assert.AreEqual("TestUnit1", actual[0].UnitOfMeasurement);
            Assert.AreEqual(1, actual[0].SampleCount);
            Assert.AreEqual(1.0, actual[0].Sample);
            Assert.AreEqual("TestSignal2", actual[1].Name);
            Assert.AreEqual(2.0, actual[1].FrequencyInHz);
            Assert.AreEqual("TestUnit2", actual[1].UnitOfMeasurement);
            Assert.AreEqual(1, actual[1].SampleCount);
            Assert.AreEqual(2.0, actual[1].Sample);
        }

        [TestMethod]
        public void ExamineReport_MultipleSignals()
        {
            DailyReport report = new();
            Session session = new();
            Signal signal1 = new()
            {
                Name = "TestSignal1",
                FrequencyInHz = 1.0,
                UnitOfMeasurement = "TestUnit1",
            };
            signal1.Samples.Add(1.0);
            session.Signals.Add(signal1);
            Signal signal2 = new()
            {
                Name = "TestSignal2",
                FrequencyInHz = 2.0,
                UnitOfMeasurement = "TestUnit2",
            };
            signal2.Samples.Add(2.0);
            session.Signals.Add(signal2);
            report.Sessions.Add(session);
            List<SignalInfo> actual = SignalInfo.ExamineReport(report);
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("TestSignal1", actual[0].Name);
            Assert.AreEqual(1.0, actual[0].FrequencyInHz);
            Assert.AreEqual("TestUnit1", actual[0].UnitOfMeasurement);
            Assert.AreEqual(1, actual[0].SampleCount);
            Assert.AreEqual(1.0, actual[0].Sample);
            Assert.AreEqual("TestSignal2", actual[1].Name);
            Assert.AreEqual(2.0, actual[1].FrequencyInHz);
            Assert.AreEqual("TestUnit2", actual[1].UnitOfMeasurement);
            Assert.AreEqual(1, actual[1].SampleCount);
            Assert.AreEqual(2.0, actual[1].Sample);
        }
    }
}
