public abstract class ComicsLevel : LevelManager
{
    protected abstract string ComicsID { get; }
    protected abstract int ComicPage { get; }

    public override void Win()
    {
        base.Win();
        ShowWinComic();
    }

    public override void SkipLevel()
    {
        base.SkipLevel();
        ShowWinComic();
    }

    private void ShowWinComic()
    {
        gameManager.ShowComicWinUI(ComicsID, finalRegionLevel);
        ComicsManager.Instance.SetLevelComic(ComicPage);
        AudioManager.Instance.StopWinSources();
    }
}