using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Defines a provider for default values determining the visual styles and behaviors of Aura Content.
    /// </summary>
    /// <remarks>This interface allows customization of the appearance and animation of Aura Content by
    /// providing brushes, dimensions, and other style-related properties based on the characteristics of
    /// a given <see cref="IAuraContent"/>.</remarks>
    public interface IStylingCueProvider
    {
        /// <summary>
        /// Retrieves a brush that visually represents the attention stripe for the specified Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content for which the attention stripe brush is to be generated.</param>
        /// <returns>A <see cref="Brush"/> object that represents the attention stripe for the given Aura Content.</returns>
        Brush GetAttentionStripeBrush(IAuraContent auraContent);

        /// <summary>
        /// Calculates the width of the attention stripe based on the provided Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content used to determine the width of the attention stripe.</param>
        /// <returns>The width of the attention stripe, in pixels, as a <see cref="double"/>.  Returns 0 if the content does not
        /// require an attention stripe.</returns>
        double GetAttentionStripeWidth(IAuraContent auraContent);

        /// <summary>
        /// Retrieves the appropriate foreground brush for the specified Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content for which the foreground brush is determined.</param>
        /// <returns>A <see cref="Brush"/> instance representing the foreground color for the given Aura Content. The returned
        /// brush may vary based on the content's type or severity.</returns>
        Brush GetForegroundBrush(IAuraContent auraContent);

        /// <summary>
        /// Retrieves the appropriate background brush for the specified Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content for which the background brush is determined.</param>
        /// <returns>A <see cref="Brush"/> instance representing the background styling for the given Aura Content. Returns
        /// <see langword="null"/> if no brush is applicable.</returns>
        Brush GetBackgroundBrush(IAuraContent auraContent);

        /// <summary>
        /// Determines the appropriate border brush for a Aura Presenter based on the provided Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content used to determine the border brush.</param>
        /// <returns>A <see cref="Brush"/> representing the border color for the Aura Presenter. The returned brush may vary
        /// depending on the severity or type of the Aura Content.</returns>
        Brush GetBorderBrush(IAuraContent auraContent);

        /// <summary>
        /// Retrieves the border thickness for the specified Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content for which the border thickness is determined.</param>
        /// <returns>A <see cref="Thickness"/> structure representing the border thickness associated with the given status
        /// content.</returns>
        Thickness GetBorderThickness(IAuraContent auraContent);

        /// <summary>
        /// Retrieves the corner radius value associated with the specified Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content for which the corner radius is to be determined.</param>
        /// <returns>The corner radius as a <see cref="double"/>. Returns 0 if the content does not specify a corner radius.</returns>
        double GetCornerRadius(IAuraContent auraContent);

        /// <summary>
        /// Retrieves the fade-in animation setting for the specified Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content for which the fade in setting is to be determined.</param>
        /// <returns>The fade in setting as a <see cref="bool"/>.</returns>
        bool GetFadeIn(IAuraContent auraContent);

        /// <summary>
        /// Retrieves the fade-out animation setting for the specified Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content for which the fade out setting is to be determined.</param>
        /// <returns>The fade in setting as a <see cref="bool"/>.</returns>
        bool GetFadeOut(IAuraContent auraContent);

        /// <summary>
        /// Retrieves the pulse border animation setting for the specified Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content for which the pulse border setting is to be determined.</param>
        /// <returns>The pulse border setting as a <see cref="bool"/>.</returns>
        bool GetPulseBorder(IAuraContent auraContent);

        /// <summary>
        /// Retrieves the border end color for the specified Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content for which the border end color setting is to be determined.</param>
        /// <returns>The border end color setting as a <see cref="bool"/>.</returns>
        Color GetPulseEndColor(IAuraContent auraContent);

        /// <summary>
        /// Retrieves the border start color for the specified Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content for which the border start color setting is to be determined.</param>
        /// <returns>The border start color setting as a <see cref="bool"/>.</returns>
        Color GetPulseStartColor(IAuraContent auraContent);

        /// <summary>
        /// Retrieves the drop shadow visibility setting for the specified Aura Content.
        /// </summary>
        /// <param name="auraContent">The Aura Content for which the drop shadow visibility setting is to be determined.</param>
        /// <returns>The drop shadow visibility setting as a <see cref="bool"/>.</returns>
        bool GetShowDropShadow(IAuraContent auraContent);

        #region Shadow Values

        Color GetShadowColor(IAuraContent auraContent);
        double GetShadowOpacity(IAuraContent auraContent);
        double GetShadowBlurRadius(IAuraContent auraContent);
        double GetShadowDepth(IAuraContent auraContent);

        #endregion

        TimeSpan? GetDisplayDuration(IAuraContent auraContent);
        bool? GetIsHoverPauseEnabled(IAuraContent auraContent);
    }
}