using System.Collections.Generic;

namespace MotoHub.Models.Hateoas
{
    public class Resource<T>
    {
        public T Data { get; set; } = default!;
        public List<Link> Links { get; set; } = new List<Link>();

        public Resource() { }
        public Resource(T data)
        {
            Data = data;
        }
    }
}