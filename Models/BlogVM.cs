using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BlyckBox.Models
{
    public class BlogVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public IFormFile? Image { get; set; }
    }
}