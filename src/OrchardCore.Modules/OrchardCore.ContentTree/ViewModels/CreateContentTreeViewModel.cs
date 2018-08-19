using System.ComponentModel.DataAnnotations;

namespace OrchardCore.ContentTree.ViewModels
{
    public class CreateContentTreeViewModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class EditContentTreeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
