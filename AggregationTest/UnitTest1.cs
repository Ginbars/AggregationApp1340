using AggregationApp;
using CsvHelper;
using System.Globalization;

namespace AggregationTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ProcessStream_ReturnsElectricityDataList()
        {
            var stubStream = GenerateFakeStream();
            var expected = new List<ElectricityData>()
            {
                new ElectricityData("a", "a", "a", "a", 0f, new DateTime(), 0f)
            };

            var result = DataHandler.ProcessData(stubStream);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FilterDataList_ReturnsDataOfApartment()
        {
            var electricityData = new List<ElectricityData>()
            {
                new ElectricityData("", "Butas", "", "", 0f, new DateTime(), 0f),
                new ElectricityData("", "Namas", "", "", 0f, new DateTime(), 0f)
            };

            DataHandler.FilterByObjName(electricityData, "Butas");

            Assert.IsTrue(electricityData.Count == 1);
            
        }

        [TestMethod]
        public void AddAggregation_ReturnsAggregatedData()
        {
            var aggregatedData = new AggregatedData() { Region = "", PPlusSum = 0f, PMinusSum = 0f };
            var electricityData = new ElectricityData("", "", "", "", 1.5f, new DateTime(), 0.5f);
            var expected = new AggregatedData() { Region = "", PPlusSum = 1.5f, PMinusSum = 0.5f };

            var result = aggregatedData.AddAggregation(electricityData);
            
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AggregateDataList_ReturnsAggregatedList()
        {
            var electricity = new List<ElectricityData>()
            {
                new ElectricityData("Test1 region", "", "", "", 1.5f, new DateTime(), 0.5f),
                new ElectricityData("Test1 region", "", "", "", 1.5f, new DateTime(), 0.5f),
            };
            var expected = new List<AggregatedData>()
            {
                new AggregatedData() { Region = "Test1 region", PPlusSum = 3.0f, PMinusSum = 1.0f },
            };

            var result = DataHandler.AggregateData(electricity);

            CollectionAssert.AreEqual(expected, result);
        }

        private Stream GenerateFakeStream()
        {
            var entries = new List<ElectricityDataEntry>()
            {
                new ElectricityDataEntry() { Region = "a", Obj_Name = "a", Obj_Number = "a", Obj_Type = "a", PPlus = 0f, Time = new DateTime(), PMinus = 0f },
                new ElectricityDataEntry() { Region = "a", Obj_Number = "a", Obj_Type = "a", PPlus = 0f, Time = new DateTime(), PMinus = 0f },
                new ElectricityDataEntry() { Region = "a", Obj_Name = "a", Obj_Number = "a", Obj_Type = "a", PPlus = 0f, Time = new DateTime() },
            };

            var memStream = new MemoryStream();
            var writer = new StreamWriter(memStream);
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            
            csv.WriteRecords(entries);
            csv.Flush();
            memStream.Position = 0;
            
            return memStream;
        }
    }
}