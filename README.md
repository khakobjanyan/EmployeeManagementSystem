# Employee System

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
- [Usage](#usage)
  - [Running the UI](#running-the-ui)
  - [Database Setup](#database-setup)
  - [Image Resizing](#image-resizing)
- [Project Structure](#project-structure)

## Introduction

The Employee System is a web-based application designed to help businesses manage their employees' information and records. This system provides a convenient way to add, edit, view, and delete employee profiles, making it easier to keep track of important employee details.

## Features

- **User Authentication**: Secure login and registration system for users with different roles (e.g., admin, employee).

- **Employee Management**: Add, edit, view, and delete employee profiles, including personal information, job details, and profile pictures.

- **Image Upload**: Supports uploading and storing employee profile pictures.

- **Pagination**: Display a list of employees with pagination to improve performance and usability.

- **Password Reset**: Allow users to reset their passwords via email.

## Getting Started

### Prerequisites

Before you begin, ensure you have the following prerequisites:

- **Node.js**: Install Node.js by visiting [https://nodejs.org/](https://nodejs.org/) and download the latest LTS version.

- **Angular CLI**: Install Angular CLI globally using the following command:

  ```shell
  npm install -g @angular/cli
  ```
 - **SQL Server**: Ensure you have SQL Server installed and running. You will need it to store the application's data.
 - **SMTP Server**: Set up an SMTP server for sending password reset emails.
 
## Installation

Navigate to the project directory: EmployeeManagementSystem
 - Install project dependencies: npm install
 - To run the Employee System UI: ng serve
 - Configure the database connection string in the appsettings.json file of your API project (EmployeeSystemAPI). Update the ConnectionStrings section with your SQL Server connection details.

### Image Resizing
The project includes an ImageResizing console app. To automate image resizing, follow these steps:

1. Build the ImageResizing project.

2. Configure the ImageResizing console app with the correct paths for the temporary and image directories.

3. Set up a Windows Task Scheduler task to run the ImageResizing console app at specific intervals. Configure the task to execute the `ImageResizing.exe` executable. 

## Project Structure

The Employee System project follows a standard directory structure:

- **EmployeeSystemUI**: Contains the Angular-based user interface for the Employee System.

- **EmployeeSystemAPI**: Includes the API project responsible for managing employee data and interactions with the database.

- **ImageResizing**: This directory is part of the EmployeeSystemAPI solution and contains the console app for automating image resizing. It allows you to efficiently process and manage employee profile pictures.

- **Database**: Contains SQL Server database scripts and backups. You can use these scripts to set up and configure the database schema for the Employee System.

