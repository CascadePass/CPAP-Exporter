using cpaplib;
using System.IO;

namespace CascadePass.CPAPExporter
{
    public class CpapSourceValidator : ICpapSourceValidator
    {
        public bool IsCpapFolderStructure(string rootFolder)
        {
            var loader = this.GetLoader(rootFolder);

            if (loader != null)
            {
                return loader.HasCorrectFolderStructure(rootFolder);
            }

            return false;
        }

        public ICpapDataLoader GetLoader(string rootFolder)
        {
            if (!Path.Exists(rootFolder))
            {
                return null;
            }

            ICpapDataLoader loader = new ResMedDataLoader();

            if (!loader.HasCorrectFolderStructure(rootFolder))
            {
                loader = new PRS1DataLoader();

                if (!loader.HasCorrectFolderStructure(rootFolder))
                {
                    return null;
                }
            }

            return loader;
        }
    }
}
