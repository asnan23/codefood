using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models.Response
{
    public class ServeHistoryAllDto
    {
        public string id { get; set; }
        public int userId { get; set; }
        public int nServing { get; set; }
        public int recipeId { get; set; }
        public int recipeCategoryId { get; set; }
        public string recipeName { get; set; }
        public string recipeCategoryName { get; set; }
        public string recipeImage { get; set; }
        public int nStep { get; set; }
        public int nStepDone { get; set; }
        public string reaction { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        public DateTime updatedAt { get; set; } = DateTime.Now;
    }

    public class ServeHisotryResponse
    {
        public int total { get; set; }
        public List<ServeHistoryAllDto> history { get; set; }
    }
}
