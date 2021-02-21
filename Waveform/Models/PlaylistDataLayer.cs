using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Waveform.Areas.Identity.Data;

namespace Waveform.Models
{
  public class PlaylistDataLayer
  {
    readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Waveform;Trusted_Connection=True;MultipleActiveResultSets=true";

    public IEnumerable<Playlist> GetAllPlaylists(String userId)
    {
      List<Playlist> playlists = new List<Playlist>();

      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        SqlCommand command = new SqlCommand("GetAllPlaylists", connection)
        {
          CommandType = System.Data.CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@UserId", userId);
        connection.Open();

        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          Playlist playlist = new Playlist
          {
            Id = Convert.ToInt32(reader["Id"].ToString()),
            Title = reader["Title"].ToString(),
            Image = (byte[])reader["Image"],
            Songs = reader["Songs"].ToString(),
            UserId = reader["UserId"].ToString()
          };
          playlists.Add(playlist);
        }
        connection.Close();
      }
      return playlists;
    }

    public void CreatePlaylist(Playlist playlist, String userId)
    {
      using SqlConnection connection = new SqlConnection(connectionString);
      SqlCommand command = new SqlCommand("CreatePlaylist", connection)
      {
        CommandType = System.Data.CommandType.StoredProcedure
      };

      byte[] image = playlist.Image ?? File.ReadAllBytes("wwwroot/lib/images/default.png");

      command.Parameters.AddWithValue("@Title", playlist.Title);
      command.Parameters.AddWithValue("@Image", image);
      command.Parameters.AddWithValue("@Songs", playlist.Songs);
      command.Parameters.AddWithValue("@UserId", userId);

      connection.Open();
      command.ExecuteNonQuery();
      connection.Close();
    }

    public void UpdatePlaylist(Playlist playlist)
    {
      using SqlConnection connection = new SqlConnection(connectionString);
      SqlCommand command = new SqlCommand(playlist.Image == null ? "UpdatePlaylist" : "UpdatePlaylistWithImage", connection)
      {
        CommandType = System.Data.CommandType.StoredProcedure
      };

      command.Parameters.AddWithValue("@Id", playlist.Id);
      command.Parameters.AddWithValue("@Title", playlist.Title);
      command.Parameters.AddWithValue("@Songs", playlist.Songs);
      if (playlist.Image != null) command.Parameters.AddWithValue("@Image", playlist.Image);

      connection.Open();
      command.ExecuteNonQuery();
      connection.Close();
    }

    public void DeletePlaylist(int? Id)
    {
      using SqlConnection connection = new SqlConnection(connectionString);
      SqlCommand command = new SqlCommand("DeletePlaylist", connection)
      {
        CommandType = System.Data.CommandType.StoredProcedure
      };

      command.Parameters.AddWithValue("@Id", Id);

      connection.Open();
      command.ExecuteNonQuery();
      connection.Close();
    }

    public Playlist GetPlaylist(int? Id)
    {
      Playlist playlist = new Playlist();

      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        SqlCommand command = new SqlCommand("GetPlaylist", connection)
        {
          CommandType = System.Data.CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Id", Id);

        connection.Open();

        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          playlist.Id = Convert.ToInt32(reader["Id"].ToString());
          playlist.Title = reader["Title"].ToString();
          playlist.Image = (byte[])reader["Image"];
          playlist.Songs = reader["Songs"].ToString();
          playlist.UserId = reader["UserId"].ToString();
          playlist.DateCreated = reader["DateCreated"].ToString();
        }
        connection.Close();
      }
      return playlist;
    }

    public Song GetPlaylistSong(string Id)
    {
      Song song = new Song();

      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        SqlCommand command = new SqlCommand("GetPlaylistSong", connection)
        {
          CommandType = System.Data.CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Id", Id);

        connection.Open();

        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          song.Id = Convert.ToInt32(reader["Id"].ToString());
          song.Name = reader["Name"].ToString();
          song.Audio = (byte[])reader["Audio"];
          song.UserId = reader["UserId"].ToString();
          song.DateAdded = reader["DateAdded"].ToString();
        }
        connection.Close();
      }
      return song;
    }
  }
}
