namespace grid
{
    public enum TileMapType
    {
        Floor,
        Furniture,
        FurniturePreview,
        Decorate,
        Max
    }

    // note : tilemap에 필요한 정의
    public static class TileMapDefinition
    {
        // note : tilemap의 범위
        public static class TileRange
        {
            public static int Left = -10;
            public static int Right = 10;
            public static int Down = -10;
            public static int Up = 10;
        }
    }
}