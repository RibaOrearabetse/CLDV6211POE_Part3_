# CLDV6211POE_Part3_

# EventEase Venue Booking System

EventEase is a cloud-based web application built with ASP.NET Core MVC that allows users to manage venues, events, and bookings in a streamlined, user-friendly interface. This project was developed as part of the CLDV6211 Portfolio of Evidence (POE) for IIE MSA.

## 🌐 Live Demo

> 🔗 [View deployed app on Azure]([https://youreventeaseapp.azurewebsites.net](https://st10446648-cmfxfnbcaebgdgd3.southafricanorth-01.azurewebsites.net/ ))
> 🔗 [View deployed app on YouTube]()
---

## 📌 Features

### 🔹 Venues
- Add, update, and delete venues
- Upload venue images via Azure Blob Storage
- Set and view venue availability

### 🔹 Events
- Create and manage events
- Link events to venues
- Support for predefined event types (e.g. Wedding, Conference)

### 🔹 Bookings
- Book venues for events
- Prevent double bookings through validation
- Restrict deletion of active bookings
- Search and filter bookings by:
  - Event type
  - Date range
  - Venue availability

---

## 🛠️ Technologies Used

- **Frontend & Backend:** ASP.NET Core MVC (C#)
- **Database:** Azure SQL Database
- **ORM:** Entity Framework Core
- **File Storage:** Azure Blob Storage
- **Hosting:** Azure App Service
- **Version Control:** Git & GitHub

---

## ☁️ Azure Services Used

| Azure Service         | Purpose                                                  |
|-----------------------|----------------------------------------------------------|
| Azure SQL Database    | Structured data for venues, events, and bookings         |
| Azure Blob Storage    | Stores uploaded venue images                             |
| Azure App Service     | Hosts the deployed web application                       |
| Azure Event Grid (Optional) | Could be used to trigger booking notifications     |

