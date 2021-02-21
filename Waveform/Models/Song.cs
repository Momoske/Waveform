using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Waveform.Models
{
  public class Song
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public byte[] Audio { get; set; }

    [Required]
    public string UserId { get; set; }

    public string DateAdded { get; set; }
  }
}