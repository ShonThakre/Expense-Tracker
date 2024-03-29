﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models;

public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    [Required(ErrorMessage ="Title is required.")]
    public string Title { get; set; } 

    [Column(TypeName = "nvarchar(5)")]
    public string Icon { get; set; } = "";

    [Column(TypeName = "nvarchar(10)")]
    public string Type { get; set; } = "Expense";

    [NotMapped]
    public string? TitleWithIcon{ get {
            return this.Icon + " " + this.Title;
        } }

    //public string? Id {  get; set; } 
    //[ForeignKey("Id")]
    //public IdentityUser? UserId { get; set; }

    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    public IdentityUser? User { get; set; }

}
