namespace CascadePass.CPAPExporter
{
    public enum MessageOverrideBehavior
    {
        PreferLocalValues,       // Local XAML or code-behind values take precedence
        PreferMessageValues,     // Always apply message values, even if local values exist
        AutoFallback             // Use local if set; otherwise defer to message then style provider
    }
}
