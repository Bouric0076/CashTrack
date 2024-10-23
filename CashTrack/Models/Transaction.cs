// Models/Transaction.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Models
{
    public enum TransactionType
    {
        Income,
        Expense
    }

    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction type is required.")]
        public TransactionType Type { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Transaction Date")]
        public DateTime Date { get; set; } = DateTime.Now;

        // Foreign key to IdentityUser
        [ForeignKey("IdentityUser")]
        public string? UserId { get; set; }

        // Payment Method
        [Required(ErrorMessage = "Payment method is required.")]
        [Display(Name = "Payment Method")]
        public string? PaymentMethod { get; set; } // Options: MPesa, Cash, Bank
    }
}
