using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waveform.Models
{
  public class SongDataLayer
  {
    readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Waveform;Trusted_Connection=True;MultipleActiveResultSets=true";

    public IEnumerable<Song> GetAllSongs(String userId)
    {
      List<Song> songs = new List<Song>();

      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        SqlCommand command = new SqlCommand("GetAllSongs", connection)
        {
          CommandType = System.Data.CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@UserId", userId);
        connection.Open();

        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          Song song = new Song
          {
            Id = Convert.ToInt32(reader["Id"].ToString()),
            Name = reader["Name"].ToString(),
            Audio = (byte[])reader["Audio"],
            UserId = reader["UserId"].ToString(),
            DateAdded = reader["DateAdded"].ToString()
          };
          songs.Add(song);
        }
        connection.Close();
      }
      return songs;
    }

    public void AddSong(Song song)
    {
      using SqlConnection connection = new SqlConnection(connectionString);
      SqlCommand command = new SqlCommand("AddSong", connection)
      {
        CommandType = System.Data.CommandType.StoredProcedure
      };

      command.Parameters.AddWithValue("@Name", song.Name);
      command.Parameters.AddWithValue("@Audio", song.Audio);
      command.Parameters.AddWithValue("@UserId", song.UserId);

      connection.Open();
      command.ExecuteNonQuery();
      connection.Close();
    }

    public void DeleteSong(int? Id)
    {
      using SqlConnection connection = new SqlConnection(connectionString);
      SqlCommand command = new SqlCommand("DeleteSong", connection)
      {
        CommandType = System.Data.CommandType.StoredProcedure
      };

      command.Parameters.AddWithValue("@Id", Id);

      connection.Open();
      command.ExecuteNonQuery();
      connection.Close();
    }

    public Song GetSong(int? Id)
    {
      Song song = new Song();

      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        SqlCommand command = new SqlCommand("GetSong", connection)
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
