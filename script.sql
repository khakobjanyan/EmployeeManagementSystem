USE [master]
GO
/****** Object:  Database [EmployeeManagementSystemDB]    Script Date: 9/14/2023 9:43:47 PM ******/
CREATE DATABASE [EmployeeManagementSystemDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EmployeeManagementSystemDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\EmployeeManagementSystemDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EmployeeManagementSystemDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\EmployeeManagementSystemDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EmployeeManagementSystemDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET RECOVERY FULL 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET  MULTI_USER 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'EmployeeManagementSystemDB', N'ON'
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [EmployeeManagementSystemDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 9/14/2023 9:43:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[employees]    Script Date: 9/14/2023 9:43:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[employees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Position] [nvarchar](max) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[StatusId] [int] NOT NULL,
	[ProfilePictureUrl] [nvarchar](max) NULL,
 CONSTRAINT [PK_employees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 9/14/2023 9:43:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Token] [nvarchar](max) NULL,
	[Role] [nvarchar](max) NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[RefreshTokenExpTime] [datetime2](7) NULL,
	[ResetPasswordToken] [nvarchar](max) NULL,
	[ResetPasswordExpired] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [EmployeeManagementSystemDB] SET  READ_WRITE 
GO
