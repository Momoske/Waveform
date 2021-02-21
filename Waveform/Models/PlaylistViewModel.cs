using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waveform.Models
{
  public class PlaylistViewModel
  {
    public Playlist Playlist { get; set; }
    public List<IFormFile> Image { get; set; }
    public IEnumerable<Song> Songs { get; set; }
  }
}
