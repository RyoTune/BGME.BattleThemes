namespace BGME.BattleThemes.Interfaces;

/// <summary>
/// Record for storing information on a song from a mod
/// </summary>
/// <param name="ModOwner">Mod ID indicating the song's origin</param>
/// <param name="Name">Name of the song</param>
/// <param name="BgmId">BGM ID assigned to the song</param>
public record ModSong(string ModOwner, string Name, int BgmId);

/// <summary>
/// API for BGME Battle Themes
/// </summary>
public interface IBattleThemesApi
{
    /// <summary>
    /// Add a path to load themes from. Can be a <c>file</c> or <c>folder</c>.
    /// </summary>
    /// <param name="modId">ID of the target mod</param>
    /// <param name="path">Path to theme file or folder containing themes.</param>
    void AddPath(string modId, string path);

    /// <summary>
    /// Remove a previously added theme path.
    /// </summary>
    /// <param name="path">Theme path.</param>
    void RemovePath(string path);

    /// <summary>
    /// Add a listener that triggers if a battle theme is registered belonging to the target mod.
    /// </summary>
    /// <param name="modId">Mod ID that the song will belong to.</param>
    /// <param name="callback">Callback to execute.</param>
    void OnMusicRegistered(string modId, Action<ModSong> callback);

    /// <summary>
    /// Returns a list of songs registered to BGME Battle Themes from the given Mod ID.
    /// </summary>
    /// <param name="modId">ID of the target mod</param>
    /// <returns>A list of songs that have been registered. If registration has not happened yet or the mod
    /// doesn't use Battle Themes, an empty list is returned</returns>
    List<ModSong> GetRegisteredMusic(string modId);
}
