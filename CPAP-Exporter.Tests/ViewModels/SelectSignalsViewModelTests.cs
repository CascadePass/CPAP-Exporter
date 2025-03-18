using cpaplib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class SelectSignalsViewModelTests
    {
        #region Title/Description Validation

        [TestMethod]
        public void HasCorrectTitle()
        {
            var vm = new SelectSignalsViewModel();
            Assert.AreEqual(Resources.PageTitle_SelectSignals, vm.Title);
        }

        [TestMethod]
        public void HasCorrectDescription()
        {
            var vm = new SelectSignalsViewModel();
            Assert.AreEqual(Resources.PageDesc_SelectSignals, vm.PageDescription);
        }

        #endregion

        [TestMethod]
        public void Validate_NoSignals()
        {
            ExportParameters exportParameters = new()
            {
                Reports = new()
            };

            SelectSignalsViewModel viewModel = new()
            {
                ExportParameters = exportParameters
            };

            var result = viewModel.Validate();

            Assert.IsFalse(result);

            Assert.IsNull(viewModel.ExportDetails);
            Assert.IsNull(viewModel.SignalDescriptions);
        }

        [TestMethod]
        public void Validate_NoReports()
        {
            ExportParameters exportParameters = new()
            {
                Signals =
                [
                    new(new()) { IsSelected = true },
                    new(new()) { IsSelected = true },
                    new(new()) { IsSelected = false },
                    new(new()) { IsSelected = false },
                ]
            };

            SelectSignalsViewModel viewModel = new()
            {
                ExportParameters = exportParameters
            };

            var result = viewModel.Validate();

            Assert.IsNull(viewModel.ExportDetails);
            Assert.IsNull(viewModel.SignalDescriptions);
        }

        [TestMethod]
        public void Validate_HasSelectedReports()
        {
            ExportParameters exportParameters = new()
            {
                Signals =
                [
                    new(new()) { IsSelected = true },
                    new(new()) { IsSelected = true },
                    new(new()) { IsSelected = false },
                    new(new()) { IsSelected = false },
                ]
            };

            SelectSignalsViewModel viewModel = new()
            {
                ExportParameters = exportParameters
            };

            var result = viewModel.Validate();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Validate_HasNoSelectedReports()
        {
            ExportParameters exportParameters = new()
            {
                Signals =
                [
                    new(new()) { IsSelected = false },
                    new(new()) { IsSelected = false },
                    new(new()) { IsSelected = false },
                    new(new()) { IsSelected = false },
                ]
            };

            SelectSignalsViewModel viewModel = new()
            {
                ExportParameters = exportParameters
            };

            var result = viewModel.Validate();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GenerateSampleCSV()
        {
            ExportParameters exportParameters = new()
            {
                Signals =
                [
                    new(new() { Name = "Signal1" } ) { IsSelected = true },
                    new(new() { Name = "Signal2" } ) { IsSelected = true },
                    new(new() { Name = "Signal3" }) { IsSelected = true },
                    new(new() { Name = "Signal4" }) { IsSelected = true },
                ],
                Reports =
                [
                    new(
                        new() {
                            Sessions =
                            [
                                new Session() {
                                    Signals =
                                    [
                                        new() { Name = "Signal1", FrequencyInHz = 0.5 },
                                        new() { Name = "Signal2", FrequencyInHz = 0.5 },
                                        new() { Name = "Signal3", FrequencyInHz = 0.5 },
                                        new() { Name = "Signal4", FrequencyInHz = 0.5 },
                                    ]
                                }
                            ]
                        },
                        true
                        )
                ]
            };

            SelectSignalsViewModel viewModel = new()
            {
                ExportParameters = exportParameters,
            };

            for (int signalIndex = 0; signalIndex < 4; signalIndex++)
            {
                for (double d = 0; d < 8; d++)
                {
                    exportParameters.Reports[0].DailyReport.Sessions[0].Signals[signalIndex].Samples.Add(d);
                }
            }

            string csv = viewModel.GenerateSampleCSV();

            // Log result for dev examination
            Console.WriteLine(csv);

            // Should contain a string value
            Assert.IsNotNull(csv);

            string[] lines = csv.Split(Environment.NewLine);

            // Should contain columns for all four test signals
            Assert.IsTrue(lines[0].Contains(",Signal1,Signal2,Signal3,Signal4"));

            // Should contain a line for each sample
            for (int i = 0; i < exportParameters.Reports[0].DailyReport.Sessions[0].Signals.Count; i++)
            {
                Assert.IsTrue(lines[i + 1].StartsWith($"{i},"), $"Line {i+1} did not start with '{i},'");
            }

            Assert.AreEqual(csv, viewModel.SampleCSV);
        }
    }
}
