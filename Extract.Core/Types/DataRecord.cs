namespace Extract.Core.Types
{
    public record DataRecord {
        public int FileIndex { get; init; }
        public int StartIndex { get; init; }
        public int Length { get; init; }
        public char StartChar { get; init; }
        public char EndChar { get; init; }
    }
}
