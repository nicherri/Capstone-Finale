﻿using System.ComponentModel.DataAnnotations;
using TravelEasy.Models;

public class Comment
{
    public int Id { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}

