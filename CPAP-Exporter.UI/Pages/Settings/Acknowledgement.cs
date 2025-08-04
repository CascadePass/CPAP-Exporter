namespace CascadePass.CPAPExporter
{
    public class Acknowledgement
    {
        private DelegateCommand visitUrl;

        public string Title { get; set; }

        public string Url { get; set; }

        public DelegateCommand VisitUrlCommand =>  this.visitUrl ??= new DelegateCommand(() => this.VisitUrl());

        public void VisitUrl()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.Url))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = this.Url,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
