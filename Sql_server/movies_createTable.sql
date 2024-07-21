-- Tạo cơ sở dữ liệu
CREATE DATABASE MovieStreamingDB;
GO

-- Sử dụng cơ sở dữ liệu vừa tạo
USE MovieStreamingDB;
GO

-- Tạo các bảng
CREATE TABLE [categories](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [title] [nvarchar](100) NOT NULL,
    [description] [nvarchar](255) NOT NULL,
    [status] [int] NOT NULL,
    [slug] [nvarchar](255) NOT NULL,
    [position] [int] NOT NULL,
    [appear_nav] [int] NULL,
    CONSTRAINT [PK_categories_id] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [countries](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [title] [nvarchar](100) NOT NULL,
    [description] [nvarchar](255) NULL,
    [status] [int] NOT NULL,
    [slug] [nvarchar](255) NOT NULL,
    [position] [int] NOT NULL,
    CONSTRAINT [PK_countries_id] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [genres](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [title] [nvarchar](100) NOT NULL,
    [description] [nvarchar](255) NOT NULL,
    [status] [int] NOT NULL,
    [slug] [nvarchar](255) NOT NULL,
    [position] [int] NOT NULL,
    CONSTRAINT [PK_genres_id] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [linkmovie](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [title] [nvarchar](100) NOT NULL,
    [description] [nvarchar](255) NOT NULL,
    [status] [int] NOT NULL,
    CONSTRAINT [PK_linkmovie_id] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [movies](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [title] [nvarchar](100) NOT NULL,
    [thoiluong] [nvarchar](50) NULL,
    [name_eng] [nvarchar](100) NOT NULL,
    [slug] [nvarchar](255) NOT NULL,
    [description] [nvarchar](max) NOT NULL,
    [status] [int] NOT NULL,
    [image] [nvarchar](255) NOT NULL,
    [thuocphim] [nvarchar](50) NOT NULL,
    [category_id] [int] NULL,
    [genre_id] [int] NULL,
    [country_id] [int] NOT NULL,
    [phim_hot] [int] NOT NULL,
    [resolution] [int] NULL,
    [phude] [int] NOT NULL,
    [season] [int] NULL,
    [create_at] [nvarchar](50) NULL,
    [update_at] [nvarchar](50) NULL,
    [year] [nvarchar](20) NULL,
    [tags] [nvarchar](255) NOT NULL,
    [topview] [int] NULL,
    [trailer] [nvarchar](500) NULL,
    [sotap] [nvarchar](10) NOT NULL,
    [views] [int] NULL,
    [actor] [nvarchar](500) NULL,
    [director] [nvarchar](100) NULL,
    CONSTRAINT [PK_movies_id] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [episodes](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [movie_id] [int] NOT NULL,
    [link] [nvarchar](500) NOT NULL,
    [episode] [nvarchar](50) NOT NULL,
    [server] [int] NULL,
    [updated_at] [nvarchar](50) NOT NULL,
    [created_at] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_episodes_id] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_episodes_movie_id] FOREIGN KEY ([movie_id]) REFERENCES [movies]([id])
);

CREATE TABLE [movie_category](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [movie_id] [int] NOT NULL,
    [category_id] [int] NOT NULL,
    CONSTRAINT [PK_movie_category_id] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_movie_category_movie_id] FOREIGN KEY ([movie_id]) REFERENCES [movies]([id])
);

CREATE TABLE [movie_genre](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [movie_id] [int] NOT NULL,
    [genre_id] [int] NOT NULL,
    CONSTRAINT [PK_movie_genre_id] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_movie_genre_movie_id] FOREIGN KEY ([movie_id]) REFERENCES [movies]([id])
);

CREATE TABLE [users](
    [id] [numeric](20, 0) IDENTITY(1,1) NOT NULL,
    [name] [nvarchar](255) NOT NULL,
    [email] [nvarchar](255) NOT NULL,
    [email_verified_at] [datetime] NULL,
    [IsVerified] [int] NULL,
    [verificationCode] [uniqueidentifier] NULL,
    [password] [nvarchar](255) NOT NULL,
    [remember_token] [nvarchar](100) NULL,
    [role] [nchar](20) NULL,
    [created_at] [datetime] NULL,
    [updated_at] [datetime] NULL,
    CONSTRAINT [PK_users_id] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [users_email_unique] UNIQUE NONCLUSTERED ([email] ASC)
);