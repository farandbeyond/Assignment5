namespace FirstApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GameGenre
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public string GameGenreId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name ="Game")]
        public string GameId { get; set; }

        [ForeignKey("GameId")]
        public virtual Game Game { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name ="Genre")]
        public string GenreId { get; set; }

        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }

        [Display(Name = "Create Date")]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; } = DateTime.UtcNow;

        public override string ToString()
        {
            return String.Format("{0} - {1}", Game.ToString(), Genre.ToString());
        }
    }
}
