using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SubCategoryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public CategoryModel Category { get; set; }
        public ICollection<ProductModel>? Products { get; set; }
    }
}
