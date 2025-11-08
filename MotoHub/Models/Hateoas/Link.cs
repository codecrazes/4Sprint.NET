namespace MotoHub.Models.Hateoas
{
    public class Link
    {
        public string Rel { get; set; } = string.Empty;
        public string Href { get; set; } = string.Empty;
        public string Method { get; set; } = "GET";
        public Link() { }
        public Link(string rel, string href, string method = "GET")
        {
            Rel = rel;
            Href = href;
            Method = method;
        }
    }
}
