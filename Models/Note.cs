using System.ComponentModel.DataAnnotations;

namespace ADAssignment.Models
{
    public class Note
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name="Contents")]
        [StringLength(1500, MinimumLength = 3)]
        public string NoteBody { get; set; }

        [Required]
        public Category? Category { get; set; }
    }
}