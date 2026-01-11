using Ciciovan_Bogdan_Ionut_HotelReservations.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class BookingHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        [Display(Name = "Rezervare")]
        public int BookingId { get; set; }

        [StringLength(100)]
        [Display(Name = "Modificat de")]
        public string? ChangedByUser { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tip modificare")]
        public string ChangeType { get; set; }

        [StringLength(500)]
        [Display(Name = "Descriere modificare")]
        public string? ChangeDescription { get; set; }

        [StringLength(20)]
        [Display(Name = "Status vechi")]
        public string? OldStatus { get; set; }

        [StringLength(20)]
        [Display(Name = "Status nou")]
        public string? NewStatus { get; set; }

        [Required]
        [Display(Name = "Data modificarii")]
        public DateTime ChangedDate { get; set; } = DateTime.Now;

        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }
    }
}