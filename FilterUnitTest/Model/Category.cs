namespace FilterUnitTest.Model
{
    using System.Collections.Generic;

    public sealed class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public byte[] RowTimeStamp { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
