using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDirectoryTgBot;

/// <summary>
/// Класс "Фильм"
/// </summary>
public partial class Movie
{
    public int Id { get; set; }

    public string TitleRu { get; set; } = null!;

    public string? TitleOriginal { get; set; } = null!;

    public DateTime DateYear { get; set; }

    public string Director { get; set; } = null!;

    public string? Genre { get; set; } = null!;

    public double Rating { get; set; }

    public string Country { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public bool CompareMovie(List<Movie> movies)
    {
        bool checkMovie = movies.Any(movie => (TitleRu.Trim().Equals(movie.TitleRu.Trim(), StringComparison.OrdinalIgnoreCase)
                                      || TitleOriginal.Trim().Equals(movie.TitleOriginal.Trim(), StringComparison.OrdinalIgnoreCase))
                                     && Director.Trim().Equals(movie.Director.Trim(), StringComparison.OrdinalIgnoreCase)
                                     && DateYear.Year == movie.DateYear.Year);
        if (checkMovie)
            return true;
        else
            return false;
    }
}
