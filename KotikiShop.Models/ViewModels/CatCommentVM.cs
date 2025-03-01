using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.Models.ViewModels
{
    public class CatCommentVM
    {
        public IEnumerable<CatComment>? catComments = new List<CatComment>();
        public Cat? cat {  get; set; }
        public int? TotalLikes { get; set; }
    }
}
