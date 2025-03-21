using cpaplib;

namespace CascadePass.CPAPExporter
{
    public interface ICpapSourceValidator
    {
        ICpapDataLoader GetLoader(string rootFolder);

        bool IsCpapFolderStructure(string rootFolder);
    }
}
