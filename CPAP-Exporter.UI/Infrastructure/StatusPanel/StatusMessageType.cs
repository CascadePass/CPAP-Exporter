namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Represents the semantic classification of a status message, guiding its visual and behavioral presentation.
    /// </summary>
    public enum StatusMessageType
    {
        /// <summary>
        /// Indicates that no classification has been explicitly set.
        /// Used when the status message has default or neutral styling,
        /// or when the content is unrelated to IStatusMessage semantics.
        /// </summary>
        None,

        /// <summary>
        /// Represents informational messages conveying neutral system feedback.
        /// Typically styled with low visual urgency.
        /// </summary>
        Info,

        /// <summary>
        /// Represents messages signaling potential issues or user-related concerns.
        /// Often styled to gently attract attention without implying critical failure.
        /// </summary>
        Warning,

        /// <summary>
        /// Represents error states or critical failures that require immediate attention.
        /// Typically styled with high visual prominence and may trigger accessibility alerts.
        /// </summary>
        Error,

        /// <summary>
        /// Represents ongoing system activity such as loading or processing.
        /// May include progress indicators or subtle animations.
        /// Can be used to improve accessibility signaling for assistive technologies.
        /// </summary>
        Busy,

        /// <summary>
        /// Represents custom or interactive UI content beyond standard message types.
        /// Used when the message contains elements such as buttons, forms, or dynamic visuals.
        /// Styling and behavior are defined by the message implementation.
        /// </summary>
        Custom,
    }
}
