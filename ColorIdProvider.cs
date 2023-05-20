public static class ColorIdProvider
{
    public readonly record struct ColorIdInfo(string Number, string Name);
    
    private enum ColorIds
    {
        // Non-working
        Misc = 2, // sage
        Wasted = 11, // Tomato
        
        // Working
        Implementation = 3, // grape
        Learning = 5, // banana
        Meta = 9, // blueberry
    }

    public static List<ColorIdInfo> GetAllRelevantColorIdInfo()
    {
        return Enum.GetValues(typeof(ColorIds))
            .Cast<ColorIds>()
            .Select(c => new ColorIdInfo(c.ToNumber(), c.ToName()))
            .ToList();
    }

    public static List<ColorIdInfo> GetOnlyWorkingColorIds()
    {
        return GetAllRelevantColorIdInfo()
            .Where(info => info.Number != ColorIds.Misc.ToNumber() && info.Number != ColorIds.Wasted.ToNumber())
            .ToList();
    }

    public static ColorIdInfo GetDefaultCalendarColorIdInfo()
    {
        return new(ColorIds.Meta.ToNumber(), ColorIds.Meta.ToName());
    }

    private static string ToNumber(this ColorIds colorId)
    {
        return ((int) colorId).ToString();
    }   

    private static string ToName(this ColorIds colorId)
    {
        return colorId.ToString();
    }
}
