using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
