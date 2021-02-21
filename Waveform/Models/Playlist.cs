using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Waveform.Models
{
  public class Playlist
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public byte[] Image { get; set; }

    public string Songs { get; set; }

    [Required]
    public string UserId { get; set; }

    public string DateCreated { get; set; }
  }
}
