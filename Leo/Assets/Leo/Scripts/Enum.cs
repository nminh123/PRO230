namespace Leo
{
    public enum GrowthStage
    {
        barren,
        ploughed,
        planted,
        growing1,
        growing2,
        ripe
    }

    public enum ToolType
    {
        plough,
        wateringCan,
        seeds,
        basket
    }

    /// <summary>
    /// Locations on the players body where items can be equipped.
    /// </summary>
    public enum EquipLocation
    {
        Helmet,
        Necklace,
        Body,
        Trousers,
        Boots,
        Weapon,
        Shield,
        Gloves
    }
}