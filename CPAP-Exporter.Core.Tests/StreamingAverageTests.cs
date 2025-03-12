using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CascadePass.CPAPExporter.Core.Aggregates;

namespace CascadePass.CPAPExporter.Core.Tests
{
    [TestClass]
    public class StreamingAverageTests
    {
        [TestMethod]
        public void Sample_AddsValueToSumAndIncrementsCount()
        {
            var streamingAverage = new StreamingAverage();
            double value = 5.0;

            streamingAverage.Sample(value);

            Assert.AreEqual(value, streamingAverage.Value);
            Assert.AreEqual(1, streamingAverage.Count);
        }

        [TestMethod]
        public void Value_ReturnsZeroWhenNoSamples()
        {
            var streamingAverage = new StreamingAverage();

            Assert.AreEqual(0.0, streamingAverage.Value);
            Assert.AreEqual(0, streamingAverage.Count);
        }

        [TestMethod]
        public void Value_ReturnsCorrectAverage()
        {
            var streamingAverage = new StreamingAverage();
            streamingAverage.Sample(4.0);
            streamingAverage.Sample(6.0);

            Assert.AreEqual(5.0, streamingAverage.Value);
            Assert.AreEqual(2, streamingAverage.Count);
        }

        [TestMethod]
        public void Value_CalculatesCorrectAverageForLargeNumberOfSamples()
        {
            var streamingAverage = new StreamingAverage();
            int numberOfSamples = 1000000;
            double expectedAverage = 0.0;

            for (int i = 1; i <= numberOfSamples; i++)
            {
                streamingAverage.Sample(i);
                expectedAverage += i;
            }

            expectedAverage /= numberOfSamples;

            Assert.AreEqual(expectedAverage, streamingAverage.Value, 1e-9);
            Assert.AreEqual(numberOfSamples, streamingAverage.Count);
        }

        [TestMethod]
        public void Reset_ClearsSumAndCount()
        {
            var streamingAverage = new StreamingAverage();

            streamingAverage.Sample(4.0);
            streamingAverage.Sample(6.0);

            Assert.AreEqual(5.0, streamingAverage.Value);
            Assert.AreEqual(2, streamingAverage.Count);

            streamingAverage.Reset();

            Assert.AreEqual(0, streamingAverage.Value);
            Assert.AreEqual(0, streamingAverage.Count);
        }
    }
}
