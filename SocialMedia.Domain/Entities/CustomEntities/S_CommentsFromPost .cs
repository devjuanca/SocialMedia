using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Domain.Entities.CustomEntities
{
  public partial class S_CommentsFromPost
    {
        public int Comment_Id { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string User_Id { get; set; }
        public int PostId { get; set; }
        public string Name { get; set; }
    }
}
