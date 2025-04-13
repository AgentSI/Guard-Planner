namespace Domain.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string? OriginalName { get; set; }
        public string? DisplayName { get; set; }
        public bool IsChecked { get; set; }
        public string? Href { get; set; }
        public string? Icon { get; set; }
    }
}
