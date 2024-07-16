using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_Movies.Models;

public partial class MoviesDbContext : DbContext
{
    public MoviesDbContext()
    {
    }

    public MoviesDbContext(DbContextOptions<MoviesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<FailedJob> FailedJobs { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Linkmovie> Linkmovies { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieCategory> MovieCategories { get; set; }

    public virtual DbSet<MovieGenre> MovieGenres { get; set; }

    public virtual DbSet<PasswordReset> PasswordResets { get; set; }

    public virtual DbSet<PersonalAccessToken> PersonalAccessTokens { get; set; }

    public virtual DbSet<Truycap> Truycaps { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WebInfo> WebInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=movies_API;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_categories_id");

            entity.ToTable("categories", "movies_web");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppearNav)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("appear_nav");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_countries_id");

            entity.ToTable("countries", "movies_web");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("description");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_episodes_id");

            entity.ToTable("episodes", "movies_web");

            entity.HasIndex(e => e.MovieId, "movie_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(50)
                .HasColumnName("created_at");
            entity.Property(e => e.Episode1)
                .HasMaxLength(50)
                .HasColumnName("episode");
            entity.Property(e => e.Link)
                .HasMaxLength(500)
                .HasColumnName("link");
            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.Server)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("server");
            entity.Property(e => e.UpdatedAt)
                .HasMaxLength(50)
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Movie).WithMany(p => p.Episodes)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("episodes$episodes_ibfk_1");
        });

        modelBuilder.Entity<FailedJob>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_failed_jobs_id");

            entity.ToTable("failed_jobs", "movies_web");

            entity.HasIndex(e => e.Uuid, "failed_jobs$failed_jobs_uuid_unique").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(20, 0)")
                .HasColumnName("id");
            entity.Property(e => e.Connection).HasColumnName("connection");
            entity.Property(e => e.Exception).HasColumnName("exception");
            entity.Property(e => e.FailedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("failed_at");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.Queue).HasColumnName("queue");
            entity.Property(e => e.Uuid)
                .HasMaxLength(255)
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_genres_id");

            entity.ToTable("genres", "movies_web");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_jobs_id");

            entity.ToTable("jobs", "movies_web");

            entity.HasIndex(e => e.Queue, "jobs_queue_index");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(20, 0)")
                .HasColumnName("id");
            entity.Property(e => e.Attempts).HasColumnName("attempts");
            entity.Property(e => e.AvailableAt).HasColumnName("available_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.Queue)
                .HasMaxLength(255)
                .HasColumnName("queue");
            entity.Property(e => e.ReservedAt)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("reserved_at");
        });

        modelBuilder.Entity<Linkmovie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_linkmovie_id");

            entity.ToTable("linkmovie", "movies_web");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Migration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_migrations_id");

            entity.ToTable("migrations", "movies_web");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Batch).HasColumnName("batch");
            entity.Property(e => e.Migration1)
                .HasMaxLength(255)
                .HasColumnName("migration");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_movies_id");

            entity.ToTable("movies", "movies_web");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Actor)
                .HasMaxLength(500)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("actor");
            entity.Property(e => e.CategoryId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("category_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreateAt)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("create_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Director)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("director");
            entity.Property(e => e.GenreId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("genre_id");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.NameEng)
                .HasMaxLength(100)
                .HasColumnName("name_eng");
            entity.Property(e => e.PhimHot).HasColumnName("phim_hot");
            entity.Property(e => e.Phude).HasColumnName("phude");
            entity.Property(e => e.Resolution)
                .HasDefaultValue(0)
                .HasColumnName("resolution");
            entity.Property(e => e.Season)
                .HasDefaultValue(0)
                .HasColumnName("season");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.Sotap)
                .HasMaxLength(10)
                .HasDefaultValue("0")
                .HasColumnName("sotap");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Tags)
                .HasMaxLength(255)
                .HasColumnName("tags");
            entity.Property(e => e.Thoiluong)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("thoiluong");
            entity.Property(e => e.Thuocphim)
                .HasMaxLength(50)
                .HasColumnName("thuocphim");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.Topview)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("topview");
            entity.Property(e => e.Trailer)
                .HasMaxLength(500)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("trailer");
            entity.Property(e => e.UpdateAt)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("update_at");
            entity.Property(e => e.Views)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("views");
            entity.Property(e => e.Year)
                .HasMaxLength(20)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("year");
        });

        modelBuilder.Entity<MovieCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_movie_category_id");

            entity.ToTable("movie_category", "movies_web");

            entity.HasIndex(e => e.MovieId, "movie_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.MovieId).HasColumnName("movie_id");

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieCategories)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("movie_category$movie_category_ibfk_1");
        });

        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_movie_genre_id");

            entity.ToTable("movie_genre", "movies_web");

            entity.HasIndex(e => e.MovieId, "movie_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.MovieId).HasColumnName("movie_id");

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieGenres)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("movie_genre$movie_genre_ibfk_1");
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("password_resets", "movies_web");

            entity.HasIndex(e => e.Email, "password_resets_email_index");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");
        });

        modelBuilder.Entity<PersonalAccessToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_personal_access_tokens_id");

            entity.ToTable("personal_access_tokens", "movies_web");

            entity.HasIndex(e => e.Token, "personal_access_tokens$personal_access_tokens_token_unique").IsUnique();

            entity.HasIndex(e => new { e.TokenableType, e.TokenableId }, "personal_access_tokens_tokenable_type_tokenable_id_index");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(20, 0)")
                .HasColumnName("id");
            entity.Property(e => e.Abilities)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("abilities");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.LastUsedAt)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("last_used_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Token)
                .HasMaxLength(64)
                .HasColumnName("token");
            entity.Property(e => e.TokenableId)
                .HasColumnType("numeric(20, 0)")
                .HasColumnName("tokenable_id");
            entity.Property(e => e.TokenableType)
                .HasMaxLength(255)
                .HasColumnName("tokenable_type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Truycap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_truycap_id");

            entity.ToTable("truycap", "movies_web");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Access)
                .HasDefaultValue(1)
                .HasColumnName("access");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_users_id");

            entity.ToTable("users", "movies_web");

            entity.HasIndex(e => e.Email, "users$users_email_unique").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(20, 0)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerifiedAt)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("email_verified_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RememberToken)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("remember_token");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<WebInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_web_info_id");

            entity.ToTable("web_info", "movies_web");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Logo)
                .HasMaxLength(255)
                .HasColumnName("logo");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
