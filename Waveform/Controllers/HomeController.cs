using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Waveform.Areas.Identity.Data;
using Waveform.Models;

namespace Waveform.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<WaveformUser> _userManager;
    private readonly SignInManager<WaveformUser> _signInManager;

    public HomeController(ILogger<HomeController> logger, UserManager<WaveformUser> userManager, SignInManager<WaveformUser> signInManager)
    {
      _logger = logger;
      _userManager = userManager;
      _signInManager = signInManager;
    }

    readonly SongDataLayer songDataLayer = new SongDataLayer();

    /*public IActionResult Index()
    {
      SongsViewModel allSongs = new SongsViewModel
      {
        Online = new List<String>(),
        Offline = new List<FileInfo>()
      };

      List<FileInfo> files = new List<FileInfo>();
      string[] extensions = { ".mp3", ".aac", ".ogg" };
      DirectoryInfo directory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Waveform"));

      foreach (FileInfo file in directory.GetFiles())
      {
        foreach (string extension in extensions)
        {
          if (file.Extension == extension)
          {
            //allSongs.Offline.Add(file);
            files.Add(file);
          }
        }
      }

      return View(files);
    }*/

    public IActionResult Index()
    {
      return View(songDataLayer.GetAllSongs(_userManager.GetUserId(User)).ToList());
    }

    [HttpGet]
    public IActionResult Add()
    {
      if (!(_signInManager.IsSignedIn(User))) return RedirectToAction("Index", "Home", new { area = "" });
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([Bind] Song song, List<IFormFile> Audio)
    {
      if (ModelState.IsValid)
      {
        foreach (var item in Audio)
        {
          if (item.Length > 0)
          {
            using (var stream = new MemoryStream())
            {
              await item.CopyToAsync(stream);
              song.Audio = stream.ToArray();
            }
            songDataLayer.AddSong(song);
          }
        }
        return RedirectToAction("Index");
      }

      return View(song);
    }

    [HttpGet]
    public IActionResult Delete(int? Id)
    {
      if (!(_signInManager.IsSignedIn(User))) return RedirectToAction("Index", "Home", new { area = "" });
      if (Id == null) return RedirectToAction("Index");

      Song song = songDataLayer.GetSong(Id);

      if (
        song == null || song.Id == 0 ||
        song.UserId != _userManager.GetUserId(User)
      )
      { return RedirectToAction("Index"); }

      return View(song);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteSong(int? Id)
    {
      songDataLayer.DeleteSong(Id);
      return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
