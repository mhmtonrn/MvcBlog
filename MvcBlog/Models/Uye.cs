namespace MvcBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Uye")]
    public partial class Uye
    {
        public Uye()
        {
            Yorums = new HashSet<Yorum>();
        }

        public int UyeId { get; set; }

        [StringLength(50)]
        public string KullaniciAdi { get; set; }

        [StringLength(50)]
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [StringLength(50)]
        public string Sifre { get; set; }

        [StringLength(50)]
        public string AdSoyad { get; set; }

        [StringLength(250)]
        public string Foto { get; set; }

        public int? Yetki { get; set; }

        public virtual Yetki Yetki1 { get; set; }

        public virtual ICollection<Yorum> Yorums { get; set; }
    }
}
