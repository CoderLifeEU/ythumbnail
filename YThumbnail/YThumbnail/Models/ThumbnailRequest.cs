using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YThumbnail.Models
{
    public class ThumbnailRequest
    {
        [Required]
        public string Prefix { get; set; }
        [Required]
        public string Link { get; set; }

        public ThumbnailRequest()
        {
            this.Prefix = this.GeneratePrefix();
        }
        public ThumbnailRequest(string prefix)
        {
            this.Prefix = prefix;
        }

        public string GeneratePrefix()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
