using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CategoryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<SubCategoryModel>? SubCategories { get; set; }
    }
}
