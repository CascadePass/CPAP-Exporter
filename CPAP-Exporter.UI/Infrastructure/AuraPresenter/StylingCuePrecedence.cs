namespace CascadePass.CPAPExporter
{
    public enum StylingCuePrecedence
    {
        /// <summary>
        /// Local XAML or code-behind values take precedence
        /// </summary>
        PreferLocalValues,

        /// <summary>
        /// Always apply message values, even if local values exist
        /// </summary>
        PreferMessageValues,

        /// <summary>
        /// Use local if set; otherwise defer to message then style provider
        /// </summary>
        AutoFallback,
    }
}
