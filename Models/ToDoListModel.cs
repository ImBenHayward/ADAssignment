using System;
using System.ComponentModel.DataAnnotations;

namespace ADAssignment.Models
{
    public class ToDoList
    {
        public string Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name="Card Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public string Url { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
    }
}