using Microsoft.EntityFrameworkCore;
using MovieDirectoryTgBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Класс подключения к базе данных с помощью EF
/// </summary>
public partial class MoviesApplicationContext : DbContext
{
    public MoviesApplicationContext()
    {
    }

    public MoviesApplicationContext(DbContextOptions<MoviesApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer("Server=DESKTOP-5DQNS6K\\SQLEXPRESS;Database=MoviesApplication;Trusted_Connection=true;Encrypt=false");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);

        //Настройка уникальности столбцов
        modelBuilder.Entity<Movie>().Property(movie => movie.TitleOriginal).IsRequired(false);
        modelBuilder.Entity<Movie>().Property(movie => movie.Genre).IsRequired(false);
        modelBuilder.Entity<Movie>().Property(movie => movie.Description).IsRequired(true);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
