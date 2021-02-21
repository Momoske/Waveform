using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Waveform.Areas.Identity.Data;
using Waveform.Models;

namespace Waveform.Controllers
{
  public class PlaylistController : Controller
  {
    private readonly UserManager<WaveformUser> _userManager;
    private readonly SignInManager<WaveformUser> _signInManager;

    public PlaylistController(UserManager<WaveformUser> userManager, SignInManager<WaveformUser> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    readonly SongDataLayer songDataLayer = new SongDataLayer();
    readonly PlaylistDataLayer playlistDataLayer = new PlaylistDataLayer();

    public IActionResult Index()
    {
      if (!(_signInManager.IsSignedIn(User))) return RedirectToAction("Index", "Home", new { area = "" });
      return View(playlistDataLayer.GetAllPlaylists(_userManager.GetUserId(User)).ToList());
    }

    [HttpGet]
    public IActionResult Create()
    {
      if (!(_signInManager.IsSignedIn(User))) return RedirectToAction("Index", "Home", new { area = "" });

      PlaylistViewModel playlistSongs = new PlaylistViewModel
      {
        Songs = songDataLayer.GetAllSongs(_userManager.GetUserId(User)).ToList()
      };

      return View(playlistSongs);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind] PlaylistViewModel playlistSongs)
    {
      // Keeping for potential future projects
      /*foreach (var value in ModelState.Values)
      {
        foreach (ModelError error in value.Errors)
        {
          Debug.WriteLine(error.ErrorMessage);
        }
      }*/
      if (ModelState.IsValid)
      {
        if (playlistSongs.Image != null)
        {
          foreach (var item in playlistSongs.Image)
          {
            if (item.Length > 0)
            {
              using var stream = new MemoryStream();
              await item.CopyToAsync(stream);
              playlistSongs.Playlist.Image = stream.ToArray();
            }
          }
        }
        playlistDataLayer.CreatePlaylist(playlistSongs.Playlist, _userManager.GetUserId(User));
        return RedirectToAction("Index");
      }

      return View(playlistSongs);
    }

    [HttpGet]
    public IActionResult Edit(int? Id)
    {
      if (!(_signInManager.IsSignedIn(User))) return RedirectToAction("Index", "Home", new { area = "" });
      if (Id == null) return RedirectToAction("Index");

      EditPlaylistViewModel editPlaylist = new EditPlaylistViewModel
      {
        Playlist = playlistDataLayer.GetPlaylist(Id),
        AllSongs = songDataLayer.GetAllSongs(_userManager.GetUserId(User)).ToList()
      };

      if (
        editPlaylist.Playlist == null || editPlaylist.Playlist.Id == 0
        || editPlaylist.Playlist.UserId != _userManager.GetUserId(User)
      )
      { return RedirectToAction("Index"); }

      List<Song> songs = new List<Song>();
      string[] ids = editPlaylist.Playlist.Songs.Split(',');

      foreach (var id in ids)
      {
        if (id != null && id != "") songs.Add(playlistDataLayer.GetPlaylistSong(id));
      }

      if (songs.Any()) editPlaylist.PlaylistSongs = songs;

      return View(editPlaylist);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? Id, [Bind] EditPlaylistViewModel editPlaylist)
    {
      if (Id == null || editPlaylist.Playlist.Id == 0) return RedirectToAction("Index");

      if (ModelState.IsValid)
      {
        if (editPlaylist.Image != null)
        {
          foreach (var item in editPlaylist.Image)
          {
            if (item.Length > 0)
            {
              using var stream = new MemoryStream();
              await item.CopyToAsync(stream);
              editPlaylist.Playlist.Image = stream.ToArray();
            }
          }
        }
        playlistDataLayer.UpdatePlaylist(editPlaylist.Playlist);
        return RedirectToAction("Index");
      }
      
      return View(editPlaylist);
    }

    [HttpGet]
    public IActionResult Details(int? Id)
    {
      PlaylistViewModel details = new PlaylistViewModel();

      if (!(_signInManager.IsSignedIn(User))) return RedirectToAction("Index", "Home", new { area = "" });
      if (Id == null) return RedirectToAction("Index");

      Playlist playlist = playlistDataLayer.GetPlaylist(Id);

      if (
        playlist == null || playlist.Id == 0 ||
        playlist.UserId != _userManager.GetUserId(User)
      )
      { return RedirectToAction("Index"); }

      details.Playlist = playlist;
      
      List<Song> songs = new List<Song>();
      string[] ids = playlist.Songs.Split(',');

      foreach (var id in ids)
      {
        if (id != null && id != "") songs.Add(playlistDataLayer.GetPlaylistSong(id));
      }

      if (songs.Any()) details.Songs = songs;

      return View(details);
    }

    [HttpGet]
    public IActionResult Delete(int? Id)
    {
      if (!(_signInManager.IsSignedIn(User))) return RedirectToAction("Index", "Home", new { area = "" });
      if (Id == null) return RedirectToAction("Index");

      Playlist playlist = playlistDataLayer.GetPlaylist(Id);

      if (
        playlist == null || playlist.Id == 0 ||
        playlist.UserId != _userManager.GetUserId(User)
      )
      { return RedirectToAction("Index"); }

      return View(playlist);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePlaylist(int? Id)
    {
      playlistDataLayer.DeletePlaylist(Id);
      return RedirectToAction("Index");
    }

    [HttpGet]
    public Song GetSong(string Id)
    {
      return playlistDataLayer.GetPlaylistSong(Id);
    }
  }
}
