using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Models
{
    public class MessageContent
    {
        [Required]
        [StringLength(128, ErrorMessage = "Maximum 128 characters")]
        public string Text { get; set; }
    }
}
