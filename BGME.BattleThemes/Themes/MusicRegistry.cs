using BGME.BattleThemes.Configuration;
using Ryo.Interfaces;

namespace BGME.BattleThemes.Themes;

internal class MusicRegistry(IRyoApi ryo, Game game, Config config)
{
    public record ModSong(string ModOwner, string Name, int BgmId);

    private readonly string[] _supportedExts = [".hca"];
    private readonly List<ModSong> _music = [];

    public void RegisterModMusic(string modId, string modDir)
    {
        var musicDir = Path.Join(modDir, "battle-themes", "music");
        if (!Directory.Exists(musicDir)) return;

        foreach (var file in Directory.EnumerateFiles(musicDir, "*", SearchOption.AllDirectories))
        {
            if (!_supportedExts.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase)) continue;

            var name = Path.GetFileNameWithoutExtension(file);
            var bgmId = GetNextBgmId();
            RegisterRyoSong(file, bgmId);
            
            _music.Add(new(modId, name, bgmId));
            Log.Information($"Registered song: {name} || Mod: {modId} || BGM ID: {bgmId}");
        }
    }

    /// <summary>
    /// Gets the list of songs added by the specified mod.
    /// </summary>
    /// <param name="modId">Mod ID to get songs for.</param>
    /// <returns>Array of songs.</returns>
    public ModSong[] GetModSongs(string modId) => _music.Where(x => x.ModOwner == modId).ToArray();

    private void RegisterRyoSong(string file, int bgmId)
    {
        switch (game)
        {
            case Game.P3R_PC:
                ryo.AddAudioPath(file, new()
                {
                    CueName = bgmId.ToString(),
                    AcbName = "bgm",
                    CategoryIds = [0, 13],
                });
                break;
            case Game.P5R_PC:
                ryo.AddAudioPath(file, new()
                {
                    CueName = bgmId.ToString(),
                    AcbName = "bgm",
                    CategoryIds = [1, 8],
                });
                break;
            case Game.P4G_PC:
                ryo.AddAudioPath(file, new()
                {
                    CueName = bgmId.ToString(),
                    AcbName = "snd00_bgm",
                    CategoryIds = [6, 13],
                });
                break;
            case Game.P3P_PC:
                ryo.AddAudioPath(file, new()
                {
                    AudioFilePath = $"data/sound/bgm/{bgmId}.adx",
                });
                break;
            case Game.Metaphor:
                ryo.AddAudioPath(file, new()
                {
                    CueName = bgmId.ToString(),
                    AcbName = "bgm",
                    CategoryIds = [0, 9, 12],
                });
                break;
            case Game.SMT5V:
                ryo.AddAudioPath(file, new()
                {
                    CueName = bgmId.ToString(),
                    AcbName = "bgm",
                    CategoryIds = [0, 4, 9, 40, 11, 35, 50],
                });
                break;
            default:
                throw new ArgumentOutOfRangeException($"Unknown game: {game}");
        }
    }

    private int GetNextBgmId() => GetBaseBgmId() + _music.Count;

    private int GetBaseBgmId() => game switch
    {
        Game.P3P_PC => config.BaseBgmId_P3P,
        Game.P4G_PC => config.BaseBgmId_P4G,
        Game.P5R_PC => config.BaseBgmId_P5R,
        Game.P3R_PC => config.BaseBgmId_P3R,
        Game.Metaphor => config.BaseBgmId_Meta,
        Game.SMT5V => config.BaseBgmId_SMT5V,
        _ => throw new Exception("Unknown game."),
    };
}
