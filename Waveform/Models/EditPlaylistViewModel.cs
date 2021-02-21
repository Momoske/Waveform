using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waveform.Models
{
  public class EditPlaylistViewModel
  {
    public Playlist Playlist { get; set; }
    public List<IFormFile> Image { get; set; }
    public IEnumerable<Song> PlaylistSongs { get; set; }
    public IEnumerable<Song> AllSongs { get; set; }
  }
}
