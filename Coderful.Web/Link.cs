namespace Coderful.Web
{
    public class Link
    {
        public Link(string anchor, string url)
        {
            this.Anchor = anchor;
            this.Url = url;
        }
        
        public string Url { get; set; }
        public string Anchor { get; set; }
    }
}