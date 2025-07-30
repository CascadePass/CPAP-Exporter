using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Defines a provider for default values determining the visual styles and behaviors of status messages.
    /// </summary>
    /// <remarks>This interface allows customization of the appearance and animation of status messages by
    /// providing brushes, dimensions, and other style-related properties based on the characteristics of
    /// a given <see cref="IStatusMessage"/>.</remarks>
    public interface IStatusMessageStyleProvider
    {
        /// <summary>
        /// Retrieves a brush that visually represents the attention stripe for the specified status message.
        /// </summary>
        /// <param name="message">The status message for which the attention stripe brush is to be generated.</param>
        /// <returns>A <see cref="Brush"/> object that represents the attention stripe for the given status message.</returns>
        Brush GetAttentionStripeBrush(IStatusMessage message);

        /// <summary>
        /// Calculates the width of the attention stripe based on the provided status message.
        /// </summary>
        /// <param name="message">The status message used to determine the width of the attention stripe.</param>
        /// <returns>The width of the attention stripe, in pixels, as a <see cref="double"/>.  Returns 0 if the message does not
        /// require an attention stripe.</returns>
        double GetAttentionStripeWidth(IStatusMessage message);

        /// <summary>
        /// Retrieves the appropriate foreground brush for the specified status message.
        /// </summary>
        /// <param name="message">The status message for which the foreground brush is determined.</param>
        /// <returns>A <see cref="Brush"/> instance representing the foreground color for the given status message. The returned
        /// brush may vary based on the message's type or severity.</returns>
        Brush GetForegroundBrush(IStatusMessage message);

        /// <summary>
        /// Retrieves the appropriate background brush for the specified status message.
        /// </summary>
        /// <param name="message">The status message for which the background brush is determined.</param>
        /// <returns>A <see cref="Brush"/> instance representing the background styling for the given status message. Returns
        /// <see langword="null"/> if no brush is applicable.</returns>
        Brush GetBackgroundBrush(IStatusMessage message);

        /// <summary>
        /// Determines the appropriate border brush for a status panel based on the provided status message.
        /// </summary>
        /// <param name="message">The status message used to determine the border brush.</param>
        /// <returns>A <see cref="Brush"/> representing the border color for the status panel. The returned brush may vary
        /// depending on the severity or type of the status message.</returns>
        Brush GetStatusPanelBorderBrush(IStatusMessage message);

        /// <summary>
        /// Retrieves the border thickness for the specified status message.
        /// </summary>
        /// <param name="message">The status message for which the border thickness is determined.</param>
        /// <returns>A <see cref="Thickness"/> structure representing the border thickness associated with the given status
        /// message.</returns>
        Thickness GetBorderThickness(IStatusMessage message);

        /// <summary>
        /// Retrieves the corner radius value associated with the specified status message.
        /// </summary>
        /// <param name="message">The status message for which the corner radius is to be determined.</param>
        /// <returns>The corner radius as a <see cref="double"/>. Returns 0 if the message does not specify a corner radius.</returns>
        double GetCornerRadius(IStatusMessage message);

        /// <summary>
        /// Retrieves the fade-in animation setting for the specified status message.
        /// </summary>
        /// <param name="message">The status message for which the fade in setting is to be determined.</param>
        /// <returns>The fade in setting as a <see cref="bool"/>.</returns>
        bool GetFadeIn(IStatusMessage message);

        /// <summary>
        /// Retrieves the fade-out animation setting for the specified status message.
        /// </summary>
        /// <param name="message">The status message for which the fade out setting is to be determined.</param>
        /// <returns>The fade in setting as a <see cref="bool"/>.</returns>
        bool GetFadeOut(IStatusMessage message);

        /// <summary>
        /// Retrieves the pulse border animation setting for the specified status message.
        /// </summary>
        /// <param name="message">The status message for which the pulse border setting is to be determined.</param>
        /// <returns>The pulse border setting as a <see cref="bool"/>.</returns>
        bool GetPulseBorder(IStatusMessage message);

        /// <summary>
        /// Retrieves the border end color for the specified status message.
        /// </summary>
        /// <param name="message">The status message for which the border end color setting is to be determined.</param>
        /// <returns>The border end color setting as a <see cref="bool"/>.</returns>
        Color GetPulseEndColor(IStatusMessage message);

        /// <summary>
        /// Retrieves the border start color for the specified status message.
        /// </summary>
        /// <param name="message">The status message for which the border start color setting is to be determined.</param>
        /// <returns>The border start color setting as a <see cref="bool"/>.</returns>
        Color GetPulseStartColor(IStatusMessage message);

        /// <summary>
        /// Retrieves the drop shadow visibility setting for the specified status message.
        /// </summary>
        /// <param name="message">The status message for which the drop shadow visibility setting is to be determined.</param>
        /// <returns>The drop shadow visibility setting as a <see cref="bool"/>.</returns>
        bool GetShowDropShadow(IStatusMessage message);

        #region Shadow Values

        Color GetShadowColor(IStatusMessage message);
        double GetShadowOpacity(IStatusMessage message);
        double GetShadowBlurRadius(IStatusMessage message);
        double GetShadowDepth(IStatusMessage message);

        #endregion

        public TimeSpan? GetDisplayDuration(IStatusMessage message);
    }
}