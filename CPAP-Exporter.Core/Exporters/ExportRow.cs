namespace CascadePass.CPAPExporter.Core
{
    public class ExportRow
    {
        public ExportRow()
        {
            this.Values = [];
        }

        public ExportRow(int rowNumber, int sessionNumber, DateTime timestamp) : this()
        {
            this.RowNumber = rowNumber;
            this.Timestamp = timestamp;
            this.SessionNumber = sessionNumber;
        }

        public int RowNumber { get; set; }

        public int SessionNumber { get; set; }

        public DateTime Timestamp { get; set; }

        public Dictionary<string, string> Values { get; set; }

        public bool IsEmpty
        {
            get
            {
                if(this.Values is null || this.Values.Count == 0)
                {
                    return true;
                }

                return !this.Values.Any(item => !string.IsNullOrWhiteSpace(item.Value) && item.Value != "0");
            }
        }
    }
}
