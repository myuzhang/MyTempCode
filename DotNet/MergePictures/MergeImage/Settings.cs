namespace MergeImage
{
    public class Settings
    {
        public int BlockNumber { get; set; }
        public int BorderWidthInPixel { get; set; }
        public string BackgroundColorName { get; set; }
        public string PlotBorderColorName { get; set; }
        public int TolorenceBackgroundR { get; set; }
        public int TolorenceBackgroundG { get; set; }
        public int TolorenceBackgroundB { get; set; }
        public int TolorencePlotBorderR { get; set; }
        public int TolorencePlotBorderG { get; set; }
        public int TolorencePlotBorderB { get; set; }
        public bool NeedScale { get; set; }
    }
}
